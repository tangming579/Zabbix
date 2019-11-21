using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixCore
{
    public class ZabbixPassiveAgent
    {
        private IPAddress ip;
        private int port;

        public ZabbixPassiveAgent(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public ZabbixPassiveAgent(string ip, int port)
        {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
        }

        protected virtual void Send(string msg)
        {
            var sendMsg = System.Text.Encoding.UTF8.GetBytes(msg);
            Send(sendMsg);
        }

        protected virtual void Send(byte[] buf)
        {
            
        }

    }
}
