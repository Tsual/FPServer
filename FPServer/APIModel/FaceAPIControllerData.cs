using FPServer.Enums;
using Microsoft.AspNetCore.Http;

namespace FPServer.APIModel
{
    public class FaceAPIControllerData
    {
        private IFormFile image;
        private FaceApiOperation operation;
        private string loginToken;
        private string lid;
        private string pwd;

        public IFormFile Image { get => image; set => image = value; }
        public FaceApiOperation Operation { get => operation; set => operation = value; }
        public string LoginToken { get => loginToken; set => loginToken = value; }
        public string Lid { get => lid; set => lid = value; }
        public string Pwd { get => pwd; set => pwd = value; }
    }
}
