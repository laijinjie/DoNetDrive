Imports DotNetty.Buffers

Namespace Data
    Public Interface INData
        ''' <summary>
        ''' 获取数据的字节长度
        ''' </summary>
        ''' <returns>返回数据的字节长度</returns>
        Function GetDataLen() As Integer

        ''' <summary>
        ''' 将一个缓冲区 ByteBuf 设置到数据结构中
        ''' </summary>
        ''' <param name="databuf">需要设置到结构中的ByteBuf</param>
        Sub SetBytes(databuf As IByteBuffer)

        ''' <summary>
        ''' 获取一个 ByteBuf 此 缓冲中包含了此数据结构的所有数据
        ''' </summary>
        ''' <returns>返回一个包含此结构的ByteBuf</returns>
        Function GetBytes() As IByteBuffer


        ''' <summary>
        ''' 将数据序列化到指定的 ByteBuf 中
        ''' </summary>
        ''' <param name="databuf">具有足够缓冲空间的bytebuf</param>
        ''' <returns>返回传入的bytebuf</returns>
        Function GetBytes(databuf As IByteBuffer) As IByteBuffer
    End Interface

End Namespace
