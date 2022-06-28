## UDP 服务器示例

~~~c#
using DoNetDrive.Core;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.UDP;
using System.Text.Json;

// 获取连接分配器
var Allocator = ConnectorAllocator.GetAllocator();

//读取配置文件
var sConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
var UDPConfig = JsonSerializer.Deserialize<UDPServerSetting>(File.ReadAllText(sConfigFile));

#region 启动UDP服务器
UDPServerDetail serverDetail = new UDPServerDetail(UDPConfig.LocalIP, UDPConfig.LocalPort);

try
{
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 准备启动 UDP服务,服务器信息:UDP {IotConfig.LocalIP}:{IotConfig.LocalPort}....");

    Allocator.ClientOnline += (client, arg) =>
    (client as INConnector).AddRequestHandle(new CommandHandle());
    var UDPServer = await Allocator.OpenForciblyConnectAsync(serverDetail);
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 服务器已启动,服务器信息:UDP {IotConfig.LocalIP}:{IotConfig.LocalPort}，等待连接....");

}
catch (Exception ex)
{
    Console.WriteLine("本地UDP绑定失败！");
    return;
}
#endregion

Thread.Sleep(-1);
~~~



~~~ c#
    public class UDPServerSetting
    {
        /// <summary>
        /// 本地IP
        /// </summary>
        public string LocalIP { get; set; }
        /// <summary>
        /// 本地端口号
        /// </summary>
        public int LocalPort { get; set; }
    }
~~~





~~~ C#
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	public class CommandHandle : INRequestHandle
    {
        private static byte[] SendOKBuff = Encoding.ASCII.GetBytes("OK");

        private IByteBuffer SendOKByteBuffer;

        public CommandHandle()
        {
            SendOKByteBuffer = Unpooled.UnreleasableBuffer(Unpooled.WrappedBuffer(SendOKBuff));
        }

        public void Dispose()
        {
            return;
        }


        //接收到数据
        public void DisposeRequest(INConnector connector, IByteBuffer msg)
        {
            int ilen = msg.ReadableBytes;//接收到的数据长度

            //转为字符串
            string sMsg = System.Text.Encoding.ASCII.GetString(msg.Array, msg.ArrayOffset, ilen);
            Console.WriteLine($"接收到数据：{sMsg}");

            connector.WriteByteBuf(SendOKByteBuffer);

        }

        //发送的数据
        public void DisposeResponse(INConnector connector, IByteBuffer msg)
        {
            return;
        }
    }
~~~

![image-20220510092814801](C:\Users\kaifa\AppData\Roaming\Typora\typora-user-images\image-20220510092814801.png)
