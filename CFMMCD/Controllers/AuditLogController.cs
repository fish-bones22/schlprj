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
    public class AuditLogController : Controller
    {
        // GET: AuditLog
        public ActionResult Index()
        {
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "Index",
                Controller = "AuditLog"
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
            AuditLogViewModel ULViewModel = new AuditLogViewModel();
            AuditLogManager ALManager = new AuditLogManager();
            List<AuditLogViewModel> ALList = ALManager.GetAuditLog();
            ViewData["ModelList"] = ALList;
            return View();
        }
    }
}