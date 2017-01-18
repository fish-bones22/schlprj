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
    public class MenuItemPriceUpdateController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.MIM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("MIP", "HOME", "LOG");

            // SearchItemSelected is assigned value at DisplaySearchResult
            MenuItemMasterViewModel MIMViewModel = new MenuItemMasterViewModel();
            MenuItemPriceUpdateViewModel MIPViewModel = new MenuItemPriceUpdateViewModel();
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems("ALL");
            for (int i = 0; i < MIMViewModel.MenuItemMasterList.Count(); i++)
            {
                TierUpdate tu = MenuItemMasterManager.SearchPriceTierUpdate(MIMViewModel.MenuItemMasterList[i].MIMMIC);
                if (tu != null)
                    MIPViewModel.TierUpdateList.Add(tu);
            }
                
            return View(MIPViewModel);
        }

        [HttpPost]
        public ActionResult Index(MenuItemMasterViewModel MIMViewModel)
        {   // Search
            MIMViewModel.MenuItemMasterList = MenuItemMasterManager.SearchMenuItems(MIMViewModel.SearchItem);
            if (MIMViewModel.MenuItemMasterList != null)
            {
                TempData["SearchResult"] = 1;   // Stores 1 if a search returned results.
                Session["ViewModelList"] = MIMViewModel.TierList;
            }
            else
                ModelState.AddModelError("", "No results found");
            return View(MIMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(MenuItemPriceUpdateViewModel MIPViewModel, string command)
        {
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUploaded"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    // store the file inside ~/App_Data/uploads folder
                    string path = "~/App_Data/uploads/" + fileName;
                    file.SaveAs(Server.MapPath(path));
                }
            }
            if (command == "Save")
            {
                result = MenuItemMasterManager.UpdatePriceTier(MIPViewModel.TierUpdateList);
                PageAction = "Update price";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                foreach (var v in MIPViewModel.TierUpdateList)
                {
                    new AuditLogManager().Audit(user.Username, DateTime.Now, "Menu Item Price Update", PageAction, v.MIMMIC, v.MIMNAM);
                }
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}