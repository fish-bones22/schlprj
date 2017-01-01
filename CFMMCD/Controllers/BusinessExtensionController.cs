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
    public class BusinessExtensionController : Controller
    {
        UserSession user;
        // GET: BusinessExtension
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("BEX", "HOME", "LOG");
            return View(new BusinessExtensionViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(BusinessExtensionViewModel BEXViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                BusinessExtensionManager BEXManager = new BusinessExtensionManager();
                result = BEXManager.UpdateBusinessExtension(BEXViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                BusinessExtensionManager BEXManager = new BusinessExtensionManager();
                result = BEXManager.DeleteBusinessExtension(BEXViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Breakfast Price Tier", PageAction, BEXViewModel.Id, BEXViewModel.LONGNM);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}