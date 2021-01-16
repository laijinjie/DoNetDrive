Imports DotNetty.Common.Concurrency

Namespace Connector
    ''' <summary>
    ''' 连接器的状态
    ''' </summary>
    Public Interface INConnectorStatus


        ''' <summary>
        ''' 获取当前状态的描述
        ''' </summary>
        ''' <returns></returns>
        Function Status() As String

        ''' <summary>
        ''' 检查状态是否需要变化
        ''' </summary>
        ''' <param name="connector"></param>
        Sub CheckStatus(ByVal connector As INConnector)
    End Interface
End Namespace

