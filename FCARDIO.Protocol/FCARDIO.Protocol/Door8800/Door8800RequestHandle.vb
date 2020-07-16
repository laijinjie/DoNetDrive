Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Packet
Imports DoNetDrive.Core.Extension
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.OnlineAccess
Imports DoNetDrive.Protocol.Transaction

Namespace Door8800
    ''' <summary>
    ''' 用于处理关于Door8800系列协议中产生的设备主动上报消息的处理类
    ''' </summary>
    Public Class Door8800RequestHandle
        Inherits AbstractRequestHandle

        ''' <summary>
        ''' 事务工厂，用于处理获取各种类型的事物的类型
        ''' </summary>
        Public TransactionFactory As Func(Of String, Byte, Byte, AbstractTransaction)

        ''' <summary>
        ''' 用于处理关于Door8800系列协议中产生的设备主动上报消息的处理类
        ''' </summary>
        ''' <param name="allocator">用于保存临时数据的ByteBuf分配器</param>
        ''' <param name="factory">用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂</param>
        Public Sub New(allocator As IByteBufferAllocator, factory As Func(Of String, Byte, Byte, AbstractTransaction))
            MyBase.New(New OnlineAccessDecompile(allocator))
            TransactionFactory = factory
        End Sub

        ''' <summary>
        ''' 处理响应
        ''' </summary>
        Public Overrides Sub DisposeResponse(connector As INConnector, msg As IByteBuffer)
            Return
        End Sub

        ''' <summary>
        ''' 当处理类接收并验证完毕数据包后，由此产生消息
        ''' </summary>
        ''' <param name="connector">消息所在连接通道</param>
        ''' <param name="p">接收到的数据包</param>
        Protected Overrides Sub fireRequestEvent(connector As INConnector, p As INPacket)
            Dim dp As OnlineAccessPacket = TryCast(p, OnlineAccessPacket)
            If dp.Code = OnlineAccessPacket.BroadcastCode Then
                If dp.CmdType = &H19 Then
                    Dim sSN As String, ci As Byte, cp As Byte
                    sSN = dp.SN.GetString()
                    ci = dp.CmdIndex
                    cp = dp.CmdPar

                    Try
                        Dim trcEvent As AbstractTransaction = TransactionFactory(sSN, ci, cp)
                        If (trcEvent IsNot Nothing) Then

                            trcEvent.SetBytes(dp.CmdData)
                            Dim cdtl = connector.GetConnectorDetail()
                            Dim trc = New Door8800Transaction(cdtl, sSN, ci, cp, trcEvent)

                            connector.FireTransactionMessage(trc)
                        End If

                    Catch ex As Exception

                    End Try

                End If
            End If
        End Sub
    End Class
End Namespace