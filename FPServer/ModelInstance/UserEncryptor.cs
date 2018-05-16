using FPServer.Helper;
using FPServer.Interfaces;
using System;

namespace FPServer.ModelInstance
{

        class UserEncryptor : IEncryptor
        {
            private Userx User;

            private AESProvider AESobj
            {
                get
                {
                    if (_AESobj == null&& !string.IsNullOrEmpty(User.Origin.LID))
                    {
                        var ivhash = new HashProvider(HashProvider.HashAlgorithms.MD5);
                        byte[] _iv = ivhash.Hashbytes(User.Origin.LID);

                        string ranstr = AssetsController.getLocalSequenceString(User.Origin.ID);
                        string kstr1 = ranstr + User.Origin.LID ;

                        var keyhash = new HashProvider();
                        byte[] _key = new byte[32];
                        byte[] btar = keyhash.Hashbytes(kstr1);
                        Array.Copy(btar, 0, _key, 0, 32);
                        _AESobj = new AESProvider(_iv, _key);

                    }


                    return _AESobj;
                }
            }
            AESProvider _AESobj;

            internal UserEncryptor(Userx User)
            {
                this.User = User;
            }

            public string Decrypt(string metaStr)
            {
                return AESobj.Decrypt(metaStr);
            }

            public string Encrypt(string metaStr)
            {
                return AESobj.Encrypt(metaStr); ;
            }
        }


    

}
