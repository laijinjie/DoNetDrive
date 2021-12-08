Imports DotNetty.Buffers
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Packet
Imports DoNetDrive.Protocol.Packet
Namespace FrameCommand

    Public MustInherit Class AbstractFrameCommand(Of T As {AbstractFramePacket, New})
        Inherits AbstractCommand

        ''' <summary>
        ''' 表示校验和错误的状态
        ''' </summary>
        Public Shared CheckSumErrorStatus = New CheckSumError()
        ''' <summary>
        ''' 表示通讯密码错误的状态
        ''' </summary>
        Public Shared PasswordErrorStatus = New PasswordErrorStatus()


        ''' <summary>
        ''' 处理当前命令逻辑的数据包和 _Packet 是同一个对象
        ''' </summary>
        Protected FPacket As T



        ''' <summary>
        ''' 创建一个帧数据包命令，并初始化为命令发送后需要等待回应
        ''' </summary>
        ''' <param name="cd"></param>
        ''' <param name="par"></param>
        Sub New(cd As INCommandDetail, par As INCommandParameter)
            MyBase.New(cd, par)
            FPacket = New T()
            FPacket.SetCommandDetail(cd)
            _Packet = FPacket
            _IsWaitResponse = True
        End Sub


        ''' <summary>
        ''' 指令开始执行时，用于让命令组装第一个用于发送的数据包 CommandNext0 中组装（如果有的话）
        ''' </summary>
        Protected Overrides Sub CreatePacket()
            _Decompile = CreateDecompile(_Connector.GetByteBufAllocator())
            CreatePacket0()
        End Sub

        ''' <summary>
        ''' 生成命令的第一个数据包，后续的数据包应该在接收到返回值在 CommandNext 函数中陆续组装
        ''' </summary>
        Protected MustOverride Function CreateDecompile(Allocator As IByteBufferAllocator) As Core.Packet.INPacketDecompile


        ''' <summary>
        ''' 生成命令的第一个数据包，后续的数据包应该在接收到返回值在 CommandNext 函数中陆续组装
        ''' </summary>
        Protected MustOverride Sub CreatePacket0()

        ''' <summary>
        ''' 命令准备就绪，等待下次发送
        ''' </summary>
        Protected Overrides Sub CommandReady()
            FPacket.Code += 1
            MyBase.CommandReady()
        End Sub




        ''' <summary>
        ''' 修改当前的数据包
        ''' </summary>
        ''' <param name="CmdType">命令分类</param>
        ''' <param name="CmdIndex">命令索引</param>
        ''' <param name="CmdPar">命令参数</param>
        ''' <param name="DataLen">数据长度</param>
        ''' <param name="CmdDataBuf">命令数据</param>
        Protected Overridable Sub Packet(CmdType As Byte, CmdIndex As Byte, CmdPar As Byte,
                       DataLen As UInt32, CmdDataBuf As IByteBuffer)
            FPacket.SetPacket(CmdType, CmdIndex, CmdPar, DataLen, CmdDataBuf)
        End Sub

        ''' <summary>
        ''' 修改当前的数据包
        ''' </summary>
        ''' <param name="CmdType">命令分类</param>
        ''' <param name="CmdIndex">命令索引</param>
        Protected Overridable Sub Packet(CmdType As Byte, CmdIndex As Byte)
            Packet(CmdType, CmdIndex, 0)
        End Sub

        ''' <summary>
        ''' 修改当前的数据包
        ''' </summary>
        ''' <param name="CmdType">命令分类</param>
        ''' <param name="CmdIndex">命令索引</param>
        ''' <param name="CmdPar">命令参数</param>
        Protected Overridable Sub Packet(CmdType As Byte, CmdIndex As Byte, CmdPar As Byte)
            Packet(CmdType, CmdIndex, CmdPar, 0, Nothing)
        End Sub

        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected Overrides Sub Release0()
            FPacket = Nothing
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
            Dim oPck As T = TryCast(readPacket, T)
            If oPck Is Nothing Then Return
            If FPacket Is Nothing Then Return
            If oPck.Code <> FPacket.Code Then
                'Trace.WriteLine($"{_Connector.GetKey()} 信息代码不一致，已抛弃，返回的代码：{oPck.Code}，期望的代码：{FPacket.Code}")
                Return '信息代码不一致，不是此命令的后续
            End If

            '检查命令返回值是否为密码错误
            If CheckResponse_PasswordErr(oPck) Then
                '发生错误
                SetStatus(PasswordErrorStatus)
                fireAuthenticationErrorEvent()
                Return
            End If

            '检查命令返回值是否为校验和错误
            If CheckResponse_CheckSumErr(oPck) Then
                If _ReSendCount <= 100 Then
                    SetStatus(GetStatus_Runing())
                    CommandReSend()
                    SetRuningStatus()
                Else
                    SetStatus(CheckSumErrorStatus)
                    fireFireCommandErrorEvent()
                End If
                Return
            End If

            '继续检查响应是否为命令的下一步骤
            Try
                CommandNext0(oPck)
            Catch ex As Exception
                Trace.WriteLine($"{_Connector.GetKey()}  {Me.GetType().Name}_CommandNext:" & ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' 命令收到响应，解析响应并继续执行命令
        ''' </summary>
        ''' <param name="oPck">待处理的数据包(接收到的数据包)</param>
        Protected Overridable Sub CommandNext0(oPck As T)
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
        Protected MustOverride Sub CommandNext1(oPck As T)



        ''' <summary>
        ''' 命令返回值成功
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected MustOverride Function CheckResponse_OK(oPck As T) As Boolean


        ''' <summary>
        ''' 通讯密码错误
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected MustOverride Function CheckResponse_PasswordErr(oPck As T) As Boolean


        ''' <summary>
        ''' 检验和错误
        ''' </summary>
        ''' <param name="oPck">需要检查的指令--一般值接收到的数据包</param>
        ''' <returns></returns>
        Protected MustOverride Function CheckResponse_CheckSumErr(oPck As T) As Boolean

    End Class


End Namespace