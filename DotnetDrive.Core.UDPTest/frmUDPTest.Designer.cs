namespace DotnetDrive.Core.UDPTest
{
    partial class frmUDPTest
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbLocalIP = new System.Windows.Forms.ComboBox();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.NumericUpDown();
            this.btnBindPort = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnCloseBind = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.txtRemotePort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenUDPClient = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemotePort)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLocalIP
            // 
            this.cmbLocalIP.FormattingEnabled = true;
            this.cmbLocalIP.Location = new System.Drawing.Point(100, 12);
            this.cmbLocalIP.Name = "cmbLocalIP";
            this.cmbLocalIP.Size = new System.Drawing.Size(169, 25);
            this.cmbLocalIP.TabIndex = 0;
            this.cmbLocalIP.Text = "*";
            // 
            // lblLocalIP
            // 
            this.lblLocalIP.AutoSize = true;
            this.lblLocalIP.Location = new System.Drawing.Point(48, 16);
            this.lblLocalIP.Name = "lblLocalIP";
            this.lblLocalIP.Size = new System.Drawing.Size(46, 17);
            this.lblLocalIP.TabIndex = 1;
            this.lblLocalIP.Text = "本地IP:";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(292, 16);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(80, 17);
            this.lblLocalPort.TabIndex = 2;
            this.lblLocalPort.Text = "本地端口号：";
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(378, 13);
            this.txtLocalPort.Maximum = new decimal(new int[] {
            65500,
            0,
            0,
            0});
            this.txtLocalPort.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(120, 23);
            this.txtLocalPort.TabIndex = 3;
            this.txtLocalPort.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            // 
            // btnBindPort
            // 
            this.btnBindPort.Location = new System.Drawing.Point(524, 9);
            this.btnBindPort.Name = "btnBindPort";
            this.btnBindPort.Size = new System.Drawing.Size(95, 31);
            this.btnBindPort.TabIndex = 4;
            this.btnBindPort.Text = "绑定本地端口";
            this.btnBindPort.UseVisualStyleBackColor = true;
            this.btnBindPort.Click += new System.EventHandler(this.btnBindPort_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(2, 176);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(741, 415);
            this.txtLog.TabIndex = 5;
            this.txtLog.DoubleClick += new System.EventHandler(this.txtLog_DoubleClick);
            // 
            // btnCloseBind
            // 
            this.btnCloseBind.Location = new System.Drawing.Point(625, 9);
            this.btnCloseBind.Name = "btnCloseBind";
            this.btnCloseBind.Size = new System.Drawing.Size(95, 31);
            this.btnCloseBind.TabIndex = 6;
            this.btnCloseBind.Text = "关闭UDP绑定";
            this.btnCloseBind.UseVisualStyleBackColor = true;
            this.btnCloseBind.Click += new System.EventHandler(this.btnCloseBind_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenUDPClient);
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.txtSend);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRemoteIP);
            this.groupBox1.Controls.Add(this.txtRemotePort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(21, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 101);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "目标";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(503, 17);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(95, 31);
            this.btnSend.TabIndex = 11;
            this.btnSend.Text = "发送文本";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(79, 61);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(620, 23);
            this.txtSend.TabIndex = 10;
            this.txtSend.Text = "你好";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "待发送内容:";
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Location = new System.Drawing.Point(79, 22);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(169, 23);
            this.txtRemoteIP.TabIndex = 8;
            this.txtRemoteIP.Text = "255.255.255.255";
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.Location = new System.Drawing.Point(357, 22);
            this.txtRemotePort.Maximum = new decimal(new int[] {
            65500,
            0,
            0,
            0});
            this.txtRemotePort.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(120, 23);
            this.txtRemotePort.TabIndex = 7;
            this.txtRemotePort.Value = new decimal(new int[] {
            888,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "远程端口号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "远程IP:";
            // 
            // btnOpenUDPClient
            // 
            this.btnOpenUDPClient.Location = new System.Drawing.Point(604, 18);
            this.btnOpenUDPClient.Name = "btnOpenUDPClient";
            this.btnOpenUDPClient.Size = new System.Drawing.Size(95, 31);
            this.btnOpenUDPClient.TabIndex = 12;
            this.btnOpenUDPClient.Text = "打开子通道";
            this.btnOpenUDPClient.UseVisualStyleBackColor = true;
            this.btnOpenUDPClient.Click += new System.EventHandler(this.btnOpenUDPClient_Click);
            // 
            // frmUDPTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 603);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCloseBind);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnBindPort);
            this.Controls.Add(this.txtLocalPort);
            this.Controls.Add(this.lblLocalPort);
            this.Controls.Add(this.lblLocalIP);
            this.Controls.Add(this.cmbLocalIP);
            this.Name = "frmUDPTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UDP 测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUDPTest_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemotePort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cmbLocalIP;
        private Label lblLocalIP;
        private Label lblLocalPort;
        private NumericUpDown txtLocalPort;
        private Button btnBindPort;
        private TextBox txtLog;
        private Button btnCloseBind;
        private GroupBox groupBox1;
        private Button btnSend;
        private TextBox txtSend;
        private Label label3;
        private TextBox txtRemoteIP;
        private NumericUpDown txtRemotePort;
        private Label label1;
        private Label label2;
        private Button btnOpenUDPClient;
    }
}