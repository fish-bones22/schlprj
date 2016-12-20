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
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null) // Guard in case unlogged access happened
                    {
                        // Used in CSHMIMP0 table, MIMUSR row 
                        UserSession user = (UserSession)Session["User"];
                        bool isSuccessful = MIMManager.SaveMIM(MIMViewModel, user.Username);
                        // Used for alert messages in view 
                        if (isSuccessful) TempData["SuccessMessage"] = "Saved";
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