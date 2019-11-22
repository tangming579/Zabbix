using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZabbixCore;

namespace ZabbixActiveAgent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static EasyClient<MyPackageInfo> client = null;
        private JToken JData;

        public MyPackageInfo info;
        public int id;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            client = new EasyClient<MyPackageInfo>();
            client.Initialize(new ZabbixReceiveFilter());
            client.Connected += OnClientConnected;
            client.NewPackageReceived += Client_NewPackageReceived1;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;
        }

        private void Client_NewPackageReceived1(object sender, PackageEventArgs<MyPackageInfo> e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine(e.Package.Key);
                if (JData == null)
                    JData = e.Package.JData;
            }));
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (client.IsConnected)
                {
                    if (JData == null)
                    {
                        JObject obj = new JObject();
                        obj["request"] = "active checks";
                        obj["host"] = "Dell Laptop";
                        var sendMsg = ZabbixProtocol.WriteWithHeader(obj);

                        client.Send(sendMsg);
                    }
                    else
                    {
                        JObject obj = new JObject();
                        obj["request"] = "agent data";
                        JArray jArray = new JArray();

                        var data = JData.SelectToken("data") as JArray;
                        foreach (var da in data)
                        {
                            JObject dataObj = new JObject();
                            dataObj["key"] = da["key"] + "";
                            dataObj["host"] = "Dell Laptop";
                            dataObj["value"] = "48";
                            //dataObj["clock"] = DateTime.Now.Ticks;
                            dataObj["ns"] = 76808644;
                            jArray.Add(dataObj);
                        }
                        obj["data"] = jArray;
                        var sendMsg = ZabbixProtocol.WriteWithHeader(obj);

                        client.Send(sendMsg);
                    }
                }
            }));
        }
        private void OnClientClosed(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {

            }));
        }

        private void OnClientError(object sender, ErrorEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
            client.ConnectAsync(new IPEndPoint(IPAddress.Parse(baseUrl), 10051));


        }
    }
}
