Imports DotNetty.Buffers
Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.Command


Namespace Connector.SerialPort
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

        ''' <summary>
        ''' 保存和此通道相关联的描述
        ''' </summary>
        Private _Detail As SerialPortDetail

        ''' <summary>
        ''' 此通道所在的事件循环
        ''' </summary>
        Private _EventLoop As IEventLoop

        ''' <summary>
        ''' 根据指定的串口参数，创建连接通道
        ''' </summary>
        ''' <param name="dtl"></param>
        Public Sub New(ByVal dtl As SerialPortDetail)
            _Detail = dtl.Clone
            _EventLoop = DotNettyAllocator.GetClientEventLoopGroup().GetNext()
        End Sub

        ''' <summary>
        ''' 返回描述此通道的连接对象描述符
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorDetail() As INConnectorDetail
            Return _Detail
        End Function

        ''' <summary>
        ''' 返回此通道的初始化状态
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function GetInitializationStatus() As INConnectorStatus
            Return SerialPortStatus_Free.Free
        End Function

        ''' <summary>
        ''' 返回此通道的类路径
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetConnectorType() As String
            Return ConnectorType.SerialPort
        End Function



        ''' <summary>
        ''' 获取连接通道支持的bytebuf分配器
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetByteBufAllocator() As IByteBufferAllocator
            Return DotNettyAllocator.GetBufferAllocator()
        End Function


        ''' <summary>
        ''' 获取此通道所依附的事件循环通道
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetEventLoop() As IEventLoop
            Return _EventLoop
        End Function

        ''' <summary>
        ''' 获取本地绑定地址
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function LocalAddress() As IPDetail
            Return New IPDetail("SerialPort", _Detail.Port)
        End Function

#Region "打开串口"
        ''' <summary>
        ''' 打开串口
        ''' </summary>
        Public Sub Open()
            If _isRelease Then Return

            If mSerialPort Is Nothing Then
                mSerialPort = New IO.Ports.SerialPort
            End If

            If mSerialPort.IsOpen Then
                mSerialPort.Close()
            End If

            If Not mSerialPort.PortName = ("COM" & _Detail.Port.ToString) Then
                mSerialPort.PortName = ("COM" & _Detail.Port.ToString)
            End If

            '设置波特率
            If Not _CommandList.IsEmpty() Then
                Dim cmd As INCommand = Nothing
                If _CommandList.TryPeek(cmd) Then
                    Dim tmpDtl As SerialPortDetail = cmd.CommandDetail.Connector
                    mSerialPort.BaudRate = tmpDtl.Baudrate
                Else
                    mSerialPort.BaudRate = _Detail.Baudrate
                End If
            Else
                If _Detail.Baudrate = 0 Then
                    mSerialPort.BaudRate = 19200
                Else
                    mSerialPort.BaudRate = _Detail.Baudrate
                End If

            End If

            mSerialPort.WriteTimeout = 500
            mSerialPort.ReadTimeout = 500
            '保存波特率
            _Detail.Baudrate = mSerialPort.BaudRate

            Try
                '打开串口
                mSerialPort.Open()
                _Status = SerialPortStatus_Opened.Opened
                _IsActivity = True
                'Trace.WriteLine($"串口已打开：COM{_Detail.Port}")
                FireConnectorConnectedEvent(GetConnectorDetail())
                _EventLoop.Execute(Me) '加入事件循环
            Catch ex As Exception
                Trace.WriteLine($"打开串口：COM{_Detail.Port} 时发生错误，{ex.Message}")
                '串口无效
                FireConnectorErrorEvent(GetConnectorDetail())
                CloseConnector()
                _Status = ConnectorStatus_Invalid.Invalid
                _IsActivity = False
                SetInvalid()
            End Try
        End Sub
#End Region

#Region "关闭通道"
        ''' <summary>
        ''' 关闭串口
        ''' </summary>
        Public Overrides Sub CloseConnector()
            If _isRelease Then Return

            If mSerialPort Is Nothing Then
                Return
            End If

            If mSerialPort.IsOpen Then
                'Trace.WriteLine($"串口已关闭--CloseConnector：COM{_Detail.Port}")
                mSerialPort.Close()
                '关闭串口
                FireConnectorClosedEvent(GetConnectorDetail())
            End If
            _IsActivity = False
            _Status = GetInitializationStatus()
        End Sub
#End Region

        ''' <summary>
        ''' 检查通道是否已开启
        ''' </summary>
        ''' <returns></returns>
        Private Function CheckConnector() As Boolean
            If mSerialPort Is Nothing Then
                'Trace.WriteLine($"串口已关闭--CheckConnector 1：{_Detail.Port}")
                _Status = GetInitializationStatus()
                Return False
            End If

            If Not mSerialPort.IsOpen Then
                'Trace.WriteLine($"串口已关闭--CheckConnector 2：{_Detail.Port}")
                _Status = GetInitializationStatus()
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' 读通道中的数据
        ''' </summary>
        Public Function ReadConnector() As Boolean
            If Not CheckConnector() Then

                Return False
            End If

            '检测接收缓冲区
            Dim iReadLen As Integer = mSerialPort.BytesToRead

            If iReadLen = 0 Then
                Return True
            End If

            '读取接收缓冲区
            Dim buf = GetByteBufAllocator().Buffer(iReadLen)

            mSerialPort.Read(buf.Array, buf.ArrayOffset, iReadLen)
            buf.SetWriterIndex(iReadLen)
            'Trace.WriteLine($"{Date.Now:HH:mm:ss.ffff} 接收长度：{iReadLen} 数据：{ByteBufferUtil.HexDump(buf)}")
            '进行后续处理
            ReadByteBuffer(buf)
            buf.Release()
            buf = Nothing
            Return True
        End Function

        ''' <summary>
        ''' 发送数据
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Overrides Function WriteByteBuf(buf As IByteBuffer) As Task
            If _isRelease Then Return Nothing
            If Not CheckConnector() Then
                Return Nothing
            End If

            mSerialPort.DiscardOutBuffer()
            UpdateActivityTime()


            Dim tsk As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)

            Try
                'Trace.WriteLine($"串口：{_Detail.Port} {Date.Now:HH:mm:ss.ffff} 准备写数据")
                mSerialPort.Write(buf.Array, buf.ArrayOffset, buf.ReadableBytes)
                'Trace.WriteLine($"串口：{_Detail.Port} {Date.Now:HH:mm:ss.ffff} 发送长度：{buf.ReadableBytes} 数据：{ByteBufferUtil.HexDump(buf)}")
                tsk.TrySetResult(True)
                mConnectorWriteError = 0
            Catch ex As Exception
                mConnectorWriteError += 1

                'Trace.WriteLine($"串口：{_Detail.Port} {Date.Now:HH:mm:ss.ffff} 写串口出现错误：{ex.Message}")
                CloseConnector()
                tsk.TrySetException(ex)

                If mConnectorWriteError > 3 Then
                    Trace.WriteLine($"写：COM{_Detail.Port} 时发生错误，{ex.Message}")
                    '串口无效
                    FireConnectorErrorEvent(GetConnectorDetail())
                    CloseConnector()
                    _Status = ConnectorStatus_Invalid.Invalid
                    _IsActivity = False
                    SetInvalid()
                End If
            End Try

            DisposeResponse(buf)

            Return tsk.Task
        End Function



        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            _Status = ConnectorStatus_Invalid.Invalid

            CloseConnector()

        End Sub
    End Class
End Namespace

