Imports System.Net

Namespace Connector
    ''' <summary>
    ''' 保存IP信息 -- IPv4
    ''' </summary>
    Public Structure IPDetail
        ''' <summary>
        ''' IP地址或网址
        ''' </summary>
        Public Addr As String
        ''' <summary>
        ''' 网络端口 1-65535
        ''' </summary>
        Public Port As Integer

        ''' <summary>
        ''' 将IP参数保存起来
        ''' </summary>
        ''' <param name="localAddress"></param>
        Public Sub New(localAddress As EndPoint)
            Dim ip As IPEndPoint = TryCast(localAddress, IPEndPoint)

            If ip IsNot Nothing Then
                Dim p As IPAddress = ip.Address
                If p.IsIPv4MappedToIPv6 Then
                    p = p.MapToIPv4
                End If
                Addr = p.ToString()
                Port = ip.Port
            Else
                Dim dns = TryCast(localAddress, DnsEndPoint)
                If dns IsNot Nothing Then
                    Addr = dns.Host
                    Port = dns.Port
                End If
            End If
        End Sub

        ''' <summary>
        ''' 将IP参数保存起来
        ''' </summary>
        ''' <param name="ip"></param>
        ''' <param name="iPort"></param>
        Sub New(ip As String, iPort As Integer)
            Addr = ip
            Port = iPort
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Addr}:{Port}"
        End Function
    End Structure

End Namespace
