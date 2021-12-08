
Namespace Command


    ''' <summary>
    ''' 命令由于发生错误而完成
    ''' </summary>
    Public Class CommandStatus_Faulted
        Inherits AbstractCommandStatus_Faulted

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class
End Namespace