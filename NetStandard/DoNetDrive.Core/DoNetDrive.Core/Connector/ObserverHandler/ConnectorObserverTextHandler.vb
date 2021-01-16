Imports System.Text
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Command

Namespace Connector
    ''' <summary>
    ''' 基于文本的通道观察者
    ''' </summary>
    Public Class ConnectorObserverTextHandler
        Implements INRequestHandle
        ''' <summary>
        ''' 输出接口
        ''' </summary>
        Private _Debug As IObserverTextDebug

        ''' <summary>
        ''' 文字编码
        ''' </summary>
        Private TextEncoding As Encoding


        Public Sub New(log As IObserverTextDebug, enc As Encoding)
            _Debug = log
            TextEncoding = enc
        End Sub

        ''' <summary>
        ''' 接收的数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Public Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
            Dim sMsg = msg.ReadString(msg.ReadableBytes, TextEncoding)
            _Debug?.DisposeRequest(connector, sMsg)
        End Sub

        ''' <summary>
        ''' 发送的数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Public Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
            Dim sMsg = msg.ReadString(msg.ReadableBytes, TextEncoding)
            _Debug?.DisposeResponse(connector, sMsg)
        End Sub

        ''' <summary>
        ''' 关闭观察者
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            '_Debug = Nothing
            'TextEncoding = Nothing
            Return
        End Sub
    End Class

End Namespace
