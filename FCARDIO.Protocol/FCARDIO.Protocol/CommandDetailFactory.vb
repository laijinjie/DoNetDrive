Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Common.Extensions

''' <summary>
''' 命令详情的创造工厂
''' </summary>
Public Class CommandDetailFactory
    ''' <summary>
    ''' 连接参数
    ''' </summary>
    Public Enum ConnectType
        ''' <summary>
        ''' 访问远程TCP服务器
        ''' </summary>
        TCPClient

        ''' <summary>
        ''' 本地开启的TCP 服务器中，查找已连接到服务器的客户端
        ''' </summary>
        TCPServerClient

        ''' <summary>
        ''' 访问远程UDP服务器
        ''' </summary>
        UDPClient

        ''' <summary>
        ''' 串口通讯
        ''' </summary>
        SerialPort
    End Enum

    ''' <summary>
    ''' 控制器型号
    ''' </summary>
    Public Enum ControllerType
        ''' <summary>
        ''' Door8800系列控制器，已知型号：
        ''' 8810A\8820A\8840A
        ''' </summary>
        Door88

        ''' <summary>
        ''' Door5800系列控制器，已知型号：
        ''' 5812R\5812T\5824R\5824T\5848R\5848T
        ''' </summary>
        Door58

        ''' <summary>
        ''' 5900系列控制器，已知型号：
        ''' 5912T\5924T\5948T
        ''' </summary>
        Door59

        ''' <summary>
        ''' 5926T道闸控制器，已知型号：
        ''' 5926T
        ''' </summary>
        Door5926T

        ''' <summary>
        ''' Door8900A系列控制器，已知型号：
        ''' 8910A\8920A\8940A
        ''' </summary>
        Door89A

        ''' <summary>
        ''' Door8900H系列控制器，已知型号：
        ''' 8910H\8920H\8940H
        ''' </summary>
        Door89H

        ''' <summary>
        ''' Door989脱机刷卡门禁系列，已知型号：
        ''' 988M\1882M\2882M\989M
        ''' </summary>
        Door989


        ''' <summary>
        ''' USB设备中的USB 卡片读写器
        ''' </summary>
        USBDrive_CardReader

        ''' <summary>
        ''' USB设备中的USB离线巡更棒
        ''' </summary>
        USBDrive_OfflinePatrol
    End Enum


    ''' <summary>
    ''' 创建一个命令详情
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function CreateDetail(connType As ConnectType, addr As String, port As Integer,
                                        colerType As ControllerType, SN As String, Password As String) As INCommandDetail

        Dim conn As INConnectorDetail = Nothing
        Select Case connType
            Case ConnectType.TCPClient
                conn = New TCPClient.TCPClientDetail(addr, port)
            Case ConnectType.TCPServerClient
                conn = New TCPServer.Client.TCPServerClientDetail(addr)
            Case ConnectType.UDPClient
                conn = New UDP.UDPClientDetail(addr, port)
            Case ConnectType.SerialPort
                conn = New SerialPort.SerialPortDetail(port)
        End Select

        If conn Is Nothing Then Return Nothing
        Dim cmd As INCommandDetail = Nothing
        Select Case colerType
            Case ControllerType.Door88, ControllerType.Door89A, ControllerType.Door89H,
                 ControllerType.Door989,
                 ControllerType.Door58, ControllerType.Door59, ControllerType.Door5926T
                cmd = New OnlineAccess.OnlineAccessCommandDetail(conn, SN, Password)

            Case ControllerType.USBDrive_CardReader, ControllerType.USBDrive_OfflinePatrol
                cmd = New USBDrive.USBDriveCommandDetail(conn, SN.ToInt32())
        End Select
        Return cmd
    End Function
End Class
