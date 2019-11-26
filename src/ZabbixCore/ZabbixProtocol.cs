using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixCore
{
    //参考：https://www.zabbix.com/documentation/4.0/zh/manual/appendix/items/activepassive
    //更多细节：https://www.zabbix.org/wiki/Docs/protocols/zabbix_agent/3.0
    public class ZabbixProtocol
    {
        public static byte[] WriteWithHeader(string sendData)
        {
            byte[] header = Encoding.UTF8.GetBytes(ZabbixConstants.HeaderString);
            byte[] dataLen = BitConverter.GetBytes((long)sendData.Length);
            byte[] data = Encoding.UTF8.GetBytes(sendData);
            byte[] totalMessage = new byte[header.Length + dataLen.Length + data.Length];
            Buffer.BlockCopy(header, 0, totalMessage, 0, header.Length);
            Buffer.BlockCopy(dataLen, 0, totalMessage, header.Length, dataLen.Length);
            Buffer.BlockCopy(data, 0, totalMessage, header.Length + dataLen.Length, data.Length);

            return totalMessage;
        }

        public static byte[] WriteWithHeader(JToken sendData)
        {
            return WriteWithHeader(sendData + "");
        }
    }
}
