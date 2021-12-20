Imports DoNetDrive.Core.Connector.Client
Imports DotNetty.Buffers

Namespace Connector.TCPClient
    Public Class TCPClientConnectorStatus_Connected
        Inherits ConnectorStatus_Connected

        Protected Overrides Sub CheckCommandList(connector As INConnector)
            Dim client As AbstractConnector = TryCast(connector, AbstractConnector)
            If client Is Nothing Then Return

            client.CheckCommandList()
        End Sub

        Protected Overrides Sub CloseConnector(connector As INConnector)
            Dim client As AbstractConnector = TryCast(connector, AbstractConnector)
            If client Is Nothing Then Return
            client.CloseConnector()
        End Sub
    End Class
End Namespace

