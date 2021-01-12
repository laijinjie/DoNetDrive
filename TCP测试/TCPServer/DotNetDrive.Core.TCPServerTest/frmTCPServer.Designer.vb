<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTCPServer
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
        Me.txtWatchPort = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.butCloseTCPServer = New System.Windows.Forms.Button()
        Me.butOpenTCPServer = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.butReloadClientList = New System.Windows.Forms.Button()
        Me.butTCPClientSend_200K = New System.Windows.Forms.Button()
        Me.butCloseTCPClient = New System.Windows.Forms.Button()
        Me.butTCPClientSend = New System.Windows.Forms.Button()
        Me.cmbTCPClient = New System.Windows.Forms.ComboBox()
        Me.txtTCPClientText = New System.Windows.Forms.TextBox()
        Me.cmbLocalIP = New System.Windows.Forms.ComboBox()
        Me.chkSSL = New System.Windows.Forms.CheckBox()
        Me.chkShowLog = New System.Windows.Forms.CheckBox()
        Me.txtConnectCount = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tmrTotal = New System.Windows.Forms.Timer(Me.components)
        Me.txtReadBytes = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnGC = New System.Windows.Forms.Button()
        Me.butDebugList = New System.Windows.Forms.Button()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtWatchPort
        '
        Me.txtWatchPort.Location = New System.Drawing.Point(521, 10)
        Me.txtWatchPort.Name = "txtWatchPort"
        Me.txtWatchPort.Size = New System.Drawing.Size(70, 21)
        Me.txtWatchPort.TabIndex = 27
        Me.txtWatchPort.Text = "9001"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(462, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "监听端口"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(228, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "监听地址"
        '
        'butCloseTCPServer
        '
        Me.butCloseTCPServer.Location = New System.Drawing.Point(415, 46)
        Me.butCloseTCPServer.Name = "butCloseTCPServer"
        Me.butCloseTCPServer.Size = New System.Drawing.Size(189, 27)
        Me.butCloseTCPServer.TabIndex = 23
        Me.butCloseTCPServer.Text = "关闭 TCPServer"
        Me.butCloseTCPServer.UseVisualStyleBackColor = True
        '
        'butOpenTCPServer
        '
        Me.butOpenTCPServer.Location = New System.Drawing.Point(220, 46)
        Me.butOpenTCPServer.Name = "butOpenTCPServer"
        Me.butOpenTCPServer.Size = New System.Drawing.Size(189, 27)
        Me.butOpenTCPServer.TabIndex = 22
        Me.butOpenTCPServer.Text = "打开 TCPServer"
        Me.butOpenTCPServer.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 188)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(815, 282)
        Me.txtLog.TabIndex = 28
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.butReloadClientList)
        Me.GroupBox3.Controls.Add(Me.butTCPClientSend_200K)
        Me.GroupBox3.Controls.Add(Me.butCloseTCPClient)
        Me.GroupBox3.Controls.Add(Me.butTCPClientSend)
        Me.GroupBox3.Controls.Add(Me.cmbTCPClient)
        Me.GroupBox3.Controls.Add(Me.txtTCPClientText)
        Me.GroupBox3.Location = New System.Drawing.Point(115, 78)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(613, 104)
        Me.GroupBox3.TabIndex = 29
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "TCP客户端_被动连接"
        '
        'butReloadClientList
        '
        Me.butReloadClientList.Location = New System.Drawing.Point(370, 73)
        Me.butReloadClientList.Name = "butReloadClientList"
        Me.butReloadClientList.Size = New System.Drawing.Size(75, 23)
        Me.butReloadClientList.TabIndex = 12
        Me.butReloadClientList.Text = "刷新列表"
        Me.butReloadClientList.UseVisualStyleBackColor = True
        '
        'butTCPClientSend_200K
        '
        Me.butTCPClientSend_200K.Location = New System.Drawing.Point(532, 73)
        Me.butTCPClientSend_200K.Name = "butTCPClientSend_200K"
        Me.butTCPClientSend_200K.Size = New System.Drawing.Size(75, 23)
        Me.butTCPClientSend_200K.TabIndex = 11
        Me.butTCPClientSend_200K.Text = "发送200KB"
        Me.butTCPClientSend_200K.UseVisualStyleBackColor = True
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
        Me.butTCPClientSend.Location = New System.Drawing.Point(451, 73)
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
        Me.cmbTCPClient.Size = New System.Drawing.Size(596, 20)
        Me.cmbTCPClient.TabIndex = 6
        '
        'txtTCPClientText
        '
        Me.txtTCPClientText.Location = New System.Drawing.Point(11, 46)
        Me.txtTCPClientText.Name = "txtTCPClientText"
        Me.txtTCPClientText.Size = New System.Drawing.Size(596, 21)
        Me.txtTCPClientText.TabIndex = 7
        Me.txtTCPClientText.Text = "1234"
        '
        'cmbLocalIP
        '
        Me.cmbLocalIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalIP.FormattingEnabled = True
        Me.cmbLocalIP.Location = New System.Drawing.Point(288, 10)
        Me.cmbLocalIP.Name = "cmbLocalIP"
        Me.cmbLocalIP.Size = New System.Drawing.Size(140, 20)
        Me.cmbLocalIP.TabIndex = 30
        '
        'chkSSL
        '
        Me.chkSSL.AutoSize = True
        Me.chkSSL.Location = New System.Drawing.Point(610, 52)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(42, 16)
        Me.chkSSL.TabIndex = 31
        Me.chkSSL.Text = "SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'chkShowLog
        '
        Me.chkShowLog.AutoSize = True
        Me.chkShowLog.Location = New System.Drawing.Point(12, 166)
        Me.chkShowLog.Name = "chkShowLog"
        Me.chkShowLog.Size = New System.Drawing.Size(72, 16)
        Me.chkShowLog.TabIndex = 55
        Me.chkShowLog.Text = "显示日志"
        Me.chkShowLog.UseVisualStyleBackColor = True
        '
        'txtConnectCount
        '
        Me.txtConnectCount.Location = New System.Drawing.Point(757, 10)
        Me.txtConnectCount.Name = "txtConnectCount"
        Me.txtConnectCount.Size = New System.Drawing.Size(70, 21)
        Me.txtConnectCount.TabIndex = 57
        Me.txtConnectCount.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(686, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 56
        Me.Label1.Text = "已连接数："
        '
        'tmrTotal
        '
        Me.tmrTotal.Enabled = True
        Me.tmrTotal.Interval = 500
        '
        'txtReadBytes
        '
        Me.txtReadBytes.Location = New System.Drawing.Point(757, 38)
        Me.txtReadBytes.Name = "txtReadBytes"
        Me.txtReadBytes.Size = New System.Drawing.Size(70, 21)
        Me.txtReadBytes.TabIndex = 59
        Me.txtReadBytes.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(658, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 12)
        Me.Label2.TabIndex = 58
        Me.Label2.Text = "已收到字节数："
        '
        'btnGC
        '
        Me.btnGC.Location = New System.Drawing.Point(757, 65)
        Me.btnGC.Name = "btnGC"
        Me.btnGC.Size = New System.Drawing.Size(70, 23)
        Me.btnGC.TabIndex = 60
        Me.btnGC.Text = "GC"
        Me.btnGC.UseVisualStyleBackColor = True
        '
        'butDebugList
        '
        Me.butDebugList.Location = New System.Drawing.Point(757, 94)
        Me.butDebugList.Name = "butDebugList"
        Me.butDebugList.Size = New System.Drawing.Size(70, 23)
        Me.butDebugList.TabIndex = 61
        Me.butDebugList.Text = "打印列表"
        Me.butDebugList.UseVisualStyleBackColor = True
        '
        'frmTCPServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(839, 482)
        Me.Controls.Add(Me.butDebugList)
        Me.Controls.Add(Me.btnGC)
        Me.Controls.Add(Me.txtReadBytes)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtConnectCount)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkShowLog)
        Me.Controls.Add(Me.chkSSL)
        Me.Controls.Add(Me.cmbLocalIP)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.txtWatchPort)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.butCloseTCPServer)
        Me.Controls.Add(Me.butOpenTCPServer)
        Me.Name = "frmTCPServer"
        Me.Text = "frmTCPServer"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtWatchPort As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents butCloseTCPServer As Button
    Friend WithEvents butOpenTCPServer As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents butCloseTCPClient As Button
    Friend WithEvents butTCPClientSend As Button
    Friend WithEvents cmbTCPClient As ComboBox
    Friend WithEvents txtTCPClientText As TextBox
    Friend WithEvents cmbLocalIP As ComboBox
    Friend WithEvents butTCPClientSend_200K As Button
    Friend WithEvents chkSSL As CheckBox
    Friend WithEvents chkShowLog As CheckBox
    Friend WithEvents butReloadClientList As Button
    Friend WithEvents txtConnectCount As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents tmrTotal As Timer
    Friend WithEvents txtReadBytes As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnGC As Button
    Friend WithEvents butDebugList As Button
End Class
