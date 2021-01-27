

Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Packet
Imports DoNetDrive.Common.Extensions
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.Transaction

Namespace USBDrive
    ''' <summary>
    ''' 用于处理关于 USBDrive 设备协议中产生的设备主动上报消息的处理类
    ''' </summary>
    Public Class USBDriveRequestHandle
        Inherits AbstractRequestHandle

        ''' <summary>
        ''' 事务工厂，用于处理获取各种类型的事物的类型
        ''' 第一个参数是机器号，第二个参数是命令索引，需要返回一个成产好的消息实体
        ''' </summary>
        Public TransactionFactory As Func(Of Byte, Byte, AbstractTransaction)


        ''' <summary>
        ''' 创建一个用于解析 USB 设备消息的处理器
        ''' </summary>
        ''' <param name="allocator">用于保存临时数据的ByteBuf分配器</param>
        ''' <param name="factory">用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂</param>
        Public Sub New(allocator As IByteBufferAllocator, factory As Func(Of Byte, Byte, AbstractTransaction))
            MyBase.New(New USBDriveDecompile(allocator))
            TransactionFactory = factory
        End Sub

        ''' <summary>
        ''' 处理响应
        ''' </summary>
        Public Overrides Sub DisposeResponse(connector As INConnector, msg As IByteBuffer)
            Return
        End Sub


        ''' <summary>
        ''' 检查命令是否为事务
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function CheckPacketIsTransaction(ByVal Pck As USBDrivePacket) As Boolean
            Return Pck.CmdType = &H19
        End Function


        ''' <summary>
        ''' 当处理类接收并验证完毕数据包后，由此产生消息
        ''' </summary>
        ''' <param name="connector">消息所在连接通道</param>
        ''' <param name="p">接收到的数据包</param>
        Protected Overrides Sub fireRequestEvent(connector As INConnector, p As INPacket)
            Dim dp As USBDrivePacket = TryCast(p, USBDrivePacket)

            If CheckPacketIsTransaction(dp) Then
                Dim iAddr As Byte, ci As Byte
                iAddr = dp.Addr
                ci = dp.CmdIndex

                Try
                    Dim trcEvent As AbstractTransaction = TransactionFactory(dp.Addr, ci)
                    If (trcEvent IsNot Nothing) Then

                        trcEvent.SetBytes(dp.CmdData)
                        Dim cdtl = connector.GetConnectorDetail()
                        Dim trc = New USBDriveTransaction(cdtl, iAddr, ci, trcEvent)

                        connector.FireTransactionMessage(trc)
                    End If

                Catch ex As Exception

                End Try

            End If
        End Sub
    End Class
End Namespace