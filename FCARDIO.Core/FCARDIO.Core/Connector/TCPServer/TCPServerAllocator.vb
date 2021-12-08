
Imports System.Threading
Imports System.Net
Imports DotNetty.Transport.Bootstrapping
Imports DotNetty.Transport.Channels
Imports DotNetty.Transport.Channels.Sockets
Imports DoNetDrive.Core.Connector.TCPServer.Client
Imports DotNetty.Handlers.Logging
Imports System.Runtime.InteropServices
Imports System.Net.Sockets

Namespace Connector.TCPServer
    Public Class TCPServerAllocator
        Implements INConnectorAllocator

        Public Shared SoBacklog As Integer = 200
#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mTCPServerAllocator As TCPServerAllocator
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetAllocator() As TCPServerAllocator
            If mTCPServerAllocator Is Nothing Then
                SyncLock lockobj
                    If mTCPServerAllocator Is Nothing Then
                        mTCPServerAllocator = New TCPServerAllocator()
                    End If
                End SyncLock
            End If
            Return mTCPServerAllocator
        End Function
#End Region


#Region "获取客户端的Key"
        ''' <summary>
        ''' 所有客户端的ID
        ''' </summary>
        Protected Shared ClientID As Long

        ''' <summary>
        ''' 获取客户端的Key值
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetClientKey(local As IPDetail, remote As IPEndPoint, ByRef outClientID As Long) As String
            Dim p As IPAddress = remote.Address
            If p.IsIPv4MappedToIPv6 Then
                p = p.MapToIPv4
            End If

            Dim id = Interlocked.Increment(ClientID)
            outClientID = id
            Dim sKey = $"TCPServer_Local:{local}_Remote:{p}:{remote.Port}_ClientID:{id}"
            Return sKey
        End Function
#End Region


        ''' <summary>
        ''' 获取分配器可分配的连接器类全名
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConnectorTypeName() As String Implements INConnectorAllocator.GetConnectorTypeName
            Return "TCPServer.TCPServerConnector"
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function GetNewConnector(detail As INConnectorDetail) As INConnector Implements INConnectorAllocator.GetNewConnector
            Return New TCPServerConnector(detail)
        End Function


        Public Async Function GetNewConnectorAsync(detail As INConnectorDetail) As Task(Of INConnector) Implements INConnectorAllocator.GetNewConnectorAsync
            Dim server = New TCPServerConnector(detail)
            Await server.ConnectAsync
            Return server
        End Function

        ''' <summary>
        ''' 关闭这个连接通道分配器
        ''' </summary>
        Public Sub shutdownGracefully() Implements INConnectorAllocator.shutdownGracefully
            Return
        End Sub

    End Class
End Namespace

