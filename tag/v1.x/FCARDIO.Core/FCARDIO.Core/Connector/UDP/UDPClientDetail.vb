Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO

Namespace Connector.UDP
    Public Class UDPClientDetail
        Inherits Connector.TCPClient.TCPClientDetail

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sAddr">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        Sub New(sAddr As String, iPort As Integer)
            Me.New(sAddr, iPort, String.Empty, 0)

        End Sub

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sAddr">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(sAddr As String, iPort As Integer, slocal As String, ilocalPort As Integer)
            MyBase.New(sAddr, iPort, slocal, ilocalPort)
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
            Me.New(sAddr, iPort, slocal, ilocalPort, bSSL, oX509, Nothing)
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
                       bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream))
            MyBase.New(sAddr, iPort, slocal, ilocalPort, bSSL, oX509, oSSLFac)
        End Sub

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.UDPClient
        End Function

    End Class
End Namespace


