Imports System.Runtime.CompilerServices
Imports DoNetDrive.Core.Util

Namespace Extension
    Public Module NumExtensions
        ''' <summary>
        ''' 返回8个字节
        ''' </summary>
        ''' <param name="l"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToBytes(ByVal l As Long) As Byte()
            Dim v As UInt64 = 0
            If l < 0 Then
                v = l And Long.MaxValue
                Dim b = Conversion.LongToBytes(v)
                l = l >> 56
                v = l And Byte.MaxValue
                b(0) = v
                Return b
            Else
                v = l
            End If
            Return Conversion.LongToBytes(v)
        End Function

        <Extension()>
        Public Function To4Bytes(ByVal l As Long) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32ToByte(v)
        End Function

        <Extension()>
        Public Function To3Bytes(ByVal l As Long) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32To3Byte(v)
        End Function


        <Extension()>
        Public Function To2Bytes(ByVal l As Long) As Byte()
            Dim v As UInt16 = (l And UInt16.MaxValue)
            Return Conversion.Int16ToByte(v)
        End Function


        ''' <summary>
        ''' 返回8个字节
        ''' </summary>
        ''' <param name="l"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function To8Bytes(ByVal l As UInt64) As Byte()
            Return Conversion.LongToBytes(l)
        End Function

        <Extension()>
        Public Function To4Bytes(ByVal l As UInt64) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32ToByte(v)
        End Function

        <Extension()>
        Public Function To3Bytes(ByVal l As UInt64) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32To3Byte(v)
        End Function


        <Extension()>
        Public Function To2Bytes(ByVal l As UInt64) As Byte()
            Dim v As UInt16 = (l And UInt16.MaxValue)
            Return Conversion.Int16ToByte(v)
        End Function




        <Extension()>
        Public Function To4Bytes(ByVal l As Integer) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32ToByte(v)
        End Function

        <Extension()>
        Public Function To3Bytes(ByVal l As Integer) As Byte()
            Dim v As UInt32 = (l And UInt32.MaxValue)
            Return Conversion.Int32To3Byte(v)
        End Function

        <Extension()>
        Public Function To2Bytes(ByVal l As Integer) As Byte()
            Dim v As UInt16 = (l And UInt16.MaxValue)
            Return Conversion.Int16ToByte(v)
        End Function

        <Extension()>
        Public Function To2Bytes(ByVal l As UInt16) As Byte()
            Return Conversion.Int16ToByte(l)
        End Function

        <Extension()>
        Public Function To2Bytes(ByVal l As Int16) As Byte()
            Dim v As UInt16 = (l And UInt16.MaxValue)
            Return Conversion.Int16ToByte(v)
        End Function


        <Extension()>
        Public Function To4Bytes(ByVal l As UInt32) As Byte()
            Return Conversion.Int32ToByte(l)
        End Function

        <Extension()>
        Public Function To3Bytes(ByVal l As UInt32) As Byte()
            Return Conversion.Int32To3Byte(l)
        End Function

        <Extension()>
        Public Function To2Bytes(ByVal l As UInt32) As Byte()
            Dim v As UInt16 = (l And UInt16.MaxValue)
            Return Conversion.Int16ToByte(v)
        End Function
    End Module
End Namespace

