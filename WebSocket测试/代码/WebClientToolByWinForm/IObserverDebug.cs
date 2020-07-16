using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCARDIO.Core.Connector;

namespace WebClientToolByWinForm
{
    /// <summary>
    /// 观察者输出
    /// </summary>
    public interface IObserverDebug
    {
        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="msg"></param>
        void DisposeRequest(INConnector connector, string msg);
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="msg"></param>
        void DisposeResponse(INConnector connector, string msg);
    }
}
