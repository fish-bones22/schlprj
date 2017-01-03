using CFMMCD.Models.EntityManager;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using Rotativa;
using System.Web;
using System.Web.Mvc;

namespace CFMMCD.Controllers
{
    public class ReportGenerationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExportPDF()
        {
            var model = new GeneratePDF();
            //Code to get content
            return new Rotativa.ActionAsPdf("ReportGeneration", model) { FileName = "ReportGeneration.pdf" };
        }
    }
}