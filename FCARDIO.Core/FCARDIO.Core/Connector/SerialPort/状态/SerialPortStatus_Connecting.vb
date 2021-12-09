Namespace Connector.SerialPort
    Public Class SerialPortStatus_Connecting
        Implements INConnectorStatus
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connecting"
        End Function


        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Return
        End Sub
    End Class
End Namespace

