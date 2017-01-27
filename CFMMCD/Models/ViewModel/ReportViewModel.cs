using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            Result = true;
            Message = "";
        }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int ErrorLevel { get; set; }
    }
}