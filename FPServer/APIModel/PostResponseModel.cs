using FPServer.Enums;
using System.Collections.Generic;

namespace FPServer.APIModel
{
    public class PostResponseModel
    {
        public APIResult Result { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> ExtResult { get; set; }
    }

}
