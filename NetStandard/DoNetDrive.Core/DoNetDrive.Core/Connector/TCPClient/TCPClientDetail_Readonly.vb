Namespace Connector.TCPClient
    Public Class TCPClientDetail_Readonly
        Inherits TCPClientDetail

        ''' <summary>
        ''' 别名
        ''' </summary>
        Protected _ConnectAlias As String

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
        ''' <param name="oTCP">远程服务器的详情</param>
        Sub New(oTCP As TCPClientDetail)
            MyBase.New(oTCP.Addr, oTCP.Port,
                       oTCP.LocalAddr, oTCP.LocalPort,
                       oTCP.IsSSL, oTCP.Certificate, oTCP.SSLStreamFactory)
            _Addr = oTCP.Addr
            _Port = oTCP.Port
            _LocalAddr = oTCP.LocalAddr
            _LocalPort = oTCP.LocalPort
            _ConnectAlias = oTCP.ConnectAlias
        End Sub



        Overrides Property Addr As String
            Get
                Return _Addr
            End Get
            Set(value As String)
                Return
            End Set
        End Property



        Overrides Property Port As Integer
            Get
                Return _Port
            End Get
            Set(value As Integer)
                Return
            End Set
        End Property


        Public Overrides Property LocalAddr As String
            Get
                Return _LocalAddr
            End Get
            Set(value As String)
                Return
            End Set
        End Property

        Public Overrides Property LocalPort As Integer
            Get
                Return _LocalPort
            End Get
            Set(value As Integer)
                Return
            End Set
        End Property



        Public Overrides Property ConnectAlias As String
            Get
                Return _ConnectAlias
            End Get
            Set(value As String)
                Return
            End Set
        End Property
    End Class
End Namespace

