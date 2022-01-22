
Imports System.Threading
Imports System.Net
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports DoNetDrive.Core.Connector.TCPServer.Client
Imports System.Runtime.InteropServices
Imports System.Net.Sockets
Imports DoNetDrive.Core.Factory

Namespace Connector.TCPServer
    Public Class TCPServerFactory
        Implements INConnectorFactory

        Public Shared SoBacklog As Integer = 200
#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mTCPServerFactory As TCPServerFactory
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As TCPServerFactory
            If mTCPServerFactory Is Nothing Then
                SyncLock lockobj
                    If mTCPServerFactory Is Nothing Then
                        mTCPServerFactory = New TCPServerFactory()
                    End If
                End SyncLock
            End If
            Return mTCPServerFactory
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
        ''' <returns></returns>
        Public Shared Function GetClientKey(local As IPDetail, remote As IPEndPoint, ByRef outClientID As Long) As String
            Dim p As IPAddress = remote.Address
            If p.IsIPv4MappedToIPv6 Then
                p = p.MapToIPv4
            End If

            Dim id = Interlocked.Increment(ClientID)
            outClientID = id
            Dim sKey = $"TCPServer_Local:{local}_Remote:{p}:{remote.Port}_ClientID:{id}"
            Return sKey
        End Function
#End Region


        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function CreateConnector(detail As INConnectorDetail, ConnecterManage As IConnecterManage) As INConnector Implements INConnectorFactory.CreateConnector
            Return New TCPServerConnector(detail, ConnecterManage)
        End Function


        Public Async Function CreateConnectorAsync(detail As INConnectorDetail, ConnecterManage As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim server = New TCPServerConnector(detail, ConnecterManage)
            Await server.ConnectAsync().ConfigureAwait(False)
            Return server
        End Function

    End Class
End Namespace

