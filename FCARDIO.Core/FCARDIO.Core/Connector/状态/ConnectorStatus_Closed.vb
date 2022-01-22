Namespace Connector
    ''' <summary>
    ''' 连接器状态--已关闭
    ''' </summary>
    Public Class ConnectorStatus_Closed
        Implements INConnectorStatus

        Public Overridable Function Status() As String Implements INConnectorStatus.Status
            Return "Closed"
        End Function

        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            If (Not connector.CheckIsInvalid()) Then
                connector.ConnectAsync().ConfigureAwait(False)
            End If
        End Sub
    End Class
End Namespace

