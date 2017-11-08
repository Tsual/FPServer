using FPServer.Core;
using FPServer.Exceptions;
using FPServer.Interfaces;
using FPServer.Models;
using System.Linq;
using System;

namespace FPServer.ModelInstance
{
    public class UserRecordInstance : IUserRecordInstance
    {
        private string _LID
        {
            get => userx.Origin.LID;
        }
        private Userx userx;
        private AppDbContext db = new AppDbContext();


        internal UserRecordInstance(ServiceInstance Instance)
        {
            userx = FrameCorex.GetServiceInstanceInfo(Instance).User;
        }

        internal UserRecordInstance(Userx User)
        {
            if (!FrameCorex.CheckUserLogin(User))
                throw new UserNotLoginException() { User = User };
            userx = User;
        }

        private string _GetRecord(string key)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 1) return userx.Encryptor.Decrypt(lis[0].Value);
            return null;
        }

        private void _SetRecord(string key, string value)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 1)
            {
                var ins = lis[0];
                ins.Value = userx.Encryptor.Encrypt(value);
                db.Entry(ins).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                var ins = new UserRecordModel()
                {
                    LID = _LID,
                    Key = key,
                    Value = userx.Encryptor.Encrypt(value)
                };
                db.Entry(ins).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            db.SaveChanges();
        }

        public string this[string key]
        {
            get
            {
                return _GetRecord(key);
            }
            set
            {
                _SetRecord(key, value);
            }
        }

    }

}
