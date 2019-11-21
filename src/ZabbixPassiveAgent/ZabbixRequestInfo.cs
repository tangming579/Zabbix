using Newtonsoft.Json.Linq;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixPassiveAgent
{
    public class ZabbixRequestInfo : IRequestInfo
    {
        public string Key { get; set; }
        public string Message { get; set; }

        public ZabbixRequestInfo(byte[] buffer)
        {
            try
            {
                Message = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                byte[] data = new byte[buffer.Length - 13];
                Buffer.BlockCopy(buffer, 13, data, 0, data.Length);
                Key = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
            }
            catch (Exception exp)
            {

            }
        }
    }
}
