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
    public class ReportGenerationController : Controller
    {
        // GET: ReportGeneration
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.REG) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("REG", "HOME", "LOG");
            
            return View();
        }

        [HttpPost]
        public ActionResult Export(MenuItemMasterViewModel MIMVM, RawItemMasterViewModel RIMVM, MenuRecipeViewModel MRVM, StoreProfileViewModel SPVM, string command)
        {
            bool result = false;

            if (command == "toExcel")
            {
                ReportGenerationManager RGM = new ReportGenerationManager();
                result = RGM.RGtoExcel(MIMVM, MRVM, RIMVM, SPVM);
            }
            if (command == "toPDF")
            {

            }
            return RedirectToAction("Index");
        }
    }
}