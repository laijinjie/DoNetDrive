Namespace Connector.TCPClient
    Public MustInherit Class TCPClientConnectorStatus
        ''' <summary>
        ''' 空闲状态
        ''' </summary>
        Public Shared Free As INConnectorStatus = New TCPClientConnectorStatus_Free
        ''' <summary>
        ''' 正在连接远程主机的状态
        ''' </summary>
        Public Shared Connecting As INConnectorStatus = New TCPClientConnectorStatus_Connecting
        ''' <summary>
        ''' 远程连接成功的状态
        ''' </summary>
        Public Shared Connected As INConnectorStatus = New TCPClientConnectorStatus_Connected
        ''' <summary>
        ''' 远程连接失败的状态
        ''' </summary>
        Public Shared Fail As INConnectorStatus = New TCPClientConnectorStatus_Fail

    End Class
End Namespace

