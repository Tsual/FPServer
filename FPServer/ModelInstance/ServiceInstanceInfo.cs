using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.ModelInstance
{
    public class ServiceInstanceInfo
    {
        private DateTime _CreateTime = new DateTime();
        private bool _IsLogin = false;
        private Userx _User = null;

        public DateTime CreateTime { get => _CreateTime; set => _CreateTime = value; }
        public bool IsLogin { get => _IsLogin; set => _IsLogin = value; }
        public Userx User { get => _User; set => _User = value; }
    }
}
