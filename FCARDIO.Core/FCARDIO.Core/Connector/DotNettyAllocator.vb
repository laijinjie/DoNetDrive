Imports System.Threading
Imports DotNetty.Transport.Channels


Namespace Connector
    Public MustInherit Class DotNettyAllocator

        ''' <summary>
        ''' 本系统使用的Buffer分配器
        ''' </summary>
        Public Shared BufferAllocator As DotNetty.Buffers.IByteBufferAllocator




        Shared Sub New()

            BufferAllocator = DotNetty.Buffers.UnpooledByteBufferAllocator.Default
        End Sub

        Shared Function GetBufferAllocator() As DotNetty.Buffers.IByteBufferAllocator
            Return BufferAllocator
        End Function


    End Class

End Namespace
