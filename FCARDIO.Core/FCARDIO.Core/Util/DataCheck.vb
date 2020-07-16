Imports DoNetDrive.Core.Extension

Namespace Util
    Public Class DataCheck
        ''' <summary>
        ''' 检查字符串是否为十六进制
        ''' </summary>
        ''' <param name="sValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsHex(ByVal sValue As String) As Boolean
            If String.IsNullOrWhiteSpace(sValue) Then
                Return False
            End If
            Dim sFormat As String = "1234567890ABCDEFabcdef"
            For i As Integer = 0 To sValue.Length - 1
                If InStr(sFormat, sValue.Substring(i, 1)) = 0 Then
                    Return False
                End If
            Next
            Return True
        End Function


        Public Shared Function IsNum(ByVal sValue As String) As Boolean
            If String.IsNullOrWhiteSpace(sValue) Then
                Return False
            End If

            Dim sFormat As String = "1234567890"
            For i As Integer = 0 To sValue.Length - 1
                If sFormat.IndexOf(sValue.Substring(i, 1)) = -1 Then
                    Return False
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' 检查十六进制字符串格式及长度
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckHexLen(ByVal sHex As String, ByVal iLen As Integer) As String
            sHex = sHex.ToUpper
            If sHex.Length > iLen Then
                sHex = sHex.Substring(0, iLen)
            End If

            If sHex.Length < iLen Then
                sHex = sHex & New String("F", iLen - sHex.Length)
            End If

            If Not IsHex(sHex) Then
                sHex = New String("F", iLen)
            End If

            Return sHex
        End Function

        ''' <summary>
        ''' 检查字符串是否为指定长度
        ''' </summary>
        ''' <param name="sValue">待检查字符串</param>
        ''' <param name="iLength">需要的长度</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckStringLen(ByVal sValue As String, ByVal iLength As Integer, Optional ByVal sNullString As String = " ") As String
            If (String.Equals(sValue, String.Empty)) Then
                sValue = New String(sNullString, iLength)
            End If
            If sValue.Length > iLength Then
                sValue = sValue.Substring(0, iLength)
            End If
            If sValue.Length < iLength Then
                sValue = sValue & New String(sNullString, iLength - sValue.Length)
            End If

            If sValue.Length > iLength Then
                sValue = sValue.Substring(0, iLength)
            End If

            Return sValue
        End Function


        ''' <summary>
        ''' 检查输入的缓冲区长度是否达到指定最大值
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckBuffLen(ByVal bBuf() As Byte, ByVal iMax As Integer) As Byte()
            If bBuf Is Nothing Then
                ReDim bBuf(iMax - 1)
                Return bBuf
            End If
            If bBuf.Length = iMax Then Return bBuf

            If bBuf.Length > iMax Then
                bBuf = bBuf.Copy(0, iMax)
            ElseIf bBuf.Length < iMax Then
                Dim bList = New List(Of Byte)
                bList.Capacity = iMax
                bList.AddRange(bBuf)
                Dim b() As Byte
                ReDim b(iMax - bBuf.Length - 1)
                bList.AddRange(b)
                bBuf = bList.ToArray
                bList = Nothing
            End If
            Return bBuf
        End Function




    End Class
End Namespace

