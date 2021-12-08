Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Data

Namespace Connector
    ''' <summary>
    ''' 触发事件 ,用于激活 INConnectorEvent 接口定义的事件
    ''' </summary>
    Public Interface INFireConnectorEvent

        ''' <summary>
        ''' 连接通道发生错误时触发事件
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Sub FireConnectorErrorEvent(connector As INConnectorDetail)


        ''' <summary>
        ''' 连接通道连接建立成功时发生
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Sub FireConnectorConnectedEvent(connector As INConnectorDetail)


        ''' <summary>
        ''' 连接通道连接关闭时发生
        ''' </summary>
        ''' <param name="connector">触发事件的连接通道信息</param>
        Sub FireConnectorClosedEvent(connector As INConnectorDetail)



        ''' <summary>
        ''' 事务消息，对端主动传送到本地的请求数据，此数据解码后产生此事件
        ''' </summary>
        ''' <param name="EventData"></param>
        Sub FireTransactionMessage(EventData As INData)


        ''' <summary>
        ''' 客户端上线
        ''' </summary>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Sub FireClientOnline(e As INConnector)


        ''' <summary>
        ''' 客户端离线
        ''' </summary>
        ''' <param name="e">包含事件所代表的客户端及服务器信息</param>
        Sub FireClientOffline(e As INConnector)
    End Interface
End Namespace

