namespace WebClientToolByWinForm
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServerInfo = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.butSendMessage = new System.Windows.Forms.Button();
            this.butCloseTCPClient = new System.Windows.Forms.Button();
            this.butBeginTCPServer = new System.Windows.Forms.Button();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbTCPClient = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkShowIO = new System.Windows.Forms.CheckBox();
            this.butClear = new System.Windows.Forms.Button();
            this.gbServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.Location = new System.Drawing.Point(157, 19);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(65, 12);
            this.lblServerInfo.TabIndex = 0;
            this.lblServerInfo.Text = "服务信息：";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(228, 16);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(238, 21);
            this.txtServer.TabIndex = 1;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(13, 146);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1031, 534);
            this.txtLog.TabIndex = 2;
            this.txtLog.WordWrap = false;
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.butSendMessage);
            this.gbServer.Controls.Add(this.butCloseTCPClient);
            this.gbServer.Controls.Add(this.butBeginTCPServer);
            this.gbServer.Controls.Add(this.txtServerPort);
            this.gbServer.Controls.Add(this.label9);
            this.gbServer.Controls.Add(this.cmbTCPClient);
            this.gbServer.Controls.Add(this.label8);
            this.gbServer.Location = new System.Drawing.Point(12, 43);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new System.Drawing.Size(598, 72);
            this.gbServer.TabIndex = 11;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "TCP客户端";
            // 
            // butSendMessage
            // 
            this.butSendMessage.Location = new System.Drawing.Point(436, 18);
            this.butSendMessage.Name = "butSendMessage";
            this.butSendMessage.Size = new System.Drawing.Size(75, 23);
            this.butSendMessage.TabIndex = 11;
            this.butSendMessage.Text = "发送消息";
            this.butSendMessage.UseVisualStyleBackColor = true;
            this.butSendMessage.Click += new System.EventHandler(this.butSendMessage_Click);
            // 
            // butCloseTCPClient
            // 
            this.butCloseTCPClient.Location = new System.Drawing.Point(517, 18);
            this.butCloseTCPClient.Name = "butCloseTCPClient";
            this.butCloseTCPClient.Size = new System.Drawing.Size(75, 23);
            this.butCloseTCPClient.TabIndex = 10;
            this.butCloseTCPClient.Text = "关闭客户端";
            this.butCloseTCPClient.UseVisualStyleBackColor = true;
            this.butCloseTCPClient.Click += new System.EventHandler(this.butCloseTCPClient_Click);
            // 
            // butBeginTCPServer
            // 
            this.butBeginTCPServer.Location = new System.Drawing.Point(135, 18);
            this.butBeginTCPServer.Name = "butBeginTCPServer";
            this.butBeginTCPServer.Size = new System.Drawing.Size(75, 23);
            this.butBeginTCPServer.TabIndex = 9;
            this.butBeginTCPServer.Text = "开始监听";
            this.butBeginTCPServer.UseVisualStyleBackColor = true;
            this.butBeginTCPServer.Click += new System.EventHandler(this.butBeginTCPServer_Click);
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(69, 20);
            this.txtServerPort.MaxLength = 5;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(58, 21);
            this.txtServerPort.TabIndex = 8;
            this.txtServerPort.Text = "8000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "服务端口：";
            // 
            // cmbTCPClient
            // 
            this.cmbTCPClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTCPClient.FormattingEnabled = true;
            this.cmbTCPClient.Location = new System.Drawing.Point(69, 47);
            this.cmbTCPClient.Name = "cmbTCPClient";
            this.cmbTCPClient.Size = new System.Drawing.Size(523, 20);
            this.cmbTCPClient.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "客户端：";
            // 
            // chkShowIO
            // 
            this.chkShowIO.AutoSize = true;
            this.chkShowIO.Checked = true;
            this.chkShowIO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowIO.Location = new System.Drawing.Point(92, 124);
            this.chkShowIO.Name = "chkShowIO";
            this.chkShowIO.Size = new System.Drawing.Size(84, 16);
            this.chkShowIO.TabIndex = 13;
            this.chkShowIO.Text = "显示IO日志";
            this.chkShowIO.UseVisualStyleBackColor = true;
            // 
            // butClear
            // 
            this.butClear.Location = new System.Drawing.Point(11, 120);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(75, 23);
            this.butClear.TabIndex = 12;
            this.butClear.Text = "清空";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 683);
            this.Controls.Add(this.chkShowIO);
            this.Controls.Add(this.butClear);
            this.Controls.Add(this.gbServer);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServerInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "云一卡通桌面辅助工具";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerInfo;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.Button butSendMessage;
        private System.Windows.Forms.Button butCloseTCPClient;
        private System.Windows.Forms.Button butBeginTCPServer;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbTCPClient;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkShowIO;
        private System.Windows.Forms.Button butClear;
    }
}

