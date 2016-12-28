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
            StoreProfileViewModel SPViewModel = (StoreProfileViewModel)TempData["SearchItemSelected"];
            if (SPViewModel == null)
            {
                SPViewModel = new StoreProfileViewModel();
                SPViewModel.BusinessExtList = new List<CheckBoxList>();
                SPViewModel = SPManager.InitializeDropDowns(SPViewModel);
                SPViewModel.BusinessExtList = SPManager.SetBusinessExtension();
            }
            System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].Cb);
            System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].value);
            System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].text);
            return View(SPViewModel);
        }
        [HttpPost]
        public ActionResult Index(StoreProfileViewModel SPViewModel)
        {
            StoreProfileManager SPManager = new StoreProfileManager();
            SPViewModel.StoreList = SPManager.SearchStore(SPViewModel);
            if (SPViewModel.StoreList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = SPViewModel.StoreList;
            }
            else
                ModelState.AddModelError("", "No results found");
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
                System.Diagnostics.Debug.WriteLine(SPViewModel.STORE_NAME);
                System.Diagnostics.Debug.WriteLine(SPViewModel.STORE_NO);
                System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].Cb);
                System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].value);
                System.Diagnostics.Debug.WriteLine(SPViewModel.BusinessExtList[0].text);
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
        /*
         * This method is called after selecting an item from a list of search result.
         * Parameter SPViewModel still holds the list of searched ViewModels and
         * parameter value stores the value of the item selected.
         * 
         * The value is then searched in the list and stores it in a TempData to be used by Index().
         * */
        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<StoreProfileViewModel> SPList = (List<StoreProfileViewModel>)Session["ViewModelList"];
            StoreProfileViewModel SPViewModel = SPList.Where(o => o.STORE_NO.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = SPViewModel;
            return RedirectToAction("Index", "StoreProfile");
        }
    }
}