Imports System.Net
Imports System.Threading
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Handlers.Tls
Imports DotNetty.Transport.Channels

Namespace Connector.TCPServer.Client
    ''' <summary>
    ''' TCP 客户端通道初始化类，在此类中初始化  TCPServer.Client.TCPClientConnector 类
    ''' </summary>
    Public Class TCPClientChannelInitializer
        Inherits ChannelInitializer(Of IChannel)

        ''' <summary>
        ''' 初始化服务器客户端(子节点)
        ''' </summary>
        ''' <param name="channel"></param>
        Protected Overrides Sub InitChannel(channel As IChannel)
            channel.Pipeline().AddLast(New IdleStateHandler(120, 120, 0)) '超时检查

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
                Dim iClientID As Long = 0
                Dim sKey As String = TCPServerAllocator.GetClientKey(channel, iClientID)
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
                Dim conn As TCPClientConnector = New TCPClientConnector(sKey, channel, iClientID)
                pnl.ServerConnector.FireClientOnline(conn)
            Else
                channel.CloseAsync()
            End If
        End Sub
    End Class

End Namespace
