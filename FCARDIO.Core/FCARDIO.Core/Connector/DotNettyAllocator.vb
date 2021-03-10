Imports System.Threading
Imports DotNetty.Transport.Channels


Namespace Connector
    Public MustInherit Class DotNettyAllocator

        ''' <summary>
        ''' 本系统使用的Buffer分配器
        ''' </summary>
        Public Shared BufferAllocator As DotNetty.Buffers.IByteBufferAllocator

        ''' <summary>
        ''' 服务端主节点事件循环组
        ''' </summary>
        Private Shared ServerParentEventLoopGroup As IEventLoopGroup

        ''' <summary>
        ''' 服务端子节点事件循环组
        ''' </summary>
        Private Shared ServerChildEventLoopGroup As IEventLoopGroup

        ''' <summary>
        ''' 客户端事件循环组 TCP/Client, UDP Client
        ''' </summary>
        Private Shared ClientEventLoopGroup As IEventLoopGroup

        Private Shared mLockObj As Object = New Object

        ''' <summary>
        ''' 使用 Libuv 库
        ''' </summary>
        Public Shared UseLibuv As Boolean = False

        ''' <summary>
        ''' 是否已释放
        ''' </summary>
        Private Shared mIsRelease As Boolean = False


        ''' <summary>
        ''' 默认的事件循环数量
        ''' </summary>
        Public Shared DefaultServerEventLoopGroupCount As Integer = 0

        ''' <summary>
        ''' 默认的事件循环数量
        ''' </summary>
        Public Shared DefaultChildEventLoopGroupCount As Integer = 0



        Shared Sub New()
            UseLibuv = False
            mIsRelease = False
            'BufferAllocator = DotNetty.Buffers.PooledByteBufferAllocator.Default
            BufferAllocator = DotNetty.Buffers.UnpooledByteBufferAllocator.Default
        End Sub

        ''' <summary>
        ''' 检查事件循环组是否已释放
        ''' </summary>
        ''' <returns></returns>
        Shared ReadOnly Property IsRelease() As Boolean
            Get
                Return mIsRelease
            End Get
        End Property

        Shared Function GetBufferAllocator() As DotNetty.Buffers.IByteBufferAllocator
            Return BufferAllocator
        End Function

        Private Shared Sub CheckServerEventLoopGroup()
            If ServerParentEventLoopGroup Is Nothing Then
                SyncLock mLockObj
                    If Not ServerParentEventLoopGroup Is Nothing Then Return

                    'If DotNettyAllocator.UseLibuv Then
                    '    ServerParentEventLoopGroup = New DispatcherEventLoopGroup()
                    '    ServerChildEventLoopGroup = New WorkerEventLoopGroup(ServerParentEventLoopGroup)
                    'Else
                    If DefaultServerEventLoopGroupCount <= 0 Then
                            DefaultServerEventLoopGroupCount = Environment.ProcessorCount
                        End If

                        If DefaultChildEventLoopGroupCount <= 0 Then
                            DefaultChildEventLoopGroupCount = Environment.ProcessorCount * 2
                        End If
                        '静态变量初始化
                        'Trace.WriteLine("调用 DotNettyAllocator.CheckServerEventLoopGroup,创建时间循环组 ServerParentEventLoopGroup,ServerChildEventLoopGroup")
                        ServerParentEventLoopGroup = New MultithreadEventLoopGroup(AddressOf CreateServerParentThreadEventLoop, DefaultServerEventLoopGroupCount)
                        ServerChildEventLoopGroup = New MultithreadEventLoopGroup(AddressOf CreateServerChildThreadEventLoop, DefaultChildEventLoopGroupCount)

                    'End If


                End SyncLock
            End If

        End Sub

        Private Shared Sub CheckClientEventLoopGroup()
            If ClientEventLoopGroup Is Nothing Then
                SyncLock mLockObj
                    If Not ClientEventLoopGroup Is Nothing Then Return
                    'Trace.WriteLine("调用 DotNettyAllocator.ClientEventLoopGroup,创建时间循环组 ClientEventLoopGroup")

                    If DefaultChildEventLoopGroupCount <= 0 Then
                        DefaultChildEventLoopGroupCount = Environment.ProcessorCount * 2
                    End If
                    '静态变量初始化
                    ClientEventLoopGroup = New MultithreadEventLoopGroup(AddressOf CreateThreadEventLoop, DefaultChildEventLoopGroupCount)
                End SyncLock
            End If

        End Sub

        Private Shared ClientEventLoopID, ServerParentEventLoopID, ServerChildEventLoopID As Integer
        Private Shared Function CreateThreadEventLoop(group As IEventLoopGroup) As IEventLoop
            Interlocked.Increment(ClientEventLoopID)
            Return New SingleThreadEventLoop(group, $"DoNetDrive.Core.Connector.DotNettyAllocator.ClientEventLoopGroup.EventLoop {ClientEventLoopID}")
        End Function

        Private Shared Function CreateServerParentThreadEventLoop(group As IEventLoopGroup) As IEventLoop
            Interlocked.Increment(ServerParentEventLoopID)
            Return New SingleThreadEventLoop(group, $"DoNetDrive.Core.Connector.DotNettyAllocator.ServerParentEventLoopGroup.EventLoop {ServerParentEventLoopID}")
        End Function

        Private Shared Function CreateServerChildThreadEventLoop(group As IEventLoopGroup) As IEventLoop
            Interlocked.Increment(ServerChildEventLoopID)
            Return New SingleThreadEventLoop(group, $"DoNetDrive.Core.Connector.DotNettyAllocator.ServerChildEventLoopGroup.EventLoop {ServerChildEventLoopID}")
        End Function


        ''' <summary>
        ''' 返回用于初始化服务器 ServerBootstrap 的主节点 EventLoopGroup
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetServerParentEventLoopGroup() As IEventLoopGroup
            CheckServerEventLoopGroup()

            Return ServerParentEventLoopGroup
        End Function

        ''' <summary>
        ''' 返回用于初始化服务器 ServerBootstrap 的子节点 EventLoopGroup
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetServerChildEventLoopGroup() As IEventLoopGroup
            CheckServerEventLoopGroup()

            Return ServerChildEventLoopGroup
        End Function

        ''' <summary>
        ''' 返回用于初始化客户端通道的 Bootstrap (TCPClient or UDP) 的 EventLoopGroup
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetClientEventLoopGroup() As IEventLoopGroup
            CheckClientEventLoopGroup()

            Return ClientEventLoopGroup
        End Function

        ''' <summary>
        ''' 释放所有已创建的 EventLoopGroup
        ''' </summary>
        Public Shared Async Function shutdownGracefully() As Task
            'Trace.WriteLine("调用 DotNettyAllocator.shutdownGracefully,准备释放事件循环组")

            Try
                If ServerChildEventLoopGroup IsNot Nothing Then
                    Await ServerChildEventLoopGroup.ShutdownGracefullyAsync()
                    ServerChildEventLoopGroup = Nothing
                End If

                If ServerParentEventLoopGroup IsNot Nothing Then
                    Await ServerParentEventLoopGroup.ShutdownGracefullyAsync()
                    ServerParentEventLoopGroup = Nothing
                End If

                If ClientEventLoopGroup IsNot Nothing Then
                    Await ClientEventLoopGroup.ShutdownGracefullyAsync()
                    ClientEventLoopGroup = Nothing
                End If

                BufferAllocator = Nothing
            Catch ex As Exception
                'Trace.WriteLine("DotNettyAllocator.shutdownGracefully" & System.Environment.NewLine & ex.ToString())
            End Try
            'Trace.WriteLine("调用 DotNettyAllocator.shutdownGracefully,事件循环组释放完毕")

            mIsRelease = True
        End Function

    End Class

End Namespace
