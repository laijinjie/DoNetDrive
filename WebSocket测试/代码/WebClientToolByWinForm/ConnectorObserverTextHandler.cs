using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using FCARDIO.Core.Command;
using FCARDIO.Core.Connector;

namespace WebClientToolByWinForm
{
    /// <summary>
    /// 文本观察者
    /// </summary>
    public class ConnectorObserverTextHandler : INRequestHandle
    {
        private IObserverDebug _Debug;
        private System.Text.Encoding TextEncoding;


        public ConnectorObserverTextHandler(IObserverDebug log, Encoding enc)
        {
            _Debug = log;
            TextEncoding = enc;
        }


        public void Dispose()
        {
            return;
        }
        /// <summary>
        /// 接收的数据
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="msg"></param>
        public void DisposeRequest(INConnector connector, IByteBuffer msg)
        {
            string sValue = msg.ReadString(msg.ReadableBytes, TextEncoding);

            _Debug.DisposeRequest(connector, sValue);
        }
        /// <summary>
        /// 发送的数据
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="msg"></param>
        public void DisposeResponse(INConnector connector, IByteBuffer msg)
        {
            string sValue = msg.ReadString(msg.ReadableBytes, TextEncoding);

            _Debug.DisposeResponse(connector, sValue);
        }
    }

}
