using Newtonsoft.Json.Linq;
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

namespace CSharpAPIDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //登录
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();
            objParams["user"] = txbUser.Text;
            objParams["password"] = txbPassword.Text;

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "user.login";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.result + "";
                WebClientManager.Token = result.result + "";
                if (string.IsNullOrEmpty(WebClientManager.Token))
                    MessageBox.Show("登录失败！");
                else
                {
                    btnLogin.Content = "已登录";
                }
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }
        //检索主机
        private void BtnGetHost_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();
            var output = new JArray() { "hostid", "host" };
            var selectInterfaces = new JArray() { "interfaceid", "ip" };

            objParams["output"] = output;
            objParams["selectInterfaces"] = selectInterfaces;

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "host.get";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.result + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }
    }
}
