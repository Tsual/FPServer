using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Enums;
using FPServer.Interfaces;
using FPServer.Attribute;

namespace FPServer.APIModel
{
    public class PostInparamModel: IAPIModel
    {
        public string LID { get; set; }
        public string PWD { get; set; }
        public string Token { get; set; }
        public APIOperation Operation { get; set; }
        public Dictionary<string,string> Params { get; set; }

        public bool InparamCheck()
        {
            if (LID == null || LID == "") return false;
            if (PWD == null || PWD == "") return false;
            if (Operation == APIOperation.None) return false;
            return true;
        }
    }

}
