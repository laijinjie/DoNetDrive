Imports System.Text
Imports DotNetty.Buffers
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets
Imports DotNetty.Common.Concurrency
Imports DotNetty.Common.Utilities
Imports DotNetty.Transport.Channels

Namespace Connector.WebSocket.Client
    Public Class WebSocketClientHandshakeHandler
        Inherits SimpleChannelInboundHandler(Of IFullHttpResponse)

        Public ReadOnly handshaker As WebSocketClientHandshaker
        Public ReadOnly completionSource As TaskCompletionSource

        Public Sub New(oHandshaker As WebSocketClientHandshaker)
            handshaker = oHandshaker
            completionSource = New TaskCompletionSource()
        End Sub

        ''' <summary>
        ''' 指示Websocket Client 握手是否完成
        ''' </summary>
        ''' <returns></returns>
        Public Function HandshakeCompletion() As Task
            Return completionSource.Task
        End Function

        ''' <summary>
        ''' Websocket 连接成功，握手完毕
        ''' </summary>
        ''' <param name="ctx"></param>
        Public Overrides Sub ChannelActive(ctx As IChannelHandlerContext)
            handshaker.HandshakeAsync(ctx.Channel).LinkOutcome(completionSource)
        End Sub


        Protected Overrides Sub ChannelRead0(ctx As IChannelHandlerContext, msg As IFullHttpResponse)
            Dim ch As IChannel = ctx.Channel
            If Not handshaker.IsHandshakeComplete Then
                Try
                    handshaker.FinishHandshake(ch, msg)
                    'Console.WriteLine("WebSocket Client connected!");
                    completionSource.TryComplete()

                Catch e As WebSocketHandshakeException

                    'Console.WriteLine("WebSocket Client failed to connect")
                    completionSource.TrySetException(e)
                End Try

                Return
            End If


            completionSource.TrySetException(New Exception(msg.Content.ToString(Encoding.UTF8)))
            ctx.CloseAsync()


        End Sub


        ''' <summary>
        ''' 意外发生错误
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="ex"></param>
        Public Overrides Sub ExceptionCaught(ctx As IChannelHandlerContext, ex As Exception)
            completionSource.TrySetException(ex)
            MyBase.ExceptionCaught(ctx, ex)
        End Sub
    End Class
End Namespace

