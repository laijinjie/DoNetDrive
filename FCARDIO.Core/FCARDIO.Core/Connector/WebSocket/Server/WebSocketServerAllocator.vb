Imports System.Threading
Imports System.Net
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports DoNetDrive.Core.Connector.WebSocket.Server.Client
Imports DotNetty.Transport.Libuv
Imports System.Runtime.InteropServices

Namespace Connector.WebSocket.Server
    ''' <summary>
    ''' WebSocket 服务器分配器
    ''' </summary>
    Friend Class WebSocketServerAllocator
        Implements INConnectorAllocator

#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成WebSocket服务器的分配器
        ''' </summary>
        Private Shared mWebSocketServerAllocator As WebSocketServerAllocator

        ''' <summary>
        ''' 获取用于生成WebSocket服务器的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetAllocator() As WebSocketServerAllocator
            If mWebSocketServerAllocator Is Nothing Then
                SyncLock lockobj
                    If mWebSocketServerAllocator Is Nothing Then
                        mWebSocketServerAllocator = New WebSocketServerAllocator()
                    End If
                End SyncLock
            End If
            Return mWebSocketServerAllocator
        End Function
#End Region

#Region "获取客户端的Key"
        ''' <summary>
        ''' 所有客户端的ID
        ''' </summary>
        Protected Shared ClientID As Long

        ''' <summary>
        ''' 获取客户端的Key值
        ''' </summary>
        ''' <param name="channel"></param>
        ''' <returns></returns>
        Public Shared Function GetClientKey(channel As IChannel, ByRef iClientID As Long) As String
            Dim local As String = New IPDetail(channel.LocalAddress).ToString()
            Dim remote As IPEndPoint = TryCast(channel.RemoteAddress, IPEndPoint)
            Dim p As IPAddress = remote.Address
            If p.IsIPv4MappedToIPv6 Then
                p = p.MapToIPv4
            End If

            Dim id = Interlocked.Increment(ClientID)
            iClientID = id
            local = $"WebSocketServer:{local}_Remote:{p.ToString()}:{remote.Port}_ClientID:{id}"
            Return local
        End Function
#End Region




        ''' <summary>
        ''' 服务器快速启动器
        ''' </summary>
        Protected mServerBootstrap As ServerBootstrap

        ''' <summary>
        ''' 用于管理tcp客户端创建的处理程序
        ''' </summary>
        Protected mWebSocketClientHandler As WebSocketServerClientChannelInitializer

        ''' <summary>
        ''' 类初始化，初始化 ServerBootstrap
        ''' </summary>
        Private Sub New()
            mServerBootstrap = New ServerBootstrap
            Dim serverGroup As IEventLoopGroup = DotNettyAllocator.GetServerParentEventLoopGroup()
            Dim ClientGroup As IEventLoopGroup = DotNettyAllocator.GetServerChildEventLoopGroup()

            With mServerBootstrap
                .Group(serverGroup, ClientGroup)


                'If DotNettyAllocator.UseLibuv Then
                '    .Channel(Of Connector.TCPServer.TcpServerSocketChannelLibuv)()
                '    If (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Or
                '        RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Then
                '        .Option(ChannelOption.SoReuseport, True)
                '        .ChildOption(ChannelOption.SoReuseaddr, True)
                '    End If
                'Else
                .Channel(Of Connector.TCPServer.TcpServerSocketChannelEx)()
                'End If


                .Option(ChannelOption.Allocator, DotNettyAllocator.GetBufferAllocator())
                .Option(ChannelOption.SoBacklog, 8192)

                mWebSocketClientHandler = New WebSocketServerClientChannelInitializer()
                .ChildHandler(mWebSocketClientHandler)
            End With
        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "DoNetDrive.Core.Connector.WebSocket.WebSocketServerConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function GetNewConnector(detail As INConnectorDetail) As INConnector Implements INConnectorAllocator.GetNewConnector
            Dim serverDtl As WebSocketServerDetail = TryCast(detail, WebSocketServerDetail)
            Dim oIP As IPAddress = Nothing
            If String.IsNullOrEmpty(serverDtl.LocalAddr) Then
                oIP = IPAddress.Any
            Else
                If Not IPAddress.TryParse(serverDtl.LocalAddr, oIP) Then
                    Throw New ArgumentException("LocalAddr Is Error")
                End If
            End If
            '开始绑定本地端口
            Dim tsk = mServerBootstrap.BindAsync(oIP, serverDtl.LocalPort)
            Dim server = New WebSocketServerConnector(tsk, detail)
            Return server

        End Function

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            mServerBootstrap = Nothing
            mWebSocketClientHandler = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
