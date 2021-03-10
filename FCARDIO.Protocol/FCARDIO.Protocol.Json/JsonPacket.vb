Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers
Imports DoNetDrive.Common.Extensions
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Connector.WebSocket

Public Class JsonPacket
    Implements INPacket
    Public Shared JsonEncoding As Text.Encoding = Text.Encoding.UTF8

    Private JsonValue As Object
    Private JsonString As String


    ''' <summary>
    ''' 通道中发送文本数据
    ''' </summary>
    ''' <param name="connector"></param>
    ''' <param name="sJson"></param>
    Public Shared Sub SendWebsocketPacket(connector As INConnector, ByVal sJson As String)
        Dim tBuf = New WebsocketTextBuffer(sJson, JsonEncoding)
        connector.WriteByteBuf(tBuf)
    End Sub


    Public Function GetJson() As String
        Dim sJson As String
        If JsonValue IsNot Nothing Then
            sJson = StringExtensions.ToJSON(JsonValue)
        Else
            sJson = JsonString
        End If
        Return sJson
    End Function


    ''' <summary>
    ''' 初始化数据包，让数据包和一个参数关联
    ''' </summary>
    ''' <param name="par"></param>
    Public Sub New(par As Object)
        If TypeOf par Is String Then
            JsonString = TryCast(par, String)
        Else
            JsonValue = par
        End If

    End Sub

    Public Sub ReleaseDataBuf() Implements INPacket.ReleaseDataBuf
        Return
    End Sub

    ''' <summary>
    ''' 获取一个buf用于发送数据
    ''' </summary>
    ''' <param name="Allocator"></param>
    ''' <returns></returns>
    Public Function GetPacketData(Allocator As IByteBufferAllocator) As IByteBuffer Implements INPacket.GetPacketData
        Dim sJson = GetJson()

        Dim iCount = JsonEncoding.GetByteCount(sJson)

        Dim buf = Allocator.Buffer(iCount)
        buf.WriteString(sJson, JsonEncoding)
        Return buf
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 要检测冗余调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)。
                JsonValue = Nothing
            End If

            ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
            ' TODO: 将大型字段设置为 null。
        End If
        disposedValue = True
    End Sub

    ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码以正确实现可释放模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        Dispose(True)
        ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
