using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Common
{
    public class OrderNumber
    {
        /// <summary>
        /// Build an orderNumber
        /// </summary>
        /// <returns>orderNumber</returns>
        public static string GetOrderNumber()
        {
            Random randomOrderNumber = new Random();
            return DateTime.Now.ToString("yyyyMMddHHmmss") + randomOrderNumber.Next(10, 99);
        }

        /// <summary>
        /// Get an orderNumber
        /// </summary>
        /// <param name="message">FunctionCodeParameter</param>
        /// <returns>orderNumber</returns>
        public static string GetOrderNumber(string message)
        {
            if (message.Length == 36)
            {
                return message.Substring(4, 16);
            }
            else if (message.Length == 44)
            {
                return message.Substring(12, 16);
            }
            else
            {
                return null;
            }
        }
    }
}
