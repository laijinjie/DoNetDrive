Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Data

Namespace Connector
    ''' <summary>
    ''' 当通讯连接器有事件需要通知时，调用对应的事件函数
    ''' </summary>
    Public Interface INConnectorEvent
        ''' <summary>
        ''' 连接通道发生错误时触发事件
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Event ConnectorErrorEvent(sender As Object, connector As INConnectorDetail)


        ''' <summary>
        ''' 连接通道连接建立成功时发生
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Event ConnectorConnectedEvent(sender As Object, connector As INConnectorDetail)


        ''' <summary>
        ''' 连接通道连接关闭时发生
        ''' </summary>
        ''' <param name="sender">触发事件的调用者</param>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Event ConnectorClosedEvent(sender As Object, connector As INConnectorDetail)


        ''' <summary>
        ''' 事务消息，有些命令发生后会需要异步等待对端传回结果，结果将自动序列化为事物消息，并触发此事件
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        ''' <param name="EventData">事件所包含数据</param>
        Event TransactionMessage(connector As INConnectorDetail, EventData As INData)


        ''' <summary>
        ''' 客户端上线
        ''' </summary>
        ''' <param name="sender">触发事件的连接通道信息</param>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Event ClientOnline(sender As Object, e As ServerEventArgs)


        ''' <summary>
        ''' 客户端离线
        ''' </summary>
        ''' <param name="sender">触发事件的连接通道信息</param>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Event ClientOffline(sender As Object, e As ServerEventArgs)
    End Interface
End Namespace

