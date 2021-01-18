
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports System.Net

Namespace Connector.TCPServer
    ''' <summary>
    ''' TCPServer监听通道
    ''' </summary>
    Public Class TCPServerConnector
        Inherits AbstractConnector
        ''' <summary>
        ''' 服务器监听通道
        ''' </summary>
        Protected _ServerChannel As TcpServerSocketChannelEx

        ''' <summary>
        ''' 表示当前监听器的信息
        ''' </summary>
        Protected _Detail As TCPServerDetail

        ''' <summary>
        ''' 本地端点信息
        ''' </summary>
        Protected mLocal As IPEndPoint

        ''' <summary>
        ''' 初始化服务器监听通道
        ''' </summary>
        ''' <param name="chl">通道的绑定任务</param>
        ''' <param name="detail">通道的详情描述类</param>
        Public Sub New(chl As Task(Of IChannel), detail As TCPServerDetail)
            chl.ContinueWith(AddressOf BindOver)

            _CommandList = Nothing
            _DecompileList = Nothing
            _ActivityCommand = Nothing
            _IsActivity = True
            _IsForcibly = True
            _Detail = detail.Clone
        End Sub

        ''' <summary>
        ''' 绑定完毕
        ''' </summary>
        Private Sub BindOver(t As Task(Of IChannel))
            Threading.Thread.Sleep(20)
            If t.IsCanceled Or t.IsFaulted Then
                _IsActivity = False
                Dim dtl = GetConnectorDetail()
                dtl.SetError(t.Exception)
                FireConnectorErrorEvent(GetConnectorDetail())
                CloseConnector()
            Else
                _IsActivity = True
                _ServerChannel = t.Result
                _ServerChannel.ServerConnector = Me
                FireConnectorConnectedEvent(GetConnectorDetail())

            End If
        End Sub



        ''' <summary>
        ''' 返回本地绑定信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function LocalAddress() As IPDetail
            If _ServerChannel Is Nothing Then Return Nothing
            Return New IPDetail(_ServerChannel.LocalAddress)
        End Function

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPServer
        End Function



        ''' <summary>
        ''' 关闭连接
        ''' </summary>
        Public Overrides Sub CloseConnector()
            If _ServerChannel IsNot Nothing Then
                If _ServerChannel.Active Then
                    _ServerChannel.ServerConnector = Nothing
                    FireConnectorClosedEvent(GetConnectorDetail())
                    _ServerChannel.CloseAsync() '关闭通道

                End If
                _ServerChannel.ServerConnector = Nothing
            End If
            _IsActivity = False
            _IsForcibly = False

            _ServerChannel = Nothing
            _Status = ConnectorStatus_Invalid.Invalid
            SetInvalid()

            CloseConnectCheck()
        End Sub

        ''' <summary>
        ''' 获取初始化通道状态
        ''' </summary>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return ConnectorStatus_Bind.Bind
        End Function

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            CloseConnector()
        End Sub


        ''' <summary>
        ''' 获取关于本通道的详情
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorDetail() As INConnectorDetail
            Return _Detail
        End Function
        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetByteBufAllocator() As IByteBufferAllocator
            Return Nothing
        End Function
        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overrides Function WriteByteBuf(buf As IByteBuffer) As Task
            Return Nothing
        End Function
        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetEventLoop() As IEventLoop
            Return Nothing
        End Function


    End Class
End Namespace

