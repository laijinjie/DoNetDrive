Imports System.Text
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Common.Extensions

Namespace USBDrive
    ''' <summary>
    ''' 用于标记USB设备的身份详情记录
    ''' </summary>
    Public Class USBDriveCommandDetail
        Inherits AbstractCommandDetail
        ''' <summary>
        ''' 用于ASCII 字符串数据编解码
        ''' </summary>
        Private Shared ASCII As Encoding = Encoding.ASCII 'ASCII编码



        ''' <summary>
        ''' 控制器的SN
        ''' </summary>
        Public Addr As String



        ''' <summary>
        ''' 初始化一个新的控制器命令详情类
        ''' </summary>
        ''' <param name="cnt">只是通讯连接通道</param>
        ''' <param name="ar">设备机器号</param>
        Public Sub New(cnt As INConnectorDetail, ar As Integer)
            MyBase.New(cnt)
            Addr = ar
        End Sub

        ''' <summary>
        ''' 释放资源
        ''' </summary>
        Protected Overrides Sub Release0()
            Return
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
            Dim ot As USBDriveCommandDetail = TryCast(other, USBDriveCommandDetail)
            If ot Is Nothing Then
                If Connector.Equals(other.Connector) Then
                    Return Addr.Equals(ot.Addr)
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

        Public Overrides Function ToString() As String
            If Connector Is Nothing Then
                Return $"Addr:{Addr}"
            End If
            Return $"{Connector.ToString()} Addr:{Addr}"
        End Function
    End Class
End Namespace


