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
    public class McCafeLevel3PriceTierController : Controller
    {
        UserSession user;
        // GET: McCafeLevel3PriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.STP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("MCL3", "HOME", "LOG");
            return View(new McCafeLevel3PriceTierViewModel());
        }
        public ActionResult UpdateDelete(McCafeLevel3PriceTierViewModel MCL3ViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeLevel3PriceTierManager MCL3Manager = new McCafeLevel3PriceTierManager();
                result = MCL3Manager.UpdateMcCafeLevel3PriceTier(MCL3ViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeLevel3PriceTierManager MCL3Manager = new McCafeLevel3PriceTierManager();
                result = MCL3Manager.DeleteMcCafeLevel3PriceTier(MCL3ViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeLevel3 Price Tier", PageAction, MCL3ViewModel.Id, MCL3ViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}