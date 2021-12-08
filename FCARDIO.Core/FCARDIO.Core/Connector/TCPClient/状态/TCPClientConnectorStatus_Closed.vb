Imports DoNetDrive.Core.Connector.TCPClient

Namespace Connector.TCPClient

    ''' <summary>
    ''' 连接器状态--已关闭
    ''' </summary>
    Public Class TCPClientConnectorStatus_Closed
        Implements INConnectorStatus
        Public Function Status() As String Implements INConnectorStatus.Status
            Return "Closed"
        End Function


        Public Overridable Sub CheckStatus(connector As INConnector) Implements INConnectorStatus.CheckStatus
            If Not connector.CheckIsInvalid() Then
                Dim Client As TCPClientConnector = connector
                connector.ConnectAsync().ContinueWith(AddressOf Client.ConnectingNext)
            End If
        End Sub


    End Class

End Namespace
