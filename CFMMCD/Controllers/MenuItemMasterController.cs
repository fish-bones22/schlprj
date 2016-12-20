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
        // GET: MenuItemMaster
        public ActionResult MenuItemMaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateDelete(MenuItemMasterViewModel MIMViewModel, string command)
        {
            if (command.Equals("Save"))
            {
                if (ModelState.IsValid)
                {
                    MenuItemMasterManager MIMManager = new MenuItemMasterManager();
                    UserSession user = (UserSession)Session["User"];
                    MIMManager.SaveMIM(MIMViewModel, user.Username); 
                }
                else
                    ModelState.AddModelError("", "Fill up all fields");
            }
            return View();
        }
    }
}