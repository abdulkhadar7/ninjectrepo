using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using NInject.API.Controllers;
using NInject.API.Providers;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NInject.API
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        
        public void Configuration(IAppBuilder app)
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthCustomeTokenProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                AllowInsecureHttp = true,
                RefreshTokenProvider = new OAuthCustomRefreshTokenProvider()
            };
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}