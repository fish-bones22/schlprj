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
                    Session["User"] = userSession;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "The password supplied is incorrect");

            }
            return View();
        }
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel account)
        {
            if(ModelState.IsValid)
            {
                AccountManager accMan = new AccountManager();
                if (!accMan.IsUsernameExist(account.Username))
                {
                    accMan.CreateUserAccount(account);
                    FormsAuthentication.SetAuthCookie(account.Username, false);
                }
                else
                    ModelState.AddModelError("", "Username already taken");
            }
            return View();
        }
    }
}