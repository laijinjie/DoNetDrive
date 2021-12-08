
Namespace Command
    ''' <summary>
    ''' 命令由于对端超过指定时间后为接收到响应（命令超时）而完成
    ''' </summary>
    Public Class CommandStatus_Timeout
        Inherits AbstractCommandStatus_Timeout

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

End Namespace
