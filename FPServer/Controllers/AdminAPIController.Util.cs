using FPServer.APIModel;
using FPServer.Core;
using FPServer.ModelInstance;
using System;
using System.Collections.Generic;

namespace FPServer.Controllers
{
    public partial class AdminAPIController
    {
        private class Util
        {
            public static PostResponseModel _GetAllUserState(AdminPostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.getService())
                    {
                        server.UserLogin(value.LID, value.PWD, Enums.Permission.Administor);
                        FrameCorex.GetServiceInstanceInfo(server).DisposeInfo = false;
                        var result = new PostResponseModel();
                        result.ExtResult.Add("Current users", Dealdct(FrameCorex.getCurrentUsers(server)));
                        result.ExtResult.Add("Interrupt users", Dealdct(FrameCorex.getInterruptUsers(server)));
                        return result;
                    }
                }
                catch (FPServer.Exceptions.FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            private static List<Dictionary<string, string>> Dealdct(IReadOnlyCollection<ServiceInstanceInfo> list)
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                foreach (var t in list)
                {
                    Dictionary<string, string> dics = new Dictionary<string, string>();
                    dics.Add("LID", t.User.Origin.LID);
                    dics.Add("IsLogin", t.IsLogin.ToString());
                    dics.Add("DuoTime", t.DuoTime.ToString());
                    dics.Add("DurTime", t.DurTime.ToString());
                    result.Add( dics);
                }
                return result;
            }

        }
    }
}