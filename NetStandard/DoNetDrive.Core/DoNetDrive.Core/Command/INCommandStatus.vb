Namespace Command

    Public Interface INCommandStatus
        ''' <summary>
        ''' 命令是否已完成
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsCompleted As Boolean

        ''' <summary>
        ''' 是否由于未经处理异常的原因而完成
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsFaulted As Boolean

        ''' <summary>
        ''' 命令知否正在执行
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsRuning As Boolean

        ''' <summary>
        ''' 是否已取消执行
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsCanceled As Boolean

        ''' <summary>
        ''' 触发命令事件
        ''' </summary>
        ''' <param name="cmd"></param>
        Sub CheckStatus(cmd As INCommand)
    End Interface

    Public MustInherit Class AbstractCommandStatus
        Implements INCommandStatus

        Public Overridable ReadOnly Property IsCompleted As Boolean Implements INCommandStatus.IsCompleted
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property IsFaulted As Boolean Implements INCommandStatus.IsFaulted
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property IsRuning As Boolean Implements INCommandStatus.IsRuning
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property IsCanceled As Boolean Implements INCommandStatus.IsCanceled
            Get
                Return False
            End Get
        End Property

        Public MustOverride Sub CheckStatus(cmd As INCommand) Implements INCommandStatus.CheckStatus
    End Class



    ''' <summary>
    ''' 命令已完成的状态
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Completed
        Inherits AbstractCommandStatus

        Public Overrides ReadOnly Property IsCompleted As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    ''' <summary>
    ''' 命令由于发生错误而完成
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Faulted
        Inherits AbstractCommandStatus_Completed

        Public Overrides ReadOnly Property IsFaulted As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

    ''' <summary>
    ''' 命令由于对端超过指定时间后为接收到响应（命令超时）而完成
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Timeout
        Inherits AbstractCommandStatus_Faulted
    End Class

    ''' <summary>
    ''' 命令正在执行
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Runing
        Inherits AbstractCommandStatus

        Public Overrides ReadOnly Property IsRuning As Boolean
            Get
                Return True
            End Get
        End Property

    End Class

    ''' <summary>
    ''' 命令已发送数据包，等待对端的响应
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_WaitResponse
        Inherits AbstractCommandStatus_Runing
    End Class


    ''' <summary>
    ''' 命令还未开始执行
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Waiting
        Inherits AbstractCommandStatus
    End Class


    ''' <summary>
    ''' 命令由于用户取消而完成
    ''' </summary>
    Public MustInherit Class AbstractCommandStatus_Stop
        Inherits AbstractCommandStatus_Completed


        Public Overrides ReadOnly Property IsCanceled As Boolean
            Get
                Return True
            End Get
        End Property
    End Class

End Namespace




