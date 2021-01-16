Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.Packet

Namespace FrameCommand
    Public MustInherit Class AbstractFramePacket
        Inherits BasePacket

        ''' <summary>
        ''' 信息代码
        ''' </summary>
        Public Code As UInt32
        ''' <summary>
        ''' 设备SN
        ''' </summary>
        Public SN As Byte()
        ''' <summary>
        ''' 通讯密码
        ''' </summary>
        Public Password As Byte()




#Region "改变数据包结构"

        ''' <summary>
        ''' 设置数据包绑定的命令详情
        ''' </summary>
        ''' <param name="dtl"></param>
        Public MustOverride Sub SetCommandDetail(dtl As Core.Command.INCommandDetail)


        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public MustOverride Sub SetPacket(ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)


        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        Public Overridable Sub SetPacket(ct As Byte, ci As Byte, cp As Byte)
            SetPacket(ct, ci, cp, 0, Nothing)
        End Sub
#End Region


#Region "网络标志的修改"


        ''' <summary>
        ''' 设置当前的数据包为UDP广播数据包,UDP广播发送和接收数据时使用
        ''' </summary>
        Public MustOverride Sub SetUDPBroadcastPacket()


        ''' <summary>
        ''' 设置当前的数据包为正常播数据包（默认状态就是正常，除非经过修改，否则不必调用此函数）
        ''' </summary>
        Public MustOverride Sub SetNormalPacket()

        ''' <summary>
        ''' 设置当前的数据包为正常播数据包（默认状态就是正常，除非经过修改，否则不必调用此函数）
        ''' </summary>
        Public Overridable Sub SetNormalPacket(iCode As Integer)
            Code = iCode
        End Sub
#End Region

    End Class
End Namespace

