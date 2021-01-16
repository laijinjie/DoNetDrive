Imports System.Net

Namespace Connector
    ''' <summary>
    ''' 服务器事件参数
    ''' </summary>
    Public Class ServerEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' 客户端的key值，后续通过此值和其通讯
        ''' </summary>
        Public ReadOnly ClientKey As String

        ''' <summary>
        ''' 服务器本地IP信息(仅供查看，不可修改)
        ''' </summary>
        Public ReadOnly Local As IPEndPoint

        ''' <summary>
        ''' 远程计算机IP信息(仅供查看，不可修改)
        ''' </summary>
        Public ReadOnly Remote As IPEndPoint



        Sub New(k As String, l As IPEndPoint, r As IPEndPoint)
            ClientKey = k
            Local = l
            Remote = r
        End Sub
    End Class
End Namespace

