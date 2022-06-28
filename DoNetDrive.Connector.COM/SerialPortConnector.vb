Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core

''' <summary>
''' 表示一个串口通道，使用RS232、RS485方式或USB转串口方式和硬件通讯
''' </summary>
Public Class SerialPortConnector
    Inherits AbstractConnector

    ''' <summary>
    ''' 串口的写操作错误次数
    ''' </summary>
    Private mConnectorWriteError As Integer

    ''' <summary>
    ''' 串口操作类
    ''' </summary>
    Private mSerialPort As IO.Ports.SerialPort

    Protected Property _LocalAddress As IPDetail

    ''' <summary>
    ''' 根据指定的串口参数，创建连接通道
    ''' </summary>
    ''' <param name="dtl"></param>
    Public Sub New(ByVal dtl As SerialPortDetail)
        MyBase._ConnectorDetail = dtl
        _LocalAddress = New IPDetail("COM", dtl.Port)
    End Sub

    Protected Overrides Function GetInitializationStatus() As INConnectorStatus
        Return SerialPortStatus.Closed
    End Function

    Public Overrides Function RemoteAddress() As IPDetail
        Return Nothing
    End Function

    Public Overrides Function LocalAddress() As IPDetail
        Return _LocalAddress
    End Function

    ''' <summary>
    ''' 返回此通道的类路径
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetConnectorType() As String
        Return ConnectorType.SerialPort
    End Function

#Region "接收数据"

    ''' <summary>
    ''' 读通道中的数据
    ''' </summary>
    Public Function ReadConnector() As Boolean
        '检测接收缓冲区
        Dim iReadLen As Integer = mSerialPort.BytesToRead

        If iReadLen = 0 Then
            Return True
        End If

        '读取接收缓冲区
        Dim buf = GetByteBufAllocator().Buffer(iReadLen)

        mSerialPort.Read(buf.Array, buf.ArrayOffset, iReadLen)
        buf.SetWriterIndex(iReadLen)

        '进行后续处理
        ReadByteBuffer(buf)
        buf.Release()
        buf = Nothing
        Return True
    End Function
#End Region

#Region "发送数据"


    ''' <summary>
    ''' 发送数据
    ''' </summary>
    ''' <param name="buf"></param>
    ''' <returns></returns>
    Protected Overrides Async Function WriteByteBuf0(buf As IByteBuffer) As Task
        If _isRelease Then Return
        If Not CheckConnector() Then
            Return
        End If


        If (CommandSendIntervalTimeMS > 0) Then
            Dim iDelayTime As Integer

            If LastReadDataTime > LastSendDataTime Then
                iDelayTime = (DateTime.Now - LastReadDataTime).Milliseconds
            Else
                iDelayTime = (DateTime.Now - LastSendDataTime).Milliseconds
            End If

            If (iDelayTime < CommandSendIntervalTimeMS) Then
                iDelayTime = CommandSendIntervalTimeMS - iDelayTime
                Await Task.Delay(iDelayTime)
            End If

        End If

        mSerialPort.DiscardOutBuffer()
        UpdateActivityTime()

        Try
            mSerialPort.Write(buf.Array, buf.ArrayOffset, buf.ReadableBytes)
            mConnectorWriteError = 0
        Catch ex As Exception
            mConnectorWriteError += 1
            _ConnectorDetail.SetError(ex)
        End Try

        If mConnectorWriteError > 3 Then
            _IsActivity = False
            '串口无效
            FireConnectorErrorEvent(GetConnectorDetail())

            Await CloseAsync().ConfigureAwait(False)
        End If
    End Function
#End Region

#Region "打开串口"
    ''' <summary>
    ''' 打开串口
    ''' </summary>
    Public Overrides Async Function ConnectAsync() As Task
        If Me.CheckIsInvalid() Then Return
        If Me.IsActivity Then Return
        _Status = SerialPortStatus.Connecting
        _IsActivity = False
        If mSerialPort Is Nothing Then
            mSerialPort = New IO.Ports.SerialPort
        End If

        If mSerialPort.IsOpen Then
            mSerialPort.Close()
        End If
        Dim dtl As SerialPortDetail = _ConnectorDetail
        Dim sCOM = $"COM{dtl.Port}"
        If Not mSerialPort.PortName = sCOM Then
            mSerialPort.PortName = sCOM
        End If

        '设置波特率
        Dim lBaudRate As Integer = 19200
        If _ActivityCommand Is Nothing Then
            '一个新的指令
            If _CommandList.TryPeek(_ActivityCommand) Then
                _ActivityCommand.SetStatus(_ActivityCommand.GetStatus_Wating())
                dtl = _ActivityCommand.CommandDetail.Connector

                fireCommandProcessEvent(_ActivityCommand.GetEventArgs())
            End If
            _ActivityCommand = Nothing
        End If
        lBaudRate = dtl.Baudrate
        If lBaudRate = 0 Then lBaudRate = 19200
        mSerialPort.BaudRate = lBaudRate

        mSerialPort.WriteTimeout = 500
        mSerialPort.ReadTimeout = 500
        '保存波特率
        dtl = _ConnectorDetail
        dtl.Baudrate = mSerialPort.BaudRate
        Dim openex As Exception = Nothing
        Try
            '打开串口
            mSerialPort.Open()
            _Status = SerialPortStatus.Connected
            _IsActivity = True

            FireConnectorConnectedEvent(GetConnectorDetail())
            CheckCommandList()

        Catch ex As Exception
            '串口无效
            _ConnectorDetail.SetError(ex)
            openex = ex
        End Try
        If openex IsNot Nothing Then
            Await Task.FromException(openex)
        End If
    End Function

    ''' <summary>
    ''' 异步处理成功的后续操作
    ''' </summary>
    Protected Friend Sub ConnectingNext(connTask As Task)
        If Me.CheckIsInvalid() Then Return
        If Not Me._Status.Status = "Connecting" Then Return
        If connTask.IsCompleted = False Then Return
        If connTask.IsFaulted Or connTask.IsCanceled Then
            '有错误，或已取消
            FireConnectorErrorEvent(GetConnectorDetail())
            mSerialPort = Nothing
            _Status = ConnectorStatus.Invalid
            _IsActivity = False
            SetInvalid()
        End If
    End Sub



    ''' <summary>
    ''' 检查通道是否已开启
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckConnector() As Boolean
        If Not Me.IsActivity Then Return False

        If mSerialPort Is Nothing Then
            _Status = SerialPortStatus.Closed
            Return False
        End If

        If Not mSerialPort.IsOpen Then
            CloseAsync()
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 连接状态检查，当连接成功时，检查连接状态
    ''' </summary>
    Protected Friend Overridable Sub CheckConnectedStatus()
        If Not CheckIsInvalid() Then
            If CheckConnector() Then
                ReadConnector()

                If _CommandList.Count > 0 Then
                    CheckCommandList()
                End If
            End If
        End If
    End Sub
#End Region

#Region "关闭通道"
    ''' <summary>
    ''' 关闭串口
    ''' </summary>
    Public Overrides Async Function CloseAsync() As Task
        If Me._isRelease Then Return

        _IsActivity = False
        If mSerialPort Is Nothing Then
            Return
        End If

        If mSerialPort.IsOpen Then
            'Trace.WriteLine($"串口已关闭--CloseConnector：COM{_Detail.Port}")
            mSerialPort.Close()
            '关闭串口
        End If
        mSerialPort = Nothing



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
        Return

    End Sub


End Class


