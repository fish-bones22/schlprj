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
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_LE3", "HOME", "LOG");

            // Get all data stored in DB table
            McCafeLevel3PriceTierManager ML3Manager = new McCafeLevel3PriceTierManager();
            McCafeLevel3PriceTierViewModel ML3ViewModel = new McCafeLevel3PriceTierViewModel();
            ML3ViewModel.M3PTList = ML3Manager.GetML3();
            if (ML3ViewModel.M3PTList == null || ML3ViewModel.M3PTList.Count() == 0)
                ML3ViewModel.M3PTList = new List<McCafeLevel3PriceTierViewModel>();
            // return View with ViewModel
            return View(ML3ViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(McCafeLevel3PriceTierViewModel ML3ViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeLevel3PriceTierManager ML3Manager = new McCafeLevel3PriceTierManager();
                result = ML3Manager.UpdateMcCafeLevel3PriceTier(ML3ViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeLevel3PriceTierManager ML3Manager = new McCafeLevel3PriceTierManager();
                result = ML3Manager.DeleteMcCafeLevel3PriceTier(ML3ViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeLevel3 Price Tier", PageAction, ML3ViewModel.Id, ML3ViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}