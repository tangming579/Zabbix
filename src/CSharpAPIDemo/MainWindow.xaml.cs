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

        public void Login()
        {
            JObject obj = new JObject();
            obj["jsonrpc"] = "2.0";
            obj["method"] = "user.login";
            obj["id"] = 1;
            obj["auth"] = null;

            var objParams = new JObject();
            objParams["user"] = "Admin";
            objParams["password"] = "zabbix";
            obj["params"] = objParams;
            WebClientManager.GetData(obj + "");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
