using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Common
{
    public class DataHelper
    {
        /// <summary>
        /// 将对象序列化为二进制数据 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);

            byte[] data = stream.ToArray();
            stream.Close();

            return data;
        }
        public static object DeserializeWithBinary(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);

            stream.Close();

            return obj;
        }
        public static byte[] HexStringToByteArray(string s)
        {
            try
            {
                s = s.Replace(" ", "");
                byte[] buffer = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string ByteArrayToHexString(byte[] bytes)
        {
            var hex = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            return hex;
        }
        public static string OrderNumber()
        {
            //法一
            //string orderNumberTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            //string orderNumberHead = "";
            //for (int i = 0; i < 16 - orderNumberTime.Length; i++)
            //{
            //    orderNumberHead = orderNumberHead + "0";
            //}
            //string orderNumber = orderNumberHead + orderNumberTime;
            //Console.WriteLine(orderNumber);
            //Console.WriteLine(orderNumber.Length);
            //法二
            string orderNumberTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string orderNumber = orderNumberTime.PadLeft(16, '0');
            return orderNumber;
        }
    }
}
