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
    public class StoreProfileController : Controller
    {
        // GET: StoreProfile
        public ActionResult StoreProfile()
        {
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "StoreProfile",
                Controller = "StoreProfile"
            };
            CPSession.Parent = new CurrentPageSession.LinkString
            {
                Action = "Index",
                Controller = "Home"
            };
            CPSession.Grandparent = new CurrentPageSession.LinkString
            {
                Action = "Login",
                Controller = "Account"
            };
            Session["CurrentPage"] = CPSession;
            return View(new StoreProfileViewModel());
        }

        [HttpPost]
        public ActionResult StoreProfile(StoreProfileViewModel StoreProf)
        {
            // Search
            StoreProfileManager SPManager = new StoreProfileManager();
            StoreProfileViewModel model = new StoreProfileViewModel();
            model = SPManager.SearchStoreProfile(StoreProf);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveUpdateDelete(StoreProfileViewModel SPViewModel, string command)
        {
            if (command == "Save")
            {
                //Code for Add
                Session["pageact"] = "Add"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.CreateStoreProfile(SPViewModel);
                
                //** Audit Log **
                // Prepare data to be passed to adit log manager
                UserSession us = (UserSession)Session["User"];
                AuditLogViewModel ALViewModel = new AuditLogViewModel
                {
                    Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                    Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")),
                    Name = us.Username,
                    Page = "STORPROF",
                    Page_Action = "Create",
                    UserId = us.UserID
                };
                // Add to audit log
                new AuditLogManager().Audit(ALViewModel);
            }
            else if (command == "Update")
            {
                //Code for Update
                Session["pageact"] = "Update"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.UpdateStoreProfile(SPViewModel);

                //** Audit Log ** /       /*TODO: TO BE TRANSFERRED ON ITS OWN OBJECT (or methhod)
                // Prepare data to be passed to adit log manager
                UserSession us = (UserSession)Session["User"];
                AuditLogViewModel ALViewModel = new AuditLogViewModel
                {
                    Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                    Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")),
                    Name = us.Username,
                    Page = "STORPROF",
                    Page_Action = "Update",
                    UserId = us.UserID
                };
                // Add to audit log
                new AuditLogManager().Audit(ALViewModel);
            }
            else if (command == "Delete")
            {
                //Code for Delete
                Session["pageact"] = "Delete"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.DeleteStoreProfile(SPViewModel);

                //** Audit Log **
                // Prepare data to be passed to adit log manager
                UserSession us = (UserSession)Session["User"];
                AuditLogViewModel ALViewModel = new AuditLogViewModel
                {
                    Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                    Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")),
                    Name = us.Username,
                    Page = "STORPROF",
                    Page_Action = "Delete",
                    UserId = us.UserID
                };
                // Add to audit log
                new AuditLogManager().Audit(ALViewModel);
            }
            return RedirectToAction("StoreProfile");
        }
    }
}