Imports System.Net
Imports DotNetty.Buffers

Namespace Connector.UDP

    Public Interface IUDPServerConnector
        ''' <summary>
        ''' UDP发送数据
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <param name="oPacketRemote"></param>
        ''' <returns></returns>
        Function WriteByteBufByUDP(buf As IByteBuffer, oPacketRemote As IPEndPoint) As Task

        ''' <summary>
        ''' 服务器关闭事件
        ''' </summary>
        Event ServerCloseEvent(sender As IUDPServerConnector)

    End Interface


End Namespace
