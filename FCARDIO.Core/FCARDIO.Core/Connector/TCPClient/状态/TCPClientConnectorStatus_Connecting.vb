Namespace Connector.TCPClient
    Public Class TCPClientConnectorStatus_Connecting
        Implements INConnectorStatus
        ''' <summary>
        ''' 检查状态是否需要变化
        ''' </summary>
        ''' <param name="connector"></param>
        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Dim Client As TCPClientConnector = connector
            Client.CheckConnectorTimeout()

        End Sub
        ''' <summary>
        ''' 获取当前状态的描述
        ''' </summary>
        ''' <returns></returns>
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connecting"
        End Function
    End Class

End Namespace
