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
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_MDS", "HOME", "LOG");

            // Get all data stored in DB table
            MDSPriceTierManager MDSManager = new MDSPriceTierManager();
            MDSPriceTierViewModel MDSViewModel = new MDSPriceTierViewModel();
            MDSViewModel.MDPTList = MDSManager.GetMDS();
            if (MDSViewModel.MDPTList == null || MDSViewModel.MDPTList.Count() == 0)
                MDSViewModel.MDPTList = new List<MDSPriceTierViewModel>();
            // return View with ViewModel
            return View(MDSViewModel);
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