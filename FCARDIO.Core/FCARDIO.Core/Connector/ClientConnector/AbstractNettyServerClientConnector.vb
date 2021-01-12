Imports DotNetty.Transport.Channels

Namespace Connector.Client
    Public MustInherit Class AbstractNettyServerClientConnector(Of T)
        Inherits AbstractNettyClientConnector(Of T)

        ''' <summary>
        ''' 表示一个代表在TCP服务器节点下的唯一键值，通过此键值查询通道
        ''' </summary>
        Protected mKey As String
        ''' <summary>
        ''' 表示此通道的事件参数，当发生通道事件时传输给事件订阅者
        ''' </summary>
        Public EventArg As ServerEventArgs

        Public ReadOnly ClientID As Long

        ''' <summary>
        ''' 创建一个客户端
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="channel"></param>
        Public Sub New(sKey As String, channel As IChannel, ByVal _ClientID As Long)
            _ClientChannel = channel
            _Handler = New TCPClientNettyChannelHandler(Of T)(Me)
            _ClientChannel.Pipeline.AddLast(_Handler)
            _ClientChannel.Configuration.SetOption(ChannelOption.SoKeepalive, True)
            ClientID = _ClientID
            mKey = sKey
            '_IsForcibly = True
            ConnectSuccess()
        End Sub



        ''' <summary>
        ''' 返回此通道的初始化状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return GetStatus_Fail()
        End Function

        ''' <summary>
        ''' 获取一个状态表示连接通道连接失败
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetStatus_Fail() As INConnectorStatus
            Return ConnectorStatus_Invalid.Invalid
        End Function

        ''' <summary>
        ''' 获取一个状态表示连接通道连接已建立并工作正常的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetStatus_Connected() As INConnectorStatus
            Return TCPClient.TCPClientConnectorStatus.Connected
        End Function


        ''' <summary>
        ''' 当连接通道连接已失效时调用
        ''' </summary>
        Protected Overrides Sub ConnectFail0()
            'If _IsForcibly = False Then
            FireClientOffline(EventArg)
            Trace.WriteLine($"TCP已离线：{GetKey()} {Date.Now:HH:mm:ss.ffff} ")
            'End If

            SetInvalid()

            Dispose() '超过最大连接次数还是连接不上，直接释放此通道所有资源
        End Sub




        ''' <summary>
        ''' 连接通道建立连接成功后的后续处理
        ''' </summary>
        Protected Overrides Sub ConnectSuccess0()
            LocalDetail = New IPDetail(_ClientChannel.LocalAddress)
            RemoteDetail = New IPDetail(_ClientChannel.RemoteAddress)
            EventArg = New ServerEventArgs(mKey, _ClientChannel.LocalAddress, _ClientChannel.RemoteAddress)
        End Sub

        ''' <summary>
        ''' 释放资源时调用
        ''' </summary>
        Protected Overrides Sub Release1()
            'Throw New NotImplementedException()
        End Sub
    End Class
End Namespace

