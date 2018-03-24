using Emgu.CV;
using Emgu.CV.CvEnum;
using FPServer.APIModel;
using FPServer.Attribute;
using FPServer.Enums;
using FPServer.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.Helper
{
    public static class ExtendFunctions
    {
        public static string GetFirstValue(this IFormCollection Form,string key)
        {
            if (Form.ContainsKey(key))
                if (Form[key].Count > 0)
                    return Form[key][0];
            return null;
        }

        public static FaceAPIControllerData GenerateFaceAPIControllerData(this HttpRequest Request)
        {
            Request.ReadFormAsync().Wait();
            return new FaceAPIControllerData()
            {
                Image = Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null,
                Operation = Request.Form.GetFirstValue("OP").TryParse_Enum_FaceApiOperation(),
                LoginToken = Request.Form.GetFirstValue("LoginToken"),
                Lid = Request.Form.GetFirstValue("LID"),
                Pwd = Request.Form.GetFirstValue("PWD")
            };
        }

        public static FaceApiOperation TryParse_Enum_FaceApiOperation(this string value)
        {
            if (Enum.TryParse(typeof(FaceApiOperation), value, out object res))
            {
                return (FaceApiOperation)res;
            }
            throw new FPException("Operation not support, APIOperation Enum:" + String.Join(",", Enum.GetNames(typeof(FaceApiOperation))));
        }

        public static string ConvertBase64StringFormat(this string obj)
        {
           return StringByteHelperBase.GetStringFromBytes(Convert.FromBase64String(obj));
        }

        public static Mat UnionSize(this Mat obj)
        {
            var res = new Mat();
            CvInvoke.Resize(obj, res, new System.Drawing.Size(1000, 1000));
            return res;
        }

        public static void EqualizeHist(this Mat obj)
        {
            CvInvoke.EqualizeHist(obj, obj);
        }

        public static Mat CvtColor(this Mat obj, ColorConversion code, int dstCn = 0)
        {
            Mat res = new Mat();
            CvInvoke.CvtColor(obj, res, ColorConversion.Bgr2Gray, dstCn);
            return res;
        }

        public static TempFilePath ToTempFilePath(this string obj)
        {
            return new TempFilePath(obj);
        }
    }
}
