
Imports System.Security.Cryptography.X509Certificates
Imports DoNetDrive.Core.Connector.TCPServer

Namespace Connector.WebSocket.Server

    ''' <summary>
    ''' 表示者一个WebSocketServer的连接通道参数
    ''' </summary>
    Public Class WebSocketServerDetail
        Inherits TCPServerDetail
        ''' <summary>
        ''' WebSocket 服务器监听的路径
        ''' </summary>
        Public ReadOnly WebsocketPath As String

        ''' <summary>
        ''' 初始化详情，本地地址指向 "0.0.0.0"
        ''' </summary>
        ''' <param name="Port">本地监听端口</param>
        Public Sub New(Port As Integer)
            Me.New(String.Empty, Port)
        End Sub

        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="Addr">本地监听地址</param>
        ''' <param name="Port">本地监听端口</param>
        Public Sub New(Addr As String, Port As Integer)
            Me.New(Addr, Port, False, Nothing, "/WebSocket")
        End Sub

        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="Addr">本地监听地址</param>
        ''' <param name="Port">本地监听端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        Public Sub New(Addr As String, Port As Integer, bSSL As Boolean, oX509 As X509Certificate2, path As String)
            MyBase.New(Addr, Port, bSSL, oX509)
            WebsocketPath = path
        End Sub

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.WebSocketServer
        End Function

    End Class
End Namespace