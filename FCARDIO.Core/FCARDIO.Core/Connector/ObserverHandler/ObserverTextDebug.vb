
Namespace Connector
    ''' <summary>
    ''' 文本观察者输出接口
    ''' </summary>
    Public Class ObserverTextDebug
        Implements IObserverTextDebug

        Private ReadCallblack As Action(Of INConnector, String)
        Private SendCallblack As Action(Of INConnector, String)
        ''' <summary>
        ''' 创建文本观察者输出接口，并绑定回调委托
        ''' </summary>
        Public Sub New(ByVal read As Action(Of INConnector, String), ByVal send As Action(Of INConnector, String))
            ReadCallblack = read
            SendCallblack = send
        End Sub


        Public Sub DisposeRequest(connector As INConnector, msg As String) Implements IObserverTextDebug.DisposeRequest
            ReadCallblack?.Invoke(connector, msg)
        End Sub

        Public Sub DisposeResponse(connector As INConnector, msg As String) Implements IObserverTextDebug.DisposeResponse
            SendCallblack?.Invoke(connector, msg)
        End Sub
    End Class

End Namespace
