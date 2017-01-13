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
    public class BreakfastPriceTierController : Controller
    {
        UserSession user;
        // GET: BreakfastPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
                                // TIP -> Price Tier
                                // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_BRE", "HOME", "LOG");

            // Get all data stored in DB table
            BreakfastPriceTierManager BPTManager = new BreakfastPriceTierManager();
            BreakfastPriceTierViewModel BPTViewModel = new BreakfastPriceTierViewModel();
            BPTViewModel.BreakPTList = BPTManager.GetBPT();
            if (BPTViewModel.BreakPTList == null || BPTViewModel.BreakPTList.Count() == 0)
                BPTViewModel.BreakPTList = new List<BreakfastPriceTierViewModel>();
            // return View with ViewModel
            return View(BPTViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(BreakfastPriceTierViewModel BPTViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                BreakfastPriceTierManager BPTManager = new BreakfastPriceTierManager();
                result = BPTManager.UpdateBreakfastPriceTier(BPTViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                BreakfastPriceTierManager BPTManager = new BreakfastPriceTierManager();
                result = BPTManager.DeleteBreakfastPriceTier(BPTViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Breakfast Price Tier", PageAction, BPTViewModel.Id, BPTViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}