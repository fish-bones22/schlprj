﻿using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class RegularPriceTierController : Controller
    {
        UserSession user;
        // GET: RegularPriceTier
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.TIP) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("RPT", "HOME", "LOG");
            return View(new RegularPriceTierViewModel());
        }

        [HttpPost]
        public ActionResult UpdateDelete(RegularPriceTierViewModel RPTViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                RegularPriceTierManager RPTManager = new RegularPriceTierManager();
                result = RPTManager.UpdateRegularPriceTier(RPTViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                RegularPriceTierManager RPTManager = new RegularPriceTierManager();
                result = RPTManager.DeleteRegularPriceTier(RPTViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Regular Price Tier", PageAction, RPTViewModel.Id, RPTViewModel.Price_Tier);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}