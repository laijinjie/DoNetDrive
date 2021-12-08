Namespace Connector
    Public Interface INConnectorAllocator
        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Function GetConnectorTypeName() As String

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Function GetNewConnector(detail As INConnectorDetail) As INConnector

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Function GetNewConnectorAsync(detail As INConnectorDetail) As Task(Of INConnector)

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Sub shutdownGracefully()
    End Interface
End Namespace

