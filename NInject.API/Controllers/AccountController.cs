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

        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginModel model)
        {
            HttpResponseMessage response = null;
            try
            {
                LoginSuccessModel data = _accountService.Login(model); 
                if(data!=null)
                {
                    string token = CreateToken(data);
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

        private string CreateToken(LoginSuccessModel model)
        {
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.userId));

            claims.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", model.Rolenames)));

            //foreach(var item in model.Rolenames)
            //{
            //    var roleclaims = new Claim(ClaimTypes.Role, item);
            //    claims.AddClaim(roleclaims);
            //}
            

            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier, model.userId)
            //});

            const string sec = "P6CZQPptm7y535swXauhWQga9JuC2KZGpRWmFXpcNLJfMwELBezkvtYpHERVfmtFf7RZjVqqJvSbQLsHJ8ddJ3NCAd5ueCsCySYZPvdGDkveV92qn2S2WZCMHFRPQ7tb";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

           
            //create the jwt
            var token = tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:53060", audience: "http://localhost:53060",
                        subject: claims, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
