using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CFMMCD.Models.ViewModel.MenuItemMasterViewModel;

namespace CFMMCD.Controllers
{
    public class MenuItemMasterController : Controller
    {
        /*
        * Default method.
        * TempData is used to store the ViewModel after a Search action.
        */
        public ActionResult Index(string id)
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MIM", "HOME", "LOG");

            // Initialize page
            MenuItemMasterViewModel MIMViewModel = new MenuItemMasterViewModel();
            if (id != null)
                MIMViewModel = MenuItemMasterManager.SearchSingleMenuItem(id);
            MIMViewModel.MenuItemMasterList = new List<MenuItem>();
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems("ALL");
            return View(MIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMViewModel, string value)
        {
            MIMViewModel = MenuItemMasterManager.SearchSingleMenuItem(value);
            MIMViewModel.HasSearched = true;
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems("ALL");
            return View(MIMViewModel);
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
        public ActionResult UpdateDelete(MenuItemMasterViewModel MIMViewModel, string command)
        {
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUploaded"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    ReportViewModel report = MenuItemMasterManager.ImportExcel(file.InputStream, user.Username);
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
                result = MenuItemMasterManager.UpdateMenuItem(MIMViewModel, user.Username);
                PageAction = "Update";
                if (!MIMViewModel.HasSearched)
                    PageAction = "Create";
            }
            else if (command == "Delete")
            {
                result = MenuItemMasterManager.DeleteMenuItem(MIMViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Menu Item Master", PageAction, MIMViewModel.MIMMIC, MIMViewModel.MIMNAM);
                if (!PageAction.Equals("Delete"))
                    return RedirectToAction("Index", new { id = MIMViewModel.MIMMIC });
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}