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
