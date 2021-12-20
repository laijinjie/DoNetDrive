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

    Private mHexLog As Boolean

    ''' <summary>
    ''' 接收到数据
    ''' </summary>
    Public ReadDataCallblack As Action(Of INConnector, Integer, IByteBuffer)
    ''' <summary>
    ''' 准备发送数据
    ''' </summary>
    Public SendDataCallbalck As Action(Of INConnector, Integer, IByteBuffer)

    Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
        'Dim sHex As String
        'Dim iLen = msg.ReadableBytes

        'If mHexLog Then
        '    sHex = ByteBufferUtil.HexDump(msg)
        'Else
        '    sHex = UTF8.GetString(msg.Array, msg.ArrayOffset, iLen)
        'End If

        ReadDataCallblack?.Invoke(connector, msg.ReadableBytes, msg)

    End Sub

    Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
        'Dim sHex As String
        'Dim iLen = msg.ReadableBytes

        'If mHexLog Then
        '    sHex = ByteBufferUtil.HexDump(msg)
        'Else
        '    sHex = UTF8.GetString(msg.Array, msg.ArrayOffset, iLen)
        'End If

        SendDataCallbalck?.Invoke(connector, msg.ReadableBytes, msg)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Return
    End Sub

    Friend Sub ShowHex(value As Boolean)
        mHexLog = value
    End Sub
End Class
