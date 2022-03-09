

Imports DoNetDrive.Core
Imports DoNetDrive.Core.Connector
''' <summary>
''' 表示一个串口通讯的通道详情
''' </summary>
Public Class SerialPortDetail
    Inherits AbstractConnectorDetail

    ''' <summary>
    ''' 电脑上的串口号
    ''' </summary>
    Public ReadOnly Port As Byte
    ''' <summary>
    ''' 波特率；
    ''' 可选值：19200,115200 等标准值
    ''' </summary>
    Public Baudrate As Integer

    ''' <summary>
    ''' 根据串口号和波特率初始化
    ''' </summary>
    ''' <param name="iport">串口号</param>
    Public Sub New(ByVal iport As Byte)
        Me.New(iport, 19200)
    End Sub


    ''' <summary>
    ''' 根据串口号和波特率初始化
    ''' </summary>
    ''' <param name="iport">串口号</param>
    ''' <param name="iBaudrate">波特率</param>
    Public Sub New(ByVal iport As Byte, ByVal iBaudrate As Integer)
        Port = iport
        Baudrate = iBaudrate
    End Sub


    ''' <summary>
    ''' 获取连接通道的类名
    ''' </summary>
    ''' <returns>类名的全名</returns>
    Public Overrides Function GetTypeName() As String
        Return ConnectorType.SerialPort
    End Function

    ''' <summary>
    ''' 用来比较此连接通道是否为同一个
    ''' </summary>
    ''' <param name="other"></param>
    ''' <returns></returns>
    Public Overrides Function Equals(other As INConnectorDetail) As Boolean
        If other Is Nothing Then Return False
        Dim t As SerialPortDetail = TryCast(other, SerialPortDetail)
        If t IsNot Nothing Then
            If Not t.Port.Equals(Port) Then Return False
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
        Return $"{GetTypeName()}_COM{Port}"
    End Function

    ''' <summary>
    ''' 打印此详情所指示的连接信息
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function ToString() As String
        Return $" {GetTypeName()} COM{Port}_Baudrate:{Baudrate}"
    End Function



    ''' <summary>
    ''' 获取连接通道所在的程序集
    ''' 例如：DoNetDrive.Core
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetAssemblyName() As String
        Return "DoNetDrive.Core"
    End Function
End Class


