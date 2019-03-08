using SaeaServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.TcpIp
{
    public class ClientEventDispatcher : ITcpSocketSaeaClientEventDispatcher
    {
        //TCP会话开始
        public delegate void OnServerStartedEventHandler(object sender, TcpSocketSaeaClient e);
        public event OnServerStartedEventHandler ClientStartedEvent;
        //数据接收
        public delegate void OnServerDataReceivedEventHandler(object sender, ClientDataReceivedEventArgs e);
        public event OnServerDataReceivedEventHandler ClientDataReceivedEvent;
        //TCP会话结束
        public delegate void OnServerClosedEventHandler(object sender, TcpSocketSaeaClient session);
        public event OnServerClosedEventHandler ClientClosedEvent;
        public async Task OnServerConnected(TcpSocketSaeaClient client)
        {
            ClientStartedEvent(this, client);
            await Task.CompletedTask;
        }
        public async Task OnServerDataReceived(TcpSocketSaeaClient client, byte[] data, int offset, int count)
        {
            ClientDataReceivedEvent(this, new ClientDataReceivedEventArgs(client, data, offset, count));
            await Task.CompletedTask;
        }
        public async Task OnServerDisconnected(TcpSocketSaeaClient client)
        {
            ClientClosedEvent(this, client);
            await Task.CompletedTask;
        }
    }
}
