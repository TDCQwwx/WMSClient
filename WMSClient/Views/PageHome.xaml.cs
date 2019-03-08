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
using WMSClient.ViewModels;

namespace WMSClient.Views
{
    /// <summary>
    /// PageHome.xaml 的交互逻辑
    /// </summary>
    public partial class PageHome : Page
    {
        public PageHome()
        {
            InitializeComponent();
            this.DataContext = UserViewModel.InstanceUserViewModel;
        }
        private static string _operationMode = "online";

        public static string OperationMode
        {
            get { return _operationMode; }
            set { _operationMode = value; }
        }

        private void RadioButtonOffline_Checked(object sender, RoutedEventArgs e)
        {
            OperationMode = "offline";
        }

        private void RadioButtonOnline_Checked(object sender, RoutedEventArgs e)
        {
            OperationMode = "online";
        }
    }
}
