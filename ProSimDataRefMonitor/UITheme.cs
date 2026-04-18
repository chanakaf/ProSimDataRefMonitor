using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProSimDataRefMonitor
{
    /// <summary>
    /// Centralised visual theme for the ProSim DataRef Monitor.
    /// Call UITheme.Apply(form) once after InitializeComponent().
    /// </summary>
    internal static class UITheme
    {
        // ── Palette ────────────────────────────────────────────────────────────
        public static readonly Color BackgroundPage    = Color.FromArgb(245, 245, 247);
        public static readonly Color BackgroundPanel   = Color.FromArgb(255, 255, 255);
        public static readonly Color BackgroundToolbar = Color.FromArgb(250, 250, 251);
        public static readonly Color BackgroundInput   = Color.FromArgb(255, 255, 255);

        public static readonly Color AccentBlue        = Color.FromArgb( 24,  95, 165);
        public static readonly Color AccentBlueHover   = Color.FromArgb( 16,  72, 128);
        public static readonly Color AccentGreen       = Color.FromArgb( 59, 109,  17);
        public static readonly Color AccentGreenLight  = Color.FromArgb(234, 243, 222);
        public static readonly Color AccentRed         = Color.FromArgb(163,  45,  45);
        public static readonly Color AccentRedLight    = Color.FromArgb(252, 235, 235);

        public static readonly Color TextPrimary       = Color.FromArgb( 28,  28,  30);
        public static readonly Color TextSecondary     = Color.FromArgb( 99,  99, 102);
        public static readonly Color TextMuted         = Color.FromArgb(174, 174, 178);

        public static readonly Color Border            = Color.FromArgb(220, 220, 225);
        public static readonly Color BorderStrong      = Color.FromArgb(180, 180, 188);

        public static readonly Color GridHeaderBack    = Color.FromArgb(248, 248, 250);
        public static readonly Color GridAltRow        = Color.FromArgb(250, 250, 252);
        public static readonly Color GridSelection     = Color.FromArgb(230, 241, 255);
        public static readonly Color GridSelectionText = Color.FromArgb( 10,  68, 124);

        // Status dot colours
        public static readonly Color DotConnected    = Color.FromArgb( 99, 153,  34);
        public static readonly Color DotDisconnected = Color.FromArgb(163,  45,  45);
        public static readonly Color DotMonitoring   = Color.FromArgb( 24,  95, 165);

        // ── Fonts ──────────────────────────────────────────────────────────────
        public static readonly Font FontUI       = new Font("Segoe UI",  9f,  FontStyle.Regular);
        public static readonly Font FontUIBold   = new Font("Segoe UI",  9f,  FontStyle.Bold);
        public static readonly Font FontSmall    = new Font("Segoe UI",  8.5f, FontStyle.Regular);
        public static readonly Font FontMono     = new Font("Consolas", 9f,  FontStyle.Regular);
        public static readonly Font FontLabel    = new Font("Segoe UI",  8f,  FontStyle.Regular);

        // ── Apply ──────────────────────────────────────────────────────────────
        public static void Apply(MainForm form)
        {
            form.BackColor = BackgroundPage;
            form.Font = FontUI;

            StyleToolbar(form.pnlToolbar);
            StyleActionBar(form.pnlActionBar);
            // Flow panels inherit the wrong system colour without this
            form.flpConnectRight.BackColor = BackgroundPanel;
            form.flpActionLeft.BackColor   = BackgroundToolbar;
            form.flpActionRight.BackColor  = BackgroundToolbar;
            StyleConnectButton(form.btnConnect);
            StyleDisconnectButton(form.btnDisconnect);
            StyleAddButton(form.btnAdd);
            StyleRemoveButton(form.btnRemove);
            StylePasteButton(form.btnPaste);
            StyleStartButton(form.btnStart);
            StyleStopButton(form.btnStop);
            StyleIpBox(form.txtIp);
            StyleIpLabel(form.lblIp);
            StyleGrid(form.grid);
            StyleStatusStrip(form.statusStrip, form.lblStatus);
        }

        // ── Panel styling ──────────────────────────────────────────────────────
        private static void StyleToolbar(Panel p)
        {
            p.BackColor = BackgroundPanel;
            p.Paint += (s, e) =>
            {
                // 1px bottom border
                using (var pen = new Pen(Border, 1))
                    e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
            };
        }

        private static void StyleActionBar(Panel p)
        {
            p.BackColor = BackgroundToolbar;
            p.Paint += (s, e) =>
            {
                using (var pen = new Pen(Border, 1))
                    e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
            };
        }

        // ── Button helpers ─────────────────────────────────────────────────────
        private static void ApplyFlatStyle(Button b, Color bg, Color fg, Color border)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = border;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bg, 0.15f);
            b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bg, 0.05f);
            b.BackColor = bg;
            b.ForeColor = fg;
            b.Font = FontUI;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        public static void StyleConnectButton(Button b)
        {
            ApplyFlatStyle(b, AccentBlue, Color.White, AccentBlueHover);
            b.Text = "Connect";
        }

        public static void StyleDisconnectButton(Button b)
        {
            ApplyFlatStyle(b, BackgroundPanel, TextSecondary, BorderStrong);
            b.Text = "Disconnect";
        }

        public static void StyleAddButton(Button b)
        {
            ApplyFlatStyle(b, BackgroundPanel, TextPrimary, BorderStrong);
            b.Text = "+ Add";
        }

        public static void StyleRemoveButton(Button b)
        {
            ApplyFlatStyle(b, BackgroundPanel, TextSecondary, Border);
            b.Text = "− Remove";
        }

        public static void StylePasteButton(Button b)
        {
            ApplyFlatStyle(b, BackgroundPanel, TextSecondary, Border);
            b.Text = "Paste Lines";
        }

        public static void StyleStartButton(Button b)
        {
            ApplyFlatStyle(b, AccentGreenLight, AccentGreen, Color.FromArgb(152, 196, 80));
            b.Text = "▶  Start";
        }

        public static void StyleStopButton(Button b)
        {
            ApplyFlatStyle(b, AccentRedLight, AccentRed, Color.FromArgb(224, 150, 150));
            b.Text = "■  Stop";
        }

        // ── Input ──────────────────────────────────────────────────────────────
        private static void StyleIpBox(TextBox tb)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = BackgroundInput;
            tb.ForeColor = TextPrimary;
            tb.Font = FontUI;
        }

        private static void StyleIpLabel(Label lbl)
        {
            lbl.ForeColor = TextSecondary;
            lbl.Font = FontSmall;
        }

        // ── DataGridView ───────────────────────────────────────────────────────
        public static void StyleGrid(DataGridView g)
        {
            g.BorderStyle = BorderStyle.FixedSingle;
            g.BackgroundColor = BackgroundPanel;
            g.GridColor = Border;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Header
            g.ColumnHeadersDefaultCellStyle.BackColor  = GridHeaderBack;
            g.ColumnHeadersDefaultCellStyle.ForeColor  = TextSecondary;
            g.ColumnHeadersDefaultCellStyle.Font       = FontLabel;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderBack;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextSecondary;
            g.ColumnHeadersDefaultCellStyle.Padding    = new Padding(6, 4, 6, 4);
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 28;
            g.EnableHeadersVisualStyles = false;

            // Rows
            g.DefaultCellStyle.BackColor          = BackgroundPanel;
            g.DefaultCellStyle.ForeColor          = TextPrimary;
            g.DefaultCellStyle.Font               = FontUI;
            g.DefaultCellStyle.SelectionBackColor = GridSelection;
            g.DefaultCellStyle.SelectionForeColor = GridSelectionText;
            g.DefaultCellStyle.Padding            = new Padding(6, 3, 6, 3);

            g.AlternatingRowsDefaultCellStyle.BackColor          = GridAltRow;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = GridSelection;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = GridSelectionText;

            g.RowTemplate.Height = 28;

            // Wire up post-add column styling so fonts/wrap are set on the actual column objects
            g.ColumnAdded += Grid_ColumnAdded;

            // Custom cell painting – only the Type pill badge
            g.CellPainting += Grid_CellPainting;
        }

        private static void Grid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            var col = e.Column;
            col.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            if (col.Name == "colDataRef")
            {
                col.DefaultCellStyle.Font     = FontMono;
                col.DefaultCellStyle.ForeColor = AccentBlue;
                col.DefaultCellStyle.SelectionForeColor = GridSelectionText;
            }
            else if (col.Name == "colValue")
            {
                col.DefaultCellStyle.Font      = FontUIBold;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private static void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Type column – pill badge (only custom painting we keep)
            if (e.ColumnIndex == 2)
            {
                e.PaintBackground(e.ClipBounds, true);
                var val = e.Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(val))
                    DrawTypeBadge(e.Graphics, val, e.CellBounds);
                e.Handled = true;
            }
        }

        private static void DrawTypeBadge(Graphics g, string typeName, Rectangle cell)
        {
            Color bgColor, fgColor, borderColor;
            switch (typeName.ToLowerInvariant())
            {
                case "bool":
                case "boolean":
                    bgColor = Color.FromArgb(238, 237, 254);
                    fgColor = Color.FromArgb(60,  52, 137);
                    borderColor = Color.FromArgb(175, 169, 236);
                    break;
                case "double":
                case "float":
                    bgColor = Color.FromArgb(230, 241, 251);
                    fgColor = Color.FromArgb(12,  68, 124);
                    borderColor = Color.FromArgb(133, 183, 235);
                    break;
                case "int":
                case "integer":
                    bgColor = Color.FromArgb(234, 243, 222);
                    fgColor = Color.FromArgb(39,  80,  10);
                    borderColor = Color.FromArgb(151, 196,  89);
                    break;
                case "string":
                    bgColor = Color.FromArgb(250, 238, 218);
                    fgColor = Color.FromArgb(99,  56,   6);
                    borderColor = Color.FromArgb(239, 159,  39);
                    break;
                default:
                    bgColor = Color.FromArgb(241, 239, 232);
                    fgColor = Color.FromArgb(68,  68,  65);
                    borderColor = Color.FromArgb(180, 178, 169);
                    break;
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Pill rect
            var sz = g.MeasureString(typeName, FontLabel);
            int pw = (int)sz.Width + 14;
            int ph = 18;
            int px = cell.X + 6;
            int py = cell.Y + (cell.Height - ph) / 2;

            using (var bgBrush = new SolidBrush(bgColor))
            using (var borderPen = new Pen(borderColor, 1))
            using (var fgBrush = new SolidBrush(fgColor))
            {
                var pillRect = new Rectangle(px, py, pw, ph);
                // filled pill
                g.FillRoundedRectangle(bgBrush, pillRect, 9);
                g.DrawRoundedRectangle(borderPen, pillRect, 9);

                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(typeName, FontLabel, fgBrush, new Rectangle(px, py, pw, ph), sf);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        // ── StatusStrip ────────────────────────────────────────────────────────
        public static void StyleStatusStrip(StatusStrip ss, ToolStripStatusLabel lbl)
        {
            ss.BackColor = BackgroundPanel;
            ss.GripStyle = ToolStripGripStyle.Hidden;
            ss.SizingGrip = false;
            ss.RenderMode = ToolStripRenderMode.Professional;
            ss.Renderer = new StatusStripRenderer();

            lbl.ForeColor = TextSecondary;
            lbl.Font = FontSmall;
            lbl.Text = "Disconnected";
        }

        // ── Status helpers (called from MainForm) ──────────────────────────────
        public static void SetStatusConnected(ToolStripStatusLabel lbl, string host)
        {
            lbl.Text = "  ●  Connected to " + host;
            lbl.ForeColor = DotConnected;
        }

        public static void SetStatusDisconnected(ToolStripStatusLabel lbl)
        {
            lbl.Text = "  ○  Disconnected";
            lbl.ForeColor = DotDisconnected;
        }

        public static void SetStatusMonitoring(ToolStripStatusLabel lbl, int count)
        {
            lbl.Text = $"  ●  Monitoring {count} ref{(count == 1 ? "" : "s")}";
            lbl.ForeColor = DotMonitoring;
        }

        public static void SetStatusStopped(ToolStripStatusLabel lbl)
        {
            lbl.Text = "  ◌  Stopped";
            lbl.ForeColor = TextSecondary;
        }

        public static void SetStatusInfo(ToolStripStatusLabel lbl, string msg)
        {
            lbl.Text = "  " + msg;
            lbl.ForeColor = TextSecondary;
        }

        // ── Custom StatusStrip renderer (removes the top border line) ──────────
        private class StatusStripRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(Border, 1))
                    e.Graphics.DrawLine(pen, 0, 0, e.ToolStrip.Width, 0);
            }
        }
    }

    // ── Graphics extension – rounded rectangle helpers ─────────────────────────
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (var path = RoundedPath(rect, radius))
                g.FillPath(brush, path);
        }

        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (var path = RoundedPath(rect, radius))
                g.DrawPath(pen, path);
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedPath(Rectangle rect, int r)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, r * 2, r * 2, 180, 90);
            path.AddArc(rect.Right - r * 2, rect.Y, r * 2, r * 2, 270, 90);
            path.AddArc(rect.Right - r * 2, rect.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
