Imports System.Runtime.CompilerServices
Imports System.Text

Namespace Extension
    Public Module ModListExtensions
        <Extension>
        Public Function Join(Of TOuter)(outer As IEnumerable(Of TOuter), Optional ByVal sSplit As String = ",") As String
            If outer.Count = 0 Then Return String.Empty

            Dim sJoin = New StringBuilder(4096)

            For Each o As TOuter In outer
                sJoin.Append(o.ToString())
                sJoin.Append(sSplit)
            Next
            sJoin.Length -= sSplit.Length

            Return sJoin.ToString()
        End Function
    End Module
End Namespace

