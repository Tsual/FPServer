using FPServer.APIModel;
using FPServer.Core;
using FPServer.Exceptions;
using System.Diagnostics;
using System.IO;
using FPServer.Helper;
using System;

namespace FPServer.Controllers
{
    public partial class FaceAPIController
    {
        private static class Utils
        {
            public static PostResponseModel _AppendFace(FaceAPIControllerData value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.LoginToken, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    using (var tempfilepath = ("Emgu\\Temp\\" + server.HashMark.ConvertBase64StringFormat()).ToTempFilePath())
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.Lid, value.Pwd);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;

                        using (var fs = new FileStream(tempfilepath, FileMode.OpenOrCreate))
                        {
                            value.Image.CopyTo(fs);
                            fs.Flush();
                        }

                        user.SaveFaceRcognizer(tempfilepath);
                        user.Infos.HasFaceData = true;



                        return new PostResponseModel()
                        {
                            Message = "成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "Name", user.Infos.Name } },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            public static PostResponseModel _PrediectLogin(FaceAPIControllerData value)
            {
                try
                {
                    using (var server = FrameCorex.GetService())
                    using (var tempfilepath = ("Emgu\\Temp\\" + server.HashMark.ConvertBase64StringFormat()).ToTempFilePath())
                    {
                        using (var fs = new FileStream(tempfilepath, FileMode.OpenOrCreate))
                        {
                            value.Image.CopyTo(fs);
                            fs.Flush();
                        }
                        try
                        {
                            var regRes = Emgu.EmguInvoker.Current.Predict(tempfilepath);
                            server.UserLogin(regRes);
                        }
                        catch (FPException ex) { throw ex; }
                        catch (Exception) { throw new UserFaceLoginException(); }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;


                        return new PostResponseModel()
                        {
                            Message = "成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "Name", user.Infos.Name } },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            public static PostResponseModel _PrediectRegist(FaceAPIControllerData value)
            {
                try
                {
                    using (var server = FrameCorex.GetService())
                    using (var tempfilepath = ("Emgu\\Temp\\" + server.HashMark.ConvertBase64StringFormat()).ToTempFilePath())
                    {
                        using (var fs = new FileStream(tempfilepath, FileMode.OpenOrCreate))
                        {
                            value.Image.CopyTo(fs);
                            fs.Flush();
                        }
                        try
                        {
                            var regRes = Emgu.EmguInvoker.Current.Predict(tempfilepath);
                            if (!server.CheckUserNotExist(regRes.Label))
                                throw new FPException("你注册过了，别闹");
                        }
                        catch (FPException ex) { throw ex; }
                        catch (Exception) { }

                        server.GenerateEmptyUser();
                        var user = FrameCorex.ServiceInstanceInfo(server).User;

                        try
                        {
                            user.SaveFaceRcognizer(tempfilepath);
                        }
                        catch (Exception)
                        {
                            server.DeleteCurrentUser();
                            throw new FPException("识别失败");
                        }

                        if (!string.IsNullOrEmpty(value.Lid))
                            user.Origin.LID = value.Lid;

                        if (!string.IsNullOrEmpty(value.Pwd)) {
                            server.UserChangePassword(null, value.Pwd);
                        }

                        user.SaveInfos();

                        return new PostResponseModel()
                        {
                            Message = "成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "Name", user.Infos.Name } },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };

                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }

            }


        }
    }




}