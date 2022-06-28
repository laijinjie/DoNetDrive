# DoNetDrive.Connector.SerialPort

## 介绍

实现基于串口的命令收发功能，可用于RS232/485通讯


## 软件架构
基于 netstandard2.0 ；


## 使用说明

~~~ vb
 Dim defFactory As DefaultConnectorFactory = TryCast(Allocator.ConnectorFactory, DefaultConnectorFactory)
 defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance())
~~~



## 版本记录

### ver 1.0.0

DoNetDrive.Core 通讯库中的串口通讯实现，当需要使用串口通讯时，需要引用这个动态库，并将这个库和DoNetDrive.Core中的分配器关联 VB.NET代码 Dim defFactory As DefaultConnectorFactory = TryCast(Allocator.ConnectorFactory, DefaultConnectorFactory) defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance())


### ver 1.01.0

增加命令发送间隔，使通道的 CommandSendIntervalTimeMS 属性可以作用到每一次发

### ver 1.02.0

增加命令发送间隔


### ver 1.03.0
修复上个版本命令发送间隔计算错误的问题
修改串口不能关闭的bug
