Namespace Connector
    ''' <summary>
    ''' 通讯连接器详情基类基本定义
    ''' </summary>
    Public MustInherit Class AbstractConnectorDetail
        Implements INConnectorDetail

        ''' <summary>
        ''' 初始化默认的连接等待时间和重试次数
        ''' </summary>
        Sub New()
            Timeout = 5000 '默认值是5秒
            RestartCount = 2 '默认重试2次
        End Sub

        ''' <summary>
        ''' 连接器连接到对端时最大等待时间，单位毫秒
        ''' </summary>
        ''' <returns></returns>
        Public Property Timeout As Integer Implements INConnectorDetail.Timeout

        ''' <summary>
        ''' 连接器连接到对端失败后，最大重试次数
        ''' </summary>
        ''' <returns></returns>
        Public Property RestartCount As Integer Implements INConnectorDetail.RestartCount

        ''' <summary>
        ''' 获取连接通道所在的程序集
        ''' 例如：DoNetDrive.Core
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetAssemblyName() As String Implements INConnectorDetail.GetAssemblyName


        ''' <summary>
        ''' 获取连接通道的类名
        ''' 例如：Connector.TCPClient.TCPClientConnector
        ''' </summary>
        ''' <returns></returns>
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


    End Class
End Namespace

