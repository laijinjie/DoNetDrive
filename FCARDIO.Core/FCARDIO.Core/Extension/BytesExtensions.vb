Imports System.Runtime.CompilerServices
Imports DoNetDrive.Core.Util

Namespace Extension
    Public Module BytesExtensions

        ''' <summary>
        ''' 转换字节编码为字符串
        ''' </summary>
        ''' <param name="b"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function GetString(ByVal b() As Byte) As String
            If b(0) = 0 Or b(0) = 255 Then
                Return String.Empty
            End If
            Dim iIndex = Array.FindIndex(b, Function(x) x = 0)
            If iIndex > 0 Then
                b = b.Copy(0, iIndex)
            End If

            Return Conversion.ASCII.GetString(b)
        End Function




        ''' <summary>
        ''' 转换字节数组为十六进制字符串
        ''' </summary>
        ''' <param name="b"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToHex(ByVal b() As Byte) As String
            Return Conversion.ByteToHex(b)
        End Function


        <Extension()>
        Public Function ToInt32(ByVal b() As Byte) As UInt32
            Return Conversion.ByteToInt32(b)
        End Function

        <Extension()>
        Public Function ToInt32(ByVal b() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 4) As UInt32
            Return Conversion.ByteToInt32(b, iBeginIndex, iLen)
        End Function


        <Extension()>
        Public Function ToInt24(ByVal b() As Byte) As UInt32
            Return Conversion.ByteToInt32(b, 0, 3)
        End Function

        <Extension()>
        Public Function ToInt24(ByVal b() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 3) As UInt32
            Return Conversion.ByteToInt32(b, iBeginIndex, iLen)
        End Function



        <Extension()>
        Public Function ToInt64(ByVal b() As Byte) As UInt64
            Return Conversion.BytesToLong(b)
        End Function

        <Extension()>
        Public Function ToInt64(ByVal b() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 8) As UInt64
            Return Conversion.BytesToLong(b, iBeginIndex, iLen)
        End Function


        <Extension()>
        Public Function ToInt32Rev(ByVal b() As Byte) As UInt32
            Dim br = b.Clone
            Array.Reverse(br)
            Return Conversion.ByteToInt32(br)
        End Function

        <Extension()>
        Public Function ToInt32Rev(ByVal b() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 4) As UInt32
            Dim br = b.Copy(iBeginIndex, iLen)
            Array.Reverse(br)
            Return Conversion.ByteToInt32(br)
        End Function



        <Extension()>
        Public Function ToInt16(ByVal b() As Byte) As UInt16
            Return Conversion.ByteToInt16(b)
        End Function


        <Extension()>
        Public Function ToInt16(ByVal b() As Byte, ByVal iBeginIndex As Integer) As UInt16
            Return Conversion.ByteToInt16(b, iBeginIndex)
        End Function



        <Extension()>
        Public Function Copy(ByVal b() As Byte, ByVal iIndex As Int32, ByVal iCount As Int32) As Byte()
            Return Arrays(Of Byte).copyOfRange(b, iIndex, iCount)
        End Function


        <Extension()>
        Public Function BytesEquals(ByVal b() As Byte, ByVal eb As Byte()) As Boolean
            Return Arrays(Of Byte).BytesEquals(b, eb)
        End Function


        <Extension()>
        Public Function toByte(ByVal b As Boolean) As Byte
            If b Then Return 1
            Return 0
        End Function
    End Module
End Namespace

