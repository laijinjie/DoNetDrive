Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers
Imports System.Text
''' <summary>
''' 连接通道观察者，可以观察连接通道上的数据收发 十六进制格式输出
''' </summary>
Public Class TCPIOObserverHandler
    Implements INRequestHandle

    Private UTF8 As Encoding = Encoding.UTF8

    Private Const MsgDebugLen = 40
    ''' <summary>
    ''' 接收到数据
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    Event DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As String)
    ''' <summary>
    ''' 准备发送数据
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    Event DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As String)

    Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
        Dim sHex As String
        Dim iLen = msg.ReadableBytes

        sHex = UTF8.GetString(msg.Array, msg.ArrayOffset, iLen)


        RaiseEvent DisposeRequestEvent(connector, msg.ReadableBytes, sHex)

    End Sub

    Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
        Dim sHex As String
        Dim iLen = msg.ReadableBytes

        sHex = UTF8.GetString(msg.Array, msg.ArrayOffset, iLen)

        RaiseEvent DisposeResponseEvent(connector, msg.ReadableBytes, sHex)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Return
    End Sub
End Class
