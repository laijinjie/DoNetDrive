Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Connector.Client

Namespace Connector.TCPServer.Client
    Friend Class TCPClientConnector
        Inherits AbstractNettyServerClientConnector(Of IByteBuffer)


        ''' <summary>
        ''' 创建一个客户端
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="channel"></param>
        Public Sub New(sKey As String, channel As IChannel)
            MyBase.New(sKey, channel)
        End Sub

        ''' <summary>
        ''' 释放资源时调用
        ''' </summary>
        Protected Overrides Sub Release1()
            MyBase.Release1()
        End Sub

        ''' <summary>
        ''' 创建一个连接头像详情对象，包含用于描述当前连接通道的信息
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetConnectorDetail0() As INConnectorDetail
            Return New TCPServerClientDetail(mKey, _ClientChannel.RemoteAddress， _ClientChannel.LocalAddress)
        End Function


        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPServerClient
        End Function


    End Class

End Namespace
