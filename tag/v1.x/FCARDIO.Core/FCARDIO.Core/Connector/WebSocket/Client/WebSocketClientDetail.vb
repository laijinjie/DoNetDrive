Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO

Namespace Connector.WebSocket.Client
    Public Class WebSocketClientDetail
        Inherits Connector.TCPClient.TCPClientDetail

        ''' <summary>
        ''' 路径
        ''' </summary>
        Public Path As String



        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sHost">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        Sub New(sHost As String, iPort As Integer)
            MyBase.New(sHost, iPort, String.Empty, 0)
            Path = String.Empty
        End Sub

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="oWebSocketClient">远程服务器的详情</param>
        Sub New(oWebSocketClient As WebSocketClientDetail)
            Me.New(oWebSocketClient.RemoteHost, oWebSocketClient.Port,
                   oWebSocketClient.LocalAddr, oWebSocketClient.LocalPort,
                   oWebSocketClient.IsSSL, oWebSocketClient.Certificate, oWebSocketClient.SSLStreamFactory,
                   oWebSocketClient.Path)
        End Sub


        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sHost">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(sHost As String, iPort As Integer, slocal As String, ilocalPort As Integer)
            MyBase.New(sHost, iPort, slocal, ilocalPort)
            Path = String.Empty
        End Sub

        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="sAddr">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        Public Sub New(sAddr As String, iPort As Integer, slocal As String, ilocalPort As Integer,
                       bSSL As Boolean, oX509 As X509Certificate2)
            Me.New(sAddr, iPort, slocal, ilocalPort, bSSL, oX509, Nothing, String.Empty)
        End Sub


        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="sAddr">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        ''' <param name="oSSLFac">用于创建SSL安全套接字的流工厂</param>
        Public Sub New(sAddr As String, iPort As Integer, slocal As String, ilocalPort As Integer,
                       bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream),
                       sWebsocketPath As String)
            MyBase.New(sAddr, iPort, slocal, ilocalPort, bSSL, oX509, oSSLFac)
            Path = sWebsocketPath
        End Sub

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.WebSocketClient
        End Function
    End Class

End Namespace
