using System.Xml.Serialization;

namespace FPServer.ModelInstance
{
    public partial class Userx
    {
        public class Info
        {
            [XmlIgnore]
            private string _Remark = "";

            public string Remark { get => _Remark; set => _Remark = value; }
        }

    }




}
