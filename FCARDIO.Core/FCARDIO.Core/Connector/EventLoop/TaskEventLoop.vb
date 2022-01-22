Imports System.Threading
Imports DotNetty.Common.Concurrency
Imports DotNetty.Transport.Channels
Imports System.Threading.Tasks

Namespace Connector
    Public Class TaskEventLoop
        Implements IEventLoop

        Public Shared [Default] As TaskEventLoop = New TaskEventLoop

        Public ReadOnly Property Parent As IEventLoopGroup Implements IEventLoop.Parent
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property Items As IEnumerable(Of IEventLoop) Implements IEventLoopGroup.Items
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property InEventLoop As Boolean Implements IEventExecutor.InEventLoop
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property IsShuttingDown As Boolean Implements IEventExecutorGroup.IsShuttingDown
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property TerminationCompletion As Task Implements IEventExecutorGroup.TerminationCompletion
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property IsShutdown As Boolean Implements IExecutorService.IsShutdown
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property IsTerminated As Boolean Implements IExecutorService.IsTerminated
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private ReadOnly Property IEventExecutor_Parent As IEventExecutorGroup Implements IEventExecutor.Parent
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private ReadOnly Property IEventExecutorGroup_Items As IEnumerable(Of IEventExecutor) Implements IEventExecutorGroup.Items
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Sub Execute(oTask As IRunnable) Implements IExecutor.Execute
            Task.Run(AddressOf oTask.Run)
        End Sub

        Public Sub Execute(action As Action(Of Object), state As Object) Implements IExecutor.Execute
            Task.Run(Sub()
                         action(state)
                     End Sub)
        End Sub

        Public Sub Execute(action As Action) Implements IExecutor.Execute
            Task.Run(action)
        End Sub

        Public Sub Execute(action As Action(Of Object, Object), context As Object, state As Object) Implements IExecutor.Execute
            Task.Run(Sub()
                         action(context, state)
                     End Sub)
        End Sub

        Public Function GetNext() As IEventLoop Implements IEventLoopGroup.GetNext
            Throw New NotImplementedException()
        End Function

        Public Function RegisterAsync(channel As IChannel) As Task Implements IEventLoopGroup.RegisterAsync
            Throw New NotImplementedException()
        End Function

        Public Function IsInEventLoop(thread As Thread) As Boolean Implements IEventExecutor.IsInEventLoop
            Throw New NotImplementedException()
        End Function

        Public Function ShutdownGracefullyAsync() As Task Implements IEventExecutorGroup.ShutdownGracefullyAsync
            Throw New NotImplementedException()
        End Function

        Public Function ShutdownGracefullyAsync(quietPeriod As TimeSpan, timeout As TimeSpan) As Task Implements IEventExecutorGroup.ShutdownGracefullyAsync
            Throw New NotImplementedException()
        End Function

        Public Function Schedule(action As IRunnable, delay As TimeSpan) As IScheduledTask Implements IScheduledExecutorService.Schedule
            Dim oTask = Task.Run(Async Function()
                                     Await Task.Delay(delay.TotalMilliseconds).ConfigureAwait(False)
                                     action.Run()
                                 End Function)
            Return New TaskEventLoopScheduledTask(oTask)
        End Function

        Public Function Schedule(action As Action, delay As TimeSpan) As IScheduledTask Implements IScheduledExecutorService.Schedule
            Dim oTask = Task.Run(Async Function()
                                     Await Task.Delay(delay.TotalMilliseconds).ConfigureAwait(False)
                                     action()
                                 End Function)
            Return New TaskEventLoopScheduledTask(oTask)
        End Function

        Public Function Schedule(action As Action(Of Object), state As Object, delay As TimeSpan) As IScheduledTask Implements IScheduledExecutorService.Schedule
            Dim oTask = Task.Run(Async Function()
                                     Await Task.Delay(delay.TotalMilliseconds).ConfigureAwait(False)
                                     action(state)
                                 End Function)
            Return New TaskEventLoopScheduledTask(oTask)
        End Function

        Public Function Schedule(action As Action(Of Object, Object), context As Object, state As Object, delay As TimeSpan) As IScheduledTask Implements IScheduledExecutorService.Schedule
            Dim oTask = Task.Run(Async Function()
                                     Await Task.Delay(delay.TotalMilliseconds).ConfigureAwait(False)
                                     action(context, state)
                                 End Function)
            Return New TaskEventLoopScheduledTask(oTask)
        End Function

        Public Async Function ScheduleAsync(action As Action(Of Object), state As Object, delay As TimeSpan, cancellationToken As CancellationToken) As Task Implements IScheduledExecutorService.ScheduleAsync
            Dim oTask = Task.Run(Async Function()
                                     Await Task.Delay(delay.TotalMilliseconds).ConfigureAwait(False)
                                     action(state)
                                 End Function, cancellationToken)
            Await oTask
        End Function

        Public Function ScheduleAsync(action As Action(Of Object), state As Object, delay As TimeSpan) As Task Implements IScheduledExecutorService.ScheduleAsync
            Throw New NotImplementedException()
        End Function

        Public Function ScheduleAsync(action As Action, delay As TimeSpan, cancellationToken As CancellationToken) As Task Implements IScheduledExecutorService.ScheduleAsync
            Throw New NotImplementedException()
        End Function

        Public Function ScheduleAsync(action As Action, delay As TimeSpan) As Task Implements IScheduledExecutorService.ScheduleAsync
            Throw New NotImplementedException()
        End Function

        Public Function ScheduleAsync(action As Action(Of Object, Object), context As Object, state As Object, delay As TimeSpan) As Task Implements IScheduledExecutorService.ScheduleAsync
            Throw New NotImplementedException()
        End Function

        Public Function ScheduleAsync(action As Action(Of Object, Object), context As Object, state As Object, delay As TimeSpan, cancellationToken As CancellationToken) As Task Implements IScheduledExecutorService.ScheduleAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of T)) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of T), cancellationToken As CancellationToken) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of Object, T), state As Object) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of Object, T), state As Object, cancellationToken As CancellationToken) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of Object, Object, T), context As Object, state As Object) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Public Function SubmitAsync(Of T)(func As Func(Of Object, Object, T), context As Object, state As Object, cancellationToken As CancellationToken) As Task(Of T) Implements IExecutorService.SubmitAsync
            Throw New NotImplementedException()
        End Function

        Private Function IEventExecutorGroup_GetNext() As IEventExecutor Implements IEventExecutorGroup.GetNext
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

