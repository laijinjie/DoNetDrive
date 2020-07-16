Imports DoNetDrive.Core.Command
Namespace FrameCommand
    ''' <summary>
    ''' 表示校验和错误的状态
    ''' </summary>
    Public Class CheckSumError
        Inherits CommandStatus_Faulted

    End Class
    ''' <summary>
    ''' 表示通讯密码错误的状态
    ''' </summary>
    Public Class PasswordErrorStatus
        Inherits CommandStatus_Faulted

    End Class
End Namespace

