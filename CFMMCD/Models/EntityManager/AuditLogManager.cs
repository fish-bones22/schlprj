using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class AuditLogManager
    {
        public List<AuditLogViewModel> GetAuditLog()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<AuditLogViewModel> ALList = new List<AuditLogViewModel>();
                foreach (Audit_Log al in db.Audit_Log)
                {
                    AuditLogViewModel ALViewModel = new AuditLogViewModel();
                    ALViewModel.Audit_Id = al.Audit_Id;
                    ALViewModel.Date = al.Date;
                    ALViewModel.Name = al.Name;
                    ALViewModel.Page = al.Page;
                    ALViewModel.Page_Action = al.Page_Action;
                    ALViewModel.Time = al.Time;
                    ALViewModel.UserId = al.UserId;
                    ALList.Add(ALViewModel);
                }
                return ALList;
            }
        }
        public bool Audit(AuditLogViewModel ALViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Audit_Log alRow = new Audit_Log();
                alRow.Audit_Id = int.Parse(DateTime.Now.ToString("yyMMdd")) + int.Parse(DateTime.Now.ToString("HHmmss")) + ( new Random().Next(9999));
                alRow.Name = ALViewModel.Name;
                alRow.Page = ALViewModel.Page;
                alRow.Page_Action = ALViewModel.Page_Action;
                alRow.Time = ALViewModel.Time;
                alRow.UserId = ALViewModel.UserId;
                alRow.Date = ALViewModel.Date;

                try
                {
                    db.Audit_Log.Add(alRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}