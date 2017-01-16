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
    public class RawItemMasterController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.RIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("RIM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            RawItemMasterViewModel RIMViewModel = new RawItemMasterViewModel();
            RIMViewModel.RawItemMasterList = RawItemMasterManager.GetRawItems("ALL");
            return View(RIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(RawItemMasterViewModel RIMViewModel, string value)
        {   // Search
            RIMViewModel = RawItemMasterManager.SearchSingleRawItem(value);
            RIMViewModel.RawItemMasterList = RawItemMasterManager.GetRawItems("ALL");
            return View(RIMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(RawItemMasterViewModel RIMViewModel, string command)
        {
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == null)
            {
                result = RawItemMasterManager.SwitchRawItem(RIMViewModel.RIMRIC, RIMViewModel.SwitchItem, user.Username);
                PageAction = "Switch";
            }
            if (command == "Save")
            {
                result = RawItemMasterManager.UpdateRawItem(RIMViewModel, user.Username);
                PageAction = "Update";
            }
            else if (command == "Delete")
            {
                result = RawItemMasterManager.DeleteRawItem(RIMViewModel);
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
    }
}