Imports DotNetty.Buffers
Imports DoNetDrive.Core.Data

Namespace Transaction


    ''' <summary>
    ''' 表示一个事务
    ''' </summary>
    Public MustInherit Class AbstractTransaction
        Implements INTransaction

        ''' <summary>
        ''' 事务序列号
        ''' </summary>
        Protected _SerialNumber As Integer

        ''' <summary>
        ''' 事务时间
        ''' </summary>
        Protected _TransactionDate As Date
        ''' <summary>
        ''' 事务类型
        ''' </summary>
        Protected _TransactionType As Integer

        ''' <summary>
        ''' 事务代码
        ''' </summary>
        Protected _TransactionCode As Integer

        ''' <summary>
        ''' 记录是否为空记录
        ''' </summary>
        Protected _IsNull As Boolean

        ''' <summary>
        ''' 事务序列号
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property SerialNumber As Integer Implements INTransaction.SerialNumber
            Get
                Return _SerialNumber
            End Get
        End Property

        ''' <summary>
        ''' 设置记录的事务序号--一般是逻辑存储序号
        ''' </summary>
        ''' <param name="num">事务序号</param>
        Public Sub SetSerialNumber(num As Integer)
            _SerialNumber = num
        End Sub


        ''' <summary>
        ''' 事务时间
        ''' </summary>
        Public Overridable ReadOnly Property TransactionDate As Date Implements INTransaction.TransactionDate
            Get
                Return _TransactionDate
            End Get
        End Property

        ''' <summary>
        ''' 事务类型
        ''' </summary>
        Public Overridable ReadOnly Property TransactionType As Integer Implements INTransaction.TransactionType
            Get
                Return _TransactionType
            End Get
        End Property

        ''' <summary>
        ''' 事务代码
        ''' </summary>
        Public Overridable ReadOnly Property TransactionCode As Integer Implements INTransaction.TransactionCode
            Get
                Return _TransactionCode
            End Get
        End Property

        ''' <summary>
        ''' 记录是否为空记录
        ''' </summary>
        Public Overridable Function IsNull() As Boolean Implements INTransaction.IsNull
            Return _IsNull
        End Function

        ''' <summary>
        ''' 将一个缓冲区 ByteBuf 设置到数据结构中
        ''' </summary>
        Public MustOverride Sub SetBytes(databuf As IByteBuffer) Implements INData.SetBytes

        ''' <summary>
        ''' 获取数据的字节长度
        ''' </summary>
        Public MustOverride Function GetDataLen() As Integer Implements INData.GetDataLen

        ''' <summary>
        ''' 获取一个 ByteBuf 此 缓冲中包含了此数据结构的所有数据  --- 此功能未实现
        ''' </summary>
        Public Overridable Function GetBytes() As IByteBuffer Implements INData.GetBytes
            Return Nothing
        End Function

        ''' <summary>
        ''' 将数据序列化到指定的 ByteBuf 中  --- 此功能未实现
        ''' </summary>
        ''' <param name="databuf">具有足够缓冲空间的bytebuf</param>
        ''' <returns>返回传入的bytebuf</returns>
        Public Overridable Function GetBytes(databuf As IByteBuffer) As IByteBuffer Implements INData.GetBytes
            Return databuf
        End Function

        ''' <summary>
        ''' 检查记录是否为空
        ''' </summary>
        ''' <param name="databuf">记录缓冲区</param>
        ''' <param name="iCheckLen">需要检查的数据长度</param>
        ''' <returns></returns>
        Protected Overridable Function CheckNull(databuf As IByteBuffer, iCheckLen As Integer) As Boolean
            Dim iIndex = databuf.ReaderIndex
            Dim bNull = True
            For index = 1 To iCheckLen
                If (databuf.ReadByte() <> 255) Then
                    bNull = False
                    Exit For
                End If
            Next

            databuf.SetReaderIndex(iIndex)
            Return bNull
        End Function


        ''' <summary>
        ''' 将记录所有字节从缓冲区中读出，但不处理
        ''' </summary>
        Protected Overridable Sub ReadNullRecord(databuf As IByteBuffer)
            Dim iRecordLen = GetDataLen()
            For index = 1 To iRecordLen
                databuf.ReadByte()
            Next
        End Sub
    End Class
End Namespace
