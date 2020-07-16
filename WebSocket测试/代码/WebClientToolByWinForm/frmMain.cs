using FCARD.Common.Extensions;
using FCARDIO.Core;
using FCARDIO.Core.Command;
using FCARDIO.Core.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCARDIO.Core.Connector.WebSocket.Server;
using FCARDIO.Core.Connector.WebSocket.Server.Client;
using FCARDIO.Core.Command.Text;
using DotNetty.Buffers;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using WebClientToolByWinForm.Properties;

namespace WebClientToolByWinForm
{
    public partial class frmMain : Form, IObserverDebug
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverTextHandler mObserver;




        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 退出标记
        /// </summary>
        bool _IsClosed;

        private void frmMain_Load(object sender, EventArgs e)
        {
            _IsClosed = false;
            Task.Run(() =>
            {
                Sleep(100);
                Invoke(new Action(IniForm));

            });

        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        #region 窗体关闭
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IsClosed = true;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //释放资源
            mAllocator.Dispose();
            Sleep(500);
        }
        #endregion


        /// <summary>
        /// 窗体初始化
        /// </summary>
        public void IniForm()
        {
            if (_IsClosed) return;

            mAllocator = ConnectorAllocator.GetAllocator();
            mObserver = new ConnectorObserverTextHandler(this, Encoding.UTF8);

            mAllocator.CommandCompleteEvent += mAllocator_CommandCompleteEvent;
            mAllocator.ConnectorConnectedEvent += mAllocator_ConnectorConnectedEvent;
            mAllocator.ConnectorClosedEvent += mAllocator_ConnectorClosedEvent;
            mAllocator.ConnectorErrorEvent += mAllocator_ConnectorErrorEvent;

            mAllocator.ClientOnline += MAllocator_ClientOnline;
            mAllocator.ClientOffline += MAllocator_ClientOffline;


            TCPServerClients = new Dictionary<string, WebSocketClientDetail_Item>();

        }


        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            string cName = e.Command.GetType().FullName;
            TextCommandParameter par = e.Command.Parameter as TextCommandParameter;

            AddIOLog(e.CommandDetail.Connector, "发送消息完成！", par.Text);
        }


        #region 通道事件
        /// <summary>
        /// 客户端离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://TCP客户端已连接
                    RemoveTCPServer_Client(inc.GetConnectorDetail());
                    break;
                case ConnectorType.UDPClient://UDP客户端已连接
                    //RemoveUDPClient(inc.GetConnectorDetail());
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// 客户端上线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            switch (inc.GetConnectorType())
            {
                case ConnectorType.WebSocketServerClient://TCP客户端已连接
                    AddTCPServer_Client(inc.GetConnectorDetail());
                    break;
                default:
                    break;
            }
        }


        #region "通讯日志"
        private bool mShowIOEvent = true;
        private void chkShowIO_CheckedChanged(object sender, EventArgs e)
        {
            mShowIOEvent = chkShowIO.Checked;
        }

        private void AddIOLog(INConnectorDetail connDetail, string head, string Txt)
        {
            if (!mShowIOEvent) return;
            if (txtLog.InvokeRequired)
            {
                Invoke(() => AddIOLog(connDetail, head, Txt));
                return;
            }
            if (connDetail == null)
            {
                string sLog = $"{ head} { DateTime.Now.ToString("HH:mm:ss")} : {Txt} {Environment.NewLine}";
                txtLog.AppendText(sLog);
            }
            else
            {
                string Local, Remote, cType;
                GetConnectorDetail(connDetail, out cType, out Local, out Remote);
                string sLog;
                switch (connDetail.GetTypeName())
                {
                    case ConnectorType.WebSocketServerClient:
                        sLog = $"{ head} { DateTime.Now.ToString("HH:mm:ss")} 本地信息 { Local} 远程信息 { Remote} : {Txt} {Environment.NewLine}";
                        break;
                    case ConnectorType.WebSocketServer:
                        sLog = $"{ head} { DateTime.Now.ToString("HH:mm:ss")} 本地信息 { Local} : {Txt} {Environment.NewLine}";
                        break;

                    default:
                        sLog = $"{ head} { DateTime.Now.ToString("HH:mm:ss")} 本地信息 { Local} 远程信息 { Remote} : {Txt} {Environment.NewLine}";
                        break;
                }

                
                txtLog.AppendText(sLog);
            }

        }

        /// <summary>
        /// 获取连接通道详情
        /// </summary>
        /// <param name="conn">连接通道描述符</param>
        /// <param name="Local">返回描述本地信息</param>
        /// <param name="Remote">返回描述远程信息</param>
        /// <returns></returns>
        private void GetConnectorDetail(INConnectorDetail conn, out string cType, out string Local, out string Remote)
        {
            Local = string.Empty;
            Remote = string.Empty;
            cType = string.Empty;
            IPDetail local;


            var wsd = conn as WebSocketServerDetail;
            if (wsd == null) return;

            local = new IPDetail(wsd.LocalAddr, wsd.LocalPort);


            

            switch (conn.GetTypeName())
            {
                case ConnectorType.WebSocketServerClient:
                    cType = "WebSocket客户端节点";
                    var clientdtl = conn as WebSocketServerClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{clientdtl.Remote.ToString()}";
                    break;
                case ConnectorType.WebSocketServer:
                    cType = "WebSocket服务器";
                    var serverdtl = conn as WebSocketServerDetail;

                    Local = $"{local.ToString()}，WebSocket路径：{serverdtl.WebsocketPath}";
                    break;

                default:
                    cType = conn.GetTypeName();
                    Local = $"{conn.GetKey()}";
                    break;
            }
        }


        /// <summary>
        /// 清空所有通讯日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butClear_Click(object sender, EventArgs e)
        {

            txtLog.Text = string.Empty;
        }

        void IObserverDebug.DisposeResponse(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), "发送数据", msg);
        }


        void IObserverDebug.DisposeRequest(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), "接收数据", msg);
        }
        #endregion

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.WebSocketServer://TCP Server 服务器
                    Invoke(() => WebSocketServerBindOver(false));
                    AddIOLog(connector, "WebSocket服务", "WebSocket服务器开启失败");
                    break;
                default:
                    AddIOLog(connector, "错误", "连接失败");
                    break;
            }


        }


        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.WebSocketServer://TCP Server 服务器
                    Invoke(() => WebSocketServerBindOver(false));
                    AddIOLog(connector, "WebSocket服务", "WebSocket服务已关闭");
                    break;
                default:
                    AddIOLog(connector, "关闭", "连接通道已关闭");
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.WebSocketServer://TCP Server 服务器
                    Invoke(() => WebSocketServerBindOver(true));
                    AddIOLog(connector, "WebSocket服务", "WebSocket服务已启动");
                    break;
                default:
                    mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

                    AddIOLog(connector, "成功", "通道连接成功");
                    break;
            }
        }


        #endregion

        #region 提示框
        public void MsgTip(string sText)
        {
            MessageBox.Show(sText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        private void Invoke(Action p)
        {
            try
            {
                Invoke((Delegate)p);
            }
            catch (Exception)
            {

                return;
            }

        }

        #region TCP 服务器
        /// <summary>
        /// WebSocket是否已绑定
        /// </summary>
        private bool mWebSocketServerBind;

        /// <summary>
        /// 包含所有客户端的项
        /// </summary>
        private Dictionary<string, WebSocketClientDetail_Item> TCPServerClients;

        /// <summary>
        /// 保存TCP客户端的详情
        /// </summary>
        private class WebSocketClientDetail_Item
        {
            /// <summary>
            /// 表示客户端的唯一Key
            /// </summary>
            public string Key;

            /// <summary>
            /// 客户端本地IP
            /// </summary>
            public IPDetail Local;

            /// <summary>
            /// 客户端的远程IP
            /// </summary>
            public IPDetail Remote;

            public WebSocketClientDetail_Item(WebSocketServerClientDetail detail)
            {
                Key = detail.Key;
                Remote = new IPDetail(detail.Remote.Addr, detail.Remote.Port);
                Local = new IPDetail(detail.Local.Addr, detail.Local.Port);
            }

            public override string ToString()
            {

                return $"远程:{Remote.Addr}:{Remote.Port}";


            }
        }

        private void butBeginTCPServer_Click(object sender, EventArgs e)
        {
            if (!txtServerPort.Text.IsNum())
            {
                MsgErr("端口号不正确！");
                return;
            }
            int port = txtServerPort.Text.ToInt32();
            string sLocalIP = "192.168.1.124";

            WebSocketServerDetail detail;
            if (mWebSocketServerBind)
            {
                detail = new WebSocketServerDetail(sLocalIP, port);

                //关闭WebSocket服务器
                mAllocator.CloseConnector(detail);
                butBeginTCPServer.Text = "开启服务";
                mWebSocketServerBind = false;
                txtServerPort.Enabled = true;
            }
            else
            {


                detail = new WebSocketServerDetail(sLocalIP, port);
                butBeginTCPServer.Enabled = false;
                mWebSocketServerBind = true;
                txtServerPort.Enabled = false;

                //打开WebSocket服务器
                mAllocator.OpenConnector(detail);

                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败


            }

        }
        /// <summary>
        /// WebSocket绑定完毕
        /// </summary>
        /// <param name="bind">true 表示绑定成功</param>
        private void WebSocketServerBindOver(bool bind)
        {
            if (bind)
            {
                txtServer.Text = $"ws://127.0.0.1:{txtServerPort.Text}/WebSocket";
                butBeginTCPServer.Text = "关闭服务";
            }
            else
            {
                txtServer.Text = "已关闭服务";
                mWebSocketServerBind = false;
                txtServerPort.Enabled = true;
            }

            butBeginTCPServer.Enabled = true;
        }

        /// <summary>
        /// 关闭一个已连接的TCP连接通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCloseTCPClient_Click(object sender, EventArgs e)
        {
            WebSocketClientDetail_Item oItem = cmbTCPClient.SelectedItem as WebSocketClientDetail_Item;
            if (oItem == null)
            {
                MsgErr("请选择一个客户端！");
                return;
            }

            WebSocketServerClientDetail detail = new WebSocketServerClientDetail(oItem.Key);
            mAllocator.CloseConnector(detail);

        }

        private void butSendMessage_Click(object sender, EventArgs e)
        {
            WebSocketClientDetail_Item oItem = cmbTCPClient.SelectedItem as WebSocketClientDetail_Item;
            if (oItem == null)
            {
                MsgErr("请选择一个客户端！");
                return;
            }

            WebSocketServerClientDetail WebSocketdtl = new WebSocketServerClientDetail(oItem.Key);
            FCARDIO.Core.Command.Text.TextCommandDetail dtl = new FCARDIO.Core.Command.Text.TextCommandDetail(WebSocketdtl);
            FCARDIO.Core.Command.Text.TextCommandParameter par = new FCARDIO.Core.Command.Text.TextCommandParameter("Server Hello");
            FCARDIO.Core.Command.Text.TextCommand oMsg = new FCARDIO.Core.Command.Text.TextCommand(dtl, par);
            mAllocator.AddCommand(oMsg);
        }


        /// <summary>
        /// 将客户端添加到列表中
        /// </summary>
        /// <param name="detail"></param>
        private void AddTCPServer_Client(INConnectorDetail detail)
        {
            if (cmbTCPClient.InvokeRequired)
            {
                Invoke(() => AddTCPServer_Client(detail));
                return;
            }
            WebSocketServerClientDetail oClient = detail as WebSocketServerClientDetail;
            var oItem = new WebSocketClientDetail_Item(oClient);

            cmbTCPClient.Items.Add(oItem);
            cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1;
            TCPServerClients.Add(oItem.Key, oItem);

            AddIOLog(detail, "上线", "Websocket 客户端已上线");
        }

        /// <summary>
        /// 从列表中删除TCP客户端
        /// </summary>
        /// <param name="detail"></param>
        private void RemoveTCPServer_Client(INConnectorDetail detail)
        {
            if (cmbTCPClient.InvokeRequired)
            {
                Invoke(() => RemoveTCPServer_Client(detail));
                return;
            }
            WebSocketServerClientDetail oClient = detail as WebSocketServerClientDetail;

            if (!TCPServerClients.ContainsKey(oClient.Key)) return;

            var oItem = TCPServerClients[oClient.Key];
            cmbTCPClient.Items.Remove(oItem);
            cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1;
            TCPServerClients.Remove(oItem.Key);
            AddIOLog(detail, "离线", "Websocket 客户端已离线");
        }


        #endregion


    }
}
