using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAPIDemo
{
    public static class WebClientManager
    {
        public static void GetData(string jsonStr, Action<string> callback)
        {
            var client = CreateWebClient();
            var postUrl = ConfigurationManager.AppSettings["baseUrl"];
            byte[] postData = Encoding.UTF8.GetBytes(jsonStr);
            client.UploadDataCompleted += (sender, e) =>
            {
                string result = string.Empty;
                try
                {
                    result = Encoding.UTF8.GetString(e.Result);
                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                }
                callback?.Invoke(result);
            };
        }

        private static void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        static ZabbixWebClient CreateWebClient()
        {
            ZabbixWebClient webClient = new ZabbixWebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //webClient.Headers.Add("Content-Type", "application/json-rpc; charset=utf-8");
            return webClient;
        }
    }

    public class ZabbixWebClient : WebClient
    {
        private int timeOut;
        public ZabbixWebClient()
        {
            this.timeOut = int.Parse(ConfigurationManager.AppSettings["webClientTimeOut"]);
            this.Encoding = Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            return request;
        }
    }
}
