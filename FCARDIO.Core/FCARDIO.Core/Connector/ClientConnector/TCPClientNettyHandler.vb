Imports DotNetty.Transport.Channels
Imports DotNetty.Buffers

Namespace Connector.Client
    ''' <summary>
    ''' TCP Client 连接通道的 Netty处理器
    ''' </summary>
    Public Class TCPClientNettyChannelHandler(Of T)
        Inherits SimpleChannelInboundHandler(Of T)
        Private _Client As AbstractNettyClientConnector(Of T)

        Sub New(c As AbstractNettyClientConnector(Of T))
            _Client = c
        End Sub


        ''' <summary>
        ''' 释放关联
        ''' </summary>
        Sub Release()
            _Client = Nothing
        End Sub

        ''' <summary>
        ''' 发生错误时，触发此事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="ex"></param>
        Public Overrides Sub ExceptionCaught(ctx As IChannelHandlerContext, ex As Exception)

            _Client?.ExceptionCaught(ctx, ex)

            MyBase.ExceptionCaught(ctx, ex)
        End Sub

        ''' <summary>
        ''' 接收到数据事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Protected Overrides Sub ChannelRead0(ctx As IChannelHandlerContext, msg As T)
            _Client?.channelRead0(ctx, msg)
        End Sub

        ''' <summary>
        ''' 当通道全部读取完毕后发生的事件
        ''' </summary>
        ''' <param name="context"></param>
        Public Overrides Sub ChannelReadComplete(context As IChannelHandlerContext)
            context.Flush() '刷新缓冲区，开始发送数据
        End Sub


        ''' <summary>
        ''' 不活跃事件，一般就是关闭连接
        ''' </summary>
        ''' <param name="ctx"></param>
        Public Overrides Sub ChannelInactive(ctx As IChannelHandlerContext)

            _Client?.ChannelInactive(ctx)
            MyBase.ChannelInactive(ctx)
        End Sub

        '''' <summary>
        '''' 自定义用户事件，在这里用于接收超时事件
        '''' </summary>
        '''' <param name="ctx"></param>
        '''' <param name="evt"></param>
        'Public Overrides Sub UserEventTriggered(ctx As IChannelHandlerContext, evt As Object)
        '    If _Client IsNot Nothing Then
        '        _Client?.UserEventTriggered(ctx, evt)
        '    End If
        '    MyBase.UserEventTriggered(ctx, evt)
        'End Sub

    End Class

End Namespace
