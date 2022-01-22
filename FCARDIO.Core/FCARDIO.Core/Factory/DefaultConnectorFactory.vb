Imports System.Reflection
Imports DoNetDrive.Core.Connector

Namespace Factory
    ''' <summary>
    ''' 默认的连接通道分配工厂，可以分配一下几种类型的连接器
    ''' TCPClient、TCPServer、UDPServer、UDPClient、SerialPort、WebSocketClient、WebSocketServer
    ''' 
    ''' </summary>
    Public Class DefaultConnectorFactory
        Implements INConnectorFactory

        ''' <summary>
        ''' 保存连接器创建工厂
        ''' </summary>
        ''' <returns></returns>
        Public Property ConnectorFactoryDictionary As Dictionary(Of String, INConnectorFactory)

        Public Sub New()
            ConnectorFactoryDictionary = New Dictionary(Of String, INConnectorFactory)
            ConnectorFactoryDictionary.Add(ConnectorType.TCPClient, TCPClient.TCPClientFactory.GetInstance())
            ConnectorFactoryDictionary.Add(ConnectorType.TCPServer, TCPServer.TCPServerFactory.GetInstance())
            ConnectorFactoryDictionary.Add(ConnectorType.UDPServer, UDP.UDPServerFactory.GetInstance())
            ConnectorFactoryDictionary.Add(ConnectorType.UDPClient, UDP.UDPClientFactory.GetInstance())
            ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, SerialPort.SerialPortFactory.GetInstance())
        End Sub

        ''' <summary>
        ''' 创建一个连接通道
        ''' </summary>
        ''' <param name="cd">包含一个描述连接通道详情的内容用于创建连接通道</param>
        ''' <returns></returns>
        Public Function CreateConnector(cd As INConnectorDetail, ConnecterManage As IConnecterManage) As INConnector Implements INConnectorFactory.CreateConnector
            Dim cdTypeName = cd.GetTypeName()
            Dim ConnAor As INConnectorFactory '用于创建连接通道的分配器
            If ConnectorFactoryDictionary.ContainsKey(cdTypeName) Then
                ConnAor = ConnectorFactoryDictionary(cdTypeName)
                Return ConnAor.CreateConnector(cd, ConnecterManage)
            Else
                Return Nothing
            End If
        End Function


        ''' <summary>
        ''' 创建一个连接通道
        ''' </summary>
        ''' <param name="cd">包含一个描述连接通道详情的内容用于创建连接通道</param>
        ''' <returns></returns>
        Public Async Function CreateConnectorAsync(cd As INConnectorDetail, ConnecterManage As IConnecterManage) As Task(Of INConnector) Implements INConnectorFactory.CreateConnectorAsync
            Dim cdTypeName = cd.GetTypeName()
            Dim ConnAor As INConnectorFactory '用于创建连接通道的分配器
            If ConnectorFactoryDictionary.ContainsKey(cdTypeName) Then
                ConnAor = ConnectorFactoryDictionary(cdTypeName)
                Return Await ConnAor.CreateConnectorAsync(cd, ConnecterManage).ConfigureAwait(False)
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace

