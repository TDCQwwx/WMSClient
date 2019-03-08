using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Models
{
    //用户的用户名和密码，用于登陆时用
    public class UserModel : NotificationObject
    {
        //用户名
        public string UserName { get; set; }
        //密码
        public string Password { get; set; }

    }
}
