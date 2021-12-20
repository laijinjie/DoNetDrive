Namespace Connector.TCPClient
    Public Class TCPClientConnectorStatus_Connecting
        Inherits ConnectorStatus_Connecting

        Protected Overrides Sub CheckConnectingStatus(connector As INConnector)
            connector?.UpdateActivityTime()
        End Sub
    End Class

End Namespace
