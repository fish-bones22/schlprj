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

            ValueMealManager VMManager = new ValueMealManager();
            ValueMealViewModel VMViewModel = new ValueMealViewModel();
            VMViewModel.ValueMealList = VMManager.SearchValueMeals("ALL");
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult Index(ValueMealViewModel VMViewModel, string value)
        {
            ValueMealManager VMManager = new ValueMealManager();
            VMViewModel = VMManager.SearchSingleValueMeal(value);
            VMViewModel.ValueMealList = VMManager.SearchValueMeals("ALL");
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
    }
}