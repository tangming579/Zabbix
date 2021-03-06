﻿using Newtonsoft.Json.Linq;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Random random = new Random();
        DateTime oldValue = DateTime.Now;
        //接收客户端消息
        private void App_NewRequestReceived(ZabbixSession session, ZabbixRequestInfo requestInfo)
        {
            if (appServer != null && appServer.State == ServerState.Running && appServer.SessionCount > 0)
            {

                Console.WriteLine($"收到：{requestInfo.Key}, Time:{DateTime.Now:HH:mm:ss}");

                switch (requestInfo.Key)
                {
                    case "agent.ping":
                        {
                            var data = ZabbixProtocol.WriteWithHeader("1");
                            session.Send(data, 0, data.Length);
                        }
                        break;
                    case "device.discovery":
                        {
                            var data = HandleDeviceDiscovery();
                            session.Send(data, 0, data.Length);
                        }
                        break;
                    case "station.discovery":
                        {
                            var data = HandleStationDiscovery();
                            session.Send(data, 0, data.Length);
                        }
                        break;
                    default:
                        {
                            var data = HandleItemValue(requestInfo.Key);
                            session.Send(data, 0, data.Length);
                        }
                        break;
                }
            }
        }

        private byte[] HandleDeviceDiscovery()
        {
            var jDevice = new JObject();
            var datas = new JArray();
            for (int i = 0; i < 5; i++)
            {
                var data = new JObject();
                data["{#NAME}"] = "device" + i;
                datas.Add(data);
            }
            jDevice["data"] = datas;

            return ZabbixProtocol.WriteWithHeader(jDevice);
        }

        private byte[] HandleStationDiscovery()
        {
            var jDevice = new JObject();
            var datas = new JArray();
            for (int i = 0; i < 3; i++)
            {
                var data = new JObject();
                data["{#NAME}"] = "station" + i;
                datas.Add(data);
            }
            jDevice["data"] = datas;

            return ZabbixProtocol.WriteWithHeader(jDevice);
        }

        private byte[] HandleItemValue(string itemKey)
        {
            switch (itemKey)
            {
                case "Name":
                    {
                        return ZabbixProtocol.WriteWithHeader("Tang Ming");
                    }
                case "Percent":
                    {
                        return ZabbixProtocol.WriteWithHeader("70");
                    }
                case "CPU":
                    {
                        var span = (DateTime.Now - oldValue).TotalSeconds;
                        oldValue = DateTime.Now;
                        Console.WriteLine(span);
                        var value = random.Next(30, 90) + "";
                        return ZabbixProtocol.WriteWithHeader(value);
                    }
                case "Detail":
                    {

                        return ZabbixProtocol.WriteWithHeader("Hello World!");
                    }
                default:
                    {
                        if (itemKey.Contains("station"))
                            return GetStationValue(itemKey);
                        else if (itemKey.Contains("device"))
                            return GetDeviceValue(itemKey);
                        else
                            return ZabbixProtocol.WriteWithHeader($"{ZabbixConstants.NotSupported}\0Cannot find the item key");
                    }
            }
        }

        public byte[] GetDeviceValue(string itemKey)
        {
            int index1 = itemKey.IndexOf('[');
            int index2 = itemKey.IndexOf(']');
            string dicKey = itemKey.Substring(index1 + 1, index2 - index1 - 1);
            if (itemKey.Contains(".ip"))
            {
                return ZabbixProtocol.WriteWithHeader($"192.168.100.1");
            }
            else if (itemKey.Contains(".name"))
            {
                return ZabbixProtocol.WriteWithHeader($"deviceName");
            }
            else if (itemKey.Contains(".state"))
            {
                return ZabbixProtocol.WriteWithHeader($"1");
            }
            else if (itemKey.Contains(".errormessage"))
            {
                return ZabbixProtocol.WriteWithHeader($"None");
            }
            return ZabbixProtocol.WriteWithHeader($"{ZabbixConstants.NotSupported}\0Cannot find the item key");
        }

        public byte[] GetStationValue(string itemKey)
        {
            int index1 = itemKey.IndexOf('[');
            int index2 = itemKey.IndexOf(']');
            string dicKey = itemKey.Substring(index1 + 1, index2 - index1 - 1);
            if (itemKey.Contains(".ip"))
            {
                return ZabbixProtocol.WriteWithHeader($"192.168.100.1");
            }
            else if (itemKey.Contains(".name"))
            {
                return ZabbixProtocol.WriteWithHeader($"stationName");
            }
            else if (itemKey.Contains(".state"))
            {
                return ZabbixProtocol.WriteWithHeader($"1");
            }
            else if (itemKey.Contains(".errormessage"))
            {
                return ZabbixProtocol.WriteWithHeader($"None");
            }
            return ZabbixProtocol.WriteWithHeader($"{ZabbixConstants.NotSupported}\0Cannot find the item key");
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
