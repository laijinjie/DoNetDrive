Imports System.Net
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' 创建分配一个UDP连接通道
    ''' </summary>
    Friend Class UDPAllocator
        Implements INConnectorAllocator

#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mAllocator As UDPAllocator
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetAllocator() As UDPAllocator
            If mAllocator Is Nothing Then
                SyncLock lockobj
                    If mAllocator Is Nothing Then
                        mAllocator = New UDPAllocator()
                    End If
                End SyncLock
            End If
            Return mAllocator
        End Function
#End Region
        ''' <summary>
        ''' 保存有UDPServer的服务器列表
        ''' </summary>
        Protected mServerList As Dictionary(Of String, UDPServerConnector)

        ''' <summary>
        ''' 用于创建UDP通道的快速启动器
        ''' </summary>
        Protected mUDPBootstrap As Bootstrap

        ''' <summary>
        ''' 创建一个UDP分配器
        ''' </summary>
        Private Sub New()
            mServerList = New Dictionary(Of String, UDPServerConnector)
            mUDPBootstrap = New Bootstrap
            Dim eventgroup = DotNettyAllocator.GetClientEventLoopGroup()
            With mUDPBootstrap
                .Group(eventgroup)
                .Channel(Of SocketDatagramChannel)()
                .Option(ChannelOption.SoBroadcast, True)
                .Option(ChannelOption.TcpNodelay, True)
                .Option(ChannelOption.SoReuseaddr, True)
                .Option(ChannelOption.Allocator, DotNettyAllocator.GetBufferAllocator())
                .Handler(New ActionChannelInitializer(Of IChannel)(Sub(x) x.Pipeline.AddLast(New IdleStateHandler(50, 50, 0))))
            End With
        End Sub

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            Try
                mUDPBootstrap = Nothing
            Catch ex As Exception

            End Try
            mAllocator = Nothing
        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "DoNetDrive.Core.Connector.UDP.UDPClientConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function GetNewConnector(detail As INConnectorDetail) As INConnector Implements INConnectorAllocator.GetNewConnector
            Dim clientdtl As UDPClientDetail = TryCast(detail, UDPClientDetail)
            Dim serverdtl As UDPServerDetail

            If clientdtl Is Nothing Then
                'UDP服务器模式
                serverdtl = TryCast(detail, UDPServerDetail)
                Return GetNewUDPServer(serverdtl)
            Else
                serverdtl = New UDPServerDetail(clientdtl.LocalAddr, clientdtl.LocalPort)
                Dim UDPServer As UDPServerConnector
                Dim sKey = serverdtl.GetKey()
                If mServerList.ContainsKey(sKey) Then
                    UDPServer = mServerList(sKey)
                Else
                    '需要先创建UDP服务器
                    Throw New ArgumentException($"Not find udp server [{clientdtl.LocalAddr}:{clientdtl.LocalPort}]")
                End If


                Dim oIP As IPAddress = GetIPAddress(clientdtl.Addr)
                Dim oEnd As EndPoint
                oEnd = New IPEndPoint(oIP, clientdtl.Port)
                sKey = (New IPDetail(oEnd)).ToString()

                Return UDPServer.AddClientConnector(sKey, oEnd)
            End If

        End Function

        ''' <summary>
        ''' 创建一个UDP服务器通道
        ''' </summary>
        ''' <param name="dtl"></param>
        ''' <returns></returns>
        Private Function GetNewUDPServer(dtl As UDPServerDetail) As UDPServerConnector
            If dtl Is Nothing Then Return Nothing
            Dim sKey = dtl.GetKey()

            If mServerList.ContainsKey(sKey) Then
                Return mServerList(sKey)
            End If

            SyncLock Me

                If mServerList.ContainsKey(sKey) Then
                    Return mServerList(sKey)
                End If

                Dim oIP As IPAddress
                oIP = GetIPAddress(dtl.LocalAddr)
                Dim oEndPoint As EndPoint = New IPEndPoint(oIP, dtl.LocalPort)
                '开始绑定本地端口
                Dim tsk = mUDPBootstrap.BindAsync(oEndPoint)
                Dim conn = New UDPServerConnector(tsk, dtl)
                mServerList.Add(sKey, conn)
                AddHandler conn.ConnectorDisposeEvent, AddressOf ConnectorDisposeEventHandler
                Return conn
            End SyncLock

        End Function

        Private Sub ConnectorDisposeEventHandler(ByVal dtl As INConnectorDetail)
            Dim sKey As String = dtl.GetKey()
            If Not mServerList.ContainsKey(sKey) Then
                Return
            End If
            SyncLock Me
                If Not mServerList.ContainsKey(sKey) Then
                    Return
                End If
                RemoveHandler mServerList(sKey).ConnectorDisposeEvent, AddressOf ConnectorDisposeEventHandler
                mServerList.Remove(sKey)

            End SyncLock
        End Sub


        ''' <summary>
        ''' 将一个地址转换为IPAddress
        ''' </summary>
        ''' <returns></returns>
        Private Function GetIPAddress(ByVal addr As String) As IPAddress
            Dim oIP As IPAddress = IPAddress.Any
            If String.IsNullOrEmpty(addr) Then
                Return oIP
            Else
                If Not IPAddress.TryParse(addr, oIP) Then
                    Throw New ArgumentException("LocalAddr Is Error")
                End If
            End If
            Return oIP
        End Function
    End Class
End Namespace


