
Imports DotNetty.Transport.Channels

Namespace Connector.TCPServer
    Public Interface IDoNetTCPServerChannel
        Inherits IChannel
        ''' <summary>
        ''' 和通道相关联的连接器处理类
        ''' </summary>
        Property ServerConnector As TCPServerConnector
    End Interface

End Namespace
