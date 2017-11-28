namespace Test_Bulk
{
    partial class fTestBulk
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsNumDevices = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsRefresh = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxEndpoint = new System.Windows.Forms.ComboBox();
            this.cmdOpen = new System.Windows.Forms.Button();
            this.cboDevices = new System.Windows.Forms.ComboBox();
            this.panTransfer = new System.Windows.Forms.Panel();
            this.time = new System.Windows.Forms.Label();
            this.buffersize = new System.Windows.Forms.Label();
            this.grpLogToFile = new System.Windows.Forms.GroupBox();
            this.cmdOpenLogFile = new System.Windows.Forms.Button();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            chkLogToFile = new System.Windows.Forms.CheckBox();
            zGraphTest = new ZhengJuyin.UI.ZGraph();
            this.ckShowAsHex = new System.Windows.Forms.CheckBox();
            this.tWrite = new System.Windows.Forms.TextBox();
            this.chkRead = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tRecv = new System.Windows.Forms.TextBox();
            this.ctxRecvTextBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.timerRead = new System.Windows.Forms.Timer(this.components);
            this.sfdLogFile = new System.Windows.Forms.SaveFileDialog();
            this.timerScreen = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panTransfer.SuspendLayout();
            this.grpLogToFile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ctxRecvTextBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsNumDevices,
            this.tsRefresh,
            this.tsStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 563);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(717, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(95, 17);
            this.toolStripStatusLabel1.Text = "Devices Found:";
            // 
            // tsNumDevices
            // 
            this.tsNumDevices.Name = "tsNumDevices";
            this.tsNumDevices.Size = new System.Drawing.Size(62, 17);
            this.tsNumDevices.Text = "Unknown";
            // 
            // tsRefresh
            // 
            this.tsRefresh.Name = "tsRefresh";
            this.tsRefresh.Size = new System.Drawing.Size(0, 17);
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(545, 17);
            this.tsStatus.Spring = true;
            this.tsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxEndpoint);
            this.groupBox1.Controls.Add(this.cmdOpen);
            this.groupBox1.Controls.Add(this.cboDevices);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 40);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "USB Device";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(522, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Endpoint:";
            // 
            // comboBoxEndpoint
            // 
            this.comboBoxEndpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEndpoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEndpoint.FormattingEnabled = true;
            this.comboBoxEndpoint.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F"});
            this.comboBoxEndpoint.Location = new System.Drawing.Point(587, 15);
            this.comboBoxEndpoint.Name = "comboBoxEndpoint";
            this.comboBoxEndpoint.Size = new System.Drawing.Size(44, 20);
            this.comboBoxEndpoint.TabIndex = 2;
            // 
            // cmdOpen
            // 
            this.cmdOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOpen.Location = new System.Drawing.Point(637, 13);
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(75, 21);
            this.cmdOpen.TabIndex = 1;
            this.cmdOpen.Text = "Open";
            this.cmdOpen.UseVisualStyleBackColor = true;
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // cboDevices
            // 
            this.cboDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevices.FormattingEnabled = true;
            this.cboDevices.Items.AddRange(new object[] {
            "b35924d6-0000-4a9e-9782-5524a4b79bac"});
            this.cboDevices.Location = new System.Drawing.Point(3, 15);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.Size = new System.Drawing.Size(513, 20);
            this.cboDevices.TabIndex = 0;
            this.cboDevices.DropDown += new System.EventHandler(this.cboDevices_DropDown);
            // 
            // panTransfer
            // 
            this.panTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panTransfer.Controls.Add(this.time);
            this.panTransfer.Controls.Add(this.buffersize);
            this.panTransfer.Controls.Add(this.grpLogToFile);
            this.panTransfer.Controls.Add(chkLogToFile);
            this.panTransfer.Controls.Add(zGraphTest);
            this.panTransfer.Controls.Add(this.ckShowAsHex);
            this.panTransfer.Controls.Add(this.tWrite);
            this.panTransfer.Controls.Add(this.chkRead);
            this.panTransfer.Controls.Add(this.groupBox2);
            this.panTransfer.Controls.Add(this.cmdRead);
            this.panTransfer.Controls.Add(this.cmdWrite);
            this.panTransfer.Enabled = false;
            this.panTransfer.Location = new System.Drawing.Point(0, 64);
            this.panTransfer.Name = "panTransfer";
            this.panTransfer.Size = new System.Drawing.Size(717, 496);
            this.panTransfer.TabIndex = 3;
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Location = new System.Drawing.Point(471, 36);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(29, 12);
            this.time.TabIndex = 11;
            this.time.Text = "time";
            // 
            // buffersize
            // 
            this.buffersize.AutoSize = true;
            this.buffersize.Location = new System.Drawing.Point(390, 35);
            this.buffersize.Name = "buffersize";
            this.buffersize.Size = new System.Drawing.Size(65, 12);
            this.buffersize.TabIndex = 10;
            this.buffersize.Text = "buffersize";
            // 
            // grpLogToFile
            // 
            this.grpLogToFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLogToFile.Controls.Add(this.cmdOpenLogFile);
            this.grpLogToFile.Controls.Add(this.txtLogFile);
            this.grpLogToFile.Enabled = false;
            this.grpLogToFile.Location = new System.Drawing.Point(13, 57);
            this.grpLogToFile.Name = "grpLogToFile";
            this.grpLogToFile.Size = new System.Drawing.Size(701, 43);
            this.grpLogToFile.TabIndex = 9;
            this.grpLogToFile.TabStop = false;
            this.grpLogToFile.Text = "Log Filename";
            // 
            // cmdOpenLogFile
            // 
            this.cmdOpenLogFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdOpenLogFile.Location = new System.Drawing.Point(670, 17);
            this.cmdOpenLogFile.Name = "cmdOpenLogFile";
            this.cmdOpenLogFile.Size = new System.Drawing.Size(28, 23);
            this.cmdOpenLogFile.TabIndex = 1;
            this.cmdOpenLogFile.Text = "...";
            this.cmdOpenLogFile.UseVisualStyleBackColor = true;
            this.cmdOpenLogFile.Click += new System.EventHandler(this.cmdOpenLogFile_Click);
            // 
            // txtLogFile
            // 
            this.txtLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFile.Location = new System.Drawing.Point(6, 18);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.ReadOnly = true;
            this.txtLogFile.Size = new System.Drawing.Size(658, 21);
            this.txtLogFile.TabIndex = 0;
            // 
            // chkLogToFile
            // 
            chkLogToFile.AutoSize = true;
            chkLogToFile.Location = new System.Drawing.Point(115, 34);
            chkLogToFile.Name = "chkLogToFile";
            chkLogToFile.Size = new System.Drawing.Size(90, 16);
            chkLogToFile.TabIndex = 8;
            chkLogToFile.Text = "Log To File";
            chkLogToFile.UseVisualStyleBackColor = true;
            chkLogToFile.CheckedChanged += new System.EventHandler(chkLogToFile_CheckedChanged);
            // 
            // zGraphTest
            // 
            zGraphTest.BackColor = System.Drawing.Color.White;
            zGraphTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            zGraphTest.Location = new System.Drawing.Point(15, 111);
            zGraphTest.m_backColorH = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            zGraphTest.m_backColorL = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            zGraphTest.m_BigXYBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            zGraphTest.m_BigXYButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            zGraphTest.m_BigXYButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_ControlButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_ControlButtonForeColorH = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            zGraphTest.m_ControlButtonForeColorL = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            zGraphTest.m_ControlItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_coordinateLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_coordinateStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_coordinateStringTitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_DirectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            zGraphTest.m_DirectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            zGraphTest.m_fXBeginSYS = 0F;
            zGraphTest.m_fXEndSYS = 225F;
            zGraphTest.m_fYBeginSYS = 0F;
            zGraphTest.m_fYEndSYS = 4000F;
            zGraphTest.m_GraphBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_iLineShowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            zGraphTest.m_iLineShowColorAlpha = 100;
            zGraphTest.m_SySnameX = "";
            zGraphTest.m_SySnameY = "";
            zGraphTest.m_SyStitle = "≤®–Œœ‘ æ";
            zGraphTest.m_titleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            zGraphTest.m_titleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            zGraphTest.m_titlePosition = 0.4F;
            zGraphTest.m_titleSize = 14;
            zGraphTest.Margin = new System.Windows.Forms.Padding(0);
            zGraphTest.MinimumSize = new System.Drawing.Size(390, 270);
            zGraphTest.Name = "zGraphTest";
            zGraphTest.Size = new System.Drawing.Size(688, 282);
            zGraphTest.TabIndex = 7;
            // 
            // ckShowAsHex
            // 
            this.ckShowAsHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckShowAsHex.AutoSize = true;
            this.ckShowAsHex.Checked = true;
            this.ckShowAsHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckShowAsHex.Location = new System.Drawing.Point(595, 35);
            this.ckShowAsHex.Name = "ckShowAsHex";
            this.ckShowAsHex.Size = new System.Drawing.Size(108, 16);
            this.ckShowAsHex.TabIndex = 6;
            this.ckShowAsHex.Text = "Convert to Hex";
            this.ckShowAsHex.UseVisualStyleBackColor = true;
            // 
            // tWrite
            // 
            this.tWrite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tWrite.Location = new System.Drawing.Point(96, 4);
            this.tWrite.Multiline = true;
            this.tWrite.Name = "tWrite";
            this.tWrite.Size = new System.Drawing.Size(615, 19);
            this.tWrite.TabIndex = 5;
            // 
            // chkRead
            // 
            this.chkRead.AutoSize = true;
            this.chkRead.Location = new System.Drawing.Point(211, 34);
            this.chkRead.Name = "chkRead";
            this.chkRead.Size = new System.Drawing.Size(162, 16);
            this.chkRead.TabIndex = 4;
            this.chkRead.Text = "Use Data Received Event";
            this.chkRead.UseVisualStyleBackColor = true;
            this.chkRead.CheckedChanged += new System.EventHandler(this.chkRead_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tRecv);
            this.groupBox2.Location = new System.Drawing.Point(0, 402);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(717, 91);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Received";
            // 
            // tRecv
            // 
            this.tRecv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tRecv.ContextMenuStrip = this.ctxRecvTextBox;
            this.tRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRecv.Location = new System.Drawing.Point(3, 17);
            this.tRecv.MaxLength = 0;
            this.tRecv.Multiline = true;
            this.tRecv.Name = "tRecv";
            this.tRecv.Size = new System.Drawing.Size(711, 71);
            this.tRecv.TabIndex = 2;
            // 
            // ctxRecvTextBox
            // 
            this.ctxRecvTextBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.ctxRecvTextBox.Name = "ctxRecvTextBox";
            this.ctxRecvTextBox.Size = new System.Drawing.Size(107, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // cmdRead
            // 
            this.cmdRead.Location = new System.Drawing.Point(15, 29);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.Size = new System.Drawing.Size(75, 21);
            this.cmdRead.TabIndex = 1;
            this.cmdRead.Text = "Read";
            this.cmdRead.UseVisualStyleBackColor = true;
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // cmdWrite
            // 
            this.cmdWrite.Location = new System.Drawing.Point(15, 2);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.Size = new System.Drawing.Size(75, 21);
            this.cmdWrite.TabIndex = 0;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.UseVisualStyleBackColor = true;
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // timerRead
            // 
            this.timerRead.Enabled = true;
            this.timerRead.Interval = 1;
            this.timerRead.Tick += new System.EventHandler(this.timerRead_Tick);
            // 
            // sfdLogFile
            // 
            this.sfdLogFile.DefaultExt = "bin";
            this.sfdLogFile.Filter = "Bin Files|*.bin|Hex Files|*.hex|All Files|*.*";
            this.sfdLogFile.SupportMultiDottedExtensions = true;
            // 
            // timerScreen
            // 
            this.timerScreen.Interval = 20;
            // 
            // fTestBulk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 585);
            this.Controls.Add(this.panTransfer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "fTestBulk";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fTestBulk_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panTransfer.ResumeLayout(false);
            this.panTransfer.PerformLayout();
            this.grpLogToFile.ResumeLayout(false);
            this.grpLogToFile.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ctxRecvTextBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsNumDevices;
        private System.Windows.Forms.ToolStripStatusLabel tsRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboDevices;
        private System.Windows.Forms.Panel panTransfer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tRecv;
        private System.Windows.Forms.Button cmdRead;
        private System.Windows.Forms.Button cmdWrite;
        private System.Windows.Forms.TextBox tWrite;
        private System.Windows.Forms.CheckBox chkRead;
        private System.Windows.Forms.Button cmdOpen;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ContextMenuStrip ctxRecvTextBox;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.CheckBox ckShowAsHex;

        // private System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxEndpoint;
        private System.Windows.Forms.Timer timerRead;
        private System.Windows.Forms.Timer timerScreen;
        private static System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.GroupBox grpLogToFile;
        private System.Windows.Forms.Button cmdOpenLogFile;
        private System.Windows.Forms.SaveFileDialog sfdLogFile;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.Label buffersize;
        private System.Windows.Forms.Label time;
        private static ZhengJuyin.UI.ZGraph zGraphTest;
    }
}