Imports DoNetDrive.Core.Data
Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.OnlineAccess
Imports DoNetDrive.Protocol.Transaction

Namespace USBDrive
    ''' <summary>
    ''' 适用于使用Door8800协议格式的监控消息
    ''' </summary>
    Public Class USBDriveTransaction
        Implements INData

        ''' <summary>
        ''' 事务发生时的连接通道信息
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property Connector As INConnectorDetail

        ''' <summary>
        ''' 命令索引
        ''' </summary>
        ReadOnly Property CmdIndex As Byte

        ''' <summary>
        ''' 发送者的身份
        ''' </summary>
        ReadOnly Property Addr As Byte

        ''' <summary>
        ''' 事务的据结构
        ''' </summary>
        Public ReadOnly EventData As INTransaction

        ''' <summary>
        ''' 初始化一个事务
        ''' </summary>
        ''' <param name="cdtl">连接通道信息</param>
        ''' <param name="num">USB设备的机器号</param>
        ''' <param name="ci">命令索引</param>
        Public Sub New(cdtl As INConnectorDetail, num As Byte, ci As Byte, trn As INTransaction)
            Connector = cdtl
            Addr = num
            CmdIndex = ci
            EventData = trn
        End Sub

        ''' <summary>
        ''' 未使用
        ''' </summary>
        ''' <param name="databuf"></param>
        Public Sub SetBytes(databuf As IByteBuffer) Implements INData.SetBytes
            EventData?.SetBytes(databuf)
        End Sub

        ''' <summary>
        ''' 返回这个消息的数据长度
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDataLen() As Integer Implements INData.GetDataLen
            If EventData Is Nothing Then Return 0
            Return EventData.GetDataLen()
        End Function

        ''' <summary>
        ''' 未使用
        ''' </summary>
        Public Function GetBytes() As IByteBuffer Implements INData.GetBytes
            Return Nothing
        End Function
        ''' <summary>
        ''' 未使用
        ''' </summary>
        Public Function GetBytes(databuf As IByteBuffer) As IByteBuffer Implements INData.GetBytes
            Return Nothing
        End Function
    End Class
End Namespace

