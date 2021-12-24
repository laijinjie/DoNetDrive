Imports System.Collections.Concurrent
Imports DotNetty.Common.Concurrency
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Data
Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.TaskManage
Imports System.Threading

Namespace Connector
    ''' <summary>
    ''' 连接通道的抽象类，定义了一组连接通道的基本运行逻辑
    ''' </summary>
    Public MustInherit Class AbstractConnector
        Implements INConnector

        ''' <summary>
        ''' 默认的保活包
        ''' </summary>
        Public Shared DefaultKeepalivePacket As Byte() = System.Text.Encoding.ASCII.GetBytes("PING")

        ''' <summary>
        ''' 默认的通道连接保持时间
        ''' </summary>
        Public Shared DefaultChannelKeepaliveMaxTime As Integer = 60

        ''' <summary>
        ''' 通道保活时长，单位秒，超过这个时间没有调用  UpdateActivityTime 函数就会自动断开连接
        ''' </summary>
        Public Property ChannelKeepaliveMaxTime As Integer = 60

        ''' <summary>
        ''' 命令间的发送间隔，单位毫秒
        ''' </summary>
        ''' <returns></returns>
        Public Property CommandSendIntervalTimeMS As Integer = 0

        ''' <summary>
        ''' 连接通道中存在于队列中的命令列表，先进先出集合
        ''' </summary>
        Protected _CommandList As ConcurrentQueue(Of INCommandRuntime)

        ''' <summary>
        ''' 解码器列表，处理连接管道接收和发送的所有数据
        ''' </summary>
        Protected _DecompileList As ConcurrentDictionary(Of String, INRequestHandle)

        ''' <summary>
        ''' 表示此连接是保持连接
        ''' </summary>
        Protected _IsForcibly As Boolean

        ''' <summary>
        ''' 正在活动中的命令
        ''' </summary>
        Protected _ActivityCommand As INCommandRuntime

        ''' <summary>
        ''' 连接器通道的状态
        ''' </summary>
        Protected _Status As INConnectorStatus

        ''' <summary>
        ''' 指示此通道是否已释放
        ''' </summary>
        Protected _isRelease As Boolean

        ''' <summary>
        ''' 表示此通道已失效,失效表示连接已断开并达到重试上限或已被手动关闭
        ''' </summary>
        Private _isInvalid As Boolean

        ''' <summary>
        ''' 表示此通道上次的活动时间，最近收发数据的时间
        ''' </summary>
        Protected _ActivityDate As Date


        ''' <summary>
        ''' 表示此通道最近一次接受数据的时间
        ''' </summary>
        Protected _LastReadDataTime As Date

        ''' <summary>
        ''' 表示此通道最近一次的发送数据的时间
        ''' </summary>
        Protected _LastSendDataTime As Date

        ''' <summary>
        ''' 发送保活包时间
        ''' </summary>
        Protected _SendKeepaliveTime As Date

        ''' <summary>
        ''' 保活包最大间隔
        ''' </summary>
        Protected _KeepAliveMaxTime As Integer

        ''' <summary>
        ''' 发送字节数
        ''' </summary>
        Protected _SendTotalBytes As Long

        ''' <summary>
        ''' 接收字节数
        ''' </summary>
        Protected _ReadTotalBytes As Long

        ''' <summary>
        ''' 累计处理的命令数量
        ''' </summary>
        Protected _CommandTotal As Long


        ''' <summary>
        ''' 检查通道是否为活动的状态（已连接的）
        ''' </summary>
        Protected _IsActivity As Boolean


        ''' <summary>
        ''' 连接开始时间
        ''' </summary>
        Protected _ConnectDate As Date

        ''' <summary>
        ''' 连接通道的唯一身份标识
        ''' </summary>
        Private mKey As String

        ''' <summary>
        ''' 标识本通道的信息类，只读
        ''' </summary>
        Protected _ConnectorDetail As INConnectorDetail


#Region "事件定义"
        ''' <summary>
        ''' 当命令完成时，会触发此函数回调
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandCompleteEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandCompleteEvent
        ''' <summary>
        ''' 命令进度指示，当命令开始执行会连续触发，汇报命令执行的进度
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandProcessEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandProcessEvent
        ''' <summary>
        ''' 发生错误时触发事件，一般是连接握手失败，串口不存在，usb不存在，没有写文件权限等
        ''' 还有可能是用户调用Stop指令强制停止命令
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandErrorEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandErrorEvent
        ''' <summary>
        ''' 命令超时时，触发此回到函数
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event CommandTimeout(sender As Object, e As CommandEventArgs) Implements INCommandEvent.CommandTimeout

        ''' <summary>
        ''' 身份鉴权时发生错误的事件,
        ''' 一般发生于密码错误，校验失败等情况！
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Event AuthenticationErrorEvent(sender As Object, e As CommandEventArgs) Implements INCommandEvent.AuthenticationErrorEvent

        ''' <summary>
        ''' 事务消息，有些命令发生后会需要异步等待对端传回结果，结果将自动序列化为事物消息，并触发此事件
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        ''' <param name="EventData">事件所包含数据</param>
        Public Event TransactionMessage(connector As INConnectorDetail, EventData As INData) Implements INConnectorEvent.TransactionMessage
        ''' <summary>
        ''' 客户端上线
        ''' </summary>
        ''' <param name="sender">触发事件的连接通道信息</param>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Public Event ClientOnline(sender As Object, e As ServerEventArgs) Implements INConnectorEvent.ClientOnline
        ''' <summary>
        ''' 客户端离线
        ''' </summary>
        ''' <param name="sender">触发事件的连接通道信息</param>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Public Event ClientOffline(sender As Object, e As ServerEventArgs) Implements INConnectorEvent.ClientOffline

        ''' <summary>
        ''' 连接通道发生错误时触发事件
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Public Event ConnectorErrorEvent(sender As Object, connector As INConnectorDetail) Implements INConnectorEvent.ConnectorErrorEvent
        ''' <summary>
        ''' 连接通道连接建立成功时发生
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Public Event ConnectorConnectedEvent(sender As Object, connector As INConnectorDetail) Implements INConnectorEvent.ConnectorConnectedEvent

        ''' <summary>
        ''' 连接通道连接关闭时发生
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="connector"></param>
        Public Event ConnectorClosedEvent(sender As Object, connector As INConnectorDetail) Implements INConnectorEvent.ConnectorClosedEvent
#End Region

        ''' <summary>
        ''' 初始化连接通道中的队列列表和解析器列表，并初始化连接通道状态
        ''' </summary>
        Public Sub New()
            _CommandList = New ConcurrentQueue(Of INCommandRuntime)
            _DecompileList = New ConcurrentDictionary(Of String, INRequestHandle)
            _ActivityCommand = Nothing

            _IsForcibly = False
            _IsActivity = False
            _isRelease = False
            _isInvalid = False

            _Status = GetInitializationStatus()
            _ActivityDate = Date.Now
            _LastReadDataTime = Date.MinValue
            _LastSendDataTime = Date.MinValue
            _SendKeepaliveTime = Date.MinValue
            _KeepAliveMaxTime = 30

            ChannelKeepaliveMaxTime = DefaultChannelKeepaliveMaxTime
        End Sub

        Protected Overrides Sub Finalize()
            'Trace.WriteLine($"{GetKey()} 被销毁 {DateTime.Now:HH:mm:ss.fff}")
            Dispose()
            MyBase.Finalize()
        End Sub


#Region "虚函数，需要派生类实现，具体操作连接通道的方法"


        ''' <summary>
        ''' 获取初始化通道状态
        ''' </summary>
        Protected MustOverride Function GetInitializationStatus() As INConnectorStatus

        ''' <summary>
        ''' 获取此通道的连接器类型
        ''' </summary>
        ''' <returns>连接器类型</returns>
        Public MustOverride Function GetConnectorType() As String Implements INConnector.GetConnectorType

        ''' <summary>
        ''' 返回记录此通道信息的类
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetConnectorDetail() As INConnectorDetail Implements INConnector.GetConnectorDetail
            If _ConnectorDetail Is Nothing Then
                _ConnectorDetail = GetConnectorDetail0()
            End If
            Return _ConnectorDetail
        End Function


        Public Overridable Function GetKey() As String Implements INConnector.GetKey
            If String.IsNullOrEmpty(mKey) Then
                Dim dtl = GetConnectorDetail()
                If dtl IsNot Nothing Then
                    mKey = dtl.GetKey()
                Else
                    mKey = String.Empty
                End If
            End If
            Return mKey
        End Function

        ''' <summary>
        ''' 获取关于本通道的详情
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetConnectorDetail0() As INConnectorDetail
            Return _ConnectorDetail
        End Function


        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetByteBufAllocator() As IByteBufferAllocator Implements INConnector.GetByteBufAllocator
            Return UnpooledByteBufferAllocator.Default
        End Function

        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetEventLoop() As IEventLoop Implements INConnector.GetEventLoop
            Return TaskEventLoop.Default
        End Function

        ''' <summary>
        ''' 获取本地绑定地址
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function LocalAddress() As IPDetail Implements INConnector.LocalAddress


        ''' <summary>
        ''' 获取远程服务器地址
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function RemoteAddress() As IPDetail Implements INConnector.RemoteAddress
#End Region


#Region "命令管理"
        ''' <summary>
        ''' 命令队列数量
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCommandCount() As Integer Implements INConnector.GetCommandCount
            If (_isRelease) Then Return 0
            If _CommandList Is Nothing Then Return 0
            Return _CommandList.Count()
        End Function

        ''' <summary>
        ''' 命令队列是否为空
        ''' </summary>
        ''' <returns></returns>
        Public Function CommandListIsEmpty() As Boolean Implements INConnector.CommandListIsEmpty
            If (_isRelease) Then Return True
            Return _CommandList.IsEmpty()
        End Function


        Public Sub AddCommand(cd As INCommandRuntime) Implements INConnector.AddCommand
            cd.SetConnector(Me)

            If (_isRelease) Then
                cd.RemoveBinding()
                cd.SetStatus(cd.GetStatus_Faulted())

                Return
            End If
            _CommandTotal += 1
            _CommandList.Enqueue(cd)
        End Sub

        ''' <summary>
        ''' 将一个命令添加到本通道的命令队列中，并等待命令执行完毕
        ''' </summary>
        ''' <param name="cd"></param>
        ''' <returns></returns>
        Public Overridable Async Function RunCommandAsync(cd As INCommandRuntime) As Task(Of INCommand) Implements INConnector.RunCommandAsync
            Dim CommandTaskCompletionSource As TaskCompletionSource(Of INCommand) = New TaskCompletionSource(Of INCommand)
            cd.SetConnector(Me)

            If (_isRelease) Then
                cd.RemoveBinding()
                cd.SetStatus(cd.GetStatus_Faulted())

                Return cd
            End If
            cd.SetTaskCompletionSource(CommandTaskCompletionSource)
            _CommandTotal += 1
            _CommandList.Enqueue(cd)

            Return Await CommandTaskCompletionSource.Task
        End Function

        ''' <summary>
        ''' 从队列中删除一个命令
        ''' </summary>
        ''' <param name="eventCommand"></param>
        Protected Overridable Sub RemoveCommand(ByVal eventCommand As INCommandRuntime, ByVal bAutoCheckCommandList As Boolean)
            If (_isRelease) Then Return
            Try
                If eventCommand IsNot Nothing Then
                    If Object.ReferenceEquals(_ActivityCommand, eventCommand) Then
                        Dim tmp As INCommandRuntime = Nothing
                        If _CommandList.TryPeek(tmp) Then
                            If Object.ReferenceEquals(_ActivityCommand, tmp) Then
                                tmp = Nothing
                                Call _CommandList.TryDequeue(tmp)
                                tmp = Nothing
                            End If
                            tmp = Nothing
                        End If
                        _ActivityCommand?.RemoveBinding()
                        _ActivityCommand = Nothing
                    End If
                End If

                If bAutoCheckCommandList Then
                    If _ActivityCommand Is Nothing And _CommandList IsNot Nothing Then
                        If Me.CheckIsInvalid Then Return
                        If Not _CommandList.IsEmpty Then CheckCommandList()
                    End If
                End If
            Catch ex As Exception
                If (_isRelease) Then Return
            End Try

        End Sub

        ''' <summary>
        ''' 检查命令状态
        ''' </summary>
        Public Overridable Sub CheckCommandList() Implements INConnector.CheckCommandList
            If (_isRelease) Then Return
            If _ActivityCommand IsNot Nothing Then Return
            UpdateActivityTime()
            If _CommandList.Count = 0 Then Return
            Do
                Try
                    If _CommandList.Count = 0 Then Return
                    If _CommandList.TryPeek(_ActivityCommand) Then
                        If _ActivityCommand.IsRelease() Then
                            RemoveCommand(_ActivityCommand, False)
                        Else
                            If CommandSendIntervalTimeMS > 0 Then
                                '将命令放到管道相同的线程中执行
                                GetEventLoop()?.Schedule(_ActivityCommand, TimeSpan.FromMilliseconds(CommandSendIntervalTimeMS))
                            Else
                                '将命令放到管道相同的线程中执行
                                GetEventLoop()?.Execute(_ActivityCommand)
                            End If

                            Exit Do
                        End If
                    End If
                Catch ex As Exception
                    Trace.WriteLine($" {GetKey()} CheckCommandList {ex.ToString()}")
                End Try
            Loop While True
        End Sub

#Region "停止命令"
        ''' <summary>
        ''' 停止指定类型的命令，终止命令继续执行
        ''' </summary>
        ''' <param name="cdt">命令详情，如果为Null表示停止此通道中的所有命令</param>
        ''' <returns></returns>
        Public Function StopCommand(cdt As INCommandDetail) As Boolean Implements INConnector.StopCommand
            If (_isRelease) Then Return True
            Dim cmd As INCommandRuntime = Nothing
            If cdt Is Nothing Then
                '停止所有命令
                ClearCommand(New Exception("Stop"))
                Return True
            Else
                Dim cmdCount As Integer = 0
                Dim sKey = cdt.Key
                For Each cmd In _CommandList
                    If cmd.CommandDetail.Key = sKey Then
                        cmdCount += 1
                        Exit For
                    End If
                Next

                Dim tmpQue As Queue(Of INCommandRuntime) = New Queue(Of INCommandRuntime)
                If cmdCount > 0 Then
                    Dim retGetCommand As Boolean
                    Do
                        cmd = Nothing
                        retGetCommand = _CommandList.TryDequeue(cmd)
                        If retGetCommand Then
                            If Not cmd.CommandDetail.ToString() = sKey Then
                                tmpQue.Enqueue(cmd) '临时加入队列
                            Else
                                cmd.RemoveBinding()
                                If Object.ReferenceEquals(cmd, _ActivityCommand) Then
                                    _ActivityCommand = Nothing
                                End If
                                RaiseCommandErrorEvent(cmd, True)
                            End If
                        End If
                    Loop While retGetCommand
                    If tmpQue.Count > 0 Then
                        Do
                            cmd = tmpQue.Dequeue()
                            _CommandList.Enqueue(cmd)
                        Loop While tmpQue.Count > 0
                    End If

                End If

            End If
            Return True
        End Function

        ''' <summary>
        ''' 清空所有在缓冲中的命令
        ''' </summary>
        ''' <param name="isStop"></param>
        Protected Overridable Sub ClearCommand(ByVal ex As Exception)
            If (_isRelease) Then Return
            Dim cmd As INCommandRuntime = Nothing
            Dim retGetCommand As Boolean
            _ActivityCommand = Nothing
            If _CommandList Is Nothing Then Return
            Do
                cmd = Nothing
                retGetCommand = _CommandList.TryDequeue(cmd)
                If retGetCommand Then
                    cmd.RemoveBinding()
                    If ex.Message = "Stop" Then
                        cmd.SetStatus(cmd.GetStatus_Stop())
                    Else
                        cmd.SetException(ex)
                    End If
                    RaiseEvent CommandErrorEvent(Me, cmd.GetEventArgs)
                End If
            Loop While retGetCommand
        End Sub

        ''' <summary>
        ''' 连接错误时，触发此回到函数
        ''' </summary>
        ''' <param name="cmd"></param>
        ''' <param name="bIsStopCommand"></param>
        Private Sub RaiseCommandErrorEvent(cmd As INCommandRuntime, bIsStopCommand As Boolean)

            Dim args = cmd.GetEventArgs
            If bIsStopCommand Then
                cmd.SetStatus(cmd.GetStatus_Stop())
            Else
                cmd.SetStatus(cmd.GetStatus_Faulted())
            End If
            RaiseEvent CommandErrorEvent(Me, args)
        End Sub

#End Region

#End Region

#Region "通道状态指示和自检函数"
        ''' <summary>
        ''' 确定通道是否已失效
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsInvalid As Boolean Implements INConnector.IsInvalid
            Get
                If _isRelease Then Return _isRelease

                Return _isInvalid
            End Get
        End Property

        ''' <summary>
        ''' 将通道设置为无效状态
        ''' </summary>
        Protected Sub SetInvalid()
            _isInvalid = True
            Me._Status = ConnectorStatus.Invalid
        End Sub

        ''' <summary>
        ''' 检查通道是否为活动的状态（已连接的）
        ''' </summary>
        ''' <returns></returns>
        Public Function IsActivity() As Boolean Implements INConnector.IsActivity
            If _isRelease Then Return False
            Return _IsActivity
        End Function


        ''' <summary>
        ''' 更新通道活动时间
        ''' </summary>
        Public Sub UpdateActivityTime() Implements INConnector.UpdateActivityTime
            _ActivityDate = Date.Now
        End Sub

        Public ReadOnly Property LastReadDataTime As DateTime Implements INConnector.LastReadDataTime
            Get
                Return _LastReadDataTime
            End Get

        End Property

        Public ReadOnly Property LastSendDataTime As DateTime Implements INConnector.LastSendDataTime
            Get
                Return _LastSendDataTime
            End Get

        End Property


        Public ReadOnly Property SendTotalBytes As Long Implements INConnector.SendTotalBytes
            Get
                Return _SendTotalBytes
            End Get

        End Property


        Public ReadOnly Property ReadTotalBytes As Long Implements INConnector.ReadTotalBytes
            Get
                Return _ReadTotalBytes
            End Get

        End Property

        Public ReadOnly Property CommandTotal As Long Implements INConnector.CommandTotal
            Get
                Return _CommandTotal
            End Get

        End Property

        ''' <summary>
        ''' 更新通道活动时间
        ''' </summary>
        Protected Sub UpdateReadDataTime()
            '_ActivityDate = Date.Now
            _LastReadDataTime = Date.Now
        End Sub


        ''' <summary>
        ''' 更新通道活动时间
        ''' </summary>
        Protected Sub UpdateSendDataTime()
            _ActivityDate = Date.Now
            _LastSendDataTime = Date.Now
        End Sub

        ''' <summary>
        ''' 检查通道是否已失效 1分钟无活动，无命令任务则自动失效
        ''' </summary>
        Protected Overridable Function CheckIsInvalid() As Boolean Implements INConnector.CheckIsInvalid
            If (_isRelease) Then Return True
            If _isInvalid Then Return True '已失效
            If _IsForcibly Then
                _isInvalid = False
                Return False
            End If

            If Not _CommandList.IsEmpty Then
                _isInvalid = False
                Return False
            End If

            Dim lElapse = (Date.Now - _ActivityDate).TotalSeconds()
            If (lElapse > ChannelKeepaliveMaxTime) Then

                SetInvalid()
                Return True
            Else
                Return False
            End If

        End Function



        ''' <summary>
        '''  判断此通道是否保持连接，即通道在发送完毕命令后保持连接
        ''' </summary>
        ''' <returns> true 表示通道保持打开</returns>
        Public Function IsForciblyConnect() As Boolean Implements INConnector.IsForciblyConnect
            If (_isRelease) Then Return False
            Return _IsForcibly
        End Function

        ''' <summary>
        ''' 设定此连接器通道为保持打开状态
        ''' </summary>
        Public Sub OpenForciblyConnect() Implements INConnector.OpenForciblyConnect
            _IsForcibly = True
        End Sub

        ''' <summary>
        ''' 禁止此连接器通道为保持连接状态，即命令发送完毕后关闭连接。
        ''' </summary>
        Public Sub CloseForciblyConnect() Implements INConnector.CloseForciblyConnect
            _IsForcibly = False
        End Sub

        ''' <summary>
        ''' 获取此连接通道的状态
        ''' </summary>
        ''' <returns></returns>
        Public Function GetStatus() As INConnectorStatus Implements INConnector.GetStatus
            Return _Status
        End Function
#End Region

#Region "用于执行通道自检，推进通道内命令执行"
        ''' <summary>
        ''' 开始执行这个连接通道
        ''' </summary>
        Public Sub Run() Implements IRunnable.Run
            If (_isRelease) Then Return
            Try
                If (_isRelease) Then Return
                CheckStatus()
            Catch ex As Exception
                Trace.WriteLine($"key:{GetKey()} AbstractConnector.Run 出现错误：{ex.ToString()}")
            End Try
        End Sub

        ''' <summary>
        ''' 检查当前状态
        ''' </summary>
        Protected Overridable Sub CheckStatus()
            _Status?.CheckStatus(Me)
        End Sub
#End Region


#Region "产生事件的函数"

#Region "触发事件--命令完毕"
        ''' <summary>
        ''' 触发命令完成消息，并将当前命令从队列中移除
        ''' </summary>
        Public Sub fireCommandCompleteEvent(e As CommandEventArgs) Implements INConnector.FireCommandCompleteEvent

            RaiseEvent CommandCompleteEvent(Me, e)
            RemoveCommand(e.Command, True)
        End Sub

        ''' <summary>
        ''' 触发命令完成消息 ，但是并不移除当前命令
        ''' </summary>
        Public Sub fireCommandCompleteEventNotRemoveCommand(e As CommandEventArgs) Implements INConnector.fireCommandCompleteEventNotRemoveCommand

            RaiseEvent CommandCompleteEvent(Me, e)
        End Sub
#End Region

#Region "触发事件--命令进度指示"
        ''' <summary>
        ''' 触发事件--命令进度指示
        ''' </summary>
        ''' <param name="e"></param>
        Public Sub fireCommandProcessEvent(e As CommandEventArgs) Implements INConnector.FireCommandProcessEvent
            RaiseEvent CommandProcessEvent(Me, e)
        End Sub
#End Region

#Region "触发事件-- 命令超时"
        ''' <summary>
        ''' 触发事件-- 命令超时
        ''' </summary>
        ''' <param name="e"></param>
        Public Sub fireCommandTimeout(e As CommandEventArgs) Implements INConnector.FireCommandTimeout

            RaiseEvent CommandTimeout(Me, e)
            RemoveCommand(e.Command, True)
        End Sub
#End Region

#Region "触发事件--身份鉴权错误"
        ''' <summary>
        ''' 触发事件--身份鉴权错误
        ''' </summary>
        ''' <param name="e"></param>
        Public Sub fireAuthenticationErrorEvent(e As CommandEventArgs) Implements INConnector.FireAuthenticationErrorEvent

            RaiseEvent AuthenticationErrorEvent(Me, e)
            RemoveCommand(e.Command, True)
        End Sub
#End Region

#Region "触发事件--命令错误事件"
        ''' <summary>
        ''' 触发事件--命令错误事件
        ''' </summary>
        ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
        Public Sub FireCommandErrorEvent(e As CommandEventArgs) Implements INFireCommandEvent.FireCommandErrorEvent
            RaiseEvent CommandErrorEvent(Me, e)
            RemoveCommand(e.Command, True)
        End Sub
#End Region


#Region "触发事件--事务消息"
        ''' <summary>
        ''' 触发事件--事务消息
        ''' </summary>
        ''' <param name="EventData">事件所包含数据</param>
        Public Sub fireTransactionMessage(EventData As INData) Implements INConnector.FireTransactionMessage
            RaiseEvent TransactionMessage(Me.GetConnectorDetail(), EventData)
        End Sub
#End Region



#Region "触发事件--客户端上线通知"
        ''' <summary>
        ''' 客户端上线通知
        ''' </summary>
        ''' <param name="conn">客户端所绑定的连接器</param>
        Public Sub FireClientOnline(conn As INConnector) Implements INFireConnectorEvent.FireClientOnline
            GetConnectorDetail()?.ClientOnlineCallBlack?(conn)
            RaiseEvent ClientOnline(conn, Nothing)
        End Sub
#End Region

#Region "触发事件--客户端离线通知"
        ''' <summary>
        ''' 触发事件--客户端离线通知
        ''' </summary> 
        Public Sub FireClientOffline(conn As INConnector) Implements INFireConnectorEvent.FireClientOffline
            GetConnectorDetail()?.ClientOfflineCallBlack?(conn)
            RaiseEvent ClientOffline(conn, Nothing)
        End Sub
#End Region

#Region "产生通道已连接的事件"
        ''' <summary>
        ''' 产生通道已连接的事件
        ''' </summary>
        Protected Sub FireConnectorConnectedEvent(connector As INConnectorDetail) Implements INFireConnectorEvent.FireConnectorConnectedEvent
            connector?.ConnectedCallBlack?(connector)
            RaiseEvent ConnectorConnectedEvent(Me, connector)
        End Sub
#End Region

        ''' <summary>
        ''' 连接通道连接关闭时发生
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Public Overridable Sub FireConnectorClosedEvent(connector As INConnectorDetail) Implements INFireConnectorEvent.FireConnectorClosedEvent
            ClearCommand(New Exception("Connect Closed"))
            connector?.ClosedCallBlack?(connector)
            RaiseEvent ConnectorClosedEvent(Me, connector)
        End Sub

        ''' <summary>
        ''' 连接通道发生错误时触发事件
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Public Sub FireConnectorErrorEvent(connector As INConnectorDetail) Implements INFireConnectorEvent.FireConnectorErrorEvent
            ClearCommand(New Exception("Connect error", connector.GetError))
            connector?.ErrorCallBlack?(connector)
            RaiseEvent ConnectorErrorEvent(Me, connector)
        End Sub
#End Region


#Region "解析器管理"
        ''' <summary>
        ''' 当需要解析监控指令时，添加数据包解析器到解析器列表中
        ''' </summary>
        ''' <param name="handle">数据包解析器</param>
        Public Sub AddRequestHandle(handle As INRequestHandle) Implements INConnector.AddRequestHandle
            If (_isRelease) Then Return
            If (handle Is Nothing) Then Return
            If _DecompileList Is Nothing Then Return

            Dim key = handle.GetType().FullName()
            If (Not _DecompileList.ContainsKey(key)) Then
                _DecompileList.TryAdd(key, handle)
            End If
        End Sub

        ''' <summary>
        ''' 从连接通道中删除指定类型的数据包解析器
        ''' </summary>
        ''' <param name="handle">数据包解析器的类型</param>
        Public Sub RemoveRequestHandle(handle As Type) Implements INConnector.RemoveRequestHandle
            If (_isRelease) Then Return
            If (handle Is Nothing) Then Return
            If _DecompileList Is Nothing Then Return

            Dim key = handle.FullName()
            Dim oHandle As INRequestHandle = Nothing
            If _DecompileList.TryRemove(key, oHandle) Then

                oHandle.Dispose()
                oHandle = Nothing
            End If
        End Sub

        ''' <summary>
        ''' 解析收到的监控数据包
        ''' </summary>
        ''' <param name="msg"></param>
        Protected Sub DisposeRequest(msg As IByteBuffer)
            If (_isRelease) Then Return
            If _DecompileList Is Nothing Then Return
            If (_DecompileList.IsEmpty) Then Return

            Dim iMaskReaderIndex = msg.ReaderIndex
            For Each wr In _DecompileList
                Dim reqHandle As INRequestHandle = wr.Value
                Try
                    reqHandle.DisposeRequest(Me, msg)
                Catch ex As Exception

                End Try

                msg.SetReaderIndex(iMaskReaderIndex)
            Next
        End Sub

        ''' <summary>
        ''' 处理命令的响应包
        ''' </summary>
        Protected Sub DisposeResponse(msg As IByteBuffer)
            If (_isRelease) Then Return
            If _DecompileList Is Nothing Then Return
            If (_DecompileList.IsEmpty) Then Return

            Dim iMaskReaderIndex = msg.ReaderIndex
            For Each wr In _DecompileList
                Dim reqHandle As INRequestHandle = wr.Value
                reqHandle.DisposeResponse(Me, msg)
                msg.SetReaderIndex(iMaskReaderIndex)
            Next
        End Sub
#End Region

#Region "读取数据"
        ''' <summary>
        ''' 读取到数据后的处理
        ''' </summary>
        ''' <param name="msg">将读取到的数据打包到bytebuffer</param>
        Protected Sub ReadByteBuffer(msg As IByteBuffer)
            If _isRelease Then Return
            UpdateReadDataTime()
            Me._ReadTotalBytes += msg.ReadableBytes
            Dim tmpMsg = Unpooled.UnreleasableBuffer(msg)
            Dim iMaskReadIndex As Integer = tmpMsg.ReaderIndex

            If _ActivityCommand IsNot Nothing Then
                'Trace.WriteLine($"通道：{GetConnectorDetail().GetKey()}，尝试解析命令返回值 ")

                _ActivityCommand.PushReadByteBuf(msg)
            Else
                'Trace.WriteLine($"通道：{GetConnectorDetail().GetKey()}，通道空闲没有等待的指令 ")
            End If

            tmpMsg.SetReaderIndex(iMaskReadIndex)
            If _DecompileList.Count = 0 Then
                'Trace.WriteLine($"收到数据：{GetKey()} Len:{ msg.ReadableBytes} 但是未找到处理器")
            End If

            DisposeRequest(tmpMsg)
            tmpMsg = Nothing
            '检查是否需要发送下一条命令 --  #

        End Sub
#End Region

#Region "写入数据"
        ''' <summary>
        ''' 将生成的bytebuf写入到通道中
        ''' 写入完毕后自动释放
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overridable Async Function WriteByteBuf(buf As IByteBuffer) As Task Implements INConnector.WriteByteBuf
            If _isRelease Then Return
            UpdateSendDataTime()

            DisposeResponse(buf)
            Me._SendTotalBytes += buf.ReadableBytes
            Await WriteByteBuf0(buf)
        End Function

        ''' <summary>
        ''' 将buf中的数据写入到连接通道中
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Protected MustOverride Function WriteByteBuf0(buf As IByteBuffer) As Task
#End Region


#Region "建立连接"
        Public MustOverride Async Function ConnectAsync() As Task Implements INConnector.ConnectAsync

        ''' <summary>
        ''' 检查是否需要发送保活包
        ''' </summary>
        Protected Overridable Sub CheckKeepaliveTime()
            Dim oBeginTime As DateTime
            If Me._LastReadDataTime = Date.MinValue Then
                '没收到过数据
                oBeginTime = _ConnectDate
            Else
                oBeginTime = Me._LastReadDataTime
            End If

            Dim lTotal = (DateTime.Now - oBeginTime).TotalSeconds
            If lTotal > _KeepAliveMaxTime Then
                Dim bSend As Boolean

                If _SendKeepaliveTime = Date.MinValue Then
                    bSend = True
                Else
                    lTotal = (DateTime.Now - _SendKeepaliveTime).TotalSeconds
                    If lTotal > _KeepAliveMaxTime Then bSend = True
                End If

                If bSend Then
                    SendKeepalivePacket()
                    _SendKeepaliveTime = Date.Now
                End If

            End If
        End Sub

        ''' <summary>
        ''' 发送保活包
        ''' </summary>
        Protected Overridable Sub SendKeepalivePacket()
            If _IsActivity Then
                WriteByteBuf(Unpooled.WrappedBuffer(DefaultKeepalivePacket))
            End If
        End Sub

#End Region

#Region "关闭通道"
        Public MustOverride Function CloseAsync() As Task Implements INConnector.CloseAsync
#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                disposedValue = True

                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。

                    CloseAsync()
                    _IsActivity = False
                    Try
                        Release0()
                    Catch ex As Exception

                    End Try
                    _isRelease = True
                    SetInvalid()
                    _IsForcibly = False


                    If _CommandList IsNot Nothing Then
                        ClearCommand(New Exception("Connect Dispose"))
                    End If
                    _CommandList = Nothing

                    If _DecompileList IsNot Nothing Then
                        For Each wv In _DecompileList
                            wv.Value.Dispose()
                        Next
                        _DecompileList.Clear()
                    End If
                    _DecompileList = Nothing



                End If
                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If

        End Sub

        Protected MustOverride Sub Release0()



        ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码以正确实现可释放模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(True)
            ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
            ' GC.SuppressFinalize(Me)
        End Sub




#End Region

    End Class

End Namespace
