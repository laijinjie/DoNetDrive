Imports DotNetty.Transport.Channels

Namespace Connector
    ''' <summary>
    ''' 连接通道管理器，管理一组连接通道，用于维护通道状态和执行通道指令
    ''' </summary>
    Public Class ConnectorManager
        Inherits TaskManage.AbstractTaskManage(Of INConnector)

        ''' <summary>
        ''' 通道分配器
        ''' </summary>
        Protected mAllocator As ConnectorAllocator

        ''' <summary>
        ''' 连接通道管理器，管理一组连接通道，用于维护通道状态和执行通道指令
        ''' </summary>
        ''' <param name="elp"></param>
        ''' <param name="Acr"></param>
        Sub New(elp As IEventLoop, Acr As ConnectorAllocator)
            MyBase.New(elp)
            mAllocator = Acr
        End Sub

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub _Release()
            mAllocator = Nothing
        End Sub

        ''' <summary>
        ''' 检查客户端状态
        ''' </summary>
        ''' <param name="oConn"></param>
        Protected Overrides Sub ClientRun(oConn As INConnector)
            If mAllocator Is Nothing Then Return
            If mAllocator.IsRelease Then
                Return
            End If
            oConn.Run()
        End Sub

        Protected Overrides Sub _RemoveClient(oClient As INConnector)
            Dim sKey As String = oClient.GetKey()

            mAllocator.RemoveConnector(sKey, oClient)
        End Sub
    End Class
End Namespace

