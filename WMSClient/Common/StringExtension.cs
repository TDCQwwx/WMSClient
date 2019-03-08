using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Common
{
    public static class StringExtension
    {
        /// <summary>
        /// 将十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(this string str)
        {
            try
            {
                str = str.Replace(" ", "");
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < str.Length; i += 2)
                    buffer[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 截取控制器ID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InterceptControllerId(this string str)
        {
            return str.Substring(0, 4);
        }
        /// <summary>
        /// 截取功能码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InterceptFunctionCode(this string str)
        {
            return str.Substring(4, 4);
        }
        /// <summary>
        /// 截取功能码参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InterceptFunctionCodeParameter(this string str)
        {
            return str.Substring(8);
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterObjectId(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(0, 4);
            }
            else if (str.Length == 44)
            {
                return str.Substring(8, 4);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterOrdernumber(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(4, 16);
            }
            else if (str.Length == 44)
            {
                return str.Substring(12, 16);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterTaskType(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(20, 4);
            }
            else if (str.Length == 44)
            {
                return str.Substring(28, 4);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterParameter1(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(24, 4);
            }
            else if (str.Length == 44)
            {
                return str.Substring(32, 4);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterParameter2(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(28, 4);
            }
            else if (str.Length == 44)
            {
                return str.Substring(36, 4);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterParameter3(this string str)
        {
            if (str.Length == 36)
            {
                return str.Substring(32, 4);
            }
            else if (str.Length == 44)
            {
                return str.Substring(40, 4);
            }

            return null;
        }
        public static string InterceptPredefinedTaskFunctionCodeParameterTaskState(this string str)
        {
            if (str.Length == 48)
            {
                return str.Substring(44);
            }

            return null;
        }
    }
}
