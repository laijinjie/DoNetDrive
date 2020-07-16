Namespace Command
    Public Interface INFireCommandEvent
        ''' <summary>
        ''' 当命令完成时，并将当前命令从队列中移除,会触发此函数回调
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub FireCommandCompleteEvent(e As CommandEventArgs)

        ''' <summary>
        ''' 触发命令完成消息 ，但是并不移除当前命令
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub fireCommandCompleteEventNotRemoveCommand(e As CommandEventArgs)

        ''' <summary>
        ''' 命令进度指示，当命令开始执行会连续触发，汇报命令执行的进度
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub FireCommandProcessEvent(e As CommandEventArgs)


        ''' <summary>
        ''' 发生错误时触发事件，一般是连接握手失败，串口不存在，usb不存在，没有写文件权限等
        ''' 还有可能是用户调用Stop指令强制停止命令
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub FireCommandErrorEvent(e As CommandEventArgs)


        ''' <summary>
        ''' 命令超时时，触发此回到函数
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub FireCommandTimeout(e As CommandEventArgs)



        ''' <summary>
        ''' 身份鉴权时发生错误的事件,
        ''' 一般发生于密码错误，校验失败等情况！
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Sub FireAuthenticationErrorEvent(e As CommandEventArgs)
    End Interface
End Namespace

