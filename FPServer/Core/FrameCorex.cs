using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FPServer.Interfaces;
using FPServer.ModelInstance;
using FPServer.Models;
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

        internal static bool _CheckLIDPWD(string LID,string PWD)
        {
            AppDbContext db = new AppDbContext();
            string PWD_ori_hash = Userx.HashOripwd("Guest", "Guest");
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
