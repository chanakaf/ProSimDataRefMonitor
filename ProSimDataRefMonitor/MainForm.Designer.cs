using System.Windows.Forms;

namespace ProSimDataRefMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            // ── Controls ────────────────────────────────────────────────────────
            this.pnlToolbar     = new System.Windows.Forms.Panel();
            this.lblIp          = new System.Windows.Forms.Label();
            this.txtIp          = new System.Windows.Forms.TextBox();
            this.flpConnectRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnConnect     = new System.Windows.Forms.Button();
            this.btnDisconnect  = new System.Windows.Forms.Button();

            this.pnlActionBar   = new System.Windows.Forms.Panel();
            this.flpActionLeft  = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd         = new System.Windows.Forms.Button();
            this.btnRemove      = new System.Windows.Forms.Button();
            this.btnPaste       = new System.Windows.Forms.Button();
            this.flpActionRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop        = new System.Windows.Forms.Button();
            this.btnStart       = new System.Windows.Forms.Button();

            this.grid           = new System.Windows.Forms.DataGridView();
            this.colDataRef     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit        = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.statusStrip    = new System.Windows.Forms.StatusStrip();
            this.lblStatus      = new System.Windows.Forms.ToolStripStatusLabel();

            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.pnlToolbar.SuspendLayout();
            this.flpConnectRight.SuspendLayout();
            this.pnlActionBar.SuspendLayout();
            this.flpActionLeft.SuspendLayout();
            this.flpActionRight.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // ── pnlToolbar (top strip: Host label + IP box + Connect/Disconnect) ─
            this.pnlToolbar.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Height    = 48;
            this.pnlToolbar.Padding   = new System.Windows.Forms.Padding(12, 0, 8, 0);
            this.pnlToolbar.Controls.Add(this.flpConnectRight);
            this.pnlToolbar.Controls.Add(this.txtIp);
            this.pnlToolbar.Controls.Add(this.lblIp);

            // ── lblIp ──────────────────────────────────────────────────────────
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(12, 16);
            this.lblIp.Text     = "Host";
            this.lblIp.TabIndex = 0;

            // ── txtIp ──────────────────────────────────────────────────────────
            this.txtIp.Location = new System.Drawing.Point(52, 13);
            this.txtIp.Size     = new System.Drawing.Size(160, 22);
            this.txtIp.Text     = "127.0.0.1";
            this.txtIp.TabIndex = 1;
            this.txtIp.Anchor   = System.Windows.Forms.AnchorStyles.Top
                                | System.Windows.Forms.AnchorStyles.Left;

            // ── flpConnectRight (flows Connect | Disconnect from the right) ────
            this.flpConnectRight.Dock          = System.Windows.Forms.DockStyle.Right;
            this.flpConnectRight.Width         = 212;
            this.flpConnectRight.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpConnectRight.WrapContents  = false;
            this.flpConnectRight.Padding       = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flpConnectRight.Controls.Add(this.btnConnect);
            this.flpConnectRight.Controls.Add(this.btnDisconnect);

            // ── btnConnect ─────────────────────────────────────────────────────
            this.btnConnect.Size     = new System.Drawing.Size(96, 28);
            this.btnConnect.Margin   = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Click   += new System.EventHandler(this.btnConnect_Click);

            // ── btnDisconnect ──────────────────────────────────────────────────
            this.btnDisconnect.Size     = new System.Drawing.Size(104, 28);
            this.btnDisconnect.Margin   = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Enabled  = false;
            this.btnDisconnect.Click   += new System.EventHandler(this.btnDisconnect_Click);

            // ── pnlActionBar (second strip: Add/Remove/Paste | Start/Stop) ─────
            this.pnlActionBar.Dock    = System.Windows.Forms.DockStyle.Top;
            this.pnlActionBar.Height  = 44;
            this.pnlActionBar.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.pnlActionBar.Controls.Add(this.flpActionRight);
            this.pnlActionBar.Controls.Add(this.flpActionLeft);

            // ── flpActionLeft (Add / Remove / Paste) ──────────────────────────
            this.flpActionLeft.Dock          = System.Windows.Forms.DockStyle.Left;
            this.flpActionLeft.Width         = 310;
            this.flpActionLeft.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpActionLeft.WrapContents  = false;
            this.flpActionLeft.Padding       = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpActionLeft.Controls.Add(this.btnAdd);
            this.flpActionLeft.Controls.Add(this.btnRemove);
            this.flpActionLeft.Controls.Add(this.btnPaste);

            // ── btnAdd ─────────────────────────────────────────────────────────
            this.btnAdd.Size     = new System.Drawing.Size(80, 28);
            this.btnAdd.Margin   = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Click   += new System.EventHandler(this.btnAdd_Click);

            // ── btnRemove ──────────────────────────────────────────────────────
            this.btnRemove.Size     = new System.Drawing.Size(90, 28);
            this.btnRemove.Margin   = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Click   += new System.EventHandler(this.btnRemove_Click);

            // ── btnPaste ───────────────────────────────────────────────────────
            this.btnPaste.Size     = new System.Drawing.Size(100, 28);
            this.btnPaste.Margin   = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnPaste.TabIndex = 6;
            this.btnPaste.Click   += new System.EventHandler(this.btnPaste_Click);

            // ── flpActionRight (Stop | Start — right-aligned) ─────────────────
            this.flpActionRight.Dock          = System.Windows.Forms.DockStyle.Right;
            this.flpActionRight.Width         = 196;
            this.flpActionRight.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpActionRight.WrapContents  = false;
            this.flpActionRight.Padding       = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpActionRight.Controls.Add(this.btnStop);
            this.flpActionRight.Controls.Add(this.btnStart);

            // ── btnStop ────────────────────────────────────────────────────────
            this.btnStop.Size     = new System.Drawing.Size(88, 28);
            this.btnStop.Margin   = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnStop.TabIndex = 7;
            this.btnStop.Click   += new System.EventHandler(this.btnStop_Click);

            // ── btnStart ───────────────────────────────────────────────────────
            this.btnStart.Size     = new System.Drawing.Size(88, 28);
            this.btnStart.Margin   = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnStart.TabIndex = 8;
            this.btnStart.Click   += new System.EventHandler(this.btnStart_Click);

            // ── grid ───────────────────────────────────────────────────────────
            this.grid.Anchor = System.Windows.Forms.AnchorStyles.Top
                             | System.Windows.Forms.AnchorStyles.Bottom
                             | System.Windows.Forms.AnchorStyles.Left
                             | System.Windows.Forms.AnchorStyles.Right;
            this.grid.Location              = new System.Drawing.Point(12, 100);
            this.grid.Size                  = new System.Drawing.Size(842, 452);
            this.grid.TabIndex              = 9;
            this.grid.RowHeadersVisible     = false;
            this.grid.AllowUserToAddRows    = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoSizeRowsMode      = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.grid.RowTemplate.Height    = 28;
            this.grid.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.MultiSelect           = true;
            this.grid.EditMode              = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDataRef, this.colValue, this.colType, this.colUnit });
            this.grid.EditingControlShowing +=
                new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_EditingControlShowing);
            this.grid.ColumnWidthChanged +=
                new System.Windows.Forms.DataGridViewColumnEventHandler(this.grid_ColumnWidthChanged);

            // ── colDataRef ─────────────────────────────────────────────────────
            this.colDataRef.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDataRef.HeaderText   = "DATAREF";
            this.colDataRef.MinimumWidth = 150;
            this.colDataRef.Width        = 300;
            this.colDataRef.Name         = "colDataRef";
            this.colDataRef.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;

            // ── colValue ───────────────────────────────────────────────────────
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colValue.HeaderText   = "VALUE";
            this.colValue.MinimumWidth = 90;
            this.colValue.Name         = "colValue";
            this.colValue.ReadOnly     = true;
            this.colValue.Width        = 110;
            this.colValue.DefaultCellStyle.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // ── colType ────────────────────────────────────────────────────────
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colType.HeaderText   = "TYPE";
            this.colType.MinimumWidth = 80;
            this.colType.Name         = "colType";
            this.colType.ReadOnly     = true;
            this.colType.Width        = 90;

            // ── colUnit ────────────────────────────────────────────────────────
            this.colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUnit.HeaderText   = "UNIT";
            this.colUnit.MinimumWidth = 40;
            this.colUnit.Name         = "colUnit";
            this.colUnit.ReadOnly     = true;

            // ── statusStrip ────────────────────────────────────────────────────
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus });
            this.statusStrip.Name             = "statusStrip";
            this.statusStrip.TabIndex         = 10;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(16, 16);

            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "  ○  Disconnected";

            // ── MainForm ───────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode  = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize     = new System.Drawing.Size(866, 588);
            this.MinimumSize    = new System.Drawing.Size(720, 480);
            this.Text           = "ProSim DataRef Monitor";
            this.Name           = "MainForm";

            this.Controls.Add(this.grid);
            this.Controls.Add(this.pnlActionBar);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.statusStrip);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.flpConnectRight.ResumeLayout(false);
            this.pnlActionBar.ResumeLayout(false);
            this.flpActionLeft.ResumeLayout(false);
            this.flpActionRight.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        // ── Field declarations ─────────────────────────────────────────────────
        internal System.Windows.Forms.Panel              pnlToolbar;
        internal System.Windows.Forms.Panel              pnlActionBar;
        internal System.Windows.Forms.FlowLayoutPanel    flpConnectRight;
        internal System.Windows.Forms.FlowLayoutPanel    flpActionLeft;
        internal System.Windows.Forms.FlowLayoutPanel    flpActionRight;
        internal System.Windows.Forms.Label              lblIp;
        internal System.Windows.Forms.TextBox            txtIp;
        internal System.Windows.Forms.Button             btnConnect;
        internal System.Windows.Forms.Button             btnDisconnect;
        internal System.Windows.Forms.Button             btnAdd;
        internal System.Windows.Forms.Button             btnRemove;
        internal System.Windows.Forms.Button             btnPaste;
        internal System.Windows.Forms.Button             btnStart;
        internal System.Windows.Forms.Button             btnStop;
        internal System.Windows.Forms.DataGridView       grid;
        internal System.Windows.Forms.StatusStrip        statusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
    }
}
