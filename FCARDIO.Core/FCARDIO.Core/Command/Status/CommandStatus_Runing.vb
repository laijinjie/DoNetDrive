Namespace Command
    ''' <summary>
    ''' 命令正在执行
    ''' </summary>
    Public Class CommandStatus_Runing
        Inherits AbstractCommandStatus_Runing

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Dim base As AbstractCommand = TryCast(cmd, AbstractCommand)
            If base Is Nothing Then Return
            base.SendPacket()
        End Sub
    End Class
End Namespace


