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
        {
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "Login",
                Controller = "Account"
            };
            Session["CurrentPage"] = CPSession;
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
        {
            CurrentPageSession CPSession = new CurrentPageSession();
            CPSession.Self = new CurrentPageSession.LinkString
            {
                Action = "CreateAccount",
                Controller = "Account"
            };
            CPSession.Parent = new CurrentPageSession.LinkString
            {
                Action = "Index",
                Controller = "Home"
            };
            CPSession.Grandparent = new CurrentPageSession.LinkString
            {
                Action = "Login",
                Controller = "Account"
            };
            Session["CurrentPage"] = CPSession;
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel account)
        {
            if(ModelState.IsValid)
            {
                Session["pageact"] = "Create User";
                AccountManager accMan = new AccountManager();
                if (!accMan.IsUsernameExist(account.Username))
                {
                    accMan.CreateUserAccount(account);
                    FormsAuthentication.SetAuthCookie(account.Username, false);
                    TempData["SuccessMessage"] = "Successfuly created account";
                    //** Audit Log **
                    // Prepare data to be passed to adit log manager
                    UserSession us = (UserSession)Session["User"];
                    AuditLogViewModel ALViewModel = new AuditLogViewModel
                    {
                        Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                        Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm")),
                        Name = us.Username,
                        Page = "USERACC",
                        Page_Action = "Create",
                        UserId = us.UserID
                    };
                    // Add to audit log
                    new AuditLogManager().Audit(ALViewModel);
                }
                else
                    ModelState.AddModelError("", "Username already taken");
            }
            return View(new CreateAccountViewModel());
        }
    }
}