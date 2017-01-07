#define DEBUG

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
        // GET: StoreProfile
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.STP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("STP", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            StoreProfileManager SPManager = new StoreProfileManager();
            StoreProfileViewModel SPViewModel = new StoreProfileViewModel();
            SPViewModel.StoreList = SPManager.SearchStores("ALL");
            return View(SPViewModel);
        }
        [HttpPost]
        public ActionResult Index(StoreProfileViewModel SPViewModel, string value)
        {
            StoreProfileManager SPManager = new StoreProfileManager();
            SPViewModel = SPManager.SearchSingleStore(value);
            SPViewModel.StoreList = SPManager.SearchStores("ALL");
            return View(SPViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(StoreProfileViewModel SPViewModel, string command)
        {
            StoreProfileManager SPManager = new StoreProfileManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (command == "Save")
            {
                result = SPManager.UpdateStore(SPViewModel);
                PageAction = "Update";
            }
            else if (command == "Delete")
            {
                result = SPManager.DeleteStore(SPViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Store Profile", PageAction, SPViewModel.STORE_NO, SPViewModel.STORE_NAME);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}