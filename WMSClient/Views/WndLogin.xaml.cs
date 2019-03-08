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
using System.Windows.Shapes;

namespace WMSClient.Views
{
    /// <summary>
    /// WndLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WndLogin : Window
    {
        public WndLogin()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbxUsername.Text == "admin" && pwdBox.Password == "admin123")
            {
                WndMain wndMain = new WndMain();
                this.Close();
                wndMain.Show();
            }
            else if(tbxUsername.Text==""|| pwdBox.Password == "")
            {
                MessageBox.Show("用户名或密码不能为空！");
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
        }
        private void btnLoginPageExit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
