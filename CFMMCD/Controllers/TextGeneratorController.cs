using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class TextGeneratorController : Controller
    {
        // GET: TextGenerator
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TEG) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("TEG", "HOME", "LOG");
            return View();
        }
        [HttpPost]
        public ActionResult Index(TextGeneratorViewModel TGViewModel)
        {
            TextGeneratorManager TGManager = new TextGeneratorManager();
            TGManager.GeneratePackets(TGViewModel);
            return View();
        }

    }
}