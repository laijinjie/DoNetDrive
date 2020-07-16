Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Extension
Imports DoNetDrive.Protocol.Packet
Imports DoNetDrive.Protocol.FrameCommand

Namespace OnlineAccess
    ''' <summary>
    ''' 适用于在线门禁、在线电梯、指纹机、人脸机等设备通讯数据包
    ''' </summary>
    Public Class OnlineAccessPacket
        Inherits AbstractFramePacket

        Private Shared CodeMin As UInt32 = 268435456 '0x10000000
        Private Shared CodeMax As UInt32 = 3758096384 '0xB0000000

        ''' <summary>
        ''' UDP广播时使用的网络标志
        ''' </summary>
        Private Shared UDPBroadcastCode As UInt32 = 3217009339 '0xBFBFAABB

        ''' <summary>
        ''' 广播代码
        ''' </summary>
        Public Const BroadcastCode As UInteger = 4294967295 '0xFFFFFFFF

        ''' <summary>
        ''' 密码长度
        ''' </summary>
        Public Const ConnectPassordLen As Integer = 4



        ''' <summary>
        ''' 命令类型
        ''' </summary>
        Public CmdType As Byte
        ''' <summary>
        ''' 命令索引
        ''' </summary>
        Public CmdIndex As Byte
        ''' <summary>
        ''' 命令参数
        ''' </summary>
        Public CmdPar As Byte


#Region "类的初始化"
        Public Sub New()
            Code = GetRandomCode(CodeMin, CodeMax)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">在线门禁的身份详情，要求包含SN和通讯密码</param>
        Public Sub New(detail As Core.Command.INCommandDetail)
            Me.New(detail, 0, 0, 0)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">在线门禁的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        Public Sub New(detail As OnlineAccessCommandDetail, cc As UInt32,
                       ct As Byte, ci As Byte, cp As Byte)
            Me.New(detail, cc, ct, ci, cp, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">在线门禁的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(detail As OnlineAccessCommandDetail, cc As UInt32,
                       ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)
            Me.New(detail.SNByte, detail.PasswordByte, cc, ct, ci, cp, dl, cd)
        End Sub




        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">在线门禁的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        Public Sub New(detail As OnlineAccessCommandDetail,
                       ct As Byte, ci As Byte, cp As Byte)
            Me.New(detail, ct, ci, cp, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="detail">在线门禁的身份详情，要求包含SN和通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(detail As OnlineAccessCommandDetail,
                       ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)
            Me.New(detail.SNByte, detail.PasswordByte, ct, ci, cp, dl, cd)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="s">设备SN</param>
        ''' <param name="ps">通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        Public Sub New(s() As Byte, ps() As Byte,
                       ct As Byte, ci As Byte, cp As Byte)
            Me.New(s, ps, GetRandomCode(CodeMin, CodeMax), ct, ci, cp, 0, Nothing)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="s">设备SN</param>
        ''' <param name="ps">通讯密码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(s() As Byte, ps() As Byte,
                       ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)
            Me.New(s, ps, GetRandomCode(CodeMin, CodeMax), ct, ci, cp, dl, cd)
        End Sub


        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="s">设备SN</param>
        ''' <param name="ps">通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        Public Sub New(s() As Byte, ps() As Byte, cc As UInt32,
                       ct As Byte, ci As Byte, cp As Byte)
            Me.New(s, ps, cc, ct, ci, cp, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化数据包类
        ''' </summary>
        ''' <param name="s">设备SN</param>
        ''' <param name="ps">通讯密码</param>
        ''' <param name="cc">信息代码</param>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub New(s() As Byte, ps() As Byte, cc As UInt32,
                       ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)
            SetPacketTarget(s, ps)
            Code = cc
            SetPacket(ct, ci, cp, dl, cd)
        End Sub

        ''' <summary>
        ''' 设置数据包的目标（SN和密码）
        ''' </summary>
        ''' <param name="SNbuf">SN</param>
        ''' <param name="ps">密码</param>
        Public Overridable Sub SetPacketTarget(SNbuf() As Byte, ps() As Byte)
            If SNbuf Is Nothing Then
                VerifyError("SN")
                Return
            End If
            If SNbuf.Length <> 16 Then
                VerifyError("SN")
                Return
            End If

            If ps Is Nothing Then
                VerifyError("Password")
                Return
            End If
            If ps.Length <> 4 Then
                VerifyError("Password")
                Return
            End If

            SN = SNbuf
            Password = ps
        End Sub

#End Region

#Region "改变数据包结构"
        ''' <summary>
        ''' 设置数据包绑定的命令详情
        ''' </summary>
        ''' <param name="dtl"></param>
        Public Overrides Sub SetCommandDetail(dtl As Core.Command.INCommandDetail)
            Dim detail As OnlineAccessCommandDetail = TryCast(dtl, OnlineAccessCommandDetail)
            If detail Is Nothing Then
                VerifyError("CommandDetail")
                Return
            End If
            Dim s = detail.SNByte, ps = detail.PasswordByte
            SetPacketTarget(s, ps)

        End Sub


        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="cp">命令参数</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Overrides Sub SetPacket(ct As Byte, ci As Byte, cp As Byte,
                       dl As UInt32, cd As IByteBuffer)
            CmdType = ct
            CmdIndex = ci
            CmdPar = cp
            SetPacketCmdData(dl, cd)
        End Sub

#End Region


#Region "网络标志的修改"


        ''' <summary>
        ''' 设置当前的数据包为UDP广播数据包,UDP广播发送和接收数据时使用
        ''' </summary>
        Public Overrides Sub SetUDPBroadcastPacket()
            Code = UDPBroadcastCode
        End Sub

        ''' <summary>
        ''' 设置当前的数据包为正常播数据包（默认状态就是正常，除非经过修改，否则不必调用此函数）
        ''' </summary>
        Public Overrides Sub SetNormalPacket()
            SetNormalPacket(GetRandomCode(CodeMin, CodeMax))
        End Sub
#End Region



        ''' <summary>
        ''' 获取数据包的打包后的ByteBuf，用于发送数据
        ''' </summary>
        ''' <param name="Allocator">用于分配ByteBuf的分配器</param>
        ''' <returns></returns>
        Public Overrides Function GetPacketData(Allocator As IByteBufferAllocator) As IByteBuffer
            '设备SN  密码    信息代码    分类   命令   参数    长度       数据     检验值
            '16Byte  4Byte   4byte       1Byte  1Byte  1Byte   4Byte    可变长度   1Byte
            Dim buf = Allocator.Buffer(30 + DataLen)


            Check = 0

            buf.WriteBytes(SN) '设备SN
            buf.WriteBytes(Password) '密码
            buf.WriteUnsignedInt(Code) '信息代码
            buf.WriteByte(CmdType) '分类
            buf.WriteByte(CmdIndex) '命令
            buf.WriteByte(CmdPar) '参数

            If DataLen > 0 Then
                Try
                    If CmdData.ReadableBytes >= DataLen Then
                        buf.WriteInt(DataLen) '长度
                        buf.WriteBytes(CmdData, CmdData.ReaderIndex, DataLen) '数据
                    Else
                        buf.WriteInt(0) '长度
                    End If
                Catch ex As Exception
                    buf.SetInt(27, 0)
                End Try

            Else
                buf.WriteInt(DataLen) '长度
            End If
            '创建数据包并进行转义和校验和计算
            Dim buf2 = CreatePacket(Allocator, buf)
            buf.Release()

            Return buf2
        End Function


        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected Overrides Sub Release()
            Password = Nothing
            SN = Nothing
        End Sub


    End Class
End Namespace

