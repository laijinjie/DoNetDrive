Imports DoNetDrive.Protocol.OnlineAccess
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Command
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Extension
Imports DoNetDrive.Core.Packet
Imports System.Threading.Tasks
Imports DoNetDrive.Core.Connector.TCPClient
Imports DoNetDrive.Core
Imports DoNetDrive.Protocol
Imports DoNetDrive.Core.Data
Imports System.Security.Cryptography.X509Certificates
Imports DoNetDrive.Core.Connector.WebSocket.Server

Public Class Form1



    Private WithEvents Allocator As ConnectorAllocator
    ''' <summary>
    ''' 连接通道观察者，检查数据收发
    ''' </summary>
    Private WithEvents obServer As ConnectorObserverHandler

    Private Class ReadSN
        Inherits DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.ReadSN

        Public CmdNum As Integer


        Public Sub New(dtl As INCommandDetail, num As Integer)
            MyBase.New(dtl)
            CmdNum = num
        End Sub

    End Class

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        For i = 1 To 10
            Dim cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.TCPClient, "192.168.1.86", 9687,
                                                        CommandDetailFactory.ControllerType.Door88, "FC-8940H09030001", "FFFFFFFF")

            Dim Cmd = New ReadSN(cmdDtl, i + 1)
            Allocator.AddCommand(Cmd)
        Next

    End Sub


    Private Sub TestSub()
        Dim p As OnlineAccessPacket
        Dim c As INConnectorDetail = New TCPClient.TCPClientDetail("192.168.1.30", 8000)
        Dim d As OnlineAccessCommandDetail = New OnlineAccessCommandDetail(c, "8940H09030001", "FFFFFFFF")
        'Dim acr = PooledByteBufferAllocator.Default
        Dim acr = UnpooledByteBufferAllocator.Default
        Dim i As Long
        Dim buf As IByteBuffer
        Dim dec As OnlineAccessDecompile
        Dim b = "7EBAAE54C346432D38393430483039303330303031FFFFFFFF310400000000046006FFFF787E".HexToByte
        Dim databuf = "4643617264597A".HexToByte()



        buf = acr.Buffer(7)
        buf.WriteBytes(databuf)
        p = New OnlineAccessPacket(d, 1, 4, 0, 7, buf)
        buf = p.GetPacketData(acr)
        p.Dispose()


        buf.Clear()


        buf.WriteBytes(b)
        dec = New OnlineAccessDecompile(acr)
        Dim oPacket As List(Of INPacket) = New List(Of INPacket)

        If dec.Decompile(buf, oPacket) Then
            For Each p In oPacket
                Debug.Print(p.SN.GetString())
                Debug.Print(p.Password.ToHex())
                Debug.Print(p.DataLen)
                If p.DataLen > 0 Then
                    Debug.Print(p.CmdData.ToHex())
                End If
                p.Dispose()
            Next
        End If
        buf.Release()
        dec.Dispose()


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectorAllocator.DefaultEventLoopGroupTaskCount = 1
        DotNettyAllocator.DefaultChildEventLoopGroupCount = 1
        DotNettyAllocator.DefaultServerEventLoopGroupCount = 1

        Allocator = ConnectorAllocator.GetAllocator()
        obServer = New ConnectorObserverHandler()
        obServer.UseEcho = False
        obServer.HexDump = True

        InIUDP()
        InITCPServerClient()
        'Environment.SetEnvironmentVariable("io.netty.allocator.numDirectArenas", "0")
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub Allocator_ClientOffline(sender As Object, e As ServerEventArgs) Handles Allocator.ClientOffline
        Dim inc = TryCast(sender, INConnector)

        If inc.GetConnectorType = ConnectorType.TCPServerClient Then
            RemoveTCPClientItem(inc.GetConnectorDetail())
        End If

        If inc.GetConnectorType = ConnectorType.UDPClient Then
            RemoveUDPItem(inc.GetConnectorDetail())
        End If

        AddLog($"连接断开  {GetConnectorDetail(inc)}")
    End Sub

    Private Sub Allocator_ClientOnline(sender As Object, e As ServerEventArgs) Handles Allocator.ClientOnline
        Dim inc = TryCast(sender, INConnector)
        inc.AddRequestHandle(obServer)

        If inc.GetConnectorType = ConnectorType.TCPServerClient Then
            AddTCPClientItem(inc.GetConnectorDetail())
        End If

        If inc.GetConnectorType = ConnectorType.UDPClient Then
            AddUDPItem(inc.GetConnectorDetail())
        End If
        AddLog($"连接建立  {GetConnectorDetail(inc)}")
    End Sub

    Private Sub Allocator_CommandProcessEvent(sender As Object, e As CommandEventArgs) Handles Allocator.CommandProcessEvent
        If mUseEcho Then Return

        Dim dtl = e.ConnectorDetail

        AddLog($"执行命令  {GetConnectorDetail(dtl)}")
    End Sub

    Private Sub Allocator_CommandTimeout(sender As Object, e As CommandEventArgs) Handles Allocator.CommandTimeout
        AddLog($"命令发送超时 {GetConnectorDetail(e.CommandDetail.Connector)}")
    End Sub

    Private Sub Allocator_CommandErrorEvent(sender As Object, e As CommandEventArgs) Handles Allocator.CommandErrorEvent
        AddLog($"命令执行时发成错误 {GetConnectorDetail(e.CommandDetail.Connector)}")
    End Sub

    Private Sub Allocator_AuthenticationErrorEvent(sender As Object, e As CommandEventArgs) Handles Allocator.AuthenticationErrorEvent
        AddLog($"身份验证错误 {GetConnectorDetail(e.CommandDetail.Connector)}")
    End Sub

    Private Sub Allocator_CommandCompleteEvent(sender As Object, e As CommandEventArgs) Handles Allocator.CommandCompleteEvent
        If mUseEcho Then Return

        Dim dtl = e.CommandDetail
        Dim lSec = (dtl.EndTime - dtl.BeginTime).TotalMilliseconds()
        Dim cmd As ReadSN = TryCast(e.Command, ReadSN)
        If cmd IsNot Nothing Then
            AddLog($"命令完成，耗时：{lSec:0.0} 毫秒，序号：" & cmd.CmdNum)
        Else
            AddLog($"命令完成，耗时：{lSec:0.0} 毫秒")
        End If
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

    Private Sub Allocator_TransactionMessage(connector As INConnectorDetail, EventData As INData) Handles Allocator.TransactionMessage
        AddLog($"事务消息 {GetConnectorDetail(connector)}")
    End Sub

    Private Function GetThreadID() As Integer
        Return Threading.Thread.CurrentThread.ManagedThreadId
    End Function

    Private mDisTime As Date
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        mDisTime = Now
        Allocator.Dispose()
    End Sub
    Private Sub Allocator_DisposeCallBlack() Handles Allocator.DisposeCallBlack
        AddLog($"对象已完成销毁工作！ {(Now - mDisTime).TotalSeconds()}")
    End Sub


    Private Sub butTCPServer_Click(sender As Object, e As EventArgs) Handles butOpenTCPServer.Click
        Dim sAddr As String = txtWatchAddr.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As TCPServer.TCPServerDetail = New TCPServer.TCPServerDetail(sAddr, sPort)
        Allocator.OpenConnector(serverDtl)

    End Sub

    Private Sub butCloseTCPServer_Click(sender As Object, e As EventArgs) Handles butCloseTCPServer.Click
        Dim sAddr As String = txtWatchAddr.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As TCPServer.TCPServerDetail = New TCPServer.TCPServerDetail(sAddr, sPort)
        Allocator.CloseConnector(serverDtl)
    End Sub

    Private Sub butOpenUDPServer_Click(sender As Object, e As EventArgs) Handles butOpenUDPServer.Click
        Dim sAddr As String = txtWatchAddr.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As UDP.UDPServerDetail = New UDP.UDPServerDetail(sAddr, sPort)
        Allocator.OpenConnector(serverDtl)
    End Sub

    Private Sub butCloseUDPServer_Click(sender As Object, e As EventArgs) Handles butCloseUDPServer.Click
        Dim sAddr As String = txtWatchAddr.Text
        Dim sPort As Integer = txtWatchPort.Text.ToInt32()
        Dim serverDtl As UDP.UDPServerDetail = New UDP.UDPServerDetail(sAddr, sPort)
        Allocator.CloseConnector(serverDtl)
    End Sub



    Private Function GetConnectorDetail(conn As INConnector) As String
        Return GetConnectorDetail(conn.GetConnectorDetail())
    End Function

    Private Function GetConnectorDetail(conn As INConnectorDetail) As String
        Dim oConn = Allocator.GetConnector(conn)
        If oConn Is Nothing Then Return conn?.ToString()
        Dim local = oConn.LocalAddress

        Select Case conn.GetTypeName
            Case ConnectorType.TCPClient
                Dim tcpclient = TryCast(conn, TCPClientDetail)
                Return $"通道类型：TCP客户端 本地IP：{local.Addr}:{local.Port} ,远端IP：{tcpclient.Addr}:{tcpclient.Port} :{Now.ToTimeffff()}"
            Case ConnectorType.TCPServerClient

                Return $"通道类型：TCPServer_Client key：{conn.GetKey()} :{Now.ToTimeffff()}"

            Case ConnectorType.UDPClient
                Dim udpOnly = TryCast(conn, TCPClientDetail)
                Return $"类型：UDP Key：{conn.GetKey()} :{Now.ToTimeffff()}"
            Case ConnectorType.UDPServer
                Dim udpserver = TryCast(conn, UDP.UDPServerDetail)
                Return $"类型：UDP 服务器  本地绑定IP：{local.Addr}:{local.Port}:{Now.ToTimeffff()}"
            Case ConnectorType.TCPServer
                Dim tcpserver = TryCast(conn, TCPServer.TCPServerDetail)
                Return $"类型：TCP服务器  本地绑定IP：{local.Addr}:{local.Port} :{Now.ToTimeffff()}"
            Case ConnectorType.SerialPort
                Dim com = TryCast(conn, SerialPort.SerialPortDetail)
                Return $"类型：串口 COM：{local.Port},波特率:{com.Baudrate} :{Now.ToTimeffff()}"
            Case ConnectorType.WebSocketServer
                Dim tcpserver = TryCast(conn, WebSocket.Server.WebSocketServerDetail)
                Return $"类型：Websocket服务器  本地绑定IP：{local.Addr}:{local.Port} :{Now.ToTimeffff()}"
            Case ConnectorType.WebSocketServerClient
                Dim tcpserver = TryCast(conn, WebSocket.Server.WebSocketServerDetail)
                Return $"类型：Websocket服务器的客户端  key：{conn.GetKey()} :{Now.ToTimeffff()}"
            Case ConnectorType.WebSocketClient
                Return $"类型：Websocket客户端  key：{conn.GetKey()} :{Now.ToTimeffff()}"
        End Select
        Return conn.ToString()
    End Function



    Private Sub obServer_DisposeRequestEvent(connector As INConnector, msg As String) Handles obServer.DisposeRequestEvent
        If mUseEcho Then
            '发送回去
            msg = System.Text.Encoding.Default.GetString(msg.HexToByte())
            Dim bEcho As Boolean = False
            Select Case connector.GetConnectorType()
                Case ConnectorType.TCPServerClient
                    bEcho = True

                Case ConnectorType.UDPClient
                    bEcho = True
                Case ConnectorType.TCPServer
                    bEcho = True

                Case ConnectorType.SerialPort
                    bEcho = True

                Case ConnectorType.WebSocketServerClient
                    bEcho = True
            End Select

            If bEcho Then
                Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(connector.GetConnectorDetail()),
                                                    New Command.Text.TextCommandParameter(
                                                    msg, System.Text.Encoding.Default))
                Allocator.AddCommand(txt)
            End If
            Return

        End If
        AddLog($"接收数据:{GetConnectorDetail(connector)}: 0x{msg}")


    End Sub

    Private Sub obServer_DisposeResponseEvent(connector As INConnector, msg As String) Handles obServer.DisposeResponseEvent
        If mUseEcho Then
            Return
        End If
        AddLog($"发送数据:{GetConnectorDetail(connector)}: 0x{msg}")
    End Sub


    Private Sub AddLog(ByVal sTxt As String)
        Try
            If txtLog.InvokeRequired Then
                Invoke(New Action(Of String)(AddressOf AddLog), sTxt)
                Return
            End If
            txtLog.AppendText(sTxt + vbNewLine)
        Catch ex As Exception

        End Try


    End Sub

#Region "UDP"
    Private UDPClient As Dictionary(Of String, UDPClientItem)

    Private Sub InIUDP()
        UDPClient = New Dictionary(Of String, UDPClientItem)
    End Sub

    Private Class UDPClientItem
        Public UDPDetail As UDP.UDPClientDetail

        Public Sub New(ByVal dtl As UDP.UDPClientDetail_ReadOnly)
            UDPDetail = New UDP.UDPClientDetail(dtl.Addr, dtl.Port, dtl.LocalAddr, dtl.LocalPort)
        End Sub

        Public Overrides Function ToString() As String

            Return $"{UDPDetail.Addr}:{UDPDetail.Port}"
        End Function
    End Class




    Private Sub AddUDPItem(dtl As INConnectorDetail)

        If cmdUDPClient.InvokeRequired Then
            Me.Invoke(Sub() AddUDPItem(dtl))
            Return
        End If
        Dim item = New UDPClientItem(dtl)
        cmdUDPClient.Items.Add(item)
        cmdUDPClient.SelectedIndex = cmdUDPClient.Items.Count - 1
        UDPClient.Add(dtl.GetKey, item)
    End Sub

    Private Sub RemoveUDPItem(dtl As INConnectorDetail)

        If cmdUDPClient.InvokeRequired Then
            Me.Invoke(Sub() RemoveUDPItem(dtl))
            Return
        End If
        Dim sKey As String = dtl.GetKey()
        If UDPClient.ContainsKey(sKey) Then
            cmdUDPClient.Items.Remove(UDPClient(sKey))
            cmdUDPClient.SelectedIndex = cmdUDPClient.Items.Count - 1
            UDPClient.Remove(sKey)
        End If
    End Sub



    Private Sub butUDPSend_Click(sender As Object, e As EventArgs) Handles butUDPSend.Click
        Dim item As UDPClientItem = cmdUDPClient.SelectedItem
        If item Is Nothing Then Return
        Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(item.UDPDetail),
                                                New Command.Text.TextCommandParameter(txtUDPClient.Text, System.Text.Encoding.Default))
        Allocator.AddCommand(txt)
    End Sub

    Private Sub butCloseUDP_Click(sender As Object, e As EventArgs) Handles butCloseUDP.Click
        Dim item As UDPClientItem = cmdUDPClient.SelectedItem
        If item Is Nothing Then Return
        Allocator.CloseConnector(item.UDPDetail)
    End Sub

    Private Sub butSendUDPClient_Click(sender As Object, e As EventArgs) Handles butSendUDPClient.Click
        Dim sLocalAddr As String = txtWatchAddr.Text
        Dim sLocalPort As Integer = txtWatchPort.Text.ToInt32()

        Dim sAddr As String = txtUDPAddr.Text.Trim
        Dim port As Integer = Integer.Parse(txtUDPPort.Text.Trim)

        Dim sTxt As String = txtUDPClientText.Text
        If String.IsNullOrEmpty(sTxt) Then
            Return
        End If

        Dim conn = New UDP.UDPClientDetail(sAddr, port, sLocalAddr, sLocalPort)



        Try
            If chkUDPIsHEX.Checked Then
                Dim bBuf = sTxt.HexToByte()
                Dim bBuffer As IByteBuffer = UnpooledByteBufferAllocator.Default.Buffer(bBuf.Length)
                bBuffer.WriteBytes(bBuf)

                Dim binCMD As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(conn),
                                                    New Command.Byte.ByteCommandParameter(
                                                    bBuffer))
                Allocator.AddCommand(binCMD)
            Else
                Dim dtl = New Text.TextCommandDetail(conn)
                dtl.Timeout = 300
                dtl.RestartCount = 20
                Dim txt As New Command.Text.TextCommand(dtl,
                                                    New Command.Text.TextCommandParameter(
                                                    sTxt, System.Text.Encoding.Default))
                Allocator.AddCommand(txt)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, 16, "错误")
        End Try

    End Sub

    Private Sub butTCPClientByDomain_Click(sender As Object, e As EventArgs) Handles butTCPClientByDomain.Click
        Dim connDtl = New TCPClient.TCPClientDetail("yun.pc15.net", 9001)

        Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(connDtl),
                                                New Command.Text.TextCommandParameter("asdasfsdf", System.Text.Encoding.Default))
        Allocator.AddCommand(txt)
    End Sub
#End Region

#Region "TCP Server"

    Private TCPServerClients As Dictionary(Of String, TCPServerClientDetail_Item)
    Private Sub InITCPServerClient()
        TCPServerClients = New Dictionary(Of String, TCPServerClientDetail_Item)
    End Sub


    Private Class TCPServerClientDetail_Item
        Public Remote As IPDetail
        Public Local As IPDetail
        Public Key As String

        Public Sub New(ByVal dtl As TCPServer.Client.TCPServerClientDetail)
            Remote = New IPDetail(dtl.Remote.Addr, dtl.Remote.Port)
            Local = New IPDetail(dtl.Local.Addr, dtl.Local.Port)
            Key = dtl.Key
        End Sub

        Public Overrides Function ToString() As String
            Return $"本地：{Local.Addr}:{Local.Port} -- 远程:{Remote.Addr}:{Remote.Port}"
        End Function
    End Class

    Private Sub RemoveTCPClientItem(dtl As INConnectorDetail)
        If cmbTCPClient.InvokeRequired Then
            Me.Invoke(Sub() RemoveTCPClientItem(dtl))
            Return
        End If
        Dim oClient As TCPServer.Client.TCPServerClientDetail = dtl

        If Not TCPServerClients.ContainsKey(oClient.Key) Then
            Return
        End If
        Dim oItem = TCPServerClients(oClient.Key)
        cmbTCPClient.Items.Remove(oItem)
        cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1
        TCPServerClients.Remove(oItem.Key)
    End Sub

    Private Sub AddTCPClientItem(dtl As INConnectorDetail)
        If cmbTCPClient.InvokeRequired Then
            Me.Invoke(Sub() AddTCPClientItem(dtl))
            Return
        End If
        Dim oClient As TCPServer.Client.TCPServerClientDetail = dtl
        Dim oItem = New TCPServerClientDetail_Item(oClient)

        cmbTCPClient.Items.Add(oItem)
        cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1
        TCPServerClients.Add(oItem.Key, oItem)
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

    Private Sub butSerialPort_Click(sender As Object, e As EventArgs) Handles butSerialPort.Click
        For i = 1 To 10
            Dim cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.SerialPort, String.Empty, 3,
                                                        CommandDetailFactory.ControllerType.Door88, "8940H09030001", "FFFFFFFF")

            Dim Cmd = New ReadSN(cmdDtl, i + 1)
            Allocator.AddCommand(Cmd)
        Next
    End Sub


#End Region



#Region "WebSocket Server"
    Private Sub ButOpenWebSocketServer_Click(sender As Object, e As EventArgs) Handles ButOpenWebSocketServer.Click
        Dim x509Data = My.Resources.SSLX509


        Dim x509 As X509Certificate2 = New X509Certificate2(x509Data, "BRUsqOWH")
        Dim detail As WebSocketServerDetail = New WebSocketServerDetail("127.0.0.1", 6001, True, x509, "/WebToolUSBAPI")

        '打开WebSocket服务器
        Allocator.OpenConnector(detail)
    End Sub

    Private Sub ButCloseWebSocketServer_Click(sender As Object, e As EventArgs) Handles ButCloseWebSocketServer.Click
        Dim detail As WebSocketServerDetail = New WebSocketServerDetail("127.0.0.1", 6001)
        Allocator.CloseConnector(detail)
    End Sub
#End Region

#Region "Websoket Client"
    Private Sub butWebsocketClient_Click(sender As Object, e As EventArgs) Handles butWebsocketClient.Click
        Dim x509Data = My.Resources.SSLX509


        Dim x509 As X509Certificate2 = New X509Certificate2(x509Data, "BRUsqOWH")
        'wss://yun.pc15.net/websocket
        Dim connDtl = New WebSocket.Client.WebSocketClientDetail("yun.pc15.net", 443,
                                                                 String.Empty, 0, True, x509, Nothing, "websocket")

        Dim txt As New Command.Text.TextCommand(New Text.TextCommandDetail(connDtl),
                                                New Command.Text.TextCommandParameter("123456789", System.Text.Encoding.Default))
        Allocator.AddCommand(txt)
    End Sub


#End Region
    Private mUseEcho As Boolean
    Private Sub ChkEcho_CheckedChanged(sender As Object, e As EventArgs) Handles chkEcho.CheckedChanged
        'If obServer Is Nothing Then Return
        mUseEcho = chkEcho.Checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkHexDump.CheckedChanged
        If obServer Is Nothing Then Return
        obServer.HexDump = chkHexDump.Checked
    End Sub


    Private Sub txtLog_DoubleClick(sender As Object, e As EventArgs) Handles txtLog.DoubleClick
        txtLog.Clear()
    End Sub


End Class
