
Imports DotNetty.Buffers
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Packet

Namespace Command
    Public MustInherit Class AbstractRequestHandle
        Implements INRequestHandle

        ''' <summary>
        ''' 命令解析器
        ''' </summary>
        Private _Decompile As INPacketDecompile

        Public Sub New(dec As INPacketDecompile)
            _Decompile = dec
        End Sub


        Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
            Dim oRetPack As List(Of INPacket) = New List(Of INPacket)(10)
            Dim decompile As Boolean = False

            decompile = _Decompile.Decompile(msg, oRetPack)
            If decompile Then
                Dim iLen = oRetPack.Count
                For Each p In oRetPack
                    fireRequestEvent(connector, p)
                    p.Dispose()
                Next
            End If
        End Sub

        ''' <summary>
        ''' 触发数据请求事件
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="p"></param>
        Protected MustOverride Sub fireRequestEvent(connector As INConnector, p As INPacket)

        ''' <summary>
        ''' 处理响应
        ''' </summary>
        Public MustOverride Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    _Decompile.Dispose()
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

End Namespace
