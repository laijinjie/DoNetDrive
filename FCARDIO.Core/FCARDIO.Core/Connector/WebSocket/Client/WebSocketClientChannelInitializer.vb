

Imports System.Net
Imports DoNetDrive.Core.Connector.TCPClient
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets
Imports DotNetty.Codecs.Http.WebSockets.Extensions.Compression
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Transport.Channels

Namespace Connector.WebSocket.Client
    Friend Class WebSocketClientChannelInitializer
        Inherits Connector.TCPClient.TCPClientChannelInitializer

        Private _WebsocketServerPathList As Dictionary(Of String, Uri)

        Private _WebsocketPath As Uri

        Public Sub New()
            MyBase.New()
            _WebsocketServerPathList = New Dictionary(Of String, Uri)()
        End Sub

        Protected Overrides Sub AddPipeline(channel As IChannel)
            channel.Pipeline.AddLast(New HttpClientCodec(), New HttpObjectAggregator(8192),
                        WebSocketClientCompressionHandler.Instance)

            Dim owebScoket As WebsocketClientSocket = channel
            Dim oRemote As Net.IPEndPoint = channel.RemoteAddress
            Dim sKey = oRemote.ToString()

            Dim oPath As Uri

            If _WebsocketServerPathList.ContainsKey(sKey) Then
                oPath = _WebsocketServerPathList(sKey)
            Else
                oPath = _WebsocketPath
            End If

            Dim oWebsocketHandler = New WebSocketClientHandshakeHandler(
                    DotNetty.Codecs.Http.WebSockets.WebSocketClientHandshakerFactory.NewHandshaker(
                            oPath, WebSocketVersion.V13, Nothing, True, New DefaultHttpHeaders(),
                            5 * 1024 * 1024))

            owebScoket.WebsocketServerURI = _WebsocketPath
            owebScoket.HandshakeHandler = oWebsocketHandler

            channel.Pipeline.AddLast(oWebsocketHandler)


            'MyBase.AddPipeline(channel)
        End Sub

        Friend Sub WebsocketPar(oRemote As IPEndPoint, oUri As Uri)
            Dim sKey = oRemote.ToString()
            If Not _WebsocketServerPathList.ContainsKey(sKey) Then
                _WebsocketServerPathList.Add(sKey, oUri)
            End If

            _WebsocketPath = oUri
        End Sub
    End Class

End Namespace
