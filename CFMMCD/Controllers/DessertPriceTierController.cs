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
    public class DessertPriceTierController : Controller
    {
        UserSession user;
        // GET: DessertPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_DES", "HOME", "LOG");

            // Get all data stored in DB table
            DessertPriceTierManager DPTManager = new DessertPriceTierManager();
            DessertPriceTierViewModel DPTViewModel = new DessertPriceTierViewModel();
            DPTViewModel.DesPTList = DPTManager.GetDPT();
            if (DPTViewModel.DesPTList == null || DPTViewModel.DesPTList.Count() == 0)
                DPTViewModel.DesPTList = new List<DessertPriceTierViewModel>();
            // return View with ViewModel
            return View(DPTViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(DessertPriceTierViewModel DPTViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                DessertPriceTierManager DPTManager = new DessertPriceTierManager();
                result = DPTManager.UpdateDessertPriceTier(DPTViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                DessertPriceTierManager DPTManager = new DessertPriceTierManager();
                result = DPTManager.DeleteDessertPriceTier(DPTViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Dessert Price Tier", PageAction, DPTViewModel.Id, DPTViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}