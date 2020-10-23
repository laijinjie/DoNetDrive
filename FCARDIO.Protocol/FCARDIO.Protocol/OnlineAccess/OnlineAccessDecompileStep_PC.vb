
Imports DotNetty.Buffers
Imports DoNetDrive.Protocol.Packet

Namespace OnlineAccess

    ''' <summary>
    ''' 适用于 OnlineAccess 数据包格式的命令包解析步骤
    ''' </summary>
    Friend Class OnlineAccessDecompileStep_PC
        ''' <summary>
        ''' 解析信息SN
        ''' </summary>
        Public Shared SN As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_SN_PC()
        ''' <summary>
        ''' 解析密码
        ''' </summary>
        Public Shared Password As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_Password_PC()
        ''' <summary>
        ''' 解析信息代码
        ''' </summary>
        Public Shared Code As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CommandCode_PC()
        ''' <summary>
        ''' 解析命令部分
        ''' </summary>
        Public Shared Command As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_Command_PC()
        ''' <summary>
        ''' 解析命令长度 
        ''' </summary>
        Public Shared DataLen As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_DataLen_PC()
        ''' <summary>
        ''' 解析数据部分
        ''' </summary>
        Public Shared CmdData As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CmdData_PC()
        ''' <summary>
        ''' 判断检验和
        ''' </summary>
        Public Shared CheckSum As INDecompileStep(Of OnlineAccessPacket) = New DecompileStep_CheckSum_PC()
    End Class



    ''' <summary>
    ''' 解析SN
    ''' </summary>
    Friend Class DecompileStep_SN_PC
        Inherits DecompileStep_SN

        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, pck As OnlineAccessPacket)
            MyBase.DecompileNext(decompile, buf, pck)
            decompile.DecompileStep = OnlineAccessDecompileStep_PC.Password '进入到下一步骤
        End Sub
    End Class


    ''' <summary>
    ''' 解析密码
    ''' </summary>
    Friend Class DecompileStep_Password_PC
        Inherits DecompileStep_Password

        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, pck As OnlineAccessPacket)
            MyBase.DecompileNext(decompile, buf, pck)
            decompile.DecompileStep = OnlineAccessDecompileStep_PC.Code '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析信息代码
    ''' </summary>
    Friend Class DecompileStep_CommandCode_PC
        Inherits DecompileStep_CommandCode
        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, pck As OnlineAccessPacket)
            MyBase.DecompileNext(decompile, buf, pck)
            decompile.DecompileStep = OnlineAccessDecompileStep_PC.Command '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令
    ''' </summary>
    Friend Class DecompileStep_Command_PC
        Inherits DecompileStep_Command

        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, p As OnlineAccessPacket)
            MyBase.DecompileNext(decompile, buf, p)
            decompile.DecompileStep = OnlineAccessDecompileStep_PC.DataLen '进入到下一步骤
        End Sub
    End Class

    ''' <summary>
    ''' 解析命令长度
    ''' </summary>
    Friend Class DecompileStep_DataLen_PC
        Inherits DecompileStep_DataLen

        Protected Overrides Sub DecompileNext(decompile As BaseDecompile(Of OnlineAccessPacket), buf As IByteBuffer, p As OnlineAccessPacket)
            MyBase.DecompileNext(decompile, buf, p)

            If p.DataLen > 0 Then
                decompile.DecompileStep = OnlineAccessDecompileStep_PC.CmdData '进入到下一步骤
            Else
                decompile.DecompileStep = OnlineAccessDecompileStep_PC.CheckSum '进入到下一步骤
            End If

        End Sub
    End Class

    ''' <summary>
    ''' 解析命令数据
    ''' </summary>
    Friend Class DecompileStep_CmdData_PC
        Inherits DecompileStep_CmdData

        Public Overrides Function DecompileStep(decompile As BaseDecompile(Of OnlineAccessPacket), value As Byte) As Boolean ' Implements INDecompileStep(Of OnlineAccessPacket).DecompileStep
            MyBase.DecompileStep(decompile, value)
            Dim p = decompile.GetPacket()
            Dim buf = p.CmdData
            Dim iLen = p.DataLen

            If buf.ReadableBytes = iLen Then
                decompile.DecompileStep = OnlineAccessDecompileStep_PC.CheckSum '进入到下一步骤
            End If
            Return False
        End Function
    End Class

    ''' <summary>
    ''' 解析命令校验和
    ''' </summary>
    Friend Class DecompileStep_CheckSum_PC
        Inherits DecompileStep_CheckSum

        Public Overrides Function DecompileStep(decompile As BaseDecompile(Of OnlineAccessPacket), value As Byte) As Boolean 'Implements INDecompileStep(Of OnlineAccessPacket).DecompileStep
            Dim bRet = MyBase.DecompileStep(decompile, value)

            decompile.DecompileStep = OnlineAccessDecompileStep_PC.SN '进入到下一步骤

            Return bRet
        End Function
    End Class






End Namespace


