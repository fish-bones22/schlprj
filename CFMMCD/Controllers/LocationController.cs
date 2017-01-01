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
    public class LocationController : Controller
    {
        UserSession user;
        // GET: Location
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.LCN) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("LCN", "HOME", "LOG");
            return View(new LocationViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(LocationViewModel LCNViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                LocationManager LCNManager = new LocationManager();
                result = LCNManager.UpdateLocation(LCNViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                LocationManager LCNManager = new LocationManager();
                result = LCNManager.DeleteLocation(LCNViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Breakfast Price Tier", PageAction, LCNViewModel.Id, LCNViewModel.LOCATN);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}