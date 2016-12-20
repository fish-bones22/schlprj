using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using CFMMCD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class AuditLogManager
    {
        /*
         * Views all of the values in the Table Audit_Log
         */
         public void ViewAuditLog(AuditLogViewModel AudLog)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Audit_Log alRow = new Audit_Log();

                alRow.Audit_Id = AudLog.Audit_Id;
                alRow.UserId = Session["userid"]; //Session of UserId
                alRow.Date = DateTime.Now;
                alRow.Time = DateTime.FromFileTimeUtc;
                alRow.Page = Session["page"]; //Session where  the page was from
                alRow.Page_Action = Session["pageact"]; //Session of what Action was made
                alRow.Name = Session["name"]; //Session of the name of the user
            }
        }
    }
}