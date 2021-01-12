Namespace Connector
    ''' <summary>
    ''' 连接器状态--空闲
    ''' </summary>
    Public MustInherit Class ConnectorStatus_Free
        Implements INConnectorStatus

        Public Overridable Function Status() As String Implements INConnectorStatus.Status
            Return "Free"
        End Function

        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            connector.CheckIsInvalid()
            If (Not connector.IsInvalid) Then
                OpenConnector(connector)
            Else
                connector.CheckIsInvalid()
            End If
        End Sub

        ''' <summary>
        ''' 打开连接器
        ''' </summary>
        ''' <param name="connector"></param>
        Protected MustOverride Sub OpenConnector(connector As INConnector)

    End Class


    ''' <summary>
    ''' 连接器状态--正在连接
    ''' </summary>
    Public MustInherit Class ConnectorStatus_Connecting
        Implements INConnectorStatus

        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connecting"
        End Function

        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            connector?.UpdateActivityTime()
            CheckConnectingStatus(connector)
        End Sub

        ''' <summary>
        ''' 检查连接状态
        ''' </summary>
        ''' <param name="connector"></param>
        Protected MustOverride Sub CheckConnectingStatus(connector As INConnector)

    End Class

    ''' <summary>
    ''' 连接器状态--已连接
    ''' </summary>
    Public MustInherit Class ConnectorStatus_Connected
        Implements INConnectorStatus
        ''' <summary>
        ''' 连接建立完毕后，每次检查连接器状态的间隔时间
        ''' 默认为5毫秒
        ''' </summary>
        Public Shared TimeInterval As TimeSpan = New TimeSpan(0, 0, 0, 0, 10)
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Connected"
        End Function


        Public Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            connector.CheckIsInvalid()
            If Not connector.IsInvalid Then
                CheckCommandList(connector)
                'connector?.GetEventLoop()?.Schedule(connector, TimeInterval) '加入事件循环
            Else
                CloseConnector(connector)
            End If
        End Sub


        ''' <summary>
        ''' 检查命令队列中是否有需要发送的命令
        ''' </summary>
        ''' <param name="connector"></param>
        Protected MustOverride Sub CheckCommandList(connector As INConnector)

        ''' <summary>
        ''' 无命令也无需占用，通道需要关闭
        ''' </summary>
        ''' <param name="connector"></param>
        Protected MustOverride Sub CloseConnector(connector As INConnector)
    End Class
End Namespace

