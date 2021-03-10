Imports DoNetDrive.Core.Packet
Imports DotNetty.Buffers

Public Class JsonPacketDecompile
    Implements INPacketDecompile
    ''' <summary>
    ''' 缓存的Json
    ''' </summary>
    Public mCacheJson As String


    Public Sub Dispose() Implements IDisposable.Dispose
        Return
    End Sub

    Public Function Decompile(buf As IByteBuffer, retPacketList As List(Of INPacket)) As Boolean Implements INPacketDecompile.Decompile
        Dim sJson = buf.ReadString(buf.ReadableBytes, JsonPacket.JsonEncoding)
        sJson = sJson.Trim()
        If sJson.Contains(vbNewLine) Then
            sJson = sJson.Replace(vbNewLine, String.Empty)
        End If
        If sJson.Contains(vbCr) Then
            sJson = sJson.Replace(vbCr, String.Empty)
        End If
        If sJson.Contains(vbLf) Then
            sJson = sJson.Replace(vbLf, String.Empty)
        End If
        If sJson.StartsWith("{") And sJson.EndsWith("}") Then
            retPacketList.Add(New JsonPacket(sJson))
            mCacheJson = String.Empty
            Return True
        Else
            If String.IsNullOrEmpty(mCacheJson) Then
                If sJson.StartsWith("{") Then
                    mCacheJson = sJson '先缓存
                Else
                    Return False '啥也不干，丢弃消息
                End If
            Else
                mCacheJson += sJson

                If mCacheJson.EndsWith("}") Then
                    retPacketList.Add(New JsonPacket(sJson))
                    mCacheJson = String.Empty
                    Return True
                End If
            End If
        End If
        Return False
    End Function
End Class
