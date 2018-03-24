using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPServer.Helper
{
    public class StringByteHelper
    {
        public static string GetStringFromBytes(byte[] ba) => Convert.ToBase64String(ba, Base64FormattingOptions.InsertLineBreaks);
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < ba.Length; i++)
        //{
        //    sb.Append(Convert.ToChar(ba[i]));
        //}
        //return sb.ToString();

        public static byte[] GetBytesFromString(string str) => Convert.FromBase64String(str);
        //List<byte> lb = new List<byte>();
        //for (int i = 0; i < str.Length; i++)
        //{
        //    lb.Add(Convert.ToByte(str[i]));
        //}
        //return lb.ToArray();

    }

    public class StringByteHelperBase
    {
        private static readonly char[] CharCollection = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        public static string GetStringFromBytes(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(CharCollection[ba[i]/16]+ CharCollection[ba[i] % 16]);
            }
            return sb.ToString();
        }
    }
}
