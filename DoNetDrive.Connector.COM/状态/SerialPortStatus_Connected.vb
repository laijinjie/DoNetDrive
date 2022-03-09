
Imports DoNetDrive.Core.Connector
''' <summary>
''' 表示串口已开启的状态
''' </summary>
Public Class SerialPortStatus_Connected
    Implements INConnectorStatus
    Public Function Status() As String Implements INConnectorStatus.Status
        Return "Connected"
    End Function


    Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
        Dim Client As SerialPortConnector = connector
        Client.CheckConnectedStatus()
    End Sub

End Class
