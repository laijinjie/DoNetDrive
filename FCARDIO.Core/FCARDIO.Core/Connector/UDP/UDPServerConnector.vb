Imports System.Net
Imports System.Net.Sockets
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports System.Collections.Concurrent
Imports DoNetDrive.Core.Command

Namespace Connector.UDP
    ''' <summary>
    ''' 表示一个UDP监听服务器，当收到一个新的请求时，将创建一个UDPClient通道用户处理请求
    ''' </summary>
    Public Class UDPServerConnector
        Inherits AbstractConnector
        Implements IUDPServerConnector


        ''' <summary>
        ''' 默认的读取缓冲区
        ''' </summary>
        Public Shared DefaultReadDataBufferSize As Integer = 2048

        ''' <summary>
        ''' 本地端点信息
        ''' </summary>
        Protected Property _LocalAddress As IPDetail
        ''' <summary>
        ''' 服务器套接字
        ''' </summary>
        Protected _UDPServer As Socket


        Protected ConnecterManage As IConnecterManage
        Public Event ServerCloseEvent(sender As IUDPServerConnector) Implements IUDPServerConnector.ServerCloseEvent

        ''' <summary>
        ''' 初始化通道
        ''' </summary>
        ''' <param name="detail"></param>
        Public Sub New(detail As UDPServerDetail, cntManage As IConnecterManage)
            _CommandList = Nothing
            _DecompileList = Nothing
            _ActivityCommand = Nothing
            _IsForcibly = True
            ConnecterManage = cntManage

            MyBase._ConnectorDetail = detail
            _LocalAddress = New IPDetail(detail.LocalAddr, detail.LocalPort)

        End Sub

        ''' <summary>
        ''' 获取初始化通道状态
        ''' </summary>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return ConnectorStatus.Closed
        End Function

        Public Overrides Function RemoteAddress() As IPDetail
            Return Nothing
        End Function

        Public Overrides Function LocalAddress() As IPDetail
            Return _LocalAddress
        End Function


        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.UDPServer
        End Function

#Region "绑定"
        Public Overrides Async Function ConnectAsync() As Task
            If _isRelease Then Return
            If Me.CheckIsInvalid() Then Return
            If Me.IsActivity Then Return
            If Me._Status.Status = "Bind" Then Return

            Dim detail As UDPServerDetail = _ConnectorDetail
            UpdateActivityTime()

            Me._IsActivity = False
            Dim localIPAddress As IPAddress
            If Not String.IsNullOrEmpty(detail.LocalAddr) Then
                If detail.LocalAddr = "*" Then
                    localIPAddress = IPAddress.Any
                Else
                    localIPAddress = IPAddress.Parse(detail.LocalAddr)
                End If
            Else
                localIPAddress = IPAddress.Any
            End If
            _UDPServer = New Socket(localIPAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp)


            _UDPServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, True)
            Dim oLocalIPEndPoint As IPEndPoint
            oLocalIPEndPoint = New IPEndPoint(localIPAddress, detail.LocalPort)
            '需要先绑定本地IP和端口
            _UDPServer.Bind(oLocalIPEndPoint)


            _ConnectDate = DateTime.Now
            FireConnectorConnectedEvent(_ConnectorDetail)
            '连接成功
            _Status = ConnectorStatus.Bind
            Me._IsActivity = True


            '开始等待响应
            ReceiveAsync().ConfigureAwait(False)

            Await Task.CompletedTask()
        End Function


        ''' <summary>
        ''' 异步处理成功的后续操作
        ''' </summary>
        Protected Friend Sub ConnectingNext(connTask As Task)
            If Me.CheckIsInvalid() Then Return
            If Not Me._Status.Status = "Connecting" Then Return
            If _UDPServer Is Nothing Then Return
            If connTask.IsFaulted Or connTask.IsCanceled Then
                '有错误，或已取消
                ConnectFail(connTask.Exception)
            End If
        End Sub

        ''' <summary>
        ''' 连接失败
        ''' </summary>
        Protected Overridable Sub ConnectFail(ByVal ex As Exception)
            If TypeOf ex Is ObjectDisposedException Then
                '说明是被强制取消的
                Return
            End If

            If _Status.Status = "Invalid" Then
                Return
            End If

            If _UDPServer IsNot Nothing Then
                _UDPServer.Close()
                _UDPServer.Dispose()
            End If

            _UDPServer = Nothing
            Me._IsActivity = False

            _ConnectorDetail.SetError(ex)
            FireConnectorErrorEvent(_ConnectorDetail)
            Me.SetInvalid() '被关闭了就表示无效了
        End Sub


#End Region

#Region "接收数据"
        ''' <summary>
        ''' 开始接收数据
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Async Function ReceiveAsync() As Task
            Dim bBuf(DefaultReadDataBufferSize) As Byte
            'Trace.WriteLine("开始 ReceiveAsync")
            Dim abuf = New ArraySegment(Of Byte)(bBuf)
            Dim remote = New IPEndPoint(IPAddress.Any, 0)
            Dim NettyBuf = Unpooled.WrappedBuffer(bBuf)
            Dim SocketException As Exception

            Dim iReReceiveFromCount As Integer

            Do
                NettyBuf.Clear()
                Try
                    Dim ReceiveFromResult As SocketReceiveFromResult =
                    Await _UDPServer.ReceiveFromAsync(abuf, SocketFlags.None, remote).ConfigureAwait(False)

                    If ReceiveFromResult.ReceivedBytes > 0 Then
                        iReReceiveFromCount = 0
                        If _isRelease Then Exit Do
                        NettyBuf.SetWriterIndex(ReceiveFromResult.ReceivedBytes)
                        Dim oPacketRemote As IPEndPoint = ReceiveFromResult.RemoteEndPoint
                        ReadByteBufferNext(NettyBuf, oPacketRemote)
                        'Exit Do
                    Else
                        '操作失败，重试
                        Await Task.Delay(10)
                        iReReceiveFromCount += 1
                        If iReReceiveFromCount > 100 Then
                            Exit Do '关闭连接
                        End If

                    End If


                Catch socketEx As SocketException
                    'ConnectionReset	10054	此连接由远程对等计算机重置。  此时不理会，直接重新调用 ReceiveFromAsync
                    If (socketEx.NativeErrorCode <> 10054) Then
                        SocketException = socketEx
                        Exit Do
                    End If
                Catch ex As Exception
                    SocketException = ex

                    Exit Do
                End Try
            Loop While True


            NettyBuf.Release()



            If IsInvalid Then Return
            If Not _IsActivity Then Return

            ClearCommand(SocketException)
            Me._ConnectorDetail.SetError(SocketException)

            CloseAsync()

            SetInvalid()
        End Function



        Private mUDPClientDetail As UDPClientDetail

        ''' <summary>
        ''' 读取到数据后的处理
        ''' </summary>
        ''' <param name="msg">将读取到的数据打包到bytebuffer</param>
        Protected Sub ReadByteBufferNext(msg As IByteBuffer, oPacketRemote As IPEndPoint)
            If _isRelease Then Return
            UpdateReadDataTime()
            Me._ReadTotalBytes += msg.ReadableBytes




            Dim sRemoteIP As String = oPacketRemote.Address.ToString
            Dim iRemotePort As Integer = oPacketRemote.Port

            If (mUDPClientDetail Is Nothing) Then
                mUDPClientDetail = New UDPClientDetail(sRemoteIP, iRemotePort, _LocalAddress.Addr, _LocalAddress.Port)
            Else
                mUDPClientDetail.Addr = sRemoteIP
                mUDPClientDetail.Port = iRemotePort
            End If

            Dim sKey = mUDPClientDetail.GetKey()
            Dim Client As UDPServerClientConnector = ConnecterManage.GetConnector(sKey)

            If Client Is Nothing Then
                '不存在
                Client = AddClientConnector()
            End If

            If Client IsNot Nothing Then
                Client.ReadByteBufferNext(msg)
            End If


            '检查是否存在广播通道


            Dim sKeys = ConnecterManage.GetAllConnectorKeys()
            Dim sFindKey = $"{mUDPClientDetail.GetTypeName()}_Local:{mUDPClientDetail.LocalAddr}:{mUDPClientDetail.LocalPort}_Remote:255.255.255.255"
            For Each clientKey In sKeys
                If clientKey.StartsWith(sFindKey) Then
                    Client = ConnecterManage.GetConnector(clientKey)
                    If Client IsNot Nothing Then
                        Client.ReadByteBufferNext(msg)
                    End If
                End If
            Next
        End Sub
#End Region

#Region "发送数据"
        Protected Friend Async Function WriteByteBufByUDP(buf As IByteBuffer, oPacketRemote As IPEndPoint) As Task Implements IUDPServerConnector.WriteByteBufByUDP
            Dim arrybuf = New ArraySegment(Of Byte)(buf.Array, buf.ArrayOffset, buf.ReadableBytes)
            Await _UDPServer.SendToAsync(arrybuf, SocketFlags.None, oPacketRemote).ConfigureAwait(False)
        End Function


        Protected Overrides Function WriteByteBuf0(buf As IByteBuffer) As Task
            Throw New Exception("server conncet nonsupport WriteByteBuf")
        End Function
        Public Overrides Async Function WriteByteBuf(buf As IByteBuffer) As Task
            Await Task.FromException(New Exception("server conncet nonsupport WriteByteBuf"))
        End Function
#End Region


#Region "关闭连接"
        Public Overrides Async Function CloseAsync() As Task
            If CheckIsInvalid() Then Return
            If Not _IsActivity Then Return


            Me._Status = ConnectorStatus.Invalid
            If _UDPServer Is Nothing Then Return
            Try

                Me._IsForcibly = False
                Me._IsActivity = False

                If _UDPServer IsNot Nothing Then
                    'Trace.WriteLine("关闭连接")
                    _UDPServer.Close()
                    _UDPServer.Dispose()
                End If

            Catch ex As Exception
                _ConnectorDetail.SetError(ex)
            End Try

            RaiseEvent ServerCloseEvent(Me)

            _UDPServer = Nothing
            UpdateActivityTime()


            FireConnectorClosedEvent(Me._ConnectorDetail)
            Me.SetInvalid() '被关闭了就表示无效了
            Await Task.CompletedTask
        End Function
#End Region

#Region "去掉命令响应"
        Public Overrides Sub AddCommand(cd As INCommandRuntime)
            Throw New Exception("server conncet nonsupport AddCommand")
        End Sub

        Public Overrides Async Function RunCommandAsync(cd As INCommandRuntime) As Task(Of INCommand)
            Return Await Task.FromException(Of INCommand)(New Exception("server conncet nonsupport RunCommandAsync"))
        End Function
#End Region


#Region "创建客户端通道"
        Public Function AddClientConnector(oRemotePoint As IPEndPoint) As UDPServerClientConnector
            If mUDPClientDetail Is Nothing Then
                mUDPClientDetail = New UDPClientDetail(oRemotePoint.Address.ToString(), oRemotePoint.Port, _LocalAddress.Addr, _LocalAddress.Port)
            Else
                mUDPClientDetail.Addr = oRemotePoint.Address.ToString()
                mUDPClientDetail.Port = oRemotePoint.Port
            End If

            Return AddClientConnector()
        End Function


        ''' <summary>
        ''' 给通道添加一个UDP子节点
        ''' </summary>
        Public Function AddClientConnector() As UDPServerClientConnector
            Dim client As UDPServerClientConnector = ConnecterManage.GetConnector(mUDPClientDetail.GetKey())

            '检查连接通道是否已存在，不存在则重新建立连接
            If client Is Nothing Then

                Dim oDetail As UDPClientDetail = mUDPClientDetail.Clone()
                oDetail.ClientOnlineCallBlack = _ConnectorDetail.ClientOnlineCallBlack
                oDetail.ClientOfflineCallBlack = _ConnectorDetail.ClientOfflineCallBlack
                oDetail.ConnectedCallBlack = _ConnectorDetail.ConnectedCallBlack
                oDetail.ClosedCallBlack = _ConnectorDetail.ClosedCallBlack
                oDetail.ErrorCallBlack = _ConnectorDetail.ErrorCallBlack
                client = New UDPServerClientConnector(oDetail, Me, ConnecterManage)

                Return client
            Else
                Return client
            End If
        End Function

#End Region
        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            _UDPServer = Nothing
            _LocalAddress = Nothing
            ConnecterManage = Nothing
        End Sub


    End Class
End Namespace

