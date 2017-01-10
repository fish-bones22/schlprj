using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class AuditLogViewModel
    {
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public string Date_Time { get; set; }
        public string Page { get; set; }
        public string Page_Action { get; set; }
        public string Name { get; set; }
    }
}