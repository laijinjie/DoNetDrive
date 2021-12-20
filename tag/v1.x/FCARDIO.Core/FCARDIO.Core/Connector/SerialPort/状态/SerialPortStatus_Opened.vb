Namespace Connector.SerialPort

    ''' <summary>
    ''' 表示串口已开启的状态
    ''' </summary>
    Public Class SerialPortStatus_Opened
        Inherits ConnectorStatus_Connected

        ''' <summary>
        '''  表示串口已开启的状态
        ''' </summary>
        Public Shared Opened As SerialPortStatus_Opened = New SerialPortStatus_Opened

        ''' <summary>
        ''' 检查通道中的命令列表，执行命令
        ''' </summary>
        ''' <param name="connector"></param>
        Protected Overrides Sub CheckCommandList(connector As INConnector)
            Dim client As SerialPortConnector = TryCast(connector, SerialPortConnector)
            If client Is Nothing Then Return
            '先读接收缓冲
            If client.ReadConnector() Then
                '检查命令状态
                client.CheckCommandList()
            End If

        End Sub

        ''' <summary>
        ''' 关闭串口
        ''' </summary>
        ''' <param name="connector"></param>
        Protected Overrides Sub CloseConnector(connector As INConnector)
            Dim client As SerialPortConnector = TryCast(connector, SerialPortConnector)
            If client Is Nothing Then Return
            client.CloseConnector()
        End Sub
    End Class

End Namespace