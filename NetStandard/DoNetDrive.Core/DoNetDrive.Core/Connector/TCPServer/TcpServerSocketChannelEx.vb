

Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.TCPServer
    ''' <summary>
    ''' 处理服务器连接的通道类
    ''' </summary>
    Public Class TcpServerSocketChannelEx
        Inherits TcpServerSocketChannel
        Implements IDoNetTCPServerChannel

        Public Property ServerConnector As TCPServerConnector Implements IDoNetTCPServerChannel.ServerConnector
    End Class
End Namespace

