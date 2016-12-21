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
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "Index",
                Controller = "TextGenerator"
            };
            CPSession.Parent = new CurrentPageSession.LinkString
            {
                Action = "Index",
                Controller = "Home"
            };
            CPSession.Grandparent = new CurrentPageSession.LinkString
            {
                Action = "Login",
                Controller = "Account"
            };
            Session["CurrentPage"] = CPSession;
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