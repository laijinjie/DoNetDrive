Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers

Namespace Command
    ''' <summary>
    ''' 处理连接通道上接收到的数据
    ''' </summary>
    Public Interface INRequestHandle
        Inherits IDisposable


        ''' <summary>
        ''' 处理接收的数据
        ''' </summary>
        Sub DisposeRequest(connector As INConnector, msg As IByteBuffer)

        ''' <summary>
        ''' 处理响应
        ''' </summary>
        Sub DisposeResponse(connector As INConnector, msg As IByteBuffer)
    End Interface
End Namespace

