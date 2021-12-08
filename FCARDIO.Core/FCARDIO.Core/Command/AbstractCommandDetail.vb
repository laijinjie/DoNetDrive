Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Data

Namespace Command
    Public MustInherit Class AbstractCommandDetail
        Implements INCommandDetail

        ''' <summary>
        ''' 用来存储命令将要使用的连接器信息
        ''' </summary>
        ''' <returns></returns>
        Public Property Connector As INConnectorDetail Implements INCommandDetail.Connector

        ''' <summary>
        ''' 起始时间
        ''' </summary>
        ''' <returns></returns>
        Public Property BeginTime As Date Implements INCommandDetail.BeginTime

        ''' <summary>
        ''' 命令执行完毕时间
        ''' </summary>
        ''' <returns></returns>
        Public Property EndTime As Date Implements INCommandDetail.EndTime
        ''' <summary>
        ''' 命令在通道中发送后的最大等待应答事件，单位毫秒
        ''' </summary>
        ''' <returns></returns>
        Public Property Timeout As Integer Implements INCommandDetail.Timeout
        ''' <summary>
        ''' 当命令发生超时后，最大重试次数。
        ''' </summary>
        ''' <returns></returns>
        Public Property RestartCount As Integer Implements INCommandDetail.RestartCount

        ''' <summary>
        ''' 用户数据，可以用于保存临时数据
        ''' </summary>
        ''' <returns></returns>
        Public Property UserData As Object Implements INCommandDetail.UserData



        Protected _Key As String
        Public ReadOnly Property Key As String Implements INCommandDetail.Key
            Get
                Return _Key
            End Get
        End Property


#Region "事件定义"
        ''' <summary>
        ''' 当命令完成时，会触发此函数回调
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandCompleteEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandCompleteEvent
        ''' <summary>
        ''' 命令进度指示，当命令开始执行会连续触发，汇报命令执行的进度
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandProcessEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandProcessEvent
        ''' <summary>
        ''' 发生错误时触发事件，一般是连接握手失败，串口不存在，usb不存在，没有写文件权限等
        ''' 还有可能是用户调用Stop指令强制停止命令
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandErrorEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandErrorEvent
        ''' <summary>
        ''' 命令超时时，触发此回到函数
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandTimeout(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandTimeout

        ''' <summary>
        ''' 身份鉴权时发生错误的事件,
        ''' 一般发生于密码错误，校验失败等情况！
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event AuthenticationErrorEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.AuthenticationErrorEvent


#End Region

#Region "事件触发处理"

#Region "触发事件--命令完毕"
        ''' <summary>
        ''' 触发命令完成消息
        ''' </summary>
        Public Sub fireCommandCompleteEvent(e As CommandEventArgs) Implements INFireCommandEvent.FireCommandCompleteEvent, INFireCommandEvent.fireCommandCompleteEventNotRemoveCommand
            RaiseEvent CommandCompleteEvent(Me, e)
        End Sub


#End Region

#Region "触发事件--命令进度指示"
        ''' <summary>
        ''' 触发事件--命令进度指示
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Sub fireCommandProcessEvent(e As CommandEventArgs) Implements INFireCommandEvent.FireCommandProcessEvent
            RaiseEvent CommandProcessEvent(Me, e)
        End Sub
#End Region

#Region "触发事件-- 命令超时"
        ''' <summary>
        ''' 触发事件-- 命令超时
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Sub fireCommandTimeout(e As CommandEventArgs) Implements INFireCommandEvent.FireCommandTimeout
            RaiseEvent CommandTimeout(Me, e)
        End Sub
#End Region

#Region "触发事件--身份鉴权错误"
        ''' <summary>
        ''' 触发事件--身份鉴权错误
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Sub fireAuthenticationErrorEvent(e As CommandEventArgs) Implements INFireCommandEvent.FireAuthenticationErrorEvent
            RaiseEvent AuthenticationErrorEvent(Me, e)
        End Sub
#End Region

#Region "触发事件--命令错误事件"
        ''' <summary>
        ''' 触发事件--命令错误事件
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Sub FireCommandErrorEvent(e As CommandEventArgs) Implements INFireCommandEvent.FireCommandErrorEvent
            RaiseEvent CommandErrorEvent(Me, e)
        End Sub
#End Region



#End Region

        ''' <summary>
        ''' 初始化详情，登记连接通道信息，并初始化命令超时时间和重发次数
        ''' </summary>
        ''' <param name="cnt"></param>
        Public Sub New(cnt As INConnectorDetail)
            Connector = cnt
            Timeout = 300
            RestartCount = 2
            _Key = Guid.NewGuid.ToString
        End Sub

        ''' <summary>
        ''' 复制当前命令详情的浅表副本
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function Clone() As Object Implements ICloneable.Clone

        ''' <summary>
        ''' 比较命令详情是否都指向相同通道和相同的对端身份
        ''' </summary>
        ''' <param name="other">待比较的详情</param>
        ''' <returns></returns>
        Public MustOverride Function Equals(other As INCommandDetail) As Boolean Implements IEquatable(Of INCommandDetail).Equals


#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        Protected MustOverride Sub Release0()

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    Release0()
                    Connector = Nothing
                End If

                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If
            disposedValue = True
        End Sub

        ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码以正确实现可释放模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(True)
            ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
            ' GC.SuppressFinalize(Me)
        End Sub


#End Region
    End Class
End Namespace

