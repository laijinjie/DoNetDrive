Imports DoNetDrive.Core.Connector

Namespace Factory
    ''' <summary>
    ''' 连接通道分配器工厂
    ''' </summary>
    Public Interface INConnectorFactory
        ''' <summary>
        ''' 创建一个连接通道
        ''' </summary>
        ''' <param name="cd">包含一个描述连接通道详情的内容用于创建连接通道</param>
        ''' <returns></returns>
        Function CreateConnector(cd As INConnectorDetail) As INConnector
        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Function Release() As Task
    End Interface
End Namespace

