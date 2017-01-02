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
    public class ValueMealController : Controller
    {
        // GET: ValueMeal
        public ActionResult Index()
        {
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.VAM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("VAM", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            ValueMealViewModel VMViewModel = (ValueMealViewModel)TempData["SearchItemSelected"];
            ValueMealManager VMManager = new ValueMealManager();
            if (VMViewModel == null)
            {
                VMViewModel = new ValueMealViewModel();
            }
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult Index(ValueMealViewModel VMViewModel)
        {
            ValueMealManager VMManager = new ValueMealManager();
            VMViewModel.ValueMealList = VMManager.SearchValueMeal(VMViewModel);
            if (VMViewModel.ValueMealList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = VMViewModel.ValueMealList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(VMViewModel);
        }

        public ActionResult UpdateDelete(ValueMealViewModel VMViewModel, string command)
        {
            ValueMealManager VMManager = new ValueMealManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == "Save")
            {
                result = VMManager.UpdateValueMeal(VMViewModel, user.Username);
                PageAction = "Update";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Value Meal", PageAction, VMViewModel.VMLNUM, VMViewModel.VMLNAM);
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
            List<ValueMealViewModel> VMList = (List<ValueMealViewModel>)Session["ViewModelList"];
            ValueMealViewModel VMViewModel = VMList.Where(o => o.VMLNUM.ToString().Equals(value)).FirstOrDefault();
            TempData["SearchItemSelected"] = VMViewModel;
            return RedirectToAction("Index", "ValueMeal");
        }
    }
}