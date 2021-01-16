Imports DoNetDrive.Core.Connector

Namespace Command
    ''' <summary>
    ''' 包含命令的执行时的一些必要信息，命令执行的连接器通道，命令身份验证信息，用户附加数据，超时重试参数
    ''' </summary>
    Public Interface INCommandDetail
        Inherits IDisposable, ICloneable, IEquatable(Of INCommandDetail),
            INCommandEvent, INFireCommandEvent
        ''' <summary>
        ''' 用来存储命令将要使用的连接器信息
        ''' </summary>
        ''' <returns></returns>
        Property Connector As INConnectorDetail

        ''' <summary>
        ''' 起始时间
        ''' </summary>
        ''' <returns></returns>
        Property BeginTime As DateTime

        ''' <summary>
        ''' 命令执行完毕时间
        ''' </summary>
        ''' <returns></returns>
        Property EndTime As DateTime

        ''' <summary>
        ''' 命令在通道中发送后的最大等待应答事件，单位毫秒
        ''' </summary>
        ''' <returns></returns>
        Property Timeout As Integer

        ''' <summary>
        ''' 当命令发生超时后，最大重试次数。
        ''' </summary>
        ''' <returns></returns>
        Property RestartCount As Integer

        ''' <summary>
        ''' 用户数据，可以用于保存临时数据
        ''' </summary>
        ''' <returns></returns>
        Property UserData As Object
    End Interface
End Namespace

