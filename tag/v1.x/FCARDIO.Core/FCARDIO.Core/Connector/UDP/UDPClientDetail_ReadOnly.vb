Namespace Connector.UDP

    ''' <summary>
    ''' 只读的UDP连接器客户端详情
    ''' </summary>
    Public Class UDPClientDetail_ReadOnly
        Inherits TCPClient.TCPClientDetail_Readonly
        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        Sub New(dtl As TCPClient.TCPClientDetail)
            MyBase.New(dtl)
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

