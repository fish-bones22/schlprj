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
    public class MDSPriceTierController : Controller
    {
        UserSession user;
        // GET: MDSPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("MDS", "HOME", "LOG");
            return View(new MDSPriceTierViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(MDSPriceTierViewModel MDSViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                MDSPriceTierManager MDSManager = new MDSPriceTierManager();
                result = MDSManager.UpdateMDSPriceTier(MDSViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                MDSPriceTierManager MDSManager = new MDSPriceTierManager();
                result = MDSManager.DeleteMDSPriceTier(MDSViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "MDS Price Tier", PageAction, MDSViewModel.Id, MDSViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}