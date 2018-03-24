using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FPServer.APIModel;
using FPServer.Enums;
using FPServer.Helper;
using FPServer.Exceptions;

namespace FPServer.Controllers
{
    //[Produces("application/json")]
    [Route("api/FaceAPI")]
    public partial class FaceAPIController : Controller
    {
        [HttpPost]
        public PostResponseModel Post()
        {
            try
            {
                var dataPac = Request.GenerateFaceAPIControllerData();
                if (dataPac.Image == null)
                    return new PostResponseModel
                    {
                        Message = "调用失败",
                        Result = APIResult.Error
                    };
                return typeof(Utils).GetMethod("_" + dataPac.Operation.ToString())?
                    .Invoke(null, new object[] { dataPac }) as PostResponseModel;
            }
            catch (FPException ex)
            {
                return new PostResponseModel()
                {
                    Message = ex.Message,
                    Result = APIResult.Error
                };
            }
        }
    }




}