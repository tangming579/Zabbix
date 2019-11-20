using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

            //设备状态信息（代替心跳包）
            timer = new System.Timers.Timer(6000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!client.IsConnected)
                    {
                        client.ConnectAsync(new IPEndPoint(IPAddress.Parse("154.8.184.140"), 10052));
                    }
                }));

            });
            timer.Enabled = true;
            timer.Start();
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
    }
}
