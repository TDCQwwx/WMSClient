using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Models
{
    public class StorageInfoModel : NotificationObject
    {
        public string RFIDNumber { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public int Location { get; set; }
        //用于标记是否可以修改库位信息，true表示可以修改，false表示不可以修改
        //瓶子，盖子，空料盘可以改为无料盘
        //瓶子，盖子，空料盘可以互相更改
        public bool EditStoragesInfo { get; set; }
        //用于标记是否可以出库，瓶子，盖子，空料盘可以出库
        public bool EditOutput { get; set; }
    }
}
