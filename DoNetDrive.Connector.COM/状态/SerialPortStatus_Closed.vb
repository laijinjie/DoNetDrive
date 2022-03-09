
Imports DoNetDrive.Core.Connector
''' <summary>
''' 表示通道已关闭，未开启串口的状态
''' </summary>
Public Class SerialPortStatus_Closed
    Implements INConnectorStatus

    Public Function Status() As String Implements INConnectorStatus.Status
        Return "Closed"
    End Function


    Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
        If Not connector.CheckIsInvalid() Then
            Dim serial As SerialPortConnector = connector
            connector.ConnectAsync().ContinueWith(AddressOf serial.ConnectingNext)
        End If
    End Sub
End Class
