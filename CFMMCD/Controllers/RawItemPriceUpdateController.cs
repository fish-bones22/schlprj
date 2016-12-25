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
    public class RawItemPriceUpdateController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.RIP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("RIP", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            RawItemMasterViewModel RIMViewModel = (RawItemMasterViewModel)TempData["SearchItemSelected"];
            if (RIMViewModel == null)
                RIMViewModel = new RawItemMasterViewModel();
            return View(RIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(RawItemMasterViewModel RIMViewModel)
        {   // Search
            RawItemMasterManager RIMManager = new RawItemMasterManager();
            RIMViewModel.RawItemMasterList = RIMManager.SearchRawItem(RIMViewModel);
            if (RIMViewModel.RawItemMasterList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = RIMViewModel.RawItemMasterList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(RIMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(RawItemMasterViewModel RIMViewModel, string command)
        {
            RawItemMasterManager RIMManager = new RawItemMasterManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (command == "Save")
            {
                result = RIMManager.UpdateRawItem( RIMViewModel, user.Username );
                PageAction = "Update price";
            }
            else if (command == "Delete")
            {
                result = RIMManager.DeleteRawItem(RIMViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Master", PageAction, RIMViewModel.RIMRIC, RIMViewModel.RIMRID);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
        /*
         * This method is called after selecting an item from a list of search result.
         * Parameter RIMViewModel still holds the list of searched ViewModels and
         * parameter value stores the value of the item selected.
         * 
         * The value is then searched in the list and stores it in a TempData to be used by Index().
         * */
        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<RawItemMasterViewModel> RIMList = (List<RawItemMasterViewModel>)Session["ViewModelList"];
            RawItemMasterViewModel RIMViewModel = RIMList.Where(o => o.RIMRIC.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = RIMViewModel;
            return RedirectToAction("Index", "RawItemPriceUpdate");
        }
    }
}