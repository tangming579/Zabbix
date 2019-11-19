using Newtonsoft.Json.Linq;
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
        public static string Token { set; get; }

        public static void GetZabbixData(ZabbixParam zabbixParam)
        {
            JObject obj = new JObject();
            obj["jsonrpc"] = "2.0";
            obj["method"] = zabbixParam.method;
            obj["id"] = 1;
            obj["auth"] = Token;
            obj["params"] = zabbixParam.jParams;

            var postResult = new ZabbixParam() { method = zabbixParam.method };

            GetZabbixData(obj + "", (str) =>
               {
                   var jResult = JObject.Parse(str);
                   var result = new ZabbixResult();
                   result.id = int.Parse(jResult["id"] + "");
                   result.jsonrpc = jResult["jsonrpc"] + "";
                   result.result = jResult["result"];
                   result.resultTotal = jResult;
                   zabbixParam.Callback?.Invoke(result);
               });
        }

        public static void GetZabbixData(string jsonStr, Action<string> callback)
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
                    callback?.Invoke(result);
                }
                catch (Exception exp)
                {
                    result = "Error:" + (exp.InnerException?.Message ?? exp.Message);
                }
            };
            client.UploadDataAsync(new Uri(postUrl), "POST", postData);
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

    public class ZabbixParam
    {
        public string method { get; set; }
        public JToken jParams { get; set; }
        public Action<ZabbixResult> Callback { get; set; }
    }

    public class ZabbixResult
    {
        public string jsonrpc { get; set; }

        public JToken result { get; set; }

        public JObject resultTotal { get; set; }

        public int id { get; set; }
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
