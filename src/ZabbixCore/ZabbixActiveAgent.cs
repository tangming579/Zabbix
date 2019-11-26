using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixCore
{
    //https://www.zabbix.com/documentation/4.0/zh/manual/appendix/items/activepassive
    public class ZabbixActiveAgent
    {
        private IPAddress ip;
        private int port;
        private TcpClient tcpClient;

        public ZabbixActiveAgent(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public ZabbixActiveAgent(string ip, int port)
        {
            this.ip = IPAddress.Parse(ip);
            this.port = port;
        }

        public virtual void Send()
        {
            tcpClient = new TcpClient();

            tcpClient.Connect(ip, port);

            NetworkStream stream = tcpClient.GetStream();

            JObject obj = new JObject();
            obj["request"] = "active checks";
            obj["host"] = "Dell Laptop";
            string strSend = obj + "";

            var data = ZabbixProtocol.WriteWithHeader(strSend);

            stream.Write(data, 0, data.Length);

            data = new Byte[2048];

            String responseData = String.Empty;

            if (tcpClient.Connected)
            {
                int bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            } 

            stream.Close();
            tcpClient.Close();
        }
    }
}
