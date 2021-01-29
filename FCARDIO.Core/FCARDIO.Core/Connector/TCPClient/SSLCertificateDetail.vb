Imports System.IO
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates

Namespace Connector.TCPClient
    ''' <summary>
    ''' 表示一个SSL证书
    ''' </summary>
    Public Class SSLCertificateDetail
        ''' <summary>
        ''' 是否启用SSL安全连接
        ''' </summary>
        Public IsSSL As Boolean

        ''' <summary>
        ''' 用于SSL安全连接的数字证书
        ''' </summary>
        Public Certificate As X509Certificate2

        ''' <summary>
        ''' 使用SSL时的SSL协议版本
        ''' </summary>
        Public UseSSLProtocols As SslProtocols = SslProtocols.Tls12

        ''' <summary>
        ''' 用于创建SSL安全套接字的流工厂
        ''' </summary>
        Public SSLStreamFactory As Func(Of Stream, SslStream)
        ''' <summary>
        ''' 创建一个证书详情
        ''' </summary>
        ''' <param name="bSSL"></param>
        ''' <param name="oX509"></param>
        ''' <param name="oSSLFac"></param>
        Public Sub New(bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream))
            IsSSL = bSSL
            Certificate = oX509
            SSLStreamFactory = oSSLFac
        End Sub

        Friend Sub SetCertificate(bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream))
            IsSSL = bSSL
            Certificate = oX509
            SSLStreamFactory = oSSLFac
        End Sub
    End Class
End Namespace