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
                Session["User"] = null;
            if (Session["UserAccess"] != null)
                Session["UserAccess"] = null;

            Session["CurrentPage"] = new CurrentPageSession("LOG");
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
                string password = accMan.GetPassword(credentials.Username);

                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The username supplied is not valid");
                else if (credentials.Password.Equals(password))
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
                    ModelState.AddModelError("", "The password supplied is incorrect");

            }
            return View();
        }
        public ActionResult CreateAccount()
        {   // Validate log in and user access
            // Validate log in and user access
            UserAccessSession UASession = (UserAccessSession)Session["UserAccess"];
            if (UASession == null || !UASession.UAP) return RedirectToAction("Login", "Account");

            Session["CurrentPage"] = new CurrentPageSession("UAP_CREATE", "HOME", "LOG");
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel CAViewModel)
        {
            if(ModelState.IsValid)
            {
                AccountManager accMan = new AccountManager();
                if (!accMan.IsUsernameExist(CAViewModel.Username))
                {
                    accMan.CreateUserAccount(CAViewModel);
                    FormsAuthentication.SetAuthCookie(CAViewModel.Username, false);
                    TempData["SuccessMessage"] = "Successfuly created account";
                    UserSession us = (UserSession)Session["User"];
                    // Add to audit log
                    new AuditLogManager().Audit(us.Username, DateTime.Now, "User Access page", "CREATE", CAViewModel.Username, CAViewModel.Username);
                }
                else
                    ModelState.AddModelError("", "Username already taken");
            }
            return View(new CreateAccountViewModel());
        }
    }
}