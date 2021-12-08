Imports System.Runtime.CompilerServices
Imports DotNetty.Common
Imports DotNetty.Common.Concurrency

Namespace Connector
    Public Class TaskEventLoopScheduledTask
        Implements IScheduledTask

        Private _Task As Task

        Public Sub New(ByVal t As Task)
            _Task = t
        End Sub

        Public ReadOnly Property Deadline As PreciseTimeSpan Implements IScheduledTask.Deadline
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property Completion As Task Implements IScheduledTask.Completion
            Get
                Return _Task
            End Get
        End Property

        Public Function Cancel() As Boolean Implements IScheduledTask.Cancel
            Throw New NotImplementedException()
        End Function

        Public Function GetAwaiter() As TaskAwaiter Implements IScheduledTask.GetAwaiter
            Return _Task.GetAwaiter()
        End Function
    End Class
End Namespace

