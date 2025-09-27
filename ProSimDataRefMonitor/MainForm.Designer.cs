using System.Windows.Forms;

namespace ProSimDataRefMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
			this.txtIp = new System.Windows.Forms.TextBox();
			this.lblIp = new System.Windows.Forms.Label();
			this.btnConnect = new System.Windows.Forms.Button();
			this.grid = new System.Windows.Forms.DataGridView();
			this.colDataRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnDisconnect = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtIp
			// 
			this.txtIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtIp.Location = new System.Drawing.Point(111, 15);
			this.txtIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtIp.Name = "txtIp";
			this.txtIp.Size = new System.Drawing.Size(418, 26);
			this.txtIp.TabIndex = 0;
			this.txtIp.Text = "127.0.0.1";
			// 
			// lblIp
			// 
			this.lblIp.AutoSize = true;
			this.lblIp.Location = new System.Drawing.Point(15, 19);
			this.lblIp.Name = "lblIp";
			this.lblIp.Size = new System.Drawing.Size(95, 20);
			this.lblIp.TabIndex = 1;
			this.lblIp.Text = "ProSim host";
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Location = new System.Drawing.Point(537, 12);
			this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(99, 32);
			this.btnConnect.TabIndex = 2;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataRef,
            this.colValue,
            this.colType,
            this.colUnit});
			this.grid.Location = new System.Drawing.Point(18, 95);
			this.grid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.grid.Name = "grid";
			this.grid.RowHeadersVisible = false;
			this.grid.RowHeadersWidth = 62;
			this.grid.RowTemplate.Height = 24;
			this.grid.Size = new System.Drawing.Size(834, 445);
			this.grid.TabIndex = 3;
			this.grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_EditingControlShowing);
			// 
			// colDataRef
			// 
			this.colDataRef.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colDataRef.HeaderText = "DataRef (name/path)";
			this.colDataRef.MinimumWidth = 250;
			this.colDataRef.Name = "colDataRef";
			// 
			// colValue
			// 
			this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colValue.HeaderText = "Value";
			this.colValue.MinimumWidth = 8;
			this.colValue.Name = "colValue";
			this.colValue.ReadOnly = true;
			this.colValue.Width = 86;
			// 
			// colType
			// 
			this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colType.HeaderText = "Type";
			this.colType.MinimumWidth = 8;
			this.colType.Name = "colType";
			this.colType.ReadOnly = true;
			this.colType.Width = 79;
			// 
			// colUnit
			// 
			this.colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colUnit.HeaderText = "Unit";
			this.colUnit.MinimumWidth = 8;
			this.colUnit.Name = "colUnit";
			this.colUnit.ReadOnly = true;
			this.colUnit.Width = 74;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(18, 55);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(99, 32);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "+ Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(124, 55);
			this.btnRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(99, 32);
			this.btnRemove.TabIndex = 5;
			this.btnRemove.Text = "− Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Location = new System.Drawing.Point(753, 55);
			this.btnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(99, 32);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Location = new System.Drawing.Point(647, 55);
			this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(99, 32);
			this.btnStop.TabIndex = 7;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip.Location = new System.Drawing.Point(0, 556);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusStrip.Size = new System.Drawing.Size(865, 32);
			this.statusStrip.TabIndex = 8;
			this.statusStrip.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(119, 25);
			this.lblStatus.Text = "Disconnected";
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Enabled = false;
			this.btnDisconnect.Location = new System.Drawing.Point(647, 13);
			this.btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(106, 32);
			this.btnDisconnect.TabIndex = 7;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(865, 588);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.lblIp);
			this.Controls.Add(this.txtIp);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(785, 461);
			this.Name = "MainForm";
			this.Text = "ProSim DataRef Monitor";
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
		private System.Windows.Forms.Button btnDisconnect;
	}
}
