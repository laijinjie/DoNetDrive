Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.WebSocket.Client
    Public Class WebsocketClientSocket
        Inherits TcpSocketChannel

        Public WebsocketServerURI As Uri

        Public HandshakeHandler As WebSocketClientHandshakeHandler

    End Class
End Namespace

