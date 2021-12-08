Namespace Connector
    ''' <summary>
    ''' 指示通道已绑定本地端口
    ''' </summary>
    Public Class ConnectorStatus_Bind
        Implements INConnectorStatus
        ''' <summary>
        ''' 检查状态是否需要变化
        ''' </summary>
        ''' <param name="connector"></param>
        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            Return
        End Sub
        ''' <summary>
        ''' 获取当前状态的描述
        ''' </summary>
        ''' <returns></returns>
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Bind"
        End Function
    End Class

End Namespace
