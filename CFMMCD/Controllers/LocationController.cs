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
    public class LocationController : Controller
    {
        UserSession user;
        // GET: Location
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            // LOC -> Location
            // Refer to UserAccessSession 
            if (UASession == null || !UASession.LOC) return RedirectToAction("Login", "Account");

            user = (UserSession)Session["User"];
            Session["CurrentPage"] = new CurrentPageSession("LCN", "HOME", "LOG");

            // Get all data stored in DB table
            LocationManager LCNManager = new LocationManager();
            LocationViewModel LCNViewModel = new LocationViewModel();
            LCNViewModel.LCList = LCNManager.GetLCN();
            if (LCNViewModel.LCList == null || LCNViewModel.LCList.Count() == 0)
                LCNViewModel.LCList = new List<LocationViewModel>();
            // return View with ViewModel
            return View(LCNViewModel);
        }

        [HttpPost]
        public ActionResult UpdateDelete(LocationViewModel LCNViewModel, string command)
        {
            string PageAction = "";
            bool result = false;
            user = (UserSession)Session["User"];

            if (command == "Save")
            {
                LocationManager LCNManager = new LocationManager();
                result = LCNManager.UpdateLocation(LCNViewModel);
                PageAction = "UPDATE";
            }
            else if (command == "Delete")
            {
                LocationManager LCNManager = new LocationManager();
                result = LCNManager.DeleteLocation(LCNViewModel);
                PageAction = "DELETE";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Location", PageAction, LCNViewModel.Id, LCNViewModel.LOCATN);
            }
            else TempData["ErrorMessage"] = PageAction + " failed";
            return RedirectToAction("Index");
        }
    }
}