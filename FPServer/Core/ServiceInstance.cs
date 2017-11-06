using FPServer.Exceptions;
using FPServer.Helper;
using FPServer.ModelInstance;
using System.Linq;

namespace FPServer.Core
{
    public class ServiceInstance
    {
        internal ServiceInstance()
        {

        }

        public void UserLogin(string LID,string PWD_ori)
        {
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var userset = from t in db.M_UserModels
                          where t.LID==LID
                          select t;
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
            var info = FrameCorex.GetServiceInstanceInfo(this);
            info.IsLogin = false;
            info.User = null;
        }

        

    }
}
