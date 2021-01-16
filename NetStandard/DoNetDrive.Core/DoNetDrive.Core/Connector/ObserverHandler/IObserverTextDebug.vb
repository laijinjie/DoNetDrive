Namespace Connector
    ''' <summary>
    ''' 文本观察者输出接口
    ''' </summary>
    Public Interface IObserverTextDebug
        ''' <summary>
        ''' 接收文本
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Sub DisposeRequest(connector As INConnector, msg As String)

        ''' <summary>
        ''' 发送文本
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Sub DisposeResponse(connector As INConnector, msg As String)
    End Interface
End Namespace

