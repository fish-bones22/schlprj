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
    public class AuditLogController : Controller
    {
        // GET: AuditLog
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.AUL) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("AUL", "HOME", "LOG");

            AuditLogViewModel ULViewModel = new AuditLogViewModel();
            AuditLogManager ALManager = new AuditLogManager();
            List<AuditLogViewModel> ALList = ALManager.GetAuditLog();

            ViewData["ModelList"] = ALList;
            return View();
        }
    }
}