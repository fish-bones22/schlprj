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
        public ActionResult Index(string id)
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.STP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("STP", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            StoreProfileManager SPManager = new StoreProfileManager();
            StoreProfileViewModel SPViewModel = new StoreProfileViewModel();
            if (id != null)
                SPViewModel = SPManager.SearchSingleStore(id);
            SPViewModel.StoreList = SPManager.SearchStores("ALL");
            return View(SPViewModel);
        }
        [HttpPost]
        public ActionResult Index(StoreProfileViewModel SPViewModel, string value)
        {
            StoreProfileManager SPManager = new StoreProfileManager();
            SPViewModel = SPManager.SearchSingleStore(value);
            SPViewModel.HasSearched = true;
            SPViewModel.StoreList = SPManager.SearchStores("ALL");
            return View(SPViewModel);
        }

        /*
         * UpdateDelete Method is generally composed of three possible actions: Import, Save/Update, and Delete.
         * Import action base solely on the availability of Request File. If Request File Count is zero, then the 
         * action is skipped.
         * The Save/Update and Delete actions rely on the value of the parameter 'command' sent by the View
         * These actions are logged if successully performed with the exception for Import action since it Updates/Creates data
         * one batch at a time thus logging is handled in the Entity Manager.
         */
        [HttpPost]
        public ActionResult UpdateDelete(StoreProfileViewModel SPViewModel, string command)
        {
            StoreProfileManager SPManager = new StoreProfileManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUploaded"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    ReportViewModel report = StoreProfileManager.ImportExcel(file.InputStream, user.Username);
                    PageAction = "Import";
                    result = report.Result;
                    if (!result)
                    {
                        if (report.ErrorLevel == 2)
                            PageAction = report.Message + ": Import partially";
                        else
                            PageAction = report.Message + ": Import";
                    }
                    else
                        PageAction = report.Message + ": Import";
                }
            }
            if (command == "Save")
            {
                result = SPManager.UpdateStore(SPViewModel);
                PageAction = "Create";
                if (SPViewModel.HasSearched)
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
                if (!PageAction.Equals("Delete"))
                    return RedirectToAction("Index", new { id = SPViewModel.STORE_NO });
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}