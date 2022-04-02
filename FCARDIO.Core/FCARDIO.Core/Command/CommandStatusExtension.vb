
Imports System.Runtime.CompilerServices

Namespace Command
    Public Module CommandStatusExtension
        <Extension()>
        Public Function IsCommandSuccessful(ByVal status As INCommandStatus) As Boolean
            If status.IsCompleted = False Then Return False
            If status.IsFaulted Then Return False
            If status.IsCanceled Then Return False
            Return True
        End Function
    End Module

End Namespace

