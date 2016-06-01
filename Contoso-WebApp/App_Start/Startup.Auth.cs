using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System.IdentityModel.Claims;
using System.Threading.Tasks;

namespace test
{
    public partial class Startup
    {
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string metadata = ConfigurationManager.AppSettings["ida:AzureADMetadata"];
        private static readonly bool enableCustomClaimsInjection = false;

             public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = realm,
                    MetadataAddress = metadata

                    #region Claims Injection 
                    //Example of adding custom claims inside application
                    ,Notifications = new WsFederationAuthenticationNotifications()
                     {
                        SecurityTokenReceived = notification =>
                        {
                            var protocalMessage = notification.ProtocolMessage;
                            return Task.FromResult(0);
                        },

                         SecurityTokenValidated = notification =>
                         {
                             if (enableCustomClaimsInjection)
                             {

                                 notification.AuthenticationTicket.Identity.AddClaim(
                                     new System.Security.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/SSN", "123-456-789"));
                                 
                             }
                             return Task.FromResult(0);
                         } 
                     }
              
                    #endregion
                });
        }
    }
}