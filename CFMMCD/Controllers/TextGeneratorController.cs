using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class TextGeneratorController : Controller
    {
        // GET: TextGenerator
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Index(TextGeneratorViewModel TGViewModel)
        {
            return View();
        }

    }
}