using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProSimDataRefMonitor
{
    public partial class MainForm : Form
    {
        private IProSimClient _client;
        private CancellationTokenSource _cts;
        private volatile bool _formDisposed = false;

        // ==== Catalog ====
        private class DataRefEntry
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
            public bool CanRead { get; set; }
            public bool CanWrite { get; set; }
            public string Description { get; set; }
        }

        private List<DataRefEntry> _catalog = new List<DataRefEntry>();
        private AutoCompleteStringCollection _autoNames = new AutoCompleteStringCollection();

        private string SessionPath =>
            Path.Combine(Application.UserAppDataPath, "last_session.xml");

        public MainForm()
        {
            InitializeComponent();

            // Apply visual theme
            UITheme.Apply(this);

            // Load icon from the assembly's embedded manifest resource
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                // ApplicationIcon embeds the .ico under the assembly name as a Win32 resource;
                // extract it via the executing assembly's icon (works for both exe and WinForms host)
                this.Icon = Icon.ExtractAssociatedIcon(asm.Location);
            }
            catch { /* non-fatal */ }

            LoadCatalog();
            LoadSession();

            this.FormClosing += MainForm_FormClosing;
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            grid.CellEndEdit += grid_CellEndEdit;
            grid.CurrentCellChanged += grid_CurrentCellChanged;

            foreach (DataGridViewRow row in grid.Rows)
                ApplyMetaToRow(row);

            AutoFitDataRefColumn();
            ResizeFormToGrid();
        }

        // One-shot autofit: measure content, set the width, then restore None so the
        // user can freely drag the column divider afterwards.
        private void AutoFitDataRefColumn()
        {
            grid.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.AllCells);
            grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        // Resize the form width so the grid always shows all columns without
        // scrolling. Called when columns change width (e.g. DataRef autosize).
        private void ResizeFormToGrid()
        {
            int totalColWidth = grid.Columns.Cast<DataGridViewColumn>()
                                    .Sum(c => c.Width);
            // Add grid left margin (12) + right margin (12) + scrollbar allowance (20)
            int ideal = totalColWidth + 12 + 12 + 20;
            int minW  = this.MinimumSize.Width;
            this.Width = Math.Max(ideal, minW);
        }

        private void grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Fill column (UNIT) automatically absorbs or releases space – no form resize needed
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formDisposed = true;
            SaveSession();
        }

        // =========================
        // UI Event Handlers
        // =========================

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                var host = (txtIp.Text ?? "").Trim();

                var old = _client;
                _client = null;
                if (old != null)
                {
                    try { await old.StopAsync(); } catch { }
                    try { old.Dispose(); } catch { }
                }

                _client = new SdkClient(host);
                await _client.ConnectAsync();

                UITheme.SetStatusConnected(lblStatus, host);
                btnDisconnect.Enabled = true;
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UITheme.SetStatusDisconnected(lblStatus);
                btnDisconnect.Enabled = false;
                btnStart.Enabled = false;
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;

            var client = _client;
            _client = null;

            if (client != null)
            {
                try { await client.StopAsync(); } catch { }
                try { client.Dispose(); } catch { }
            }

            if (_cts != null)
            {
                try { _cts.Cancel(); } catch { }
                _cts = null;
            }

            UITheme.SetStatusDisconnected(lblStatus);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int idx = grid.Rows.Add();
            var row = grid.Rows[idx];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "";
            row.Cells[2].Value = "";
            row.Cells[3].Value = "";
            grid.CurrentCell = row.Cells[0];
            grid.BeginEdit(true);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            PasteDataRefsFromText(Clipboard.GetText());
        }

        private void PasteDataRefsFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            var separators = new[] { '\r', '\n', ',', ';' };
            var tokens = text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                             .Select(t => t.Trim())
                             .Where(t => !string.IsNullOrWhiteSpace(t))
                             .Distinct(StringComparer.OrdinalIgnoreCase)
                             .ToList();

            if (tokens.Count == 0) return;

            var existing = new HashSet<string>(
                grid.Rows.Cast<DataGridViewRow>()
                    .Select(r => r.Cells[0].Value?.ToString()?.Trim() ?? ""),
                StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var token in tokens)
            {
                if (existing.Contains(token)) continue;
                int idx = grid.Rows.Add();
                var row = grid.Rows[idx];
                row.Cells[0].Value = token;
                row.Cells[1].Value = "";
                ApplyMetaToRow(row);
                existing.Add(token);
                added++;
            }

            if (added > 0)
            {
                UITheme.SetStatusInfo(lblStatus, $"Added {added} DataRef(s) from clipboard");
                AutoFitDataRefColumn();
                ResizeFormToGrid();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var toRemove = grid.SelectedRows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .ToList();

            foreach (var r in toRemove)
                grid.Rows.Remove(r);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show(this, "Please connect first.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var names = grid.Rows.Cast<DataGridViewRow>()
                .Select(r => r.Cells[0].Value?.ToString()?.Trim() ?? "")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(50)
                .ToArray();

            if (names.Length == 0)
            {
                MessageBox.Show(this, "Add DataRefs first (use + Add, autocomplete, or Paste Lines).",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in grid.Rows)
                ApplyMetaToRow(row);

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await _client.StartAsync(names, OnValue);
                UITheme.SetStatusMonitoring(lblStatus, names.Length);
            }
            catch (Exception ex)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                MessageBox.Show(this, ex.Message, "Start error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UITheme.SetStatusInfo(lblStatus, "Error – not monitoring");
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            try
            {
                if (_client != null)
                    await _client.StopAsync();

                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts = null;
                }

                UITheme.SetStatusStopped(lblStatus);
            }
            catch (Exception ex)
            {
                UITheme.SetStatusInfo(lblStatus, "Stop error: " + ex.Message);
            }
            finally
            {
                btnStart.Enabled = true;
            }
        }

        private void OnValue(string name, ProSimValue value)
        {
            if (_formDisposed) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<string, ProSimValue>(OnValue), name, value); }
                catch (ObjectDisposedException) { }
                return;
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                var refName = row.Cells[0].Value?.ToString() ?? "";
                if (string.Equals(refName, name, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells[1].Value = value.ValueAsString;
                    break;
                }
            }
        }

        // =========================
        // Catalog & Autocomplete
        // =========================

        private void LoadCatalog()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProSimDataRefs.xml");
                if (!File.Exists(path))
                {
                    _catalog = new List<DataRefEntry>();
                    _autoNames.Clear();
                    return;
                }

                var doc = XDocument.Load(path);
                _catalog = doc.Root
                    .Elements("DataRef")
                    .Select(x => new DataRefEntry
                    {
                        Name     = (string)x.Attribute("name"),
                        Type     = (string)x.Attribute("type"),
                        Unit     = (string)x.Attribute("unit"),
                        CanRead  = string.Equals((string)x.Attribute("canRead"),  "true", StringComparison.OrdinalIgnoreCase),
                        CanWrite = string.Equals((string)x.Attribute("canWrite"), "true", StringComparison.OrdinalIgnoreCase),
                        Description = (string)x.Element("Description") ?? ""
                    })
                    .Where(e => !string.IsNullOrWhiteSpace(e.Name))
                    .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                _autoNames.Clear();
                if (_catalog.Count > 0)
                {
                    var names = _catalog
                        .Select(c => c.Name ?? "")
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                    _autoNames.AddRange(names);
                }
            }
            catch
            {
                _catalog = new List<DataRefEntry>();
                _autoNames.Clear();
            }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var tb = e.Control as DataGridViewTextBoxEditingControl;
            if (tb == null) return;

            if (grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == 0)
            {
                tb.AutoCompleteMode          = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource        = AutoCompleteSource.CustomSource;
                tb.AutoCompleteCustomSource  = _autoNames;
                tb.Font                      = UITheme.FontMono;

                tb.KeyDown -= DataRefTextBox_KeyDown;
                tb.KeyDown += DataRefTextBox_KeyDown;
                tb.KeyDown -= DataRefTextBox_PasteKeyDown;
                tb.KeyDown += DataRefTextBox_PasteKeyDown;
            }
            else
            {
                tb.AutoCompleteMode         = AutoCompleteMode.None;
                tb.AutoCompleteSource       = AutoCompleteSource.None;
                tb.AutoCompleteCustomSource = null;
                tb.KeyDown -= DataRefTextBox_KeyDown;
                tb.KeyDown -= DataRefTextBox_PasteKeyDown;
            }
        }

        private void grid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (grid.EditingControl is DataGridViewTextBoxEditingControl tb)
            {
                if (grid.CurrentCell == null || grid.CurrentCell.ColumnIndex != 0)
                {
                    tb.AutoCompleteMode         = AutoCompleteMode.None;
                    tb.AutoCompleteSource       = AutoCompleteSource.None;
                    tb.AutoCompleteCustomSource = null;
                    tb.KeyDown -= DataRefTextBox_KeyDown;
                    tb.KeyDown -= DataRefTextBox_PasteKeyDown;
                }
            }
        }

        private void DataRefTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Space && sender is TextBox tb)
            {
                var t = tb.Text;
                tb.Text = t + " ";
                tb.SelectionStart = tb.TextLength;
                tb.Text = t;
                tb.SelectionStart = tb.TextLength;
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void DataRefTextBox_PasteKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control || e.KeyCode != Keys.V) return;

            var clipText = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(clipText)) return;

            var separators = new[] { '\r', '\n', ',', ';' };
            var tokens = clipText.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(t => t.Trim())
                                 .Where(t => !string.IsNullOrWhiteSpace(t))
                                 .ToArray();

            if (tokens.Length <= 1) return;

            grid.EndEdit();
            if (grid.CurrentRow != null &&
                grid.CurrentRow.Cells[0].Value?.ToString()?.Trim() == "" &&
                !grid.CurrentRow.IsNewRow)
            {
                grid.Rows.Remove(grid.CurrentRow);
            }

            PasteDataRefsFromText(clipText);
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 || e.RowIndex < 0) return;
            ApplyMetaToRow(grid.Rows[e.RowIndex]);
            AutoFitDataRefColumn();
            ResizeFormToGrid();
        }

        private void ApplyMetaToRow(DataGridViewRow row)
        {
            if (row == null) return;
            var name = row.Cells[0].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrEmpty(name) || _catalog == null || _catalog.Count == 0) return;

            var meta = _catalog.FirstOrDefault(x =>
                string.Equals(x.Name ?? "", name, StringComparison.OrdinalIgnoreCase));
            if (meta != null)
            {
                row.Cells[2].Value = meta.Type ?? "";
                row.Cells[3].Value = meta.Unit ?? "";
            }
        }

        // =========================
        // Session persistence
        // =========================

        private void LoadSession()
        {
            try
            {
                if (!File.Exists(SessionPath)) return;
                var doc  = XDocument.Load(SessionPath);
                var root = doc.Root;
                txtIp.Text = (string)root.Element("Ip") ?? "127.0.0.1";

                grid.Rows.Clear();
                foreach (var e in root.Elements("Ref"))
                {
                    var r = grid.Rows.Add();
                    grid.Rows[r].Cells[0].Value = (string)e.Attribute("name") ?? "";
                    grid.Rows[r].Cells[2].Value = (string)e.Attribute("type") ?? "";
                    grid.Rows[r].Cells[3].Value = (string)e.Attribute("unit") ?? "";
                }
            }
            catch { }
        }

        private void SaveSession()
        {
            try
            {
                var dir = Path.GetDirectoryName(SessionPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                new XDocument(new XElement("Session",
                    new XElement("Ip", (txtIp.Text ?? "").Trim()),
                    grid.Rows.Cast<DataGridViewRow>()
                        .Select(r => new XElement("Ref",
                            new XAttribute("name", r.Cells[0].Value?.ToString()?.Trim() ?? ""),
                            new XAttribute("type", r.Cells[2].Value?.ToString() ?? ""),
                            new XAttribute("unit", r.Cells[3].Value?.ToString() ?? "")
                        ))
                )).Save(SessionPath);
            }
            catch { }
        }
    }
}
