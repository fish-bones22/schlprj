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
    public class ProjectGoldPriceTierController : Controller
    {
        UserSession user;
        // GET: ProjectGoldPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.STP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("PG", "HOME", "LOG");
            return View(new ProjectGoldPriceTierViewModel());
        }
        public ActionResult UpdateDelete(ProjectGoldPriceTierViewModel PGViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                ProjectGoldPriceTierManager PGManager = new ProjectGoldPriceTierManager();
                result = PGManager.UpdateProjectGoldPriceTier(PGViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                ProjectGoldPriceTierManager PGManager = new ProjectGoldPriceTierManager();
                result = PGManager.DeleteProjectGoldPriceTier(PGViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "ProjectGold Price Tier", PageAction, PGViewModel.Id, PGViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}