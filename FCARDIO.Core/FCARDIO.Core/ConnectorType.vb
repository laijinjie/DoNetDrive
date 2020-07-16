Public Class ConnectorType
    ''' <summary>
    ''' 表示一个TCP 客户端连接通道，主动访问远程服务器
    ''' </summary>
    Public Const TCPClient As String = "TCPClientConnector"
    ''' <summary>
    ''' 表示一个TCP Server 下的 客户端节点通道，由客户端主动访问本地服务器生成
    ''' </summary>
    Public Const TCPServerClient As String = "TCPServerClientConnector"
    ''' <summary>
    ''' UDP 节点通道，一个UDP Server下的UDP节点
    ''' </summary>
    Public Const UDPClient As String = "UDPServerClientConnector"
    ''' <summary>
    ''' 表示本地的一个 UDP Server 通道
    ''' </summary>
    Public Const UDPServer As String = "UDPServerConnector"
    ''' <summary>
    ''' 表示本地的一个TCP Server 通道
    ''' </summary>
    Public Const TCPServer As String = "TCPServerConnector"
    ''' <summary>
    ''' 表示一个串口通道
    ''' </summary>
    Public Const SerialPort As String = "SerialPortConnector"
    ''' <summary>
    ''' 表示一个WebSocket Server 通道
    ''' </summary>
    Public Const WebSocketServer As String = "WebSocketServerConnector"
    ''' <summary>
    ''' 表示一个 WebSocket Server下的WebSocket 子节点通道
    ''' </summary>
    Public Const WebSocketServerClient As String = "WebSocketServerClientConnector"

    ''' <summary>
    ''' 表示一个Websocket客户端
    ''' </summary>
    Public Const WebSocketClient As String = "WebSocketClientConnector"
End Class
