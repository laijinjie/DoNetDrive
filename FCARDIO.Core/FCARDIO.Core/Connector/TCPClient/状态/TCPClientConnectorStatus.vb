Namespace Connector.TCPClient
    Public MustInherit Class TCPClientConnectorStatus
        ''' <summary>
        ''' 已处于关闭状态
        ''' </summary>
        Public Shared Closed As INConnectorStatus = New TCPClientConnectorStatus_Closed
        ''' <summary>
        ''' 正在连接远程主机的状态
        ''' </summary>
        Public Shared Connecting As INConnectorStatus = New TCPClientConnectorStatus_Connecting
        ''' <summary>
        ''' 指示通道已连接
        ''' 通常用在客户端管道上
        ''' </summary>
        Public Shared Connected As UDPClientConnectorStatus_Connected = New UDPClientConnectorStatus_Connected()

    End Class
End Namespace

