Imports DotNetty.Buffers

Namespace Data
    ''' <summary>
    ''' 抽象INData接口，统一GetBytes的实现
    ''' </summary>
    Public MustInherit Class AbstractData
        Implements INData
        ''' <summary>
        ''' 将一个缓冲区 ByteBuf 设置到数据结构中
        ''' </summary>
        ''' <param name="databuf">需要设置到结构中的ByteBuf</param>
        Public MustOverride Sub SetBytes(databuf As IByteBuffer) Implements INData.SetBytes

        ''' <summary>
        ''' 获取数据的字节长度
        ''' </summary>
        ''' <returns>返回数据的字节长度</returns>
        Public MustOverride Function GetDataLen() As Integer Implements INData.GetDataLen

        ''' <summary>
        ''' 获取一个 ByteBuf 此 缓冲中包含了此数据结构的所有数据
        ''' </summary>
        ''' <returns>返回一个包含此结构的ByteBuf</returns>
        Public Function GetBytes() As IByteBuffer Implements INData.GetBytes
            Return GetBytes(UnpooledByteBufferAllocator.Default.Buffer(GetDataLen()))
        End Function

        ''' <summary>
        ''' 将数据序列化到指定的 ByteBuf 中
        ''' </summary>
        ''' <param name="databuf">具有足够缓冲空间的bytebuf</param>
        ''' <returns>返回传入的bytebuf</returns>
        Public MustOverride Function GetBytes(databuf As IByteBuffer) As IByteBuffer Implements INData.GetBytes

    End Class

End Namespace
