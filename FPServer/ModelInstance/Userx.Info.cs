using System.Xml.Serialization;
using FPServer.Enums;

namespace FPServer.ModelInstance
{
    public partial class Userx
    {
        public class Info
        {
            [XmlIgnore]
            private string _Remark = "";
            [XmlIgnore]
            private string _Remark2 = "";
            [XmlIgnore]
            private Permission _UserPermission = 0;
            [XmlIgnore]
            private string _Name = "";
            [XmlIgnore]
            private bool _HasFaceData = false;

            public string Remark { get => _Remark; set => _Remark = value; }
            public string Remark2 { get => _Remark2; set => _Remark2 = value; }
            public Permission UserPermission { get => _UserPermission; set => _UserPermission = value; }
            public string Name { get => _Name; set => _Name = value; }
            public bool HasFaceData { get => _HasFaceData; set => _HasFaceData = value; }
        }

    }




}
