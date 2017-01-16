using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFMMCD.Models.ViewModel;
using CFMMCD.Models.EntityManager;
using CFMMCD.Sessions;

namespace CFMMCD.Controllers
{
    public class MenuRecipeController : Controller
    {
        // GET: MenuRecipe
        public ActionResult Index()
        {
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MER) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MER", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            MenuRecipeViewModel MRViewModel = new MenuRecipeViewModel();
            MRViewModel.MenuItemList = MenuRecipeManager.SearchMenuItem("ALL");
            MRViewModel.RawItemList = RawItemMasterManager.GetRawItems("ALL");
            return View(MRViewModel);
        }
        [HttpPost]
        public ActionResult Index(MenuRecipeViewModel MRViewModel, string value)
        {
            MRViewModel = MenuRecipeManager.SearchMenuRecipe(value);
            MRViewModel.MenuItemList = MenuRecipeManager.SearchMenuItem("ALL");
            MRViewModel.RawItemList = RawItemMasterManager.GetRawItems("ALL");
            return View(MRViewModel);
        }

        public ActionResult UpdateDelete(MenuRecipeViewModel MRViewModel, string command)
        {
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == "Save")
            {
                result = MenuRecipeManager.UpdateMenuItem(MRViewModel, user.Username);
                PageAction = "Update";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Menu Recipe Master", PageAction, MRViewModel.RIRMIC, MRViewModel.MIMLON);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}