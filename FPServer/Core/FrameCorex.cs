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
        private FrameCorex()
        {

        }

        static FrameCorex()
        {
            Config[Enums.AppConfigEnum.AppDBex] = DateTime.Now.ToShortDateString();
        }

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
        private static List<ServiceInstance> _AvaServiceInstances = new List<ServiceInstance>();

        internal static ServiceInstanceInfo GetServiceInstanceInfo(ServiceInstance Instance)
        {
            return _ServiceInstances.ContainsKey(Instance)
                ? _ServiceInstances[Instance]
                : throw new ServiceNotfindException() { Instance = Instance };
        }


        public static ServiceInstance getService()
        {
            if (_AvaServiceInstances.Count == 0)
                CreateInstance();
            var ins = _AvaServiceInstances[0];
            _ServiceInstances.Add(ins, new ServiceInstanceInfo());
            _AvaServiceInstances.Remove(ins);
            return ins;
        }

        private static void CreateInstance()
        {
            ServiceInstance res = new ServiceInstance();
            _AvaServiceInstances.Add(res);
        }

        internal static void DropInstance(ServiceInstance Instance)
        {
            _ServiceInstances.Remove(Instance);
            _AvaServiceInstances.Add(Instance);
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
