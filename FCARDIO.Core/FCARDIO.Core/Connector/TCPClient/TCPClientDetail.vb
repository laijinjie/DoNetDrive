Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates

Namespace Connector.TCPClient
    ''' <summary>
    ''' 表示一个 TCP Client 通道的详情
    ''' </summary>
    Public Class TCPClientDetail
        Inherits AbstractConnectorDetail

        Private _Remote As IPDetail

        Public ReadOnly Property Remote As IPDetail
            Get
                Return _Remote
            End Get
        End Property
        ''' <summary>
        ''' 通道的别名 自定义通道Key
        ''' </summary>
        Public Overridable Property ConnectAlias As String

        ''' <summary>
        ''' 远程服务器的IP
        ''' </summary>
        Public Overridable Property Addr As String

        ''' <summary>
        ''' 远程主机名
        ''' </summary>
        Public Overridable Property RemoteHost As String

        ''' <summary>
        ''' 远程服务器的监听端口
        ''' </summary>
        Public Overridable Property Port As Integer

        ''' <summary>
        ''' 连接远程服务器的本地IP
        ''' </summary>
        Public Overridable Property LocalAddr As String
        ''' <summary>
        ''' 连接远程服务器的本地端口
        ''' </summary>
        Public Overridable Property LocalPort As Integer


        ''' <summary>
        ''' 是否启用SSL安全连接
        ''' </summary>
        Public ReadOnly IsSSL As Boolean
        ''' <summary>
        ''' 使用SSL时的SSL协议版本
        ''' </summary>
        Public UseSSLProtocols As SslProtocols = SslProtocols.Tls12

        ''' <summary>
        ''' 用于SSL安全连接的数字证书
        ''' </summary>
        Public ReadOnly Certificate As X509Certificate2

        ''' <summary>
        ''' 用于创建SSL安全套接字的流工厂
        ''' </summary>
        Public ReadOnly SSLStreamFactory As Func(Of Stream, SslStream)



        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sRemoteAddr">远程服务器的IP或域名</param>
        ''' <param name="iRemotePort">远程服务器的监听端口</param>
        Sub New(sRemoteAddr As String, iRemotePort As Integer)
            Me.New(sRemoteAddr, iRemotePort, String.Empty, 0)
        End Sub

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sRemoteAddr">远程服务器的IP或域名</param>
        ''' <param name="iRemotePort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(sRemoteAddr As String, iRemotePort As Integer, slocal As String, ilocalPort As Integer)
            Me.New(sRemoteAddr, iRemotePort, slocal, ilocalPort, False, Nothing, Nothing)

        End Sub


        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="sRemoteAddr">远程服务器的IP或域名</param>
        ''' <param name="iRemotePort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        Public Sub New(sRemoteAddr As String, iRemotePort As Integer, slocal As String, ilocalPort As Integer,
                       bSSL As Boolean, oX509 As X509Certificate2)
            Me.New(sRemoteAddr, iRemotePort, slocal, ilocalPort, bSSL, oX509, Nothing)
        End Sub


        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="sRemoteAddr">远程服务器的IP或域名</param>
        ''' <param name="iRemotePort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        ''' <param name="oSSLFac">用于创建SSL安全套接字的流工厂</param>
        Public Sub New(sRemoteAddr As String, iRemotePort As Integer, slocal As String, ilocalPort As Integer,
                       bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream))
            Timeout = TCPClientFactory.CONNECT_TIMEOUT_Default '默认值是5秒
            RestartCount = TCPClientFactory.CONNECT_RECONNECT_Default '默认重试2次
            ConnectAlias = String.Empty
            Dim oIP As IPAddress = Nothing
            RemoteHost = sRemoteAddr
            If Not String.IsNullOrEmpty(sRemoteAddr) Then
                If IPAddress.TryParse(sRemoteAddr, oIP) Then
                    If oIP.AddressFamily <> Sockets.AddressFamily.InterNetwork Then
                        Throw New ArgumentException($"{sRemoteAddr} is not IPv4")
                    Else
                        Addr = sRemoteAddr
                    End If
                Else
                    Try
                        Dim oDNSIP As IPHostEntry = Dns.GetHostEntry(sRemoteAddr)
                        If oDNSIP.AddressList.Length > 0 Then
                            '获取服务器节点
                            Addr = oDNSIP.AddressList(0).ToString()
                        End If
                    Catch ex As Exception
                        Throw New ArgumentException($"{sRemoteAddr} host not find!")
                    End Try

                End If
            End If

            If Not String.IsNullOrEmpty(RemoteHost) And String.IsNullOrEmpty(Addr) Then
                Addr = RemoteHost
            End If

            Port = iRemotePort

            oIP = Nothing
            If Not String.IsNullOrEmpty(LocalAddr) Then
                If Not IPAddress.TryParse(LocalAddr, oIP) Then
                    Throw New ArgumentException("slocal is Error")
                End If
            End If
            LocalAddr = slocal

            LocalPort = ilocalPort

            _Remote = New IPDetail(Addr, Port)
            'If bSSL Then
            '    If oX509 Is Nothing Then bSSL = False
            'End If

            IsSSL = bSSL
            Certificate = oX509

            SSLStreamFactory = oSSLFac
        End Sub



        ''' <summary>
        ''' 获取连接通道所在的程序集
        ''' </summary>
        ''' <returns>程序集名称</returns>
        Public Overrides Function GetAssemblyName() As String
            Return "DoNetDrive.Core"
        End Function

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.TCPClient
        End Function

        ''' <summary>
        ''' 用来比较此连接通道是否为同一个
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(other As INConnectorDetail) As Boolean
            If other Is Nothing Then Return False
            Dim t As TCPClientDetail = TryCast(other, TCPClientDetail)
            If t IsNot Nothing Then
                If Not t.Addr.Equals(Addr) Then Return False
                If Not t.Port.Equals(Port) Then Return False
                If Not t.LocalAddr.Equals(LocalAddr) Then Return False
                If Not t.LocalPort.Equals(LocalPort) Then Return False
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' 获取一个用于界定此通道的唯一Key值
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetKey() As String
            If (String.IsNullOrEmpty(ConnectAlias)) Then
                Return $"{GetTypeName()}_Local:{LocalAddr}:{LocalPort}_Remote:{Addr}:{Port}"
            Else
                Return ConnectAlias
            End If

        End Function

        ''' <summary>
        ''' 打印此详情所指示的连接信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return GetKey()
        End Function
    End Class
End Namespace

