Imports DoNetDrive.Core.Connector.TCPClient

Namespace Connector.TCPClient

    ''' <summary>
    ''' 连接器状态--已连接
    ''' </summary>
    Public Class TCPClientConnectorStatus_Connected
        Implements INConnectorStatus
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connected"
        End Function


        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Dim Client As TCPClientConnector = connector
            Client.CheckConnectedStatus()

        End Sub


    End Class

End Namespace
