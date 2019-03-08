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
    /// PageQueryModifyStorageRecord.xaml 的交互逻辑
    /// </summary>
    public partial class PageQueryModifyStorageRecord : Page
    {
        public PageQueryModifyStorageRecord()
        {
            InitializeComponent();
            this.DataContext = UserViewModel.InstanceUserViewModel;
        }
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
