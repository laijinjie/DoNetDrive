Imports DotNetty.Buffers
Imports DoNetDrive.Core.Command

Namespace Connector
    ''' <summary>
    ''' 连接通道观察者，可以观察连接通道上的数据收发 十六进制格式输出
    ''' </summary>
    Public Class ConnectorObserverHandler
        Implements INRequestHandle

        ''' <summary>
        ''' 使用Echo，返回发送的内容
        ''' </summary>
        Public UseEcho As Boolean

        ''' <summary>
        ''' 十六进制输出内容
        ''' </summary>
        Public HexDump As Boolean

        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Event DisposeRequestEvent(connector As INConnector, msg As String)
        ''' <summary>
        ''' 准备发送数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msg"></param>
        Event DisposeResponseEvent(connector As INConnector, msg As String)

        Public Sub New()
            UseEcho = False
            HexDump = True
        End Sub

        Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
            If HexDump Then RaiseEvent DisposeRequestEvent(connector, ByteBufferUtil.HexDump(msg))


            If UseEcho Then
                Dim bEcho As Boolean = False
                Select Case connector.GetConnectorType()
                    Case ConnectorType.TCPServerClient
                        bEcho = True

                    Case ConnectorType.UDPClient
                        bEcho = True
                    Case ConnectorType.TCPServer
                        bEcho = True

                    Case ConnectorType.SerialPort
                        bEcho = True

                    Case ConnectorType.WebSocketServerClient
                        bEcho = True
                End Select

                If bEcho Then
                    connector.WriteByteBuf(msg)
                End If
            End If


        End Sub

        Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
            If HexDump Then RaiseEvent DisposeResponseEvent(connector, ByteBufferUtil.HexDump(msg))
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Return
        End Sub
    End Class
End Namespace

