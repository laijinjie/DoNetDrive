Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Command
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Extension
Imports DoNetDrive.Core
Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Collections.Concurrent


Public Class frmTCPServer


    Private WithEvents Allocator As ConnectorAllocator
    ''' <summary>
    ''' 连接通道观察者，检查数据收发
    ''' </summary>
    Private WithEvents obServer As TCPIOObserverHandler

    Private mShowLog As Boolean
    Private mReadBytes As Long



    ''' <summary>
    ''' 连接通道观察者，可以观察连接通道上的数据收发 十六进制格式输出
    ''' </summary>
    Public Class TCPIOObserverHandler
        Implements INRequestHandle

        Private Const MsgDebugLen = 40
        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msgLen"></param>
        Event DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As String)
        ''' <summary>
        ''' 准备发送数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msgLen"></param>
        Event DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As String)

        Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
            Dim sHex As String
            Dim iLen = msg.ReadableBytes

            If iLen > MsgDebugLen Then
                iLen = MsgDebugLen
            End If
            sHex = ByteBufferUtil.HexDump(msg, 0, iLen)


            RaiseEvent DisposeRequestEvent(connector, msg.ReadableBytes, sHex)

        End Sub

        Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
            Dim sHex As String
            Dim iLen = msg.ReadableBytes
            If iLen > MsgDebugLen Then
                iLen = MsgDebugLen
            End If
            sHex = ByteBufferUtil.HexDump(msg, 0, iLen)

            RaiseEvent DisposeResponseEvent(connector, msg.ReadableBytes, sHex)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Return
        End Sub
    End Class


    Private Sub butTCPServer_Click(sender As Object, e As EventArgs) Handles butOpenTCPServer.Click
        Dim sAddr As String = cmbLocalIP.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As TCPServer.TCPServerDetail

        If chkSSL.Checked Then
            Dim sSSLFile = Path.Combine(Application.StartupPath, "SSLX509.pfx")
            If Not File.Exists(sSSLFile) Then
                MsgBox("证书不存在！", 16, "错误")
                Return
            End If
            Dim x509Data = File.ReadAllBytes(sSSLFile)
            Dim x509 As X509Certificate2 = New X509Certificate2(x509Data, "BRUsqOWH")
            serverDtl = New TCPServer.TCPServerDetail(sAddr, sPort, True, x509)
        Else
            serverDtl = New TCPServer.TCPServerDetail(sAddr, sPort)
        End If

        Allocator.OpenConnector(serverDtl)

    End Sub


    Private Sub butCloseTCPServer_Click(sender As Object, e As EventArgs) Handles butCloseTCPServer.Click
        Dim sAddr As String = cmbLocalIP.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As TCPServer.TCPServerDetail = New TCPServer.TCPServerDetail(sAddr, sPort)
        Allocator.CloseConnector(serverDtl)
    End Sub


    Private Sub frmTCPServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyTraceListener.Ini()
        Allocator = ConnectorAllocator.GetAllocator()
        AbstractConnector.DefaultChannelKeepaliveMaxTime = 10

        obServer = New TCPIOObserverHandler()
        InITCPServerClient()
        IniLoadLocalIP()

        mShowLog = chkShowLog.Checked
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
        If (cmbLocalIP.Items.Count > 0) Then
            cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1
        End If
    End Sub


    Private Sub Allocator_ClientOffline(sender As Object, e As ServerEventArgs) Handles Allocator.ClientOffline
        Dim inc = TryCast(sender, INConnector)

        If inc.GetConnectorType = ConnectorType.TCPServerClient Then
            RemoveTCPClientItem(inc.GetConnectorDetail())
        End If

        AddLog($"连接断开  {GetConnectorDetail(inc)}")
    End Sub

    Private Sub Allocator_ClientOnline(sender As Object, e As ServerEventArgs) Handles Allocator.ClientOnline
        Dim inc = TryCast(sender, INConnector)
        inc.AddRequestHandle(obServer)

        If inc.GetConnectorType = ConnectorType.TCPServerClient Then
            AddTCPClientItem(inc.GetConnectorDetail())
        End If

        AddLog($"连接建立  {GetConnectorDetail(inc)}")
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
        Allocator.GetConnector(connector).AddRequestHandle(obServer)
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
                Dim dtl As TCPServer.Client.TCPServerClientDetail = oConn.GetConnectorDetail()

                Return $"R:{dtl.Remote.ToString()} T:{Now.ToTimeffff()}"
            Case ConnectorType.TCPServer
                Dim local = oConn.LocalAddress
                Return $"Server  本地绑定IP：{local.Addr}:{local.Port} :{Now.ToTimeffff()}"
        End Select
        Return conn.ToString()
    End Function

    Private Sub obServer_DisposeRequestEvent(connector As INConnector, msgLen As Integer, msgHex As String) Handles obServer.DisposeRequestEvent
        mReadBytes += msgLen
        AddLog($"{GetConnectorDetail(connector)} 接收 长度：{msgLen}  0x{msgHex}")
    End Sub

    Private Sub obServer_DisposeResponseEvent(connector As INConnector, msgLen As Integer, msgHex As String) Handles obServer.DisposeResponseEvent
        'If mUseEcho Then
        '    Return
        'End If
        AddLog($"{GetConnectorDetail(connector)} 发送 长度：{msgLen}  0x{msgHex}")
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

        Public Sub New(ByVal dtl As TCPServer.Client.TCPServerClientDetail)
            Remote = New IPDetail(dtl.Remote.Addr, dtl.Remote.Port)
            Local = New IPDetail(dtl.Local.Addr, dtl.Local.Port)
            ClientID = dtl.ClientID
            Key = dtl.Key
        End Sub

        Public Overrides Function ToString() As String
            Return $"本地：{Local.Addr}:{Local.Port} -- 远程:{Remote.Addr}:{Remote.Port} ,ID:{ClientID}"
        End Function
    End Class

    Private Sub RemoveTCPClientItem(dtl As INConnectorDetail)
        Dim oItem As TCPServerClientDetail_Item = Nothing
        Dim sKey As String = dtl.GetKey()
        Trace.WriteLine($"删除通道 {sKey} ")
        If Not TCPServerClients.TryRemove(sKey, oItem) Then
            Return
        End If
    End Sub

    Private Sub AddTCPClientItem(dtl As INConnectorDetail)
        Dim sKey As String = dtl.GetKey()
        Dim oClient As TCPServer.Client.TCPServerClientDetail = dtl
        Dim oItem = New TCPServerClientDetail_Item(oClient)
        Trace.WriteLine($"添加通道 {sKey} ")
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

        Dim connDtl = New TCPServer.Client.TCPServerClientDetail(sKey)

        Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(connDtl),
                                                New Command.Text.TextCommandParameter(txtTCPClientText.Text, System.Text.Encoding.Default))
        Allocator.AddCommand(txt)
    End Sub

    Private Sub butCloseTCPClient_Click(sender As Object, e As EventArgs) Handles butCloseTCPClient.Click
        Dim oItem = TryCast(cmbTCPClient.SelectedItem, TCPServerClientDetail_Item)
        If oItem Is Nothing Then Return
        Dim sKey As String = oItem.Key

        Dim connDtl = New TCPServer.Client.TCPServerClientDetail(sKey)
        Allocator.CloseConnector(connDtl)
    End Sub

    Private Sub butTCPClientSend_200K_Click(sender As Object, e As EventArgs) Handles butTCPClientSend_200K.Click
        Dim oItem = TryCast(cmbTCPClient.SelectedItem, TCPServerClientDetail_Item)
        If oItem Is Nothing Then Return
        Dim sKey As String = oItem.Key
        Dim ilen = 200 * 1024
        Dim buf = UnpooledByteBufferAllocator.Default.Buffer(ilen + 16)
        buf.WriteString("DDDDDDDDDD", System.Text.Encoding.ASCII)
        buf.WriteInt(ilen)

        For i = 1 To ilen
            buf.WriteByte(i Mod 255)
        Next
        Dim crc32 = MyCRC32.CalculateDigest(buf.Array, buf.ArrayOffset, ilen + 14)
        buf.WriteInt(UInt32ToInt32(crc32))
        AddLog($"CRC32:{crc32:X}")

        Dim connDtl = New TCPServer.Client.TCPServerClientDetail(sKey)

        Dim txt As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(connDtl),
                                                New Command.Byte.ByteCommandParameter(buf))
        Allocator.AddCommand(txt)
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
            sBuf.AppendLine(s)
        Next

        txtLog.Text = sBuf.ToString()
    End Sub
End Class