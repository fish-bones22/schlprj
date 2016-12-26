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
    public class McCafeBistroPriceTierController : Controller
    {
        UserSession user;
        // GET: McCafeBistroPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("MCB", "HOME", "LOG");
            return View(new McCafeBistroPriceTierViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(McCafeBistroPriceTierViewModel MCBViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeBistroPriceTierManager MCBManager = new McCafeBistroPriceTierManager();
                result = MCBManager.UpdateMcCafeBistroPriceTier(MCBViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeBistroPriceTierManager MCBManager = new McCafeBistroPriceTierManager();
                result = MCBManager.DeleteMcCafeBistroPriceTier(MCBViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeBistro Price Tier", PageAction, MCBViewModel.Id, MCBViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}