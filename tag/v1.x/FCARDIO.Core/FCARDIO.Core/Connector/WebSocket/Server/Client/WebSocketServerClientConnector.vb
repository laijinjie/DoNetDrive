Imports System.Text
Imports DotNetty.Buffers
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets
Imports DotNetty.Common.Utilities
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Connector.Client
Imports DoNetDrive.Core.Connector.TCPServer

Namespace Connector.WebSocket.Server.Client
    ''' <summary>
    ''' 表示一个WebSocket Server下的客户端节点连接通道
    ''' </summary>
    Friend Class WebSocketServerClientConnector
        Inherits AbstractNettyServerClientConnector(Of Object)

        ''' <summary>
        ''' 从 HTTP 到 WebSocket 协议升级处理器
        ''' </summary>
        Protected _Handshaker As WebSocketServerHandshaker

        ''' <summary>
        ''' 检查握手是否成功
        ''' </summary>
        Protected _HandshakeIsCompleted As Boolean

        ''' <summary>
        ''' 本次请求的URL地址参数
        ''' </summary>
        Public RequestURL As String

        ''' <summary>
        ''' 创建一个客户端
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="channel"></param>
        Public Sub New(sKey As String, channel As IChannel, ByVal iClientID As Long)
            MyBase.New(sKey, channel, iClientID)
            _HandshakeIsCompleted = False
        End Sub

#Region "接收数据"
        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Friend Overrides Sub channelRead0(ctx As IChannelHandlerContext, msg As Object)
            If TypeOf msg Is IFullHttpRequest Then
                HandleHttpRequest(ctx, msg)
                Return
            End If

            If TypeOf msg Is WebSocketFrame Then
                HandleWebSocketFrame(ctx, msg)
                Return
            End If

            If TypeOf msg Is IByteBuffer Then
                ReadByteBuffer(TryCast(msg, IByteBuffer))
                Return
            End If

        End Sub
#End Region

#Region "检查 WebSocket Http连接握手"

        ''' <summary>
        ''' 处理 HTTP 握手请求
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="req"></param>
        Private Sub HandleHttpRequest(ByVal ctx As IChannelHandlerContext, ByVal req As IFullHttpRequest)
            If Not req.Result.IsSuccess Then
                SendHttpResponse(ctx, req, New DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.BadRequest))
                Return
            End If

            If Not Equals(req.Method, HttpMethod.[Get]) Then
                SendHttpResponse(ctx, req, New DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.Forbidden))
                Return
            End If

            'If "/".Equals(req.Uri) Then
            '    SendHttpResponse(ctx, req, New DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.NotFound))
            '    Return
            'End If

            If "/favicon.ico".Equals(req.Uri) Then
                SendHttpResponse(ctx, req, New DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.NotFound))
                Return
            End If

            RequestURL = req.Uri
            Dim dtl = TryCast(GetConnectorDetail(), WebSocketServerClientDetail)
            dtl.RequestURL = req.Uri

            Dim wsFactory = New WebSocketServerHandshakerFactory(GetWebSocketLocation(req), Nothing, True, 5 * 1024 * 1024)
            Me._Handshaker = wsFactory.NewHandshaker(req)
            '握手完成
            If Me._Handshaker Is Nothing Then
                WebSocketServerHandshakerFactory.SendUnsupportedVersionResponse(ctx.Channel)
            Else
                Me._Handshaker.HandshakeAsync(ctx.Channel, req).ContinueWith(AddressOf HandshakeOver)
            End If
        End Sub

        ''' <summary>
        ''' 握手完毕
        ''' </summary>
        ''' <param name="oTsk"></param>
        Private Sub HandshakeOver(oTsk As Task)
            Dim pnl As IDoNetTCPServerChannel = TryCast(_ClientChannel.Parent, IDoNetTCPServerChannel）
            If oTsk.IsCanceled Or oTsk.IsFaulted Then
                pnl.ServerConnector.FireConnectorErrorEvent(GetConnectorDetail())
            Else
                _HandshakeIsCompleted = oTsk.IsCompleted
                pnl.ServerConnector.FireClientOnline(Me)
                'Trace.WriteLine("握手成功！")
            End If
        End Sub

        ''' <summary>
        ''' 处理 Websocket  数据包
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="frame"></param>
        Private Sub HandleWebSocketFrame(ByVal ctx As IChannelHandlerContext, ByVal frame As WebSocketFrame)
            If TypeOf frame Is CloseWebSocketFrame Then
                Me._Handshaker.CloseAsync(ctx.Channel, CType(frame.Retain(), CloseWebSocketFrame))
                Return
            End If

            If TypeOf frame Is PingWebSocketFrame Then
                ctx.WriteAsync(New PongWebSocketFrame(CType(frame.Content.Retain(), IByteBuffer)))
                Return
            End If

            If TypeOf frame Is TextWebSocketFrame Then
                ReadByteBuffer(frame.Content)
                Return
            End If

            If TypeOf frame Is BinaryWebSocketFrame Then
                ReadByteBuffer(frame.Content)
            End If

            If TypeOf frame Is ContinuationWebSocketFrame Then '接续包
                ReadByteBuffer(frame.Content)
            End If
        End Sub

        ''' <summary>
        ''' 发送HTTP消息
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="req"></param>
        ''' <param name="res"></param>
        Private Shared Sub SendHttpResponse(ByVal ctx As IChannelHandlerContext, ByVal req As IFullHttpRequest, ByVal res As IFullHttpResponse)
            If res.Status.Code <> 200 Then
                Dim buf As IByteBuffer = Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes(res.Status.ToString()))
                res.Content.WriteBytes(buf)
                buf.Release()
                HttpUtil.SetContentLength(res, res.Content.ReadableBytes)
            End If

            Dim task As Task = ctx.Channel.WriteAndFlushAsync(res)

            If Not HttpUtil.IsKeepAlive(req) OrElse res.Status.Code <> 200 Then
                task.ContinueWith(Function(t, c) (CType(c, IChannelHandlerContext)).CloseAsync(), ctx, TaskContinuationOptions.ExecuteSynchronously)
            End If
        End Sub

        ''' <summary>
        ''' 生成WebSocket的有效URL路径
        ''' </summary>
        ''' <param name="req"></param>
        ''' <returns></returns>
        Private Function GetWebSocketLocation(ByVal req As IFullHttpRequest) As String
            Dim value As ICharSequence = Nothing
            Dim result As Boolean = req.Headers.TryGet(HttpHeaderNames.Host, value)
            Dim location As String = String.Empty
            If result Then location = value.ToString()


            '取 WebSocket 地址
            Dim pnl As IDoNetTCPServerChannel = TryCast(_ClientChannel.Parent, IDoNetTCPServerChannel）
            Dim server As WebSocketServerConnector = TryCast(pnl.ServerConnector, WebSocketServerConnector)
            Dim oDetail As WebSocketServerDetail = server.GetConnectorDetail()

            location = location + oDetail.WebsocketPath

            If oDetail.IsSSL Then
                Return "wss://" & location
            Else
                Return "ws://" & location
            End If
        End Function

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
            If Not _HandshakeIsCompleted Then Return Nothing
            If _ClientChannel Is Nothing Then
                Return Nothing
            End If
            Dim IsUseBinary As Boolean = (TypeOf _ActivityCommand IsNot Command.Text.TextCommand)


            If TypeOf buf Is WebsocketTextBuffer Then
                IsUseBinary = False
                buf = TryCast(buf, WebsocketTextBuffer).GetBuf()
            End If

            UpdateActivityTime()
            DisposeResponse(buf)


            If IsUseBinary Then
                Return _ClientChannel?.WriteAndFlushAsync(New BinaryWebSocketFrame(buf))
            Else
                Return _ClientChannel?.WriteAndFlushAsync(New TextWebSocketFrame(buf))
            End If
        End Function
#End Region

        ''' <summary>
        ''' 创建一个连接头像详情对象，包含用于描述当前连接通道的信息
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetConnectorDetail0() As INConnectorDetail
            Return New WebSocketServerClientDetail(mKey, _ClientChannel.RemoteAddress， _ClientChannel.LocalAddress, ClientID)
        End Function


        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.WebSocketServerClient
        End Function
    End Class

End Namespace
