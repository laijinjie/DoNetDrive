Imports System.Text
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Common.Extensions

Namespace OnlineAccess
    ''' <summary>
    ''' 用于在线门禁的身份详情记录
    ''' </summary>
    Public Class OnlineAccessCommandDetail
        Inherits AbstractCommandDetail
        ''' <summary>
        ''' 用于ASCII 字符串数据编解码
        ''' </summary>
        Protected Shared ASCII As Encoding = Encoding.ASCII 'ASCII编码
        ''' <summary>
        ''' 指示了一个空的SN
        ''' </summary>
        Public Shared EmptySN As Byte() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ''' <summary>
        ''' 默认的空密码
        ''' </summary>
        Public Shared EmptyPassword As Byte() = {255, 255, 255, 255}

        ''' <summary>
        ''' 控制器的SN
        ''' </summary>
        Protected mSN As String

        ''' <summary>
        ''' 控制器的SN
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Property SN As String
            Get
                Return mSN
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then
                    Throw New ArgumentException("SN Is Error")
                End If
                If ASCII.GetByteCount(value) <> 16 Then
                    Throw New ArgumentException("SN Is Error")
                End If
                mSN = value
            End Set
        End Property

        ''' <summary>
        ''' 控制器的密码
        ''' </summary>
        Protected mPassword As String

        ''' <summary>
        ''' 控制器的密码
        ''' 即长度为8的十六进制字符串
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Property Password As String
            Get
                Return mPassword
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then
                    Throw New ArgumentException("Password Is Error")
                End If
                If ASCII.GetByteCount(value) <> 8 Then
                    Throw New ArgumentException("Password Is Error")
                End If
                If Not value.IsHex() Then
                    Throw New ArgumentException("Password Error,Isnot Hex!")
                End If
                mPassword = value
            End Set
        End Property

        ''' <summary>
        ''' 初始化一个新的控制器命令详情类
        ''' </summary>
        ''' <param name="cnt">只是通讯连接通道</param>
        ''' <param name="s">控制器的SN，16个字符</param>
        ''' <param name="p">控制器的密码，8个字符的十六进制字符串</param>
        Public Sub New(cnt As INConnectorDetail, s As String, p As String)
            MyBase.New(cnt)
            SN = s
            Password = p
        End Sub

        ''' <summary>
        ''' 控制器SN的字节数组
        ''' </summary>
        ''' <returns></returns>
        Overridable ReadOnly Property SNByte As Byte()
            Get
                If String.IsNullOrEmpty(SN) Then
                    Return EmptySN
                End If
                Return SN.GetBytes()
            End Get
        End Property

        ''' <summary>
        ''' 控制器密码的字节数组
        ''' </summary>
        ''' <returns></returns>
        Overridable ReadOnly Property PasswordByte As Byte()
            Get
                If String.IsNullOrEmpty(Password) Then
                    Return EmptyPassword
                End If
                Return Password.HexToByte()
            End Get
        End Property

        ''' <summary>
        ''' 释放当前实例所使用的资源
        ''' </summary>
        Protected Overrides Sub Release0()
            SN = Nothing
            Password = Nothing
        End Sub

        ''' <summary>
        ''' 返回一个浅表副本
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function Clone() As Object
            Return MemberwiseClone()
        End Function

        ''' <summary>
        ''' 比较连接通道指向和SN是否相同
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(other As INCommandDetail) As Boolean
            Dim ot As OnlineAccessCommandDetail = TryCast(other, OnlineAccessCommandDetail)
            If ot Is Nothing Then
                If Connector.Equals(other.Connector) Then
                    Return SN.Equals(ot.SN)
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

        Public Overrides Function ToString() As String
            If Connector Is Nothing Then
                Return $"SN:{SN}"
            End If
            Return $"{Connector.ToString()} SN:{SN}"
        End Function
    End Class
End Namespace

