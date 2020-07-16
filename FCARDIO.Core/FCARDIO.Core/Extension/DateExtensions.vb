Imports System.Runtime.CompilerServices

Namespace Extension
    Public Module DateExtensions
        <Extension()>
        Public Function ToDateTimeStr(ByVal d As Date) As String
            Return d.ToString(UserDateTimeFormat)
        End Function

        <Extension()>
        Public Function ToDateStr(ByVal d As Date) As String
            Return d.ToString(UserDateFormat)
        End Function

        <Extension()>
        Public Function ToTimeStr(ByVal d As Date) As String
            Return d.ToString(UserTimeFormat)
        End Function

        <Extension()>
        Public Function ToTimeffff(ByVal d As Date) As String
            Return d.ToString("HH:mm:ss.ffff")
        End Function


        <Extension()>
        Public Function ToDate(ByVal b As Double) As Date
            Return Date.FromOADate(b)
        End Function

        <Extension()>
        Public Function ToDateStr(ByVal b As Double) As String
            Return b.ToDate.ToDateStr
        End Function

        <Extension()>
        Public Function ToDateTimeStr(ByVal b As Double) As String
            Return b.ToDate.ToDateTimeStr
        End Function
    End Module
End Namespace

