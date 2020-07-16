
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.Packet

Namespace USBDrive

    ''' <summary>
    ''' 适用于 USBDrive 数据包格式的命令包解析步骤
    ''' </summary>
    Friend Class USBDriveDecompileStep
        ''' <summary>
        ''' 解析机器号
        ''' </summary>
        Public Shared Addr As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_Addr()
        ''' <summary>
        ''' 解析信息代码
        ''' </summary>
        Public Shared Code As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_CommandCode()

        ''' <summary>
        ''' 解析命令部分
        ''' </summary>
        Public Shared Command As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_Command()

        ''' <summary>
        ''' 解析命令长度 
        ''' </summary>
        Public Shared DataLen As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_DataLen()
        ''' <summary>
        ''' 解析数据部分
        ''' </summary>
        Public Shared CmdData As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_CmdData()
        ''' <summary>
        ''' 判断检验和
        ''' </summary>
        Public Shared CheckSum As BaseDecompileStep(Of USBDrivePacket) = New DecompileStep_CheckSum()
    End Class

    ''' <summary>
    ''' 解析机器号
    ''' </summary>
    Friend Class DecompileStep_Addr
        Inherits BaseDecompileStep(Of USBDrivePacket)

        ''' <summary>
        ''' 信息代码需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ReadBufLen As Integer = 1


        ''' <summary>
        ''' 解析机器号，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of USBDrivePacket), buf As IByteBuffer, pck As USBDrivePacket)
            AddSumCheck(buf, pck)

            Dim p = decompile._Packet
            p.Addr = buf.ReadByte()
            decompile.DecompileStep = USBDriveDecompileStep.Code '进入到下一步骤
        End Sub

    End Class


    ''' <summary>
    ''' 解析信息代码
    ''' </summary>
    Friend Class DecompileStep_CommandCode
        Inherits BaseDecompileStep(Of USBDrivePacket)

        ''' <summary>
        ''' 信息代码需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ReadBufLen As Integer = 2

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of USBDrivePacket), buf As IByteBuffer, pck As USBDrivePacket)
            AddSumCheck(buf, pck)
            pck.Code = buf.ReadUnsignedShort()
            decompile.DecompileStep = USBDriveDecompileStep.Command '进入到下一步骤
        End Sub
    End Class




    ''' <summary>
    ''' 解析命令
    ''' </summary>
    Friend Class DecompileStep_Command
        Inherits DecompileStep_CommandCode

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of USBDrivePacket), buf As IByteBuffer, p As USBDrivePacket)
            AddSumCheck(buf, p)

            p.CmdType = buf.ReadByte()
            p.CmdIndex = buf.ReadByte()
            decompile.DecompileStep = USBDriveDecompileStep.DataLen '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令长度
    ''' </summary>
    Friend Class DecompileStep_DataLen
        Inherits DecompileStep_Addr

        ''' <summary>
        ''' 解析命令长度，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of USBDrivePacket), buf As IByteBuffer, pck As USBDrivePacket)
            AddSumCheck(buf, pck)

            Dim p = decompile._Packet
            Dim iLen = buf.ReadByte
            p.DataLen = iLen
            If p.CmdData IsNot Nothing Then
                If p.CmdData.ReferenceCount > 0 Then
                    p.CmdData.Release()
                End If
                p.CmdData = Nothing
            End If
            If iLen > 0 Then
                p.CmdData = decompile._Allocator.Buffer(iLen)
                decompile.DecompileStep = USBDriveDecompileStep.CmdData '进入到下一步骤
            Else
                decompile.DecompileStep = USBDriveDecompileStep.CheckSum '进入到下一步骤
            End If
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令数据
    ''' </summary>
    Friend Class DecompileStep_CmdData
        Inherits DecompileStep_Addr

        ''' <summary>
        ''' 解析命令数据，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Public Overrides Function DecompileStep(decompile As BaseDecompile(Of USBDrivePacket), value As Byte) As Boolean
            Dim p = decompile._Packet
            Dim buf = p.CmdData
            Dim iLen = p.DataLen
            p.Check += value
            buf.WriteByte(value)
            If buf.ReadableBytes() = iLen Then
                decompile.DecompileStep = USBDriveDecompileStep.CheckSum '进入到下一步骤
            End If
            Return False
        End Function


    End Class

    ''' <summary>
    ''' 解析命令校验和
    ''' </summary>
    Friend Class DecompileStep_CheckSum
        Inherits DecompileStep_Addr

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Public Overrides Function DecompileStep(decompile As BaseDecompile(Of USBDrivePacket), value As Byte) As Boolean
            Dim p = decompile._Packet
            Dim chk = p.Check And 255
            p.Check = value
            If chk = value Then
                decompile.DecompileStep = USBDriveDecompileStep.Addr '进入到下一步骤
                Return True
            Else
                '数据长度异常
                decompile.ClearBuf()
                Return False
            End If

        End Function
    End Class

End Namespace