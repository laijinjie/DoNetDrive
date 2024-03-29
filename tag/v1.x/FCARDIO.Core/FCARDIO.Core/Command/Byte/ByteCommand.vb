﻿Imports DoNetDrive.Core.Packet

Namespace Command.Byte
    ''' <summary>
    ''' 用于将一个字节流命令发送出去
    ''' </summary>
    Public Class ByteCommand
        Inherits AbstractCommand


        ''' <summary>
        ''' 初始化两个重要参数，并进行参数检查
        ''' </summary>
        ''' <param name="cd">表示命令详情，包含通道信息，对端信息，超时时间，重发次数</param>
        Sub New(cd As INCommandDetail, par As ByteCommandParameter)
            MyBase.New(cd, par)
            _IsWaitResponse = False
        End Sub

        ''' <summary>
        ''' 指令开始执行时，用于让命令组装第一个用于发送的数据包 CommandNext0 中组装（如果有的话）
        ''' </summary>
        Protected Overrides Sub CreatePacket()
            _Packet = New BytePacket(_Parameter)
        End Sub

        ''' <summary>
        ''' 释放使用的资源
        ''' </summary>
        Protected Overrides Sub Release0()
            Return
        End Sub

        ''' <summary>
        ''' 检查并进行命令的下一部分
        ''' </summary>
        ''' <param name="readPacket">收到的数据包</param>
        Protected Overrides Sub CommandNext(readPacket As INPacket)
            Return
        End Sub



        ''' <summary>
        ''' 重发时的设置参数
        ''' </summary>
        Protected Overrides Sub CommandReSend()
            Return
        End Sub

        ''' <summary>
        ''' 检查命令参数
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Protected Overrides Function CheckCommandParameter(value As INCommandParameter) As Boolean
            Return True
        End Function
    End Class
End Namespace

