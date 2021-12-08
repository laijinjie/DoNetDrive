Namespace Connector
    ''' <summary>
    ''' 指示通道已失效的状态
    ''' </summary>
    Public Class ConnectorStatus_Invalid
        Implements INConnectorStatus


        ''' <summary>
        ''' 获取当前状态的描述
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function Status() As String Implements INConnectorStatus.Status
            Return "Invalid"
        End Function

        ''' <summary>
        ''' 检查状态是否需要变化
        ''' </summary>
        ''' <param name="connector"></param>
        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Return
        End Sub
    End Class

End Namespace
