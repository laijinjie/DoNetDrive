Namespace Command
    ''' <summary>
    ''' 命令还未开始执行
    ''' </summary>
    Public Class CommandStatus_Waiting
        Inherits AbstractCommandStatus_Waiting

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Dim base As AbstractCommand = TryCast(cmd, AbstractCommand)
            If base Is Nothing Then Return
            base.fireCommandProcessEvent()
        End Sub
    End Class
End Namespace