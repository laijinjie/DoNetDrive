Imports DotNetty.Buffers
Imports DoNetDrive.Core.Packet
Imports DoNetDrive.Protocol.Packet

Namespace USBDrive
    ''' <summary>
    ''' 数据包 解析类；<para/>
    ''' 适用于 USB设备的通讯数据包，适合设备 usb读写器、巡更棒
    ''' </summary>
    Public Class USBDriveDecompile
        Inherits BaseDecompile(Of USBDrivePacket)

        ''' <summary>
        ''' 创建一个解析 OnlineAccess 类型数据包的解析器
        ''' </summary>
        ''' <param name="acr"></param>
        Public Sub New(acr As IByteBufferAllocator)
            MyBase.New(acr)
            _Buf = acr.Buffer(10)
        End Sub

        ''' <summary>
        ''' 获取用于解析命令的第一个步骤
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetFirstDecompileStep() As BaseDecompileStep(Of USBDrivePacket)
            Return USBDriveDecompileStep.Addr
        End Function

    End Class

End Namespace


