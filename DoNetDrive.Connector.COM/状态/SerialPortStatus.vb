
Imports DoNetDrive.Core.Connector

Public Class SerialPortStatus
    ''' <summary>
    ''' 空闲状态
    ''' </summary>
    Public Shared Closed As SerialPortStatus_Closed = New SerialPortStatus_Closed

    ''' <summary>
    '''  表示串口已开启的状态
    ''' </summary>
    Public Shared Connected As SerialPortStatus_Connected = New SerialPortStatus_Connected

    ''' <summary>
    ''' 正在打开串口
    ''' </summary>
    Public Shared Connecting As INConnectorStatus = New SerialPortStatus_Connecting

End Class

