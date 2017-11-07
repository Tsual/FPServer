using System.Xml.Serialization;
using System.IO;
using System.Text;
using FPServer.Models;
using FPServer.Helper;

namespace FPServer.ModelInstance
{
    public partial class Userx
    {
        public static string HashOripwd(string LID, string PWD_ori)
        {
            string str1 = LID + PWD_ori;
            string str2 = PWD_ori + LID;
            var hashobj = new HashProvider();
            string hstr1 = hashobj.Hash(str1);
            string hstr2 = hashobj.Hash(str2);
            return hstr1 + hstr2;
        }

        public static implicit operator UserModel(Userx obj)
        {
            UserModel res = obj._Origin;
            using (var ms = new MemoryStream())
            {
                var serobj = new XmlSerializer(typeof(Info));
                serobj.Serialize(ms, obj._Infos);
                res.EXT = Encoding.UTF8.GetString(ms.ToArray());
            }
            return res;
        }

        public static implicit operator Userx(UserModel obj)
        {
            Userx res = new Userx();
            if (obj.EXT != "" && obj.EXT != null)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(obj.EXT)))
                {
                    var serobj = new XmlSerializer(typeof(Info));
                    var target = serobj.Deserialize(ms) as Info;
                    res._Infos = target ?? new Info();
                }
            }
            else
                res._Infos = new Info();
            res._Origin = obj;
            return res;
        }

        #region 密钥
        [XmlIgnore]
        private UserModel _Origin;

        [XmlIgnore]
        public UserModel Origin { get => _Origin; private set => _Origin = value; }

        #endregion
        #region 信息
        [XmlIgnore]
        private Info _Infos;

        public Info Infos { get => _Infos; set => _Infos = value; }
        #endregion

        public void SaveInfos()
        {
            AppDbContext db = new AppDbContext();
            db.Entry((UserModel)this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

    }




}
