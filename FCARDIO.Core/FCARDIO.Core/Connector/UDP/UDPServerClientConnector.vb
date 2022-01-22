Imports System.Net
Imports System.Net.Sockets
Imports DoNetDrive.Core.Connector.TCPClient
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets

Namespace Connector.UDP
    ''' <summary>
    ''' 附属于UDPServer Connector 下的UDP子节点，每个子节点对应了一个远程主机
    ''' </summary>
    Public Class UDPServerClientConnector
        Inherits AbstractConnector

        ''' <summary>
        ''' 记录远程主机的IP
        ''' </summary>
        Public ReadOnly RemoteIP As EndPoint
        Protected Property _RemoteAddress As IPDetail
        Protected Property _LocalAddress As IPDetail


        Private UDPServer As IUDPServerConnector

        ''' <summary>
        ''' 通过远程主机和本地绑定的Netty通道初始化此类
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <param name="server"></param>
        Public Sub New(detail As UDPClientDetail, server As IUDPServerConnector, connecterManage As IConnecterManage)
            MyBase._ConnectorDetail = detail
            _RemoteAddress = New IPDetail(detail.Addr, detail.Port)
            _LocalAddress = New IPDetail(detail.LocalAddr, detail.LocalPort)
            RemoteIP = New IPEndPoint(IPAddress.Parse(detail.Addr), detail.Port)

            UDPServer = server
            AddHandler UDPServer.ServerCloseEvent, AddressOf UDPServer_ServerCloseEvent
            _IsActivity = True

            connecterManage.AddConnector(detail.GetKey, Me)
            FireClientOnline(Me)
            _ConnectDate = DateTime.Now
        End Sub

        Private Sub UDPServer_ServerCloseEvent(sender As IUDPServerConnector)
            If _isRelease Then Return
            CloseAsync()
        End Sub

        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return UDPClientConnectorStatus_Connected.Connected
        End Function

        Public Overrides Function RemoteAddress() As IPDetail
            Return _RemoteAddress
        End Function

        Public Overrides Function LocalAddress() As IPDetail
            Return _LocalAddress
        End Function

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.UDPClient
        End Function




#Region "接收数据"

        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="msg"></param>
        Protected Friend Sub ReadByteBufferNext(msg As IByteBuffer)
            MyBase.ReadByteBuffer(msg)
        End Sub
#End Region

#Region "发送数据"

        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Protected Overrides Async Function WriteByteBuf0(buf As IByteBuffer) As Task
            If CheckIsInvalid() Then
                Await Task.FromException(New Exception("connect is invalid"))

            End If

            If UDPServer Is Nothing Then
                Await Task.FromException(New Exception($"connect is {_Status.Status}"))
                Return
            End If

            Await UDPServer.WriteByteBufByUDP(buf, RemoteIP).ConfigureAwait(False)
            buf.Release()
        End Function
#End Region



#Region "连接服务器"
        Public Overrides Async Function ConnectAsync() As Task
            Await Task.FromException(New Exception("UDP Client  nonsupport ConnectAsync"))
        End Function


        ''' <summary>
        ''' 连接状态检查，当连接成功时，检查连接状态
        ''' </summary>
        Protected Friend Overridable Sub CheckConnectedStatus()
            If Not CheckIsInvalid() Then
                If _CommandList.Count = 0 Then
                    If _IsForcibly Then
                        CheckKeepaliveTime()
                    End If
                Else
                    CheckCommandList()
                End If
            End If
        End Sub
#End Region

#Region "关闭连接"
        Public Overrides Async Function CloseAsync() As Task
            If Me._isRelease Then Return
            If Not _IsActivity Then Return

            Me._Status = ConnectorStatus.Invalid

            Me._IsActivity = False

            UpdateActivityTime()
            If UDPServer IsNot Nothing Then
                RemoveHandler UDPServer.ServerCloseEvent, AddressOf UDPServer_ServerCloseEvent
                UDPServer = Nothing
            End If

            Await Task.Run(Sub()
                               FireConnectorClosedEvent(Me._ConnectorDetail)
                               Me.SetInvalid() '被关闭了就表示无效了
                           End Sub).ConfigureAwait(False)
            Me._IsForcibly = False
        End Function
#End Region



        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            '_Client = Nothing
            _RemoteAddress = Nothing
            _LocalAddress = Nothing
            If UDPServer IsNot Nothing Then
                RemoveHandler UDPServer.ServerCloseEvent, AddressOf UDPServer_ServerCloseEvent

            End If
            UDPServer = Nothing
        End Sub

    End Class
End Namespace

