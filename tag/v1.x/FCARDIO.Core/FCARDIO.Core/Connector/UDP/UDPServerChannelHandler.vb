Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' UDP Server的 Netty 通道处理器
    ''' </summary>
    Public Class UDPServerChannelHandler
        Inherits SimpleChannelInboundHandler(Of DatagramPacket)
        Private _Server As UDPServerConnector

        Sub New(c As UDPServerConnector)
            _Server = c
        End Sub

        ''' <summary>
        ''' 释放关联
        ''' </summary>
        Sub Release()
            _Server = Nothing
        End Sub


        ''' <summary>
        ''' 接收到数据事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Protected Overrides Sub ChannelRead0(ctx As IChannelHandlerContext, msg As DatagramPacket)
            If _Server Is Nothing Then Return
            _Server.ChannelRead0(ctx, msg)
        End Sub


        ''' <summary>
        ''' 自定义用户事件，在这里用于接收超时事件
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="evt"></param>
        Public Overrides Sub UserEventTriggered(ctx As IChannelHandlerContext, evt As Object)
            If _Server IsNot Nothing Then
                _Server.UserEventTriggered(ctx, evt)
            End If
            MyBase.UserEventTriggered(ctx, evt)
        End Sub

    End Class
End Namespace

