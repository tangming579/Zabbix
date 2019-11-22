using Newtonsoft.Json.Linq;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixActiveAgent
{
    public class MyPackageInfo : IPackageInfo
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public JToken JData { get; set; }

        public MyPackageInfo(byte[] buffer)
        {
            Message = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            byte[] data = new byte[buffer.Length - 13];
            Buffer.BlockCopy(buffer, 13, data, 0, data.Length);
            Key = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
            JData = JObject.Parse(Key);
        }
    }
}
