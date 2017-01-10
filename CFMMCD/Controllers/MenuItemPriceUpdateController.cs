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
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            MenuItemMasterViewModel MIMViewModel = new MenuItemMasterViewModel();
            MIMViewModel.MenuItemMasterList = MIMManager.SearchMenuItems("ALL");
            for (int i = 0; i < MIMViewModel.MenuItemMasterList.Count(); i++)
                MIMViewModel.MenuItemPriceList.Add(MIMManager.SearchSingleMenuItem(MIMViewModel.MenuItemMasterList[i].RIRMIC));
            return View(MIMViewModel);
        }

        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMViewModel)
        {   // Search
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            MIMViewModel.MenuItemMasterList = MIMManager.SearchMenuItems(MIMViewModel.SearchItem);
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
                result = MIMManager.UpdatePriceTier(MIMViewModel.MenuItemPriceList);
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