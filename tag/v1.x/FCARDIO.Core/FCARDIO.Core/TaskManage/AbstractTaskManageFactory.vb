Imports System.Threading
Imports DotNetty.Transport.Channels

Namespace TaskManage
    Public MustInherit Class AbstractTaskManageFactory(Of T As ITaskClient)
        Implements IDisposable
        Private mManagers As List(Of AbstractTaskManage(Of T))

        ''' <summary>
        ''' 初始化任务管理器工厂
        ''' </summary>
        Sub New(works As IEventLoopGroup)
            IniManage(works)
        End Sub

        ''' <summary>
        ''' 初始化任务管理器工厂
        ''' </summary>
        Protected Sub IniManage(works As IEventLoopGroup)
            '获取所有线程池中的线程
            Dim HashID As HashSet(Of IEventLoop) = New HashSet(Of IEventLoop)
            Dim elp As IEventLoop
            Dim EventLoops = New List(Of IEventLoop)
            Do
                elp = works.GetNext()
                If Not HashID.Contains(elp) Then
                    EventLoops.Add(elp)
                    HashID.Add(elp)
                Else
                    elp = Nothing
                End If
            Loop While (elp IsNot Nothing)
            '根据线程池线程数创建任务分片器
            Dim iCount = HashID.Count

            mManagers = New List(Of AbstractTaskManage(Of T))(iCount)

            Dim oMgs As AbstractTaskManage(Of T)
            For i = 0 To iCount - 1
                oMgs = GetNewTaskManage(EventLoops(i))
                If oMgs IsNot Nothing Then
                    mManagers.Add(oMgs)
                End If
            Next
        End Sub


        ''' <summary>
        ''' 创建一个通道管理器
        ''' </summary>
        ''' <param name="elp"></param>
        ''' <returns></returns>
        Protected MustOverride Function GetNewTaskManage(elp As IEventLoop) As AbstractTaskManage(Of T)

        ''' <summary>
        ''' 已创建的通道数
        ''' </summary>
        Private mConnectorCreateCount As Integer

        ''' <summary>
        ''' 返回一个管理者
        ''' </summary>
        ''' <returns></returns>
        Public Function GetManager() As AbstractTaskManage(Of T)
            If disposedValue Then Return Nothing

            Dim id = Interlocked.Increment(mConnectorCreateCount)
            Dim iCount = mManagers.Count
            Return mManagers(Math.Abs(id Mod iCount))
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                disposedValue = True
                Dim oType = Me.GetType()
                'Trace.WriteLine("调用 AbstractTaskManageFactory.Dispose,准备释放管理器工厂,管理器标识：" & oType.Name)
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                End If
                For Each m In mManagers
                    m.Release()
                Next
                mManagers.Clear()
                mManagers = Nothing


                'Trace.WriteLine("调用 AbstractTaskManageFactory.Dispose,管理器工厂释放完毕,管理器标识：" & oType.Name)
                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If

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

