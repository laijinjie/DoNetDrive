
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector.TCPServer
Imports System.Net

Namespace Connector.WebSocket.Server
    ''' <summary>
    ''' WebSocket Server 监听通道
    ''' </summary>
    Public Class WebSocketServerConnector
        Inherits TCPServerConnector

        ''' <summary>
        ''' 初始化服务器监听通道
        ''' </summary>
        ''' <param name="chl">通道的绑定任务</param>
        ''' <param name="detail">通道的详情描述类</param>
        Public Sub New(chl As Task(Of IChannel), detail As WebSocketServerDetail)
            MyBase.New(chl, detail)
        End Sub

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.WebSocketServer
        End Function
    End Class
End Namespace

