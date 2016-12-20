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
            return View();
        }

        [HttpPost]
        public ActionResult StoreProfile(StoreProfileViewModel StoreProf, string command)
        {
            if (command == "Search")
            {
                //Code for Search
                StoreProfileManager SPManager = new StoreProfileManager();
                StoreProfileViewModel model = new StoreProfileViewModel();
                model = SPManager.SearchStoreProfile(StoreProf);
                return View(model);
            }
            else if (command == "Save")
            {
                //Code for Add
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.CreateStoreProfile(StoreProf);
            }
            else if (command == "Update")
            {
                //Code for Update
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.UpdateStoreProfile(StoreProf);
            }
            else if (command == "Delete")
            {
                //Code for Delete
                StoreProfileManager SPManager = new StoreProfileManager();
                SPManager.DeleteStoreProfile(StoreProf);
            }
            return View();
        }
    }
}