Namespace Command
    Public Interface INCommandEvent
        ''' <summary>
        ''' 当命令完成时，会触发此函数回调
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Event CommandCompleteEvent(sender As Object, e As CommandEventArgs)


        ''' <summary>
        ''' 命令进度指示，当命令开始执行会连续触发，汇报命令执行的进度
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Event CommandProcessEvent(sender As Object, e As CommandEventArgs)


        ''' <summary>
        ''' 发生错误时触发事件，一般是连接握手失败，串口不存在，usb不存在，没有写文件权限等
        ''' 还有可能是用户调用Stop指令强制停止命令
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Event CommandErrorEvent(sender As Object, e As CommandEventArgs)


        ''' <summary>
        ''' 命令超时时，触发此回到函数
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Event CommandTimeout(sender As Object, e As CommandEventArgs)

        ''' <summary>
        ''' 身份鉴权时发生错误的事件,
        ''' 一般发生于密码错误，校验失败等情况！
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Event AuthenticationErrorEvent(sender As Object, e As CommandEventArgs)
    End Interface

End Namespace
