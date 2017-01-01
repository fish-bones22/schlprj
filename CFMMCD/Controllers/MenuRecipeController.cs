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
            MenuRecipeViewModel MRViewModel = (MenuRecipeViewModel)TempData["SearchItemSelected"];
            MenuRecipeManager MRManager = new MenuRecipeManager();
            if (MRViewModel == null)
            {
                MRViewModel = new MenuRecipeViewModel();
            }
            return View(MRViewModel);
        }
        [HttpPost]
        public ActionResult Index(MenuRecipeViewModel MRViewModel)
        {
            MenuRecipeManager MRManager = new MenuRecipeManager();
            MRViewModel.MenuItemList = MRManager.SearchMenuItem(MRViewModel);
            if (MRViewModel.MenuItemList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = MRViewModel.MenuItemList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(MRViewModel);
        }

        public ActionResult UpdateDelete(MenuRecipeViewModel MRViewModel, string command)
        {
            MenuRecipeManager MRManager = new MenuRecipeManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == "Save")
            {
                result = MRManager.UpdateMenuItem(MRViewModel, user.Username);
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

        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            MenuRecipeViewModel MRViewModel = new MenuRecipeManager().SearchMenuRecipe(value);
            TempData["SearchItemSelected"] = MRViewModel;
            return RedirectToAction("Index", "MenuRecipe");
        }
    }
}