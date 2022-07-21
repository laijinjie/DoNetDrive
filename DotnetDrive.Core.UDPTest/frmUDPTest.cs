using DoNetDrive.Core;
using DoNetDrive.Core.Command.Text;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.UDP;
using System.Net;
using System.Net.Sockets;

namespace DotnetDrive.Core.UDPTest
{
    public partial class frmUDPTest : Form
    {
        private ConnectorAllocator Allocator;
        private bool IsRelease;
        public frmUDPTest()
        {
            IsRelease = false;
            Allocator = ConnectorAllocator.GetAllocator();

            InitializeComponent();
            IniLocalIP();
        }

        public void IniLocalIP()
        {
            cmbLocalIP.Items.Clear();
            cmbLocalIP.Items.Add("*");
            cmbLocalIP.Items.Add("127.0.0.1");
            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ip in addr)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    cmbLocalIP.Items.Add(ip.ToString());
                }
            }
        }

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



            //�󶨴����¼�
            serverDetail.ClientOfflineCallBlack = UDPServer_ClientOfflineCallBlack;
            serverDetail.ClientOnlineCallBlack = UDPServer_ClientOnlineCallBlack;
            serverDetail.ClosedCallBlack = UDPServer_ClosedCallBlack;
            //serverDetail.ConnectedCallBlack= UDPServer_ConnectedCallBlack;
            try
            {
                var connector = await Allocator.OpenConnectorAsync(serverDetail);

                AddLog("�󶨳ɹ���" + serverDetail.GetKey());
            }
            catch (Exception ex)
            {

                AddLog("�󶨴���" + ex.Message);
                ShowErr("�󶨴���" + ex.Message);
            }

        }


        private void UDPServer_ClientOnlineCallBlack(INConnector client)
        {
            AddLog("�ͻ������ߣ�" + client.GetConnectorDetail().ToString());
            client.AddRequestHandle(new ConnectorObserverTextHandler(
                new ObserverTextDebug(
                    read: UDPClient_ReadCallBlack,
                    send: null
                    ),
                System.Text.Encoding.UTF8));
        }

        /// <summary>
        /// udp�ͻ��˽��յ����ݵĻص�
        /// </summary>
        /// <param name="client"></param>
        private void UDPClient_ReadCallBlack(INConnector client,string msg)
        {
            AddLog("�յ����ݣ�" + client.GetConnectorDetail().ToString() + "\r\n" + msg);
        }



        private void UDPServer_ClientOfflineCallBlack(INConnector client)
        {
            AddLog("�ͻ������ߣ�" + client.GetConnectorDetail().ToString());
        }

        private void UDPServer_ConnectedCallBlack(INConnectorDetail connectorDetail)
        {
            AddLog("���ӽ�����" + connectorDetail.ToString());
        }

        /// <summary>
        /// ͨ���ر�ʱ�Ļص�
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void UDPServer_ClosedCallBlack(INConnectorDetail connDtl)
        {
            AddLog("���ӹرգ�" + connDtl.ToString());
        }

        private async void frmUDPTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsRelease = true;
            await Allocator.Release();
        }

        private void ShowMsg(string sMsg)
        {
            if (IsRelease) return;
            MessageBox.Show(sMsg, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowErr(string sMsg)
        {
            if (IsRelease) return;
            MessageBox.Show(sMsg, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddLog(string sLog)
        {
            if (IsRelease) return;
            if (txtLog.InvokeRequired)
            {
                Invoke(AddLog, sLog);
                return;
            }
            if (IsRelease) return;
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss} -- {sLog} \r\n");
        }

        private async void btnCloseBind_Click(object sender, EventArgs e)
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

            try
            {
                var connector = await Allocator.CloseConnectorAsync(serverDetail);

                AddLog("�ر�UDP�������ɹ���" + serverDetail.GetKey());
            }
            catch (Exception ex)
            {

                AddLog("�ر�UDP����������" + ex.Message);
                ShowErr("�ر�UDP����������" + ex.Message);
            }
        }

        private void txtLog_DoubleClick(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            UDPClientDetail clientDetail;
            string sSendTxt = txtSend.Text;
            if (string.IsNullOrWhiteSpace(sSendTxt))
            {
                ShowErr("�������ݲ���Ϊ��!");
                return;
            }

            #region ���ؼ�������
            string sLocalIP = cmbLocalIP.Text;
            int iLocalPort = (int)txtLocalPort.Value;

            if (string.IsNullOrWhiteSpace(sLocalIP))
            {
                sLocalIP = String.Empty;
            }
            else
            {
                if (sLocalIP.Equals("*"))
                    sLocalIP = String.Empty;
            }
            #endregion

            #region Զ�̷���������
            string sRemoteIP = txtRemoteIP.Text;
            int iRemotePort = (int)txtRemotePort.Value;
            if (string.IsNullOrEmpty(sRemoteIP))
            {
                ShowErr("Զ��IP����Ϊ��!");
                return;
            }
            #endregion

            clientDetail = new UDPClientDetail(sRemoteIP, iRemotePort, sLocalIP, iLocalPort);

            //�󶨴����¼�
            clientDetail.ClientOfflineCallBlack = UDPServer_ClientOfflineCallBlack;
            clientDetail.ClientOnlineCallBlack = UDPServer_ClientOnlineCallBlack;
            //serverDetail.ConnectedCallBlack= UDPServer_ConnectedCallBlack;


            try
            {
                var connector = await Allocator.OpenConnectorAsync(clientDetail);

                var connDtl = connector.GetConnectorDetail();
                await connector.RunCommandAsync(new TextCommand(connDtl, sSendTxt));
                AddLog("���ͳɹ�");
            }
            catch (Exception ex)
            {

                AddLog("���ʹ���" + ex.Message);
            }
        }

        private void btnOpenUDPClient_Click(object sender, EventArgs e)
        {
            UDPClientDetail clientDetail;
            #region ���ؼ�������
            string sLocalIP = cmbLocalIP.Text;
            int iLocalPort = (int)txtLocalPort.Value;

            if (string.IsNullOrWhiteSpace(sLocalIP))
            {
                sLocalIP = String.Empty;
            }
            else
            {
                if (sLocalIP.Equals("*"))
                    sLocalIP = String.Empty;
            }
            #endregion

            #region Զ�̷���������
            string sRemoteIP = txtRemoteIP.Text;
            int iRemotePort = (int)txtRemotePort.Value;
            if (string.IsNullOrEmpty(sRemoteIP))
            {
                ShowErr("Զ��IP����Ϊ��!");
                return;
            }
            #endregion

            clientDetail = new UDPClientDetail(sRemoteIP, iRemotePort, sLocalIP, iLocalPort);

            //�󶨴����¼�
            clientDetail.ClientOfflineCallBlack = UDPServer_ClientOfflineCallBlack;
            clientDetail.ClientOnlineCallBlack = UDPServer_ClientOnlineCallBlack;
            clientDetail.ConnectedCallBlack= UDPServer_ConnectedCallBlack;
            clientDetail.ClosedCallBlack = UDPServer_ClosedCallBlack;


            var connector = Allocator.OpenConnector(clientDetail);

        }
    }
}