Imports System.Collections.Concurrent
Imports DotNetty.Common.Concurrency
Imports DotNetty.Transport.Channels

Namespace TaskManage
    ''' <summary>
    ''' 表示着一组任务缓冲区，让任务在某个时间片内逐个执行
    ''' </summary>
    Public MustInherit Class AbstractTaskManage(Of T As ITaskClient)
        Implements IRunnable
        ''' <summary>
        ''' 一组任务执行完毕后的休眠间隔
        ''' </summary>
        Protected ScheduleTime As TimeSpan

        ''' <summary>
        ''' 任务循环器
        ''' </summary>
        Protected mEventLoop As IEventLoop
        ''' <summary>
        ''' 客户端列表
        ''' </summary>
        Protected Clients As ConcurrentDictionary(Of String, T)

        ''' <summary>
        ''' 是否已释放
        ''' </summary>
        Protected _IsRelease As Boolean

        ''' <summary>
        ''' 管理器绑定的线程ID
        ''' </summary>
        Protected _ThreadID As Long

        ''' <summary>
        ''' 初始化缓冲区
        ''' </summary>
        ''' <param name="elp">任务循环器</param>
        Sub New(elp As IEventLoop)
            Me.New(elp, New TimeSpan(0, 0, 0, 0, 10))
        End Sub

        ''' <summary>
        ''' 初始化缓冲区
        ''' </summary>
        ''' <param name="elp">任务循环器</param>
        ''' <param name="SleepTime">设定休眠间隔</param>
        Sub New(elp As IEventLoop, SleepTime As TimeSpan)
            Dim oSigElp = TryCast(elp, DotNetty.Transport.Channels.SingleThreadEventLoop)
            _ThreadID = oSigElp.Scheduler.Id
            'Trace.WriteLine("调用 AbstractTaskManage.New,构建一个管理器,管理器标识：" & Me.GetType().Name & ",事件循环标识：" & _ThreadID)

            mEventLoop = elp
            Clients = New ConcurrentDictionary(Of String, T)
            ScheduleTime = SleepTime
            mEventLoop.Schedule(Me, ScheduleTime)
            _IsRelease = False
        End Sub

        ''' <summary>
        ''' 返回管理器所使用的任务循环器
        ''' </summary>
        ''' <returns></returns>
        Public Function GetEventLoop() As IEventLoop
            Return mEventLoop
        End Function

        ''' <summary>
        ''' 添加一个客户端
        ''' </summary>
        ''' <param name="conn"></param>
        Public Sub Add(conn As T)
            If Clients.TryAdd(conn.GetKey(), conn) Then
                AddHandler conn.TaskCloseEvent, AddressOf TaskCloseEvent
                'Trace.WriteLine("添加客户端到管理器：" & conn.GetKey())
            End If
        End Sub

        ''' <summary>
        ''' 离线客户端回调
        ''' </summary>
        ''' <param name="cnt">客户端</param>
        Private Sub TaskCloseEvent(cnt As T)
            Dim sKey = cnt.GetKey()
            If Not Clients.ContainsKey(sKey) Then
                Return
            End If

            'Trace.WriteLine("客户端关闭事件：" & sKey)
            Remove(sKey)
        End Sub

        ''' <summary>
        ''' 删除一个客户端
        ''' </summary>
        ''' <param name="sKey"></param>
        Sub Remove(sKey As String)
            If Not Clients.ContainsKey(sKey) Then
                Return
            End If


            Dim oClient As T = Nothing
            If Clients.TryRemove(sKey, oClient) Then
                'Trace.WriteLine("将客户端从管理器中移除：" & sKey)
                _RemoveClient(oClient)
            End If


        End Sub

        ''' <summary>
        ''' 客户端被删除后的后续处理
        ''' </summary>
        ''' <param name="oClient"></param>
        Protected MustOverride Sub _RemoveClient(oClient As T)


        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Sub Release()
            Dim oType = Me.GetType()
            'Trace.WriteLine("调用 AbstractTaskManage.Release,准备释放管理器,管理器标识：" & oType.Name & ",事件循环标识：" & _ThreadID)
            _IsRelease = True

            mEventLoop = Nothing

            _Release()
            Clients.Clear()

            Trace.WriteLine("调用 AbstractTaskManage.Release,管理器释放完毕,管理器标识：" & oType.Name & ",事件循环标识：" & _ThreadID)
        End Sub

        ''' <summary>
        ''' 资源释放
        ''' </summary>
        Protected MustOverride Sub _Release()

        ''' <summary>
        ''' 任务处理器，再此检查各通道的状态
        ''' </summary>
        Public Sub Run() Implements IRunnable.Run
            If _IsRelease Then Return

            Try
                Dim sKeys = Clients.Keys
                Dim oClient As T = Nothing
                If sKeys.Count > 0 Then
                    For Each k In sKeys
                        If _IsRelease Then Return
                        Try
                            If Clients.TryGetValue(k, oClient) Then
                                ClientRun(oClient)
                            End If
                        Catch ex As Exception
                            Trace.WriteLine($"AbstractTaskManage.Run 出现错误 {ex.ToString()}")
                        End Try

                        If _IsRelease Then Return
                    Next

                End If
                If _IsRelease Then Return
            Catch ex As Exception
                Trace.WriteLine($"AbstractTaskManage.Run 出现错误 {ex.ToString()}")
            End Try

            '休眠30毫秒
            mEventLoop?.Schedule(Me, ScheduleTime)
        End Sub

        ''' <summary>
        ''' 执行客户端的逻辑
        ''' </summary>
        ''' <param name="tsk"></param>
        Protected MustOverride Sub ClientRun(tsk As T)
    End Class
End Namespace

