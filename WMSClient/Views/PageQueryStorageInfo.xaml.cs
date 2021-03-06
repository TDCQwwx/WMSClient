﻿using System;
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
    /// PageQueryStorageInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageQueryStorageInfo : Page
    {
        public PageQueryStorageInfo()
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
