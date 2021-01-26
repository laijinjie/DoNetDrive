Imports System.Net
Imports DoNetDrive.Core.Connector.TCPClient
Imports DotNetty.Buffers
Imports DotNetty.Codecs.Http
Imports DotNetty.Codecs.Http.WebSockets
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.WebSocket.Client
    Public Class WebSocketClientAllocator
        Inherits Connector.TCPClient.TCPClientAllocator

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成WebSocketClient的分配器
        ''' </summary>
        Private Shared mWebSocketClientAllocator As WebSocketClientAllocator

        ''' <summary>
        ''' 获取用于生成WebSocketClient的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overloads Shared Function GetAllocator() As WebSocketClientAllocator
            If mWebSocketClientAllocator Is Nothing Then
                SyncLock lockobj
                    If mWebSocketClientAllocator Is Nothing Then
                        mWebSocketClientAllocator = New WebSocketClientAllocator()
                    End If
                End SyncLock
            End If
            Return mWebSocketClientAllocator
        End Function


        ''' <summary>
        ''' 初始化分配器，建立 Bootstrap ，并分配 EventLoopGroup
        ''' </summary>
        Public Sub New()
            MyBase.New(New WebSocketClientChannelInitializer())
            TCPBootstrap.Channel(Of WebsocketClientSocket)()
        End Sub

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Overrides Sub shutdownGracefully()
            TCPInitializer = Nothing
            Try
                TCPBootstrap = Nothing
            Catch ex As Exception

            End Try
            mWebSocketClientAllocator = Nothing
            Return
        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorTypeName() As String
            Return "DoNetDrive.Core.Connector.WebSocket.Client.WebSocketClientConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Overrides Function GetNewConnector(detail As INConnectorDetail) As INConnector
            Return New WebSocketClientConnector(Me, detail)
        End Function

        ''' <summary>
        ''' 设置初始化参数
        ''' </summary>
        ''' <param name="detail"></param>
        Protected Overrides Sub SetTCPInitializerPar(oRemotePoint As IPEndPoint, detail As TCPClientDetail)
            MyBase.SetTCPInitializerPar(oRemotePoint, detail)

            Dim oWebsocketIni As WebSocketClientChannelInitializer = TCPInitializer
            Dim oWebsocketDTL As WebSocketClientDetail = detail

            Dim Scheme As String = "ws"
            If oWebsocketDTL.IsSSL Then Scheme = "wss"

            Dim builder = New UriBuilder With {
                .Scheme = Scheme,
                .Host = oWebsocketDTL.RemoteHost,
                .Port = oWebsocketDTL.Port
            }
            If Not String.IsNullOrEmpty(oWebsocketDTL.Path) Then
                builder.Path = oWebsocketDTL.Path
            End If

            oWebsocketIni.WebsocketPar(oRemotePoint, builder.Uri)

        End Sub

    End Class
End Namespace

