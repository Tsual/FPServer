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

namespace FPServer.Controllers
{
    [Produces("application/json")]
    [Route("api/API")]
    public partial class APIController : Controller
    {
        // GET: api/API
        //[HttpGet]
        //public PostInparamModel Get()
        //{
        //    return new PostInparamModel()
        //    {
        //        LID = "test",
        //        Operation = Enums.APIOperation.test,
        //        Params = new Dictionary<string, string>() { { "test", "test" } },
        //        PWD = "test"
        //    };
        //}

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
            if (!value.InparamCheck())
                return new PostResponseModel()
                {
                    Message = "missing value",
                    Result = Enums.APIResult.Error
                };
            switch (value.Operation)
            {
                case Enums.APIOperation.AddRecord:
                    return Utils._AddRecord(value);
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
