using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WMSClient.Common
{
    public static class ByteExtension
    {
        public static string ByteArrayToHexString(this byte[] bytes)
        {
            var hex = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            return hex;
        }
        public static object DeserializeWithBinary(this byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }
}
