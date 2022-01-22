
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports System.Net
Imports System.Net.Sockets

Namespace Connector.TCPServer
    ''' <summary>
    ''' TCPServer监听通道
    ''' </summary>
    Public Class TCPServerConnector
        Inherits AbstractConnector

        ''' <summary>
        ''' 本地端点信息
        ''' </summary>
        Protected Property _LocalAddress As IPDetail
        ''' <summary>
        ''' 服务器套接字
        ''' </summary>
        Protected _Server As Socket


        Protected ConnecterManage As IConnecterManage

        ''' <summary>
        ''' 初始化服务器监听通道
        ''' </summary>
        ''' <param name="detail">通道的详情描述类</param>
        Public Sub New(detail As TCPServerDetail, cntManage As IConnecterManage)
            _CommandList = Nothing
            _DecompileList = Nothing
            _ActivityCommand = Nothing
            _IsForcibly = True
            ConnecterManage = cntManage

            MyBase._ConnectorDetail = detail
            _LocalAddress = New IPDetail(detail.LocalAddr, detail.LocalPort)
        End Sub

        ''' <summary>
        ''' 获取初始化通道状态
        ''' </summary>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return ConnectorStatus.Closed
        End Function


        Public Overrides Function RemoteAddress() As IPDetail
            Return Nothing
        End Function

        Public Overrides Function LocalAddress() As IPDetail
            Return _LocalAddress
        End Function

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPServer
        End Function


        ''' <summary>
        ''' 使用非池化的缓冲区
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetByteBufAllocator() As IByteBufferAllocator
            Return UnpooledByteBufferAllocator.Default
        End Function

        Public Overrides Function GetEventLoop() As IEventLoop
            Return TaskEventLoop.Default
        End Function

#Region "绑定"
        Public Overrides Async Function ConnectAsync() As Task
            If _isRelease Then Return
            If Me.CheckIsInvalid() Then Return
            If Me.IsActivity Then Return
            If Me._Status.Status = "Bind" Then Return

            Dim detail As TCPServerDetail = _ConnectorDetail
            UpdateActivityTime()

            Me._IsActivity = False



            _Server = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            _Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
            Dim oLocalIP As IPEndPoint
            If Not String.IsNullOrEmpty(detail.LocalAddr) Then
                oLocalIP = New IPEndPoint(IPAddress.Parse(detail.LocalAddr), detail.LocalPort)
            Else
                oLocalIP = New IPEndPoint(IPAddress.Any, detail.LocalPort)
            End If
            '需要先绑定本地IP和端口
            _Server.Bind(oLocalIP)
            '开始监听
            _Server.Listen(TCPServerFactory.SoBacklog)

            _ConnectDate = DateTime.Now
            FireConnectorConnectedEvent(_ConnectorDetail)
            '连接成功
            _Status = ConnectorStatus.Bind
            Me._IsActivity = True


            '开始等待响应
            AcceptAsync().ConfigureAwait(False)

            Await Task.CompletedTask()
        End Function


        ''' <summary>
        ''' 异步处理成功的后续操作
        ''' </summary>
        Protected Friend Sub ConnectingNext(connTask As Task)
            If Me.CheckIsInvalid() Then Return
            If Not Me._Status.Status = "Connecting" Then Return
            If _Server Is Nothing Then Return
            If connTask.IsFaulted Or connTask.IsCanceled Then
                '有错误，或已取消
                ConnectFail(connTask.Exception)
            End If
        End Sub

        ''' <summary>
        ''' 连接失败
        ''' </summary>
        Protected Overridable Sub ConnectFail(ByVal ex As Exception)
            If TypeOf ex Is ObjectDisposedException Then
                '说明是被强制取消的
                Return
            End If

            If _Status.Status = "Invalid" Then
                Return
            End If

            If _Server IsNot Nothing Then
                _Server.Close()
                _Server.Dispose()
            End If

            _Server = Nothing
            Me._IsActivity = False

            _ConnectorDetail.SetError(ex)
            FireConnectorErrorEvent(_ConnectorDetail)
            Me.SetInvalid() '被关闭了就表示无效了
        End Sub


#End Region

#Region "接收客户端连接"
        ''' <summary>
        ''' 等待客户端连接
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Async Function AcceptAsync() As Task
            Dim oClient As Socket = Nothing
            Dim iStep As Integer

            Do
                Try

                    If _isRelease Then Return
                    iStep = 0
                    oClient = Await _Server.AcceptAsync(oClient).ConfigureAwait(False)
                    iStep = 1
                    Await AddClient(oClient).ConfigureAwait(False)
                    iStep = 2
                    oClient = Nothing

                Catch ex As Exception
                    '服务器已关闭.
                    Trace.WriteLine($"服务器监听连接时发生错误：{ex}")
                    If oClient IsNot Nothing Then
                        oClient.Close()
                        oClient.Dispose()

                    End If
                     oClient = Nothing
                End Try
            Loop While True
        End Function

        ''' <summary>
        ''' 加入新客户端
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Async Function AddClient(oClient As Socket) As Task



            Await Task.Run(Sub()
                               Dim iClientID As Long
                               Dim sKey = TCPServerFactory.GetClientKey(_LocalAddress, oClient.RemoteEndPoint, iClientID)
                               Dim oDtl = New TCPServer.Client.TCPServerClientDetail(sKey,
                                                                  New IPDetail(oClient.RemoteEndPoint),
                                                                  _LocalAddress,
                                                                  iClientID)
                               oDtl.ClientOnlineCallBlack = Me._ConnectorDetail.ClientOfflineCallBlack
                               oDtl.ClientOfflineCallBlack = Me._ConnectorDetail.ClientOfflineCallBlack
                               Dim oConnClient = New TCPServer.Client.TCPServerClientConnector(oDtl, oClient, ConnecterManage)

                           End Sub).ConfigureAwait(False)
        End Function
#End Region

#Region "关闭连接"
        Public Overrides Async Function CloseAsync() As Task
            If CheckIsInvalid() Then Return
            If Not _IsActivity Then Return


            Me._Status = ConnectorStatus.Invalid
            If _Server Is Nothing Then Return
            Try

                Me._IsForcibly = False
                Me._IsActivity = False

                If _Server IsNot Nothing Then
                    'Trace.WriteLine("关闭连接")
                    _Server.Close()
                    _Server.Dispose()
                End If

            Catch ex As Exception
                _ConnectorDetail.SetError(ex)
            End Try
            _Server = Nothing
            UpdateActivityTime()

            Await Task.Run(Sub()
                               FireConnectorClosedEvent(Me._ConnectorDetail)
                               Me.SetInvalid() '被关闭了就表示无效了
                           End Sub).ConfigureAwait(False)
        End Function
#End Region

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            _Server = Nothing
            _LocalAddress = Nothing
            ConnecterManage = Nothing
        End Sub

        Protected Overrides Function WriteByteBuf0(buf As IByteBuffer) As Task
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

