using FPServer.Exceptions;
using FPServer.Helper;
using FPServer.ModelInstance;
using System.Linq;
using FPServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using FPServer.Enums;
using FPServer.Interfaces;
using System.Diagnostics;
using Emgu.CV.Face;

namespace FPServer.Core
{
    public class ServiceInstance : IDisposable
    {
        AppDbContext db = new AppDbContext();
        DateTime _CreateTime;
        public DateTime CreateTime { get => _CreateTime; }
        public string HashMark { get; set; }
        public ServiceInstanceInfo Info { get => FrameCorex.ServiceInstanceInfo(this); }

        internal ServiceInstance()
        {
            _CreateTime = DateTime.Now;
        }

        public void UserLogin(string LID, string PWD_ori)
        {
            var userset = (from t in db.M_UserModels
                           where t.LID == LID
                           select t).ToList();
            if (userset.Count() == 0) throw new UserNotfindException() { LID = LID };

            Userx User = userset.ElementAt(0);

            if (User.Infos.UserPermission == Permission.root)
                throw new UserLoginPermissionException() { LID = LID };

            string PWD_ori_hash = Userx.HashOripwd(LID, PWD_ori);

            string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

            if (userset.ElementAt(0).PWD != PWD_ori_hash_aes) throw new UserPwdErrorException { LID = LID };
            var info = FrameCorex.InterruptedInfo(LID);
            if (info == null)
            {
                info = FrameCorex.ServiceInstanceInfo(this);
                info.IsLogin = true;
                info.User = User;
                info.EncryptToken = FrameCorex.CurrnetAppEncryptor.Encrypt((new HashProvider()).Hash(LID ));
            }
            else
            {
                FrameCorex.SetServiceInstanceInfo(this, info);
            }

        }

        public void UserLogin(string LID, string PWD_ori, Permission permission)
        {
            var userset = (from t in db.M_UserModels
                           where t.LID == LID
                           select t).ToList();
            if (userset.Count() == 0) throw new UserNotfindException() { LID = LID };

            Userx User = userset.ElementAt(0);

            if (User.Infos.UserPermission != permission)
                throw new UserLoginPermissionException() { LID = LID, RequirePermission = permission };

            string PWD_ori_hash = Userx.HashOripwd(LID, PWD_ori);

            string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

            if (userset.ElementAt(0).PWD != PWD_ori_hash_aes) throw new UserPwdErrorException { LID = LID };

            var info = FrameCorex.InterruptedInfo(LID);
            if (info == null)
            {
                info = FrameCorex.ServiceInstanceInfo(this);
                info.IsLogin = true;
                info.User = User;
                info.EncryptToken = FrameCorex.CurrnetAppEncryptor.Encrypt((new HashProvider()).Hash(LID + PWD_ori));
            }
            else
            {
                FrameCorex.SetServiceInstanceInfo(this, info);
            }
        }

        public void UserLogin(FaceRecognizer.PredictionResult res)
        {
            if (res.Distance > 1)
                throw new UserFaceLoginException();
            Userx User = db.M_UserModels.Find(res.Label);
            if (User == null)
                throw new UserNotfindException();
            ServiceInstanceInfo info=null;
            if(!string.IsNullOrEmpty(User.Origin.LID))
                info = FrameCorex.InterruptedInfo(User.Origin.LID);
            if (info == null)
            {
                info = FrameCorex.ServiceInstanceInfo(this);
                info.IsLogin = true;
                info.User = User;
                if (!string.IsNullOrEmpty(User.Origin.LID))
                {
                    info.EncryptToken = FrameCorex.CurrnetAppEncryptor.Encrypt((new HashProvider()).Hash(User.Origin.LID ));
                }
            }
            else
            {
                FrameCorex.SetServiceInstanceInfo(this, info);
            }
        }

        public void UserLogout()
        {
            this.Dispose();
        }

        /// <summary>
        /// 修改密码 该方法不会保存数据库
        /// </summary>
        /// <param name="old_pwd">旧密码 可为空</param>
        /// <param name="new_pwd">新密码</param>
        /// <returns></returns>
        public bool UserChangePassword(string old_pwd, string new_pwd)
        {
            var serverinfo = FrameCorex.ServiceInstanceInfo(this);
            string PWD_ori_hash = "";
            PWD_ori_hash = Userx.HashOripwd(serverinfo.User.Origin.LID, new_pwd);
            serverinfo.User.Origin.PWD = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
            return true;
        }

        public bool UserRegist(string LID, string PWD_ori)
        {
            if (CheckUserNotExist(LID) && PWD_ori != "")
            {
                string PWD_ori_hash = Userx.HashOripwd(LID, PWD_ori);
                string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

                Userx _Userx = new UserModel
                {
                    LID = LID,
                    PWD = PWD_ori_hash_aes
                };
                _Userx.Infos.UserPermission = Permission.User;
                db.Entry((UserModel)_Userx).State = EntityState.Added;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "ServiceIncetance " + Info;
        }


        public void GenerateEmptyUser()
        {
            Info.User = new UserModel();
            Info.User.Infos.UserPermission = Permission.User;
            db.Entry((UserModel)Info.User).State = EntityState.Added;
            db.SaveChanges();
        }

        public void DeleteCurrentUser()
        {
            db.Entry((UserModel)Info.User).State = EntityState.Deleted;
            db.SaveChanges();
            this.Info.User = null;
        }

        /// <summary>
        /// 需要admin+权限
        /// </summary>
        /// <param name="LID"></param>
        /// <param name="PWD"></param>
        /// <param name="_Permission">不能为root</param>
        public void CreateUser(string LID, string PWD, Permission _Permission)
        {
            if (_Permission == Permission.root) return;
            if ((int)FrameCorex.ServiceInstanceInfo(this).User.Infos.UserPermission > 1)
                throw new UserPermissionException()
                {
                    RequiredPermission = Permission.Administor
                };
            if (CheckUserNotExist(LID))
            {
                string PWD_ori_hash = Userx.HashOripwd(LID, PWD);
                string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

                Userx _Userx = new UserModel
                {
                    LID = LID,
                    PWD = PWD_ori_hash_aes
                };
                _Userx.Infos.UserPermission = _Permission;
                db.Entry((UserModel)_Userx).State = EntityState.Added;
                db.SaveChanges();
            }
            else
            {
                throw new UserLIDExsistException();
            }
        }

        /// <summary>
        /// 需要root系统权限
        /// </summary>
        /// <param name="LID"></param>
        /// <param name="PWD"></param>
        public void ChangePassword(string LID, string PWD)
        {
            if ((int)FrameCorex.ServiceInstanceInfo(this).User.Infos.UserPermission > 2)
                throw new UserPermissionException()
                {
                    RequiredPermission = Permission.root
                };
            if (CheckUserNotExist(LID))
            {
                string PWD_ori_hash = Userx.HashOripwd(LID, PWD);
                string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

                var user = (from t in db.M_UserModels
                            where t.LID == LID
                            select t).ToList()[0];

                user.PWD = PWD_ori_hash_aes;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 如果存在 返回false
        /// </summary>
        /// <param name="LID">Login ID</param>
        /// <returns></returns>
        public bool CheckUserNotExist(string LID)
        {
            var userset = (from t in db.M_UserModels
                           where t.LID == LID
                           select t).ToList();
            return userset.Count() == 0;
        }

        public bool CheckUserNotExist(int ID)
        {
            return db.M_UserModels.Find(ID) == null;
        }

        public void Dispose()
        {
            FrameCorex.DropInstance(this);
        }
    }
}
