Namespace Command
    ''' <summary>
    ''' 命令已完成的状态
    ''' </summary>
    Public Class CommandStatus_Completed
        Inherits AbstractCommandStatus_Completed

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

End Namespace
