Imports DoNetDrive.Core.Connector
Namespace Command
    ''' <summary>
    ''' 命令返回事件的事件参数
    ''' </summary>
    Public Class CommandEventArgs
        Inherits EventArgs
        Implements IDisposable

        Private Property mCommand As INCommand

        ''' <summary>
        ''' 命令的主体
        ''' </summary>
        ReadOnly Property Command As INCommand
            Get
                Return mCommand
            End Get
        End Property


        Private Property mCommandDetail As INCommandDetail

        ''' <summary>
        ''' 命令所包含的连接器，目标设备信息
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property CommandDetail As INCommandDetail
            Get
                Return mCommandDetail
            End Get
        End Property

        Private Property mConnectorDetail As INConnectorDetail
        ''' <summary>
        ''' 命令所包含的连接器，目标设备信息
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property ConnectorDetail As INConnectorDetail
            Get
                Return mConnectorDetail
            End Get
        End Property

        Protected Status As INCommandStatus


        ''' <summary>
        ''' 命令执行完毕后包含的结果
        ''' </summary>
        ReadOnly Property Result As INCommandResult
            Get
                Return Command?.getResult()
            End Get
        End Property





        ''' <summary>
        ''' 初始化事件值
        ''' </summary>
        Sub New(cd As INConnectorDetail)
            mConnectorDetail = cd
        End Sub


        ''' <summary>
        ''' 初始化事件值
        ''' </summary>
        Sub New(cmd As INCommand)
            mCommand = cmd

            mCommandDetail = cmd?.CommandDetail
            mConnectorDetail = CommandDetail?.Connector
        End Sub


        ''' <summary>
        ''' 事件的状态
        ''' </summary>
        ''' <returns></returns>
        Public Function GetStatus() As INCommandStatus
            Status = mCommand?.GetStatus
            Return Status
        End Function

        ''' <summary>
        ''' 命令是否已完成
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsCompleted As Boolean
            Get
                Dim s = mCommand?.GetStatus
                If s Is Nothing Then Return False
                Return s.IsCompleted
            End Get
        End Property


        ''' <summary>
        ''' 是否由于未经处理异常的原因而完成
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsFaulted As Boolean
            Get
                Dim s = mCommand?.GetStatus
                If s Is Nothing Then Return False
                Return s.IsFaulted
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    mCommand = Nothing
                    mCommandDetail = Nothing
                    mConnectorDetail = Nothing
                    Status = Nothing
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

