

Namespace Connector.TCPServer

    ''' <summary>
    ''' 连接器状态--已关闭
    ''' </summary>
    Public Class TCPServerConnectorStatus_Closed
        Implements INConnectorStatus
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Closed"
        End Function


        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            If Not connector.CheckIsInvalid() Then
                Dim Client As TCPServerConnector = connector
                connector.ConnectAsync().ContinueWith(AddressOf Client.ConnectingNext)
            End If
        End Sub


    End Class

End Namespace
