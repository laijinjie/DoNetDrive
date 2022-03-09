Imports System.Net
Imports System.Net.Sockets
Imports DotNetty.Buffers

Namespace Connector.TCPClient
    ''' <summary>
    ''' 用于和TCP Server进行通讯的TCP Client
    ''' </summary>
    Public Class TCPClientConnector
        Inherits AbstractConnector

        ''' <summary>
        ''' 默认的读取缓冲区
        ''' </summary>
        Public Shared DefaultReadDataBufferSize As Integer = 2048

        ''' <summary>
        ''' 客户端Socket
        ''' </summary>
        Protected _Client As Socket


        ''' <summary>
        ''' 最大连接等待时间
        ''' </summary>
        Protected _ConnectTimeoutMSEL As Integer

        ''' <summary>
        ''' 最大重新连接次数
        ''' </summary>
        Protected _ReconnectMax As Integer

        ''' <summary>
        ''' 重新连接次数
        ''' </summary>
        Protected _ReconnectCount As Integer

        Protected Property _RemoteAddress As IPDetail
        Protected Property _LocalAddress As IPDetail




        ''' <summary>
        ''' 初始化TCP客户端对象
        ''' </summary>
        ''' <param name="detail">标识此通道的信息类</param>
        Public Sub New(detail As TCPClientDetail)

            SetConnectOption(detail)
            _ReconnectCount = 0

            MyBase._ConnectorDetail = detail
            _RemoteAddress = New IPDetail(detail.Addr, detail.Port)
            _LocalAddress = New IPDetail(detail.LocalAddr, detail.LocalPort)
        End Sub

        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return TCPClientConnectorStatus.Closed
        End Function

        Public Overrides Function RemoteAddress() As IPDetail
            Return _RemoteAddress
        End Function

        Public Overrides Function LocalAddress() As IPDetail
            Return _LocalAddress
        End Function

        ''' <summary>
        ''' 设置连接参数，超时上限和重连上限
        ''' </summary>
        ''' <param name="detail"></param>
        Private Sub SetConnectOption(detail As TCPClientDetail)
            _ConnectTimeoutMSEL = detail.Timeout
            _ReconnectMax = detail.RestartCount

            If _ConnectTimeoutMSEL > TCPClientFactory.CONNECT_TIMEOUT_MILLIS_MAX Then
                _ConnectTimeoutMSEL = TCPClientFactory.CONNECT_TIMEOUT_MILLIS_MAX
            ElseIf _ConnectTimeoutMSEL < TCPClientFactory.CONNECT_TIMEOUT_MILLIS_MIN Then
                _ConnectTimeoutMSEL = TCPClientFactory.CONNECT_TIMEOUT_MILLIS_MIN
            End If

            If _ReconnectMax > TCPClientFactory.CONNECT_RECONNECT_MAX Then
                _ReconnectMax = TCPClientFactory.CONNECT_RECONNECT_MAX
            ElseIf _ReconnectMax < 0 Then
                _ReconnectMax = 0
            End If
        End Sub

        ''' <summary>
        ''' 设定默认的超时等待和重连参数
        ''' </summary>
        Private Sub SetConnectOptionByDefault()
            _ConnectTimeoutMSEL = TCPClientFactory.CONNECT_TIMEOUT_Default
            _ReconnectMax = TCPClientFactory.CONNECT_RECONNECT_Default
        End Sub

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPClient
        End Function


#Region "接收数据"
        Protected Overridable Async Function ReceiveAsync() As Task
            Dim bBuf(DefaultReadDataBufferSize) As Byte
            'Trace.WriteLine("开始 ReceiveAsync")
            Dim abuf = New ArraySegment(Of Byte)(bBuf)
            Dim NettyBuf = Unpooled.WrappedBuffer(bBuf)
            NettyBuf.Clear()
            Try
                Dim lReadCount = Await _Client.ReceiveAsync(abuf, SocketFlags.None).ConfigureAwait(False)
                While lReadCount > 0
                    If _isRelease Then Exit While
                    NettyBuf.SetWriterIndex(lReadCount)
                    Me.ReadByteBuffer(NettyBuf)
                    NettyBuf.Clear()
                    lReadCount = Await _Client.ReceiveAsync(abuf, SocketFlags.None).ConfigureAwait(False)
                End While
            Catch ex As Exception
                NettyBuf.Release()
                If TypeOf ex Is SocketException Then
                    Dim scex As SocketException = ex
                    Select Case scex.ErrorCode
                        Case 995 '由于线程退出或应用程序请求，已中止 I/O 操作。  本地关闭连接
                            Return '这种是本地主动断开连接
                        Case 10054
                            '远程主动关闭连接，说明可能是远程拒绝连接
                            _ConnectorDetail.SetError(ex)
                            FireConnectorErrorEvent(_ConnectorDetail)
                            Me.SetInvalid() '被关闭了就表示无效了
                            Return
                    End Select

                End If
                ConnectFail(ex)
                Return
            End Try

            NettyBuf.Release()
            If _Client IsNot Nothing Then
                If _Client.Connected Then
                    '这种情况说明是对方主动断开连接的
                    _Client.Close()
                    _Client.Dispose()
                    _IsActivity = False
                    ClearCommand(New SocketException(10054))
                    FireConnectorClosedEvent(Me._ConnectorDetail)

                    SetInvalid()
                End If

            End If
            'Trace.WriteLine("退出 ReceiveAsync")
        End Function
#End Region

#Region "发送数据"
        Protected Overrides Async Function WriteByteBuf0(buf As IByteBuffer) As Task
            If CheckIsInvalid() Then
                Await Task.FromException(New Exception("connect is invalid"))
                
            End If

            If _Client Is Nothing Then
                Await Task.FromException(New Exception($"connect is {_Status.Status}"))
                Return
            End If

            If _Client.Connected Then
                Await _Client.SendAsync(New ArraySegment(Of Byte)(buf.Array, buf.ArrayOffset, buf.ReadableBytes), SocketFlags.None).ConfigureAwait(False)
            Else
                Await Task.FromException(New Exception("connect is closed"))
                Return
            End If

            buf.Release()
        End Function
#End Region



#Region "连接服务器"
        Public Overrides Async Function ConnectAsync() As Task
            If Me.CheckIsInvalid() Then Return
            If Me.IsActivity Then Return
            If Me._Status.Status = "Connecting" Then Return
            _Status = TCPClientConnectorStatus.Connecting
            Dim detail As TCPClientDetail = _ConnectorDetail
            UpdateActivityTime()
            _ReconnectCount += 1
            Me._IsActivity = False
            '远端终结点
            Dim oPoint As IPEndPoint
            Dim oIP As IPAddress = Nothing
            If IPAddress.TryParse(detail.Addr, oIP) Then
                oPoint = New IPEndPoint(oIP, detail.Port)
            Else
                Dim oDNSIP As IPHostEntry = Await Dns.GetHostEntryAsync(detail.Addr).ConfigureAwait(False)
                If oDNSIP.AddressList.Length > 0 Then
                    '获取服务器节点
                    oIP = oDNSIP.AddressList(0)
                End If

                oPoint = New IPEndPoint(oIP, detail.Port)
            End If

            If _ActivityCommand Is Nothing Then
                '一个新的指令
                If _CommandList.TryPeek(_ActivityCommand) Then
                    _ActivityCommand.SetStatus(_ActivityCommand.GetStatus_Wating())
                    SetConnectOption(_ActivityCommand.CommandDetail.Connector)

                    fireCommandProcessEvent(_ActivityCommand.GetEventArgs())
                Else
                    SetConnectOptionByDefault()
                End If
                _ActivityCommand = Nothing
            End If


            _Client = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            If String.IsNullOrEmpty(detail.LocalAddr) > 0 Or detail.LocalPort > 0 Then
                Dim oLocalIP As IPEndPoint
                If Not String.IsNullOrEmpty(detail.LocalAddr) Then
                    oLocalIP = New IPEndPoint(IPAddress.Parse(detail.LocalAddr), detail.LocalPort)
                Else
                    oLocalIP = New IPEndPoint(IPAddress.Any, detail.LocalPort)
                End If
                '需要先绑定本地IP和端口
                _Client.Bind(oLocalIP)
            End If


            _ConnectDate = DateTime.Now
            FireConnectorConnectingEvent(_ConnectorDetail)
            Await _Client.ConnectAsync(oPoint).ConfigureAwait(False)
            _ConnectDate = DateTime.Now
            FireConnectorConnectedEvent(_ConnectorDetail)
            _ConnectorDetail.SetError(Nothing)
            '连接成功
            _Status = TCPClientConnectorStatus.Connected
            Me._IsActivity = True
            _ReconnectCount = 0 '连接成功，则复位此标志
            Me._LocalAddress = New IPDetail(_Client.LocalEndPoint)
            '开始等待响应
            ReceiveAsync().ConfigureAwait(False)
            '开始执行命令
            CheckCommandList()

            'Trace.WriteLine("已连接到设备")
        End Function

        ''' <summary>
        ''' 检查连接是否已超时
        ''' </summary>
        Public Overridable Sub CheckConnectorTimeout()
            If Not Me._Status.Status = "Connecting" Then Return

            Dim lTime = (DateTime.Now - _ConnectDate).TotalMilliseconds
            Try
                If lTime > _ConnectTimeoutMSEL Then
                    ConnectFail(New TimeoutException($"Connect {RemoteAddress.ToString()} Timeout"))
                Else
                    UpdateActivityTime()
                End If

            Catch ex As Exception
                If _Client IsNot Nothing Then
                    _Client.Dispose()
                End If
                _Client = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' 异步处理成功的后续操作
        ''' </summary>
        Protected Friend Sub ConnectingNext(connTask As Task)
            If Me.CheckIsInvalid() Then Return
            If Not Me._Status.Status = "Connecting" Then Return
            If _Client Is Nothing Then Return
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

            If _Client IsNot Nothing Then
                _Client.Close()
                _Client.Dispose()
            End If

            _Client = Nothing
            Me._IsActivity = False


            If _ReconnectCount > _ReconnectMax Then

                If Me._IsForcibly Then
                    _ConnectorDetail.SetError(New Exception("TCP 连接已断开，将会继续重试", ex))
                    FireConnectorErrorEvent(_ConnectorDetail)

                    _ReconnectCount = 0
                    _Status = TCPClientConnectorStatus.Closed
                Else
                    _ConnectorDetail.SetError(New Exception("已达到最大重试次数", ex))
                    FireConnectorErrorEvent(_ConnectorDetail)
                    Me.SetInvalid() '被关闭了就表示无效了
                End If
            Else
                '再次重试
                _Status = TCPClientConnectorStatus.Closed
            End If
        End Sub

        ''' <summary>
        ''' 连接状态检查，当连接成功时，检查连接状态
        ''' </summary>
        Protected Friend Overridable Sub CheckConnectedStatus()
            If Not CheckIsInvalid() Then
                If _CommandList.Count = 0 Then
                    If _IsForcibly Then
                        CheckKeepaliveTime()
                    End If
                Else
                    CheckCommandList()
                End If
            End If
        End Sub
#End Region

#Region "关闭连接"
        Public Overrides Async Function CloseAsync() As Task
            If Me._isRelease Then Return
            If Not _IsActivity Then Return

            Me._Status = ConnectorStatus.Invalid
            If _Client Is Nothing Then Return
            Try
                'Trace.WriteLine("正在关闭连接")


                Me._IsActivity = False

                If _Client IsNot Nothing Then
                    'Trace.WriteLine("关闭连接")
                    _Client.Close()
                    _Client.Dispose()
                End If

            Catch ex As Exception
                _ConnectorDetail.SetError(ex)
            End Try
            _Client = Nothing
            UpdateActivityTime()

            Await Task.Run(Sub()
                               FireConnectorClosedEvent(Me._ConnectorDetail)
                               Me.SetInvalid() '被关闭了就表示无效了
                           End Sub).ConfigureAwait(False)
            Me._IsForcibly = False
        End Function
#End Region



        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            '_Client = Nothing
            _RemoteAddress = Nothing
            _LocalAddress = Nothing
        End Sub


    End Class
End Namespace

