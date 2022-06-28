# DoNetDrive.Protocol

## 介绍

用于设备命令的协议抽象层，定义了各种产品的协议格式及编解码器


## 软件架构
基于 netstandard2.0 ；


## 依赖于此动态库的项目


1.  DoNetDrive.Protocol.Door  门禁控制板命令库
2.  DoNetDrive.Protocol.Fingerprint 指纹机/人脸机命令库
3.  DoNetDrive.Protocol.Fingerprint.Elevator 指纹机/人脸机附加电梯功能命令库
4.  DoNetDrive.Protocol.Elevator 电梯控制板命令库
5.  DoNetDrive.Protocol.POS 消费机命令库
6.  DoNetDrive.Protocol.USB.OfflinePatrol USB巡更棒命令库
7.  DoNetDrive.Protocol.USB.CardReader USB发卡器命令库

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


### ver 2.02.0
CommandDetailFactory 类删除了对SerialPort的支持，增加一个  CreateDetail(INConnectorDetail, ControllerType , string , string ) 函数，可以由外部创建INConnectorDetail 后，再组装 INCommandDetail。

### ver 2.03.0
修改封装的人脸机、控制板命令处理流程，当出现命令校验错误时，不再判断命令标识，立刻将当前命令重发一次。