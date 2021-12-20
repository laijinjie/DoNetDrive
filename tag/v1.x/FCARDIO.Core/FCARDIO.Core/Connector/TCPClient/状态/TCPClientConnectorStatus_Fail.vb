Namespace Connector.TCPClient
    Public Class TCPClientConnectorStatus_Fail
        Inherits TCPClientConnectorStatus_Free

        Public Overrides Function Status() As String
            Return "Fail"
        End Function
    End Class

End Namespace
