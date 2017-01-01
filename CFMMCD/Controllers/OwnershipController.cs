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
    public class OwnershipController : Controller
    {
        UserSession user;
        // GET: Ownership
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("OSP", "HOME", "LOG");
            return View(new OwnershipViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(OwnershipViewModel OSPViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                OwnershipManager OSPManager = new OwnershipManager();
                result = OSPManager.UpdateOwnership(OSPViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                OwnershipManager OSPManager = new OwnershipManager();
                result = OSPManager.DeleteOwnership(OSPViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Breakfast Price Tier", PageAction, OSPViewModel.Id, OSPViewModel.OWNSHP);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}