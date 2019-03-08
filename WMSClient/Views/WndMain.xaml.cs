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
    /// WndMain.xaml 的交互逻辑
    /// </summary>
    public partial class WndMain : Window
    {
        public PageHome PageHome { get; set; }
        public PageInput PageInput { get; set; }
        public PageDispatchAGV PageDispatchAGV { get; set; }
        public PageQueryStorageInfo PageQueryStorageInfo { get; set; }
        public PageQueryInputRecord PageQueryInputRecord { get; set; }
        public PageQueryOutputRecord PageQueryOutputRecord { get; set; }
        public PageQueryModifyStorageRecord PageQueryModifyStorageRecord { get; set; }
        public WndMain()
        {
            InitializeComponent();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0://Home
                    if (PageHome == null)
                    {
                        PageHome = new PageHome();
                    }
                    contentControl.Content = new Frame { Content = PageHome };
                    break;
                case 1://Input
                    if (PageHome.OperationMode == "offline")
                    {
                        if (PageInput == null)
                        {
                            PageInput = new PageInput();
                        }
                        contentControl.Content = new Frame { Content = PageInput };
                    }
                    if (PageHome.OperationMode == "online")
                    {
                        MessageBox.Show("联机模式下不允许入库作业！");
                    }
                    break;
                case 2://DispatchAGV
                    if (PageHome.OperationMode == "offline")
                    {
                        if (PageDispatchAGV == null)
                        {
                            PageDispatchAGV = new PageDispatchAGV();
                        }
                        contentControl.Content = new Frame { Content = PageDispatchAGV };
                    }
                    if (PageHome.OperationMode == "online")
                    {
                        MessageBox.Show("联机模式下不允许调度AGV！");
                    }
                    break;
                case 3://StorageInfo
                    if (PageQueryStorageInfo == null)
                    {
                        PageQueryStorageInfo = new PageQueryStorageInfo();
                    }
                    contentControl.Content = new Frame { Content = PageQueryStorageInfo };
                    break;
                case 4://InputRecord
                    if (PageQueryInputRecord == null)
                    {
                        PageQueryInputRecord = new PageQueryInputRecord();
                    }
                    contentControl.Content = new Frame { Content = PageQueryInputRecord };
                    break;
                case 5://OutputRecord
                    if (PageQueryOutputRecord == null)
                    {
                        PageQueryOutputRecord = new PageQueryOutputRecord();
                    }
                    contentControl.Content = new Frame { Content = PageQueryOutputRecord };
                    break;
                case 6://ModifyStorageRecord
                    if (PageQueryModifyStorageRecord == null)
                    {
                        PageQueryModifyStorageRecord = new PageQueryModifyStorageRecord();
                    }
                    contentControl.Content = new Frame { Content = PageQueryModifyStorageRecord };
                    break;
                case 7://Exit
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
