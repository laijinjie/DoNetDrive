Namespace Connector.SerialPort

    ''' <summary>
    ''' 表示通道空闲，未开启串口的状态
    ''' </summary>
    Public Class SerialPortStatus_Free
        Inherits ConnectorStatus_Free
        ''' <summary>
        ''' 空闲状态
        ''' </summary>
        Public Shared Free As SerialPortStatus_Free = New SerialPortStatus_Free

        ''' <summary>
        ''' 打开通道，打开串口
        ''' </summary>
        ''' <param name="connector"></param>
        Protected Overrides Sub OpenConnector(connector As INConnector)
            Dim client As SerialPortConnector = TryCast(connector, SerialPortConnector)
            If client Is Nothing Then Return
            Try
                client.Open()
            Catch ex As Exception
                client.GetConnectorDetail().SetError(ex)
                client.FireConnectorErrorEvent(client.GetConnectorDetail())
                client.Close()
            End Try


        End Sub
    End Class

End Namespace