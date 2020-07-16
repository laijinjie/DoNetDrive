Imports DotNetty.Buffers
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Packet

Namespace Packet

    ''' <summary>
    ''' 定义命令包解析的步骤的统一接口形式
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Interface INDecompileStep(Of T As {BasePacket, New})
        ''' <summary>
        ''' 解析步骤，命令包完整解析后返回true
        ''' </summary>
        ''' <returns></returns>
        Function DecompileStep(decompile As BaseDecompile(Of T), value As Byte) As Boolean
    End Interface



End Namespace


