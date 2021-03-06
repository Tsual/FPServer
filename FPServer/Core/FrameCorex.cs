﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Interfaces;
using FPServer.ModelInstance;
using FPServer.Models;
using FPServer.Exceptions;
using System.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Server;
using FPServer.Attribute;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace FPServer.Core
{
    /// <summary>
    /// 框架核心类
    /// </summary>
    public partial class FrameCorex
    {
        private static ILogger Logger = new ConsoleLoggerProvider(new ConsoleLoggerSettings()).CreateLogger("INFO-FrameCorex");


        private FrameCorex()
        {

        }


        static FrameCorex()
        {
            Config[Enums.AppConfigEnum.AppDBex] = DateTime.Now.ToShortDateString();
            UserHelper._CheckCreateDeaultUser();
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

        #region 
        private static Timer MaintenanceTimer = new Timer((o) =>
        {
            OnCirculationMaintenance?.Invoke();
            OnOnceMaintenance?.Invoke();
            OnOnceMaintenance = null;
        }, null, 0, 1000 * 60 * Convert.ToInt32(Config[Enums.AppConfigEnum.MaintenanceTime]));

        public static event Action OnCirculationMaintenance;
        public static event Action OnOnceMaintenance;

        #endregion

        #region 核心类互访方法
        internal static ServiceInstanceInfo ServiceInstanceInfo(ServiceInstance Instance)
                => _ServiceInstances.ContainsKey(Instance)
                ? _ServiceInstances[Instance]
                : throw new ServiceNotfindException() { Instance = Instance };

        internal static void SetServiceInstanceInfo(ServiceInstance Instance, ServiceInstanceInfo Info)
        {
            _ServiceInstances[Instance] = Info;
        }


        internal static ServiceInstanceInfo InterruptedInfo(string LID)
        {
            var ars = (from t in _IntServiceInstancesInfos
                       where t.Value.Value.User.Origin.LID == LID
                       select t).ToArray();
            if (ars.Length > 0)
            {
                _IntServiceInstancesInfos.Remove(ars[0].Key);
                return ars[0].Value.Value;
            }
            return null;
        }

        #endregion

        #region 生命周期管理
        #region ServiceInstanceInfo 缓存销毁

        /// <summary>
        /// 延时销毁集合
        /// </summary>
        private static Dictionary<string, KeyValuePair<DateTime, ServiceInstanceInfo>> _IntServiceInstancesInfos = new Dictionary<string, KeyValuePair<DateTime, ServiceInstanceInfo>>();

        /// <summary>
        /// 延时销毁计时器
        /// </summary>
        private static Timer InfoCheckTimer = new Timer((o) =>
        {
            foreach (var t in (from t in _IntServiceInstancesInfos.Values
                               where Math.Abs((t.Key - DateTime.Now).Minutes) <= 1
                               select t).ToList())
            {
                _IntServiceInstancesInfos.Remove(t.Value.LoginHashToken);
                Debug.WriteLine("Remove Timer Excute " + t);
            }

        }, null, 0, 800 * 1);

        public static List<ServiceInstanceInfo> CurrentUsers(ServiceInstance server) => _ServiceInstances.Values.ToList();

        public static List<ServiceInstanceInfo> InterruptUsers(ServiceInstance server) => (from t in _IntServiceInstancesInfos select t.Value.Value).ToList();
        #endregion
        #region ServiceInstance 实例获取
        public static ServiceInstance GetService(string HashToken)
        {
            foreach (var t in _ServiceInstances.Keys)
                if (t.Info.LoginHashToken == HashToken)
                    return t;

            return GetService();
        }

        public static ServiceInstance GetService()
        {
            if (_AvaServiceInstances.Count == 0)
                _CreateInstance();
            var ins = _AvaServiceInstances[0];
            _ServiceInstances.Add(ins, new ServiceInstanceInfo() { LoginHashToken = _AppHashToken(ins) });
            _AvaServiceInstances.Remove(ins);
            return ins;
        }

        private static ServiceInstance GetService(ServiceInstanceInfo info)
        {

            if (_AvaServiceInstances.Count == 0)
                _CreateInstance();
            var ins = _AvaServiceInstances[0];
            _ServiceInstances.Add(ins, info);
            _AvaServiceInstances.Remove(ins);

            Logger.Log<string>(LogLevel.Information, new EventId(), null, null, (o, ex) => { return "ServiceInstance-Recover-" + DateTime.Now; });
            return ins;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HashToken">HashToken</param>
        /// <param name="ServiceNotFindCallback">ServiceNotFind Callback</param>
        /// <returns></returns>
        public static ServiceInstance RecoverService(string HashToken, Action<string> ServiceNotFindCallback)
        {
            var list = (from t in _IntServiceInstancesInfos.Values
                        where t.Value.LoginHashToken == HashToken
                        && t.Value.LoginHashToken != null
                        select t.Value).ToList();

            ServiceInstanceInfo info = list.Count > 0 ? list[0] : null;
            if (info == null)
            {
                ServiceNotFindCallback.Invoke(HashToken);
                return GetService();
            }
            return GetService(info);
        }



        private static void _CreateInstance()
        {
            ServiceInstance res = new ServiceInstance();
            _AvaServiceInstances.Add(res);
        }
        #endregion


        internal static void DropInstance(ServiceInstance Instance)
        {
            var info = ServiceInstanceInfo(Instance);
            if (!info.DisposeInfo)
            {
                if (_IntServiceInstancesInfos.ContainsKey(info.LoginHashToken))
                    _IntServiceInstancesInfos.Remove(info.LoginHashToken);
                _IntServiceInstancesInfos.Add(info.LoginHashToken,
                    new KeyValuePair<DateTime, ServiceInstanceInfo>(
                        DateTime.Now.AddMinutes(Convert.ToDouble(Config[Enums.AppConfigEnum.ServiceDropTime])), info));
            }

            if (_AvaServiceInstances.Count <
                Convert.ToDouble(AppConfigs.Current[Enums.AppConfigEnum.ServiceInstanceObjectDestroylimit]) * _ServiceInstances.Count)
                _AvaServiceInstances.Add(Instance);
            _ServiceInstances.Remove(Instance);
        }
        #endregion




        #endregion

        #region AppEncryptor

        private static string _AppHashToken(ServiceInstance server)
        {
            var hashobj = new Helper.HashProvider();
            var ranstrobj = new Helper.RandomGenerator();
            server.HashMark = hashobj.Hash(ranstrobj.getRandomString(50));
            while (true)
            {
                bool vt = true;
                foreach (var t in _ServiceInstances.Keys)
                    if (t.Info.LoginHashToken == server.HashMark)
                        vt = false;
                if (vt) break;
                server.HashMark = hashobj.Hash(ranstrobj.getRandomString(50));
            }
            return server.HashMark;
        }

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
