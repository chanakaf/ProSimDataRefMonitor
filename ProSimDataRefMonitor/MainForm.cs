using System;
using System.Collections.Generic;
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

		// ==== Session persistence ====
		private string SessionPath
		{
			get { return Path.Combine(Application.UserAppDataPath, "last_session.xml"); }
		}

		public MainForm()
		{
			InitializeComponent();

			// Grid setup
			grid.AllowUserToAddRows = false;
			grid.AllowUserToResizeRows = false;
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.MultiSelect = true;
			grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2; // helps with immediate typing

			// Load catalog + last session
			LoadCatalog();
			LoadSession();

			// Hook events
			this.FormClosing += (s, e) => SaveSession();
			this.Load += MainForm_Load;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			grid.CellEndEdit += grid_CellEndEdit;
			grid.EditingControlShowing += grid_EditingControlShowing;
			grid.CurrentCellChanged += grid_CurrentCellChanged;

			// After loading a session, enrich any known rows with metadata
			foreach (DataGridViewRow row in grid.Rows)
				ApplyMetaToRow(row);
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

				if (_client != null) _client.Dispose();
				_client = new SdkClient(host);

				await _client.ConnectAsync();
				lblStatus.Text = "Connected to ProSim";
				lblStatus.ForeColor = System.Drawing.Color.Green;
				btnDisconnect.Enabled = true;
				btnStart.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				lblStatus.Text = "Disconnected";
				lblStatus.ForeColor = System.Drawing.Color.Red;
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
			// Gracefully stop + dispose
			try { if (_client != null) await _client.StopAsync(); } catch { }
			try { if (_client != null) _client.Dispose(); } catch { }
			_client = null;

			// UI state
			btnStart.Enabled = false;
			btnStop.Enabled = false;
			btnDisconnect.Enabled = false;
			lblStatus.Text = "Disconnected";
			lblStatus.ForeColor = System.Drawing.Color.Red;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int idx = grid.Rows.Add();
			var row = grid.Rows[idx];
			row.Cells[0].Value = ""; // user will type or pick via autocomplete
			row.Cells[1].Value = "";
			row.Cells[2].Value = "";
			row.Cells[3].Value = "";
			grid.CurrentCell = row.Cells[0];
			grid.BeginEdit(true);
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow r in grid.SelectedRows.Cast<DataGridViewRow>())
			{
				if (!r.IsNewRow)
					grid.Rows.Remove(r);
			}
		}

		private async void btnStart_Click(object sender, EventArgs e)
		{
			if (_client == null)
			{
				MessageBox.Show(this, "Please connect first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var names = grid.Rows.Cast<DataGridViewRow>()
				.Select(r => r.Cells[0].Value == null ? "" : r.Cells[0].Value.ToString().Trim())
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Take(50) // safety cap
				.ToArray();

			if (names.Length == 0)
			{
				MessageBox.Show(this, "Add DataRefs in the first column (use autocomplete).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// Fill Type/Unit columns from the catalog before starting
			foreach (DataGridViewRow row in grid.Rows)
				ApplyMetaToRow(row);

			btnStart.Enabled = false;
			btnStop.Enabled = true;
			_cts = new CancellationTokenSource();

			await _client.StartAsync(names, OnValue);
			lblStatus.Text = "Monitoring " + names.Length + " refs";
		}

		private async void btnStop_Click(object sender, EventArgs e)
		{
			btnStop.Enabled = false;
			try
			{
				if (_client != null)
					await _client.StopAsync();
				if (_cts != null)
					_cts.Cancel();
				lblStatus.Text = "Stopped";
			}
			finally
			{
				btnStart.Enabled = true;
			}
		}

		// Called by the SDK client when values arrive
		private void OnValue(string name, ProSimValue value)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<string, ProSimValue>(OnValue), name, value);
				return;
			}

			foreach (DataGridViewRow row in grid.Rows)
			{
				var refName = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString();
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
						Name = (string)x.Attribute("name"),
						Type = (string)x.Attribute("type"),
						Unit = (string)x.Attribute("unit"),
						CanRead = string.Equals((string)x.Attribute("canRead"), "true", StringComparison.OrdinalIgnoreCase),
						CanWrite = string.Equals((string)x.Attribute("canWrite"), "true", StringComparison.OrdinalIgnoreCase),
						Description = (string)x.Element("Description") ?? ""
					})
					.Where(e => !string.IsNullOrWhiteSpace(e.Name))
					.OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
					.ToList();

				// Build autocomplete list
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
			// WinForms reuses a single editing control; set autocomplete every time for col 0
			var tb = e.Control as DataGridViewTextBoxEditingControl;
			if (tb == null) return;

			if (grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == 0)
			{
				tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
				tb.AutoCompleteCustomSource = _autoNames;

				// Allow Ctrl+Space to force suggestions popup
				tb.KeyDown -= DataRefTextBox_KeyDown;
				tb.KeyDown += DataRefTextBox_KeyDown;
			}
			else
			{
				// Not the DataRef column—turn off autocomplete on the reused control
				tb.AutoCompleteMode = AutoCompleteMode.None;
				tb.AutoCompleteSource = AutoCompleteSource.None;
				tb.AutoCompleteCustomSource = null;
				tb.KeyDown -= DataRefTextBox_KeyDown;
			}
		}

		private void grid_CurrentCellChanged(object sender, EventArgs e)
		{
			// If user moves away from col 0, ensure autocomplete is off (editing control is reused)
			if (grid.EditingControl is DataGridViewTextBoxEditingControl tb)
			{
				if (grid.CurrentCell == null || grid.CurrentCell.ColumnIndex != 0)
				{
					tb.AutoCompleteMode = AutoCompleteMode.None;
					tb.AutoCompleteSource = AutoCompleteSource.None;
					tb.AutoCompleteCustomSource = null;
					tb.KeyDown -= DataRefTextBox_KeyDown;
				}
			}
		}

		private void DataRefTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.Space && sender is TextBox)
			{
				var tb = (TextBox)sender;

				// Nudge the textbox so WinForms re-shows the suggestion list
				var t = tb.Text;
				tb.Text = t + " ";
				tb.SelectionStart = tb.TextLength;
				tb.Text = t;
				tb.SelectionStart = tb.TextLength;

				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != 0 || e.RowIndex < 0) return; // only for the DataRef column
			var row = grid.Rows[e.RowIndex];
			ApplyMetaToRow(row);
		}

		private void ApplyMetaToRow(DataGridViewRow row)
		{
			if (row == null) return;
			var name = row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString().Trim();
			if (string.IsNullOrEmpty(name) || _catalog == null || _catalog.Count == 0) return;

			// exact (case-insensitive) match
			var meta = _catalog.FirstOrDefault(x => string.Equals(x.Name ?? "", name, StringComparison.OrdinalIgnoreCase));
			if (meta != null)
			{
				row.Cells[2].Value = meta.Type ?? "";
				row.Cells[3].Value = meta.Unit ?? "";
			}
		}

		// =========================
		// Session (remember IP + DataRefs)
		// =========================

		private void LoadSession()
		{
			try
			{
				if (!File.Exists(SessionPath)) return;
				var doc = XDocument.Load(SessionPath);
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
			catch
			{
				// ignore
			}
		}

		private void SaveSession()
		{
			try
			{
				var dir = Path.GetDirectoryName(SessionPath);
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

				var doc = new XDocument(new XElement("Session",
					new XElement("Ip", (txtIp.Text ?? "").Trim()),
					grid.Rows.Cast<DataGridViewRow>()
						.Select(r => new XElement("Ref",
							new XAttribute("name", ((r.Cells[0].Value == null) ? "" : r.Cells[0].Value.ToString().Trim())),
							new XAttribute("type", (r.Cells[2].Value == null) ? "" : r.Cells[2].Value.ToString()),
							new XAttribute("unit", (r.Cells[3].Value == null) ? "" : r.Cells[3].Value.ToString())
						))
				));
				doc.Save(SessionPath);
			}
			catch
			{
				// ignore
			}
		}
	}
}
