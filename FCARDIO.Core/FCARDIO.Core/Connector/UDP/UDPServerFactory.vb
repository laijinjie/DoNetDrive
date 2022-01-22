Imports System.Net
Imports DoNetDrive.Core.Factory
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' 创建分配一个UDP端点绑定连接通道
    ''' </summary>
    Friend Class UDPServerFactory
        Implements INConnectorFactory

#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mUDPServerFactory As UDPServerFactory
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As UDPServerFactory
            If mUDPServerFactory Is Nothing Then
                SyncLock lockobj
                    If mUDPServerFactory Is Nothing Then
                        mUDPServerFactory = New UDPServerFactory()
                    End If
                End SyncLock
            End If
            Return mUDPServerFactory
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
            Dim conn = New UDPServerConnector(detail, ConnecterManage)
            Return conn
        End Function


        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Async Function CreateConnectorAsync(detail As INConnectorDetail, ConnecterManage As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim conn = New UDPServerConnector(detail, ConnecterManage)
            Await conn.ConnectAsync().ConfigureAwait(False)
            Return conn
        End Function
    End Class
End Namespace


