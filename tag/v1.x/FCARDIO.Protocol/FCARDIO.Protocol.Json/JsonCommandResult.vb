Imports DoNetDrive.Core.Command

Public Class JsonCommandResult
    Implements INCommandResult

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub
    Public JsonValue As String

    Public Sub New(value As String)
        JsonValue = value
    End Sub
End Class
