Namespace Connector.UDP

    ''' <summary>
    ''' 只读的UDP连接器客户端详情
    ''' </summary>
    Public Class UDPClientDetail_ReadOnly
        Inherits TCPClient.TCPClientDetail_Readonly
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
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.UDPClient
        End Function
    End Class
End Namespace

