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
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("MCL2", "HOME", "LOG");
            return View(new McCafeLevel2PriceTierViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(McCafeLevel2PriceTierViewModel MCL2ViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeLevel2PriceTierManager MCL2Manager = new McCafeLevel2PriceTierManager();
                result = MCL2Manager.UpdateMcCafeLevel2PriceTier(MCL2ViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeLevel2PriceTierManager MCL2Manager = new McCafeLevel2PriceTierManager();
                result = MCL2Manager.DeleteMcCafeLevel2PriceTier(MCL2ViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeLevel2 Price Tier", PageAction, MCL2ViewModel.Id, MCL2ViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}