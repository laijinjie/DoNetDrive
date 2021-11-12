Imports DoNetDrive.Protocol.OnlineAccess
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.FrameCommand

Namespace Door8800
    ''' <summary>
    ''' Door8800系列命令格式的基类
    ''' 适用于：Door8800，Door8900，Door5800，Door5900，989，1882，2882
    ''' </summary>
    Public MustInherit Class Door8800Command
        Inherits AbstractFrameCommand(Of OnlineAccessPacket)
        ''' <summary>
        ''' 通讯密码空密码常量
        ''' </summary>
        Public Shared NULLPassword As String = "FFFFFFFF"
        ''' <summary>
        ''' 通讯密码默认密码的字节常量
        ''' </summary>
        Public Shared NULLPasswordBuf() As Byte = OnlineAccessCommandDetail.EmptyPassword

        ''' <summary>
        ''' 处理当前命令逻辑的数据包和 _Packet 是同一个对象
        ''' </summary>
        Protected DoorPacket As OnlineAccessPacket


        ''' <summary>
        ''' 初始化两个重要参数，并进行参数检查
        ''' </summary>
        ''' <param name="cd">表示命令详情，包含通道信息，对端信息，超时时间，重发次数</param>
        ''' <param name="par">表示此命令逻辑所需要的命令参数</param>
        Sub New(cd As INCommandDetail, par As INCommandParameter)
            MyBase.New(cd, par)
            DoorPacket = FPacket

        End Sub

        ''' <summary>
        ''' 创建命令的解码器
        ''' </summary>
        Protected Overrides Function CreateDecompile(Allocator As IByteBufferAllocator) As Core.Packet.INPacketDecompile
            Return New OnlineAccessDecompile(_Connector.GetByteBufAllocator())
        End Function

        ''' <summary>
        ''' 获取当前命令所使用的缓冲区
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetCmdBuf() As IByteBuffer
            Dim buf = DoorPacket?.CmdData
            buf?.Clear()
            Return buf
        End Function


        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected Overrides Sub Release0()

            DoorPacket = Nothing
            MyBase.Release0()
        End Sub


        ''' <summary>
        ''' 命令返回值成功
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <returns></returns>
        Protected Overrides Function CheckResponse_OK(oResultPck As OnlineAccessPacket) As Boolean
            Return (oResultPck.CmdType = &H21 And oResultPck.CmdIndex = 1)
        End Function


        ''' <summary>
        ''' 通讯密码错误
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <returns></returns>
        Protected Overrides Function CheckResponse_PasswordErr(oResultPck As OnlineAccessPacket) As Boolean
            Return (oResultPck.CmdType = &H21 And oResultPck.CmdIndex = 2)
        End Function

        ''' <summary>
        ''' 检验和错误
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <returns></returns>
        Protected Overrides Function CheckResponse_CheckSumErr(oResultPck As OnlineAccessPacket) As Boolean
            Return (oResultPck.CmdType = &H21 And oResultPck.CmdIndex = 3)
        End Function


        ''' <summary>
        ''' 指定命令的返回值中的CmdType需要增加的量
        ''' </summary>
        Protected ResultCmdTypeAddValkue As Integer = &H30

        ''' <summary>
        ''' 检查返回指令是否正确
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oResultPck As OnlineAccessPacket) As Boolean

            Return CheckResponse(oResultPck, oResultPck.DataLen)
        End Function

        ''' <summary>
        ''' 检查返回指令是否正确
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <param name="ResultDataLen">返回值的长度要求</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oResultPck As OnlineAccessPacket, ResultDataLen As Integer) As Boolean

            Return CheckResponse(oResultPck, DoorPacket.CmdType, DoorPacket.CmdIndex, DoorPacket.CmdPar, ResultDataLen)
        End Function


        ''' <summary>
        ''' 检查返回指令是否符合规则
        ''' </summary>
        ''' <param name="oResultPck">需要检查的指令--一般指接收到的数据包</param>
        ''' <param name="ResultCmdType">返回值的命令类型</param>
        ''' <param name="ResultCmdIndex">返回值的命令索引</param>
        ''' <param name="ResultCmdPar">返回值的命令参数</param>
        ''' <param name="ResultDataLen">返回值的长度要求</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oResultPck As OnlineAccessPacket,
                                                     ResultCmdType As Byte, ResultCmdIndex As Byte, ResultCmdPar As Byte,
                                                     ResultDataLen As Integer) As Boolean

            Return (oResultPck.CmdType = ResultCmdTypeAddValkue + ResultCmdType And
                oResultPck.CmdIndex = ResultCmdIndex And
                oResultPck.CmdPar = ResultCmdPar And
                oResultPck.DataLen = ResultDataLen)
        End Function

    End Class
End Namespace


