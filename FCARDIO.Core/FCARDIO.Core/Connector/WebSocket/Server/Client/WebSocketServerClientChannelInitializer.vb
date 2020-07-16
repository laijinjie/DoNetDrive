Imports System.Net
Imports System.Threading
Imports DotNetty.Codecs.Http
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Handlers.Tls
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Connector.TCPServer

Namespace Connector.WebSocket.Server.Client
    ''' <summary>
    ''' WebSocket Server下的 客户端通道初始化类，在此类中初始化  TCPServer.Client.TCPClientConnector 类
    ''' </summary>
    Public Class WebSocketServerClientChannelInitializer
        Inherits ChannelInitializer(Of IChannel)

        ''' <summary>
        ''' 初始化服务器客户端(子节点)
        ''' </summary>
        ''' <param name="channel"></param>
        Protected Overrides Sub InitChannel(channel As IChannel)
            channel.Pipeline().AddLast(New IdleStateHandler(20, 20, 0)) '超时检查
            Dim serverAddress = channel.Parent.LocalAddress
            Dim pnl As TcpServerSocketChannelEx = TryCast(channel.Parent, TcpServerSocketChannelEx）
            If channel.Allocator Is pnl.Allocator Then

            Else
                channel.Configuration.Allocator = pnl.Allocator

            End If
            If pnl IsNot Nothing Then
                If pnl.ServerConnector Is Nothing Then
                    channel.CloseAsync()
                    Return
                End If

                Dim sKey As String = WebSocketServerAllocator.GetClientKey(channel)
                Dim ServerConnt = pnl.ServerConnector
                Dim ServerDetail = TryCast(ServerConnt.GetConnectorDetail(), TCPServer.TCPServerDetail)
                If ServerDetail.IsSSL Then
                    Dim sslpar = New ServerTlsSettings(ServerDetail.Certificate, False, False,
                                    System.Security.Authentication.SslProtocols.Tls12) 'tls1.2
                    If ServerDetail.SSLStreamFactory Is Nothing Then
                        channel.Pipeline().AddLast(New TlsHandler(sslpar))
                    Else
                        channel.Pipeline().AddLast(New TlsHandler(ServerDetail.SSLStreamFactory, sslpar))
                    End If

                End If
                channel.Pipeline().AddLast(New HttpServerCodec())
                channel.Pipeline().AddLast(New HttpObjectAggregator(65536))

                Dim conn As WebSocketServerClientConnector = New WebSocketServerClientConnector(sKey, channel)
                pnl.ServerConnector.FireClientOnline(conn)
            Else
                channel.CloseAsync()
            End If
        End Sub
    End Class

End Namespace
