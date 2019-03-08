using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WMSClient.Models
{
    public class InputModel : NotificationObject
    {
        public string OldRFIDNumber { get; set; }
        public string NewRFIDNumber { get; set; }
        public ComboBoxItem Category { get; set; }
        public ComboBoxItem Amount { get; set; }
    }
}
