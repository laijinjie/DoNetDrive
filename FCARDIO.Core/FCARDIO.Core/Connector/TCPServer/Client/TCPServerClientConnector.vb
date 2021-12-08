Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports System.Net.Sockets
Imports DoNetDrive.Core.Connector.TCPClient

Namespace Connector.TCPServer.Client
    Friend Class TCPServerClientConnector
        Inherits TCPClientConnector


        ''' <summary>
        ''' 创建一个客户端
        ''' </summary>
        Public Sub New(oDtl As TCPServerClientDetail, client As Socket)
            MyBase.New(oDtl)
            _Client = client
            _Status = TCPClientConnectorStatus.Connected
            Me._IsForcibly = True
            ReceiveAsync()
        End Sub



        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPServerClient
        End Function

#Region "连接服务器"
        Public Overrides Async Function ConnectAsync() As Task
            Await Task.FromException(New Exception("TCP server clinet Cannot connect"))
        End Function

        Protected Overrides Sub ConnectFail(ByVal ex As Exception)
            If TypeOf ex Is ObjectDisposedException Then
                '说明是被强制取消的
                Return
            End If

            If _Status.Status = "Invalid" Then
                Return
            End If

            If _Client IsNot Nothing Then
                _Client.Close()
                _Client.Dispose()
            End If

            _Client = Nothing
            Me._IsActivity = False

            _ConnectorDetail.SetError(ex)
            FireConnectorErrorEvent(_ConnectorDetail)
            Me.SetInvalid() '被关闭了就表示无效了
        End Sub
#End Region

    End Class

End Namespace
