using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Models
{
    public class ModifyStorageRecordModel : NotificationObject
    {
        public int Location { get; set; }
        public int ModifyType { get; set; }//1:表示删除盘子；2：表示修改物料种类
        public string RFIDNumber { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
