using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.ModelInstance
{
    public class ServiceInstanceInfo
    {
        private DateTime _CreateTime = new DateTime();

        public DateTime CreateTime { get => _CreateTime; set => _CreateTime = value; }
    }
}
