using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class StoreProfileController : Controller
    {
        // GET: StoreProfile
        public ActionResult StoreProfile()
        {
            return View(new StoreProfileViewModel());
        }

        [HttpPost]
        public ActionResult StoreProfile(StoreProfileViewModel StoreProf)
        {
            // Search
            StoreProfileManager SPManager = new StoreProfileManager();
            StoreProfileViewModel model = new StoreProfileViewModel();
            model = SPManager.SearchStoreProfile(StoreProf);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveUpdateDelete(StoreProfileViewModel SPViewModel, string command)
        {
            if (command == "Save")
            {
                //Code for Add
                Session["pageact"] = "Add"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.CreateStoreProfile(SPViewModel);
            }
            else if (command == "Update")
            {
                //Code for Update
                Session["pageact"] = "Update"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.UpdateStoreProfile(SPViewModel);
            }
            else if (command == "Delete")
            {
                //Code for Delete
                Session["pageact"] = "Delete"; // Session for what a user did
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.DeleteStoreProfile(SPViewModel);
            }
            return RedirectToAction("StoreProfile");
        }
    }
}