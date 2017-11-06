using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Interfaces;
using FPServer.ModelInstance;

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
    }
}
