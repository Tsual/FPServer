using System.Collections.Generic;
using FPServer.Enums;
using FPServer.Interfaces;

namespace FPServer.APIModel
{
    public class AdminPostInparamModel : IAPIModel
    {
        public string LID { get; set; }
        public string PWD { get; set; }
        public string Token { get; set; }
        public AdminAPIOperation Operation { get; set; }
        public Dictionary<string, string> Params { get; set; }

        public bool InparamCheck()
        {
            if ((Token == null || Token == "") && (LID == null || LID == "" || PWD == null || PWD == "")) return false;
            if (Operation == AdminAPIOperation.None) return false;
            return true;
        }
    }
}
