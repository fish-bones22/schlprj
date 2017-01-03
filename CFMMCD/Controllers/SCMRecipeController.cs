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
        public ActionResult Index()
        {
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.SCM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("SCM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            SCMRecipeViewModel SCMViewModel = (SCMRecipeViewModel)TempData["SearchItemSelected"];
            SCMRecipeManager SCMManager = new SCMRecipeManager();
            if (SCMViewModel == null)
            {
                SCMViewModel = new SCMRecipeViewModel();
            }
            return View(SCMViewModel);
        }
        [HttpPost]
        public ActionResult Index(SCMRecipeViewModel SCMViewModel)
        {
            SCMRecipeManager SCMManager = new SCMRecipeManager();
            SCMViewModel.SCMRecipeList = SCMManager.SearchSCMRecipe(SCMViewModel);
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
                result = SCMManager.UpdateSCMRecipe(SCMViewModel, user.Username);
                PageAction = "Update";
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

        [HttpPost]
        public ActionResult DisplaySearchResult(string value)
        {
            List<SCMRecipeViewModel> VMList = (List<SCMRecipeViewModel>)Session["ViewModelList"];
            SCMRecipeViewModel SCMViewModel = VMList.Where(o => o.CSMID.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = SCMViewModel;
            return RedirectToAction("Index", "SCMRecipe");
        }
    }
}