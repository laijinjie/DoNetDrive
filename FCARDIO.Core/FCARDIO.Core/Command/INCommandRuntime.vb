Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers

Namespace Command
    Public Interface INCommandRuntime
        Inherits INCommand

        ''' <summary>
        ''' 是否正在等待执行
        ''' </summary>
        ''' <returns></returns>
        Property IsWaitExecute As Boolean

        ''' <summary>
        ''' 获取用于表示命令已停止的状态
        ''' </summary>
        ''' <returns></returns>
        Function GetStatus_Stop() As AbstractCommandStatus_Stop

        ''' <summary>
        ''' 获取用于表示命令已失败的状态
        ''' </summary>
        ''' <returns></returns>
        Function GetStatus_Faulted() As AbstractCommandStatus_Faulted

        ''' <summary>
        ''' 获取用于表示命令正在准备中的状态(还未开始发送数据)
        ''' </summary>
        ''' <returns></returns>
        Function GetStatus_Wating() As AbstractCommandStatus_Waiting

        ''' <summary>
        ''' 获取用于表示命令正在运行中的状态
        ''' </summary>
        ''' <returns></returns>
        Function GetStatus_Runing() As AbstractCommandStatus_Runing

        ''' <summary>
        ''' 获取一个用户触发事件时发送到事件中的参数
        ''' </summary>
        ''' <returns></returns>
        Function GetEventArgs() As CommandEventArgs

        ''' <summary>
        ''' 命令加入到连接器
        ''' </summary>
        ''' <param name="connect"></param>
        Sub SetConnector(connect As INConnector)


        ''' <summary>
        ''' 设置命令的状态
        ''' </summary>
        Sub SetStatus(cmdstatus As INCommandStatus)



        ''' <summary>
        ''' 由连接通道推送来的接收缓冲区中的 Bytebuf
        ''' </summary>
        ''' <param name="buf"></param>
        Sub PushReadByteBuf(buf As IByteBuffer)


        ''' <summary>
        ''' 让命令强制完结，释放掉临时缓冲
        ''' </summary>
        Sub CommandOver()
    End Interface
End Namespace

