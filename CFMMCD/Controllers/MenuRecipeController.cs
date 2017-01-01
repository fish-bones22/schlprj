using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFMMCD.Models.ViewModel;

namespace CFMMCD.Controllers
{
    public class MenuRecipeController : Controller
    {
        // GET: MenuRecipe
        public ActionResult Index()
        {
            return View(new MenuRecipeViewModel());
        }
    }
}