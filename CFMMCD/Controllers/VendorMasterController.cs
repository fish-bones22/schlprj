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
    public class VendorMasterController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.VEM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MIM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            VendorMasterViewModel VMViewModel = (VendorMasterViewModel)TempData["SearchItemSelected"];
            if (VMViewModel == null)
                VMViewModel = new VendorMasterViewModel();
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult Index(VendorMasterViewModel VMViewModel)
        {   // Search
            VendorMasterManager VMManager = new VendorMasterManager();
            VMViewModel.VendorMasterList = VMManager.SearchVendor(VMViewModel);
            if (VMViewModel.VendorMasterList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = VMViewModel.VendorMasterList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(VendorMasterViewModel VMViewModel, string command)
        {
            VendorMasterManager VMManager = new VendorMasterManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (command == "Save")
            {
                result = VMManager.UpdateVendor(VMViewModel, user.Username);
                PageAction = "Update";
            }
            else if (command == "Delete")
            {
                result = VMManager.DeleteVendor(VMViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Vendor Master", PageAction, VMViewModel.VEMVEN, VMViewModel.VEMDS1);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
        /*
         * This method is called after selecting an item from a list of search result.
         * Parameter VMViewModel still holds the list of searched ViewModels and
         * parameter value stores the value of the item selected.
         * 
         * The value is then searched in the list and stores it in a TempData to be used by Index().
         * */
        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<VendorMasterViewModel> VMList = (List<VendorMasterViewModel>)Session["ViewModelList"];
            VendorMasterViewModel VMViewModel = VMList.Where(o => o.VEMVEN.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = VMViewModel;
            return RedirectToAction("Index", "VendorMaster");
        }
    }
}