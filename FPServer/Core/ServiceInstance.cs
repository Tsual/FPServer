using FPServer.Exceptions;
using FPServer.Helper;
using FPServer.ModelInstance;
using System.Linq;
using FPServer.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FPServer.Core
{
    public class ServiceInstance:IDisposable
    {
        AppDbContext db = new AppDbContext();
        DateTime _CreateTime;
        public DateTime CreateTime { get => _CreateTime; }

        internal ServiceInstance()
        {
            _CreateTime = DateTime.Now;
        }



        public void UserLogin(string LID,string PWD_ori)
        {
            var userset = (from t in db.M_UserModels
                          where t.LID==LID
                          select t).ToList();
            if (userset.Count() == 0) throw new UserNotfindException() { LID = LID };

            string PWD_ori_hash = Userx.HashOripwd(LID, PWD_ori);

            string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

            if (userset.ElementAt(0).PWD != PWD_ori_hash_aes) throw new UserPwdErrorException { LID = LID };

            var info=FrameCorex.GetServiceInstanceInfo(this);
            info.IsLogin = true;
            info.User = userset.ElementAt(0);

        }

        public void UserLogout()
        {
            FrameCorex.DropInstance(this);
        }

        public bool UserRegist(string LID, string PWD_ori)
        {
            if(UserRegist_CheckLIDNotExsist(LID))
            {
                string PWD_ori_hash = Userx.HashOripwd(LID, PWD_ori);
                string PWD_ori_hash_aes = FrameCorex.CurrnetAppEncryptor.Encrypt(PWD_ori_hash);

                Userx _Userx = new UserModel
                {
                    LID = LID,
                    PWD = PWD_ori_hash_aes
                };
                db.Entry((UserModel)_Userx).State = EntityState.Added;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果存在 返回false
        /// </summary>
        /// <param name="LID">Login ID</param>
        /// <returns></returns>
        public bool UserRegist_CheckLIDNotExsist(string LID)
        {
            var userset = (from t in db.M_UserModels
                          where t.LID == LID
                          select t).ToList();
            return userset.Count() == 0;
        }

        public void Dispose()
        {
            FrameCorex.DropInstance(this);
        }
    }
}
