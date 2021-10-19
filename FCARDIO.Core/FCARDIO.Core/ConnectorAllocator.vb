Imports System.Collections.Concurrent
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Data
Imports DoNetDrive.Core.Factory
Imports DotNetty.Transport.Channels
Imports DotNetty.Common.Concurrency
Imports System.Threading

''' <summary>
''' 基于命令模式。
''' 用于协调连接通道和命令还有调用者之间的关系
''' 将命令推送到指定的连接通道，并监督连接通道的运转情况。
''' </summary>
Public NotInheritable Class ConnectorAllocator
    Implements INConnectorEvent, IDisposable, INCommandEvent

    ''' <summary>
    ''' 默认的通道生成工厂
    ''' </summary>
    Public Shared DefaultConnectorFactory As INConnectorFactory = New DefaultConnectorFactory()

    ''' <summary>
    ''' 默认的事件循环数量
    ''' </summary>
    Public Shared DefaultEventLoopGroupTaskCount As Integer = 0


    ''' <summary>
    ''' 单例模式中的全局变量
    ''' </summary>
    Private Shared staticConnectorAllocator As ConnectorAllocator



    ''' <summary>
    ''' 单例模式中所用到的锁
    ''' </summary>
    Private Shared lockObj As Object = New Object

    ''' <summary>
    ''' 获取分配器的唯一实例
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetAllocator() As ConnectorAllocator
        If staticConnectorAllocator Is Nothing Then
            SyncLock lockObj
                If staticConnectorAllocator Is Nothing Then
                    staticConnectorAllocator = New ConnectorAllocator
                End If
            End SyncLock
        End If
        Return staticConnectorAllocator
    End Function

    ''' <summary>
    ''' 用于生成通道的工厂
    ''' </summary>
    Private ConnectorFactory As INConnectorFactory

    ''' <summary>
    ''' 保存所有的连接通道
    ''' </summary>
    Private Connectors As ConcurrentDictionary(Of String, INConnector)

    ''' <summary>
    ''' 连接分配器的工作线程，用于检查连接，分配
    ''' </summary>
    Private WorkEventLoopGroup As MultithreadEventLoopGroup

    ''' <summary>
    ''' 是否已释放
    ''' </summary>
    Private _IsRelease As Boolean
    ''' <summary>
    ''' 连接管理器工厂
    ''' </summary>
    Private mManagers As ConnectorManageFactory
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
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="connector">触发事件的连接通道信息</param>
    Public Event ConnectorClosedEvent(sender As Object, connector As INConnectorDetail) Implements INConnectorEvent.ConnectorClosedEvent
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
    ''' 表示对象已完成销毁工作
    ''' </summary>
    Public Event DisposeCallBlack()
#End Region

    ''' <summary>
    ''' 是否已释放
    ''' </summary>
    Public Function IsRelease() As Boolean
        Return _IsRelease
    End Function


    ''' <summary>
    ''' 初始化分配器参数
    ''' </summary>
    Private Sub New()
        Connectors = New ConcurrentDictionary(Of String, INConnector)()
        ConnectorFactory = DefaultConnectorFactory
        _IsRelease = False
        If DefaultEventLoopGroupTaskCount <= 0 Then
            DefaultEventLoopGroupTaskCount = Environment.ProcessorCount
        End If
        WorkEventLoopGroup = New MultithreadEventLoopGroup(DefaultEventLoopGroupTaskCount)

        mManagers = New ConnectorManageFactory(WorkEventLoopGroup, Me)
    End Sub


#Region "连接通道操作"
    ''' <summary>
    ''' 获取一个已存在的通道，不存在时则创建一个
    ''' </summary>
    ''' <param name="cdtl"></param>
    ''' <returns></returns>
    Private Function GetOrCreateConnector(cdtl As INConnectorDetail) As INConnector
        If cdtl Is Nothing Then Return Nothing
        If _IsRelease Then Return Nothing
        Dim sKey = cdtl.GetKey()
        Dim Conn As INConnector = Nothing
        If Not Connectors.TryGetValue(sKey, Conn) Then
            '通道不存在，创建一个通道
            SyncLock lockObj
                If Not Connectors.TryGetValue(sKey, Conn) Then
                    '通道不存在，创建一个通道
                    Conn = ConnectorFactory.CreateConnector(cdtl)
                    If Conn IsNot Nothing Then
                        If Not Connectors.TryAdd(sKey, Conn) Then
                            Dim oTmpConn As INConnector = Nothing
                            If Connectors.TryGetValue(sKey, oTmpConn) Then
                                If Not Object.ReferenceEquals(oTmpConn, Conn) Then
                                    '通道已存在
                                    Conn.Dispose()
                                    Conn = Nothing
                                End If

                                Return oTmpConn
                            Else
                                Return Nothing
                            End If
                        Else
                            '通道添加成功
                            '添加通道的事件绑定
                            AddEventListener(Conn)
                            mManagers.GetManager().Add(Conn)
                        End If
                    Else
                        '分配器工厂无法创建连接通道，可能 cdtl 指示了一个无效的值
                        Return Nothing
                    End If
                End If
            End SyncLock
        End If
        Return Conn
    End Function


    ''' <summary>
    ''' 强制打开一个连接通道
    ''' </summary>
    ''' <param name="connectDtl">表示连接通道的信息</param>
    ''' <returns></returns>
    Public Function OpenConnector(connectDtl As INConnectorDetail) As Boolean
        If _IsRelease Then Return False
        Dim connector As INConnector = GetOrCreateConnector(connectDtl)

        Return True
    End Function

    ''' <summary>
    ''' 强制打开一个连接通道
    ''' </summary>
    ''' <param name="connectDtl">表示连接通道的信息</param>
    ''' <returns></returns>
    Public Function OpenForciblyConnect(connectDtl As INConnectorDetail) As Boolean
        If _IsRelease Then Return False
        Dim connector As INConnector = GetOrCreateConnector(connectDtl)
        If connector Is Nothing Then Return False
        connector.OpenForciblyConnect()
        Return True
    End Function

    ''' <summary>
    ''' 强制关闭通道连接
    ''' </summary>
    ''' <param name="connectDtl">表示连接通道的信息</param>
    ''' <returns></returns>
    Public Function CloseConnector(connectDtl As INConnectorDetail) As Boolean
        If _IsRelease Then Return False
        If connectDtl Is Nothing Then Return False
        Dim connector As INConnector = Nothing
        Dim sKey = connectDtl.GetKey()
        If Connectors.TryGetValue(sKey, connector) Then

            connector.Close()
        End If
        Return True
    End Function

    ''' <summary>
    ''' 获取一个连接通道
    ''' </summary>
    ''' <param name="connectDtl"></param>
    ''' <returns></returns>
    Public Function GetConnector(connectDtl As INConnectorDetail) As INConnector
        If _IsRelease Then Return Nothing
        If connectDtl Is Nothing Then Return Nothing
        Dim connector As INConnector = Nothing
        Dim sKey = connectDtl.GetKey()
        If Connectors.TryGetValue(sKey, connector) Then
            Return connector
        End If
        Return Nothing
    End Function


    ''' <summary>
    ''' 获取一个连接通道
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    Public Function GetConnector(sKey As String) As INConnector
        If _IsRelease Then Return Nothing
        Dim connector As INConnector = Nothing
        If Connectors.TryGetValue(sKey, connector) Then
            Return connector
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' 删除一个通道
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <param name="oConn"></param>
    Public Sub RemoveConnector(ByVal sKey As String, oConn As INConnector)
        If _IsRelease Then Return
        If Connectors.TryRemove(sKey, oConn) Then
            'Trace.WriteLine("从 ConnectorAllocator 分配器中移除通道：" & sKey)

            RemoveEventListener(oConn)
        End If
    End Sub

    ''' <summary>
    ''' 添加一个连接通道
    ''' </summary>
    ''' <returns></returns>
    Public Function AddConnector(ByVal sKey As String, conn As INConnector) As Boolean
        If _IsRelease Then Return Nothing
        If Connectors.ContainsKey(sKey) Then
            Return False
        End If

        SyncLock lockObj
            If Connectors.TryAdd(sKey, conn) Then
                '添加通道的事件绑定
                AddEventListener(conn)

                mManagers.GetManager().Add(conn)
                Return True
            Else
                Return False
            End If
        End SyncLock

    End Function

    ''' <summary>
    ''' 获取所有在线的连接通道的Key
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAllConnectorKeys() As List(Of String)
        Return Connectors.Keys.ToList()
    End Function
#End Region

#Region "命令的操作"
    ''' <summary>
    ''' 添加一个指令到分配器，如果通道已存在将推送到通道上，不存在则创建一个通道。
    ''' </summary>
    ''' <param name="cmd">需要执行的命令</param>
    ''' <returns></returns>
    Public Function AddCommand(cmd As INCommand) As Boolean
        If _IsRelease Then Return False
        If cmd Is Nothing Then Return False

        Dim cdtl = cmd?.CommandDetail?.Connector
        If cdtl Is Nothing Then
            Return False
        End If

        If ConnectorFactory Is Nothing Then
            Throw New ArgumentException("ConnectorFactory  is  Null")
        End If

        Dim Conn As INConnector = GetOrCreateConnector(cdtl)

        If Conn Is Nothing Then
            Throw New ArgumentException("Connector is  Null")
        End If

        Conn.AddCommand(cmd)
        Return True
    End Function

    ''' <summary>
    ''' 停止一个命令，如果命令处于队列中，不管是正在排队，还是已经在处理，都可以立刻停止
    ''' </summary>
    ''' <param name="cmdDetail">停止具有相同命令信息的所有指令</param>
    ''' <returns></returns>
    Public Function StopCommand(cmdDetail As INCommandDetail) As Boolean
        If _IsRelease Then Return False
        Dim cdtl = cmdDetail?.Connector
        If cdtl Is Nothing Then
            Return False
        End If
        Dim Conn As INConnector = GetConnector(cdtl)
        If Conn Is Nothing Then Return False
        Conn.StopCommand(cmdDetail)
        Return True
    End Function
#End Region

#Region "事件处理"
    ''' <summary>
    ''' 添加连接通道的事件绑定
    ''' </summary>
    ''' <param name="conn"></param>
    Private Sub AddEventListener(conn As INConnector)
        AddHandler conn.CommandCompleteEvent, AddressOf FireCommandCompleteEvent
        AddHandler conn.CommandProcessEvent, AddressOf FireCommandProcessEvent
        AddHandler conn.CommandErrorEvent, AddressOf FireCommandErrorEvent
        AddHandler conn.CommandTimeout, AddressOf FireCommandTimeout
        AddHandler conn.AuthenticationErrorEvent, AddressOf FireAuthenticationErrorEvent


        AddHandler conn.ConnectorErrorEvent, AddressOf FireConnectorErrorEvent
        AddHandler conn.ConnectorConnectedEvent, AddressOf FireConnectorConnectedEvent
        AddHandler conn.ConnectorClosedEvent, AddressOf FireConnectorClosedEvent
        AddHandler conn.TransactionMessage, AddressOf FireTransactionMessage
        AddHandler conn.ClientOnline, AddressOf FireClientOnline
        AddHandler conn.ClientOffline, AddressOf FireClientOffline
    End Sub

    ''' <summary>
    ''' 解除连接通道的事件绑定
    ''' </summary>
    ''' <param name="conn"></param>
    Private Sub RemoveEventListener(conn As INConnector)
        RemoveHandler conn.CommandCompleteEvent, AddressOf FireCommandCompleteEvent
        RemoveHandler conn.CommandProcessEvent, AddressOf FireCommandProcessEvent
        RemoveHandler conn.CommandErrorEvent, AddressOf FireCommandErrorEvent
        RemoveHandler conn.CommandTimeout, AddressOf FireCommandTimeout
        RemoveHandler conn.AuthenticationErrorEvent, AddressOf FireAuthenticationErrorEvent

        RemoveHandler conn.ConnectorErrorEvent, AddressOf FireConnectorErrorEvent
        RemoveHandler conn.ConnectorConnectedEvent, AddressOf FireConnectorConnectedEvent
        RemoveHandler conn.ConnectorClosedEvent, AddressOf FireConnectorClosedEvent
        RemoveHandler conn.TransactionMessage, AddressOf FireTransactionMessage
        RemoveHandler conn.ClientOnline, AddressOf FireClientOnline
        RemoveHandler conn.ClientOffline, AddressOf FireClientOffline
    End Sub

    ''' <summary>
    ''' 当命令完成时，会触发此函数回调
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
    Private Sub FireCommandCompleteEvent(sender As Object, e As CommandEventArgs)
        Dim cmdDtl = e?.CommandDetail

        Try
            cmdDtl?.FireCommandCompleteEvent(e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandCompleteEvent.CommandDetail {System.Environment.NewLine} {ex.ToString}")
        End Try


        Try
            RaiseEvent CommandCompleteEvent(Me, e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandCompleteEvent {System.Environment.NewLine} {ex.ToString}")
        End Try

    End Sub

    ''' <summary>
    ''' 命令进度指示，当命令开始执行会连续触发，汇报命令执行的进度
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
    Private Sub FireCommandProcessEvent(sender As Object, e As CommandEventArgs)
        Dim cmdDtl = e?.CommandDetail
        Try
            cmdDtl?.FireCommandProcessEvent(e)
        Catch ex As Exception
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} ConnectorAllocator FireCommandProcessEvent.CommandDetail  {System.Environment.NewLine} {ex.ToString}")
        End Try

        Try
            RaiseEvent CommandProcessEvent(Me, e)
        Catch ex As Exception
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} ConnectorAllocator FireCommandProcessEvent  {System.Environment.NewLine} {ex.ToString}")
        End Try

    End Sub

    ''' <summary>
    ''' 发生错误时触发事件，一般是连接握手失败，串口不存在，usb不存在，没有写文件权限等
    ''' 还有可能是用户调用Stop指令强制停止命令
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
    Private Sub FireCommandErrorEvent(sender As Object, e As CommandEventArgs)
        Dim cmdDtl = e?.CommandDetail
        Try
            cmdDtl?.FireCommandErrorEvent(e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name

            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandErrorEvent.CommandDetail  {System.Environment.NewLine} {ex.ToString}")
        End Try

        Try
            RaiseEvent CommandErrorEvent(Me, e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name

            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandErrorEvent  {System.Environment.NewLine} {ex.ToString}")
        End Try
    End Sub

    ''' <summary>
    ''' 命令超时时，触发此回到函数
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
    Private Sub FireCommandTimeout(sender As Object, e As CommandEventArgs)
        Dim cmdDtl = e?.CommandDetail
        Try
            cmdDtl?.FireCommandTimeout(e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandTimeout.CommandDetail  {System.Environment.NewLine} {ex.ToString}")
        End Try

        Try
            RaiseEvent CommandTimeout(Me, e)
        Catch ex As Exception
            Dim oCommand = e?.Command?.GetType()?.Name
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} {oCommand} FireCommandTimeout  {System.Environment.NewLine} {ex.ToString}")
        End Try
    End Sub

    ''' <summary>
    ''' 身份鉴权时发生错误的事件,
    ''' 一般发生于密码错误，校验失败等情况！
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="e">事件参数，包含此事件所代表的命令信息</param>
    Private Sub FireAuthenticationErrorEvent(sender As Object, e As CommandEventArgs)
        Dim cmdDtl = e?.CommandDetail
        Try
            cmdDtl?.FireAuthenticationErrorEvent(e)
        Catch ex As Exception
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} ConnectorAllocator FireAuthenticationErrorEvent.CommandDetail  {System.Environment.NewLine} {ex.ToString}")
        End Try

        Try
            RaiseEvent AuthenticationErrorEvent(Me, e)
        Catch ex As Exception
            Trace.WriteLine($"{cmdDtl?.Connector?.GetKey()} ConnectorAllocator FireAuthenticationErrorEvent  {System.Environment.NewLine} {ex.ToString}")
        End Try
    End Sub



    ''' <summary>
    ''' 连接通道发生错误时触发事件
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="connector">触发事件的连接通道信息</param>
    Private Sub FireConnectorErrorEvent(sender As Object, connector As INConnectorDetail)
        RaiseEvent ConnectorErrorEvent(sender, connector)
    End Sub

    ''' <summary>
    ''' 连接通道连接建立成功时发生
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="connector">触发事件的连接通道信息</param>
    Private Sub FireConnectorConnectedEvent(sender As Object, connector As INConnectorDetail)
        RaiseEvent ConnectorConnectedEvent(sender, connector)
    End Sub

    ''' <summary>
    ''' 连接通道连接关闭时发生
    ''' </summary>
    ''' <param name="sender">触发事件的调用者</param>
    ''' <param name="connector">触发事件的连接通道信息</param>
    Private Sub FireConnectorClosedEvent(sender As Object, connector As INConnectorDetail)
        RaiseEvent ConnectorClosedEvent(sender, connector)
    End Sub

    ''' <summary>
    ''' 事务消息，有些命令发生后会需要异步等待对端传回结果，结果将自动序列化为事物消息，并触发此事件
    ''' </summary>
    ''' <param name="connector">触发事件的连接通道信息</param>
    ''' <param name="EventData">事件所包含数据</param>
    Private Sub FireTransactionMessage(connector As INConnectorDetail, EventData As INData)
        RaiseEvent TransactionMessage(connector, EventData)
    End Sub

    ''' <summary>
    ''' 客户端上线
    ''' </summary>
    ''' <param name="sender">触发事件的连接通道信息</param>
    ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
    Private Sub FireClientOnline(sender As Object, e As ServerEventArgs)
        Dim conn = TryCast(sender, INConnector)
        AddConnector(conn.GetConnectorDetail().GetKey(), conn)
        RaiseEvent ConnectorConnectedEvent(conn, conn.GetConnectorDetail())
        RaiseEvent ClientOnline(conn, e)
    End Sub

    ''' <summary>
    ''' 客户端离线
    ''' </summary>
    ''' <param name="sender">触发事件的连接通道信息</param>
    ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
    Private Sub FireClientOffline(sender As Object, e As ServerEventArgs)

        RaiseEvent ClientOffline(sender, e)
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 要检测冗余调用

    ' IDisposable
    Private Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                'Trace.WriteLine("调用 ConnectorAllocator.Dispose 准备释放动态库资源")
                ' TODO: 释放托管状态(托管对象)。
                _IsRelease = True
                For Each kv In Connectors
                    kv.Value.Dispose()
                Next
                Connectors.Clear()


                mManagers.Dispose()
                'Trace.WriteLine("调用 ConnectorAllocator.Dispose,动态库资源已释放，正在等待线程池退出事件...")
                WorkEventLoopGroup.ShutdownGracefullyAsync().ContinueWith(AddressOf AsyncDisposeCallblack)

            End If

            ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
            ' TODO: 将大型字段设置为 null。
        End If
        disposedValue = True
    End Sub

    Private Sub AsyncDisposeCallblack()
        'Trace.WriteLine("调用 ConnectorAllocator.AsyncDispose 动态库 WorkEventLoopGroup 线程池已完全退出,准备释放 DefaultConnectorFactory 工厂中创建的事件循环组 ")
        WorkEventLoopGroup = Nothing
        DefaultConnectorFactory.Release().ContinueWith(AddressOf AsyncDisposeConnectorFactory)
    End Sub

    Private Sub AsyncDisposeConnectorFactory()
        RaiseEvent DisposeCallBlack()
    End Sub

    ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码以正确实现可释放模式。


    ''' <summary>
    ''' 释放所有线程，关闭所有通道，这个过程需要大概60秒
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        Dispose(True)
        ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
        ' GC.SuppressFinalize(Me)
    End Sub




#End Region




End Class


