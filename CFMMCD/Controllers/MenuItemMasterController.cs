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

            // SearchItemSelected is assigned value at DisplaySearchResult
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            MenuItemMasterViewModel MIMViewModel = (MenuItemMasterViewModel)TempData["SearchItemSelected"];
            if (MIMViewModel == null)
            {
                MIMViewModel = new MenuItemMasterViewModel();
            }
            return View(MIMViewModel);
        }
        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMViewModel)
        {   // Search
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            MIMViewModel.MenuItemMasterList = MIMManager.SearchMenuItem(MIMViewModel);
            if (MIMViewModel.MenuItemMasterList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = MIMViewModel.MenuItemMasterList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(MIMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(MenuItemMasterViewModel MIMViewModel, string command)
        {
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (command == "Save")
            {
                result = MIMManager.UpdateMenuItem(MIMViewModel, user.Username);
                PageAction = "Update";
            }
            else if (command == "Delete")
            {
                result = MIMManager.DeleteMenuItem(MIMViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Master", PageAction, MIMViewModel.MIMMIC, MIMViewModel.MIMNAM);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
        /*
         * This method is called after selecting an item from a list of search result.
         * Parameter MIMViewModel still holds the list of searched ViewModels and
         * parameter value stores the value of the item selected.
         * 
         * The value is then searched in the list and stores it in a TempData to be used by Index().
         * */
        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<MenuItemMasterViewModel> MIMList = (List<MenuItemMasterViewModel>)Session["ViewModelList"];
            MenuItemMasterViewModel MIMViewModel = MIMList.Where(o => o.MIMMIC.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = MIMViewModel;
            return RedirectToAction("Index", "MenuItemMaster");
        }
    }
}