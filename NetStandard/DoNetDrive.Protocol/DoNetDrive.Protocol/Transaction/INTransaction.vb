Imports DoNetDrive.Core.Data

Namespace Transaction
    ''' <summary>
    ''' 表示一个事务
    ''' </summary>
    Public Interface INTransaction
        Inherits INData

        ''' <summary>
        ''' 事务序列号
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property SerialNumber As Integer

        ''' <summary>
        ''' 事务时间
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property TransactionDate As Date

        ''' <summary>
        ''' 事务类型
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property TransactionType As Integer

        ''' <summary>
        ''' 事务代码
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property TransactionCode As Integer

        ''' <summary>
        ''' 记录是否为空记录
        ''' </summary>
        ''' <returns></returns>
        Function IsNull() As Boolean

    End Interface

End Namespace
