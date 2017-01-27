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
    public class VendorMasterController : Controller
    {
        /*
         * Default method.
         * TempData is used to store the ViewModel after a Search action.
         */
        public ActionResult Index(string id)
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.VEM) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("VEM", "HOME", "LOG");

            // Initialize page
            VendorMasterViewModel VMViewModel = new VendorMasterViewModel();
            if (id != null)
                VMViewModel = VendorMasterManager.SearchSingleVendor(id);
            VMViewModel.VendorMasterList = VendorMasterManager.SearchVendors("ALL");
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult Index(VendorMasterViewModel VMViewModel, string value)
        {
            VMViewModel = VendorMasterManager.SearchSingleVendor(value);
            VMViewModel.VendorMasterList = VendorMasterManager.SearchVendors("ALL");
            return View(VMViewModel);
        }
        [HttpPost]
        public ActionResult UpdateDelete(VendorMasterViewModel VMViewModel, string command)
        {
            VendorMasterManager VMManager = new VendorMasterManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUploaded"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    ReportViewModel report = VendorMasterManager.ImportExcel(file.InputStream, user.Username);
                    PageAction = "Import";
                    result = report.Result;
                    if (!result)
                    {
                        if (report.ErrorLevel == 2)
                            PageAction = report.Message + ": Import partially";
                        else
                            PageAction = report.Message + ": Import";
                    }
                    else
                        PageAction = report.Message + ": Import";
                }
            }

            if (command == "Save")
            {
                result = VendorMasterManager.UpdateVendor(VMViewModel, user.Username);
                PageAction = "Update";
            }

            else if (command == "Delete")
            {
                result = VendorMasterManager.DeleteVendor(VMViewModel);
                PageAction = "Delete";
            }

            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Vendor Master", PageAction, VMViewModel.VEMVEN, VMViewModel.VEMDS1);

                if (!PageAction.Equals("Delete"))
                    return RedirectToAction("Index", new { id = VMViewModel.VEMVEN });
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}