using SaeaServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.TcpIp
{
    public class ClientDataReceivedEventArgs : EventArgs
    {
        public byte[] _dataBytes;
        public TcpSocketSaeaClient _client;
        public ClientDataReceivedEventArgs(TcpSocketSaeaClient client, byte[] bytesData, int offSet, int count)
        {
            _dataBytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                _dataBytes[i] = bytesData[offSet + i];
            }
            _client = client;
        }
    }
}
