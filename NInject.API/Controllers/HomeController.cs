using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NInject.API.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "Running ";
        }
    }
}
