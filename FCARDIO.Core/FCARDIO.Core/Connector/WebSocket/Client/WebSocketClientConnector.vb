Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector.Client
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets.Extensions.Compression
Imports DotNetty.Codecs.Http.WebSockets

Namespace Connector.WebSocket.Client
    ''' <summary>
    ''' 用于和TCP Server进行通讯的TCP Client
    ''' </summary>
    Public Class WebSocketClientConnector
        Inherits Connector.TCPClient.TCPClientConnector

        ''' <summary>
        ''' Websocket 地址
        ''' </summary>
        Protected _WebsocketPath As Uri

        ''' <summary>
        ''' Websocket Client 处理器
        ''' </summary>
        Protected _WebsocketHandler As WebSocketClientHandler

        ''' <summary>
        ''' Websocket 地址
        ''' </summary>
        Public ReadOnly Property WebsocketPath As String
            Get
                Return _WebsocketPath.ToString()
            End Get
        End Property

        ''' <summary>
        ''' 初始化TCP客户端对象
        ''' </summary>
        ''' <param name="acr">通道分配器</param>
        ''' <param name="detail">标识此通道的信息类</param>
        Public Sub New(acr As WebSocketClientAllocator, detail As WebSocketClientDetail)
            MyBase.New(acr, detail)
            ThisConnectorDetail = Nothing
            ThisConnectorDetail = New WebSocketClientDetail_Readonly(detail)
        End Sub

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.WebSocketClient
        End Function

#Region "连接服务器"

        ''' <summary>
        ''' 添加通道处理器
        ''' </summary>
        Protected Overrides Sub AddChannelHandler()
            Dim oTCPDTL As WebSocketClientDetail = ThisConnectorDetail
            Dim oSocket As WebsocketClientSocket = _ClientChannel

            _WebsocketHandler = New WebSocketClientHandler(Me)
            oSocket.Pipeline.AddLast(_WebsocketHandler)
            _WebsocketPath = oSocket.WebsocketServerURI


            Dim oHandshakeTask = oSocket.HandshakeHandler.HandshakeCompletion

            If oHandshakeTask.IsCompleted Then
                HandshakeOver(oHandshakeTask)
            Else
                oHandshakeTask.ContinueWith(AddressOf HandshakeOver)
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

            If Not _HandshakeIsCompleted Then Return Nothing
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

        Private _HandshakeIsCompleted As Boolean

        ''' <summary>
        ''' 握手完毕
        ''' </summary>
        ''' <param name="oTsk"></param>
        Private Sub HandshakeOver(oTsk As Task)
            If oTsk.IsCanceled Or oTsk.IsFaulted Then
                CloseConnector()
            Else
                _HandshakeIsCompleted = oTsk.IsCompleted
                ConnectSuccess()
                'Trace.WriteLine("握手成功！")
            End If
        End Sub



#End Region

#Region "接收数据"

        Friend Sub HandleWebSocketFrame(ByVal ctx As IChannelHandlerContext, ByVal frame As WebSocketFrame)
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

            If TypeOf frame Is PongWebSocketFrame Then
                '服务器发来的心跳包回应
                Return
            End If

            If TypeOf frame Is CloseWebSocketFrame Then
                ReadByteBuffer(frame.Content)
                Me._WebsocketHandler.CloseAsync(ctx.Channel)
                Return
            End If
        End Sub
#End Region

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release1()

            If _WebsocketHandler IsNot Nothing Then
                _WebsocketHandler.Release()
            End If
            _WebsocketHandler = Nothing

            MyBase.Release1()
        End Sub


    End Class
End Namespace


