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
using WMSClient.ViewModels;

namespace WMSClient.Views
{
    /// <summary>
    /// WndModifyStoragesInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WndModifyStoragesInfo : Window
    {
        //声明一个传递参数给修改库位信息窗体的一个委托
        public delegate void DeliveryNewMesseagesEventHandler(int location, string category, int amount);
        //声明一个传递参数给修改库位信息窗体的一个事件
        public event DeliveryNewMesseagesEventHandler DeliveryNewMesseagesEvent;
        public WndModifyStoragesInfo()
        {
            InitializeComponent();
            //UserViewModel userViewModel = new UserViewModel();
            //userViewModel.DeliveryMesseagesEvent += new UserViewModel.DeliveryMesseagesEventHandler(UserViewModel_DeliveryMesseagesEvent);
        }
        public void DeliveryMesseagesEventFunc(int location, string category, int amount)
        {
            //throw new NotImplementedException();
            txtOldLocation.Text = location.ToString();
            txtOldCategory.Text = category;
            txtOldAmount.Text = amount.ToString();
            txtNewLocation.Text = location.ToString();
        }
        //确认更改
        private void btnConfirmModification_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(txtNewLocation.Text);
            //MessageBox.Show(cbxNewCategory.Text);
            //MessageBox.Show(cbxNewAmount.Text);
            DeliveryNewMesseagesEvent += UserViewModel.EditStoragesInfoFunc;
            DeliveryNewMesseagesEvent(Convert.ToInt32(txtNewLocation.Text), cbxNewCategory.Text, Convert.ToInt32(cbxNewAmount.Text));
        }
        //退出
        private void btnModificationExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
