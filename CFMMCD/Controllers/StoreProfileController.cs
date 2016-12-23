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
    public class StoreProfileController : Controller
    {
        UserSession user;
        // GET: StoreProfile
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.STP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("STP", "HOME", "LOG");
            return View(new StoreProfileViewModel());
        }
        [HttpPost]
        public ActionResult Index(StoreProfileViewModel StoreProf)
        {
            // Search
            StoreProfileManager SPManager = new StoreProfileManager();
            StoreProfileViewModel model = new StoreProfileViewModel();
            model = SPManager.SearchStoreProfile(StoreProf);
            if (model == null)
                ModelState.AddModelError("", "No items found");
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateDelete(StoreProfileViewModel SPViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                StoreProfileManager SPManager = new StoreProfileManager();
                result = SPManager.UpdateStoreProfile(SPViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                StoreProfileManager SPManager = new StoreProfileManager();
                result = SPManager.DeleteStoreProfile(SPViewModel);
                PageAction = "DELETE";
            }

            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Master", PageAction, SPViewModel.STORE_NO, SPViewModel.STORE_NAME);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}