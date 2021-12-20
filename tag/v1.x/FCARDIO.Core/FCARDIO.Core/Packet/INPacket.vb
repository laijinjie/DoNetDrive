Imports DotNetty.Buffers

Namespace Packet
    Public Interface INPacket
        Inherits IDisposable

        ''' <summary>
        ''' 获取数据包的打包后的ByteBuf，用于发送数据
        ''' </summary>
        ''' <param name="Allocator">用于分配ByteBuf的分配器</param>
        ''' <returns></returns>
        Function GetPacketData(Allocator As IByteBufferAllocator) As IByteBuffer

        ''' <summary>
        ''' 指示数据包可以释放数据缓冲区
        ''' </summary>
        Sub ReleaseDataBuf()
    End Interface

End Namespace
