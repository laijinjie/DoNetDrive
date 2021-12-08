Namespace Connector
    ''' <summary>
    ''' 通讯连接器详情基类基本定义
    ''' </summary>
    Public MustInherit Class AbstractConnectorDetail
        Implements INConnectorDetail

        Public Property ConnectedCallBlack As Action(Of INConnectorDetail) Implements INConnectorDetail.ConnectedCallBlack
        Public Property ClosedCallBlack As Action(Of INConnectorDetail) Implements INConnectorDetail.ClosedCallBlack
        Public Property ErrorCallBlack As Action(Of INConnectorDetail) Implements INConnectorDetail.ErrorCallBlack

        Public Property ClientOnlineCallBlack As Action(Of INConnector) Implements INConnectorDetail.ClientOnlineCallBlack
        Public Property ClientOfflineCallBlack As Action(Of INConnector) Implements INConnectorDetail.ClientOfflineCallBlack

        Public Property KeepaliveTime As Integer = 30 Implements INConnectorDetail.KeepaliveTime

        Public Property Timeout As Integer Implements INConnectorDetail.Timeout

        Public Property RestartCount As Integer Implements INConnectorDetail.RestartCount

        Public MustOverride Function GetAssemblyName() As String Implements INConnectorDetail.GetAssemblyName

        Public MustOverride Function GetTypeName() As String Implements INConnectorDetail.GetTypeName

        ''' <summary>
        ''' 复制当前通道信息的浅表副本
        ''' </summary>
        ''' <returns>表示连接通道信息的副本</returns>
        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

        ''' <summary>
        ''' 用来比较此连接通道是否为同一个
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public MustOverride Overloads Function Equals(other As INConnectorDetail) As Boolean Implements IEquatable(Of INConnectorDetail).Equals

        ''' <summary>
        ''' 获取一个用于界定此通道的唯一Key值
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetKey() As String Implements INConnectorDetail.GetKey

#Region "错误"
        ''' <summary>
        ''' 一个异常报告
        ''' </summary>
        Protected _Exception As Exception

        Public ReadOnly Property IsFaulted As Boolean Implements INConnectorDetail.IsFaulted
            Get
                Return _Exception IsNot Nothing
            End Get
        End Property

        Public Sub SetError(err As Exception) Implements INConnectorDetail.SetError
            _Exception = err
        End Sub

        Public Function GetError() As Exception Implements INConnectorDetail.GetError
            Return _Exception
        End Function
#End Region
    End Class
End Namespace

