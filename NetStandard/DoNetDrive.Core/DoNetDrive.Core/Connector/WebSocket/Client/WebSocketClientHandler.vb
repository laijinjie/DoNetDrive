
Imports System.Text
Imports DotNetty.Buffers
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets
Imports DotNetty.Common.Concurrency
Imports DotNetty.Common.Utilities
Imports DotNetty.Transport.Channels

Namespace Connector.WebSocket.Client
    Public Class WebSocketClientHandler
        Inherits SimpleChannelInboundHandler(Of Object)

        Private _Client As WebSocketClientConnector

        Public Sub New(wskClient As WebSocketClientConnector)


            _Client = wskClient
        End Sub

        ''' <summary>
        ''' 释放关联
        ''' </summary>
        Sub Release()
            _Client = Nothing
        End Sub



        ''' <summary>
        ''' 不活跃事件，一般就是关闭连接
        ''' Websocket 断开连接
        ''' </summary>
        ''' <param name="ctx"></param>
        Public Overrides Sub ChannelInactive(ctx As IChannelHandlerContext)
            Try
                If _Client IsNot Nothing Then
                    _Client?.ChannelInactive(ctx)
                End If
            Catch ex As Exception

            End Try

            MyBase.ChannelInactive(ctx)

        End Sub

        Protected Overrides Sub ChannelRead0(ctx As IChannelHandlerContext, msg As Object)
            Dim ch As IChannel = ctx.Channel

            If TypeOf msg Is WebSocketFrame Then
                _Client?.HandleWebSocketFrame(ctx, msg)
                Return
            End If

            If TypeOf msg Is IByteBuffer Then
                _Client?.channelRead0(ctx, TryCast(msg, IByteBuffer))
                Return
            End If
        End Sub


        ''' <summary>
        ''' 当通道全部读取完毕后发生的事件
        ''' </summary>
        ''' <param name="context"></param>
        Public Overrides Sub ChannelReadComplete(context As IChannelHandlerContext)
            context.Flush() '刷新缓冲区，开始发送数据
        End Sub


        ''' <summary>
        ''' 意外发生错误
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="ex"></param>
        Public Overrides Sub ExceptionCaught(ctx As IChannelHandlerContext, ex As Exception)
            If _Client IsNot Nothing Then
                _Client?.ExceptionCaught(ctx, ex)
            End If
            MyBase.ExceptionCaught(ctx, ex)
        End Sub
    End Class
End Namespace

