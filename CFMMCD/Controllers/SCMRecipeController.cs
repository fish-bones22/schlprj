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
    public class SCMRecipeController : Controller
    {
        // GET: ValueMeal
        public ActionResult Index(string id)
        {
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.SCM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("SCM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            SCMRecipeViewModel SCMViewModel = new SCMRecipeViewModel();
            if (id != null)
                SCMViewModel = SCMRecipeManager.SearchSCMRecipe(id);
            return View(SCMViewModel);
        }
        [HttpPost]
        public ActionResult Index(SCMRecipeViewModel SCMViewModel, string command)
        {
            SCMViewModel = SCMRecipeManager.SearchSCMRecipe(SCMViewModel.SearchItem);
            if (SCMViewModel.SCMRecipeList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = SCMViewModel.SCMRecipeList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(SCMViewModel);
        }

        public ActionResult UpdateDelete(SCMRecipeViewModel SCMViewModel, string command)
        {
            SCMRecipeManager SCMManager = new SCMRecipeManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == "Save")
            {
                result = SCMRecipeManager.UpdateSCMRecipe(SCMViewModel, user.Username);
                PageAction = "Update";
            }
            else
            {
                result = SCMRecipeManager.UpdateSCMRecipe(SCMViewModel, user.Username);
                return RedirectToAction("Index", new { id = SCMViewModel.CSMDES });
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "SCM Recipe", PageAction, "N/A", SCMViewModel.CSMDES );
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}