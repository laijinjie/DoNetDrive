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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTCPClient))
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
        Me.SuspendLayout()
        '
        'cmbLocalIP
        '
        Me.cmbLocalIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalIP.FormattingEnabled = True
        Me.cmbLocalIP.Location = New System.Drawing.Point(287, 55)
        Me.cmbLocalIP.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbLocalIP.Name = "cmbLocalIP"
        Me.cmbLocalIP.Size = New System.Drawing.Size(163, 25)
        Me.cmbLocalIP.TabIndex = 2
        '
        'txtLocalPort
        '
        Me.txtLocalPort.Location = New System.Drawing.Point(559, 55)
        Me.txtLocalPort.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLocalPort.Name = "txtLocalPort"
        Me.txtLocalPort.Size = New System.Drawing.Size(81, 23)
        Me.txtLocalPort.TabIndex = 3
        Me.txtLocalPort.Text = "888"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(490, 61)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 17)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "本地端口"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(217, 61)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 17)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "本地地址"
        '
        'txtRemotePort
        '
        Me.txtRemotePort.Location = New System.Drawing.Point(559, 17)
        Me.txtRemotePort.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemotePort.Name = "txtRemotePort"
        Me.txtRemotePort.Size = New System.Drawing.Size(81, 23)
        Me.txtRemotePort.TabIndex = 1
        Me.txtRemotePort.Text = "9001"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(490, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 17)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "远程端口"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(217, 23)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 17)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "远程地址"
        '
        'txtRemoteIP
        '
        Me.txtRemoteIP.Location = New System.Drawing.Point(287, 17)
        Me.txtRemoteIP.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemoteIP.Name = "txtRemoteIP"
        Me.txtRemoteIP.Size = New System.Drawing.Size(163, 23)
        Me.txtRemoteIP.TabIndex = 0
        '
        'butConnect
        '
        Me.butConnect.Location = New System.Drawing.Point(287, 92)
        Me.butConnect.Margin = New System.Windows.Forms.Padding(4)
        Me.butConnect.Name = "butConnect"
        Me.butConnect.Size = New System.Drawing.Size(88, 33)
        Me.butConnect.TabIndex = 4
        Me.butConnect.Text = "连接"
        Me.butConnect.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(814, 282)
        Me.butTCPClientSend.Margin = New System.Windows.Forms.Padding(4)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(88, 33)
        Me.butTCPClientSend.TabIndex = 6
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'txtSendText
        '
        Me.txtSendText.Location = New System.Drawing.Point(108, 244)
        Me.txtSendText.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSendText.Name = "txtSendText"
        Me.txtSendText.Size = New System.Drawing.Size(793, 23)
        Me.txtSendText.TabIndex = 5
        Me.txtSendText.Text = "123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(38, 249)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 17)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "发送内容"
        '
        'butTCPClientSend_200K
        '
        Me.butTCPClientSend_200K.Location = New System.Drawing.Point(108, 282)
        Me.butTCPClientSend_200K.Margin = New System.Windows.Forms.Padding(4)
        Me.butTCPClientSend_200K.Name = "butTCPClientSend_200K"
        Me.butTCPClientSend_200K.Size = New System.Drawing.Size(88, 33)
        Me.butTCPClientSend_200K.TabIndex = 41
        Me.butTCPClientSend_200K.Text = "发送200K"
        Me.butTCPClientSend_200K.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(14, 323)
        Me.txtLog.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(1134, 396)
        Me.txtLog.TabIndex = 42
        '
        'butCloseTCP
        '
        Me.butCloseTCP.Location = New System.Drawing.Point(382, 92)
        Me.butCloseTCP.Margin = New System.Windows.Forms.Padding(4)
        Me.butCloseTCP.Name = "butCloseTCP"
        Me.butCloseTCP.Size = New System.Drawing.Size(88, 33)
        Me.butCloseTCP.TabIndex = 43
        Me.butCloseTCP.Text = "关闭"
        Me.butCloseTCP.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(714, 92)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 33)
        Me.Button1.TabIndex = 44
        Me.Button1.Text = "测试"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkSSL
        '
        Me.chkSSL.AutoSize = True
        Me.chkSSL.Location = New System.Drawing.Point(648, 62)
        Me.chkSSL.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(47, 21)
        Me.chkSSL.TabIndex = 45
        Me.chkSSL.Text = "SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(714, 23)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(88, 33)
        Me.Button2.TabIndex = 46
        Me.Button2.Text = "GC"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butOpenConnectList
        '
        Me.butOpenConnectList.Location = New System.Drawing.Point(289, 160)
        Me.butOpenConnectList.Margin = New System.Windows.Forms.Padding(4)
        Me.butOpenConnectList.Name = "butOpenConnectList"
        Me.butOpenConnectList.Size = New System.Drawing.Size(88, 33)
        Me.butOpenConnectList.TabIndex = 47
        Me.butOpenConnectList.Text = "批量连接"
        Me.butOpenConnectList.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(384, 160)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(88, 33)
        Me.Button3.TabIndex = 48
        Me.Button3.Text = "批量关闭"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(478, 160)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(88, 33)
        Me.Button4.TabIndex = 49
        Me.Button4.Text = "批量发送"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'txtNewConnects
        '
        Me.txtNewConnects.Location = New System.Drawing.Point(200, 163)
        Me.txtNewConnects.Margin = New System.Windows.Forms.Padding(4)
        Me.txtNewConnects.MaxLength = 5
        Me.txtNewConnects.Name = "txtNewConnects"
        Me.txtNewConnects.Size = New System.Drawing.Size(79, 23)
        Me.txtNewConnects.TabIndex = 50
        Me.txtNewConnects.Text = "100"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(130, 169)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 17)
        Me.Label6.TabIndex = 51
        Me.Label6.Text = "连接数量"
        '
        'txtConnected
        '
        Me.txtConnected.Location = New System.Drawing.Point(670, 160)
        Me.txtConnected.Margin = New System.Windows.Forms.Padding(4)
        Me.txtConnected.MaxLength = 0
        Me.txtConnected.Name = "txtConnected"
        Me.txtConnected.Size = New System.Drawing.Size(79, 23)
        Me.txtConnected.TabIndex = 52
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(600, 166)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 17)
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
        Me.chkShowLog.Location = New System.Drawing.Point(14, 292)
        Me.chkShowLog.Margin = New System.Windows.Forms.Padding(4)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(75, 21)
        Me.chkShowLog.TabIndex = 54
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(478, 203)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(88, 33)
        Me.Button5.TabIndex = 55
        Me.Button5.Text = "循环发送"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'butDebugList
        '
        Me.butDebugList.Location = New System.Drawing.Point(756, 160)
        Me.butDebugList.Margin = New System.Windows.Forms.Padding(4)
        Me.butDebugList.Name = "butDebugList"
        Me.butDebugList.Size = New System.Drawing.Size(82, 33)
        Me.butDebugList.TabIndex = 62
        Me.butDebugList.Text = "打印列表"
        Me.butDebugList.UseVisualStyleBackColor = True
        '
        'FrmTCPClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1163, 775)
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
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmTCPClient"
        Me.Text = "TCP Client Test v1.16"
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
End Class
