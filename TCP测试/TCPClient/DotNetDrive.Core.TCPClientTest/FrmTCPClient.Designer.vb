<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTCPClient
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
        Me.cmbLocalIP = New System.Windows.Forms.ComboBox()
        Me.txtLocalPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRemotePort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRemoteIP = New System.Windows.Forms.TextBox()
        Me.butConnect = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.txtSendText = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.butTCPClientSend_200K = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.butCloseTCP = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.chkSSL = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.butOpenConnectList = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.txtNewConnects = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtConnected = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tmrConnects = New System.Windows.Forms.Timer(Me.components)
        Me.chkShowLog = New System.Windows.Forms.CheckBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.butDebugList = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmbLocalIP
        '
        Me.cmbLocalIP.FormattingEnabled = True
        Me.cmbLocalIP.Location = New System.Drawing.Point(246, 39)
        Me.cmbLocalIP.Name = "cmbLocalIP"
        Me.cmbLocalIP.Size = New System.Drawing.Size(140, 20)
        Me.cmbLocalIP.TabIndex = 2
        Me.cmbLocalIP.Text = "192.168.1.86"
        '
        'txtLocalPort
        '
        Me.txtLocalPort.Location = New System.Drawing.Point(479, 39)
        Me.txtLocalPort.Name = "txtLocalPort"
        Me.txtLocalPort.Size = New System.Drawing.Size(70, 21)
        Me.txtLocalPort.TabIndex = 3
        Me.txtLocalPort.Text = "888"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(420, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "本地端口"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(186, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "本地地址"
        '
        'txtRemotePort
        '
        Me.txtRemotePort.Location = New System.Drawing.Point(479, 12)
        Me.txtRemotePort.Name = "txtRemotePort"
        Me.txtRemotePort.Size = New System.Drawing.Size(70, 21)
        Me.txtRemotePort.TabIndex = 1
        Me.txtRemotePort.Text = "9001"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(420, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "远程端口"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(186, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "远程地址"
        '
        'txtRemoteIP
        '
        Me.txtRemoteIP.Location = New System.Drawing.Point(246, 12)
        Me.txtRemoteIP.Name = "txtRemoteIP"
        Me.txtRemoteIP.Size = New System.Drawing.Size(140, 21)
        Me.txtRemoteIP.TabIndex = 0
        Me.txtRemoteIP.Text = "192.168.1.86"
        '
        'butConnect
        '
        Me.butConnect.Location = New System.Drawing.Point(246, 65)
        Me.butConnect.Name = "butConnect"
        Me.butConnect.Size = New System.Drawing.Size(75, 23)
        Me.butConnect.TabIndex = 4
        Me.butConnect.Text = "连接"
        Me.butConnect.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(698, 199)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend.TabIndex = 6
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'txtSendText
        '
        Me.txtSendText.Location = New System.Drawing.Point(93, 172)
        Me.txtSendText.Name = "txtSendText"
        Me.txtSendText.Size = New System.Drawing.Size(680, 21)
        Me.txtSendText.TabIndex = 5
        Me.txtSendText.Text = "123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(33, 176)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "发送内容"
        '
        'butTCPClientSend_200K
        '
        Me.butTCPClientSend_200K.Location = New System.Drawing.Point(93, 199)
        Me.butTCPClientSend_200K.Name = "butTCPClientSend_200K"
        Me.butTCPClientSend_200K.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend_200K.TabIndex = 41
        Me.butTCPClientSend_200K.Text = "发送200K"
        Me.butTCPClientSend_200K.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 228)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(973, 281)
        Me.txtLog.TabIndex = 42
        '
        'butCloseTCP
        '
        Me.butCloseTCP.Location = New System.Drawing.Point(327, 65)
        Me.butCloseTCP.Name = "butCloseTCP"
        Me.butCloseTCP.Size = New System.Drawing.Size(75, 23)
        Me.butCloseTCP.TabIndex = 43
        Me.butCloseTCP.Text = "关闭"
        Me.butCloseTCP.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(612, 65)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 44
        Me.Button1.Text = "测试"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkSSL
        '
        Me.chkSSL.AutoSize = True
        Me.chkSSL.Location = New System.Drawing.Point(555, 44)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(42, 16)
        Me.chkSSL.TabIndex = 45
        Me.chkSSL.Text = "SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(612, 16)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 46
        Me.Button2.Text = "GC"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butOpenConnectList
        '
        Me.butOpenConnectList.Location = New System.Drawing.Point(248, 113)
        Me.butOpenConnectList.Name = "butOpenConnectList"
        Me.butOpenConnectList.Size = New System.Drawing.Size(75, 23)
        Me.butOpenConnectList.TabIndex = 47
        Me.butOpenConnectList.Text = "批量连接"
        Me.butOpenConnectList.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(329, 113)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 48
        Me.Button3.Text = "批量关闭"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(410, 113)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 49
        Me.Button4.Text = "批量发送"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'txtNewConnects
        '
        Me.txtNewConnects.Location = New System.Drawing.Point(171, 115)
        Me.txtNewConnects.MaxLength = 5
        Me.txtNewConnects.Name = "txtNewConnects"
        Me.txtNewConnects.Size = New System.Drawing.Size(68, 21)
        Me.txtNewConnects.TabIndex = 50
        Me.txtNewConnects.Text = "100"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(111, 119)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 12)
        Me.Label6.TabIndex = 51
        Me.Label6.Text = "连接数量"
        '
        'txtConnected
        '
        Me.txtConnected.Location = New System.Drawing.Point(574, 113)
        Me.txtConnected.MaxLength = 0
        Me.txtConnected.Name = "txtConnected"
        Me.txtConnected.Size = New System.Drawing.Size(68, 21)
        Me.txtConnected.TabIndex = 52
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(514, 117)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 12)
        Me.Label7.TabIndex = 53
        Me.Label7.Text = "已连接数："
        '
        'tmrConnects
        '
        Me.tmrConnects.Enabled = True
        Me.tmrConnects.Interval = 500
        '
        'chkShowLog
        '
        Me.chkShowLog.AutoSize = True
        Me.chkShowLog.Checked = True
        Me.chkShowLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowLog.Location = New System.Drawing.Point(12, 206)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(72, 16)
        Me.chkShowLog.TabIndex = 54
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(410, 143)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 55
        Me.Button5.Text = "循环发送"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'butDebugList
        '
        Me.butDebugList.Location = New System.Drawing.Point(648, 113)
        Me.butDebugList.Name = "butDebugList"
        Me.butDebugList.Size = New System.Drawing.Size(70, 23)
        Me.butDebugList.TabIndex = 62
        Me.butDebugList.Text = "打印列表"
        Me.butDebugList.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(779, 199)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 23)
        Me.Button6.TabIndex = 63
        Me.Button6.Text = "发送"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'FrmTCPClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 547)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.butDebugList)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.chkShowLog)
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
        Me.Name = "FrmTCPClient"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbLocalIP As ComboBox
    Friend WithEvents txtLocalPort As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtRemotePort As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRemoteIP As TextBox
    Friend WithEvents butConnect As Button
    Friend WithEvents butTCPClientSend As Button
    Friend WithEvents txtSendText As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents butTCPClientSend_200K As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents butCloseTCP As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents chkSSL As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents butOpenConnectList As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents txtNewConnects As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtConnected As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents tmrConnects As Timer
    Friend WithEvents chkShowLog As CheckBox
    Friend WithEvents Button5 As Button
    Friend WithEvents butDebugList As Button
    Friend WithEvents Button6 As Button
End Class
