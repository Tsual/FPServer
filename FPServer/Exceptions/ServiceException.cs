using System;
using System.Collections.Generic;
using System.Linq;
using FPServer.Core;
using System.Threading.Tasks;

namespace FPServer.Exceptions
{
    public class ServiceException:Exception
    {
        private ServiceInstance _Instance = null;

        public ServiceInstance Instance { get => _Instance; set => _Instance = value; }
    }


    public class ServiceNotfindException: ServiceException
    {

    }

    public class ServiceTimeoutException : ServiceException
    {

    }
}
