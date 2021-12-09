Imports System.Net

Namespace Connector.TCPServer.Client
    ''' <summary>
    ''' 表示一个TCP服务器节点下的客户端通道详情
    ''' </summary>
    Public Class TCPServerClientDetail
        Inherits TCPClient.TCPClientDetail

        ''' <summary>
        ''' 客户端唯一号
        ''' </summary>
        Public ReadOnly ClientID As Long

        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey"></param>
        Public Sub New(skey As String)
            MyBase.New(String.Empty, 0)
            ConnectAlias = skey
        End Sub


        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey">指示此节点的唯一Key值</param>
        ''' <param name="_remote">远程客户端身份</param>
        Public Sub New(skey As String, _remote As IPDetail, _local As IPDetail, ByVal iClientID As Long)
            MyBase.New(_remote.Addr, _remote.Port, _local.Addr, _local.Port)
            ConnectAlias = skey
            ClientID = iClientID
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
            Return ConnectorType.TCPServerClient
        End Function


        ''' <summary>
        ''' 进行比较，看是否指向同一个客户端节点
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(other As INConnectorDetail) As Boolean
            Dim svr As TCPServerClientDetail = TryCast(other, TCPServerClientDetail)
            If svr Is Nothing Then Return False
            If String.IsNullOrEmpty(ConnectAlias) Then
                Return String.IsNullOrEmpty(svr.ConnectAlias)
            Else
                Return ConnectAlias.Equals(svr.ConnectAlias)
            End If
        End Function

        ''' <summary>
        ''' 获取一个关于客户端节点的Key值
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetKey() As String
            Return ConnectAlias
        End Function

        ''' <summary>
        ''' 打印此详情所指示的连接信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $" {GetTypeName()} Key： {ConnectAlias}"
        End Function
    End Class
End Namespace

