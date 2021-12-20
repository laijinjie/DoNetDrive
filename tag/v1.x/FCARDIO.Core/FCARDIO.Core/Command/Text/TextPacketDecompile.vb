Imports DotNetty.Buffers
Imports DoNetDrive.Core.Packet

Namespace Command.Text


    Public Class TextPacketDecompile
        Implements INPacketDecompile

        ''' <summary>
        ''' 文本的编码方式
        ''' </summary>
        Public Encoding As System.Text.Encoding


        Public Sub New(txtEncoding As System.Text.Encoding)
            Encoding = txtEncoding
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Return
        End Sub

        Public Function Decompile(buf As IByteBuffer, retPacketList As List(Of INPacket)) As Boolean Implements INPacketDecompile.Decompile
            Dim sValue = buf.ReadString(buf.ReadableBytes, Encoding)
            retPacketList.Add(New TextPacket(New TextCommandParameter(sValue, Encoding)))
            Return True
        End Function
    End Class


End Namespace