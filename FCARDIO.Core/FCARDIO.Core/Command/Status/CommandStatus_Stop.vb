Namespace Command
    ''' <summary>
    ''' 命令由于用户取消而完成
    ''' </summary>
    Public Class CommandStatus_Stop
        Inherits AbstractCommandStatus_Stop

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

End Namespace
