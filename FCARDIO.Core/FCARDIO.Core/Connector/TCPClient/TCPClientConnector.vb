Imports System.Net
Imports DotNetty.Buffers
Imports DotNetty.Handlers.Timeout
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector.Client
Imports DotNetty.Handlers.Tls
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO

Namespace Connector.TCPClient
    ''' <summary>
    ''' 用于和TCP Server进行通讯的TCP Client
    ''' </summary>
    Public Class TCPClientConnector
        Inherits AbstractNettyClientConnector(Of IByteBuffer)

        ''' <summary>
        ''' 本连接通道绑定的通道分配器
        ''' </summary>
        Protected ClientAllocator As TCPClientAllocator


        ''' <summary>
        ''' 连接开始时间
        ''' </summary>
        Protected _ConnectDate As Date

        ''' <summary>
        ''' 连接的异步操作类
        ''' </summary>
        Protected _ConnectFuture As Task(Of IChannel)

        ''' <summary>
        ''' 最大连接等待时间
        ''' </summary>
        Protected _ConnectTimeoutMSEL As Integer

        ''' <summary>
        ''' 最大重新连接次数
        ''' </summary>
        Protected _ReconnectMax As Integer


        ''' <summary>
        ''' 初始化TCP客户端对象
        ''' </summary>
        ''' <param name="acr">通道分配器</param>
        ''' <param name="detail">标识此通道的信息类</param>
        Public Sub New(acr As TCPClientAllocator, detail As TCPClientDetail)
            ClientAllocator = acr
            SetConnectOption(detail)

            ThisConnectorDetail = New TCPClientDetail_Readonly(detail)
            RemoteDetail = New IPDetail(detail.Addr, detail.Port)
            LocalDetail = New IPDetail(detail.LocalAddr, detail.LocalPort)

        End Sub

        ''' <summary>
        ''' 设置连接参数，超时上限和重连上限
        ''' </summary>
        ''' <param name="detail"></param>
        Private Sub SetConnectOption(detail As TCPClientDetail)
            _ConnectTimeoutMSEL = detail.Timeout
            _ReconnectMax = detail.RestartCount

            If (_ConnectTimeoutMSEL > TCPClientAllocator.CONNECT_TIMEOUT_MILLIS_MAX) Then
                _ConnectTimeoutMSEL = TCPClientAllocator.CONNECT_TIMEOUT_MILLIS_MAX
            ElseIf (_ConnectTimeoutMSEL < TCPClientAllocator.CONNECT_TIMEOUT_MILLIS_MIN) Then
                _ConnectTimeoutMSEL = TCPClientAllocator.CONNECT_TIMEOUT_MILLIS_MIN
            End If

            If (_ReconnectMax > TCPClientAllocator.CONNECT_RECONNECT_MAX) Then
                _ReconnectMax = TCPClientAllocator.CONNECT_RECONNECT_MAX
            ElseIf (_ReconnectMax < 0) Then
                _ReconnectMax = 0
            End If
        End Sub

        ''' <summary>
        ''' 设定默认的超时等待和重连参数
        ''' </summary>
        Private Sub SetConnectOptionByDefault()
            _ConnectTimeoutMSEL = TCPClientAllocator.CONNECT_TIMEOUT_MILLIS_MAX
            _ReconnectMax = TCPClientAllocator.CONNECT_RECONNECT_MAX
        End Sub

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.TCPClient
        End Function

        ''' <summary>
        ''' 返回此通道的初始化状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return TCPClientConnectorStatus.Free
        End Function

        ''' <summary>
        ''' 创建一个连接对像详情对象，包含用于描述当前连接通道的信息
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetConnectorDetail0() As INConnectorDetail
            Return Nothing 'New TCPClientDetail_Readonly(RemoteDetail.Addr, RemoteDetail.Port, LocalDetail.Addr, LocalDetail.Port)
        End Function

#Region "连接服务器"

        ''' <summary>
        ''' 开始连接到远端服务器
        ''' </summary>
        Protected Friend Sub ConnectServer()
            If ClientAllocator Is Nothing Then Return


            If _ClientChannel IsNot Nothing Then
                _ClientChannel.CloseAsync().Wait()
                _ClientChannel = Nothing
            End If

            If IsInvalid Then
                Return
            End If
            If _ActivityCommand Is Nothing Then
                '一个新的指令

                If _CommandList.TryPeek(_ActivityCommand) Then
                    _ConnectFailCount = 0
                    _ActivityCommand.SetStatus(_ActivityCommand.GetStatus_Wating())
                    SetConnectOption(_ActivityCommand.CommandDetail.Connector)

                    fireCommandProcessEvent(_ActivityCommand.GetEventArgs())
                Else
                    SetConnectOptionByDefault()
                End If

            Else
                SetConnectOption(_ActivityCommand.CommandDetail.Connector)
            End If
            Try
                _ConnectFuture = ClientAllocator.Connect(GetConnectorDetail(), _ConnectTimeoutMSEL)


                _ConnectFuture.ContinueWith(New Action(Of Task(Of IChannel))(AddressOf connectCallback))

                _Status = TCPClientConnectorStatus.Connecting
            Catch ex As Exception
                _Status = TCPClientConnectorStatus.Free
            End Try

        End Sub

        ''' <summary>
        ''' 连接完毕时的回调函数，指示连接是否已完成
        ''' </summary>
        ''' <param name="tak"></param>
        Protected Sub connectCallback(tak As Task(Of IChannel))
            If _isRelease Then Return
            If tak.IsCanceled Or tak.IsFaulted Then
                ConnectFail()
            Else
                _ClientChannel = tak.Result

                AddChannelHandler()

            End If
            _ConnectFuture = Nothing
        End Sub

        ''' <summary>
        ''' 添加通道处理器
        ''' </summary>
        Protected Overridable Sub AddChannelHandler()
            _Handler = New TCPClientNettyChannelHandler(Of IByteBuffer)(Me)
            _ClientChannel.Pipeline.AddLast(_Handler)
            ConnectSuccess()
        End Sub




        ''' <summary>
        ''' 当连接通道连接已失效时调用
        ''' </summary>
        Protected Overrides Sub ConnectFail0()
            If (_ConnectFailCount >= _ReconnectMax) Then
                FireConnectorErrorEvent(GetConnectorDetail())

                If Not _IsForcibly Then
                    SetInvalid()
                    Dispose() '超过最大连接次数还是连接不上，直接释放此通道所有资源
                End If
            End If
        End Sub


        ''' <summary>
        ''' 获取一个状态表示连接通道连接失败
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetStatus_Fail() As INConnectorStatus
            Return TCPClientConnectorStatus.Fail
        End Function


        ''' <summary>
        ''' 连接通道建立连接成功后的后续处理
        ''' </summary>
        Protected Overrides Sub ConnectSuccess0()
            LocalDetail = New IPDetail(_ClientChannel.LocalAddress)
        End Sub


        ''' <summary>
        ''' 获取一个状态表示连接通道连接已建立并工作正常的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetStatus_Connected() As INConnectorStatus
            Return TCPClientConnectorStatus.Connected
        End Function
#End Region

        ''' <summary>
        ''' 连接关闭后的后续处理
        ''' </summary>
        Protected Overrides Sub CloseConnector0()
            Return
        End Sub




        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release1()
            _ConnectFuture = Nothing
            ClientAllocator = Nothing
        End Sub


    End Class
End Namespace

