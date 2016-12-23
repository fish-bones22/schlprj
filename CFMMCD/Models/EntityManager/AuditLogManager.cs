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

                    ALViewModel.UserId = al.UserId;
                    ALViewModel.Date = al.Date;
                    ALViewModel.Time = al.Time;
                    ALViewModel.Page = al.Page;
                    ALViewModel.ItemId = al.ItemId;
                    ALViewModel.Name = al.Name;
                    ALViewModel.Page_Action = al.Page_Action;
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
                alRow.Id = int.Parse(DateTime.Now.ToString("yyMMdd")) + new Random().Next(999) + new Random().Next(999); // To be changed soon
                alRow.UserId = ALViewModel.UserId;
                alRow.Date = ALViewModel.Date;
                alRow.Time = ALViewModel.Time;
                alRow.Page = ALViewModel.Page;
                alRow.ItemId = ALViewModel.ItemId;
                alRow.Page_Action = ALViewModel.Page_Action;
                alRow.Name = ALViewModel.Name;

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
                        // Username, Date and time, Page audited, Action made, Id of affected item, Name of affected item
        public bool Audit(string UserName, DateTime Date_Time, string Page, string PageAction, string ItemId, string Name )
        {
            AuditLogViewModel ALViewModel = new AuditLogViewModel
            {
                UserId = UserName,
                Date = Date_Time.ToString("yyyy-MM-dd"),
                Time = Date_Time.ToString("hh:mm"),
                Name = Name,
                Page = Page,
                Page_Action = PageAction,
                ItemId = ItemId
            };
            Audit(ALViewModel);
            return true;
        }
    }
}