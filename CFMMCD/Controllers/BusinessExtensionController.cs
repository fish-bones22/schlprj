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
    public class BusinessExtensionController : Controller
    {
        UserSession user;
        // GET: BusinessExtension
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
                                // BUE -> Business Extension
                                // Refer to UserAccessSession 
            if (UASession == null || !UASession.BUE) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("BEX", "HOME", "LOG");

            // Get all data stored in DB table
            BusinessExtensionManager BEXManager = new BusinessExtensionManager();
            BusinessExtensionViewModel BEXViewModel = new BusinessExtensionViewModel();
            BEXViewModel.BusinessExtList = BEXManager.GetBEX();
            if (BEXViewModel.BusinessExtList == null || BEXViewModel.BusinessExtList.Count() == 0)
                BEXViewModel.BusinessExtList = new List<BusinessExtensionViewModel>();
            // return View with ViewModel
            return View(BEXViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(BusinessExtensionViewModel BEXViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                BusinessExtensionManager BEXManager = new BusinessExtensionManager();
                result = BEXManager.UpdateBusinessExtension(BEXViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                BusinessExtensionManager BEXManager = new BusinessExtensionManager();
                result = BEXManager.DeleteBusinessExtension(BEXViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Business Extension", PageAction, BEXViewModel.ID, BEXViewModel.LONGNM);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}