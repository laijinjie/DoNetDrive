Imports DotNetty.Common.Concurrency
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Data

Namespace Connector
    ''' <summary>
    ''' 所有连接器的顶级接口
    ''' </summary>
    Public Interface INConnector
        Inherits IRunnable, IDisposable, INConnectorEvent, INFireConnectorEvent, INCommandEvent, INFireCommandEvent,
            TaskManage.ITaskClient

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Function GetConnectorType() As String

        ''' <summary>
        ''' 获取连接器的详情对象
        ''' </summary>
        ''' <returns></returns>
        Function GetConnectorDetail() As INConnectorDetail

        ''' <summary>''' 检查通道是否已失效 1分钟无连接，无命令任务则自动失效
        ''' </summary>
        Sub CheckIsInvalid()

        ''' <summary>
        ''' 获取通道中的命令队列数量
        ''' </summary>
        ''' <returns>命令队列数量</returns>
        Function GetCommandCount() As Integer

        ''' <summary>
        ''' 命令队列是否为空
        ''' </summary>
        ''' <returns></returns>
        Function CommandListIsEmpty() As Boolean

        ''' <summary>
        ''' 将一个命令添加到本通道的命令队列中
        ''' </summary>
        ''' <param name="cd">命令封装类，执行具体指令</param>
        Sub AddCommand(cd As INCommandRuntime)

        ''' <summary>
        ''' 当需要解析监控指令时，添加数据包解析器到解析器列表中
        ''' </summary>
        ''' <param name="handle">数据包解析器</param>
        Sub AddRequestHandle(handle As INRequestHandle)

        ''' <summary>
        ''' 从连接通道中删除指定类型的数据包解析器
        ''' </summary>
        ''' <param name="handle"></param>
        Sub RemoveRequestHandle(handle As Type)


        ''' <summary>
        '''  判断此通道是否保持连接，即通道在发送完毕命令后保持连接
        ''' </summary>
        ''' <returns> true 表示通道保持打开</returns>
        Function IsForciblyConnect() As Boolean

        ''' <summary>
        ''' 设定此连接器通道为保持打开状态
        ''' </summary>
        Sub OpenForciblyConnect()

        ''' <summary>
        ''' 禁止此连接器通道为保持连接状态，即命令发送完毕后关闭连接。
        ''' </summary>
        Sub CloseForciblyConnect()

        ''' <summary>
        ''' 确定通道是否已失效
        ''' </summary>
        ReadOnly Property IsInvalid As Boolean

        ''' <summary>
        ''' 通道是否为活动状态(已连接)
        ''' </summary>
        ''' <returns></returns>
        Overloads Function IsActivity() As Boolean

        ''' <summary>
        ''' 获取本地绑定地址
        ''' </summary>
        ''' <returns></returns>
        Function LocalAddress() As IPDetail

        ''' <summary>
        ''' 获取此连接通道的状态
        ''' </summary>
        ''' <returns></returns>
        Function GetStatus() As INConnectorStatus

        ''' <summary>
        ''' 停止指定类型的命令，终止命令继续执行
        ''' </summary>
        ''' <param name="cdt">命令详情，如果为Null表示停止此通道中的所有命令</param>
        ''' <returns></returns>
        Function StopCommand(cdt As INCommandDetail) As Boolean

        ''' <summary>
        ''' 更新通道活动时间
        ''' </summary>
        Sub UpdateActivityTime()

        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Function GetByteBufAllocator() As IByteBufferAllocator

        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Function WriteByteBuf(ByVal buf As IByteBuffer) As Task

        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Overloads Function GetEventLoop() As IEventLoop
    End Interface
End Namespace

