Imports DoNetDrive.Core.Connector

Public Interface IConnecterManage
    ''' <summary>
    ''' 根据键获取一个连接管道
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    Function GetConnector(sKey As String) As INConnector

    ''' <summary>
    ''' 获取所有连接管道的键
    ''' </summary>
    ''' <returns></returns>
    Function GetAllConnectorKeys() As List(Of String)

    ''' <summary>
    ''' 根据键删除一个连接管道
    ''' </summary>
    ''' <param name="sKey"></param>
    ''' <param name="oConn"></param>
    Sub RemoveConnector(ByVal sKey As String, oConn As INConnector)


    ''' <summary>
    ''' 添加一个连接通道
    ''' </summary>
    ''' <returns></returns>
    Function AddConnector(ByVal sKey As String, conn As INConnector) As Boolean

End Interface