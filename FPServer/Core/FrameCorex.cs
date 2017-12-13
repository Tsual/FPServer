using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Interfaces;
using FPServer.ModelInstance;
using FPServer.Models;
using FPServer.Exceptions;
using System.Threading;
using System.Diagnostics;

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
            _CheckCreateDeaultUser();
        }

        private static void _CheckCreateDeaultUser()
        {
            AppDbContext db = new AppDbContext();
            var guest = (from t in db.M_UserModels
                         where t.LID == "Guest"
                         select t).ToArray();
            if (guest.Length == 0)
            {
                string PWD_ori_hash = Userx.HashOripwd("Guest", "Guest");
                string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                Userx um = new UserModel()
                {
                    LID = "Guest",
                    PWD = PWD_ori_hash_aes
                };
                um.Infos.UserPermission = Enums.Permission.Guest;
                db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                Userx um = guest[0];
                if (um.Infos.UserPermission != Enums.Permission.Guest)
                {
                    um.Infos.UserPermission = Enums.Permission.Guest;
                    db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }

            var root = (from t in db.M_UserModels
                        where t.LID == "Root"
                        select t).ToArray();
            if (root.Length == 0)
            {

                string PWD_ori_hash = Userx.HashOripwd("Root", "Root");
                string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                Userx um = new UserModel()
                {
                    LID = "Root",
                    PWD = PWD_ori_hash_aes
                };
                um.Infos.UserPermission = Enums.Permission.root;
                db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            else
            {
                Userx um = root[0];
                if (um.Infos.UserPermission != Enums.Permission.root)
                {
                    um.Infos.UserPermission = Enums.Permission.root;
                    db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }

            db.SaveChanges();
        }

        internal static bool _CheckLIDPWD(string LID, string PWD)
        {
            AppDbContext db = new AppDbContext();
            string PWD_ori_hash = Userx.HashOripwd(LID, PWD);
            string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
            var user = (from t in db.M_UserModels
                        where t.LID == LID && t.PWD == PWD_ori_hash_aes
                        select t).ToArray();
            return user.Length == 1;
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
        private static Dictionary<string, KeyValuePair<DateTime, ServiceInstanceInfo>> _IntServiceInstancesInfos = new Dictionary<string, KeyValuePair<DateTime, ServiceInstanceInfo>>();

        private static Timer InfoCheckTimer = new Timer((o) =>
        {
            foreach (var t in (from t in _IntServiceInstancesInfos.Values
                               where Math.Abs((t.Key - DateTime.Now).Minutes) <= 1
                               select t).ToList())
            {
                _IntServiceInstancesInfos.Remove(t.Value.HashToken);
                Debug.WriteLine("Remove Timer Excute " + t);
            }
                
        }, null, 0, 800 * 1);


        internal static ServiceInstanceInfo GetServiceInstanceInfo(ServiceInstance Instance)
                => _ServiceInstances.ContainsKey(Instance)
                ? _ServiceInstances[Instance]
                : throw new ServiceNotfindException() { Instance = Instance };


        internal static bool CheckUserLogin(Userx User) => (from t in _ServiceInstances.Values
                                                            where t.User == User
                                                            select t).ToArray().Length > 0;

        internal static string _FindEncryptToken(Userx User) => CheckUserLogin(User) ? (from t in _ServiceInstances.Values
                                                                                        where t.User == User
                                                                                        select t).ToList()[0].EncryptToken : null;


        private static string _GetHashToken(ServiceInstance server)
        {
            var hashobj = new Helper.HashProvider();
            var ranstrobj = new Helper.RandomGenerator();
            string hashtoken = hashobj.Hash(ranstrobj.getRandomString(50));
            while(true)
            {
                bool vt = true;
                foreach (var t in _ServiceInstances.Keys)
                    if (t.Info.HashToken == hashtoken)
                        vt = false;
                if (vt) break;
                hashtoken = hashobj.Hash(ranstrobj.getRandomString(50));
            }
            return hashtoken;

        }

        public static ServiceInstance recoverService(string HashToken,Action<string> del)
        {
            var list = (from t in _IntServiceInstancesInfos.Values
                       where t.Value.ToString() == HashToken
                       select t.Value).ToList();

            ServiceInstanceInfo info = list.Count > 0 ? list[0] : null;
            if (info == null)
            {
                del.Invoke(HashToken);
                return getService();
            }
            return getService(info);
        }

        public static ServiceInstance getService(string HashToken)
        {
            foreach (var t in _ServiceInstances.Keys)
                if (t.Info.HashToken == HashToken)
                    return t;
            
            return getService();
        }

        public static ServiceInstance getService()
        {
            if (_AvaServiceInstances.Count == 0)
                CreateInstance();
            var ins = _AvaServiceInstances[0];
            _ServiceInstances.Add(ins, new ServiceInstanceInfo() { HashToken = _GetHashToken(ins) });
            _AvaServiceInstances.Remove(ins);
            return ins;
        }

        private static ServiceInstance getService(ServiceInstanceInfo info)
        {
            if (_AvaServiceInstances.Count == 0)
                CreateInstance();
            var ins = _AvaServiceInstances[0];
            _ServiceInstances.Add(ins, info);
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
            var info = GetServiceInstanceInfo(Instance);
            if (!info.DisposeInfo)
            {
                _IntServiceInstancesInfos.Add(info.HashToken, new KeyValuePair<DateTime, ServiceInstanceInfo>(DateTime.Now.AddMinutes(1), info));
            }
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
