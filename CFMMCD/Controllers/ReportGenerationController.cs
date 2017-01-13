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
    public class ReportGenerationController : Controller
    {
        // GET: ReportGeneration
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.REG) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("REG", "HOME", "LOG");
            
            return View(new ReportGenerationViewModel());
        }

        [HttpPost]
        public ActionResult Export(ReportGenerationViewModel RGViewModel, string command)
        {
            bool result = false;
            ReportGenerationManager RGManager = new ReportGenerationManager();
            result = RGManager.ManageReport(RGViewModel, command);
            return RedirectToAction("Index");
        }
    }
}