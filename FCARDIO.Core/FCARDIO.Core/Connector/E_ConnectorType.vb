Namespace Connector
    Public Class E_ConnectorType
        ''' <summary>
        ''' 使用 RS232、RS485 通讯
        ''' </summary>
        Shared OnSerialPort As String = "SerialPort"

        ''' <summary>
        ''' 使用 TCP 协议，作为客户端连接到指定IP和端口的服务器
        ''' </summary>
        Shared OnTCPClient As String = "TCPClient"

        ''' <summary>
        ''' 使用 TCP 协议，在本地服务器中查询已连接到的客户端，需要指定客户端Key值
        ''' </summary>
        Shared OnTCPServer_Client As String = "TCPServer_Client"

        ''' <summary>
        ''' 使用 UDP 协议，发送数据到指定IP和端口，可指定本地绑定的IP和端口
        ''' </summary>
        Shared OnUDP As String = "UDP"

        ''' <summary>
        ''' 将需要指令写入到指定的文件地址中，需要指定文件路径和名称，并确保有可写权限
        ''' </summary>
        Shared OnFile As String = "File"
    End Class
End Namespace

