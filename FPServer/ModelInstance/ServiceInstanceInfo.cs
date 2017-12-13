using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPServer.ModelInstance
{
    public class ServiceInstanceInfo
    {
        [XmlIgnore]
        private DateTime _DuoTime = new DateTime();

        [XmlIgnore]
        private bool _IsLogin = false;

        [XmlIgnore]
        private Userx _User = null;

        [XmlIgnore]
        private string _EncryptToken = null;

        [XmlIgnore]
        private bool _DisposeInfo = true;

        [XmlIgnore]
        private string _HashToken = "";

        public bool IsLogin { get => _IsLogin; set => _IsLogin = value; }
        public Userx User { get => _User; set => _User = value; }
        public DateTime DuoTime { get => _DuoTime; set => _DuoTime = value; }
        public string EncryptToken { get => _EncryptToken; set => _EncryptToken = value; }
        public bool DisposeInfo { get => _DisposeInfo; set => _DisposeInfo = value; }
        public string HashToken { get => _HashToken; set => _HashToken = value; }

        public override string ToString()
        {
            return "User: "+User?.Origin?.LID+ ",HashToken: "+ HashToken?.ToString()+ ",DisposeInfo: "+ DisposeInfo;
        }
    }
}
