﻿Imports DotNetty.Common.Concurrency
Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Packet
Imports System.Threading

Namespace Command
    ''' <summary>
    ''' 命令类的基类，定义了一组基本命令逻辑
    ''' </summary>
    Public MustInherit Class AbstractCommand
        Implements INCommandRuntime
        ''' <summary>
        ''' 命令计数器
        ''' </summary>
        Public Shared CommandObjectTotal As Long

        ''' <summary>
        ''' 命令发生错误时，错误的详情
        ''' </summary>
        Public CommandException As Exception

        ''' <summary>
        ''' 保存用于命令的各种参数
        ''' 包含了通道连接参数，对端身份信息,以及命令参数等数据
        ''' </summary>
        Protected _Parameter As INCommandParameter

        ''' <summary>
        ''' 用于存储命令完毕后的返回值
        ''' </summary>
        Protected _Result As INCommandResult

        ''' <summary>
        ''' 此命令所依附的通讯通道
        ''' </summary>
        Protected _Connector As INConnector

        ''' <summary>
        ''' 命令的当前工作状态
        ''' </summary>
        Protected _Status As INCommandStatus

        ''' <summary>
        ''' 是否正在等待执行
        ''' </summary>
        ''' <returns></returns>
        Property IsWaitExecute As Boolean Implements Command.INCommandRuntime.IsWaitExecute

        ''' <summary>
        ''' 包含一个命令事件对象
        ''' </summary>
        Protected _EventArgs As CommandEventArgs

        ''' <summary>
        ''' 最大进度数<br/>
        ''' 当有发生变化时，会触发 CommandProcessEvent() 事件
        ''' </summary>
        Protected _ProcessMax As Integer

        ''' <summary>
        ''' 当前进度<br/>
        ''' 当有发生变化时，会触发 CommandProcessEvent() 事件
        ''' </summary>
        Protected _ProcessStep As Integer

        ''' <summary>
        ''' 最近一次发送的时间
        ''' </summary>
        Protected _SendDate As Date

        ''' <summary>
        ''' 命令是否需要等待回应
        ''' </summary>
        Protected _IsWaitResponse As Boolean

        ''' <summary>
        ''' 用于存储当前命令发送的包
        ''' </summary>
        Protected _Packet As INPacket

        ''' <summary>
        ''' 解析器，将收到的数据按包的规则拆包，以便命令类进行下一步业务处理
        ''' </summary>
        Protected _Decompile As INPacketDecompile

        ''' <summary>
        ''' 命令重发次数
        ''' </summary>
        Protected _ReSendCount As Integer

        ''' <summary>
        ''' 用于指示此命令是否已释放
        ''' </summary>
        Protected _IsRelease As Boolean


        ''' <summary>
        ''' 初始化两个重要参数，并进行参数检查
        ''' </summary>
        ''' <param name="cd">表示命令详情，包含通道信息，对端信息，超时时间，重发次数</param>
        ''' <param name="par">表示此命令逻辑所需要的命令参数</param>
        Public Sub New(cd As INCommandDetail, par As INCommandParameter)
            If Not CheckCommandParameter(par) Then
                Throw New ArgumentException("par Is Error")
            End If

            If cd Is Nothing Then
                Throw New ArgumentException("cd Is Error")
            End If
            Interlocked.Increment(CommandObjectTotal)
            CommandDetail = cd
            _Parameter = par
            _Result = Nothing
            _Connector = Nothing
            _ProcessMax = 1
            _ProcessStep = 0
            _SendDate = DateTime.Now
            _Packet = Nothing
            _Decompile = Nothing
            _ReSendCount = 0
            _IsRelease = False
            _EventArgs = New CommandEventArgs(Me)
            _Status = GetStatus_Wating()
        End Sub

        ''' <summary>
        ''' 检测命令是否已释放
        ''' </summary>
        ''' <returns></returns>
        Public Function IsRelease() As Boolean Implements INCommandRuntime.IsRelease
            Return _IsRelease
        End Function

        ''' <summary>
        ''' 命令逻辑所需要的命令参数
        ''' </summary>
        ''' <returns></returns>
        Public Property Parameter As INCommandParameter Implements INCommandRuntime.Parameter
            Get
                Return _Parameter
            End Get
            Set(value As INCommandParameter)
                If CheckCommandParameter(value) Then
                    _Parameter = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' 命令详情，包含通道信息，对端信息，超时时间，重发次数
        ''' </summary>
        ''' <returns></returns>
        Public Property CommandDetail As INCommandDetail Implements INCommand.CommandDetail

        ''' <summary>
        ''' 获取检查命令的参数类型
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function CheckCommandParameter(value As INCommandParameter) As Boolean

#Region "状态定义"


        ''' <summary>
        ''' 获取用于表示命令已停止的状态
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetStatus_Stop() As AbstractCommandStatus_Stop Implements INCommandRuntime.GetStatus_Stop
            Return CommandStatus.Stop
        End Function

        ''' <summary>
        ''' 获取用于表示命令已失败的状态
        ''' </summary>
        Public Overridable Function GetStatus_Faulted() As AbstractCommandStatus_Faulted Implements INCommandRuntime.GetStatus_Faulted
            Return CommandStatus.Faulted
        End Function

        ''' <summary>
        ''' 获取用于表示命令正在准备中的状态(还未开始发送数据)
        ''' </summary>
        Public Overridable Function GetStatus_Wating() As AbstractCommandStatus_Waiting Implements INCommandRuntime.GetStatus_Wating
            Return CommandStatus.Waiting
        End Function

        ''' <summary>
        ''' 获取表示命令已完成的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetStatus_Completed() As AbstractCommandStatus_Completed
            Return CommandStatus.Completed
        End Function


        ''' <summary>
        ''' 获取用于处理正在运行中的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetStatus_Runing() As AbstractCommandStatus_Runing Implements INCommandRuntime.GetStatus_Runing
            Return CommandStatus.Runing
        End Function

        ''' <summary>
        ''' 获取用于处理已超时的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetStatus_Timeout() As AbstractCommandStatus_Timeout
            Return CommandStatus.Timeout
        End Function

        ''' <summary>
        ''' 获取用于处理等待响应的状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetStatus_WaitResponse() As AbstractCommandStatus_WaitResponse
            Return CommandStatus.WaitResponse
        End Function

#End Region

#Region "读取参数"
        Public Function getResult() As INCommandResult Implements INCommandRuntime.getResult
            Return _Result
        End Function

        ''' <summary>
        ''' 获取一个用户触发事件时发送到事件中的参数
        ''' </summary>
        ''' <returns></returns>
        Public Function GetEventArgs() As CommandEventArgs Implements INCommandRuntime.GetEventArgs
            Return _EventArgs
        End Function

        ''' <summary>
        ''' 获取命令状态
        ''' </summary>
        Public Function GetStatus() As INCommandStatus Implements INCommandRuntime.GetStatus
            Return _Status
        End Function

        ''' <summary>
        ''' 总步骤数
        ''' </summary>
        Public Function getProcessMax() As Integer Implements INCommandRuntime.getProcessMax
            Return _ProcessMax
        End Function

        ''' <summary>
        ''' 当前指令进度
        ''' </summary>
        Public Function getProcessStep() As Integer Implements INCommandRuntime.getProcessStep
            Return _ProcessStep
        End Function
#End Region

        ''' <summary>
        ''' 命令加入到连接器，和连接器绑定关系
        ''' </summary>
        Public Sub SetConnector(connect As INConnector) Implements INCommandRuntime.SetConnector
            _Connector = connect
        End Sub

        ''' <summary>
        ''' 设置命令的状态,变更当前状态
        ''' </summary>
        Public Sub SetStatus(cmdstatus As INCommandStatus) Implements INCommandRuntime.SetStatus
            If _IsRelease Then Return
            _Status = cmdstatus
        End Sub

        ''' <summary>
        ''' 从等待执行，变更为开始执行，此时需要立刻编译此命令，生成 Packet ，方便发送时调用
        ''' </summary>
        Protected MustOverride Sub CreatePacket()

        ''' <summary>
        ''' 由连接通道推送来的接收缓冲区中的 Bytebuf
        ''' </summary>
        Public Sub PushReadByteBuf(buf As IByteBuffer) Implements INCommandRuntime.PushReadByteBuf
            If TypeOf _Status IsNot AbstractCommandStatus_Runing Then Return

            If _IsWaitResponse Then
                If _Decompile IsNot Nothing Then
                    Dim decompileRet As Boolean = True
                    Dim pList As List(Of INPacket) = New List(Of INPacket)(10)
                    decompileRet = _Decompile.Decompile(buf, pList)
                    If decompileRet Then

                        For Each p In pList
                            If _Packet IsNot Nothing Then
                                CommandNext(p)
                            End If
                            p.Dispose()
                        Next
                    End If

                End If
            End If
        End Sub

        ''' <summary>
        ''' 检查并进行命令的下一部分
        ''' </summary>
        ''' <param name="readPacket">收到的数据包</param>
        Protected MustOverride Sub CommandNext(readPacket As INPacket)

        ''' <summary>
        ''' 检查此命令是否超时
        ''' </summary>
        ''' <returns></returns>
        Protected Friend Function CheckTimeout() As Boolean
            If TypeOf _Status Is AbstractCommandStatus_Timeout Then Return True
            If TypeOf _Status IsNot AbstractCommandStatus_Runing Then Return False

            Dim iTimeout = 1000
            Dim iMaxReSendCount = 3

            Dim detail As INCommandDetail = CommandDetail
            If detail IsNot Nothing Then
                iTimeout = detail.Timeout
                iMaxReSendCount = detail.RestartCount
            End If

            Dim lElapse = (DateTime.Now - _SendDate).TotalMilliseconds
            Dim bIsTimeout = (lElapse > iTimeout)

            If (_ReSendCount > iMaxReSendCount) And iMaxReSendCount > 0 Then
                bIsTimeout = True
            End If
            If (bIsTimeout) Then

                If (_ReSendCount <= iMaxReSendCount) Then
                    'Trace.WriteLine(detail.ToString() & "," & Me.GetType().Name & " 超时重发")
                    '超时了，但是还可以重发，则重新发送
                    CommandReSend()
                    '_ReSendCount += 1
                    SetRuningStatus()
                    Return False
                Else
                    'Trace.WriteLine($"{detail.ToString()} ,{Me.GetType().Name }  Step：{getProcessStep()}  超时失败")
                    '超时了，重发次数已达到则命令触发超时事件
                    CommandOver()
                    _Status = GetStatus_Timeout()
                    fireCommandTimeout()
                    Return True
                End If
            Else
                Return False
            End If
        End Function



        ''' <summary>
        ''' 准备重新发送命令，可能子类需要清空一些标志或缓冲区，则再此函数中执行
        ''' </summary>
        Protected MustOverride Sub CommandReSend()

        ''' <summary>
        ''' 命令结束的时候调用
        ''' </summary>
        Public Sub CommandOver() Implements INCommandRuntime.CommandOver
            If CommandDetail IsNot Nothing Then CommandDetail.EndTime = DateTime.Now
            _ProcessStep = _ProcessMax
            _Packet?.Dispose()
            _Packet = Nothing

            _Decompile?.Dispose()
            _Decompile = Nothing
            Release0()
        End Sub


        ''' <summary>
        ''' 解除绑定
        ''' </summary>
        Public Sub RemoveBinding() Implements INCommandRuntime.RemoveBinding
            If _IsRelease = True Then
                Return
            End If

            _IsRelease = True
            _Connector = Nothing
            _Packet?.Dispose()
            _Packet = Nothing

            _Decompile?.Dispose()
            _Decompile = Nothing
            Release0()
        End Sub

        ''' <summary>
        ''' 表示命令已完结，改变状态，并立刻发送命令完毕的事件通知
        ''' </summary>
        Protected Sub CommandCompleted()
            CommandOver()
            _Status = GetStatus_Completed()
            _Connector.FireCommandCompleteEvent(_EventArgs)
        End Sub

        ''' <summary>
        ''' 命令发生错误，终止执行
        ''' </summary>
        Protected Sub CommandError()
            CommandOver()
            _Status = GetStatus_Faulted()
            _Connector.FireCommandErrorEvent(_EventArgs)
        End Sub


        ''' <summary>
        ''' 命令准备就绪，等待下次发送
        ''' </summary>
        Protected Overridable Sub CommandReady()
            _ReSendCount = 0
            _Status = GetStatus_Runing()
            If IsWaitExecute Then Return
            SendPacket()
            'SetRuningStatus()
        End Sub

        ''' <summary>
        ''' 设定状态为正在执行并立刻加入到线程任务队列中，为发送数据包做准备
        ''' </summary>
        Protected Sub SetRuningStatus()
            _Status = GetStatus_Runing()
            If IsWaitExecute Then Return
            SendPacket()
        End Sub


        ''' <summary>
        ''' 命令继续等待
        ''' </summary>
        Protected Sub CommandWaitResponse()
            _Status = GetStatus_WaitResponse()
            _SendDate = Date.Now
        End Sub




        ''' <summary>
        ''' 开始执行当前状态的运行逻辑
        ''' </summary>
        Public Sub Run() Implements IRunnable.Run
            If _IsRelease Then Return
            If IsWaitExecute Then
                _Connector?.GetEventLoop()?.Execute(Me)
                Return
            End If

            IsWaitExecute = True


            _Connector?.UpdateActivityTime() '命令执行期间不应该超时
            Try
                If _Status IsNot Nothing Then
                    _Status.CheckStatus(Me)
                End If
                If _Status.IsNONE() Then

                    CreatePacket()
                    CommandDetail.BeginTime = DateTime.Now
                    _Status = GetStatus_Runing() '状态变化
                    IsWaitExecute = False
                    Run()
                    Return
                End If

                If Not _Status.IsCompleted Then
                    If _IsRelease Then Return
                    _Connector?.GetEventLoop()?.Execute(Me)
                End If

            Catch ex As Exception
                CommandException = ex
                CommandError()
            End Try
            IsWaitExecute = False

        End Sub


        ''' <summary>
        ''' 发送数据包
        ''' </summary>
        Protected Friend Sub SendPacket()
            If _IsRelease Then Return
            If _Connector Is Nothing Then Return
            If TypeOf _Status IsNot AbstractCommandStatus_Runing Then Return

            If _Packet Is Nothing Then
                _Status = CommandStatus.Faulted
                fireFireCommandErrorEvent()
                Return
            End If


            IsWaitExecute = True
            Dim buf = _Packet.GetPacketData(_Connector.GetByteBufAllocator())
            'Trace.WriteLine($"{System.Threading.Thread.CurrentThread.ManagedThreadId} {_Connector.GetKey()} 发送数据:{ByteBufferUtil.HexDump(buf)}")
            Dim tWrite = _Connector.WriteByteBuf(buf)
            If tWrite Is Nothing Then
                IsWaitExecute = False
                Return
            End If
            _ReSendCount += 1
            '将状态变更为等待响应
            CommandWaitResponse()


            tWrite.ContinueWith(Sub()

                                    If tWrite.IsCanceled Or tWrite.IsFaulted Then
                                        '等待下一次运行
                                        If Not CheckTimeout() Then
                                            _Status = GetStatus_Runing()
                                        End If
                                    Else
                                        '发送完毕，切发送成功
                                        If _IsWaitResponse = False Then
                                            _ReSendCount = 0
                                            CommandCompleted()
                                            Return
                                        End If
                                    End If
                                    fireCommandProcessEvent()
                                End Sub)
            buf = Nothing
            IsWaitExecute = False
        End Sub

        ''' <summary>
        ''' 复制一个当前命令的浅表副本
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function

#Region "事件驱动"
        ''' <summary>
        ''' 命令发生进程发生变化时调用，用于触发产生命令进度变更事件
        ''' </summary>
        Protected Friend Overridable Sub fireCommandProcessEvent()
            If _Connector IsNot Nothing Then
                _Connector.FireCommandProcessEvent(_EventArgs)
            End If
        End Sub

        ''' <summary>
        ''' 完结此命令，并产生命令超时事件
        ''' </summary>
        Protected Friend Overridable Sub fireCommandTimeout()
            CommandOver()
            If _Connector IsNot Nothing Then
                _Connector.FireCommandTimeout(_EventArgs)
            End If
        End Sub

        ''' <summary>
        ''' 完结此命令，并产生身份校验错误事件
        ''' </summary>
        Protected Friend Overridable Sub fireAuthenticationErrorEvent()
            CommandOver()
            If _Connector IsNot Nothing Then
                _Connector.FireAuthenticationErrorEvent(_EventArgs)
            End If
        End Sub

        ''' <summary>
        ''' 完结此命令，并产生命令错误事件
        ''' </summary>
        Protected Friend Overridable Sub fireFireCommandErrorEvent()
            CommandOver()
            If _Connector IsNot Nothing Then
                _Connector.FireCommandErrorEvent(_EventArgs)
            End If
        End Sub
#End Region


        ''' <summary>
        ''' 获取一个指定大小的Buf
        ''' </summary>
        ''' <param name="iSize">大小</param>
        ''' <returns></returns>
        Protected Overridable Function GetNewCmdDataBuf(iSize As Integer) As IByteBuffer
            Dim acl = _Connector.GetByteBufAllocator()
            Dim buf = acl.Buffer(iSize)
            Return buf
        End Function

        ''' <summary>
        ''' 产生一个错误
        ''' </summary>
        ''' <param name="sText">错误描述</param>
        Protected Overridable Sub VerifyError(ByVal sText As String)
            Throw New ArgumentException(sText & " is Error")
        End Sub

#Region "IDisposable Support"


        Private disposedValue As Boolean ' 要检测冗余调用

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected MustOverride Sub Release0()

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then

                    ' TODO: 释放托管状态(托管对象)。
                    _Parameter?.Dispose()
                    _Parameter = Nothing

                    _Result?.Dispose()
                    _Result = Nothing

                    _Status = Nothing

                    RemoveBinding()
                End If

                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If
            disposedValue = True
        End Sub

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

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Interlocked.Decrement(CommandObjectTotal)
        End Sub

#End Region

    End Class

End Namespace
