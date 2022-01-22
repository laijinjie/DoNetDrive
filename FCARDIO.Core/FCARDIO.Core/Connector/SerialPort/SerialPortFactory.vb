Imports DoNetDrive.Core.Factory

Namespace Connector.SerialPort
    Public Class SerialPortFactory
        Implements INConnectorFactory
#Region "单例模式"

        ''' <summary>
        ''' 用于单例模式加锁的
        ''' </summary>
        Private Shared lockobj As Object = New Object

        ''' <summary>
        ''' 用于生成TCP Server的分配器
        ''' </summary>
        Private Shared mSerialPortFactory As SerialPortFactory
        ''' <summary>
        ''' 获取用于生成TCPServer的分配器
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As SerialPortFactory
            If mSerialPortFactory Is Nothing Then
                SyncLock lockobj
                    If mSerialPortFactory Is Nothing Then
                        mSerialPortFactory = New SerialPortFactory()
                    End If
                End SyncLock
            End If
            Return mSerialPortFactory
        End Function
#End Region


        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Function CreateConnector(detail As INConnectorDetail, Allocator As IConnecterManage) As INConnector Implements INConnectorFactory.CreateConnector
            Return New SerialPortConnector(detail)
        End Function

        ''' <summary>
        ''' 创建一个新的连接通道
        ''' </summary>
        ''' <param name="detail"></param>
        ''' <returns></returns>
        Public Async Function CreateConnectorAsync(detail As INConnectorDetail, Allocator As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim conncect = New SerialPortConnector(detail)
            Await conncect.ConnectAsync().ConfigureAwait(False)
            Return conncect
        End Function
    End Class
End Namespace
