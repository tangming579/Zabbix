using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ZabbixPassiveAgent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ZabbixServer appServer;
        private int port = 10086;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (appServer != null && appServer.State == SuperSocket.SocketBase.ServerState.Running)
            {
                return;
            }
            var config = new SuperSocket.SocketBase.Config.ServerConfig()
            {
                Name = "ZabbixAgent",
                ServerTypeName = "ZabbixAgent",
                ClearIdleSession = false, //60秒执行一次清理90秒没数据传送的连接
                ClearIdleSessionInterval = 60,
                IdleSessionTimeOut = 90,
                MaxRequestLength = 10000, //最大包长度
                Ip = "Any",
                Port = port,
                MaxConnectionNumber = 10000,//最大允许的客户端连接数目
            };
            appServer = new ZabbixServer(app_NewSessionConnected, app_SessionClosed);
            appServer.NewRequestReceived -= App_NewRequestReceived;
            appServer.NewRequestReceived += App_NewRequestReceived;
            if (!appServer.Setup(config))
            {
                return;
            }
            appServer.Start();
        }

        //接收客户端消息
        private void App_NewRequestReceived(ZabbixSession session, ZabbixRequestInfo requestInfo)
        {

        }
        //客户端连接
        void app_NewSessionConnected(ZabbixSession session)
        {

        }
        //客户端断开
        void app_SessionClosed(ZabbixSession session, CloseReason value)
        {

        }
    }
}
