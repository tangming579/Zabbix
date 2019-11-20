using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
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

namespace ZabbixActiveAgent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static EasyClient<MyPackageInfo> client = null;
        static System.Timers.Timer timer = null;       

        public MyPackageInfo info;
        public int id;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //client = new EasyClient<MyPackageInfo>();
            //client.Initialize(new ZabbixReceiveFilter());
            //client.Connected += OnClientConnected;
            //client.NewPackageReceived += Client_NewPackageReceived1;
            //client.Error += OnClientError;
            //client.Closed += OnClientClosed;

            
        }        

        private void Client_NewPackageReceived1(object sender, PackageEventArgs<MyPackageInfo> e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {

            }));
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {                
                if (client.IsConnected)
                {
                    JObject obj = new JObject();
                    obj["request"] = "active checks";
                    obj["host"] = "Dell Laptop";
                    string strSend = obj + "";
                    byte[] msg = Encoding.UTF8.GetBytes(strSend);
                    int len = strSend.Length;  //string strSend的长度
                    byte[] headerAndLen = new byte[] {(byte)'Z',(byte)'B',(byte)'X',(byte)'D',(byte)1,
            (byte)(len & 0xFF),
            (byte)((len>>8) & 0x00FF),
            (byte)((len>>16) & 0x0000FF),
            (byte)((len>>24) & 0x000000FF),
            (byte)0,(byte)0,(byte)0,(byte)0};

                    byte[] sendMsg = new byte[headerAndLen.Length + msg.Length];
                    headerAndLen.CopyTo(sendMsg, 0);
                    msg.CopyTo(sendMsg, headerAndLen.Length);

                    client.Send(sendMsg);
                }
            }));
        }
        private void OnClientClosed(object sender, EventArgs e)
        {            
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                var client = sender as EasyClient<MyPackageInfo>;
                
            }));
        }

        private void OnClientError(object sender, ErrorEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.ConnectAsync(new IPEndPoint(IPAddress.Parse("154.8.184.140"), 10051));
        }
    }
}
