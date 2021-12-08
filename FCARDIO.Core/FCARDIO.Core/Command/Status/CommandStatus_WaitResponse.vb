Namespace Command
    ''' <summary>
    ''' 命令已发送数据包，等待对端的响应
    ''' </summary>
    Public Class CommandStatus_WaitResponse
        Inherits AbstractCommandStatus_WaitResponse

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Dim base As AbstractCommand = TryCast(cmd, AbstractCommand)
            If base Is Nothing Then Return
            base.CheckTimeout()
        End Sub
    End Class
End Namespace
