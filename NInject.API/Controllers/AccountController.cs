using NInject.Business.Logins;
using NInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NInject.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private IAccountService _accountService;

        public AccountController()
        {

        }

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public HttpResponseMessage AddNewUser(HttpRequestMessage request,AddNewUserModel userModel)
        {
            HttpResponseMessage response = null;
            try
            {
               string userid= _accountService.AddNewUser(userModel);
               response = request.CreateResponse(HttpStatusCode.OK,userid);
            }
            catch (Exception ex)
            {

                response = request.CreateResponse(HttpStatusCode.NotImplemented,ex.Message);
            }
            return response;
        }
    }
}
