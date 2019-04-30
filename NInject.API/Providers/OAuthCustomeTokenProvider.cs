using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NInject.Business.Logins;
using NInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace NInject.API.Providers
{
    public class OAuthCustomeTokenProvider : OAuthAuthorizationServerProvider
    {
      
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            LoginModel login = new LoginModel { password = context.Password, username = context.UserName };
            LoginSuccessModel user = null;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            ServicePointManager.ServerCertificateValidationCallback = delegate
                      (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

           
            HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:53060/api/Account/LoginTest", login).Result;
            if(response.IsSuccessStatusCode)
            {
                user= response.Content.ReadAsAsync<LoginSuccessModel>().Result;
            }

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

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
        #endregion
    }
}