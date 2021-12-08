Namespace Connector
    ''' <summary>
    ''' 连接指示接口，用来告诉连接工厂应该使用哪种连接器连接对端。
    ''' </summary>
    Public Interface INConnectorDetail
        Inherits ICloneable, IEquatable(Of INConnectorDetail)
        ''' <summary>
        ''' 获取连接通道所在的程序集
        ''' 例如：DoNetDrive.Core
        ''' </summary>
        ''' <returns></returns>
        Function GetAssemblyName() As String

        ''' <summary>
        ''' 获取连接通道的类名
        ''' 例如：Connector.TCPClient.TCPClientConnector
        ''' </summary>
        ''' <returns></returns>
        Function GetTypeName() As String

        ''' <summary>
        ''' 连接器连接到对端时最大等待时间，单位毫秒
        ''' </summary>
        ''' <returns></returns>
        Property Timeout As Integer

        ''' <summary>
        ''' 连接器连接到对端失败后，最大重试次数
        ''' </summary>
        ''' <returns></returns>
        Property RestartCount As Integer

        ''' <summary>
        ''' 获取一个用于界定此通道的唯一Key值
        ''' </summary>
        ''' <returns></returns>
        Function GetKey() As String

        ''' <summary>
        ''' 可以设置一个错误信息，当错误发生时，保存在此处
        ''' </summary>
        ''' <param name="err"></param>
        Sub SetError(err As Exception)

        ''' <summary>
        ''' 当错误发生时，获取一个错误信息
        ''' </summary>
        ''' <returns></returns>
        Function GetError() As Exception

        ''' <summary>
        ''' 表示是否包含错误信息
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property IsFaulted As Boolean



        ''' <summary>
        ''' 当连接通道，连接建立成功后，会触发此函数回调
        ''' </summary>
        Property ConnectedCallBlack As Action(Of INConnectorDetail)

        ''' <summary>
        ''' 当连接通道关闭时，触发此函数
        ''' </summary>
        ''' <returns></returns>
        Property ClosedCallBlack As Action(Of INConnectorDetail)

        ''' <summary>
        ''' 当连接通道，连接建立失败后
        ''' </summary>
        Property ErrorCallBlack As Action(Of INConnectorDetail)

        ''' <summary>
        ''' 当通道为服务监听管道时生效，管道客户端离线时发生回调
        ''' </summary>
        ''' <returns></returns>
        Property ClientOfflineCallBlack As Action(Of INConnector)

        ''' <summary>
        ''' 当通道为服务监听管道时生效，管道新增客户端时发生回调
        ''' </summary>
        ''' <returns></returns>
        Property ClientOnlineCallBlack As Action(Of INConnector)

        ''' <summary>
        ''' 通道保活时间，超过指定时间未收到任意数据包则发送一个保活包,单位秒
        ''' </summary>
        ''' <returns></returns>
        Property KeepaliveTime As Integer
    End Interface

End Namespace
