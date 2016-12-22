using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("HOME", "LOG");
            return View();
        }
    }
}