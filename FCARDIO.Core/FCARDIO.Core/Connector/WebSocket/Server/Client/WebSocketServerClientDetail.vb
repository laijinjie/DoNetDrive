Imports System.Net

Namespace Connector.WebSocket.Server.Client
    ''' <summary>
    ''' 指示一个WebSocket Server的客户端节点
    ''' </summary>
    Public Class WebSocketServerClientDetail
        Inherits TCPServer.Client.TCPServerClientDetail

        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey"></param>
        Public Sub New(skey As String)
            MyBase.New(skey, Nothing, Nothing, 0)
        End Sub

        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey">指示此节点的唯一Key值</param>
        ''' <param name="_remote">远程客户端身份</param>
        Public Sub New(skey As String, _remote As IPEndPoint, _local As IPEndPoint, _ClientID As Long)
            MyBase.New(skey, _remote, _local, _ClientID)
        End Sub

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.WebSocketServerClient
        End Function
    End Class
End Namespace

