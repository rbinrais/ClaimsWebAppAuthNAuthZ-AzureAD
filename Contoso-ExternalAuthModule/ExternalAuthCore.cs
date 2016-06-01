using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Contoso_ExternalAuthModule
{
    public class ExternalAuthModuleAuthorizeAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }
        public string Provider { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var claims = (ClaimsIdentity)httpContext.User.Identity;

                return CheckUserAccess(claims);
            }
           
            return false;
 
        }

        
        private bool CheckUserAccess(ClaimsIdentity claims)
        {
            //Allow user in Admin role and lives at particular address
            if (claims.FindFirst(ClaimTypes.Role).Value == Role &&
                claims.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value.ToLowerInvariant().Contains(Provider.ToLowerInvariant())
                )
            {
                return true;
            }
            
            return false;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //When user is not authenicated send them to login page, otherwise take them to access denied page
            //Recommended reading:http://prideparrot.com/blog/archive/2012/6/customizing_authorize_attribute http://stackoverflow.com/questions/238437/why-does-authorizeattribute-redirect-to-the-login-page-for-authentication-and-aut
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)

            {

                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {

                filterContext.Result = new RedirectToRouteResult(new
                System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));

            }

        
      }

    }

 }
