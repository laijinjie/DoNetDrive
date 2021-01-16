Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Handlers.Tls
Imports DotNetty.Transport.Channels

Namespace Connector.TCPClient
    Public Class TCPClientChannelInitializer
        Inherits ChannelInitializer(Of IChannel)


        Public Sub New()
            MyBase.New
            mLastCertificate = New SSLCertificateDetail(False, Nothing, Nothing)
            _CertificateList = New Dictionary(Of String, SSLCertificateDetail)
        End Sub

        ''' <summary>
        ''' 最近使用的SSL证书
        ''' </summary>
        Private mLastCertificate As SSLCertificateDetail

        ''' <summary>
        ''' 证书列表
        ''' </summary>
        Private _CertificateList As Dictionary(Of String, SSLCertificateDetail)



        ''' <summary>
        ''' 设置SSL参数
        ''' </summary>
        ''' <param name="bSSL"></param>
        ''' <param name="oX509"></param>
        ''' <param name="oSSLFac"></param>
        Public Sub SetSSLPar(oRemotePoint As IPEndPoint, bSSL As Boolean,
                             oX509 As X509Certificate2,
                             oSSLFac As Func(Of Stream, SslStream))
            Dim sKey = oRemotePoint.ToString()

            If Not _CertificateList.ContainsKey(sKey) Then
                SyncLock Me
                    If Not _CertificateList.ContainsKey(sKey) Then
                        _CertificateList.Add(sKey, New SSLCertificateDetail(bSSL, oX509, oSSLFac))
                    End If
                End SyncLock
            Else
                Dim oCer = _CertificateList(sKey)
                oCer.SetCertificate(bSSL, oX509, oSSLFac)
            End If



            mLastCertificate.IsSSL = bSSL
            mLastCertificate.Certificate = oX509
            mLastCertificate.SSLStreamFactory = oSSLFac
        End Sub

        Protected Overrides Sub InitChannel(channel As IChannel)
            Dim oCer As SSLCertificateDetail = mLastCertificate
            Dim oRemote As Net.IPEndPoint = channel.RemoteAddress
            Dim sKey = oRemote.ToString()

            If _CertificateList.ContainsKey(sKey) Then
                oCer = _CertificateList(sKey)
                '_CertificateList.Remove(sKey)
            End If

            If oCer.IsSSL Then
                Dim targetHost As String
                Dim oCerLst = New List(Of X509Certificate)
                If oCer.Certificate Is Nothing Then
                    targetHost = String.Empty
                Else
                    targetHost = oCer.Certificate.GetNameInfo(X509NameType.DnsName, False)
                    oCerLst.Add(oCer.Certificate)
                End If



                Dim sslsetting = New ClientTlsSettings(System.Security.Authentication.SslProtocols.Tls12,
                                                       False, oCerLst, targetHost)

                Dim tls As TlsHandler
                If oCer.SSLStreamFactory Is Nothing Then

                    tls = New TlsHandler(AddressOf CreateSSLStream, sslsetting)

                Else
                    tls = New TlsHandler(oCer.SSLStreamFactory, sslsetting)
                End If
                channel.Pipeline().AddLast(tls)
            End If
            AddPipeline(channel)
        End Sub

        Protected Overridable Sub AddPipeline(channel As IChannel)
            channel.Pipeline().AddLast(New IdleStateHandler(120, 120, 0)) '超时检查
        End Sub

        ''' <summary>
        ''' 创建一个SSL流
        ''' </summary>
        ''' <param name="oIOStream"></param>
        ''' <returns></returns>
        Protected Function CreateSSLStream(oIOStream As Stream) As SslStream
            Return New SslStream(oIOStream, True, Function(sender, certificate, chain, errors) True)
        End Function

    End Class

End Namespace
