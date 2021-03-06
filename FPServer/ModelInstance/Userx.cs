﻿using System.Xml.Serialization;
using System.IO;
using System.Text;
using FPServer.Models;
using FPServer.Helper;
using FPServer.Interfaces;
using System.Diagnostics;

namespace FPServer.ModelInstance
{
    public partial class Userx
    {

        public static string HashOripwd(string LID, string PWD_ori) => new HashProvider().Hash(LID + PWD_ori);


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
            Debug.WriteLine("User Instance Active!<<LID : " + obj.LID);
            return res;
        }

        #region 密钥类

        [XmlIgnore]
        UserEncryptor _Encryptor;
        [XmlIgnore]
        public IEncryptor Encryptor
        {
            get
            {
                if (_Encryptor == null)
                    _Encryptor = new UserEncryptor(this);
                return _Encryptor;
            }
        }
        #endregion
        #region 原始对象
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
        #region 人脸识别
        public void SaveFaceRcognizer(string filepath)
        {
            Emgu.EmguInvoker.Current.Train(filepath, _Origin.ID);
        }




        #endregion  

        public void SaveInfos()
        {
            AppDbContext db = new AppDbContext();
            db.Entry((UserModel)this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        [XmlIgnore]
        private IUserRecordInstance _Records;

        [XmlIgnore]
        public IUserRecordInstance Records
        {
            get
            {
                if (_Records == null)
                    _Records = new UserRecordInstance(this);
                return _Records;
            }
        }


    }

}
