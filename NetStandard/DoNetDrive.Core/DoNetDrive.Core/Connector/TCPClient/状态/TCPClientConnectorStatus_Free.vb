Namespace Connector.TCPClient
    ''' <summary>
    ''' 表示通道空闲，连接已关闭的状态
    ''' </summary>
    Public Class TCPClientConnectorStatus_Free
        Inherits ConnectorStatus_Free

        Protected Overrides Sub OpenConnector(connector As INConnector)
            Dim client As TCPClientConnector = TryCast(connector, TCPClientConnector)
            If client Is Nothing Then Return

            client.ConnectServer()

        End Sub
    End Class
End Namespace

