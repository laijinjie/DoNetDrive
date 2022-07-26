# DoNetDrive.Core

## 介绍

用于运行设备命令的基础库，依赖于 Nettty.Buffer ，包含TCP Client 、TCP Server 、UDP  的通讯实现，抽象设备命令和执行步骤。

封装命令收发的核心逻辑


## 软件架构
基于 netstandard2.0 ；


## 依赖于此动态库的项目

1.  DoNetDrive.Protocol  设备协议的抽象
2.  DoNetDrive.Connector.SerialPort 串口通讯的实现，可用于RS232/485通讯
3.  DoNetDrive.Protocol.Door  门禁控制板命令库
4.  DoNetDrive.Protocol.Fingerprint 指纹机/人脸机命令库
5.  DoNetDrive.Protocol.Fingerprint.Elevator 指纹机/人脸机附加电梯功能命令库
6.  DoNetDrive.Protocol.Elevator 电梯控制板命令库
7.  DoNetDrive.Protocol.POS 消费机命令库
8.  DoNetDrive.Protocol.USB.OfflinePatrol USB巡更棒命令库
9.  DoNetDrive.Protocol.USB.CardReader USB发卡器命令库

## 使用说明

~~~ c#
var mAllocator = ConnectorAllocator.GetAllocator();



var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.TCPClient, "192.168.1.56", 8000,
                CommandDetailFactory.ControllerType.Door88, "0000000000000000", "FFFFFFFF");
ReadSN cmd = new ReadSN(cmdDtl);
try
{
    await mAllocator.AddCommandAsync(cmd);
    var snResult = cmd.getResult() as SN_Result;
    Console.WriteLine(snResult.SNBuf);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

//释放
mAllocator.Dispose();
~~~


### UDP Server 绑定端口使用示例

~~~ c#
        private async void btnBindPort_Click(object sender, EventArgs e)
        {
            UDPServerDetail serverDetail;
            string sLocalIP = cmbLocalIP.Text;
            int iLocalPort = (int)txtLocalPort.Value;

            if (string.IsNullOrWhiteSpace(sLocalIP))
            {
                serverDetail = new UDPServerDetail(iLocalPort);
            }
            else
            {
                if (sLocalIP.Equals("*"))
                    serverDetail = new UDPServerDetail(iLocalPort);
                else
                    serverDetail = new UDPServerDetail(sLocalIP, iLocalPort);
            }



            //绑定处理事件
            serverDetail.ClientOfflineCallBlack = UDPServer_ClientOfflineCallBlack;
            serverDetail.ClientOnlineCallBlack = UDPServer_ClientOnlineCallBlack;
            serverDetail.ClosedCallBlack = UDPServer_ClosedCallBlack;
            //serverDetail.ConnectedCallBlack= UDPServer_ConnectedCallBlack;
            try
            {
                var connector = await Allocator.OpenConnectorAsync(serverDetail);

                AddLog("绑定成功：" + serverDetail.GetKey());
            }
            catch (Exception ex)
            {

                AddLog("绑定错误：" + ex.Message);
                ShowErr("绑定错误：" + ex.Message);
            }

        }


        private void UDPServer_ClientOnlineCallBlack(INConnector client)
        {
            AddLog("客户端上线：" + client.GetConnectorDetail().ToString());
            client.AddRequestHandle(new ConnectorObserverTextHandler(
                new ObserverTextDebug(
                    read: UDPClient_ReadCallBlack,
                    send: null
                    ),
                System.Text.Encoding.UTF8));
        }

        /// <summary>
        /// udp客户端接收到数据的回调
        /// </summary>
        /// <param name="client"></param>
        private void UDPClient_ReadCallBlack(INConnector client,string msg)
        {
            AddLog("收到数据：" + client.GetConnectorDetail().ToString() + "\r\n" + msg);
        }



        private void UDPServer_ClientOfflineCallBlack(INConnector client)
        {
            AddLog("客户端离线：" + client.GetConnectorDetail().ToString());
        }

        private void UDPServer_ConnectedCallBlack(INConnectorDetail connectorDetail)
        {
            AddLog("连接建立：" + connectorDetail.ToString());
        }

        /// <summary>
        /// 通道关闭时的回调
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void UDPServer_ClosedCallBlack(INConnectorDetail connDtl)
        {
            AddLog("连接关闭：" + connDtl.ToString());
        }
~~~



## 版本记录

### ver 2.01.0

增加 async 和 await 的方式操作命令


### ver 2.02.0

改进了上个版本使用async 和 await方式时，当连接建立成功，但是发送命令后没有响应，命令超时时，不能正确返回，导致await一直等待的bug

### ver 2.03.0

1.  删除了 SerialPort 减少了依赖库，如果要使用串口通讯，需要额外添加 DoNetDrive.Connector.SerialPort.dll ，并再分配器的连接器创建工厂中增加SerialPort

代码如下：
VB.Net

```VB
Dim defFactory As DefaultConnectorFactory = TryCast(Allocator.ConnectorFactory, DefaultConnectorFactory)
defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance())
```
2.   INConnectorDetail 类 增加了 ConnectingCallBlack 回调，再TCPClient 时，发起连接时会调用
3.  ConnectorAllocator 增加了ConnectorConnectingEvent 事件，再TCPClient 时，发起连接时会调用


## ver 2.04.0

解决UDP通讯时由于对方决绝会导致udp的socket套接字意外关闭导致发送数据出错的问题
修复 CommandDetailFactory.vb 中没有移除 串口相关类型导致报错的问题
增加 CommandStatusExtension.vb ，增加判断命令是否成功完成的扩展函数

## ver 2.05.0
修改命令发送后的等待发送缓冲区成功的逻辑

## ver 2.06.0
修改设置最后发送命令时间的时机

## ver 2.08.0

#### UDPServerClientConnector 
    1. 增加连接建立事件
	2. ConnectAsync 改为可调用，但不执行任何操作
	3. CloseAsync 调用时增加触发ClientOffline回调
	
#### UDPServerConnector
    1. Bind端口时增加支持IPV6
	2. 等待接收数据报时增加出错检测及自动重试，重试次数为100，重试失败后将关闭连接
	3. 接收到数据报后将数据报发送到 本地IP:本地端口号:远程IP:远程端口号 4个组件组成的通道
	4. 接收到数据报后尝试查询本地的广播通道，将数据报转发到广播通道。
	5. 接收到数据报创建子通道时将ClosedCallBlack和ErrorCallBlack委托传递到子通道。


## ver 2.09.0

 1. 修复在单独一个代码块中为每个命令都添加 using ，或者单独调用 cmd.Dispose() 会引发阻塞的bug，
 2. 如下面代码块在第二次 await Allocator.AddCommandAsync(cmd); 会引发阻塞。
 3. 现在已对这种情况进行了修复，不会再发生阻塞了。
~~~ c#

try
{
    using var par = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
        .WriteKeepAliveInterval_Parameter(20);

    using var cmd = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
    .WriteKeepAliveInterval(cmdDtl, par);
    await Allocator.AddCommandAsync(cmd);


}
catch (Exception ex)
{

    return;
}
try
{
    using var par = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
        .WriteKeepAliveInterval_Parameter(20);

    using var cmd = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
    .WriteKeepAliveInterval(cmdDtl, par);
    
    await Allocator.AddCommandAsync(cmd);

}
catch (Exception ex)
{

    return;
}

~~~

