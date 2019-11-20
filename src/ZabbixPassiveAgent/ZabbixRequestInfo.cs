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

        public ZabbixRequestInfo(byte[] buffer)
        {
            try
            {
                Key = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            }
            catch (Exception exp)
            {

            }
        }
    }
}
