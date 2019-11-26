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
        public JToken JData { get; set; }
        public bool Response { get; private set; }

        public ZabbixRequestInfo(byte[] buffer)
        {
            try
            {
                Message = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                byte[] data = new byte[buffer.Length - 13];
                Buffer.BlockCopy(buffer, 13, data, 0, data.Length);
                Key = Encoding.ASCII.GetString(data, 0, data.Length);
                JData = JObject.Parse(Key);
                Response = string.Equals(JData.SelectToken("response") + "", "success", StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception exp)
            {

            }
        }
    }
}
