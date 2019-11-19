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
                txbResult.Text = result.resultTotal + "";
                WebClientManager.Token = result.result + "";
                if (string.IsNullOrEmpty(WebClientManager.Token))
                    MessageBox.Show("登录失败！");
                else
                {
                    btnLogin.Content = "已登录";
                    staCtl.IsEnabled = true;
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
                txbResult.Text = result.resultTotal + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }

        //获取历史数据，参考：https://www.zabbix.com/documentation/3.4/zh/manual/api/reference/history/get
        private void BtnGetHistory_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();

            objParams["output"] = "extend";//要返回的对象属性，可能的值: extend.
            objParams["history"] = 0;
            //要返回的历史对象类型
            //  0 - numeric float; 数字浮点数
            //  1 - character; 字符
            //  2 - log; 日志
            //  3 - numeric unsigned; 数字符号
            //  4 - text.文本

            objParams["itemids"] = "23296";
            objParams["sortfield"] = "clock";//按什么排序，可能的值为：itemid和clock
            objParams["sortorder"] = "DESC";
            objParams["limit"] = 10;

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "history.get";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.resultTotal + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }

        private void BtnGetItem_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();
            var search = new JObject();
            search["key_"] = "system";//key中包含什么字符串
            search["name"] = "Context switches per second";

            objParams["output"] = "extend";//要返回的对象属性，可能的值: extend.
            objParams["hostids"] = "10269";
            objParams["search"] = search;
            objParams["sortfield"] = "name";//按什么排序

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "item.get";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.resultTotal + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }

        private void BtnGetProblem_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();
            objParams["output"] = "extend";//要返回的对象属性，可能的值: extend.
            objParams["hostids"] = "10269";

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "problem.get";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.resultTotal + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }

        private void BtnGetAlert_Click(object sender, RoutedEventArgs e)
        {
            var objParams = new JObject();
            objParams["output"] = "extend";//要返回的对象属性，可能的值: extend.
            objParams["hostids"] = "10269";

            ZabbixParam zabbixParam = new ZabbixParam();
            zabbixParam.method = "alert.get";
            zabbixParam.jParams = objParams;
            zabbixParam.Callback = (result) =>
            {
                txbResult.Text = result.resultTotal + "";
            };

            WebClientManager.GetZabbixData(zabbixParam);
        }
    }
}
