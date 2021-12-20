Namespace Command.Text
    Public Class TextCommandResult
        Implements INCommandResult

        Public Result As String

        Public Sub New(rst As String)
            Result = rst
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace

