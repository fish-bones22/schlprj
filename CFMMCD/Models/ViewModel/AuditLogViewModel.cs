using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class AuditLogViewModel
    {
        public int Audit_Id { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Page { get; set; }
        public string Page_Action { get; set; }
        public string Name { get; set; }
    }
}