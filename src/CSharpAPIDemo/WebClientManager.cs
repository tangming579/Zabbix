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
        public static string Token { private set; get; }

        #region API

        public static PostResult Login(string user, string password)
        {
            var objParams = new JObject();
            objParams["user"] = user;
            objParams["password"] = password;

            PostResult postResult = GetZabbixData("user.login", objParams);

            postResult.Callback = (result) =>
               {
                   Token = result.result + "";
               };
            return postResult;
        }


        #endregion

        public static PostResult GetZabbixData(string method, JObject jParams)
        {
            JObject obj = new JObject();
            obj["jsonrpc"] = "2.0";
            obj["method"] = method;
            obj["id"] = 1;
            obj["auth"] = null;
            obj["params"] = jParams;
            obj["token"] = Token;

            var postResult = new PostResult() { method = method };

            GetZabbixData(obj + "", (str) =>
               {
                   var jResult = JObject.Parse(str);
                   var result = new zabbixResult();
                   result.id = int.Parse(jResult["id"] + "");
                   result.jsonrpc = jResult["jsonrpc"] + "";
                   result.result = jResult["result"];
                   postResult.Callback?.Invoke(result);
               });
            return postResult;
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

    public class PostResult
    {
        public string method { get; set; }
        public Action<zabbixResult> Callback { get; set; }
    }

    public class zabbixResult
    {
        public string jsonrpc { get; set; }

        public JToken result { get; set; }

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
