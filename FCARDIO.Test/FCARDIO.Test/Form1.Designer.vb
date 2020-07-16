<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.butOpenTCPServer = New System.Windows.Forms.Button()
        Me.butCloseTCPServer = New System.Windows.Forms.Button()
        Me.butCloseUDPServer = New System.Windows.Forms.Button()
        Me.butOpenUDPServer = New System.Windows.Forms.Button()
        Me.cmdUDPClient = New System.Windows.Forms.ComboBox()
        Me.txtUDPClient = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.butCloseUDP = New System.Windows.Forms.Button()
        Me.butUDPSend = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkUDPIsHEX = New System.Windows.Forms.CheckBox()
        Me.txtUDPPort = New System.Windows.Forms.TextBox()
        Me.txtUDPAddr = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.butCloseUDPClient = New System.Windows.Forms.Button()
        Me.butSendUDPClient = New System.Windows.Forms.Button()
        Me.txtUDPClientText = New System.Windows.Forms.TextBox()
        Me.butTCPClientByDomain = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.butCloseTCPClient = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.cmbTCPClient = New System.Windows.Forms.ComboBox()
        Me.txtTCPClientText = New System.Windows.Forms.TextBox()
        Me.butSerialPort = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.ButCloseWebSocketServer = New System.Windows.Forms.Button()
        Me.ButOpenWebSocketServer = New System.Windows.Forms.Button()
        Me.lblWebsocketServer = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.butWebsocketClient = New System.Windows.Forms.Button()
        Me.chkEcho = New System.Windows.Forms.CheckBox()
        Me.chkHexDump = New System.Windows.Forms.CheckBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtWatchAddr = New System.Windows.Forms.TextBox()
        Me.txtWatchPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(18, 33)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(189, 57)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "TCPClient"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(235, 451)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(189, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "释放所有资源"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butOpenTCPServer
        '
        Me.butOpenTCPServer.Location = New System.Drawing.Point(18, 96)
        Me.butOpenTCPServer.Name = "butOpenTCPServer"
        Me.butOpenTCPServer.Size = New System.Drawing.Size(189, 57)
        Me.butOpenTCPServer.TabIndex = 2
        Me.butOpenTCPServer.Text = "打开 TCPServer"
        Me.butOpenTCPServer.UseVisualStyleBackColor = True
        '
        'butCloseTCPServer
        '
        Me.butCloseTCPServer.Location = New System.Drawing.Point(213, 96)
        Me.butCloseTCPServer.Name = "butCloseTCPServer"
        Me.butCloseTCPServer.Size = New System.Drawing.Size(189, 57)
        Me.butCloseTCPServer.TabIndex = 3
        Me.butCloseTCPServer.Text = "关闭 TCPServer"
        Me.butCloseTCPServer.UseVisualStyleBackColor = True
        '
        'butCloseUDPServer
        '
        Me.butCloseUDPServer.Location = New System.Drawing.Point(213, 155)
        Me.butCloseUDPServer.Name = "butCloseUDPServer"
        Me.butCloseUDPServer.Size = New System.Drawing.Size(189, 57)
        Me.butCloseUDPServer.TabIndex = 5
        Me.butCloseUDPServer.Text = "关闭 UDPServer"
        Me.butCloseUDPServer.UseVisualStyleBackColor = True
        '
        'butOpenUDPServer
        '
        Me.butOpenUDPServer.Location = New System.Drawing.Point(18, 155)
        Me.butOpenUDPServer.Name = "butOpenUDPServer"
        Me.butOpenUDPServer.Size = New System.Drawing.Size(189, 57)
        Me.butOpenUDPServer.TabIndex = 4
        Me.butOpenUDPServer.Text = "打开 UDPServer"
        Me.butOpenUDPServer.UseVisualStyleBackColor = True
        '
        'cmdUDPClient
        '
        Me.cmdUDPClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmdUDPClient.FormattingEnabled = True
        Me.cmdUDPClient.Location = New System.Drawing.Point(11, 20)
        Me.cmdUDPClient.Name = "cmdUDPClient"
        Me.cmdUDPClient.Size = New System.Drawing.Size(384, 20)
        Me.cmdUDPClient.TabIndex = 6
        '
        'txtUDPClient
        '
        Me.txtUDPClient.Location = New System.Drawing.Point(11, 47)
        Me.txtUDPClient.Name = "txtUDPClient"
        Me.txtUDPClient.Size = New System.Drawing.Size(384, 21)
        Me.txtUDPClient.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.butCloseUDP)
        Me.GroupBox1.Controls.Add(Me.butUDPSend)
        Me.GroupBox1.Controls.Add(Me.cmdUDPClient)
        Me.GroupBox1.Controls.Add(Me.txtUDPClient)
        Me.GroupBox1.Location = New System.Drawing.Point(18, 229)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(406, 100)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "UDP客户端_被动连接"
        '
        'butCloseUDP
        '
        Me.butCloseUDP.Location = New System.Drawing.Point(11, 71)
        Me.butCloseUDP.Name = "butCloseUDP"
        Me.butCloseUDP.Size = New System.Drawing.Size(75, 23)
        Me.butCloseUDP.TabIndex = 10
        Me.butCloseUDP.Text = "关闭连接"
        Me.butCloseUDP.UseVisualStyleBackColor = True
        '
        'butUDPSend
        '
        Me.butUDPSend.Location = New System.Drawing.Point(320, 71)
        Me.butUDPSend.Name = "butUDPSend"
        Me.butUDPSend.Size = New System.Drawing.Size(75, 23)
        Me.butUDPSend.TabIndex = 9
        Me.butUDPSend.Text = "发送"
        Me.butUDPSend.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkUDPIsHEX)
        Me.GroupBox2.Controls.Add(Me.txtUDPPort)
        Me.GroupBox2.Controls.Add(Me.txtUDPAddr)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.butCloseUDPClient)
        Me.GroupBox2.Controls.Add(Me.butSendUDPClient)
        Me.GroupBox2.Controls.Add(Me.txtUDPClientText)
        Me.GroupBox2.Location = New System.Drawing.Point(18, 335)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(406, 100)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "UDP客户端_主动连接"
        '
        'chkUDPIsHEX
        '
        Me.chkUDPIsHEX.AutoSize = True
        Me.chkUDPIsHEX.Checked = True
        Me.chkUDPIsHEX.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUDPIsHEX.Location = New System.Drawing.Point(92, 74)
        Me.chkUDPIsHEX.Name = "chkUDPIsHEX"
        Me.chkUDPIsHEX.Size = New System.Drawing.Size(42, 16)
        Me.chkUDPIsHEX.TabIndex = 17
        Me.chkUDPIsHEX.Text = "Hex"
        Me.chkUDPIsHEX.UseVisualStyleBackColor = True
        '
        'txtUDPPort
        '
        Me.txtUDPPort.Location = New System.Drawing.Point(343, 20)
        Me.txtUDPPort.Name = "txtUDPPort"
        Me.txtUDPPort.Size = New System.Drawing.Size(52, 21)
        Me.txtUDPPort.TabIndex = 14
        Me.txtUDPPort.Text = "8880"
        '
        'txtUDPAddr
        '
        Me.txtUDPAddr.Location = New System.Drawing.Point(68, 20)
        Me.txtUDPAddr.Name = "txtUDPAddr"
        Me.txtUDPAddr.Size = New System.Drawing.Size(194, 21)
        Me.txtUDPAddr.TabIndex = 12
        Me.txtUDPAddr.Text = "yun.pc15.net"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(296, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 12)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "端口："
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "远程IP："
        '
        'butCloseUDPClient
        '
        Me.butCloseUDPClient.Location = New System.Drawing.Point(11, 71)
        Me.butCloseUDPClient.Name = "butCloseUDPClient"
        Me.butCloseUDPClient.Size = New System.Drawing.Size(75, 23)
        Me.butCloseUDPClient.TabIndex = 10
        Me.butCloseUDPClient.Text = "关闭连接"
        Me.butCloseUDPClient.UseVisualStyleBackColor = True
        '
        'butSendUDPClient
        '
        Me.butSendUDPClient.Location = New System.Drawing.Point(320, 71)
        Me.butSendUDPClient.Name = "butSendUDPClient"
        Me.butSendUDPClient.Size = New System.Drawing.Size(75, 23)
        Me.butSendUDPClient.TabIndex = 9
        Me.butSendUDPClient.Text = "发送"
        Me.butSendUDPClient.UseVisualStyleBackColor = True
        '
        'txtUDPClientText
        '
        Me.txtUDPClientText.Location = New System.Drawing.Point(11, 47)
        Me.txtUDPClientText.Name = "txtUDPClientText"
        Me.txtUDPClientText.Size = New System.Drawing.Size(384, 21)
        Me.txtUDPClientText.TabIndex = 7
        Me.txtUDPClientText.Text = "aabbccdd"
        '
        'butTCPClientByDomain
        '
        Me.butTCPClientByDomain.Location = New System.Drawing.Point(213, 33)
        Me.butTCPClientByDomain.Name = "butTCPClientByDomain"
        Me.butTCPClientByDomain.Size = New System.Drawing.Size(189, 57)
        Me.butTCPClientByDomain.TabIndex = 10
        Me.butTCPClientByDomain.Text = "TCPClient by 域名"
        Me.butTCPClientByDomain.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.butCloseTCPClient)
        Me.GroupBox3.Controls.Add(Me.butTCPClientSend)
        Me.GroupBox3.Controls.Add(Me.cmbTCPClient)
        Me.GroupBox3.Controls.Add(Me.txtTCPClientText)
        Me.GroupBox3.Location = New System.Drawing.Point(410, 33)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(406, 100)
        Me.GroupBox3.TabIndex = 11
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "TCP客户端_被动连接"
        '
        'butCloseTCPClient
        '
        Me.butCloseTCPClient.Location = New System.Drawing.Point(11, 71)
        Me.butCloseTCPClient.Name = "butCloseTCPClient"
        Me.butCloseTCPClient.Size = New System.Drawing.Size(75, 23)
        Me.butCloseTCPClient.TabIndex = 10
        Me.butCloseTCPClient.Text = "关闭连接"
        Me.butCloseTCPClient.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(320, 71)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend.TabIndex = 9
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'cmbTCPClient
        '
        Me.cmbTCPClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTCPClient.FormattingEnabled = True
        Me.cmbTCPClient.Location = New System.Drawing.Point(11, 20)
        Me.cmbTCPClient.Name = "cmbTCPClient"
        Me.cmbTCPClient.Size = New System.Drawing.Size(384, 20)
        Me.cmbTCPClient.TabIndex = 6
        '
        'txtTCPClientText
        '
        Me.txtTCPClientText.Location = New System.Drawing.Point(11, 47)
        Me.txtTCPClientText.Name = "txtTCPClientText"
        Me.txtTCPClientText.Size = New System.Drawing.Size(384, 21)
        Me.txtTCPClientText.TabIndex = 7
        Me.txtTCPClientText.Text = "1234"
        '
        'butSerialPort
        '
        Me.butSerialPort.Location = New System.Drawing.Point(421, 155)
        Me.butSerialPort.Name = "butSerialPort"
        Me.butSerialPort.Size = New System.Drawing.Size(189, 57)
        Me.butSerialPort.TabIndex = 12
        Me.butSerialPort.Text = "串口通讯"
        Me.butSerialPort.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ButCloseWebSocketServer)
        Me.GroupBox4.Controls.Add(Me.ButOpenWebSocketServer)
        Me.GroupBox4.Controls.Add(Me.lblWebsocketServer)
        Me.GroupBox4.Controls.Add(Me.Button3)
        Me.GroupBox4.Controls.Add(Me.Button4)
        Me.GroupBox4.Controls.Add(Me.ComboBox1)
        Me.GroupBox4.Controls.Add(Me.TextBox1)
        Me.GroupBox4.Location = New System.Drawing.Point(430, 229)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(406, 206)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "WebSocket Server"
        '
        'ButCloseWebSocketServer
        '
        Me.ButCloseWebSocketServer.Location = New System.Drawing.Point(201, 37)
        Me.ButCloseWebSocketServer.Name = "ButCloseWebSocketServer"
        Me.ButCloseWebSocketServer.Size = New System.Drawing.Size(189, 57)
        Me.ButCloseWebSocketServer.TabIndex = 13
        Me.ButCloseWebSocketServer.Text = "关闭 WebsocketServer"
        Me.ButCloseWebSocketServer.UseVisualStyleBackColor = True
        '
        'ButOpenWebSocketServer
        '
        Me.ButOpenWebSocketServer.Location = New System.Drawing.Point(6, 37)
        Me.ButOpenWebSocketServer.Name = "ButOpenWebSocketServer"
        Me.ButOpenWebSocketServer.Size = New System.Drawing.Size(189, 57)
        Me.ButOpenWebSocketServer.TabIndex = 12
        Me.ButOpenWebSocketServer.Text = "打开 webSocketServer"
        Me.ButOpenWebSocketServer.UseVisualStyleBackColor = True
        '
        'lblWebsocketServer
        '
        Me.lblWebsocketServer.AutoSize = True
        Me.lblWebsocketServer.Location = New System.Drawing.Point(7, 20)
        Me.lblWebsocketServer.Name = "lblWebsocketServer"
        Me.lblWebsocketServer.Size = New System.Drawing.Size(41, 12)
        Me.lblWebsocketServer.TabIndex = 11
        Me.lblWebsocketServer.Text = "服务端"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(6, 173)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 10
        Me.Button3.Text = "关闭连接"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(315, 173)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 9
        Me.Button4.Text = "发送"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(6, 122)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(384, 20)
        Me.ComboBox1.TabIndex = 6
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(6, 149)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(384, 21)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.Text = "1234"
        '
        'butWebsocketClient
        '
        Me.butWebsocketClient.Location = New System.Drawing.Point(436, 441)
        Me.butWebsocketClient.Name = "butWebsocketClient"
        Me.butWebsocketClient.Size = New System.Drawing.Size(400, 33)
        Me.butWebsocketClient.TabIndex = 14
        Me.butWebsocketClient.Text = "WebsocketClient"
        Me.butWebsocketClient.UseVisualStyleBackColor = True
        '
        'chkEcho
        '
        Me.chkEcho.AutoSize = True
        Me.chkEcho.Location = New System.Drawing.Point(788, 207)
        Me.chkEcho.Name = "chkEcho"
        Me.chkEcho.Size = New System.Drawing.Size(48, 16)
        Me.chkEcho.TabIndex = 15
        Me.chkEcho.Text = "Echo"
        Me.chkEcho.UseVisualStyleBackColor = True
        '
        'chkHexDump
        '
        Me.chkHexDump.AutoSize = True
        Me.chkHexDump.Checked = True
        Me.chkHexDump.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHexDump.Location = New System.Drawing.Point(716, 207)
        Me.chkHexDump.Name = "chkHexDump"
        Me.chkHexDump.Size = New System.Drawing.Size(66, 16)
        Me.chkHexDump.TabIndex = 16
        Me.chkHexDump.Text = "HexDump"
        Me.chkHexDump.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 497)
        Me.txtLog.MaxLength = 0
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLog.Size = New System.Drawing.Size(881, 245)
        Me.txtLog.TabIndex = 17
        Me.txtLog.WordWrap = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "监听地址"
        '
        'txtWatchAddr
        '
        Me.txtWatchAddr.Location = New System.Drawing.Point(79, 6)
        Me.txtWatchAddr.Name = "txtWatchAddr"
        Me.txtWatchAddr.Size = New System.Drawing.Size(128, 21)
        Me.txtWatchAddr.TabIndex = 19
        Me.txtWatchAddr.Text = "192.168.1.86"
        '
        'txtWatchPort
        '
        Me.txtWatchPort.Location = New System.Drawing.Point(285, 6)
        Me.txtWatchPort.Name = "txtWatchPort"
        Me.txtWatchPort.Size = New System.Drawing.Size(70, 21)
        Me.txtWatchPort.TabIndex = 21
        Me.txtWatchPort.Text = "9001"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(226, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "监听端口"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(906, 757)
        Me.Controls.Add(Me.txtWatchPort)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtWatchAddr)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.chkHexDump)
        Me.Controls.Add(Me.chkEcho)
        Me.Controls.Add(Me.butWebsocketClient)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.butSerialPort)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.butTCPClientByDomain)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.butCloseUDPServer)
        Me.Controls.Add(Me.butOpenUDPServer)
        Me.Controls.Add(Me.butCloseTCPServer)
        Me.Controls.Add(Me.butOpenTCPServer)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents butOpenTCPServer As Button
    Friend WithEvents butCloseTCPServer As Button
    Friend WithEvents butCloseUDPServer As Button
    Friend WithEvents butOpenUDPServer As Button
    Friend WithEvents cmdUDPClient As ComboBox
    Friend WithEvents txtUDPClient As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents butUDPSend As Button
    Friend WithEvents butCloseUDP As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents butCloseUDPClient As Button
    Friend WithEvents butSendUDPClient As Button
    Friend WithEvents txtUDPClientText As TextBox
    Friend WithEvents txtUDPPort As TextBox
    Friend WithEvents txtUDPAddr As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents butTCPClientByDomain As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents butCloseTCPClient As Button
    Friend WithEvents butTCPClientSend As Button
    Friend WithEvents cmbTCPClient As ComboBox
    Friend WithEvents txtTCPClientText As TextBox
    Friend WithEvents butSerialPort As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents ButCloseWebSocketServer As Button
    Friend WithEvents ButOpenWebSocketServer As Button
    Friend WithEvents lblWebsocketServer As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents butWebsocketClient As Button
    Friend WithEvents chkEcho As CheckBox
    Friend WithEvents chkHexDump As CheckBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtWatchAddr As TextBox
    Friend WithEvents txtWatchPort As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkUDPIsHEX As CheckBox
End Class
