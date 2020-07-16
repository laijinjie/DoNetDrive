

Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.TCPServer
    ''' <summary>
    ''' 处理服务器连接的通道类
    ''' </summary>
    Public Class TcpServerSocketChannelEx
        Inherits TcpServerSocketChannel
        ''' <summary>
        ''' 和通道相关联的连接器处理类
        ''' </summary>
        Public ServerConnector As TCPServerConnector



    End Class
End Namespace

