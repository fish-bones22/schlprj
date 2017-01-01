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
    public class ProfitCenterController : Controller
    {
        UserSession user;
        // GET: ProfitCenter
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("PRC", "HOME", "LOG");
            return View(new ProfitCenterViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(ProfitCenterViewModel PRCViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                ProfitCenterManager PRCManager = new ProfitCenterManager();
                result = PRCManager.UpdateProfitCenter(PRCViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                ProfitCenterManager PRCManager = new ProfitCenterManager();
                result = PRCManager.DeleteProfitCenter(PRCViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Breakfast Price Tier", PageAction, PRCViewModel.Id, PRCViewModel.PRFCNT);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}