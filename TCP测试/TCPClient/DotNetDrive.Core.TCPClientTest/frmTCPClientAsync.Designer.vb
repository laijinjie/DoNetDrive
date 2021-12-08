<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTCPClientAsync
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
        Me.txtRemoteIP = New System.Windows.Forms.TextBox()
        Me.txtRemotePort = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.chkShowLog = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.butCloseTCP = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.butTCPClientSend_200K = New System.Windows.Forms.Button()
        Me.txtSendText = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.butConnect = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtRemoteIP
        '
        Me.txtRemoteIP.Location = New System.Drawing.Point(108, 12)
        Me.txtRemoteIP.Name = "txtRemoteIP"
        Me.txtRemoteIP.Size = New System.Drawing.Size(140, 21)
        Me.txtRemoteIP.TabIndex = 37
        Me.txtRemoteIP.Text = "192.168.1.86"
        '
        'txtRemotePort
        '
        Me.txtRemotePort.Location = New System.Drawing.Point(341, 12)
        Me.txtRemotePort.Name = "txtRemotePort"
        Me.txtRemotePort.Size = New System.Drawing.Size(70, 21)
        Me.txtRemotePort.TabIndex = 38
        Me.txtRemotePort.Text = "9001"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(282, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 40
        Me.Label1.Text = "远程端口"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(48, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "远程地址"
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(775, 198)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 23)
        Me.Button6.TabIndex = 74
        Me.Button6.Text = "发送"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(406, 142)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 73
        Me.Button5.Text = "循环发送"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'chkShowLog
        '
        Me.chkShowLog.AutoSize = True
        Me.chkShowLog.Checked = True
        Me.chkShowLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowLog.Location = New System.Drawing.Point(8, 205)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(72, 16)
        Me.chkShowLog.TabIndex = 72
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(608, 15)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 71
        Me.Button2.Text = "GC"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'butCloseTCP
        '
        Me.butCloseTCP.Location = New System.Drawing.Point(323, 64)
        Me.butCloseTCP.Name = "butCloseTCP"
        Me.butCloseTCP.Size = New System.Drawing.Size(75, 23)
        Me.butCloseTCP.TabIndex = 70
        Me.butCloseTCP.Text = "关闭"
        Me.butCloseTCP.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(8, 227)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(973, 281)
        Me.txtLog.TabIndex = 69
        '
        'butTCPClientSend_200K
        '
        Me.butTCPClientSend_200K.Location = New System.Drawing.Point(89, 198)
        Me.butTCPClientSend_200K.Name = "butTCPClientSend_200K"
        Me.butTCPClientSend_200K.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend_200K.TabIndex = 68
        Me.butTCPClientSend_200K.Text = "发送200K"
        Me.butTCPClientSend_200K.UseVisualStyleBackColor = True
        '
        'txtSendText
        '
        Me.txtSendText.Location = New System.Drawing.Point(89, 171)
        Me.txtSendText.Name = "txtSendText"
        Me.txtSendText.Size = New System.Drawing.Size(680, 21)
        Me.txtSendText.TabIndex = 65
        Me.txtSendText.Text = "123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(29, 175)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "发送内容"
        '
        'butConnect
        '
        Me.butConnect.Location = New System.Drawing.Point(242, 64)
        Me.butConnect.Name = "butConnect"
        Me.butConnect.Size = New System.Drawing.Size(75, 23)
        Me.butConnect.TabIndex = 64
        Me.butConnect.Text = "连接"
        Me.butConnect.UseVisualStyleBackColor = True
        '
        'butTCPClientSend
        '
        Me.butTCPClientSend.Location = New System.Drawing.Point(694, 198)
        Me.butTCPClientSend.Name = "butTCPClientSend"
        Me.butTCPClientSend.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend.TabIndex = 66
        Me.butTCPClientSend.Text = "发送"
        Me.butTCPClientSend.UseVisualStyleBackColor = True
        '
        'frmTCPClientAsync
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 520)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.chkShowLog)
        Me.Controls.Add(Me.Button2)
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
        Me.Name = "frmTCPClientAsync"
        Me.Text = "frmTCPClientAsync"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtRemoteIP As TextBox
    Friend WithEvents txtRemotePort As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Button6 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents chkShowLog As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents butCloseTCP As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents butTCPClientSend_200K As Button
    Friend WithEvents txtSendText As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents butConnect As Button
    Friend WithEvents butTCPClientSend As Button
End Class
