<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWebSocketServer
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.butDebugList = New System.Windows.Forms.Button()
        Me.txtReadBytes = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tmrTotal = New System.Windows.Forms.Timer(Me.components)
        Me.txtConnectCount = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkShowLog = New System.Windows.Forms.CheckBox()
        Me.butReloadClientList = New System.Windows.Forms.Button()
        Me.butCloseTCPClient = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.chkSSL = New System.Windows.Forms.CheckBox()
        Me.cmbLocalIP = New System.Windows.Forms.ComboBox()
        Me.cmbTCPClient = New System.Windows.Forms.ComboBox()
        Me.btnGC = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtTCPClientText = New System.Windows.Forms.TextBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.butCloseTCPServer = New System.Windows.Forms.Button()
        Me.butOpenTCPServer = New System.Windows.Forms.Button()
        Me.txtWatchPort = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtServerAddr = New System.Windows.Forms.TextBox()
        Me.chkAutoACK = New System.Windows.Forms.CheckBox()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'butDebugList
        '
        Me.butDebugList.Location = New System.Drawing.Point(760, 98)
        Me.butDebugList.Name = "butDebugList"
        Me.butDebugList.Size = New System.Drawing.Size(70, 23)
        Me.butDebugList.TabIndex = 77
        Me.butDebugList.Text = "打印列表"
        Me.butDebugList.UseVisualStyleBackColor = True
        '
        'txtReadBytes
        '
        Me.txtReadBytes.Location = New System.Drawing.Point(760, 42)
        Me.txtReadBytes.Name = "txtReadBytes"
        Me.txtReadBytes.Size = New System.Drawing.Size(70, 21)
        Me.txtReadBytes.TabIndex = 75
        Me.txtReadBytes.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(661, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 12)
        Me.Label2.TabIndex = 74
        Me.Label2.Text = "已收到字节数："
        '
        'tmrTotal
        '
        Me.tmrTotal.Enabled = True
        Me.tmrTotal.Interval = 500
        '
        'txtConnectCount
        '
        Me.txtConnectCount.Location = New System.Drawing.Point(760, 14)
        Me.txtConnectCount.Name = "txtConnectCount"
        Me.txtConnectCount.Size = New System.Drawing.Size(70, 21)
        Me.txtConnectCount.TabIndex = 73
        Me.txtConnectCount.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(689, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 72
        Me.Label1.Text = "已连接数："
        '
        'chkShowLog
        '
        Me.chkShowLog.AutoSize = True
        Me.chkShowLog.Location = New System.Drawing.Point(15, 170)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(72, 16)
        Me.chkShowLog.TabIndex = 71
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'butReloadClientList
        '
        Me.butReloadClientList.Location = New System.Drawing.Point(451, 73)
        Me.butReloadClientList.Name = "butReloadClientList"
        Me.butReloadClientList.Size = New System.Drawing.Size(75, 23)
        Me.butReloadClientList.TabIndex = 12
        Me.butReloadClientList.Text = "刷新列表"
        Me.butReloadClientList.UseVisualStyleBackColor = True
        '
        'butCloseTCPClient
        '
        Me.butCloseTCPClient.Location = New System.Drawing.Point(10, 73)
        Me.butCloseTCPClient.Name = "butCloseTCPClient"
        Me.butCloseTCPClient.Size = New System.Drawing.Size(75, 23)
        Me.butCloseTCPClient.TabIndex = 10
        Me.butCloseTCPClient.Text = "关闭连接"
        Me.butCloseTCPClient.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(532, 73)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend.TabIndex = 9
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'chkSSL
        '
        Me.chkSSL.AutoSize = True
        Me.chkSSL.Location = New System.Drawing.Point(613, 56)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(42, 16)
        Me.chkSSL.TabIndex = 70
        Me.chkSSL.Text = "SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'cmbLocalIP
        '
        Me.cmbLocalIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalIP.FormattingEnabled = True
        Me.cmbLocalIP.Location = New System.Drawing.Point(129, 14)
        Me.cmbLocalIP.Name = "cmbLocalIP"
        Me.cmbLocalIP.Size = New System.Drawing.Size(140, 20)
        Me.cmbLocalIP.TabIndex = 69
        '
        'cmbTCPClient
        '
        Me.cmbTCPClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTCPClient.FormattingEnabled = True
        Me.cmbTCPClient.Location = New System.Drawing.Point(11, 20)
        Me.cmbTCPClient.Name = "cmbTCPClient"
        Me.cmbTCPClient.Size = New System.Drawing.Size(596, 20)
        Me.cmbTCPClient.TabIndex = 6
        '
        'btnGC
        '
        Me.btnGC.Location = New System.Drawing.Point(760, 69)
        Me.btnGC.Name = "btnGC"
        Me.btnGC.Size = New System.Drawing.Size(70, 23)
        Me.btnGC.TabIndex = 76
        Me.btnGC.Text = "GC"
        Me.btnGC.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Controls.Add(Me.butReloadClientList)
        Me.GroupBox3.Controls.Add(Me.butCloseTCPClient)
        Me.GroupBox3.Controls.Add(Me.butTCPClientSend)
        Me.GroupBox3.Controls.Add(Me.cmbTCPClient)
        Me.GroupBox3.Controls.Add(Me.txtTCPClientText)
        Me.GroupBox3.Location = New System.Drawing.Point(118, 82)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(613, 104)
        Me.GroupBox3.TabIndex = 68
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "TCP客户端_被动连接"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(370, 73)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "发送"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtTCPClientText
        '
        Me.txtTCPClientText.Location = New System.Drawing.Point(11, 46)
        Me.txtTCPClientText.Name = "txtTCPClientText"
        Me.txtTCPClientText.Size = New System.Drawing.Size(596, 21)
        Me.txtTCPClientText.TabIndex = 7
        Me.txtTCPClientText.Text = "1234"
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(15, 192)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(815, 282)
        Me.txtLog.TabIndex = 67
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(303, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 65
        Me.Label4.Text = "监听端口"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(69, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 64
        Me.Label3.Text = "监听地址"
        '
        'butCloseTCPServer
        '
        Me.butCloseTCPServer.Location = New System.Drawing.Point(418, 50)
        Me.butCloseTCPServer.Name = "butCloseTCPServer"
        Me.butCloseTCPServer.Size = New System.Drawing.Size(189, 27)
        Me.butCloseTCPServer.TabIndex = 63
        Me.butCloseTCPServer.Text = "关闭 WebSocketServer"
        Me.butCloseTCPServer.UseVisualStyleBackColor = True
        '
        'butOpenTCPServer
        '
        Me.butOpenTCPServer.Location = New System.Drawing.Point(223, 50)
        Me.butOpenTCPServer.Name = "butOpenTCPServer"
        Me.butOpenTCPServer.Size = New System.Drawing.Size(189, 27)
        Me.butOpenTCPServer.TabIndex = 62
        Me.butOpenTCPServer.Text = "打开 WebSocketServer"
        Me.butOpenTCPServer.UseVisualStyleBackColor = True
        '
        'txtWatchPort
        '
        Me.txtWatchPort.Location = New System.Drawing.Point(362, 14)
        Me.txtWatchPort.MaxLength = 5
        Me.txtWatchPort.Name = "txtWatchPort"
        Me.txtWatchPort.Size = New System.Drawing.Size(70, 21)
        Me.txtWatchPort.TabIndex = 66
        Me.txtWatchPort.Text = "9001"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(457, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 12)
        Me.Label5.TabIndex = 78
        Me.Label5.Text = "服务地址："
        '
        'txtServerAddr
        '
        Me.txtServerAddr.Location = New System.Drawing.Point(528, 12)
        Me.txtServerAddr.MaxLength = 100
        Me.txtServerAddr.Name = "txtServerAddr"
        Me.txtServerAddr.Size = New System.Drawing.Size(127, 21)
        Me.txtServerAddr.TabIndex = 79
        Me.txtServerAddr.Text = "/WebSocket"
        '
        'chkAutoACK
        '
        Me.chkAutoACK.AutoSize = True
        Me.chkAutoACK.Location = New System.Drawing.Point(737, 162)
        Me.chkAutoACK.Name = "chkAutoACK"
        Me.chkAutoACK.Size = New System.Drawing.Size(72, 16)
        Me.chkAutoACK.TabIndex = 80
        Me.chkAutoACK.Text = "自动回应"
        Me.chkAutoACK.UseVisualStyleBackColor = True
        '
        'frmWebSocketServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(848, 493)
        Me.Controls.Add(Me.chkAutoACK)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtServerAddr)
        Me.Controls.Add(Me.butDebugList)
        Me.Controls.Add(Me.txtReadBytes)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtConnectCount)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkShowLog)
        Me.Controls.Add(Me.chkSSL)
        Me.Controls.Add(Me.cmbLocalIP)
        Me.Controls.Add(Me.btnGC)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.butCloseTCPServer)
        Me.Controls.Add(Me.butOpenTCPServer)
        Me.Controls.Add(Me.txtWatchPort)
        Me.Name = "frmWebSocketServer"
        Me.Text = "WebSocketServer"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents butDebugList As Button
    Friend WithEvents txtReadBytes As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tmrTotal As Timer
    Friend WithEvents txtConnectCount As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents chkShowLog As CheckBox
    Friend WithEvents butReloadClientList As Button
    Friend WithEvents butCloseTCPClient As Button
    Friend WithEvents butTCPClientSend As Button
    Friend WithEvents chkSSL As CheckBox
    Friend WithEvents cmbLocalIP As ComboBox
    Friend WithEvents cmbTCPClient As ComboBox
    Friend WithEvents btnGC As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents txtTCPClientText As TextBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents butCloseTCPServer As Button
    Friend WithEvents butOpenTCPServer As Button
    Friend WithEvents txtWatchPort As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtServerAddr As TextBox
    Friend WithEvents chkAutoACK As CheckBox
    Friend WithEvents Button1 As Button
End Class
