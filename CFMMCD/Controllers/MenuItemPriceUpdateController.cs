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
    public class MenuItemPriceUpdateController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MIP", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            MenuItemMasterViewModel MIMViewModel = new MenuItemMasterViewModel();
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems("ALL");
            return View(MIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMViewModel, string value)
        {
            MIMViewModel = MenuItemMasterManager.SearchSingleMenuItem(value);
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems("ALL");
            return View(MIMViewModel);
        }
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
                    //string fileName = file.FileName;
                    // store the file inside ~/App_Data/uploads folder
                    //string path = "~/App_Data/uploads/" + fileName;
                    //file.SaveAs(Server.MapPath(path));
                    ReportViewModel report = MenuItemMasterManager.ImportExcelUpdate(file.InputStream, user.Username);
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
                result = MenuItemMasterManager.UpdatePriceTier(MIMViewModel, user.Username);
                PageAction = "Update price";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Menu Item Price Update", PageAction, MIMViewModel.MIMMIC, MIMViewModel.MIMNAM);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}