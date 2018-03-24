using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FPServer.APIModel;
using FPServer.Core;
using FPServer.Exceptions;
using FPServer.ModelInstance;
using System.Threading;
using System.Diagnostics;

namespace FPServer.Controllers
{
    public partial class APIController
    {
        private static class Utils
        {
            public static PostResponseModel _AddRecord(PostInparamModel value)
            {
                try
                {
                    using (var server = FrameCorex.GetService())
                    {
                        server.UserLogin(value.LID, value.PWD);
                        Userx User = FrameCorex.ServiceInstanceInfo(server).User;
                        if (string.IsNullOrEmpty(User.Origin.LID) || string.IsNullOrEmpty(User.Origin.PWD))
                            throw new FPException("请先补全注册信息");
                        foreach (var t in value.Params)
                            if (t.Value != null)
                                User.Records[t.Key] = t.Value;
                        return new PostResponseModel()
                        {
                            Message = "Record add successs",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }

                }
                catch (UserNotfindException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                catch (UserLoginPermissionException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                catch (UserPwdErrorException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
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

            public static PostResponseModel _Regist(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        if (server.CheckUserNotExist(value.LID))
                        {
                            server.UserRegist(value.LID, value.PWD);
                        }
                        else
                        {
                            return new PostResponseModel()
                            {
                                Message = "User already exsist",
                                Result = Enums.APIResult.Error
                            };
                        }
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
                return new PostResponseModel()
                {
                    Message = "User regist success,welcome ",
                    Result = Enums.APIResult.Success
                };
            }

            public static PostResponseModel _GetRecord(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        var tarres = new PostResponseModel()
                        {
                            Message = "Excute record query success",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken,
                            ExtResult = { }
                        };
                        if (value.Params != null)
                            foreach (var t in value.Params.Keys)
                            {
                                tarres.ExtResult.Add(t, user.Records[t]);
                            }
                        return tarres;

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

            public static PostResponseModel _DeleteRecord(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        server.UserLogin(value.LID, value.PWD);
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        foreach (var t in value.Params.Values)
                        {
                            user.Records.Delete(t);
                        }
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
                return new PostResponseModel()
                {
                    Message = "Delete record success",
                    Result = Enums.APIResult.Success
                };
            }

            public static PostResponseModel _Login(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
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

            public static PostResponseModel _InfoModify(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        #region name
                        if (value.Params.ContainsKey("name"))
                        {
                            user.Infos.Name = value.Params["name"];
                        }
                        #endregion

                        #region pwd
                        if (value.Params.ContainsKey("pwd"))
                        {
                            string pwd = value.Params["pwd"];
                            if (!string.IsNullOrEmpty(pwd))
                            {
                                if(!server.UserChangePassword(value.PWD, pwd))
                                {
                                    return new PostResponseModel()
                                    {
                                        Message = "密码修改失败",
                                        Result = Enums.APIResult.Error,
                                        ExtResult = { { "Name", user.Infos.Name } },
                                        UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                                    };
                                }
                            }
                        }
                        #endregion
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
