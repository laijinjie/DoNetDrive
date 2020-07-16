Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Namespace Connector.TCPServer
    ''' <summary>
    ''' 表示一个TCP服务器的连接通道信息
    ''' </summary>
    Public Class TCPServerDetail
        Inherits AbstractConnectorDetail
        ''' <summary>
        ''' 表示一个IP地址，IPV4，IPV6都可以
        ''' </summary>
        Public ReadOnly LocalAddr As String

        ''' <summary>
        ''' 表示本地监听的端口号
        ''' </summary>
        Public ReadOnly LocalPort As Integer

        ''' <summary>
        ''' 是否启用SSL安全连接
        ''' </summary>
        Public ReadOnly IsSSL As Boolean

        ''' <summary>
        ''' 用于SSL安全连接的数字证书
        ''' </summary>
        Public ReadOnly Certificate As X509Certificate2

        ''' <summary>
        ''' 用于创建SSL安全套接字的流工厂
        ''' </summary>
        Public ReadOnly SSLStreamFactory As Func(Of Stream, SslStream)

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
            Me.New(Addr, Port, False, Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="Addr">本地监听地址</param>
        ''' <param name="Port">本地监听端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        Public Sub New(Addr As String, Port As Integer, bSSL As Boolean, oX509 As X509Certificate2)
            Me.New(Addr, Port, bSSL, oX509, Nothing)
        End Sub


        ''' <summary>
        ''' 初始化详情
        ''' </summary>
        ''' <param name="Addr">本地监听地址</param>
        ''' <param name="Port">本地监听端口</param>
        ''' <param name="bSSL">是否开启SSL</param>
        ''' <param name="oX509">使用的证书</param>
        ''' <param name="oSSLFac">用于创建SSL安全套接字的流工厂</param>
        Public Sub New(Addr As String, Port As Integer, bSSL As Boolean, oX509 As X509Certificate2, oSSLFac As Func(Of Stream, SslStream))
            Dim oIP As IPAddress = Nothing
            If Not String.IsNullOrEmpty(Addr) Then
                If Not IPAddress.TryParse(Addr, oIP) Then
                    Throw New ArgumentException("Addr Is Error")
                End If
            End If

            LocalAddr = Addr
            LocalPort = Port

            If bSSL Then
                If oX509 Is Nothing Then bSSL = False
            End If

            IsSSL = bSSL
            Certificate = oX509

            SSLStreamFactory = oSSLFac

        End Sub


        ''' <summary>
        ''' 进行比较，看是否指向同一个服务端点
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(other As INConnectorDetail) As Boolean
            Dim svr As TCPServerDetail = TryCast(other, TCPServerDetail)
            If svr Is Nothing Then Return False
            If String.IsNullOrEmpty(LocalAddr) Then
                If Not String.IsNullOrEmpty(svr.LocalAddr) Then
                    Return False
                End If
            Else
                If Not LocalAddr.Equals(svr.LocalAddr) Then
                    Return False
                End If
            End If

            Return LocalPort.Equals(svr.LocalPort)
        End Function

        ''' <summary>
        ''' 获取一个关于服务端点的Key值
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetKey() As String
            Return $"TCPServer_{LocalAddr}:{LocalPort}"
        End Function

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.TCPServer
        End Function

        ''' <summary>
        ''' 获取连接通道所在的程序集
        ''' </summary>
        ''' <returns>程序集名称</returns>
        Public Overrides Function GetAssemblyName() As String
            Return "DoNetDrive.Core"
        End Function

        ''' <summary>
        ''' 打印此详情所指示的连接信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $" {GetTypeName()} Local: {LocalAddr}:{LocalPort}"
        End Function
    End Class
End Namespace

