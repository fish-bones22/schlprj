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
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_BIS", "HOME", "LOG");

            // Get all data stored in DB table
            McCafeBistroPriceTierManager MBTManager = new McCafeBistroPriceTierManager();
            McCafeBistroPriceTierViewModel MBTViewModel = new McCafeBistroPriceTierViewModel();
            MBTViewModel.MBPTList = MBTManager.GetMBT();
            if (MBTViewModel.MBPTList == null || MBTViewModel.MBPTList.Count() == 0)
                MBTViewModel.MBPTList = new List<McCafeBistroPriceTierViewModel>();
            // return View with ViewModel
            return View(MBTViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(McCafeBistroPriceTierViewModel MBTViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                McCafeBistroPriceTierManager MBTManager = new McCafeBistroPriceTierManager();
                result = MBTManager.UpdateMcCafeBistroPriceTier(MBTViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                McCafeBistroPriceTierManager MBTManager = new McCafeBistroPriceTierManager();
                result = MBTManager.DeleteMcCafeBistroPriceTier(MBTViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "McCafeBistro Price Tier", PageAction, MBTViewModel.Id, MBTViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}