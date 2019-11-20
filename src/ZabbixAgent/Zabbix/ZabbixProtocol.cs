using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixAgent.Zabbix
{
    public class ZabbixProtocol
    {
        public static byte[] WriteWithHeader(string data)
        {
            byte[] Header = Encoding.ASCII.GetBytes(ZabbixConstants.HeaderString);
            byte[] DataLen = BitConverter.GetBytes((long)data.Length);
            byte[] Content = Encoding.ASCII.GetBytes(data);
            byte[] Message = new byte[Header.Length + DataLen.Length + Content.Length];
            Buffer.BlockCopy(Header, 0, Message, 0, Header.Length);
            Buffer.BlockCopy(DataLen, 0, Message, Header.Length, DataLen.Length);
            Buffer.BlockCopy(Content, 0, Message, Header.Length + DataLen.Length, Content.Length);

            return Message;
        }

        public static byte[] WriteWithHeader(JToken data)
        {
            return WriteWithHeader(data + "");
        }
    }
}
