using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FPServer.APIModel;
using FPServer.Enums;

namespace FPServer.Controllers
{
    [Produces("application/json")]
    [Route("api/AdminAPI")]
    public partial class AdminAPIController : Controller
    {
        [HttpPost]
        public PostResponseModel Post([FromBody]AdminPostInparamModel value)
        {
            if (value == null) return null;
            if (!value.InparamCheck())
                return new PostResponseModel()
                {
                    Message = "missing value Operation Enum:" + String.Join(",", Enum.GetNames(typeof(AdminAPIOperation))),
                    Result = APIResult.Error
                };

            switch (value.Operation)
            {
                case AdminAPIOperation.GetAllUserState:
                    return Util._GetAllUserState(value);
                case AdminAPIOperation.GetServerState:
                    return Util._GetServerState(value);

            }
            return new PostResponseModel()
            {
                Message = "Operation not support",
                Result = APIResult.Error
            };
        }
    }
}