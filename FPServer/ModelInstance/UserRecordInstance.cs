using FPServer.Core;
using FPServer.Exceptions;
using FPServer.Interfaces;
using FPServer.Models;
using System.Linq;

namespace FPServer.ModelInstance
{
    public class UserRecordInstance : IUserRecordInstance
    {
        private string _LID;
        private AppDbContext db = new AppDbContext();
        public UserRecordInstance(string LID, string PWD)
        {
            if (!FrameCorex._CheckLIDPWD(LID, PWD))
                throw new UserPwdErrorException() { LID = LID };
            _LID = LID;
        }

        public UserRecordInstance(Userx user)
        {
            _LID = user.Origin.LID;
        }

        private string _GetRecord(string key)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 1) return lis[0].Value;
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
                ins.Value = value;
                db.Entry(ins).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                var ins = new UserRecordModel()
                {
                    LID = _LID,
                    Key = key,
                    Value = value
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
