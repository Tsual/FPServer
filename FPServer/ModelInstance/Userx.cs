using System.Xml.Serialization;
using System.IO;
using System.Text;
using FPServer.Models;

namespace FPServer.ModelInstance
{
    public class Userx
    {

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
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(obj.EXT)))
            {
                var serobj = new XmlSerializer(typeof(Info));
                var target = serobj.Deserialize(ms) as Info;
                res._Infos = target;
                res._Origin = obj;
            }
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


        public class Info
        {
            [XmlIgnore]
            private string _Remark = "";

            public string Remark { get => _Remark; set => _Remark = value; }
        }
        #endregion


    }




}
