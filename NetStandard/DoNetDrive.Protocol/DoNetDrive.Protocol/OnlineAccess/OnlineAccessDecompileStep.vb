
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.Packet

Namespace OnlineAccess

    ''' <summary>
    ''' 适用于 OnlineAccess 数据包格式的命令包解析步骤
    ''' </summary>
    Friend Class OnlineAccessDecompileStep
        ''' <summary>
        ''' 解析信息代码
        ''' </summary>
        Public Shared Code As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CommandCode()
        ''' <summary>
        ''' 解析信息SN
        ''' </summary>
        Public Shared SN As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_SN()
        ''' <summary>
        ''' 解析密码
        ''' </summary>
        Public Shared Password As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_Password()
        ''' <summary>
        ''' 解析命令部分
        ''' </summary>
        Public Shared Command As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_Command()
        ''' <summary>
        ''' 解析命令长度 
        ''' </summary>
        Public Shared DataLen As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_DataLen()
        ''' <summary>
        ''' 解析数据部分
        ''' </summary>
        Public Shared CmdData As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CmdData()
        ''' <summary>
        ''' 判断检验和
        ''' </summary>
        Public Shared CheckSum As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CheckSum()
    End Class

    ''' <summary>
    ''' 解析信息代码
    ''' </summary>
    Friend Class DecompileStep_CommandCode
        Inherits BaseDecompileStep(Of OnlineAccessPacket)

        ''' <summary>
        ''' 信息代码需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ReadBufLen As Integer = 4

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, pck As OnlineAccessPacket)
            AddSumCheck(buf, pck)
            pck.Code = buf.ReadUnsignedInt()
            decompile.DecompileStep = OnlineAccessDecompileStep.SN '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析SN
    ''' </summary>
    Friend Class DecompileStep_SN
        Inherits BaseDecompileStep(Of OnlineAccessPacket)

        ''' <summary>
        ''' SN需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ReadBufLen As Integer = 16

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, pck As OnlineAccessPacket)
            If pck.SN Is Nothing Then
                ReDim pck.SN(15)
            End If
            AddSumCheck(buf, pck)

            buf.ReadBytes(pck.SN)
            decompile.DecompileStep = OnlineAccessDecompileStep.Password '进入到下一步骤
        End Sub
    End Class


    ''' <summary>
    ''' 解析密码
    ''' </summary>
    Friend Class DecompileStep_Password
        Inherits DecompileStep_CommandCode
        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, p As OnlineAccessPacket)
            If p.Password Is Nothing Then
                ReDim p.Password(3)
            End If
            AddSumCheck(buf, p)

            buf.ReadBytes(p.Password)
            decompile.DecompileStep = OnlineAccessDecompileStep.Command '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令
    ''' </summary>
    Friend Class DecompileStep_Command
        Inherits BaseDecompileStep(Of OnlineAccessPacket)

        ''' <summary>
        ''' 信息代码需要存储的长度
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property ReadBufLen As Integer = 3

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, p As OnlineAccessPacket)
            AddSumCheck(buf, p)

            p.CmdType = buf.ReadByte()
            p.CmdIndex = buf.ReadByte()
            p.CmdPar = buf.ReadByte()
            decompile.DecompileStep = OnlineAccessDecompileStep.DataLen '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令长度
    ''' </summary>
    Friend Class DecompileStep_DataLen
        Inherits DecompileStep_CommandCode

        ''' <summary>
        ''' 解析缓冲区，并接入下一个步骤
        ''' </summary>
        ''' <param name="decompile"></param>
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, p As OnlineAccessPacket)
            AddSumCheck(buf, p)

            Dim iLen = buf.ReadUnsignedInt()

            If iLen > 5242880 Then
                '数据长度异常
                decompile.ClearBuf()
                Return
            End If
            p.DataLen = iLen
            If p.CmdData IsNot Nothing Then
                If p.CmdData.ReferenceCount > 0 Then
                    p.CmdData.Release()
                End If
                p.CmdData = Nothing
            End If
            If iLen > 0 Then
                p.CmdData = decompile.GetNewByteBuffer(iLen)
                decompile.DecompileStep = OnlineAccessDecompileStep.CmdData '进入到下一步骤
            Else
                decompile.DecompileStep = OnlineAccessDecompileStep.CheckSum '进入到下一步骤
            End If

        End Sub
    End Class

    ''' <summary>
    ''' 解析命令数据
    ''' </summary>
    Friend Class DecompileStep_CmdData
        Implements INDecompileStep(Of OnlineAccessPacket)

        Public Overridable Function DecompileStep(decompile As BaseDecompile(Of OnlineAccessPacket), value As Byte) As Boolean Implements INDecompileStep(Of OnlineAccessPacket).DecompileStep
            Dim p = decompile.GetPacket()
            Dim buf = p.CmdData
            Dim iLen = p.DataLen
            p.Check += value
            buf.WriteByte(value)
            If buf.ReadableBytes = iLen Then
                decompile.DecompileStep = OnlineAccessDecompileStep.CheckSum '进入到下一步骤
            End If
            Return False
        End Function
    End Class

    ''' <summary>
    ''' 解析命令校验和
    ''' </summary>
    Friend Class DecompileStep_CheckSum
        Implements INDecompileStep(Of OnlineAccessPacket)

        Public Overridable Function DecompileStep(decompile As BaseDecompile(Of OnlineAccessPacket), value As Byte) As Boolean Implements INDecompileStep(Of OnlineAccessPacket).DecompileStep
            Dim p = decompile._Packet
            Dim chk = p.Check And 255
            p.Check = value

            decompile.DecompileStep = OnlineAccessDecompileStep.Code '进入到下一步骤

            If chk = value Then
                Return True
            Else
                '数据长度异常
                decompile.ClearBuf()
                Return False
            End If

        End Function
    End Class

End Namespace


