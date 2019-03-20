using NInject.Business.Logins;
using NInject.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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

        [HttpPost]
        [Route("AddNewUser")]
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
        [HttpGet]
        public HttpResponseMessage verify(HttpRequestMessage request,string password)
        {
            var status = _accountService.verify(password);
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, status);
            return response;
        }

        public HttpResponseMessage Login(HttpRequestMessage request, LoginModel model)
        {
            HttpResponseMessage response = null;
            try
            {
                LoginSuccessModel data = _accountService.Login(model); 
                if(data!=null)
                {
                    string token = CreateToken(model.username);
                    response = request.CreateResponse(HttpStatusCode.OK, token);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {

                response = request.CreateResponse(HttpStatusCode.OK,ex.Message);
            }
            return response;
        }

        private string CreateToken(string username)
        {
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:53060", audience: "http://localhost:53060",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
