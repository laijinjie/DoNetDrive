Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports DotNetty.Transport.Libuv

Namespace Connector.TCPClient
    Public Class TCPClientAllocator
        Implements INConnectorAllocator

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCPClient的分配器
        ''' </summary>
        Private Shared mTCPClientAllocator As TCPClientAllocator
        ''' <summary>
        ''' 获取用于生成TCPClient的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetAllocator() As TCPClientAllocator
            If mTCPClientAllocator Is Nothing Then
                SyncLock lockobj
                    If mTCPClientAllocator Is Nothing Then
                        mTCPClientAllocator = New TCPClientAllocator(New TCPClientChannelInitializer)
                    End If
                End SyncLock
            End If
            Return mTCPClientAllocator
        End Function

        ''' <summary>
        ''' 最大40秒连接超时，单位毫秒
        ''' </summary>
        Public Shared ReadOnly CONNECT_TIMEOUT_MILLIS_MAX As Integer = 40000
        ''' <summary>
        ''' 最小1秒连接超时，单位毫秒
        ''' </summary>
        Public Shared ReadOnly CONNECT_TIMEOUT_MILLIS_MIN As Integer = 1000
        ''' <summary>
        ''' 最大重新连接次数
        ''' </summary>
        Public Shared ReadOnly CONNECT_RECONNECT_MAX As Integer = 5

        Protected TCPBootstrap As Bootstrap
        Protected TCPInitializer As TCPClientChannelInitializer

        ''' <summary>
        ''' 初始化分配器，建立 Bootstrap ，并分配 EventLoopGroup
        ''' </summary>
        Public Sub New(TCPChannelInitializer As TCPClientChannelInitializer)
            TCPBootstrap = New Bootstrap() '初始化客户端快速构造器
            TCPInitializer = TCPChannelInitializer '初始化通道的初始化工具
            Dim eventgroup = DotNettyAllocator.GetClientEventLoopGroup()
            '设定此 Bootstrap 为 TCP Client 
            With TCPBootstrap
                .Group(eventgroup)
                .Option(ChannelOption.Allocator, DotNettyAllocator.GetBufferAllocator())

                .Channel(Of TcpSocketChannel)()
                '.Option(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default)
                .Option(ChannelOption.SoKeepalive, True)
                .Option(ChannelOption.TcpNodelay, True)
                .Option(ChannelOption.SoReuseaddr, True)
                .Option(ChannelOption.SoRcvbuf, 209600)
                .Option(ChannelOption.SoSndbuf, 102400)
                .Option(ChannelOption.ConnectTimeout, New TimeSpan(0, 0, 0, 0, CONNECT_TIMEOUT_MILLIS_MAX)) '最大连接超时时间
                .Handler(TCPInitializer)
            End With
        End Sub

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Overridable Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            TCPInitializer = Nothing
            Try
                TCPBootstrap = Nothing
            Catch ex As Exception

            End Try
            mTCPClientAllocator = Nothing
            Return
        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "DoNetDrive.Core.Connector.TCPClient.TCPClientConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Overridable Function GetNewConnector(detail As INConnectorDetail) As INConnector Implements INConnectorAllocator.GetNewConnector

            Return New TCPClientConnector(Me, detail)
        End Function

        ''' <summary>
        ''' 连接到远程服务器
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <param name="iTimeOut"></param>
        ''' <returns></returns>
        Public Overridable Function Connect(detail As TCPClientDetail, iTimeOut As Integer) As Task(Of IChannel)
            If TCPBootstrap Is Nothing Then
                Return Nothing
            End If
            TCPBootstrap.Option(ChannelOption.ConnectTimeout, New TimeSpan(0, 0, 0, 0, iTimeOut)) '最大连接超时时间

            Dim oIP As IPAddress = Nothing
            If String.IsNullOrEmpty(detail.LocalAddr) Then
                oIP = IPAddress.Any
            Else
                If Not IPAddress.TryParse(detail.LocalAddr, oIP) Then
                    oIP = IPAddress.Any
                End If
            End If

            TCPBootstrap.LocalAddress(oIP, detail.LocalPort)

            '远端终结点
            Dim oPoint As IPEndPoint



            If IPAddress.TryParse(detail.Addr, oIP) Then
                oPoint = New IPEndPoint(oIP, detail.Port)
            Else
                Try
                    Dim oDNSIP As IPHostEntry = Dns.GetHostEntry(detail.Addr)
                    If oDNSIP.AddressList.Length > 0 Then
                        '获取服务器节点
                        oIP = oDNSIP.AddressList(0)
                    End If
                Catch ex As Exception
                    Throw New ArgumentException($"{detail.Addr} host not find!")
                End Try

                oPoint = New IPEndPoint(oIP, detail.Port)
            End If

            SetTCPInitializerPar(oPoint, detail)

            Return TCPBootstrap.ConnectAsync(oPoint)
        End Function

        ''' <summary>
        ''' 设置初始化参数
        ''' </summary>
        ''' <param name="detail"></param>
        Protected Overridable Sub SetTCPInitializerPar(oRemotePoint As IPEndPoint, detail As TCPClientDetail)
            TCPInitializer.SetSSLPar(oRemotePoint, detail.IsSSL, detail.Certificate, detail.SSLStreamFactory)
        End Sub

    End Class
End Namespace

