Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' 表示一个UDP监听服务器，当收到一个新的请求时，将创建一个UDPClient通道用户处理请求
    ''' </summary>
    Public Class UDPServerConnector
        Inherits AbstractConnector

        ''' <summary>
        ''' 服务器监听通道
        ''' </summary>
        Protected _Channel As IChannel


        ''' <summary>
        ''' 表示当前监听器的信息
        ''' </summary>
        Protected _Detail As UDPServerDetail

        ''' <summary>
        ''' UDP服务器的通道处理器
        ''' </summary>
        Protected mHandler As UDPServerChannelHandler

        ''' <summary>
        ''' 通道的子节点
        ''' </summary>
        Protected ChildChannel As Dictionary(Of String, UDPServerClientConnector)

        ''' <summary>
        ''' 广播的通道
        ''' </summary>
        Protected BroadcastChannel As Dictionary(Of Integer, UDPServerClientConnector)


        ''' <summary>
        ''' 通道销毁事件
        ''' </summary>
        ''' <param name="dtl"></param>
        Event ConnectorDisposeEvent(ByVal dtl As INConnectorDetail)

        ''' <summary>
        ''' 初始化通道
        ''' </summary>
        ''' <param name="tsk"></param>
        ''' <param name="serverdtl"></param>
        Public Sub New(tsk As Task(Of IChannel), serverdtl As UDPServerDetail)
            tsk.ContinueWith(AddressOf BindOver)

            _CommandList = Nothing
            _DecompileList = Nothing
            _ActivityCommand = Nothing

            _Detail = serverdtl.Clone
            _IsActivity = True
            _IsForcibly = True

            BroadcastChannel = New Dictionary(Of Integer, UDPServerClientConnector)
            ChildChannel = New Dictionary(Of String, UDPServerClientConnector)
        End Sub


        ''' <summary>
        ''' 绑定完毕
        ''' </summary>
        Private Sub BindOver(t As Task(Of IChannel))
            Threading.Thread.Sleep(20)
            If t.IsCanceled Or t.IsFaulted Then
                _IsActivity = False
                FireConnectorErrorEvent(GetConnectorDetail())
                CloseConnector()
                _Detail = Nothing
            Else
                _IsActivity = True
                _Channel = t.Result
                mHandler = New UDPServerChannelHandler(Me)

                _Channel.Pipeline.AddLast(mHandler)
                FireConnectorConnectedEvent(GetConnectorDetail())
            End If
        End Sub


        ''' <summary>
        ''' 返回本地绑定信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function LocalAddress() As IPDetail
            If _Channel Is Nothing Then Return nothing
            Return New IPDetail(_Channel.LocalAddress)
        End Function

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.UDPServer
        End Function


        ''' <summary>
        ''' 关闭连接
        ''' </summary>
        Public Overrides Sub CloseConnector()
            If _Channel IsNot Nothing Then
                If _Channel.Active Then
                    FireConnectorClosedEvent(GetConnectorDetail())
                    _Channel.CloseAsync() '关闭通道

                End If
            End If
            CloseClientConnector()
            ChildChannel = Nothing
            BroadcastChannel = Nothing

            mHandler?.Release()
            mHandler = Nothing
            _IsActivity = False
            _IsForcibly = False

            _Channel = Nothing
            _Status = ConnectorStatus_Invalid.Invalid
            SetInvalid()
            RaiseEvent ConnectorDisposeEvent(_Detail)
        End Sub

        ''' <summary>
        ''' 关闭所有子节点通道
        ''' </summary>
        Protected Sub CloseClientConnector()
            If ChildChannel Is Nothing Then Return
            BroadcastChannel?.Clear()

            Dim lst = ChildChannel?.ToArray()
            ChildChannel?.Clear()
            If lst Is Nothing Then Return
            For Each kv In lst
                Dim c = kv.Value
                c.Dispose()
            Next
        End Sub

        ''' <summary>
        ''' 给通道添加一个UDP子节点
        ''' </summary>
        Public Function AddClientConnector(ByVal sKey As String, Remote As EndPoint) As UDPServerClientConnector
            Dim client As UDPServerClientConnector = Nothing
            SyncLock Me
                '检查连接通道是否已存在，不存在则重新建立连接
                If Not ChildChannel.ContainsKey(sKey) Then
                    'Trace.WriteLine("UDP连接通道已建立：" & sKey)
                    client = New UDPServerClientConnector(Remote, _Channel, _Detail)
                    ChildChannel.Add(sKey, client)
                    FireClientOnline(client)
                    AddHandler client.ConnectorDisposeEvent, AddressOf ConnectorDisposeEventCallBlack


                    If Remote.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                        Dim oIP As IPEndPoint = Remote
                        If oIP.Address.ToString() = "255.255.255.255" Then
                            BroadcastChannel.Add(oIP.Port, client)
                        End If

                    End If


                    Return client
                End If


            End SyncLock
            Return Nothing
        End Function

        ''' <summary>
        ''' 回调解除通道绑定
        ''' </summary>
        ''' <param name="client"></param>
        Private Sub ConnectorDisposeEventCallBlack(client As UDPServerClientConnector)
            SyncLock Me
                Dim sKey = client.RemoteDetail.ToString()
                RemoveHandler client.ConnectorDisposeEvent, AddressOf ConnectorDisposeEventCallBlack
                '检查连接通道是否已存在，不存在则重新建立连接
                If ChildChannel.ContainsKey(sKey) Then
                    ChildChannel.Remove(sKey)

                    If client.RemoteDetail.Addr = "255.255.255.255" Then
                        If BroadcastChannel.ContainsKey(client.RemoteDetail.Port) Then
                            BroadcastChannel.Remove(client.RemoteDetail.Port)
                        End If

                    End If
                    client.Dispose()
                End If
            End SyncLock

        End Sub

        ''' <summary>
        ''' 获取初始化通道状态
        ''' </summary>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return ConnectorStatus_Bind.Bind
        End Function

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            CloseConnector()

        End Sub

        ''' <summary>
        ''' 获取关于本通道的详情
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorDetail() As INConnectorDetail
            Return _Detail
        End Function

        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetByteBufAllocator() As IByteBufferAllocator
            Return _Channel?.Allocator
        End Function

        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overrides Function WriteByteBuf(buf As IByteBuffer) As Task
            Return Nothing
        End Function

        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetEventLoop() As IEventLoop
            Return _Channel?.EventLoop
        End Function



        ''' <summary>
        ''' 接收到数据事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Protected Friend Sub ChannelRead0(ctx As IChannelHandlerContext, msg As DatagramPacket)
            If _isRelease Then Return

            Dim chl = ctx.Channel
            Dim remote = New IPDetail(msg.Sender)
            Dim sKey = remote.ToString()
            Dim client As UDPServerClientConnector

            Dim buf = msg.Content
            'Dim dmp = New DatagramPacket(buf, msg.Sender)
            'ctx.WriteAndFlushAsync(dmp)

            Try
                '检查是否为广播
                If BroadcastChannel.Count > 0 Then
                    If BroadcastChannel.ContainsKey(remote.Port) Then
                        client = BroadcastChannel(remote.Port)
                        clientchannelRead(buf, client, ctx)
                    End If
                End If
            Catch ex As Exception

            End Try


            Try
                '检查连接通道是否已存在，不存在则重新建立连接
                If ChildChannel.ContainsKey(sKey) Then
                    client = ChildChannel(sKey)
                Else
                    AddClientConnector(sKey, msg.Sender)
                    client = ChildChannel(sKey)
                End If
                clientchannelRead(buf, client, ctx)
            Catch ex As Exception

            End Try

        End Sub

        Protected Sub clientchannelRead(tmpBuf As IByteBuffer, clt As UDPServerClientConnector, tmpctx As IChannelHandlerContext)
            'tmpBuf.Retain()
            'clt.GetEventLoop().Execute(Sub()
            clt.channelRead0(tmpctx, tmpBuf)
            '                           End Sub)
        End Sub

        ''' <summary>
        ''' 自定义用户事件，在这里用于接收超时事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="evt"></param>
        Protected Friend Sub UserEventTriggered(ctx As IChannelHandlerContext, evt As Object)
            Return
        End Sub

    End Class
End Namespace

