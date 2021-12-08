Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Namespace Connector.TCPClient
    Public Class TCPClientAllocator
        Implements INConnectorAllocator

        ''' <summary>
        ''' 用于生成TCPClient的分配器
        ''' </summary>
        Private Shared mTCPClientAllocator As TCPClientAllocator = New TCPClientAllocator()
        ''' <summary>
        ''' 获取用于生成TCPClient的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetAllocator() As TCPClientAllocator
            Return mTCPClientAllocator
        End Function

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
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Overridable Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            Return
        End Sub

        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "TCPClient.TCPClientConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Overridable Function GetNewConnector(detail As INConnectorDetail) As INConnector Implements INConnectorAllocator.GetNewConnector
            Return New TCPClientConnector(detail)
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Overridable Async Function GetNewConnectorAsync(detail As INConnectorDetail) As Task(Of INConnector) Implements INConnectorAllocator.GetNewConnectorAsync
            Dim conncect = New TCPClientConnector(detail)
            Await conncect.ConnectAsync()
            Return conncect
        End Function
    End Class
End Namespace

