Imports System.Runtime.InteropServices
Imports DotNetty.Buffers

Namespace Util
    ''' <summary>
    ''' 可处理12个字节的大数据类型
    ''' </summary>
    <StructLayout(LayoutKind.Explicit)>
    Public Structure BigInt
        ''' <summary>
        ''' 最大包含12字节的整数
        ''' </summary>
        <FieldOffset(0)>
        Public BigValue As Decimal

        ''' <summary>
        ''' 小数点精度
        ''' </summary>
        <FieldOffset(2)>
        Public FloatDigit As Byte

        ''' <summary>
        ''' 符号位  0--正数，0x80--负数
        ''' </summary>
        <FieldOffset(3)>
        Public Negative As Byte

        ''' <summary>
        ''' 前4字节
        ''' </summary>
        <FieldOffset(4)>
        Public UIntHeadValue As UInt32
        ''' <summary>
        ''' 前4字节
        ''' </summary>
        <FieldOffset(4)>
        Public IntHeadValue As Int32

        ''' <summary>
        ''' 第4字节
        ''' </summary>
        <FieldOffset(4)>
        Public ByteValue_4 As Byte

        ''' <summary>
        ''' 后8字节
        ''' </summary>
        <FieldOffset(8)>
        Public Int64Value As Long
        ''' <summary>
        ''' 后8字节
        ''' </summary>
        <FieldOffset(8)>
        Public UInt64Value As ULong

        ''' <summary>
        ''' 后4字节
        ''' </summary>
        <FieldOffset(8)>
        Public Int32Value As Integer
        ''' <summary>
        ''' 后4字节
        ''' </summary>
        <FieldOffset(8)>
        Public UInt32Value As UInteger


        Public Function toBytes() As Byte()
            Dim bByte(11) As Byte
            Dim iAndValue As ULong = 255
            Dim lValue = UInt64Value
            For i = 11 To 4 Step -1
                bByte(i) = CByte(lValue And iAndValue)

                lValue = lValue >> 8
                If lValue = 0 Then Exit For
            Next


            If UIntHeadValue > 0 Then
                lValue = UIntHeadValue
                For i = 3 To 0 Step -1
                    bByte(i) = CByte(lValue And iAndValue)

                    lValue = lValue >> 8
                    If lValue = 0 Then Exit For
                Next
            End If
            Return bByte
        End Function

        ''' <summary>
        ''' 获取指定字节数的数组
        ''' </summary>
        ''' <param name="iLen"></param>
        ''' <returns></returns>
        Public Function toBytes(ByVal iLen As Integer) As Byte()
            Dim bByte() As Byte
            ReDim bByte(iLen - 1)

            Dim iAndValue As ULong = 255
            Dim lValue = UInt64Value
            Dim iEndIndex = 0
            If iLen > 8 Then
                iEndIndex = iLen - 8
            End If
            For i = iLen - 1 To iEndIndex Step -1
                bByte(i) = CByte(lValue And iAndValue)

                lValue = lValue >> 8
                If lValue = 0 Then Exit For
            Next


            If UIntHeadValue > 0 Then
                lValue = UIntHeadValue
                iEndIndex = 0
                Dim iIndex = iLen - 9
                iEndIndex = iIndex - 3
                If iEndIndex < 0 Then iEndIndex = 0
                For i = iIndex To iEndIndex Step -1
                    bByte(i) = CByte(lValue And iAndValue)

                    lValue = lValue >> 8
                    If lValue = 0 Then Exit For
                Next
            End If
            Return bByte
        End Function

        ''' <summary>
        ''' 将数值写入缓冲区
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <param name="iLen"></param>
        Public Sub toBytes(buf As IByteBuffer, ByVal iLen As Integer)
            If iLen = 12 Then
                buf.WriteInt(IntHeadValue)
                buf.WriteLong(Int64Value)
            End If
            If iLen = 9 Then
                buf.WriteByte(ByteValue_4)
                buf.WriteLong(Int64Value)
            End If
            If iLen = 8 Then
                buf.WriteLong(Int64Value)
            End If
            If iLen = 4 Then
                buf.WriteInt(Int32Value)
            End If
        End Sub

        ''' <summary>
        ''' 从缓冲区中读取指定字节
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <param name="iLen"></param>
        Public Sub SetBytes(buf As IByteBuffer, ByVal iLen As Integer)
            BigValue = 0
            If iLen = 12 Then
                IntHeadValue = buf.ReadInt()
                Int64Value = buf.ReadLong()
            End If
            If iLen = 9 Then
                IntHeadValue = buf.ReadByte()
                Int64Value = buf.ReadLong()
            End If
            If iLen = 8 Then
                Int64Value = buf.ReadLong()
            End If
            If iLen = 4 Then
                Int32Value = buf.ReadInt()
            End If
        End Sub

        Public Sub setBytes(ByVal bByte() As Byte)
            BigValue = 0
            Dim lValue As ULong = 0
            Dim i As Integer, iLen As Integer
            Dim iIndex As Integer
            iLen = bByte.Length
            iIndex = 0
            If iLen > 8 Then iIndex = iLen - 8

            iLen = iLen - 1
            For i = iIndex To iLen
                lValue = lValue + bByte(i)
                If Not i = iLen Then
                    lValue = lValue << 8
                End If
            Next
            UInt64Value = lValue

            iLen += 1
            If iLen > 8 Then
                iIndex = 0
                If iLen > 12 Then iIndex = iLen - 12
                iLen = iLen - 9
                lValue = 0
                For i = iIndex To iLen
                    lValue = lValue + bByte(i)
                    If Not i = iLen Then
                        lValue = lValue << 8
                    End If
                Next
                UIntHeadValue = lValue
            End If
        End Sub


        Public Sub setBytes(ByVal bByte() As Byte, ByVal iBeginIndex As Integer, ByVal iByteCount As Integer)
            BigValue = 0
            Dim lValue As ULong = 0
            Dim i As Integer, iLen As Integer
            Dim iIndex As Integer
            iLen = iByteCount
            iIndex = 0
            If iLen > 8 Then iIndex = iLen - 8

            iLen = iLen - 1
            For i = iIndex To iLen
                lValue = lValue + bByte(iBeginIndex + i)
                If Not i = iLen Then
                    lValue = lValue << 8
                End If
            Next
            UInt64Value = lValue

            iLen += 1
            If iLen > 8 Then
                iIndex = 0
                If iLen > 12 Then iIndex = iLen - 12
                iLen = iLen - 9
                lValue = 0
                For i = iIndex To iLen
                    lValue = lValue + bByte(iBeginIndex + i)
                    If Not i = iLen Then
                        lValue = lValue << 8
                    End If
                Next
                UIntHeadValue = lValue
            End If
        End Sub


    End Structure
End Namespace

