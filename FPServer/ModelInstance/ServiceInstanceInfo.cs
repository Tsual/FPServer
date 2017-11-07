using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.ModelInstance
{
    public class ServiceInstanceInfo
    {
        private DateTime _DuoTime = new DateTime();
        private bool _IsLogin = false;
        private Userx _User = null;


        public bool IsLogin { get => _IsLogin; set => _IsLogin = value; }
        public Userx User { get => _User; set => _User = value; }
        public DateTime DuoTime { get => _DuoTime; set => _DuoTime = value; }
    }
}
