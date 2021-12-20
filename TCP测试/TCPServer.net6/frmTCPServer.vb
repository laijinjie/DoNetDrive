Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Command
Imports DotNetty.Buffers
Imports DoNetDrive.Common.Extensions
Imports DoNetDrive.Core
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Collections.Concurrent
Imports System.Text


Public Class frmTCPServer


    Private WithEvents Allocator As ConnectorAllocator

    Private mShowLog As Boolean
    Private mReadBytes As Long
    Private mHexLog As Boolean
    Private mAutoEcho As Boolean

    Private Sub butTCPServer_Click(sender As Object, e As EventArgs) Handles butOpenTCPServer.Click
        Dim sAddr As String = cmbLocalIP.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As Connector.TCPServer.TCPServerDetail

        If chkSSL.Checked Then
            Dim sSSLFile = Path.Combine(Application.StartupPath, "SSLX509.pfx")
            If Not File.Exists(sSSLFile) Then
                MsgBox("证书不存在！", 16, "错误")
                Return
            End If
            Dim x509Data = File.ReadAllBytes(sSSLFile)
            Dim x509 As X509Certificate2 = New X509Certificate2(x509Data, "BRUsqOWH")
            serverDtl = New Connector.TCPServer.TCPServerDetail(sAddr, sPort, True, x509)
        Else
            serverDtl = New Connector.TCPServer.TCPServerDetail(sAddr, sPort)
        End If
        serverDtl.ClientOnlineCallBlack = AddressOf ClientOnlineCallblack
        serverDtl.ClientOfflineCallBlack = AddressOf ClientOfflineCallblack
        Allocator.OpenConnector(serverDtl)

    End Sub


    Private Sub butCloseTCPServer_Click(sender As Object, e As EventArgs) Handles butCloseTCPServer.Click
        Dim sAddr As String = cmbLocalIP.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As Connector.TCPServer.TCPServerDetail = New Connector.TCPServer.TCPServerDetail(sAddr, sPort)
        Allocator.CloseConnector(serverDtl)
    End Sub


    Private Sub frmTCPServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Allocator = ConnectorAllocator.GetAllocator()
        AbstractConnector.DefaultChannelKeepaliveMaxTime = 10


        InITCPServerClient()
        IniLoadLocalIP()
        mHexLog = chkHex.Checked

        mShowLog = chkShowLog.Checked
        mAutoEcho = chkAutoEcho.Checked
    End Sub

    Private Sub IniLoadLocalIP()
        cmbLocalIP.Items.Clear()
        Dim localentry = Dns.GetHostEntry(Dns.GetHostName())
        For Each ip As IPAddress In localentry.AddressList
            If ip.IsIPv4MappedToIPv6 Then

                ip = ip.MapToIPv4()
            End If
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                cmbLocalIP.Items.Add(ip.ToString())
            End If
        Next
        'If (cmbLocalIP.Items.Count > 0) Then
        '    cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1
        'End If
    End Sub


    Private Sub ClientOfflineCallblack(client As INConnector)

        If client.GetConnectorType = ConnectorType.TCPServerClient Then
            RemoveTCPClientItem(client.GetConnectorDetail())
        End If

        AddLog($"客户端离线  {GetConnectorDetail(client)}")
    End Sub

    Private Sub ClientOnlineCallblack(client As INConnector)

        client.AddRequestHandle(New TCPIOObserverHandler() With {
                             .ReadDataCallblack = AddressOf obServer_DisposeRequestEvent,
                             .SendDataCallbalck = AddressOf obServer_DisposeResponseEvent
                             })

        If client.GetConnectorType = ConnectorType.TCPServerClient Then
            AddTCPClientItem(client.GetConnectorDetail())
        End If

        AddLog($"客户端接入  {GetConnectorDetail(client)}")
    End Sub


    Private Sub Allocator_CommandCompleteEvent(sender As Object, e As CommandEventArgs) Handles Allocator.CommandCompleteEvent

        Dim dtl = e.CommandDetail
        Dim lSec = (dtl.EndTime - dtl.BeginTime).TotalMilliseconds()

        AddLog($"命令完成，耗时：{lSec:0.0} 毫秒")

    End Sub

    Private Sub Allocator_ConnectorClosedEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorClosedEvent
        AddLog($"连接关闭  {GetConnectorDetail(connector)} ")
    End Sub

    Private Sub Allocator_ConnectorConnectedEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorConnectedEvent
        AddLog($"连接成功  {GetConnectorDetail(connector)} ")
    End Sub

    Private Sub Allocator_ConnectorErrorEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorErrorEvent
        AddLog($"连接发生错误！  {GetConnectorDetail(connector)} ")
    End Sub

    Private Function GetConnectorDetail(conn As INConnector) As String
        Return GetConnectorDetail(conn.GetConnectorDetail())
    End Function

    Private Function GetConnectorDetail(conn As INConnectorDetail) As String
        Dim oConn = Allocator.GetConnector(conn)
        If oConn Is Nothing Then Return conn?.ToString()

        Select Case conn.GetTypeName
            Case ConnectorType.TCPServerClient
                Dim dtl As Connector.TCPServer.Client.TCPServerClientDetail = oConn.GetConnectorDetail()

                Return $"R:{dtl.Addr}:{dtl.Port} T:{Now.ToTimeffff()}"
            Case ConnectorType.TCPServer
                Dim local = oConn.LocalAddress
                Return $"Server  本地绑定IP：{local.Addr}:{local.Port} :{Now.ToTimeffff()}"
        End Select
        Return conn.ToString()
    End Function

    ''' <summary>
    ''' 处理接收
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    ''' <param name="msg"></param>
    Private Sub obServer_DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As IByteBuffer)
        mReadBytes += msgLen
        If mAutoEcho Then

            Dim buf = Unpooled.CopiedBuffer(msg)
            Dim txt As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(connector.GetConnectorDetail),
                 New Command.Byte.ByteCommandParameter(buf))
            connector.AddCommand(txt)
        Else
            If mShowLog Then
                Dim sHex As String
                Dim iLen = msg.ReadableBytes

                If mHexLog Then
                    sHex = ByteBufferUtil.HexDump(msg)
                Else
                    sHex = Encoding.UTF8.GetString(msg.Array, msg.ArrayOffset, msgLen)
                End If
                AddLog($"{GetConnectorDetail(connector)} 接收 长度：{msgLen}  {sHex}")
            End If

        End If


    End Sub
    ''' <summary>
    ''' 处理发送
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    ''' <param name="msg"></param>
    Private Sub obServer_DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As IByteBuffer)


        If mShowLog Then
            Dim sHex As String
            Dim iLen = msg.ReadableBytes

            If mHexLog Then
                sHex = ByteBufferUtil.HexDump(msg)
            Else
                sHex = Encoding.UTF8.GetString(msg.Array, msg.ArrayOffset, msgLen)
            End If
            AddLog($"{GetConnectorDetail(connector)} 发送 长度：{msgLen}  {sHex}")
        End If

    End Sub


    Private Sub AddLog(ByVal sTxt As String)
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

#Region "TCP Server"

    Private TCPServerClients As ConcurrentDictionary(Of String, TCPServerClientDetail_Item)
    Private Sub InITCPServerClient()
        TCPServerClients = New ConcurrentDictionary(Of String, TCPServerClientDetail_Item)
    End Sub


    Private Class TCPServerClientDetail_Item
        Public Remote As IPDetail
        Public Local As IPDetail
        Public Key As String
        Public ClientID As Long

        Public Sub New(ByVal dtl As Connector.TCPServer.Client.TCPServerClientDetail)
            Remote = New IPDetail(dtl.Addr, dtl.Port)
            Local = New IPDetail(dtl.LocalAddr, dtl.LocalPort)
            ClientID = dtl.ClientID
            Key = dtl.GetKey
        End Sub

        Public Overrides Function ToString() As String
            Return $"本地：{Local.Addr}:{Local.Port} -- 远程:{Remote.Addr}:{Remote.Port} ,ID:{ClientID}"
        End Function
    End Class

    Private Sub RemoveTCPClientItem(dtl As INConnectorDetail)
        Dim oItem As TCPServerClientDetail_Item = Nothing
        Dim sKey As String = dtl.GetKey()
        'Trace.WriteLine($"删除通道 {sKey} ")
        If Not TCPServerClients.TryRemove(sKey, oItem) Then
            Return
        End If
    End Sub

    Private Sub AddTCPClientItem(dtl As INConnectorDetail)
        Dim sKey As String = dtl.GetKey()
        Dim oClient As Connector.TCPServer.Client.TCPServerClientDetail = dtl
        Dim oItem = New TCPServerClientDetail_Item(oClient)
        'Trace.WriteLine($"添加通道 {sKey} ")
        TCPServerClients.TryAdd(sKey, oItem)
    End Sub


    Private Sub butReloadClientList_Click(sender As Object, e As EventArgs) Handles butReloadClientList.Click
        Dim sKeys = TCPServerClients.Keys.ToArray()
        cmbTCPClient.Items.Clear()
        cmbTCPClient.BeginUpdate()

        For Each sKey As String In sKeys
            Dim oItem As TCPServerClientDetail_Item = Nothing
            If TCPServerClients.TryGetValue(sKey, oItem) Then
                cmbTCPClient.Items.Add(oItem)
            End If
        Next
        If (sKeys.Count > 0) Then
            cmbTCPClient.SelectedIndex = 0

        End If
        cmbTCPClient.EndUpdate()
    End Sub


    Private Sub butTCPClientSend_Click(sender As Object, e As EventArgs) Handles butTCPClientSend.Click
        Dim oItem = TryCast(cmbTCPClient.SelectedItem, TCPServerClientDetail_Item)
        If oItem Is Nothing Then Return
        Dim sKey As String = oItem.Key


        Dim s As String = txtTCPClientText.Text
        If String.IsNullOrEmpty(s) Then
            Return
        End If

        Dim connDtl = New Connector.TCPServer.Client.TCPServerClientDetail(sKey)

        If mHexLog Then
            If Not s.IsHex Then
                MsgBox("请输入十六进制字符", 16, "错误")
                Return
            End If
            Dim buf = Unpooled.WrappedBuffer(s.HexToByte)
            Dim txt As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(connDtl),
                 New Command.Byte.ByteCommandParameter(buf))
            Allocator.AddCommand(txt)
        Else

            Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(connDtl),
                 New Command.Text.TextCommandParameter(s, Encoding.UTF8))
            Allocator.AddCommand(txt)
        End If

    End Sub

    Private Sub butCloseTCPClient_Click(sender As Object, e As EventArgs) Handles butCloseTCPClient.Click
        Dim oItem = TryCast(cmbTCPClient.SelectedItem, TCPServerClientDetail_Item)
        If oItem Is Nothing Then Return
        Dim sKey As String = oItem.Key

        Dim connDtl = New Connector.TCPServer.Client.TCPServerClientDetail(sKey)

        Allocator.CloseConnector(connDtl)
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
#End Region

    Private Sub chkShowLog_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowLog.CheckedChanged
        mShowLog = chkShowLog.Checked
    End Sub

    Private Sub tmrTotal_Tick(sender As Object, e As EventArgs) Handles tmrTotal.Tick
        txtConnectCount.Text = TCPServerClients.Count
        txtReadBytes.Text = mReadBytes
    End Sub

    Private Sub btnGC_Click(sender As Object, e As EventArgs) Handles btnGC.Click
        GC.Collect()
    End Sub

    Private Sub butDebugList_Click(sender As Object, e As EventArgs) Handles butDebugList.Click
        Dim oKeys = Allocator.GetAllConnectorKeys()
        Dim sBuf = New System.Text.StringBuilder()
        For Each s In oKeys
            Dim oConn = Allocator.GetConnector(s)
            sBuf.AppendLine($"{s}  -- R:{oConn.ReadTotalBytes} W:{oConn.SendTotalBytes} C:{oConn.GetCommandCount} T:{oConn.CommandTotal}")
        Next

        txtLog.Text = sBuf.ToString()
    End Sub

    Private Sub chkHex_CheckedChanged(sender As Object, e As EventArgs) Handles chkHex.CheckedChanged
        mHexLog = chkHex.Checked
    End Sub

    Private Sub chkAutoEcho_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoEcho.CheckedChanged
        mAutoEcho = chkAutoEcho.Checked
    End Sub

    Private Sub btnCloseAll_Click(sender As Object, e As EventArgs) Handles btnCloseAll.Click


        If TCPServerClients.Count = 0 Then Return

        Dim sKeys = TCPServerClients.Keys.ToArray()


        For Each sKey As String In sKeys
            Dim connDtl = New Connector.TCPServer.Client.TCPServerClientDetail(sKey)
            Allocator.CloseConnector(connDtl)
        Next


    End Sub
End Class