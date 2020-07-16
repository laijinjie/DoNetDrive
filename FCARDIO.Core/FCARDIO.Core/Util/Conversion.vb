Imports System.Runtime.InteropServices
Imports System.Text
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


    Public Class Conversion
        Public Shared ASCII As Encoding = Encoding.ASCII 'ASCII编码
        Private Shared mHexDigits() As Byte = ASCII.GetBytes("0123456789ABCDEF") '字节转十六进制时十六进制字符串代码表
        Private Shared mByteDigits() As Byte '十六进制转字节，十六进制每个字符对应的码值表


        ''' <summary>
        ''' 字节数组转长整形
        ''' </summary>
        ''' <param name="b">字节数组 1-8个元素</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BytesToLong(ByVal b() As Byte) As UInt64
            Dim lValue As UInt64 = 0
            Dim i As Byte
            If b Is Nothing Then Return 0
            If b.Length = 0 Then Return 0

            Dim iLen = b.Length
            If iLen > 8 Then
                iLen = 8
            End If
            iLen -= 1
            For i = 0 To iLen
                lValue = lValue + b(i)
                If Not i = iLen Then
                    lValue = lValue << 8
                End If
            Next

            Return lValue
        End Function

        ''' <summary>
        ''' 字节数组转长整形
        ''' </summary>
        ''' <param name="b">需要计算的数组</param>
        ''' <param name="iBeginIndex">数组起始索引号</param>
        ''' <param name="iLen">数据长度 最大8字节</param>
        ''' <returns></returns>
        Public Shared Function BytesToLong(ByVal b() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 4) As UInt64
            Dim lValue As UInt64 = 0
            Dim i As Byte
            If b Is Nothing Then Return 0
            If b.Length = 0 Then Return 0
            If iBeginIndex < 0 Or iBeginIndex > b.Length Then Return 0
            If iLen + iBeginIndex > b.Length Then Return 0



            If iLen > 8 Then iLen = 8
            iLen -= 1
            For i = 0 To iLen
                lValue = lValue + b(iBeginIndex + i)
                If Not i = iLen Then
                    lValue = lValue << 8
                End If
            Next

            Return lValue
        End Function



        ''' <summary>
        ''' 将长整形转为字节数组
        ''' </summary>
        ''' <param name="lValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function LongToBytes(ByVal lValue As UInt64) As Byte()
            Dim b(7) As Byte
            Dim l As UInt64 = 255
            For i = 7 To 0 Step -1
                b(i) = lValue And l

                lValue = lValue >> 8
                If lValue = 0 Then Exit For
            Next

            Return b
        End Function

        ''' <summary>
        ''' 字节转十六进制
        ''' </summary>
        ''' <param name="bData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToHex(ByVal bData() As Byte) As String
            Dim bHex() As Byte
            Dim i As Long, lIndex As Long
            If bData.Length = 0 Then Return "00"

            '初始化操作
            ReDim bHex(UBound(bData) * 2 + 1)

            '开始转换
            lIndex = 0
            For i = 0 To UBound(bData)
                bHex(lIndex) = mHexDigits(bData(i) \ 16) : lIndex = lIndex + 1
                bHex(lIndex) = mHexDigits(bData(i) Mod 16) : lIndex = lIndex + 1
            Next

            ByteToHex = ASCII.GetString(bHex)
        End Function

        ''' <summary>
        ''' 十六进制转字节
        ''' </summary>
        ''' <param name="sHex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function HexToByte(ByVal sHex As String) As Byte()
            '十六进制转字节
            Dim i As Long
            Dim bbuf() As Byte
            ReDim bbuf(0)

            If Len(sHex) Mod 2 = 1 Then
                sHex = "0" & sHex
            End If

            If Len(sHex) = 0 Then
                Return bbuf
            End If

            '初始化操作
            If mByteDigits Is Nothing Then
                Dim bTmp() As Byte
                ReDim mByteDigits(255)
                bTmp = ASCII.GetBytes("0123456789ABCDEF")
                For i = 0 To UBound(bTmp)
                    mByteDigits(bTmp(i)) = i
                Next
                bTmp = ASCII.GetBytes("abcdef")
                For i = 0 To UBound(bTmp)
                    mByteDigits(bTmp(i)) = i + 10
                Next
            End If

            '开始转换
            bbuf = ASCII.GetBytes(sHex)
            Dim iData As Byte, lIndex As Long
            Dim bRtn() As Byte
            ReDim bRtn((UBound(bbuf) - 1) / 2)

            lIndex = 0
            For i = 0 To UBound(bbuf)
                iData = mByteDigits(bbuf(i)) * 16 '高位
                i = i + 1
                iData = iData + mByteDigits(bbuf(i)) '低位
                bRtn(lIndex) = iData
                lIndex = lIndex + 1
            Next
            HexToByte = bRtn
        End Function

        ''' <summary>
        ''' 字节转Bit
        ''' </summary>
        ''' <param name="iNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToBit(ByVal iNum As Byte) As Byte()
            Dim bList As BitArray = New BitArray({iNum})
            Dim iBit(7) As Byte, iIndex As Byte = 0

            For Each bValue As Boolean In bList
                iBit(iIndex) = IIf(bValue, 1, 0)

                iIndex += 1
            Next

            Return iBit
        End Function

        ''' <summary>
        ''' Bit转字节
        ''' </summary>
        ''' <param name="iBits">存储位数据的8元素数组</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BitToByte(ByVal iBits() As Byte) As Byte
            Dim bList As BitArray = New BitArray(8)
            Dim iValue() As Byte = {0}
            For i As Byte = 0 To 7
                bList(i) = (iBits(i) = 1)
            Next
            '将bit数组转换为字节
            bList.CopyTo(iValue, 0)
            Return iValue(0)
        End Function

        ''' <summary>
        ''' 字节转BCD码
        ''' </summary>
        ''' <param name="iNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToBCDNUM(ByVal iNum As Byte) As Byte
            'Return Byte.Parse(iNum.ToString, Globalization.NumberStyles.HexNumber)
            Return (iNum \ 10) * 16 + (iNum Mod 10)
        End Function

        ''' <summary>
        ''' 字节转BCD码
        ''' </summary>
        ''' <param name="iNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToBCDNUM(ByVal iNum() As Byte) As Byte()
            For i As Integer = 0 To iNum.Length - 1
                iNum(i) = (iNum(i) \ 10) * 16 + (iNum(i) Mod 10)
            Next
            Return iNum
        End Function

        ''' <summary>
        ''' BCD转字节值
        ''' </summary>
        ''' <param name="iNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BCDToByte(ByVal iNum As Byte) As Byte
            Return ((iNum \ 16) * 10) + (iNum Mod 16)
        End Function

        ''' <summary>
        ''' BCD转字节值
        ''' </summary>
        ''' <param name="iNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BCDToByte(ByVal iNum() As Byte) As Byte()
            For i As Integer = 0 To iNum.Length - 1
                iNum(i) = ((iNum(i) \ 16) * 10) + (iNum(i) Mod 16)
            Next
            Return iNum
        End Function

        ''' <summary>
        ''' 无符号8位整形，转换为有符号8位整形。
        ''' </summary>
        ''' <param name="bValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToSByte(ByVal bValue As Byte) As SByte
            Dim bNum As SByte
            bNum = 0
            If bValue > 128 Then
                bNum = -(128 - (bValue - 128))
            ElseIf bValue = 128 Then
                bNum = -128
            Else
                bNum = bValue
            End If
            Return bNum

            'Return SByte.Parse(bValue.ToString("X2"), Globalization.NumberStyles.HexNumber)
        End Function

        ''' <summary>
        ''' 有符号8位整形，转换为无符号8位整形。
        ''' </summary>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SBytetobyte(ByVal b As SByte) As Byte
            Dim bNum As Byte

            If b < 0 Then
                If b = -128 Then
                    bNum = 128
                Else
                    bNum = -b
                    bNum = (128 - bNum) + 128
                End If

            Else
                bNum = b
            End If

            Return bNum
        End Function



        ''' <summary>
        ''' 字节数组转短整型 Int16 高位在前
        ''' </summary>
        ''' <param name="bData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToInt16(ByVal bData() As Byte) As UShort
            Dim iValue As UInt16 = bData(0)
            iValue = (iValue << 8) + bData(1)
            Return iValue
        End Function

        ''' <summary>
        ''' 字节数组转短整型 Int16 高位在前
        ''' </summary>
        ''' <param name="bData">需要计算的数组</param>
        ''' <param name="iBeginIndex">数组起始索引号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToInt16(ByVal bData() As Byte, ByVal iBeginIndex As Integer) As UShort
            Dim iValue As UInt16 = bData(iBeginIndex)
            iValue = (iValue << 8) + bData(iBeginIndex + 1)
            Return iValue
        End Function


        ''' <summary>
        ''' 字节数组转 Int32 整形 高位在前
        ''' </summary>
        ''' <param name="bData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToInt32(ByVal bData() As Byte) As UInt32
            Dim iValue As UInt32 = 0
            If bData Is Nothing Then Return 0

            Dim iLen As Byte = (bData.Length - 1)
            For i As Byte = 0 To iLen
                iValue = iValue << 8
                iValue += bData(i)
                If i = 3 Then Exit For
            Next
            Return iValue
        End Function

        ''' <summary>
        ''' 字节数组转 Int32 整形 高位在前
        ''' </summary>
        ''' <param name="bData">需要计算的数组</param>
        ''' <param name="iBeginIndex">数组起始索引号</param>
        ''' <param name="iLen">数据长度 最大4字节</param>
        ''' <returns></returns>
        Public Shared Function ByteToInt32(ByVal bData() As Byte, ByVal iBeginIndex As Integer, Optional ByVal iLen As Integer = 4) As UInt32
            Dim iValue As UInt32 = 0
            If bData Is Nothing Then Return 0
            If iBeginIndex < 0 Or iBeginIndex > bData.Length Then Return 0
            If iLen + iBeginIndex > bData.Length Then Return 0

            If iLen > 4 Then iLen = 4
            iLen -= 1
            For i As Byte = 0 To iLen
                iValue = iValue << 8
                iValue += bData(iBeginIndex + i)
            Next
            Return iValue
        End Function





        Public Shared Function Int32To3Byte(ByVal iValue As UInt32) As Byte()
            Dim b(2) As Byte
            For i = 2 To 0 Step -1
                b(i) = iValue And 255

                iValue = iValue >> 8
                If iValue = 0 Then Exit For
            Next

            Return b
        End Function

        ''' <summary>
        ''' Int32 整形 转 字节数组 高位在前
        ''' </summary>
        ''' <param name="iValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Int32ToByte(ByVal iValue As UInt32) As Byte()
            Dim b(3) As Byte
            For i = 3 To 0 Step -1
                b(i) = iValue And 255

                iValue = iValue >> 8
                If iValue = 0 Then Exit For
            Next

            Return b
        End Function

        ''' <summary>
        ''' Int16 整形 转 字节数组  高位在前
        ''' </summary>
        ''' <param name="iValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Int16ToByte(ByVal iValue As UInt16) As Byte()
            Dim b(1) As Byte
            Dim l As UInt16 = 255
            b(1) = iValue And l
            iValue = iValue >> 8
            b(0) = iValue And l
            Return b
        End Function



        ''' <summary>
        ''' BCD格式日期字节数组转换为日期类型   需要传入6字节
        ''' 格式 yyMMddHHmmss
        ''' </summary>
        ''' <param name="bData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToDateTime(ByVal bData() As Byte) As Date
            For i As Integer = 0 To bData.Length - 1
                If Not IsNumeric(bData(i).ToString("X")) Then
                    Return Date.MinValue
                End If
            Next

            bData = BCDToByte(bData)
            If bData.Length < 6 Then
                Return Date.MinValue
            End If

            For i As Integer = 0 To 2
                If bData(i) = 0 Then
                    Return Date.MinValue
                End If
            Next


            Return New Date(2000 + CLng(bData(0)), bData(1), bData(2), bData(3), bData(4), bData(5))
        End Function


        ''' <summary>
        ''' 字节数组转换为日期类型  BCD日期 需要传入3字节
        ''' </summary>
        ''' <param name="bData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ByteToDate(ByVal bData() As Byte) As Date
            For i As Integer = 0 To bData.Length - 1
                If Not IsNumeric(Convert.ToString(bData(i), 16)) Then
                    Return Date.MinValue
                End If
            Next

            bData = BCDToByte(bData)
            If bData.Length < 3 Then
                Return Date.MinValue
            End If

            For i As Integer = 0 To 2
                If bData(i) = 0 Then
                    Return Date.MinValue
                End If
            Next

            Try
                Return New Date(2000 + CLng(bData(0)), bData(1), bData(2))
            Catch ex As Exception
                Return Date.MinValue
            End Try
        End Function

        Public Shared Function GetBCDTime() As Byte()
            Return GetBCDTime(Date.Now)
        End Function

        Public Shared Function GetBCDTime(ByVal oDate As Date) As Byte()
            Dim bData(6) As Byte
            Dim sHex As String
            sHex = Format(oDate, "yyMMddHHmmss")
            Array.Copy(HexToByte(sHex), bData, 6)
            bData(6) = Weekday(oDate, FirstDayOfWeek.Monday) '周
            Return bData
        End Function

    End Class
End Namespace



