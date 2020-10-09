Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports DoNetDrive.Core.Connector.Client

Namespace Connector.UDP
    ''' <summary>
    ''' 附属于UDPServer Connector 下的UDP子节点，每个子节点对应了一个远程主机
    ''' </summary>
    Public Class UDPServerClientConnector
        Inherits AbstractNettyClientConnector(Of IByteBuffer)

        ''' <summary>
        ''' 记录远程主机的IP
        ''' </summary>
        Public ReadOnly RemoteIP As EndPoint

        ''' <summary>
        ''' 此通道所在的EventLoop
        ''' </summary>
        Protected _EventLoop As IEventLoop

        ''' <summary>
        ''' 通道销毁事件
        ''' </summary>
        Event ConnectorDisposeEvent(ByVal client As UDPServerClientConnector)

        ''' <summary>
        ''' 通过远程主机和本地绑定的Netty通道初始化此类
        ''' </summary>
        ''' <param name="Remote"></param>
        ''' <param name="server"></param>
        Public Sub New(Remote As EndPoint, server As IChannel, serverDTL As UDPServerDetail)
            RemoteDetail = New IPDetail(Remote)
            LocalDetail = New IPDetail(server.LocalAddress)
            ThisConnectorDetail = New UDPClientDetail_ReadOnly(RemoteDetail.Addr, RemoteDetail.Port, serverDTL.LocalAddr, serverDTL.LocalPort)
            RemoteIP = Remote

            _ClientChannel = server

            _EventLoop = _ClientChannel.EventLoop

            ConnectSuccess()
        End Sub

        ''' <summary>
        ''' 远程连接成功
        ''' </summary>
        Protected Overrides Sub ConnectSuccess()
            _IsActivity = True
            _Status = GetStatus_Connected()

            _ConnectFailCount = 0
            ConnectSuccess0()

            '将本身加入到通道所在的线程循环中
            _EventLoop.Execute(Me)
        End Sub

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.UDPClient
        End Function


        ''' <summary>
        ''' 返回此通道的初始化状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return GetStatus_Fail()
        End Function

        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetEventLoop() As IEventLoop
            Return _EventLoop
        End Function

#Region "接收数据"

        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="ctx"></param>
        ''' <param name="msg"></param>
        Friend Overrides Sub channelRead0(ctx As IChannelHandlerContext, msg As IByteBuffer)
            If _isRelease Then
                '_ClientChannel?.EventLoop?.Execute(Sub()
                '                                       msg.Release()
                '                                   End Sub)
                Return
            End If

            'Trace.WriteLine($"通道：{RemoteIP} 接收：，data：0x{ByteBufferUtil.HexDump(msg)}")
            MyBase.channelRead0(ctx, msg)

            '_ClientChannel?.EventLoop?.Execute(Sub()
            '                                       msg.Release()
            '                                   End Sub)
        End Sub
#End Region

#Region "发送数据"

        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overrides Function WriteByteBuf(buf As IByteBuffer) As Task
            If _isRelease Then Return Nothing
            If _ClientChannel Is Nothing Then
                Return Nothing
            End If
            UpdateActivityTime()
            DisposeResponse(buf)
            'Trace.WriteLine($"通道：{RemoteIP} 发送：目标:{RemoteIP.ToString()}，data：0x{ByteBufferUtil.HexDump(buf)}")

            '_ClientChannel?.EventLoop()?.Execute(Sub()
            '                                         _ClientChannel?.WriteAndFlushAsync(New DatagramPacket(buf, RemoteIP)).Wait()
            '                                     End Sub)
            Return _ClientChannel?.WriteAndFlushAsync(New DatagramPacket(buf, RemoteIP))
        End Function
#End Region

        ''' <summary>
        ''' 创建一个连接头像详情对象，包含用于描述当前连接通道的信息
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetConnectorDetail0() As INConnectorDetail
            Return Nothing 'New UDPClientDetail_ReadOnly(RemoteDetail.Addr, RemoteDetail.Port, LocalDetail.Addr, LocalDetail.Port)
        End Function

        Protected Overrides Function GetStatus_Fail() As INConnectorStatus
            Return ConnectorStatus_Invalid.Invalid
        End Function

        Protected Overrides Function GetStatus_Connected() As INConnectorStatus
            Return TCPClient.TCPClientConnectorStatus.Connected
        End Function

        ''' <summary>
        ''' 当连接通道连接已失效时调用
        ''' </summary>
        Protected Overrides Sub ConnectFail0()
            Return
        End Sub

        ''' <summary>
        ''' 表示通道建立完毕时调用
        ''' </summary>
        Protected Overrides Sub ConnectSuccess0()
            Return
        End Sub


#Region "关闭连接"
        ''' <summary>
        ''' 关闭连接
        ''' </summary>
        Public Overrides Sub CloseConnector()

            If _ClientChannel IsNot Nothing Then

                FireClientOffline(Me)
                FireConnectorClosedEvent(GetConnectorDetail())
            End If
            _ClientChannel = Nothing
            _Status = GetStatus_Fail()

            _IsActivity = False
            RaiseEvent ConnectorDisposeEvent(Me)

        End Sub

        ''' <summary>
        ''' 连接关闭后的后续处理
        ''' </summary>
        Protected Overrides Sub CloseConnector0()
            Return
        End Sub
#End Region


        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release1()
            _EventLoop = Nothing
        End Sub
    End Class
End Namespace

