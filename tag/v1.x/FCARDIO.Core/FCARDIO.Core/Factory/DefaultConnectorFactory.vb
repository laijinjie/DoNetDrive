﻿Imports System.Reflection
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
        ''' 释放资源
        ''' </summary>
        Public Async Function Release() As Task Implements INConnectorFactory.Release
            TCPClient.TCPClientAllocator.GetAllocator()?.shutdownGracefully()
            TCPServer.TCPServerAllocator.GetAllocator()?.shutdownGracefully()
            UDP.UDPAllocator.GetAllocator()?.shutdownGracefully()
            Await DotNettyAllocator.shutdownGracefully()
        End Function

        ''' <summary>
        ''' 创建一个连接通道
        ''' </summary>
        ''' <param name="cd">包含一个描述连接通道详情的内容用于创建连接通道</param>
        ''' <returns></returns>
        Public Function CreateConnector(cd As INConnectorDetail) As INConnector Implements INConnectorFactory.CreateConnector
            Dim cdTypeName = $"{cd.GetTypeName()}"
            Dim ConnAor As INConnectorAllocator '用于创建连接通道的分配器
            Select Case cdTypeName
                Case ConnectorType.TCPClient
                    ConnAor = TCPClient.TCPClientAllocator.GetAllocator()
                    Return ConnAor.GetNewConnector(cd)
                Case ConnectorType.TCPServer
                    ConnAor = TCPServer.TCPServerAllocator.GetAllocator()
                    Return ConnAor.GetNewConnector(cd)
                Case ConnectorType.UDPClient, ConnectorType.UDPServer
                    ConnAor = UDP.UDPAllocator.GetAllocator()
                    Return ConnAor.GetNewConnector(cd)
                Case ConnectorType.SerialPort
                    Return New Connector.SerialPort.SerialPortConnector(cd)
                Case ConnectorType.WebSocketServer
                    ConnAor = WebSocket.Server.WebSocketServerAllocator.GetAllocator()
                    Return ConnAor.GetNewConnector(cd)
                Case ConnectorType.WebSocketClient
                    ConnAor = WebSocket.Client.WebSocketClientAllocator.GetAllocator()
                    Return ConnAor.GetNewConnector(cd)
                Case Else
                    Return Nothing
            End Select

        End Function
    End Class
End Namespace

