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

namespace FPServer.Controllers
{
    [Produces("application/json")]
    [Route("api/API")]
    public partial class APIController : Controller
    {

        [HttpGet]
        public PostInparamModel Get()
        {
            string token = "";
            using (var server = FrameCorex.GetService())
            {
                server.UserLogin("test1", "test1");
                token = server.Info.ToString();
                server.Info.EncryptToken = "test";
                server.Info.DisposeInfo = false;
            }
            Thread.Sleep(2000);
            using (var server = FrameCorex.RecoverService(token,(c)=> { }))
            {
                server.Info.EncryptToken = "check";
            }

            return null;
        }

        // GET: api/API/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/API
        [HttpPost]
        public PostResponseModel Post([FromBody]PostInparamModel value)
        {
            if (value == null) return null;
            if (!value.InparamCheck())
                return new PostResponseModel()
                {
                    Message = "Missing value, APIOperation Enum:" + String.Join(",", Enum.GetNames(typeof(Enums.APIOperation))),
                    Result = Enums.APIResult.Error
                };
            switch (value.Operation)
            {
                case Enums.APIOperation.AddRecord:
                    return Utils._AddRecord(value);
                case Enums.APIOperation.Regist:
                    return Utils._Regist(value);
                case Enums.APIOperation.GetRecord:
                    return Utils._GetRecord(value);
                case Enums.APIOperation.DeleteRecord:
                    return Utils._DeleteRecord(value);
            }
            return new PostResponseModel()
            {
                Message = "Operation not support",
                Result = Enums.APIResult.Error
            };


        }



        // PUT: api/API/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
