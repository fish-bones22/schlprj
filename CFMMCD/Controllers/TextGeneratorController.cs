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
            TextGeneratorViewModel TGViewModel = new TextGeneratorViewModel();
            TGViewModel.IncludeAll = true;
            return View(TGViewModel);
        }
        [HttpPost]
        public ActionResult Index(TextGeneratorViewModel TGViewModel)
        {
            TextGeneratorManager TGManager = new TextGeneratorManager();
            bool result = TGManager.GeneratePackets(TGViewModel);
            string PageAction = "Text Generation";
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return View(new TextGeneratorViewModel());
        }

    }
}