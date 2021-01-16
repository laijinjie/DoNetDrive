Imports System.IO
Imports System.Text
Imports System.Threading
Imports DotNetty.Buffers
Imports DotNetty.Common
Imports DotNetty.Common.Utilities

Namespace Connector.WebSocket


    Public Class WebsocketTextBuffer
        Inherits UnpooledHeapByteBuffer

        Private _Buf As IByteBuffer
        Public Sub New(ByVal sText As String, ByVal enc As System.Text.Encoding)
            MyBase.New(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, 1, 10)
            _Buf = UnpooledByteBufferAllocator.Default.Buffer(enc.GetByteCount(sText))
            _Buf.WriteString(sText, enc)
        End Sub

        Public Function GetBuf() As IByteBuffer
            Return _Buf
        End Function




    End Class
End Namespace
