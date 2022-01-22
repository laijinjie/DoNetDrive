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
        Inherits IRunnable, IDisposable, INConnectorEvent, INFireConnectorEvent, INCommandEvent, INFireCommandEvent

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Function GetConnectorType() As String


        ''' <summary>
        ''' 获取一个表示连接通道唯一性的Key
        ''' </summary>
        ''' <returns></returns>
        Function GetKey() As String

        ''' <summary>
        ''' 获取连接器的详情对象
        ''' </summary>
        ''' <returns></returns>
        Function GetConnectorDetail() As INConnectorDetail

        ''' <summary>
        ''' 检查通道是否已失效,达到失效的条件
        ''' 1、通道内没有待发送的命令
        ''' 2、通道内已达到指定时间未发送和接收到任何数据
        ''' 3、通道不需要保持打开
        ''' 4、通道已被手动关闭
        ''' </summary>
        Function CheckIsInvalid() As Boolean

        ''' <summary>
        ''' 让通道检查命令管道，检测命令状态,驱动命令继续执行
        ''' </summary>
        Sub CheckCommandList()


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
        ''' 将一个命令添加到本通道的命令队列中，并等待命令执行完毕
        ''' </summary>
        ''' <param name="cd">命令封装类，执行具体指令</param>
        Function RunCommandAsync(cd As INCommandRuntime) As Task(Of INCommand)

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
        ''' 最后读取数据的时间
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property LastReadDataTime As Date

        ''' <summary>
        ''' 最后发送数据的时间
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property LastSendDataTime As Date


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
        ''' 获取远程服务器地址
        ''' </summary>
        ''' <returns></returns>
        Function RemoteAddress() As IPDetail

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

        ''' <summary>
        ''' 关闭链接
        ''' </summary>
        Function CloseAsync() As Task

        ''' <summary>
        ''' 建立连接
        ''' </summary>
        ''' <returns></returns>
        Function ConnectAsync() As Task

        ''' <summary>
        ''' 发送字节总数
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property SendTotalBytes As Long

        ''' <summary>
        ''' 接收字节总数
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property ReadTotalBytes As Long

        ''' <summary>
        ''' 累计接收到的命令数量
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property CommandTotal As Long


        ''' <summary>
        ''' 设置保活包参数
        ''' </summary>
        ''' <param name="bUse">启用开关</param>
        ''' <param name="iIntervalTime">间隔时间</param>
        ''' <param name="SendBuf">发送内容</param>
        Sub SetKeepAliveOption(ByVal bUse As Boolean, ByVal iIntervalTime As Integer, ByVal SendBuf() As Byte)
    End Interface
End Namespace

