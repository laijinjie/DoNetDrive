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
            MyBase.New(sRemoteAddr, iRemotePort, slocal, ilocalPort)
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


