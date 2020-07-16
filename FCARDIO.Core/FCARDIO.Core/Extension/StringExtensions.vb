Imports System.Runtime.CompilerServices
Imports DoNetDrive.Core.Util


Namespace Extension
    Public Module StringExtensions

        <Extension()>
        Public Sub Print(ByVal aString As String)
            Console.WriteLine(aString)
        End Sub


        <Extension()>
        Public Function ToInt32(ByVal aString As String) As Int32
            Dim iValue As Int32 = 0
            If Int32.TryParse(aString, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function



        <Extension()>
        Public Function ToUInt32(ByVal aString As String) As UInt32
            Dim iValue As UInt32 = 0
            If UInt32.TryParse(aString, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function


        <Extension()>
        Public Function HexToUInt32(ByVal aString As String) As UInt32
            Dim iValue As UInt32 = 0
            If UInt32.TryParse(aString, Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function


        <Extension()>
        Public Function ToDouble(ByVal aString As String) As Double
            Dim iValue As Double = 0
            If Double.TryParse(aString, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function

        <Extension()>
        Public Function ToLong(ByVal aString As String) As Long
            Dim iValue As Long = 0
            If Long.TryParse(aString, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function

        <Extension()>
        Public Function ToUInt64(ByVal aString As String) As UInt64
            Dim iValue As UInt64 = 0
            If UInt64.TryParse(aString, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function


        <Extension()>
        Public Function HexToUInt64(ByVal aString As String) As UInt64
            Dim iValue As UInt64 = 0
            If UInt64.TryParse(aString, Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture, iValue) Then
                Return iValue
            Else
                Return 0
            End If
        End Function


        ''' <summary>
        ''' 检查是否为数字
        ''' </summary>
        ''' <param name="aString"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function IsNum(ByVal aString As String) As Boolean
            Dim l As Decimal
            If String.IsNullOrWhiteSpace(aString) Then
                Return False
            End If

            Return Decimal.TryParse(aString, l)
        End Function

        ''' <summary>
        ''' 检查字符串是否为十六进制
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function IsHex(ByVal s As String) As Boolean
            Return DataCheck.IsHex(s)
        End Function

        ''' <summary>
        ''' 十六进制转为字节数字
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function HexToByte(ByVal s As String) As Byte()
            Return Conversion.HexToByte(s)
        End Function



        ''' <summary>
        ''' 获取字符串的字节编码
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function GetBytes(ByVal s As String) As Byte()
            If String.IsNullOrEmpty(s) Then
                Return Nothing
            End If
            Return Conversion.ASCII.GetBytes(s)
        End Function


        <Extension()>
        Public Function SF(ByVal s As String, ParamArray args() As Object) As String
            Return String.Format(s, args)
        End Function

        <Extension()>
        Public Function SplitTrim(ByVal s As String, ByVal sSplit As String) As String()
            Return s.Split({sSplit}, StringSplitOptions.RemoveEmptyEntries)
        End Function

        <Extension()>
        Public Function IsSame(ByVal s As String, ByVal d As String) As Boolean
            Return String.Compare(s, d, True) = 0
        End Function



        ''' <summary>
        ''' 填充字符串
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function FillString(ByVal s As String, ByVal iLen As Integer, ByVal sFillStr As String) As String
            Return DataCheck.CheckStringLen(s, iLen, sFillStr)

        End Function


    End Module
End Namespace

