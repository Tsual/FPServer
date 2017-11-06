using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Interfaces;
using FPServer.ModelInstance;

using FPServer.Exceptions;

namespace FPServer.Core
{
    public class FrameCorex
    {
        #region Config
        public static IAppConfigs Config
        {
            get
            {
                return AppConfigs.Current;
            }
        }

        #endregion
        
        #region Service
        private static Dictionary<ServiceInstance, ServiceInstanceInfo> _ServiceInstances = new Dictionary<ServiceInstance, ServiceInstanceInfo>();

        internal static ServiceInstanceInfo GetServiceInstanceInfo(ServiceInstance Instance)
        {
            return _ServiceInstances.ContainsKey(Instance)
                ? _ServiceInstances[Instance]
                : throw new ServiceNotfindException() { Instance = Instance };
        }


        public static ServiceInstance getService()
        {
            var service = new ServiceInstance();
            var serviceinfo = new ServiceInstanceInfo()
            {
                CreateTime = DateTime.Now
            };
            _ServiceInstances.Add(service, serviceinfo);
            return service;
        }

        #endregion

        #region AppEncryptor
        public static IEncryptor CurrnetAppEncryptor
        {
            get
            {
                return AppEncryptor.Current;
            }
        }
        #endregion
    }
}
