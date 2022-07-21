Imports System.Net

Namespace Connector.UDP
    ''' <summary>
    ''' 指示一个UDP服务器的详细信息
    ''' </summary>
    Public Class UDPServerDetail
        Inherits AbstractConnectorDetail

        ''' <summary>
        ''' 连接远程服务器的本地IP
        ''' </summary>
        Public ReadOnly LocalAddr As String
        ''' <summary>
        ''' 连接远程服务器的本地端口
        ''' </summary>
        Public ReadOnly LocalPort As Integer

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(ilocalPort As Integer)
            Me.New(String.Empty, ilocalPort)
        End Sub

        ''' <summary>
        ''' 初始化连接器详细
        ''' </summary>
        ''' <param name="slocal">指定本地IP</param>
        ''' <param name="ilocalPort">指定本地端口</param>
        Sub New(slocal As String, ilocalPort As Integer)
            If Not String.IsNullOrEmpty(LocalAddr) Then
                Dim oIP As IPAddress = Nothing
                If Not IPAddress.TryParse(LocalAddr, oIP) Then
                    Throw New ArgumentException("slocal is Error")
                End If
            End If
            LocalAddr = slocal

            LocalPort = ilocalPort
        End Sub


        ''' <summary>
        ''' 获取连接通道所在的程序集
        ''' </summary>
        ''' <returns>程序集名称</returns>
        Public Overrides Function GetAssemblyName() As String
            Return "DoNetDrive.Core"
        End Function

        ''' <summary>
        ''' 获取连接通道的类名
        ''' </summary>
        ''' <returns>类名的全名</returns>
        Public Overrides Function GetTypeName() As String
            Return ConnectorType.UDPServer
        End Function

        ''' <summary>
        ''' 用来比较此连接通道是否为同一个
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(other As INConnectorDetail) As Boolean
            If other Is Nothing Then Return False
            Dim t As UDPServerDetail = TryCast(other, UDPServerDetail)
            If t IsNot Nothing Then
                If Not t.LocalAddr.Equals(LocalAddr) Then Return False
                If Not t.LocalPort.Equals(LocalPort) Then Return False
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' 获取一个用于界定此通道的唯一Key值
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetKey() As String
            If "*".Equals(LocalAddr) Or String.IsNullOrEmpty(LocalAddr) Then
                Return $" {GetTypeName()}_Local:*:{LocalPort}"
            Else
                Return $" {GetTypeName()}_Local:{LocalAddr}:{LocalPort}"
            End If
        End Function


        ''' <summary>
        ''' 打印此详情所指示的连接信息
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            If "*".Equals(LocalAddr) Or String.IsNullOrEmpty(LocalAddr) Then
                Return $" {GetTypeName()} LocalAddr: *:{LocalPort}"
            Else
                Return $" {GetTypeName()} LocalAddr: {LocalAddr}:{LocalPort}"
            End If

        End Function
    End Class
End Namespace

