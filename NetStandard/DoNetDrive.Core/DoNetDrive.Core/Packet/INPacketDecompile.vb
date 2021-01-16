Imports DotNetty.Buffers

Namespace Packet
    ''' <summary>
    ''' 用于将数据解析成指定格式的数据包的解析器
    ''' </summary>
    Public Interface INPacketDecompile
        Inherits IDisposable

        ''' <summary>
        ''' 将数据解析成特定的数据包，并返回结果
        ''' </summary>
        ''' <param name="buf">待解析数据缓冲区</param>
        ''' <param name="retPacketList">已解析的包会放在这个列表中</param>
        ''' <returns>解析出完整包则返回True</returns>
        Function Decompile(buf As IByteBuffer, retPacketList As List(Of INPacket)) As Boolean
    End Interface

End Namespace

