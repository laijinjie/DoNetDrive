Imports System.Threading
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels

Namespace Connector.Client

    ''' <summary>
    ''' 基于Netty的通讯通道
    ''' </summary>
    Public MustInherit Class AbstractNettyClientConnector(Of T)
        Inherits AbstractConnector

        ''' <summary>
        ''' 远程IP信息和端口信息
        ''' </summary>
        Public RemoteDetail As IPDetail

        ''' <summary>
        ''' 本地IP信息
        ''' </summary>
        Public LocalDetail As IPDetail

        ''' <summary>
        ''' 连接管理器所管理的通道
        ''' </summary>
        Protected _ClientChannel As IChannel

        ''' <summary>
        ''' 客户端操作的操作类
        ''' </summary>
        Protected _Handler As TCPClientNettyChannelHandler(Of T)

        ''' <summary>
        ''' 连接超时时间和连接失败次数
        ''' </summary>
        Protected _ConnectFailCount As Integer



        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Overrides Function GetConnectorType() As String


        ''' <summary>
        ''' 返回此通道的初始化状态
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Overrides Function GetInitializationStatus() As INConnectorStatus


        ''' <summary>
        ''' 标识本通道的信息类，只读
        ''' </summary>
        Protected ThisConnectorDetail As INConnectorDetail
        ''' <summary>
        ''' 返回记录此通道信息的类
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorDetail() As INConnectorDetail
            If ThisConnectorDetail Is Nothing Then
                ThisConnectorDetail = GetConnectorDetail0()
            End If
            Return ThisConnectorDetail
        End Function

        ''' <summary>
        ''' 创建一个连接头像详情对象，包含用于描述当前连接通道的信息
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetConnectorDetail0() As INConnectorDetail

#Region "连接服务器"
        ''' <summary>
        ''' 远程连接失败的处理过程
        ''' </summary>
        Protected Sub ConnectFail()
            If _isRelease Then Return

            _Status = GetStatus_Fail()
            _IsActivity = False

            _ConnectFailCount += 1 '失败次数加1

            ConnectFail0()
            If _isRelease Then Return
            CloseConnectCheck()
        End Sub



        ''' <summary>
        ''' 当连接通道连接已失效时调用
        ''' </summary>
        Protected MustOverride Sub ConnectFail0()


        ''' <summary>
        ''' 获取一个状态表示连接通道连接失败
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetStatus_Fail() As INConnectorStatus


        ''' <summary>
        ''' 远程连接成功
        ''' </summary>
        Protected Overridable Sub ConnectSuccess()
            _IsActivity = True
            _Status = GetStatus_Connected()

            _ConnectFailCount = 0
            ConnectSuccess0()
            Dim dtl = GetConnectorDetail()
            dtl.SetError(Nothing)
            FireConnectorConnectedEvent(dtl)
            '立刻检查是否有需要发送的命令
            CheckCommandList()
        End Sub

        ''' <summary>
        ''' 获取本地绑定地址
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function LocalAddress() As IPDetail
            'If _ClientChannel Is Nothing Then Return Nothing

            Return LocalDetail
        End Function


        ''' <summary>
        ''' 连接通道建立连接成功后的后续处理
        ''' </summary>
        Protected MustOverride Sub ConnectSuccess0()


        ''' <summary>
        ''' 获取一个状态表示连接通道连接已建立并工作正常的状态
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetStatus_Connected() As INConnectorStatus

#End Region

#Region "关闭连接"
        ''' <summary>
        ''' 关闭连接
        ''' </summary>
        Public Overrides Async Function CloseAsync() As Task
            If _ClientChannel IsNot Nothing Then

                If _ClientChannel.Active Then
                    Await _ClientChannel.CloseAsync()
                    FireConnectorClosedEvent(GetConnectorDetail())
                    '链接断开后会自动触发 ChannelInactive 事件，再此事件中再增加通知消息
                End If
            End If

            _IsActivity = False
            CloseConnectCheck()

            '_ClientChannel = Nothing
            _Status = GetInitializationStatus()
        End Function


#End Region

#Region "接收数据"

        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Friend Overridable Sub channelRead0(ctx As IChannelHandlerContext, msg As T)
            If TypeOf msg Is IByteBuffer Then
                ReadByteBuffer(TryCast(msg, IByteBuffer))
            End If

        End Sub
#End Region

#Region "发送数据"

        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overrides Function WriteByteBuf(buf As IByteBuffer) As Task
            If _isRelease Then Return Nothing
            If _ClientChannel Is Nothing Then
                Return Nothing
            End If
            UpdateActivityTime()

            DisposeResponse(buf)
            Dim rst = _ClientChannel?.WriteAndFlushAsync(buf)
            rst = rst.ContinueWith(Sub(x)

                                       If x.IsFaulted Then
                                           CloseConnector()
                                       End If
                                   End Sub)
            Return rst
            'Return _ClientChannel?.WriteAndFlushAsync(buf)
        End Function
#End Region

        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetByteBufAllocator() As IByteBufferAllocator
            Return _ClientChannel?.Allocator
        End Function


        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetEventLoop() As IEventLoop
            Return _ClientChannel?.EventLoop
        End Function


        ''' <summary>
        ''' 自定义通道事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="evt"></param>
        Friend Sub UserEventTriggered(ctx As IChannelHandlerContext, evt As Object)
            If _isRelease Then Return
            'If TypeOf evt Is IdleStateEvent Then
            '    Dim state = TryCast(evt, IdleStateEvent)
            '    If state IsNot Nothing Then

            '    End If
            'End If
        End Sub

#Region "连接关闭事件"
        ''' <summary>
        ''' 在连接正常或不正常关闭时发生
        ''' </summary>
        ''' <param name="ctx"></param>
        Friend Sub ChannelInactive(ctx As IChannelHandlerContext)
            If _isRelease Then Return
            'Trace.WriteLine($"{GetKey()} ，线程ID:{Thread.CurrentThread.ManagedThreadId} ChannelInactive 已关闭TCP连接 Time: {Date.Now:HH:mm:ss.ffff} ")
            Dim dtl = GetConnectorDetail()
            dtl.SetError(New System.Net.Sockets.SocketException(System.Net.Sockets.SocketError.Shutdown))
            FireConnectorClosedEvent(dtl)
            _ClientChannel = Nothing
            ConnectFail()
        End Sub


        ''' <summary>
        ''' 发生错误时，触发此事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="ex"></param>
        Friend Sub ExceptionCaught(ctx As IChannelHandlerContext, ex As Exception)
            If _isRelease Then Return
            'Debug.Print(ex.Message)
            Try
                Dim dtl = GetConnectorDetail()
                dtl.SetError(ex)

                ctx.CloseAsync()

            Catch ext As Exception

            Finally
                FireConnectorClosedEvent(GetConnectorDetail())
                ConnectFail()
            End Try
        End Sub
#End Region




        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            If _Handler IsNot Nothing Then
                _Handler.Release()
            End If
            _Handler = Nothing

            CloseConnector()

            LocalDetail = Nothing
            RemoteDetail = Nothing

            Release1()
            'ThisConnectorDetail = Nothing
        End Sub

        ''' <summary>
        ''' 下一级子类释放资源时调用
        ''' </summary>
        Protected MustOverride Sub Release1()


    End Class

End Namespace

