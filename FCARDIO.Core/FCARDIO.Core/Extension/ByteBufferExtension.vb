Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports DotNetty.Buffers

Namespace Extension
    Public Module ByteBufferExtension
        <StructLayout(LayoutKind.Explicit)>
        Public Structure UnionInt32
            <FieldOffset(0)>
            Public IntValue As Int32
            <FieldOffset(0)>
            Public UIntValue As UInt32
        End Structure

        <StructLayout(LayoutKind.Explicit)>
        Public Structure UnionInt64
            <FieldOffset(0)>
            Public IntValue As Int64
            <FieldOffset(0)>
            Public UIntValue As UInt64
        End Structure


        ''' <summary>
        ''' 将UINT32类型写入到缓冲区
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <param name="v"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function WriteUnsignedInt(buf As IByteBuffer, v As UInt32) As IByteBuffer
            Dim t As UnionInt32
            t.UIntValue = v
            Return buf.WriteInt(t.IntValue)
        End Function

        ''' <summary>
        ''' 将UINT64类型写入到缓冲区
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <param name="v"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function WriteUInt64(buf As IByteBuffer, v As UInt64) As IByteBuffer
            Dim t As UnionInt64
            t.UIntValue = v
            Return buf.WriteLong(t.IntValue)
        End Function

        <Extension()>
        Public Function ToHex(buf As IByteBuffer) As String
            Return ByteBufferUtil.HexDump(buf)
        End Function
    End Module

End Namespace
