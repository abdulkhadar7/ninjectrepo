using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NInject.Business.Logins;
using NInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NInject.API.Controllers
{
    public class testclass : OAuthAuthorizationServerProvider
    {
        private IAccountService _accountService;

        public testclass()
        {

        }

        public testclass(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            LoginModel login = new LoginModel { password = context.Password, username = context.UserName };
            var user = _accountService.Login(login);

            return Task.Factory.StartNew(() =>
            {



                if (user != null)
                {
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, Convert.ToString(user.userId)),
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Email, user.userName)
            };
                    foreach (var role in user.Rolenames)
                        claims.Add(new Claim(ClaimTypes.Role, role));

                    var data = new Dictionary<string, string>
                    {
                        { "userName", user.userName },
                        { "roles", string.Join(",", user.Rolenames)}
                    };
                    var properties = new AuthenticationProperties(data);

                    ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                        Startup.OAuthOptions.AuthenticationType);

                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect");
                }
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }


        
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}