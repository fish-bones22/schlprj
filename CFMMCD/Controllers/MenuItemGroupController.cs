using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class MenuItemGroupController : Controller
    {
        // GET: MenuItemGroup
        public ActionResult Index()
        {
            ItemGroupViewModel IGViewModel = new ItemGroupViewModel();
            IGViewModel.ItemType = 0;
            IGViewModel.MenuItemList = MenuItemMasterManager.SearchMenuItems("ALL");
            IGViewModel.RawItemList = RawItemMasterManager.GetRawItems("ALL");
            IGViewModel.GroupList = ItemGroupManager.GetGroup(IGViewModel);
            return View();
        }
    }
}