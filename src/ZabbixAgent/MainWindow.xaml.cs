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

namespace ZabbixAgent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool running;
        private Socket socket;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Connect(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(iPAddress, port);

            Task.Run(() =>
            {
                while (running)
                {
                    if (!socket.Connected)
                    {
                        //socket.Connect(point);
                        //Thread.Sleep(2000);
                    }
                    else
                    {
                        Received();
                    }
                }

            });
        }

        protected virtual void Received()
        {
            while (running & socket.Connected)
            {

            }
        }

       
    }
}
