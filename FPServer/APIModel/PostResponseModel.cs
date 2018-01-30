using FPServer.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FPServer.APIModel
{
    public class PostResponseModel
    {
        public APIResult Result { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> ExtResult { get => _ExtResult; set => _ExtResult = value; }

        private Dictionary<string, object> _ExtResult = new Dictionary<string, object>();
    }

}
