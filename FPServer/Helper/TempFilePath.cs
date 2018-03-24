using System;
using System.IO;

namespace FPServer.Helper
{
    public class TempFilePath:IDisposable
    {
        private string InnerString;

        public TempFilePath(string InnerString)
        {
            this.InnerString = InnerString;
        }

        public static implicit operator  string (TempFilePath tfp)
        {
            return tfp.InnerString;
        }

        public static explicit operator TempFilePath(string str)
        {
            return new TempFilePath(str);
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(InnerString))
                if (File.Exists(InnerString))
                    File.Delete(InnerString);
        }
    }
}
