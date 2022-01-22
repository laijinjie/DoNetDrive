
Namespace Connector.UDP

    ''' <summary>
    ''' 连接器状态--已连接
    ''' </summary>
    Public Class UDPClientConnectorStatus_Connected
        Implements INConnectorStatus

        Public Shared Connected As UDPClientConnectorStatus_Connected = New UDPClientConnectorStatus_Connected

        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connected"
        End Function


        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Dim Client As UDPServerClientConnector = connector
            Client.CheckConnectedStatus()

        End Sub


    End Class

End Namespace
