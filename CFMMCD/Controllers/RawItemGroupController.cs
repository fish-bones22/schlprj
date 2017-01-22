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
    public class RawItemGroupController : Controller
    {
        public ActionResult Index()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.GRP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("GRP_RIM", "HOME", "LOG");


            ItemGroupViewModel IGViewModel = new ItemGroupViewModel();
            IGViewModel.GroupType = 2;
            IGViewModel.RawItemList = ItemGroupManager.GetFilteredRawItem();
            IGViewModel.AllStoreList = ItemGroupManager.GetFilteredStore();
            IGViewModel.GroupList = ItemGroupManager.GetGroup(IGViewModel);
            return View(IGViewModel);
        }

        [HttpPost]
        public ActionResult AddNewGroup(ItemGroupViewModel IGViewModel)
        {
            UserSession user = (UserSession)Session["User"];
            bool result;
            string PageAction = "Add group";
            result = ItemGroupManager.UpdateGroup(IGViewModel);
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Group", PageAction, IGViewModel.GroupId.ToString(), IGViewModel.GroupName);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemGroupViewModel IGViewModel, string value)
        {
            UserSession user = (UserSession)Session["User"];
            bool result;
            string PageAction = "Update";
            if (IGViewModel.ItemName == null || IGViewModel.ItemName.Equals("") || value != null)
            {
                PageAction = "Delete item";
                result = ItemGroupManager.DeleteItem(int.Parse(value));
            }
            else
            {
                PageAction = "Add item";
                result = ItemGroupManager.UpdateGroup(IGViewModel);
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Group", PageAction, IGViewModel.GroupId.ToString(), IGViewModel.GroupName);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteGroup(string value)
        {
            UserSession user = (UserSession)Session["User"];
            bool result;
            string PageAction = "Delete group";
            result = ItemGroupManager.DeleteGroup(int.Parse(value));
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                string Name = ItemGroupManager.GetGroupName(int.Parse(value));
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Raw Item Group", PageAction, value, Name);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("Index");
        }
    }
}