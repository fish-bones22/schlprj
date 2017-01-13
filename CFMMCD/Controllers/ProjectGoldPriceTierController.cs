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
            // TIP -> Price Tier
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("TIP_GOL", "HOME", "LOG");

            // Get all data stored in DB table
            ProjectGoldPriceTierManager PGTManager = new ProjectGoldPriceTierManager();
            ProjectGoldPriceTierViewModel PGTViewModel = new ProjectGoldPriceTierViewModel();
            PGTViewModel.PrjPTList = PGTManager.GetPGT();
            if (PGTViewModel.PrjPTList == null || PGTViewModel.PrjPTList.Count() == 0)
                PGTViewModel.PrjPTList = new List<ProjectGoldPriceTierViewModel>();
            // return View with ViewModel
            return View(PGTViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(ProjectGoldPriceTierViewModel PGTViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                ProjectGoldPriceTierManager PGTManager = new ProjectGoldPriceTierManager();
                result = PGTManager.UpdateProjectGoldPriceTier(PGTViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                ProjectGoldPriceTierManager PGTManager = new ProjectGoldPriceTierManager();
                result = PGTManager.DeleteProjectGoldPriceTier(PGTViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "ProjectGold Price Tier", PageAction, PGTViewModel.Id, PGTViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}