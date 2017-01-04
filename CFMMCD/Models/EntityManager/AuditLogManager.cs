using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

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
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        }
        // Username, Date and time, Page audited, Action made, Id of affected item, Name of affected item
        public bool Audit(string UserName, DateTime Date_Time, string Page, string PageAction, string ItemId, string Name)
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
        public bool ExportToExcel(AuditLogViewModel ALViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                //Creates a DataTable
                DataTable dt = new DataTable();

                //Adds the Columns to the DataTable based on the AuditLog
                dt.Columns.Add("UserId", typeof(string));
                dt.Columns.Add("Date", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("Page", typeof(string));
                dt.Columns.Add("Page_Action", typeof(string));
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("Name", typeof(string));

                //Puts all the rows of the Audit Log Table
                List<Audit_Log> AlRow = db.Audit_Log.ToList();
                foreach (var vm in AlRow)
                {
                    var row = dt.NewRow();
                    row["UserId"] = vm.UserId;
                    row["Date"] = vm.Date;
                    row["Time"] = vm.Time;
                    row["Page"] = vm.Page;
                    row["Page_Action"] = vm.Page_Action;
                    row["ID"] = vm.Id;
                    row["Name"] = vm.Name;
                    dt.Rows.Add(row);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= AuditLog.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
            }
            return true;
        }
    }
}