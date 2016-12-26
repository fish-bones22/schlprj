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
            RawItemPriceUpdateViewModel RIPViewModel = (RawItemPriceUpdateViewModel)TempData["SearchItemSelected"];
            if (RIPViewModel == null)
                RIPViewModel = new RawItemPriceUpdateViewModel();
            return View(RIPViewModel);
        }
        [HttpPost]
        public ActionResult Index(RawItemPriceUpdateViewModel RIPViewModel)
        {   // Search
            RawItemPriceManager RIMManager = new RawItemPriceManager();
            RIPViewModel.RawItemPriceMasterList = RIMManager.SearchRawItemPrice(RIPViewModel);
            if (RIPViewModel.RawItemPriceMasterList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = RIPViewModel.RawItemPriceMasterList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(RIPViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(RawItemPriceUpdateViewModel RIPViewModel, string command)
        {
            RawItemPriceManager RIPManager = new RawItemPriceManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (command == "Save")
            {
                result = RIPManager.UpdateRawItemPrice( RIPViewModel );
                PageAction = "Update price";
            }
            else if (command == "Delete")
            {
                result = RIPManager.DeleteRawItemPrice(RIPViewModel);
                PageAction = "Delete price";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Price Update", PageAction, RIPViewModel.RIMRIC, RIPViewModel.RIMRID);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
        /*
         * This method is called after selecting an item from a list of search result.
         * Parameter RIPViewModel still holds the list of searched ViewModels and
         * parameter value stores the value of the item selected.
         * 
         * The value is then searched in the list and stores it in a TempData to be used by Index().
         * */
        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<RawItemPriceUpdateViewModel> RIPList = (List<RawItemPriceUpdateViewModel>)Session["ViewModelList"];
            RawItemPriceUpdateViewModel RIPViewModel = RIPList.Where(o => o.RIM_VEM_ID.Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = RIPViewModel;
            return RedirectToAction("Index", "RawItemPriceUpdate");
        }
    }
}