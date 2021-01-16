Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers

Namespace Packet
    ''' <summary>
    ''' 抽象的数据包，仅包含必要内容
    ''' </summary>
    Public MustInherit Class BasePacket
        Implements INPacket

        ''' <summary>
        ''' 数据长度
        ''' </summary>
        Public DataLen As Integer

        ''' <summary>
        ''' 命令数据
        ''' </summary>
        Public CmdData As IByteBuffer

        ''' <summary>
        ''' 校验和
        ''' </summary>
        Public Check As Integer

        ''' <summary>
        ''' 验证参数错误时，抛出错误
        ''' </summary>
        ''' <param name="sText"></param>
        Protected Overridable Sub VerifyError(ByVal sText As String)
            Throw New ArgumentException(sText & " is Error")
        End Sub

        ''' <summary>
        ''' 设置包裹数据
        ''' </summary>
        ''' <param name="dl">数据长度</param>
        ''' <param name="cd">命令数据</param>
        Public Sub SetPacketCmdData(dl As Integer, cd As IByteBuffer)
            If dl > 0 Then
                If cd Is Nothing Then
                    VerifyError("CmdData")
                    Return
                End If
                If dl > cd.ReadableBytes Then
                    VerifyError("CmdData")
                    Return
                End If
            End If
            DataLen = dl

            Try
                If CmdData IsNot Nothing Then
                    If CmdData IsNot cd Then
                        CmdData.Release()
                    End If
                    CmdData = Nothing
                End If
            Catch ex As Exception

            End Try

            CmdData = cd
            Check = 0
        End Sub

        ''' <summary>
        ''' 释放数据包中的缓冲区
        ''' </summary>
        Public Sub ReleaseDataBuf() Implements INPacket.ReleaseDataBuf
            Try
                If (CmdData IsNot Nothing) Then
                    If (CmdData.ReferenceCount <> 1) Then
                        Debug.Print("内存泄漏")
                    Else

                        CmdData.Release()
                    End If
                End If
                CmdData = Nothing
                DataLen = 0
                Check = 0

                Release()
            Catch ex As Exception

            End Try
        End Sub


        ''' <summary>
        ''' 获取数据包的打包后的ByteBuf，用于发送数据 -- 子类必须实现
        ''' </summary>
        ''' <param name="Allocator">用于分配ByteBuf的分配器</param>
        ''' <returns></returns>
        Public MustOverride Function GetPacketData(Allocator As IByteBufferAllocator) As IByteBuffer Implements INPacket.GetPacketData


#Region "随机数"
        ''' <summary>
        ''' 随机数生成器
        ''' </summary>
        Private Shared CodeRand As Random = New System.Random()

        ''' <summary>
        ''' 获取一个随机的信息代码
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetRandomCode(ByRef iCodeMin As UInt32, ByRef iCodeMax As UInt32) As UInt32
            Dim rnd = CodeRand.NextDouble

            Return iCodeMin + (rnd * (iCodeMax - iCodeMin + 1))
        End Function
#End Region


        ''' <summary>
        ''' 生成最终指令和校验和，并对命令进行转义码转义
        ''' </summary>
        ''' <param name="Allocator">缓冲区分配器</param>
        ''' <param name="buf">已存在原始指令的缓冲区</param>
        ''' <returns></returns>
        Protected Overridable Function CreatePacket(Allocator As IByteBufferAllocator, buf As IByteBuffer) As IByteBuffer
            buf.MarkReaderIndex()
            Dim iSize As Integer
            Dim i As Integer, iCount = buf.ReadableBytes
            Dim uCheck As UInt64 = 0

            '缓冲区扫描，检查扩充的容量
            Do While (iCount > 0)
                Dim b As Byte = buf.ReadByte()
                iSize += 1
                iCount -= 1
                uCheck += b '累加和校验
                If (b = 126) Or (b = 127) Then
                    iSize += 1
                End If
            Loop

            Check = uCheck And 255 '和校验取低字节
            iSize += 1
            If (uCheck = 126) Or (uCheck = 127) Then
                iSize += 1
            End If
            buf.ResetReaderIndex()

            iCount = buf.ReadableBytes
            iSize += 2 '头和尾

            '进行转移码转换
            Dim buf2 = Allocator.Buffer(iSize)
            buf2.WriteByte(126) '&H7e
            For i = 1 To iCount
                WriteByteToBuf(buf.ReadByte(), buf2)
            Next
            WriteByteToBuf(Check, buf2)
            buf2.WriteByte(126) '&H7e

            Return buf2
        End Function

        ''' <summary>
        ''' 将一个字节写入到缓冲区，并进行转义码转换
        ''' </summary>
        ''' <param name="bValue"></param>
        ''' <param name="buf"></param>
        Protected Sub WriteByteToBuf(bValue As Byte, buf As IByteBuffer)
            If bValue = 126 Then '&H7e
                buf.WriteByte(127)
                buf.WriteByte(1)
            ElseIf bValue = 127 Then '&H7f
                buf.WriteByte(127)
                buf.WriteByte(2)
            Else
                buf.WriteByte(bValue)
            End If
        End Sub



        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected MustOverride Sub Release()

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    ReleaseDataBuf()
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

