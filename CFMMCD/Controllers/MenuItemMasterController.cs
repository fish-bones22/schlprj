using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CFMMCD.Models.ViewModel.MenuItemMasterViewModel;

namespace CFMMCD.Controllers
{
    public class MenuItemMasterController : Controller
    {
        public ActionResult MenuItemMaster()
        {
            
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "MenuItemMaster",
                Controller = "MenuItemMaster"
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
            return View(new MenuItemMasterViewModel());
        }

        [HttpPost]
        public ActionResult MenuItemMaster(MenuItemMasterViewModel MIMiewModel)
        {
            MenuItemMasterManager MIMManager = new MenuItemMasterManager();
            MenuItemMasterViewModel ReturnedModel = MIMManager.SearchMIM(MIMiewModel);
            if (ReturnedModel.MIMMIC == null)
                ModelState.AddModelError("", "No results found");
            return View(ReturnedModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(MenuItemMasterViewModel MIMViewModel, string command)
        {
            if (command.Equals("Save"))
            {
                if (ModelState.IsValid)
                {
                    Session["pageact"] = "Update"; // Session for what a user did
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null) // Guard in case unlogged access happened
                    {
                        // Used in CSHMIMP0 table, MIMUSR row 
                        UserSession user = (UserSession)Session["User"];
                        bool isSuccessful = MIMManager.SaveMIM(MIMViewModel, user.Username);
                        // Used for alert messages in view 
                        if (isSuccessful)
                        {
                            TempData["SuccessMessage"] = "Saved";
                            // Prepare data to be passed to adit log manager
                            UserSession us = (UserSession)Session["User"];
                            AuditLogViewModel ALViewModel = new AuditLogViewModel
                            {
                                Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                                Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")),
                                Name = us.Username,
                                Page = "MIM",
                                Page_Action = "Update",
                                UserId = us.UserID
                            };
                            // Add to audit log
                            new AuditLogManager().Audit(ALViewModel);
                        }
                        else TempData["ErrorMessage"] = "Saving failed";
                    }
                    else
                        ModelState.AddModelError("", "Not logged in");
                }
                else
                    ModelState.AddModelError("", "Fill up all fields");
            }
            else if (command.Equals("Delete"))
            {
                if (ModelState.IsValid)
                {
                    Session["pageact"] = "Delete"; // Session for what a user did
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null) // Guard in case unlogged access happened
                    {
                        bool isSuccessful = MIMManager.DeleteMIM(MIMViewModel);
                        // Used for alert messages in view 
                        if (isSuccessful) TempData["SuccessMessage"] = "Successfully deleted";
                        else TempData["ErrorMessage"] = "Failed to delete";
                    }
                    else
                        ModelState.AddModelError("", "Not logged in");
                }
            }
            return RedirectToAction("MenuItemMaster");
        }
    }
}