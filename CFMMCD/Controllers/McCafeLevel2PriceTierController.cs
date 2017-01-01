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
    public class McCafeLevel2PriceTierController : Controller
    {
        UserSession user;
        // GET: McCafeLevel2PriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("ML2", "HOME", "LOG");

            // Get all data stored in DB table
            McCafeLevel2PriceTierManager ML2Manager = new McCafeLevel2PriceTierManager();
            McCafeLevel2PriceTierViewModel ML2ViewModel = new McCafeLevel2PriceTierViewModel();
            ML2ViewModel.M2PTList = ML2Manager.GetML2();
            if (ML2ViewModel.M2PTList == null || ML2ViewModel.M2PTList.Count() == 0)
                ML2ViewModel.M2PTList = new List<McCafeLevel2PriceTierViewModel>();
            // return View with ViewModel
            return View(ML2ViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(McCafeLevel2PriceTierViewModel ML2ViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeLevel2PriceTierManager ML2Manager = new McCafeLevel2PriceTierManager();
                result = ML2Manager.UpdateMcCafeLevel2PriceTier(ML2ViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeLevel2PriceTierManager ML2Manager = new McCafeLevel2PriceTierManager();
                result = ML2Manager.DeleteMcCafeLevel2PriceTier(ML2ViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeLevel2 Price Tier", PageAction, ML2ViewModel.Id, ML2ViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}