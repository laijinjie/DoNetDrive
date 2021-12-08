Imports System.Collections.Concurrent
Imports System.Configuration
Imports DoNetDrive.Core
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers

Public Class frmTCPClientAsync
    Private mIsUnload As Boolean

    Private Allocator As ConnectorAllocator
    ''' <summary>
    ''' 连接通道观察者，检查数据收发
    ''' </summary>
    Private WithEvents obServer As TCPIOObserverHandler
    Private mConnect As INConnector


    Private Sub frmTCPClientAsync_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Allocator = ConnectorAllocator.GetAllocator()
        obServer = New TCPIOObserverHandler()

        LoadSetting()
        mIsUnload = False
        mShowLog = chkShowLog.Checked
    End Sub

    Private Sub frmTCPServer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        SaveSetting()
        mIsUnload = True
        Allocator.Dispose()
    End Sub

    Private Sub LoadSetting()


        txtRemoteIP.Text = ConfigurationManager.AppSettings("ServerIP")
        txtRemotePort.Text = ConfigurationManager.AppSettings("ServerPort")
        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("chkShowLog")) Then
            chkShowLog.Checked = CBool(ConfigurationManager.AppSettings("chkShowLog"))
        End If

    End Sub


    Private Sub SaveSetting()
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim kv = config.AppSettings.Settings
        SetConfigKey(config, "ServerIP", txtRemoteIP.Text)
        SetConfigKey(config, "ServerPort", txtRemotePort.Text)
        SetConfigKey(config, "chkShowLog", chkShowLog.Checked)
        config.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("appSettings")
    End Sub

    Private Sub SetConfigKey(config As Configuration, key As String, sValue As String)
        Dim kv = config.AppSettings.Settings
        If kv(key) Is Nothing Then
            kv.Add(key, sValue)
        Else
            kv(key).Value = sValue
        End If
    End Sub

    Private Sub SelectComboxItem(cmb As ComboBox, sValue As String)
        For Each sItem As String In cmb.Items
            If sItem = sValue Then
                cmb.SelectedItem = sItem
                Return
            End If
        Next
    End Sub


    Private Function GetConnectorDetail(conn As INConnector) As String
        Return GetConnectorDetail(conn.GetConnectorDetail())
    End Function

    Private Function GetConnectorDetail(conn As INConnectorDetail) As String
        If mIsUnload Then Return String.Empty
        Dim oConn = Allocator.GetConnector(conn)
        If oConn Is Nothing Then Return conn?.ToString()

        Select Case conn.GetTypeName
            Case ConnectorType.TCPClient
                Dim dtl As TCPClient.TCPClientDetail = oConn.GetConnectorDetail()

                Return $"R:{dtl.Addr}:{dtl.Port} L:{dtl.LocalAddr}:L:{dtl.LocalPort} T:{Now:ss.ffff}"
        End Select
        Return conn.ToString()
    End Function

    Private Sub obServer_DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As String) Handles obServer.DisposeRequestEvent
        If mIsUnload Then Return
        AddLog($"{GetConnectorDetail(connector)} 接收  长度：{msgLen}  {msg}")
    End Sub

    Private Sub obServer_DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As String) Handles obServer.DisposeResponseEvent
        If mIsUnload Then Return
        AddLog($"{GetConnectorDetail(connector)} 发送 长度：{msgLen}  {msg}")
    End Sub



    Private Sub AddLog(ByVal sTxt As String)
        If mIsUnload Then Return
        If Not mShowLog Then Return
        Try
            If txtLog.InvokeRequired Then
                Invoke(New Action(Of String)(AddressOf AddLog), sTxt)
                Return
            End If
            txtLog.AppendText(sTxt + vbNewLine)
        Catch ex As Exception

        End Try


    End Sub

    Private Sub txtLog_DoubleClick(sender As Object, e As EventArgs) Handles txtLog.DoubleClick
        txtLog.Text = String.Empty
    End Sub

    Private Async Sub butConnect_Click(sender As Object, e As EventArgs) Handles butConnect.Click
        Dim oTCPDTL = GetTCPClientDetail()
        If mConnect IsNot Nothing Then
            Await CloseAsync()
        End If
        Try
            mConnect = Await Allocator.OpenConnectorAsync(oTCPDTL)
            mConnect.AddRequestHandle(obServer)
            AddLog($"已连接成功 {mConnect.GetKey} {GetConnectorDetail(mConnect)} ")
        Catch ex As Exception
            AddLog($"连接发生错误！  {GetConnectorDetail(oTCPDTL)} {ex.Message} ")
        End Try

    End Sub

    Private Async Sub butCloseTCP_Click(sender As Object, e As EventArgs) Handles butCloseTCP.Click
        If mConnect Is Nothing Then Return

        Await CloseAsync()
        AddLog($"连接已关闭")
    End Sub

    Private Async Function CloseAsync() As Task
        Await mConnect.CloseAsync()
        mConnect = Nothing
    End Function


    Private Function GetTCPClientDetail() As TCPClient.TCPClientDetail
        Dim sRemoteIP, sLocalIP As String
        Dim iRemotePort, iLocalPort As Integer
        sRemoteIP = txtRemoteIP.Text
        sLocalIP = String.Empty
        iRemotePort = Integer.Parse(txtRemotePort.Text)
        iLocalPort = 0

        Return New TCPClient.TCPClientDetail(sRemoteIP, iRemotePort, sLocalIP, iLocalPort)

    End Function

    Private Async Sub butTCPClientSend_Click(sender As Object, e As EventArgs) Handles butTCPClientSend.Click
        If String.IsNullOrEmpty(txtSendText.Text) Then
            Return
        End If
        If mConnect Is Nothing Then Return

        Dim oTCPDTL = GetTCPClientDetail()
        Dim par = New Command.Text.TextCommandParameter(txtSendText.Text,
                                                        System.Text.Encoding.UTF8)
        Dim cmd = New Command.Text.TextCommand(New Text.TextCommandDetail(oTCPDTL), par)
        Try
            Await mConnect.RunCommandAsync(cmd)
        Catch ex As Exception

        End Try


        Dim result = cmd.getResult()
        AddLog($"命令完成")
    End Sub


    Private Async Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If String.IsNullOrEmpty(txtSendText.Text) Then
            Return
        End If
        Dim oTCPDTL = GetTCPClientDetail()
        Dim par = New Command.Text.TextCommandParameter(txtSendText.Text,
                                                        System.Text.Encoding.UTF8)
        Dim cmd As INCommand
        For index = 1 To 100
            cmd = New Command.Text.TextCommand(New Text.TextCommandDetail(oTCPDTL), par)
            Allocator.AddCommandAsync(cmd)
        Next


        cmd = New Command.Text.TextCommand(New Text.TextCommandDetail(oTCPDTL), par)
        Await Allocator.AddCommandAsync(cmd)

        AddLog($"命令完成")
    End Sub

    Private Sub butTCPClientSend_200K_Click(sender As Object, e As EventArgs) Handles butTCPClientSend_200K.Click
        Dim ilen = 200 * 1024
        Dim buf = UnpooledByteBufferAllocator.Default.Buffer(ilen + 16)
        'buf.WriteString("DDDDDDDDDD", System.Text.Encoding.ASCII)
        'buf.WriteInt(ilen)
        Dim rd = New Random

        For i = 1 To ilen
            buf.WriteByte(rd.Next(33, 122))
        Next
        'Dim crc32 = MyCRC32.CalculateDigest(buf.Array, buf.ArrayOffset, ilen)
        'buf.WriteInt(UInt32ToInt32(crc32))
        'AddLog($"CRC32:{crc32:X}")
        Dim oTCPDTL = GetTCPClientDetail()

        Dim cmd As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(oTCPDTL),
                                                New Command.Byte.ByteCommandParameter(buf))
        Allocator.AddCommand(cmd)
    End Sub

    ''' <summary>
    ''' Int64的最大值
    ''' </summary>
    Private _Int32Max As UInt32 = Integer.MaxValue
    ''' <summary>
    ''' 取低4位的掩码
    ''' </summary>
    Private _UInt32Mask As UInt32 = 15 '0x0F

    ''' <summary>
    ''' 将UInt32 转换为 Int32
    ''' </summary>
    ''' <param name="u">UInt64</param>
    ''' <returns>Int64</returns>
    Private Function UInt32ToInt32(ByVal u As UInt32) As Integer
        Dim l As Integer = 0
        If u > _Int32Max Then
            l = u >> 4
            l = l << 4
            u = u And _UInt32Mask
            l = l Xor u
        Else
            l = u
        End If
        Return l
    End Function


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GC.Collect()
    End Sub




    Private mShowLog As Boolean

    Private Sub chkShowLog_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowLog.CheckedChanged
        mShowLog = chkShowLog.Checked
    End Sub



End Class