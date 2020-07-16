Namespace Connector.TCPClient
    Public Class TCPClientDetail_Readonly
        Inherits TCPClientDetail
        ''' <summary>
        ''' 远程服务器的IP或域名
        ''' </summary>
        Protected _Addr As String
        ''' <summary>
        ''' 远程服务器的监听端口
        ''' </summary>
        Protected _Port As Integer

        ''' <summary>
        ''' 连接远程服务器的本地IP
        ''' </summary>
        Protected _LocalAddr As String
        ''' <summary>
        ''' 连接远程服务器的本地端口
        ''' </summary>
        Protected _LocalPort As Integer

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="sAddr">远程服务器的IP或域名</param>
        ''' <param name="iPort">远程服务器的监听端口</param>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(sAddr As String, iPort As Integer, slocal As String, ilocalPort As Integer)
            MyBase.New(sAddr, iPort, slocal, ilocalPort)
            _Addr = sAddr
            _Port = iPort
            _LocalAddr = slocal
            _LocalPort = ilocalPort
        End Sub


        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="oTCP">远程服务器的详情</param>
        Sub New(oTCP As TCPClientDetail)
            MyBase.New(oTCP.Addr, oTCP.Port, oTCP.LocalAddr, oTCP.LocalPort, oTCP.IsSSL, oTCP.Certificate, oTCP.SSLStreamFactory)
            _Addr = oTCP.Addr
            _Port = oTCP.Port
            _LocalAddr = oTCP.LocalAddr
            _LocalPort = oTCP.LocalPort
        End Sub


        ''' <summary>
        ''' 远程服务器的IP或域名
        ''' </summary>
        ''' <returns></returns>
        Overrides Property Addr As String
            Get
                Return _Addr
            End Get
            Set(value As String)
                Return
            End Set
        End Property


        ''' <summary>
        ''' 远程服务器的监听端口
        ''' </summary>
        ''' <returns></returns>
        Overrides Property Port As Integer
            Get
                Return _Port
            End Get
            Set(value As Integer)
                Return
            End Set
        End Property

        ''' <summary>
        ''' 连接远程服务器的本地IP
        ''' </summary>
        Public Overrides Property LocalAddr As String
            Get
                Return _LocalAddr
            End Get
            Set(value As String)
                Return
            End Set
        End Property
        ''' <summary>
        ''' 连接远程服务器的本地端口
        ''' </summary>
        Public Overrides Property LocalPort As Integer
            Get
                Return _LocalPort
            End Get
            Set(value As Integer)
                Return
            End Set
        End Property

    End Class
End Namespace

