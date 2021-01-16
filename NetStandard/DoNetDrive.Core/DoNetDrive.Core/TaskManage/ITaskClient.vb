Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Common.Concurrency
Imports DotNetty.Transport.Channels

Namespace TaskManage
    ''' <summary>
    ''' 表示一个任务客户端，包含一组自维护逻辑
    ''' </summary>
    Public Interface ITaskClient
        Inherits IRunnable

        ''' <summary>
        ''' 获取一个用来标识客户端的Key字符串
        ''' </summary>
        ''' <returns></returns>
        Function GetKey() As String

        ''' <summary>
        ''' 通道是否为活动状态(已连接)
        ''' </summary>
        ''' <returns></returns>
        Function IsActivity() As Boolean

        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Function GetEventLoop() As IEventLoop

        ''' <summary>
        ''' 任务关闭事件
        ''' </summary>
        ''' <param name="client"></param>
        Event TaskCloseEvent(client As ITaskClient)
    End Interface
End Namespace