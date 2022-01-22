Imports System.Net
Imports DoNetDrive.Core.Factory
Imports DotNetty.Buffers
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Namespace Connector.TCPClient
    Public Class TCPClientFactory
        Implements INConnectorFactory

#Region "单例"
        ''' <summary>
        ''' 用于生成TCPClient的分配器
        ''' </summary>
        Private Shared mTCPClientFactory As TCPClientFactory = New TCPClientFactory()
        ''' <summary>
        ''' 获取用于生成TCPClient的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As TCPClientFactory
            Return mTCPClientFactory
        End Function
#End Region


        ''' <summary>
        ''' 默认连接超时，单位毫秒
        ''' </summary>
        Public Shared CONNECT_TIMEOUT_Default As Integer = 8000
        ''' <summary>
        ''' 默认重新连接次数
        ''' </summary>
        Public Shared ReadOnly CONNECT_RECONNECT_Default As Integer = 5


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
        Public Shared ReadOnly CONNECT_RECONNECT_MAX As Integer = 30


        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function CreateConnector(detail As INConnectorDetail, Allocator As IConnecterManage) As INConnector Implements INConnectorFactory.CreateConnector
            Return New TCPClientConnector(detail)
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Async Function CreateConnectorAsync(detail As INConnectorDetail, Allocator As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim conncect = New TCPClientConnector(detail)
            Await conncect.ConnectAsync().ConfigureAwait(False)
            Return conncect
        End Function
    End Class
End Namespace

