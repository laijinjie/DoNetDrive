Imports System.Configuration
Imports System.Net
Imports System.Threading
Imports DoNetDrive.Core
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Extension
Imports DotNetty.Buffers
Imports DotNetty.Common.Utilities

Public Class FrmTCPClient
    Private mIsUnload As Boolean

    Private WithEvents Allocator As ConnectorAllocator
    ''' <summary>
    ''' 连接通道观察者，检查数据收发
    ''' </summary>
    Private WithEvents obServer As TCPIOObserverHandler



    ''' <summary>
    ''' 连接通道观察者，可以观察连接通道上的数据收发 十六进制格式输出
    ''' </summary>
    Public Class TCPIOObserverHandler
        Implements INRequestHandle

        Private Const MsgDebugLen = 40
        ''' <summary>
        ''' 接收到数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msgLen"></param>
        Event DisposeRequestEvent(connector As INConnector, msgLen As Integer, msg As String)

        ''' <summary>
        ''' 接收消息日志
        ''' </summary>
        ''' <param name="connector"></param>
        Event OnRequestLog(connector As INConnector, msg As String)

        ''' <summary>
        ''' 准备发送数据
        ''' </summary>
        ''' <param name="connector"></param>
        ''' <param name="msgLen"></param>
        Event DisposeResponseEvent(connector As INConnector, msgLen As Integer, msg As String)

        Private mRequestMsgCRC As UInt32
        Private mBeginRead As Boolean
        Private mRequestMax As Integer
        Private mRequestLen As Integer

        Private mRequestBuf As List(Of Byte)

        Public Overridable Sub DisposeRequest(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeRequest
            Dim sHex As String
            Dim iLen = msg.ReadableBytes

            If iLen > MsgDebugLen Then
                iLen = MsgDebugLen
            End If
            sHex = ByteBufferUtil.HexDump(msg, 0, iLen)

            msg.MarkReaderIndex()
            iLen = msg.ReadableBytes
            If mBeginRead Then
                If (mRequestLen + iLen) > mRequestMax Then
                    iLen = mRequestMax - mRequestLen
                End If
                mRequestLen += iLen
                mRequestBuf.AddRange(msg.Array.Slice(msg.ArrayOffset, iLen))
                mRequestMsgCRC = MyCRC32.CalculateDigest_Continue(mRequestMsgCRC, msg.Array, msg.ArrayOffset, iLen)
                If mRequestLen = mRequestMax Then
                    Debug.Print($"接收长度：{mRequestLen}")
                    mBeginRead = False
                    RaiseEvent OnRequestLog(connector, $"接收消息完毕，消息总长度：{mRequestLen} 自己计算CRC：{mRequestMsgCRC:X}")
                    mRequestMsgCRC = MyCRC32.CalculateDigest(mRequestBuf.ToArray(), 0, mRequestBuf.Count)
                    RaiseEvent OnRequestLog(connector, $"自己计算CRC2：{mRequestMsgCRC:X}")
                    mRequestBuf.Clear()
                    mRequestMsgCRC = msg.GetUnsignedInt(iLen)
                    RaiseEvent OnRequestLog(connector, $"接收的CRC：{mRequestMsgCRC:X}")
                End If
            Else
                If iLen > 10 Then
                    Dim str = msg.ReadString(10, System.Text.Encoding.ASCII)
                    If str = "DDDDDDDDDD" Then
                        mRequestBuf = New List(Of Byte)
                        mRequestBuf.AddRange(msg.Array.Slice(msg.ArrayOffset, iLen))
                        mRequestMax = msg.ReadInt() + 14
                        mRequestMsgCRC = MyCRC32.CalculateDigest(msg.Array, msg.ArrayOffset, iLen)
                        mBeginRead = True
                        RaiseEvent OnRequestLog(connector, $"开始接收消息，消息总长度：{mRequestMax}")
                        mRequestLen = iLen
                    End If

                End If
            End If

            msg.ResetReaderIndex()

            RaiseEvent DisposeRequestEvent(connector, msg.ReadableBytes, sHex)

        End Sub

        Public Overridable Sub DisposeResponse(connector As INConnector, msg As IByteBuffer) Implements INRequestHandle.DisposeResponse
            Dim sHex As String
            Dim iLen = msg.ReadableBytes
            If iLen > MsgDebugLen Then
                iLen = MsgDebugLen
            End If
            sHex = ByteBufferUtil.HexDump(msg, 0, iLen)

            RaiseEvent DisposeResponseEvent(connector, msg.ReadableBytes, sHex)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Return
        End Sub
    End Class


    Private Sub frmTCPServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Allocator = ConnectorAllocator.GetAllocator()
        obServer = New TCPIOObserverHandler()
        IniLoadLocalIP()
        LoadSetting()
        mIsUnload = False
    End Sub

    Private Sub frmTCPServer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        SaveSetting()
        mIsUnload = True
        Allocator.Dispose()
    End Sub

    Private Sub LoadSetting()
        Dim sValue As String = ConfigurationManager.AppSettings("UDPPort")

        sValue = ConfigurationManager.AppSettings("LocalIP")
        If Not String.IsNullOrEmpty(sValue) Then
            SelectComboxItem(cmbLocalIP, sValue)
        End If

        sValue = ConfigurationManager.AppSettings("LocalPort")
        If Not String.IsNullOrEmpty(sValue) Then
            txtLocalPort.Text = sValue
        End If


        txtRemoteIP.Text = ConfigurationManager.AppSettings("ServerIP")
        txtRemotePort.Text = ConfigurationManager.AppSettings("ServerPort")
    End Sub


    Private Sub SaveSetting()
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim kv = config.AppSettings.Settings
        SetConfigKey(config, "LocalIP", cmbLocalIP.Text)
        SetConfigKey(config, "LocalPort", txtLocalPort.Text)
        SetConfigKey(config, "ServerIP", txtRemoteIP.Text)
        SetConfigKey(config, "ServerPort", txtRemotePort.Text)
        config.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("appSettings")
    End Sub

    Private Sub SetConfigKey(config As Configuration, key As String, sValue As String)
        Dim kv = config.AppSettings.Settings
        If kv(key) Is Nothing Then
            kv.Add(key, sValue)
        Else
            kv(key).Value = sValue
        End If
    End Sub

    Private Sub SelectComboxItem(cmb As ComboBox, sValue As String)
        For Each sItem As String In cmb.Items
            If sItem = sValue Then
                cmb.SelectedItem = sItem
                Return
            End If
        Next
    End Sub


    Private Sub IniLoadLocalIP()
        cmbLocalIP.Items.Clear()
        Dim localentry = Dns.GetHostEntry(Dns.GetHostName())
        For Each ip As IPAddress In localentry.AddressList
            If ip.IsIPv4MappedToIPv6 Then

                ip = ip.MapToIPv4()
            End If
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                cmbLocalIP.Items.Add(ip.ToString())
            End If
        Next
        If (cmbLocalIP.Items.Count > 0) Then
            cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1
        End If
    End Sub




    Private Sub Allocator_CommandCompleteEvent(sender As Object, e As CommandEventArgs) Handles Allocator.CommandCompleteEvent
        If mIsUnload Then Return
        Dim dtl = e.CommandDetail
        Dim lSec = (dtl.EndTime - dtl.BeginTime).TotalMilliseconds()

        AddLog($"命令完成，耗时：{lSec:0.0} 毫秒")

    End Sub

    Private Sub Allocator_ConnectorClosedEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorClosedEvent
        If mIsUnload Then Return
        mConnectKey.Remove(connector.GetKey())
        AddLog($"连接已关闭  {connector.GetKey()} {GetConnectorDetail(connector)} ")
    End Sub

    Private Sub Allocator_ConnectorConnectedEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorConnectedEvent
        If mIsUnload Then Return
        Dim conn = Allocator.GetConnector(connector)
        conn.AddRequestHandle(obServer)
        mConnectKey.Add(connector.GetKey())
        AddLog($"已连接成功 {connector.GetKey()} {GetConnectorDetail(connector)} ")

    End Sub

    Private Sub Allocator_ConnectorErrorEvent(sender As Object, connector As INConnectorDetail) Handles Allocator.ConnectorErrorEvent
        If mIsUnload Then Return
        AddLog($"连接发生错误！  {GetConnectorDetail(connector)} {connector.GetError()} ")
    End Sub

    Private Function GetConnectorDetail(conn As INConnector) As String
        Return GetConnectorDetail(conn.GetConnectorDetail())
    End Function

    Private Function GetConnectorDetail(conn As INConnectorDetail) As String
        If mIsUnload Then Return String.Empty
        Dim oConn = Allocator.GetConnector(conn)
        If oConn Is Nothing Then Return conn?.ToString()

        Select Case conn.GetTypeName
            Case ConnectorType.TCPClient
                Dim dtl As TCPClient.TCPClientDetail_Readonly = oConn.GetConnectorDetail()

                Return $"R:{dtl.Addr}:{dtl.Port} L:{dtl.LocalAddr}:L:{dtl.LocalPort} T:{Now.ToTimeffff()}"
        End Select
        Return conn.ToString()
    End Function

    Private Sub obServer_DisposeRequestEvent(connector As INConnector, msgLen As Integer, msgHEX As String) Handles obServer.DisposeRequestEvent
        If mIsUnload Then Return
        AddLog($"{GetConnectorDetail(connector)} 接收  长度：{msgLen}  0x{msgHEX}")
    End Sub

    Private Sub obServer_DisposeResponseEvent(connector As INConnector, msgLen As Integer, msgHEX As String) Handles obServer.DisposeResponseEvent
        If mIsUnload Then Return
        AddLog($"{GetConnectorDetail(connector)} 发送 长度：{msgLen}  0x{msgHEX}")
    End Sub

    Private Sub obServer_OnRequestLog(connector As INConnector, msg As String) Handles obServer.OnRequestLog
        If mIsUnload Then Return
        AddLog($"{GetConnectorDetail(connector)}  {msg}")
    End Sub


    Private Sub AddLog(ByVal sTxt As String)
        If mIsUnload Then Return
        If Not chkShowLog.Checked Then Return
        Try
            If txtLog.InvokeRequired Then
                Invoke(New Action(Of String)(AddressOf AddLog), sTxt)
                Return
            End If
            txtLog.AppendText(sTxt + vbNewLine)
        Catch ex As Exception

        End Try


    End Sub

    Private Sub txtLog_DoubleClick(sender As Object, e As EventArgs) Handles txtLog.DoubleClick
        txtLog.Text = String.Empty
    End Sub

    Private Sub butConnect_Click(sender As Object, e As EventArgs) Handles butConnect.Click
        Dim oTCPDTL = GetTCPClientDetail()

        Allocator.OpenConnector(oTCPDTL)
    End Sub

    Private Sub butCloseTCP_Click(sender As Object, e As EventArgs) Handles butCloseTCP.Click
        Dim oTCPDTL = GetTCPClientDetail()
        Allocator.CloseConnector(oTCPDTL)
    End Sub

    Private Function GetTCPClientDetail() As TCPClient.TCPClientDetail
        Dim sRemoteIP, sLocalIP As String
        Dim iRemotePort, iLocalPort As Integer
        sRemoteIP = txtRemoteIP.Text
        sLocalIP = cmbLocalIP.Text
        iRemotePort = Integer.Parse(txtRemotePort.Text)
        iLocalPort = Integer.Parse(txtLocalPort.Text)
        If chkSSL.Checked Then
            Return New TCPClient.TCPClientDetail(sRemoteIP, iRemotePort, sLocalIP, iLocalPort, True, Nothing)
        Else
            Return New TCPClient.TCPClientDetail(sRemoteIP, iRemotePort, sLocalIP, iLocalPort)
        End If

    End Function

    Private Sub butTCPClientSend_Click(sender As Object, e As EventArgs) Handles butTCPClientSend.Click
        If String.IsNullOrEmpty(txtSendText.Text) Then
            Return
        End If
        Dim oTCPDTL = GetTCPClientDetail()
        Dim par = New Command.Text.TextCommandParameter(txtSendText.Text,
                                                        System.Text.Encoding.UTF8)
        Dim cmd = New Command.Text.TextCommand(New Text.TextCommandDetail(oTCPDTL), par)
        Allocator.AddCommand(cmd)
    End Sub

    Private Sub butTCPClientSend_200K_Click(sender As Object, e As EventArgs) Handles butTCPClientSend_200K.Click
        Dim ilen = 200 * 1024
        Dim buf = UnpooledByteBufferAllocator.Default.Buffer(ilen + 16)
        buf.WriteString("DDDDDDDDDD", System.Text.Encoding.ASCII)
        buf.WriteInt(ilen)

        For i = 1 To ilen
            buf.WriteByte(i Mod 255)
        Next
        Dim crc32 = MyCRC32.CalculateDigest(buf.Array, buf.ArrayOffset, ilen)
        buf.WriteInt(UInt32ToInt32(crc32))
        AddLog($"CRC32:{crc32:X}")
        Dim oTCPDTL = GetTCPClientDetail()

        Dim cmd As New Command.Byte.ByteCommand(New Command.Byte.ByteCommandDetail(oTCPDTL),
                                                New Command.Byte.ByteCommandParameter(buf))
        Allocator.AddCommand(cmd)
    End Sub

    ''' <summary>
    ''' Int64的最大值
    ''' </summary>
    Private _Int32Max As UInt32 = Integer.MaxValue
    ''' <summary>
    ''' 取低4位的掩码
    ''' </summary>
    Private _UInt32Mask As UInt32 = 15 '0x0F

    ''' <summary>
    ''' 将UInt32 转换为 Int32
    ''' </summary>
    ''' <param name="u">UInt64</param>
    ''' <returns>Int64</returns>
    Private Function UInt32ToInt32(ByVal u As UInt32) As Integer
        Dim l As Integer = 0
        If u > _Int32Max Then
            l = u >> 4
            l = l << 4
            u = u And _UInt32Mask
            l = l Xor u
        Else
            l = u
        End If
        Return l
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ilen = 200 * 1024
        Dim buf = UnpooledByteBufferAllocator.Default.Buffer(ilen)
        For i = 1 To ilen
            buf.WriteByte(i Mod 255)
        Next
        Dim crc32 = MyCRC32.CalculateDigest(buf.Array, buf.ArrayOffset, ilen)
        AddLog($"CRC32:{crc32:X}")

        crc32 = MyCRC32.CalculateDigest(buf.Array, buf.ArrayOffset, 1024)
        crc32 = MyCRC32.CalculateDigest_Continue(crc32, buf.Array, 1024, 65535)
        crc32 = MyCRC32.CalculateDigest_Continue(crc32, buf.Array, 66559, 65535)
        crc32 = MyCRC32.CalculateDigest_Continue(crc32, buf.Array, 132094, 65535)
        crc32 = MyCRC32.CalculateDigest_Continue(crc32, buf.Array, 197629, 7171)
        AddLog($"CRC32:{crc32:X}")

        buf.Release()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GC.Collect()
    End Sub


    Private mConnectKey As HashSet(Of String) = New HashSet(Of String)

    Private Sub butOpenConnectList_Click(sender As Object, e As EventArgs) Handles butOpenConnectList.Click
        Dim i As Integer
        Dim iCount As Integer = Integer.Parse(txtNewConnects.Text)

        Dim sKey As String


        For i = 1 To iCount
            Dim oTCPDTL = GetTCPClientDetail()

            sKey = $"Remote:{oTCPDTL.Addr}:{oTCPDTL.Port}_{Guid.NewGuid():N}"
            oTCPDTL.ConnectAlias = sKey

            Allocator.OpenConnector(oTCPDTL)
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sKey As String
        Dim sKeys = mConnectKey.ToArray()

        For Each sKey In sKeys
            Dim oTCPDTL = GetTCPClientDetail()
            oTCPDTL.ConnectAlias = sKey
            Allocator.CloseConnector(oTCPDTL)
        Next
        mConnectKey.Clear()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SendAllClient()
    End Sub

    Private Sub SendAllClient()
        If String.IsNullOrEmpty(txtSendText.Text) Then
            Return
        End If

        Dim sKey As String

        Dim sKeys = mConnectKey.ToArray()
        For Each sKey In sKeys
            Dim oTCPDTL = GetTCPClientDetail()
            oTCPDTL.ConnectAlias = sKey
            Dim par = New Command.Text.TextCommandParameter(txtSendText.Text,
                                                        System.Text.Encoding.UTF8)
            Dim cmd = New Command.Text.TextCommand(New Text.TextCommandDetail(oTCPDTL), par)
            Allocator.AddCommand(cmd)
        Next
    End Sub

    Private Sub tmrConnects_Tick(sender As Object, e As EventArgs) Handles tmrConnects.Tick
        txtConnected.Text = mConnectKey.Count.ToString()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If mSending = False Then
            Task.Run(AddressOf TaskSendAll)
            mSending = True
        Else
            mSending = False
        End If

    End Sub

    Private mSending As Boolean
    Private Sub TaskSendAll()
        Do

            Invoke(New Action(AddressOf SendAllClient))
            Thread.Sleep(500)
        Loop While mSending

    End Sub

End Class
