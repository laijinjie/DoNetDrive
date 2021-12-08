Imports System.Text
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DotNetty.Buffers
''' <summary>
''' 连接通道观察者，可以观察连接通道上的数据收发 十六进制格式输出
''' </summary>
Public Class TCPIOObserverHandler
    Implements INRequestHandle

    Public Shared UTF8 As Encoding = Encoding.UTF8

    Private Const MsgDebugLen = 40
    ''' <summary>
    ''' 接收到数据
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    Event DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As String)

    ''' <summary>
    ''' 接收消息日志
    ''' </summary>
    ''' <param name="connector"></param>
    Event OnRequestLog(connector As INConnector, msg As String)

    ''' <summary>
    ''' 准备发送数据
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="msgLen"></param>
    Event DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As String)

    Private mRequestMsgCRC As UInt32
    Private mBeginRead As Boolean
    Private mRequestMax As Integer
    Private mRequestLen As Integer

    Private mRequestBuf As List(Of Byte)

    Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
        Dim len = msg.ReadableBytes
        Dim sLog = msg.ReadString(msg.ReadableBytes, UTF8)
        RaiseEvent DisposeRequestEvent(connector, len, sLog)
    End Sub

    Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
        Dim len = msg.ReadableBytes
        Dim sLog = msg.ReadString(msg.ReadableBytes, UTF8)

        RaiseEvent DisposeResponseEvent(connector, len, sLog)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Return
    End Sub
End Class
