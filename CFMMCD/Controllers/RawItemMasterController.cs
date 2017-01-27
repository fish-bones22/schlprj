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
        public ActionResult Index(string id)
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.RIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("RIM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            RawItemMasterViewModel RIMViewModel = new RawItemMasterViewModel();
            if (id != null)
                RIMViewModel = RawItemMasterManager.SearchSingleRawItem(id);
            RIMViewModel.RawItemMasterList = RawItemMasterManager.GetRawItems("ALL");
            return View(RIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(RawItemMasterViewModel RIMViewModel, string value)
        {   // Search
            RIMViewModel = RawItemMasterManager.SearchSingleRawItem(value);
            RIMViewModel.HasSearched = true;
            RIMViewModel.RawItemMasterList = RawItemMasterManager.GetRawItems("ALL");
            return View(RIMViewModel);
        }
        /*
         * UpdateDelete Method is generally composed of three possible actions: Import, Save/Update, and Delete.
         * Import action base solely on the availability of Request File. If Request File Count is zero, then the 
         * action is skipped.
         * The Save/Update and Delete actions rely on the value of the parameter 'command' sent by the View
         * These actions are logged if successully performed with the exception for Import action since it Updates/Creates data
         * one batch at a time thus logging is handled in the Entity Manager.
         * 
         * Misc actions available in RawItemController: Switch
         * Switch action is performed if the ViewModel for 'SwitchRawItem' is not null nor empty.
         * 
         */
        [HttpPost]
        public ActionResult UpdateDelete(RawItemMasterViewModel RIMViewModel, string command)
        {
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUploaded"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    ReportViewModel report = RawItemMasterManager.ImportExcel(file.InputStream, user.Username);
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
            if (command == null && !(RIMViewModel.SwitchItem == null))
            {
                result = RawItemMasterManager.SwitchRawItem(RIMViewModel.RIMRIC, RIMViewModel.SwitchItem, user.Username);
                PageAction = "Switch";
            }
            if (command == "Save")
            {
                result = RawItemMasterManager.UpdateRawItem(RIMViewModel, user.Username);
                PageAction = "Update";
                if (!RIMViewModel.HasSearched)
                    PageAction = "Create";
            }
            else if (command == "Delete")
            {
                result = RawItemMasterManager.DeleteRawItem(RIMViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                if (PageAction.Equals("Switch") || PageAction.Equals("Update") || PageAction.Equals("Create"))
                    new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Master", PageAction, RIMViewModel.RIMRIC, RIMViewModel.RIMRID);
                if (RIMViewModel.RIMRIC != null && !RIMViewModel.RIMRIC.Equals("") && !PageAction.Equals("Delete"))
                    return RedirectToAction("Index", new { id = RIMViewModel.RIMRIC });
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}