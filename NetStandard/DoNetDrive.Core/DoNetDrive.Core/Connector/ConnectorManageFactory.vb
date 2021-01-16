Imports DotNetty.Transport.Channels
Imports DoNetDrive.Core.TaskManage

Namespace Connector
    Public Class ConnectorManageFactory
        Inherits TaskManage.AbstractTaskManageFactory(Of INConnector)

        ''' <summary>
        ''' 通道分配器
        ''' </summary>
        Protected mAllocator As ConnectorAllocator

        Sub New(works As IEventLoopGroup, Acr As ConnectorAllocator)
            MyBase.New(works)
            mAllocator = Acr
            IniManage(works)
        End Sub
        ''' <summary>
        ''' 创建任务管理器
        ''' </summary>
        ''' <param name="elp"></param>
        ''' <returns></returns>
        Protected Overrides Function GetNewTaskManage(elp As IEventLoop) As AbstractTaskManage(Of INConnector)
            If mAllocator Is Nothing Then Return Nothing
            Return New ConnectorManager(elp, mAllocator)
        End Function
    End Class
End Namespace

