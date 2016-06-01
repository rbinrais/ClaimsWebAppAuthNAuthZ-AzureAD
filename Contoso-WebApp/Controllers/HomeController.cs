using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Contoso_ExternalAuthModule;

namespace test.Controllers
{
    //[Authorize(Roles = "Reader,Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {

            ViewBag.data = ClaimsPrincipal.Current.Claims;

            return View();
        }

        //[Authorize(Roles = "Admin")]
        [Contoso_ExternalAuthModule.ExternalAuthModuleAuthorize(Role ="Reader", Provider ="live.com")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact for confidential orders!";

            return View();
        }

        public ActionResult AccessDenied()
        {
            
            return View();
        }

    }
}