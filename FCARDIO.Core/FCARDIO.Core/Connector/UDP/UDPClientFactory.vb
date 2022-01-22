
Imports System.Net
Imports DoNetDrive.Core.Factory
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' 创建一个UDP客户端连接通道
    ''' </summary>
    Friend Class UDPClientFactory
        Implements INConnectorFactory



#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mUDPClientFactory As UDPClientFactory
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As UDPClientFactory
            If mUDPClientFactory Is Nothing Then
                SyncLock lockobj
                    If mUDPClientFactory Is Nothing Then
                        mUDPClientFactory = New UDPClientFactory()
                    End If
                End SyncLock
            End If
            Return mUDPClientFactory
        End Function
#End Region

        ''' <summary>
        ''' 创建一个UDP分配器
        ''' </summary>
        Private Sub New()
            '
        End Sub

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function CreateConnector(detail As INConnectorDetail, ConnecterManage As IConnecterManage) As INConnector Implements INConnectorFactory.CreateConnector


            Dim conn = CreateClient(detail, ConnecterManage)
            Return conn
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Async Function CreateConnectorAsync(detail As INConnectorDetail, ConnecterManage As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim conn = CreateClient(detail, ConnecterManage)
            Return Await Task.FromResult(conn)
        End Function



        ''' <summary>
        ''' 创建一个UDP数据通道
        ''' </summary>
        ''' <param name="clientdtl"></param>
        ''' <returns></returns>
        Private Function CreateClient(clientdtl As UDPClientDetail, ConnecterManage As IConnecterManage) As UDPServerClientConnector
            Dim serverdtl = New UDPServerDetail(clientdtl.LocalAddr, clientdtl.LocalPort)
            Dim UDPServer As UDPServerConnector
            Dim sKey = serverdtl.GetKey()
            UDPServer = ConnecterManage.GetConnector(sKey)
            If UDPServer IsNot Nothing Then

                Return CreateClientByUDPServer(clientdtl, UDPServer)
            Else
                '需要先创建UDP服务器
                UDPServer = New UDPServerConnector(serverdtl, ConnecterManage)
                Dim udpBindOver = False

                Dim task = UDPServer.ConnectAsync().ContinueWith(Sub(t) udpBindOver = True).ConfigureAwait(False)
                Do
                    Threading.Thread.Sleep(5)
                Loop While Not udpBindOver
                If UDPServer.GetStatus().Status = "Bind" Then
                    ConnecterManage.AddConnector(serverdtl.GetKey(), UDPServer)
                    Return CreateClientByUDPServer(clientdtl, UDPServer)
                Else
                    Throw New Exception("UDP Bind error")
                End If
            End If
        End Function


        Private Function CreateClientByUDPServer(clientdtl As UDPClientDetail, UDPServer As UDPServerConnector) As UDPServerClientConnector
            Dim oRemotePoint As IPEndPoint
            Dim oIP As IPAddress = Nothing

            If IPAddress.TryParse(clientdtl.Addr, oIP) Then
                oRemotePoint = New IPEndPoint(oIP, clientdtl.Port)
            Else
                Dim oDNSIP As IPHostEntry = Dns.GetHostEntry(clientdtl.Addr)
                If oDNSIP.AddressList.Length > 0 Then
                    '获取服务器节点
                    oIP = oDNSIP.AddressList(0)
                End If

                oRemotePoint = New IPEndPoint(oIP, clientdtl.Port)
            End If

            Return UDPServer.AddClientConnector(oRemotePoint)
        End Function



    End Class
End Namespace


