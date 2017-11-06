using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FPServer.Helper
{
    public class HashProvider
    {
        HashAlgorithm _Hash = null;

        public enum HashAlgorithms { MD5, SHA256, SHA512 ,SHA128}

        public HashProvider()
        {
            _Hash = SHA256.Create();
        }
        public HashProvider(HashAlgorithms HashAlgorithm)
        {
            switch (HashAlgorithm)
            {
                case HashAlgorithms.MD5: _Hash = MD5.Create(); break;
                case HashAlgorithms.SHA512: _Hash = SHA512.Create(); break;
                case HashAlgorithms.SHA256: _Hash = SHA256.Create(); break;
            }

        }


        public string Hash(string str)
        {
            return gets(_Hash.ComputeHash(getb(str)));
        }

        public byte[] Hashbytes(string str)
        {
            return _Hash.ComputeHash(getb(str));
        }

        static string gets(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToChar(ba[i]));
            }
            return sb.ToString();
        }
        static byte[] getb(string str)
        {
            List<byte> lb = new List<byte>();
            for (int i = 0; i < str.Length; i++)
            {
                lb.Add(Convert.ToByte(str[i]));
            }
            return lb.ToArray();
        }
    }
}
