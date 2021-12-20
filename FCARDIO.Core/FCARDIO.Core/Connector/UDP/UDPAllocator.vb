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
        ''' 创建一个UDP分配器
        ''' </summary>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "UDP.UDPClientConnector"
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
                    Dim oIP As IPAddress = GetIPAddress(clientdtl.Addr)
                    Dim oEnd As EndPoint
                    oEnd = New IPEndPoint(oIP, clientdtl.Port)
                    sKey = (New IPDetail(oEnd)).ToString()
                    Return UDPServer.AddClientConnector(sKey, oEnd)
                Else
                    '需要先创建UDP服务器
                    Return GetNewUDPServer(serverdtl, clientdtl)
                End If
            End If
        End Function



        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Async Function GetNewConnectorAsync(detail As INConnectorDetail) As Task(Of INConnector) Implements INConnectorAllocator.GetNewConnectorAsync
            Dim clientdtl As UDPClientDetail = TryCast(detail, UDPClientDetail)
            Dim serverdtl As UDPServerDetail

            If clientdtl Is Nothing Then
                'UDP服务器模式
                Dim conn = GetNewUDPServer(serverdtl)
                Await conn.ConnectAsync
                mServerList.Add(serverdtl.GetKey, conn)
                Return conn
            Else
                serverdtl = New UDPServerDetail(clientdtl.LocalAddr, clientdtl.LocalPort)
                Dim sKey = serverdtl.GetKey()
                If mServerList.ContainsKey(sKey) Then
                    Dim conn = mServerList(sKey)
                    Return conn.AddClientConnector(clientdtl)
                Else
                    '需要先创建UDP服务器
                    Dim conn = GetNewUDPServer(serverdtl)
                    Await conn.ConnectAsync
                    mServerList.Add(serverdtl.GetKey, conn)
                    Dim clientConn = conn.AddClientConnector(clientdtl)
                    Return clientConn
                End If
            End If
        End Function

        ''' <summary>
        ''' 创建一个UDP服务器通道
        ''' </summary>
        ''' <param name="serverdtl"></param>
        ''' <returns></returns>
        Private Function GetNewUDPServer(serverdtl As UDPServerDetail, clientdtl As UDPClientDetail) As UDPServerConnector
            If serverdtl Is Nothing Then Return Nothing

            Dim conn = New UDPServerConnector(serverdtl, clientdtl)
            conn.BindOverCallblack = AddressOf BindOverCallblack
            conn.BindClosedCallblack = AddressOf BindClosedCallblack
            Return conn
        End Function
        Private Sub BindOverCallblack(ByVal conn As INConnector)
            Dim sKey As String = conn.GetKey()
            If Not mServerList.ContainsKey(sKey) Then
                mServerList.Add(sKey, conn)
            End If
        End Sub


        Private Sub BindClosedCallblack(ByVal conn As INConnector)
            Dim sKey As String = conn.GetKey()
            Dim udp As UDPServerDetail = conn

            If mServerList.ContainsKey(sKey) Then
                mServerList.Remove(sKey)
            End If
        End Sub


        Public Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            Return
        End Sub
    End Class
End Namespace


