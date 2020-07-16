Namespace Connector.TCPClient
    Public Class TCPClientConnectorStatus_Connecting
        Inherits ConnectorStatus_Connecting

        Protected Overrides Sub CheckConnectingStatus(connector As INConnector)
            Dim client As TCPClientConnector = TryCast(connector, TCPClientConnector)

            client?.UpdateActivityTime()
        End Sub
    End Class

End Namespace
