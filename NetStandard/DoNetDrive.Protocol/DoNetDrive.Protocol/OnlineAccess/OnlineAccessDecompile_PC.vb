Imports DotNetty.Buffers
Imports DoNetDrive.Core.Packet
Imports DoNetDrive.Protocol.Packet

Namespace OnlineAccess
    ''' <summary>
    ''' 数据包 解析类；<para/>
    ''' 适用于在线门禁、在线电梯、指纹机、人脸机等设备通讯
    ''' </summary>
    Public Class OnlineAccessDecompile_PC
        Inherits OnlineAccessDecompile

        ''' <summary>
        ''' 创建一个解析 OnlineAccess 类型数据包的解析器
        ''' </summary>
        ''' <param name="acr"></param>
        Public Sub New(acr As IByteBufferAllocator)
            MyBase.New(acr)
        End Sub

        ''' <summary>
        ''' 获取用于解析命令的第一个步骤
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetFirstDecompileStep() As BaseDecompileStep(Of OnlineAccessPacket)
            Return OnlineAccessDecompileStep_PC.SN
        End Function

    End Class

End Namespace
