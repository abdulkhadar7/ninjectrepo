using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NInject.API.authorizefilters
{
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public string Roles { get; set; }
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
           var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                return Task.FromResult<object>(null);
            }

            
           var claimsIdentity = (ClaimsIdentity)actionContext.RequestContext.Principal.Identity;

           var RoleList = claimsIdentity.FindAll(s => s.Type == ClaimTypes.Role).(s=>s.Value).ToList();

            //var RoleList = string.Join(",", claimsIdentity.FindAll(s => s.Type == ClaimTypes.Role).Select(s => s.Value).ToList());

            if (!RoleList.Contains(Roles))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "User is not Authorized to access this Service");
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }
    }
}