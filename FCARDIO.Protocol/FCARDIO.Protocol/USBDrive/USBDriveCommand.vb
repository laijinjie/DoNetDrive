Imports DoNetDrive.Protocol.OnlineAccess
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers

Namespace USBDrive
    ''' <summary>
    ''' USB 设备命令格式的基类
    ''' 适用于：读写器，巡更棒
    ''' </summary>
    Public MustInherit Class USBDriveCommand
        Inherits AbstractCommand

        ''' <summary>
        ''' 处理当前命令逻辑的数据包和 _Packet 是统一个对象
        ''' </summary>
        Protected USBPacket As USBDrivePacket
        ''' <summary>
        ''' 表示校验和错误的状态
        ''' </summary>
        Public Shared CheckSumErrorStatus = New USBDriveCommandStatus_CheckSumError()

        ''' <summary>
        ''' 初始化两个重要参数，并进行参数检查
        ''' </summary>
        ''' <param name="cd">表示命令详情，包含通道信息，对端信息，超时时间，重发次数</param>
        ''' <param name="par">表示此命令逻辑所需要的命令参数</param>
        Sub New(cd As INCommandDetail, par As INCommandParameter)
            MyBase.New(cd, par)
            USBPacket = New USBDrivePacket(cd)
            _Packet = USBPacket
            _IsWaitResponse = True
        End Sub

        ''' <summary>
        ''' 指令开始执行时，用于让命令组装第一个用于发送的数据包 CommandNext0 中组装（如果有的话）
        ''' </summary>
        Protected Overrides Sub CreatePacket()
            _Decompile = New USBDriveDecompile(_Connector.GetByteBufAllocator())
            CreatePacket0()
        End Sub

        ''' <summary>
        ''' 生成命令的第一个数据包，后续的数据包应该在
        ''' </summary>
        Protected MustOverride Sub CreatePacket0()


        ''' <summary>
        ''' 修改当前的数据包
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Protected Overridable Sub Packet(ct As Byte, ci As Byte,
                       dl As UInt32, cd As IByteBuffer)
            If dl > 0 Then
                If cd Is Nothing Then
                    VerifyError("CmdData")
                    Return
                End If
                If dl > cd.ReadableBytes Then
                    VerifyError("CmdData")
                    Return
                End If
            End If

            With USBPacket
                .CmdType = ct
                .CmdIndex = ci
                .DataLen = dl
                If .CmdData IsNot Nothing Then
                    .CmdData.Release()
                    .CmdData = Nothing
                End If
                .CmdData = cd
            End With
        End Sub


        ''' <summary>
        ''' 修改当前的数据包
        ''' </summary>
        ''' <param name="ct">命令分类</param>
        ''' <param name="ci">命令索引</param>
        Protected Overridable Sub Packet(ct As Byte, ci As Byte)
            Packet(ct, ci, 0, Nothing)
        End Sub


        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected Overrides Sub Release0()

            USBPacket = Nothing
            Release1()
        End Sub

        ''' <summary>
        ''' 让派生类主动释放资源
        ''' </summary>
        Protected MustOverride Sub Release1()

        ''' <summary>
        ''' 检查并进行命令的下一部分
        ''' </summary>
        ''' <param name="readPacket">收到的数据包</param>
        Protected Overrides Sub CommandNext(readPacket As INPacket)
            Dim oPck As USBDrivePacket = TryCast(readPacket, USBDrivePacket)
            If oPck Is Nothing Then Return
            If oPck.Code <> USBPacket.Code Then Return '信息代码不一致，不是此命令的后续

            '检查命令返回值是否为校验和错误
            If CheckResponse_CheckSumErr(oPck) Then
                If _ReSendCount <= 100 Then
                    _Status = GetStatus_Runing()
                    CommandReSend()
                    SetRuningStatus()
                Else
                    _Status = CheckSumErrorStatus
                    fireFireCommandErrorEvent()
                End If
                Return
            End If

            '继续检查响应是否为命令的下一步骤
            CommandNext0(oPck)
        End Sub

        ''' <summary>
        ''' 命令收到响应，解析响应并继续执行命令
        ''' </summary>
        ''' <param name="oPck">待处理的数据包(接收到的数据包)</param>
        Protected Overridable Sub CommandNext0(oPck As USBDrivePacket)
            If CheckResponse_OK(oPck) Then
                CommandCompleted()
                Return
            Else
                CommandNext1(oPck)
            End If
        End Sub
        ''' <summary>
        ''' 继续检查响应是否为命令的下一步骤
        ''' </summary>
        ''' <param name="oPck">待处理的数据包(接收到的数据包)</param>
        Protected MustOverride Sub CommandNext1(oPck As USBDrivePacket)

        ''' <summary>
        ''' 命令返回值成功
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected Function CheckResponse_OK(oPck As USBDrivePacket) As Boolean
            Return (oPck.CmdType = &H21 And oPck.CmdIndex = 1)
        End Function


        ''' <summary>
        ''' 检验和错误
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected Function CheckResponse_CheckSumErr(oPck As USBDrivePacket) As Boolean
            Return (oPck.CmdType = &H21 And oPck.CmdIndex = 3)
        End Function


        ''' <summary>
        ''' 指定命令的返回值中的CmdType需要增加的量
        ''' </summary>
        Protected ResultCmdTypeAddValkue As Integer = &H30

        ''' <summary>
        ''' 检查返回指令是否正确
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oPck As USBDrivePacket) As Boolean

            Return CheckResponse(oPck, oPck.DataLen)
        End Function

        ''' <summary>
        ''' 检查返回指令是否正确
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <param name="dl">返回值的长度要求</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oPck As USBDrivePacket, dl As Byte) As Boolean

            Return CheckResponse(oPck, USBPacket.CmdType, USBPacket.CmdIndex, dl)
        End Function


        ''' <summary>
        ''' 检查返回指令是否符合规则
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <param name="CmdType">返回值的命令类型</param>
        ''' <param name="CmdIndex">返回值的命令索引</param>
        ''' <param name="DataLen">返回值的长度要求</param>
        ''' <returns></returns>
        Protected Overridable Function CheckResponse(oPck As USBDrivePacket,
                                                     CmdType As Byte, CmdIndex As Byte,
                                                     DataLen As Byte) As Boolean

            Return (oPck.CmdType = ResultCmdTypeAddValkue + CmdType And oPck.CmdIndex = CmdIndex And oPck.DataLen = DataLen)
        End Function

    End Class
End Namespace


