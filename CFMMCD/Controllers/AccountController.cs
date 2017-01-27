using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CFMMCD.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {   // Reset Sessions
            if (Session["User"] != null)
            {
                string username = ((UserSession)Session["User"]).Username;
                Session["User"] = null;
            }

            if (Session["UserAccess"] != null)
                Session["UserAccess"] = null;
            if (Session["ViewModelList"] != null)
                Session["ViewModelList"] = null;

            if (Session["MenuItemNotif"] != null)
                Session["MenuItemNotif"] = null;
            if (Session["RawItemNotif"] != null)
                Session["RawItemNotif"] = null;
            if (Session["VendorNotif"] != null)
                Session["VendorNotif"] = null;

            Session["CurrentPage"] = new CurrentPageSession("LOGIN");
            return View();
        }
        /*
         * Validates if provided credentials matches account in the database.
         * If it matches, then a successful Login occurs and redirects to home view.
         * 
         * Returns view of Login page with error texts if unsuccessful.
         */
        [HttpPost]
        public ActionResult Login(LoginViewModel credentials, string returnURL)
        {
            if (ModelState.IsValid)
            {
                AccountManager accMan = new AccountManager();

                if (accMan.CheckPassword(credentials.Username, credentials.Password))
                {
                    FormsAuthentication.SetAuthCookie(credentials.Username, false);
                    // Set User session
                    UserSession userSession = new UserSession();
                    userSession.Username = credentials.Username;
                    userSession.UserID = accMan.GetUserID(credentials.Username);
                    Session["User"] = userSession;
                    // Set User access
                    UserAccessSession UASession = new UserAccessSession();
                    Session["UserAccess"] = accMan.SetUserAccess(credentials.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "The password or username supplied is incorrect");
            }
            return View();
        }
        public ActionResult CreateAccount()
        {   // Validate log in and user access
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.UAP) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("UAP_CREATE", "HOME", "LOG");
            AccountViewModel AViewModel = new AccountViewModel();
            AccountManager AManager = new AccountManager();
            AViewModel.AccountList = AManager.SearchAccounts("ALL");
            return View(AViewModel);
        }
        [HttpPost]
        public ActionResult CreateAccount(AccountViewModel CAViewModel)
        {
            AccountViewModel AViewModel = new AccountViewModel();
            AccountManager AManager = new AccountManager();
            AViewModel.AccountList = AManager.SearchAccounts("ALL");
            if (ModelState.IsValid)
            {
                AccountManager accMan = new AccountManager();
                if (!accMan.IsUsernameExist(CAViewModel.Username))
                {
                    accMan.UpdateUserAccount(CAViewModel);
                    FormsAuthentication.SetAuthCookie(CAViewModel.Username, false);
                    TempData["SuccessMessage"] = "Successfuly created account";
                    UserSession us = (UserSession)Session["User"];
                    // Add to audit log
                    new AuditLogManager().Audit(us.Username, DateTime.Now, "User Access page", "CREATE", CAViewModel.Username, CAViewModel.Username);
                }
                else
                    ModelState.AddModelError("", "Username already taken");
            }
            return View(AViewModel);
        }


        public ActionResult EditAccount()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.UAP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("UAP_EDIT", "HOME", "LOG");

            AccountManager AManager = new AccountManager();
            AccountViewModel AViewModel = new AccountViewModel();
            AViewModel.AccountList = AManager.SearchAccounts("ALL");
            return View(AViewModel);
        }

        [HttpPost]
        public ActionResult EditAccount ( AccountViewModel AViewModel, string command, string value )
        {
            AccountManager AManager = new AccountManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;
            if ( command == null )
            {
                AViewModel = AManager.SearchSingleAccount(value);
                AViewModel.AccountList = AManager.SearchAccounts("ALL");

                return View(AViewModel);
            }
            else if (command == "Save")
            {
                if (AViewModel.Username.Equals(""))
                    ModelState.AddModelError("", "The username field is required");
                if (AViewModel.Password == null || AViewModel.Password.Equals(""))
                    ModelState.AddModelError("", "The password field is required");
                if (AViewModel.Password == null || !AViewModel.PasswordVerify.Equals(AViewModel.Password))
                    ModelState.AddModelError("", "Passwords didn't matched");
                else result = AManager.UpdateUserAccount(AViewModel);
                PageAction = "Update";
            }
            else if (command == "Delete")
            {
                result = AManager.DeleteUserAccount(AViewModel);
                PageAction = "Delete";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Edit User Account", PageAction, AViewModel.Username, AViewModel.Username);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("EditAccount");
        }

        public ActionResult EditAccess()
        {
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.UAP) return RedirectToAction("Login", "Account");
            // Set NavBar Links accordingly
            Session["CurrentPage"] = new CurrentPageSession("UAP_ACCESS_EDIT", "HOME", "LOG");
            
            AccountManager AManager = new AccountManager();
            AccountViewModel AViewModel = new AccountViewModel();
            AViewModel.AccountList = AManager.SearchAccounts("ALL");
            return View(AViewModel);
        }

        [HttpPost]
        public ActionResult EditAccess(AccountViewModel AViewModel, string command, string value)
        {
            AccountManager AManager = new AccountManager();
            UserSession user = (UserSession)Session["User"];
            string PageAction = "";
            bool result = false;

            if (command == null)
            {
                AViewModel = AManager.SearchSingleAccount(value);
                AViewModel.AccountList = AManager.SearchAccounts("ALL");
                return View(AViewModel);
            }
            else if (command == "Save")
            {
                result = AManager.UpdateUserAccount(AViewModel);
                PageAction = "Update";
            }
            if (result)
            {
                TempData["SuccessMessage"] = PageAction + " successful";
                new AuditLogManager().Audit(user.Username, DateTime.Now, "Edit User Account", PageAction, AViewModel.Username, AViewModel.Username);
            }
            else
            {
                TempData["ErrorMessage"] = PageAction + " failed";
            }
            return RedirectToAction("EditAccess");
        }
    }
}