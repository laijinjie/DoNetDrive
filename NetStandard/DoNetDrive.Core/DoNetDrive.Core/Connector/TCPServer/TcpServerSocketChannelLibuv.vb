
Imports DotNetty.Transport.Libuv

Namespace Connector.TCPServer
    ''' <summary>
    ''' 处理服务器连接的通道类
    ''' </summary>
    Public Class TcpServerSocketChannelLibuv
        Inherits TcpServerChannel
        Implements IDoNetTCPServerChannel

        Public Property ServerConnector As TCPServerConnector Implements IDoNetTCPServerChannel.ServerConnector

    End Class
End Namespace
