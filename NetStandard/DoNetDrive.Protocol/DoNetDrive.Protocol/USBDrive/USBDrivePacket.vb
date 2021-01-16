Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers
Imports DoNetDrive.Common.Extensions
Imports DoNetDrive.Protocol.Packet

Namespace USBDrive
    ''' <summary>
    ''' USB设备的通讯数据包，适合设备 usb读写器、巡更棒
    ''' </summary>
    Public Class USBDrivePacket
        Inherits BasePacket

        Private Shared CodeMin As UInt32 = 4096 '0x1000
        Private Shared CodeMax As UInt32 = 61440 '0xF000

        ''' <summary>
        ''' 广播代码
        ''' </summary>
        Public Const BroadcastCode As UInt16 = 0

        ''' <summary>
        ''' 信息代码
        ''' </summary>
        Public Code As UInt32
        ''' <summary>
        ''' 机器号
        ''' </summary>
        Public Addr As Byte
        ''' <summary>
        ''' 命令类型
        ''' </summary>
        Public CmdType As Byte
        ''' <summary>
        ''' 命令索引
        ''' </summary>
        Public CmdIndex As Byte



#Region "类的初始化"
        Public Sub New()
            Code = GetRandomCode(CodeMin, CodeMax)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">USB设备的身份详情，要求包含SN和通讯密码</param>
        Public Sub New(detail As Core.Command.INCommandDetail)
            Me.New(detail, 0, 0, 0)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">USB设备的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Public Sub New(detail As USBDriveCommandDetail, cc As UInt16,
                       ct As Byte, ci As Byte)
            Me.New(detail, cc, ct, ci, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">USB设备的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(detail As USBDriveCommandDetail, cc As UInt16,
                       ct As Byte, ci As Byte,
                       dl As Byte, cd As IByteBuffer)
            Me.New(detail.Addr.ToInt32(), cc, ct, ci, dl, cd)
        End Sub




        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">USB设备的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Public Sub New(detail As USBDriveCommandDetail,
                       ct As Byte, ci As Byte)
            Me.New(detail, ct, ci, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">USB设备的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(detail As USBDriveCommandDetail,
                       ct As Byte, ci As Byte,
                       dl As Byte, cd As IByteBuffer)
            Me.New(detail.Addr.ToInt32(), ct, ci, dl, cd)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="addr">机器号</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Public Sub New(addr As Byte,
                       ct As Byte, ci As Byte)
            Me.New(addr, GetRandomCode(CodeMin, CodeMax), ct, ci, 0, Nothing)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="addr">机器号</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(addr As Byte,
                       ct As Byte, ci As Byte,
                       dl As Byte, cd As IByteBuffer)
            Me.New(addr, GetRandomCode(CodeMin, CodeMax), ct, ci, dl, cd)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="addr">机器号</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Public Sub New(addr As Byte, cc As UInt16,
                       ct As Byte, ci As Byte)
            Me.New(addr, cc, ct, ci, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="addr">机器号</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(addr As Byte, cc As UInt16,
                       ct As Byte, ci As Byte,
                       dl As Byte, cd As IByteBuffer)


            SetPacketCmdData(dl, cd)

            Me.Addr = addr
            Code = cc
            CmdType = ct
            CmdIndex = ci
            DataLen = dl
            CmdData = cd
            Check = 0
        End Sub



#End Region

#Region "改变数据包结构"

        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub SetPacket(ct As Byte, ci As Byte,
                       dl As Byte, cd As IByteBuffer)
            CmdType = ct
            CmdIndex = ci
            SetPacketCmdData(dl, cd)
        End Sub

        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Public Sub SetPacket(ct As Byte, ci As Byte)
            SetPacket(ct, ci, 0, Nothing)
        End Sub



#End Region


        ''' <summary>
        ''' 获取数据包的打包后的ByteBuf，用于发送数据
        ''' </summary>
        ''' <param name="Allocator">用于分配ByteBuf的分配器</param>
        ''' <returns></returns>
        Public Overrides Function GetPacketData(Allocator As IByteBufferAllocator) As IByteBuffer
            '机器码  信息代码    分类   命令   长度       数据     检验值
            '1Byte    2byte       1Byte  1Byte  1Byte    可变长度   1Byte
            Dim buf = Allocator.Buffer(6 + DataLen)


            Check = 0

            buf.WriteByte(Addr) '机器号
            buf.WriteUnsignedShort(Code) '信息代码
            buf.WriteByte(CmdType) '分类
            buf.WriteByte(CmdIndex) '命令
            buf.WriteByte(DataLen) '长度
            If DataLen > 0 Then
                buf.WriteBytes(CmdData, CmdData.ReaderIndex, DataLen) '数据
            End If

            Dim buf2 = CreatePacket(Allocator, buf)
            buf.Release()

            Return buf2
        End Function


        ''' <summary>
        ''' 释放使用的资源 --未使用
        ''' </summary>
        Protected Overrides Sub Release()
            Return
        End Sub
    End Class
End Namespace


