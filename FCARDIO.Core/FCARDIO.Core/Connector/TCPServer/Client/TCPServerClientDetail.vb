Imports System.Net

Namespace Connector.TCPServer.Client
    ''' <summary>
    ''' 表示一个TCP服务器节点下的客户端通道详情
    ''' </summary>
    Public Class TCPServerClientDetail
        Inherits AbstractConnectorDetail
        ''' <summary>
        ''' 表示一个代表在TCP服务器节点下的唯一键值，通过此键值查询通道
        ''' </summary>
        Public ReadOnly Key As String

        ''' <summary>
        ''' 远程客户端身份
        ''' </summary>
        Public ReadOnly Remote As IPDetail

        ''' <summary>
        ''' 本地端点
        ''' </summary>
        Public ReadOnly Local As IPDetail

        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey"></param>
        Public Sub New(skey As String)
            Me.New(skey, Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' 指定一个唯一Key值，用于表示一个服务器下的节点客户端
        ''' </summary>
        ''' <param name="skey">指示此节点的唯一Key值</param>
        ''' <param name="_remote">远程客户端身份</param>
        Public Sub New(skey As String, _remote As IPEndPoint, _local As IPEndPoint)
            Key = skey
            Remote = New IPDetail(_remote)
            Local = New IPDetail(_local)
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
            If String.IsNullOrEmpty(Key) Then
                Return String.IsNullOrEmpty(svr.Key)
            Else
                Return Key.Equals(svr.Key)
            End If
        End Function

        ''' <summary>
        ''' 获取一个关于客户端节点的Key值
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetKey() As String
            Return Key
        End Function

        ''' <summary>
        ''' 打印此详情所指示的连接信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $" {GetTypeName()} Key： {Key}"
        End Function
    End Class
End Namespace

