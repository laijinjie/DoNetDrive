Namespace Connector

    Public Class ConnectorStatus

        ''' <summary>
        ''' 指示通道已失效，等待销毁
        ''' </summary>
        Public Shared ReadOnly Invalid As ConnectorStatus_Invalid = New ConnectorStatus_Invalid()


        ''' <summary>
        ''' 指示服务器通道已绑定端口
        ''' </summary>
        Public Shared ReadOnly Bind As ConnectorStatus_Bind = New ConnectorStatus_Bind()

        ''' <summary>
        ''' 指示通道已关闭
        ''' </summary>
        Public Shared ReadOnly Closed As ConnectorStatus_Closed = New ConnectorStatus_Closed()

    End Class

End Namespace

