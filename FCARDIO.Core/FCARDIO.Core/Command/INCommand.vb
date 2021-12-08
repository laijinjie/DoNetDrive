Imports DotNetty.Common.Concurrency



Namespace Command
    ''' <summary>
    ''' 命令的封装主体类
    ''' 包含协议中具体的命令包装，数据分析，数值判断等一系列逻辑
    ''' </summary>
    Public Interface INCommand
        Inherits IRunnable, IDisposable, ICloneable


        ''' <summary>
        ''' 命令所需要的连接通道，对端信息等数据
        ''' </summary>
        ''' <returns></returns>
        Property CommandDetail As INCommandDetail

        ''' <summary>
        ''' 保存用于命令的各种参数
        ''' </summary>
        ''' <returns></returns>
        Property Parameter As INCommandParameter

        ''' <summary>
        ''' 命令返回结果
        ''' </summary>
        ''' <returns></returns>
        Function getResult() As INCommandResult

        ''' <summary>
        ''' 获取命令状态
        ''' </summary>
        Function GetStatus() As INCommandStatus


        ''' <summary>
        ''' 总步骤数
        ''' </summary>
        Function getProcessMax() As Integer

        ''' <summary>
        ''' 当前指令进度
        ''' </summary>
        ''' <returns></returns>
        Function getProcessStep() As Integer

        ''' <summary>
        ''' 设置和命令相关联的错误
        ''' </summary>
        ''' <param name="ex"></param>
        Sub SetException(ex As Exception)
    End Interface

End Namespace
