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
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MIM", "HOME", "LOG");

            // Initialize page
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
            if (command == "Save")
            {
                //result = MenuItemMasterManager.UpdateMenuItem(MIMViewModel, user.Username);
                PageAction = "Update";
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
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}