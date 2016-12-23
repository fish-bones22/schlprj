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
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MIM) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("MIM", "HOME", "LOG");
            return View(new MenuItemMasterViewModel());
        }

        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMiewModel)
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
            UserSession user = new UserSession();
            string PageAction = "";
            bool result = false;
            if (command.Equals("Save"))
            {
                PageAction = "UPDATE";
                if (ModelState.IsValid)
                {
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null) // Guard in case unlogged access happened
                    {
                        user = (UserSession)Session["User"];
                        result = MIMManager.SaveMIM(MIMViewModel, user.Username);
                        
                    }
                    else
                        ModelState.AddModelError("", "Not logged in");
                }
                else
                    ModelState.AddModelError("", "Fill up all fields");
            }
            else if (command.Equals("Delete"))
            {
                PageAction = "DELETE";
                if (ModelState.IsValid)
                {
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null) // Guard in case unlogged access happened
                    {
                        user = (UserSession)Session["User"];
                        result = MIMManager.DeleteMIM(MIMViewModel);
                    }
                    else
                        ModelState.AddModelError("", "Not logged in");
                }
            }
            if (result)
            {
                // Add to audit log
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Menu Item Master", PageAction, MIMViewModel.MIMMIC, MIMViewModel.MIMNAM);
                TempData["SuccessMessage"] = PageAction + "successful";
            }
            else TempData["ErrorMessage"] = PageAction + "failed";
            return RedirectToAction("Index");
        }
    }
}