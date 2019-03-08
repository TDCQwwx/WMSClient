using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Models
{
    public class OutputRecordModel : NotificationObject
    {
        public int Location { get; set; }
        public string RFIDNumber { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
