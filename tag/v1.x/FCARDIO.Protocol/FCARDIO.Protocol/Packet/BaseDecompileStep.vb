Imports DotNetty.Buffers
Imports DoNetDrive.Core.Packet

Namespace Packet
    ''' <summary>
    ''' 抽象的命令包解析步骤
    ''' </summary>
    Public MustInherit Class BaseDecompileStep(Of T As {BasePacket, New})
        Implements INDecompileStep(Of T)

        ''' <summary>
        ''' buf需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        MustOverride ReadOnly Property ReadBufLen() As Integer


        ''' <summary>
        ''' 开始逐字节解析缓冲区中的数据
        ''' </summary>
        ''' <param name="decompile"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Overridable Function DecompileStep(decompile As BaseDecompile(Of T), value As Byte) As Boolean Implements INDecompileStep(Of T).DecompileStep

            Dim buf = decompile._Buf
            decompile.AddBuf(value)
            If buf.ReadableBytes() = ReadBufLen() Then
                If decompile._Packet Is Nothing Then
                    decompile._Packet = New T
                End If
                DecompileNext(decompile, buf, decompile._Packet)
                buf.Clear()
            End If
            Return False
        End Function

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected MustOverride Sub DecompileNext(decompile As BaseDecompile(Of T), buf As IByteBuffer, pck As T)

        ''' <summary>
        ''' 累加和校验
        ''' </summary>
        Protected Sub AddSumCheck(buf As IByteBuffer, pck As T)
            buf.MarkReaderIndex()

            Do While buf.ReadableBytes > 0
                pck.Check += buf.ReadByte
            Loop

            buf.ResetReaderIndex()
        End Sub
    End Class

End Namespace
