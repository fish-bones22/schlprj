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
            return View(MIMManager.SearchMIM(MIMiewModel));
        }

        [HttpPost]
        public ActionResult UpdateDelete(MenuItemMasterViewModel MIMViewModel, string command)
        {
            if (command.Equals("Save"))
            {
                if (ModelState.IsValid)
                {
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    if (Session["User"] != null)
                    {
                        UserSession user = (UserSession)Session["User"];
                        MIMManager.SaveMIM(MIMViewModel, user.Username);
                    } else
                        ModelState.AddModelError("", "Not logged in");
                }
                else
                    ModelState.AddModelError("", "Fill up all fields");
            }
            else if (command.Equals("Delete"))
            {

            }
            return RedirectToAction("MenuItemMaster");
        }
    }
}