<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWebSocketClient
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
        Me.chkShowLog = New System.Windows.Forms.CheckBox()
        Me.tmrConnects = New System.Windows.Forms.Timer(Me.components)
        Me.Button5 = New System.Windows.Forms.Button()
        Me.txtConnected = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNewConnects = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.butOpenConnectList = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.chkSSL = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.butCloseTCP = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.butTCPClientSend_200K = New System.Windows.Forms.Button()
        Me.txtSendText = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.butConnect = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.txtRemoteIP = New System.Windows.Forms.TextBox()
        Me.txtRemotePort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbLocalIP = New System.Windows.Forms.ComboBox()
        Me.txtLocalPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSendBytes = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtReadBytes = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkAutoACK = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'butDebugList
        '
        Me.butDebugList.Location = New System.Drawing.Point(652, 109)
        Me.butDebugList.Name = "butDebugList"
        Me.butDebugList.Size = New System.Drawing.Size(70, 23)
        Me.butDebugList.TabIndex = 90
        Me.butDebugList.Text = "打印列表"
        Me.butDebugList.UseVisualStyleBackColor = True
        '
        'chkShowLog
        '
        Me.chkShowLog.AutoSize = True
        Me.chkShowLog.Location = New System.Drawing.Point(16, 202)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(72, 16)
        Me.chkShowLog.TabIndex = 88
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'tmrConnects
        '
        Me.tmrConnects.Enabled = True
        Me.tmrConnects.Interval = 500
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(414, 139)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 89
        Me.Button5.Text = "循环发送"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'txtConnected
        '
        Me.txtConnected.Location = New System.Drawing.Point(578, 109)
        Me.txtConnected.MaxLength = 0
        Me.txtConnected.Name = "txtConnected"
        Me.txtConnected.Size = New System.Drawing.Size(68, 21)
        Me.txtConnected.TabIndex = 86
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(518, 113)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 12)
        Me.Label7.TabIndex = 87
        Me.Label7.Text = "已连接数："
        '
        'txtNewConnects
        '
        Me.txtNewConnects.Location = New System.Drawing.Point(175, 111)
        Me.txtNewConnects.MaxLength = 5
        Me.txtNewConnects.Name = "txtNewConnects"
        Me.txtNewConnects.Size = New System.Drawing.Size(68, 21)
        Me.txtNewConnects.TabIndex = 84
        Me.txtNewConnects.Text = "100"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(115, 115)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 12)
        Me.Label6.TabIndex = 85
        Me.Label6.Text = "连接数量"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(414, 109)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 83
        Me.Button4.Text = "批量发送"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(333, 109)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 82
        Me.Button3.Text = "批量关闭"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'butOpenConnectList
        '
        Me.butOpenConnectList.Location = New System.Drawing.Point(252, 109)
        Me.butOpenConnectList.Name = "butOpenConnectList"
        Me.butOpenConnectList.Size = New System.Drawing.Size(75, 23)
        Me.butOpenConnectList.TabIndex = 81
        Me.butOpenConnectList.Text = "批量连接"
        Me.butOpenConnectList.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(616, 12)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 80
        Me.Button2.Text = "GC"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'chkSSL
        '
        Me.chkSSL.AutoSize = True
        Me.chkSSL.Location = New System.Drawing.Point(559, 40)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(42, 16)
        Me.chkSSL.TabIndex = 79
        Me.chkSSL.Text = "SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(616, 61)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 78
        Me.Button1.Text = "测试"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'butCloseTCP
        '
        Me.butCloseTCP.Location = New System.Drawing.Point(331, 61)
        Me.butCloseTCP.Name = "butCloseTCP"
        Me.butCloseTCP.Size = New System.Drawing.Size(75, 23)
        Me.butCloseTCP.TabIndex = 77
        Me.butCloseTCP.Text = "关闭"
        Me.butCloseTCP.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(16, 224)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(973, 281)
        Me.txtLog.TabIndex = 76
        '
        'butTCPClientSend_200K
        '
        Me.butTCPClientSend_200K.Location = New System.Drawing.Point(97, 195)
        Me.butTCPClientSend_200K.Name = "butTCPClientSend_200K"
        Me.butTCPClientSend_200K.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend_200K.TabIndex = 75
        Me.butTCPClientSend_200K.Text = "发送200K"
        Me.butTCPClientSend_200K.UseVisualStyleBackColor = True
        '
        'txtSendText
        '
        Me.txtSendText.Location = New System.Drawing.Point(97, 168)
        Me.txtSendText.Name = "txtSendText"
        Me.txtSendText.Size = New System.Drawing.Size(680, 21)
        Me.txtSendText.TabIndex = 68
        Me.txtSendText.Text = "123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(37, 172)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 74
        Me.Label5.Text = "发送内容"
        '
        'butConnect
        '
        Me.butConnect.Location = New System.Drawing.Point(250, 61)
        Me.butConnect.Name = "butConnect"
        Me.butConnect.Size = New System.Drawing.Size(75, 23)
        Me.butConnect.TabIndex = 67
        Me.butConnect.Text = "连接"
        Me.butConnect.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(702, 195)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend.TabIndex = 69
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'txtRemoteIP
        '
        Me.txtRemoteIP.Location = New System.Drawing.Point(250, 8)
        Me.txtRemoteIP.Name = "txtRemoteIP"
        Me.txtRemoteIP.Size = New System.Drawing.Size(140, 21)
        Me.txtRemoteIP.TabIndex = 63
        '
        'txtRemotePort
        '
        Me.txtRemotePort.Location = New System.Drawing.Point(483, 8)
        Me.txtRemotePort.Name = "txtRemotePort"
        Me.txtRemotePort.Size = New System.Drawing.Size(70, 21)
        Me.txtRemotePort.TabIndex = 64
        Me.txtRemotePort.Text = "9001"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(424, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 73
        Me.Label1.Text = "远程端口"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(190, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 72
        Me.Label2.Text = "远程地址"
        '
        'cmbLocalIP
        '
        Me.cmbLocalIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalIP.FormattingEnabled = True
        Me.cmbLocalIP.Location = New System.Drawing.Point(250, 35)
        Me.cmbLocalIP.Name = "cmbLocalIP"
        Me.cmbLocalIP.Size = New System.Drawing.Size(140, 20)
        Me.cmbLocalIP.TabIndex = 65
        '
        'txtLocalPort
        '
        Me.txtLocalPort.Location = New System.Drawing.Point(483, 35)
        Me.txtLocalPort.Name = "txtLocalPort"
        Me.txtLocalPort.Size = New System.Drawing.Size(70, 21)
        Me.txtLocalPort.TabIndex = 66
        Me.txtLocalPort.Text = "888"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(424, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 71
        Me.Label4.Text = "本地端口"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(190, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 70
        Me.Label3.Text = "本地地址"
        '
        'txtSendBytes
        '
        Me.txtSendBytes.Location = New System.Drawing.Point(578, 136)
        Me.txtSendBytes.MaxLength = 0
        Me.txtSendBytes.Name = "txtSendBytes"
        Me.txtSendBytes.Size = New System.Drawing.Size(68, 21)
        Me.txtSendBytes.TabIndex = 91
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(518, 140)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 12)
        Me.Label8.TabIndex = 92
        Me.Label8.Text = "发送字节："
        '
        'txtReadBytes
        '
        Me.txtReadBytes.Location = New System.Drawing.Point(737, 137)
        Me.txtReadBytes.MaxLength = 0
        Me.txtReadBytes.Name = "txtReadBytes"
        Me.txtReadBytes.Size = New System.Drawing.Size(68, 21)
        Me.txtReadBytes.TabIndex = 93
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(677, 141)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 12)
        Me.Label9.TabIndex = 94
        Me.Label9.Text = "接收字节："
        '
        'chkAutoACK
        '
        Me.chkAutoACK.AutoSize = True
        Me.chkAutoACK.Location = New System.Drawing.Point(783, 170)
        Me.chkAutoACK.Name = "chkAutoACK"
        Me.chkAutoACK.Size = New System.Drawing.Size(72, 16)
        Me.chkAutoACK.TabIndex = 95
        Me.chkAutoACK.Text = "连续响应"
        Me.chkAutoACK.UseVisualStyleBackColor = True
        '
        'frmWebSocketClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1001, 515)
        Me.Controls.Add(Me.chkAutoACK)
        Me.Controls.Add(Me.txtReadBytes)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtSendBytes)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.butDebugList)
        Me.Controls.Add(Me.chkShowLog)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.txtConnected)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNewConnects)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.butOpenConnectList)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.chkSSL)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.butCloseTCP)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.butTCPClientSend_200K)
        Me.Controls.Add(Me.txtSendText)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.butConnect)
        Me.Controls.Add(Me.butTCPClientSend)
        Me.Controls.Add(Me.txtRemoteIP)
        Me.Controls.Add(Me.txtRemotePort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbLocalIP)
        Me.Controls.Add(Me.txtLocalPort)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Name = "frmWebSocketClient"
        Me.Text = "Websocket 客户端测试"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents butDebugList As Button
    Friend WithEvents chkShowLog As CheckBox
    Friend WithEvents tmrConnects As Timer
    Friend WithEvents Button5 As Button
    Friend WithEvents txtConnected As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtNewConnects As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents butOpenConnectList As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents chkSSL As CheckBox
    Friend WithEvents Button1 As Button
    Friend WithEvents butCloseTCP As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents butTCPClientSend_200K As Button
    Friend WithEvents txtSendText As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents butConnect As Button
    Friend WithEvents butTCPClientSend As Button
    Friend WithEvents txtRemoteIP As TextBox
    Friend WithEvents txtRemotePort As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbLocalIP As ComboBox
    Friend WithEvents txtLocalPort As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSendBytes As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtReadBytes As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents chkAutoACK As CheckBox
End Class
