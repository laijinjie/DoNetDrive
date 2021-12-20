Namespace Command
    Public MustInherit Class CommandStatus

        ''' <summary>
        ''' 命令已完成的状态
        ''' </summary>
        Public Shared Completed As AbstractCommandStatus_Completed = New CommandStatus_Completed()
        ''' <summary>
        ''' 命令由于发生错误而完成
        ''' </summary>
        Public Shared Faulted As AbstractCommandStatus_Faulted = New CommandStatus_Faulted()

        ''' <summary>
        ''' 命令发生超时错误时的状态
        ''' </summary>
        Public Shared Timeout As AbstractCommandStatus_Timeout = New CommandStatus_Timeout()

        ''' <summary>
        ''' 命令正在执行
        ''' </summary>
        Public Shared Runing As AbstractCommandStatus_Runing = New CommandStatus_Runing()

        ''' <summary>
        ''' 命令还未开始执行
        ''' </summary>
        Public Shared Waiting As AbstractCommandStatus_Waiting = New CommandStatus_Waiting()

        ''' <summary>
        ''' 命令正在等待对端的响应
        ''' </summary>
        Public Shared WaitResponse As AbstractCommandStatus_WaitResponse = New CommandStatus_WaitResponse


        ''' <summary>
        ''' 命令由于用户取消而完成
        ''' </summary>
        Public Shared [Stop] As AbstractCommandStatus_Stop = New CommandStatus_Stop()
    End Class


    ''' <summary>
    ''' 命令已完成的状态
    ''' </summary>
    Public Class CommandStatus_Completed
        Inherits AbstractCommandStatus_Completed

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

    ''' <summary>
    ''' 命令由于发生错误而完成
    ''' </summary>
    Public Class CommandStatus_Faulted
        Inherits AbstractCommandStatus_Faulted

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

    ''' <summary>
    ''' 命令由于对端超过指定时间后为接收到响应（命令超时）而完成
    ''' </summary>
    Public Class CommandStatus_Timeout
        Inherits AbstractCommandStatus_Timeout

        Public Overrides Sub CheckStatus(cmd As INCommand)
            Return
        End Sub
    End Class

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
