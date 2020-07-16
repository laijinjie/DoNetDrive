Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.OnlineAccess

Namespace Packet

    ''' <summary>
    ''' 抽象的命令包解析类。
    ''' </summary>
    Public MustInherit Class BaseDecompile(Of T As {BasePacket, New})
        Implements INPacketDecompile

        ''' <summary>
        ''' 解码器中缓存数据超时时间
        ''' </summary>
        Protected DecompileTimeout As Integer = 2000

        ''' <summary>
        ''' 解码器活跃时间
        ''' </summary>
        Protected ActivityTime As Date


        ''' <summary>
        ''' 接收数据的步骤
        ''' </summary>
        Public DecompileStep As INDecompileStep(Of T)

        ''' <summary>
        ''' 存储临时数据的缓冲区
        ''' </summary>
        Protected Friend _Buf As IByteBuffer

        ''' <summary>
        ''' 接收到转移码
        ''' </summary>
        Protected Friend _Translate As Boolean

        ''' <summary>
        ''' 命令包
        ''' </summary>
        Protected Friend _Packet As T

        ''' <summary>
        ''' 存储数据包的缓冲区分配器
        ''' </summary>
        Protected Friend _Allocator As IByteBufferAllocator

        ''' <summary>
        ''' 创建一个解析器
        ''' </summary>
        ''' <param name="acr"></param>
        Public Sub New(acr As IByteBufferAllocator)
            _Allocator = acr

            ClearBuf()
        End Sub

        ''' <summary>
        ''' 获取解析器包含的命令包
        ''' </summary>
        ''' <returns></returns>
        Public Function GetPacket() As T
            Return _Packet
        End Function


        ''' <summary>
        ''' 获取用于解析命令的第一个步骤
        ''' </summary>
        ''' <returns></returns>
        Protected MustOverride Function GetFirstDecompileStep() As BaseDecompileStep(Of T)


        ''' <summary>
        ''' 获取一个新的数据缓冲区
        ''' </summary>
        ''' <param name="iLen"></param>
        ''' <returns></returns>
        Public Function GetNewByteBuffer(ByVal iLen As Integer) As IByteBuffer
            Return _Allocator.Buffer(iLen)
        End Function

        ''' <summary>
        ''' 清空缓冲区内容
        ''' </summary>
        Public Sub ClearBuf()
            _Buf?.Clear()
            DecompileStep = GetFirstDecompileStep()
            _Translate = False

            _Packet?.ReleaseDataBuf()
        End Sub

        ''' <summary>
        ''' 将数据解析成特定的数据包，并返回结果
        ''' </summary>
        ''' <param name="bDataBuf">待解析数据缓冲区</param>
        ''' <param name="retPacketList">已解析的包会放在这个列表中</param>
        ''' <returns>解析出完整包则返回True</returns>
        Public Function Decompile(bDataBuf As IByteBuffer, retPacketList As List(Of INPacket)) As Boolean Implements INPacketDecompile.Decompile
            '检查解码器中的数据是否已过期
            If Not ActivityTime = Date.MinValue Then
                Dim iMil = (Now - ActivityTime).TotalMilliseconds
                If (iMil > DecompileTimeout) Then
                    ClearBuf()
                End If
            End If


            ActivityTime = Now

            bDataBuf.MarkReaderIndex()
            'Trace.WriteLine($"解析数据包：{ByteBufferUtil.HexDump(bDataBuf)}")
            Dim bDecompile As Boolean = False
            Dim ret As Boolean = False
            Dim v As Byte
            Dim iBufLen = bDataBuf.ReadableBytes()
            Try
                For i = 1 To iBufLen
                    v = bDataBuf.ReadByte
                    ret = DecompileByte(v)
                    If ret Then
                        bDecompile = True
                        'Trace.WriteLine($"解析数据包成功")
                        retPacketList.Add(_Packet)
                        _Packet = New T
                        ClearBuf()
                    End If
                Next
            Catch ex As Exception
                Trace.WriteLine($"BaseDecompile.Decompile 解析时发生错误：{ex.Message}")
            End Try



            bDataBuf.ResetReaderIndex()
            Return bDecompile
        End Function

        ''' <summary>
        ''' 对字节进行转译
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Protected Overridable Function DecompileByte(value As Byte) As Boolean
            If value = 126 Then '0x7e
                ClearBuf()
                Return False
            ElseIf value = 127 Then '0x7f
                _Translate = True
                Return False
            Else
                If _Translate Then
                    If value = 1 Then
                        value = 126 '0x7f 01=0x7e
                    ElseIf value = 2 Then '0x7f 02=0x7f
                        value = 127
                    Else
                        ClearBuf() '错误转移码
                        Return False
                    End If
                    _Translate = False
                End If
                Return DecompileStep.DecompileStep(Me, value)
            End If
        End Function

        ''' <summary>
        ''' 加入到缓冲
        ''' </summary>
        ''' <param name="value"></param>
        Protected Friend Sub AddBuf(value As Byte)
            _Buf.WriteByte(value)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    _Buf.Release()
                    _Buf = Nothing

                    If _Packet IsNot Nothing Then
                        _Packet.Dispose()
                        _Packet = Nothing
                    End If


                    _Allocator = Nothing
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
#End Region

    End Class
End Namespace

