using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using CFMMCD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class HomeManager
    {
        public List<NotificationViewModel> GetMenuItemNotification(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<NotificationViewModel> list = new List<NotificationViewModel>();
                Account currentAccount = db.Accounts.Single(o => o.Username.Equals(username));
                if (currentAccount.TimeLastLogged != null)
                {
                    DateTime lastUserAccess = (DateTime)currentAccount.TimeLastLogged;
                    foreach (var v in db.Audit_Log.Where(o => o.Page.Contains("Menu Item Master")))
                    {
                        if (lastUserAccess < v.Date_Time)
                        {
                            NotificationViewModel notif = new NotificationViewModel();
                            notif.ItemCode = v.ItemId;
                            notif.ItemName = v.Name;
                            notif.Action = v.Page_Action;
                            notif.Page = "Menu item";
                            list.Add(notif);
                        }
                    }
                }
                return list;
            }
        }

        public List<NotificationViewModel> GetRawItemNotification(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<NotificationViewModel> list = new List<NotificationViewModel>();
                Account currentAccount = db.Accounts.Single(o => o.Username.Equals(username));
                if (currentAccount.TimeLastLogged != null)
                {
                    DateTime lastUserAccess = (DateTime)currentAccount.TimeLastLogged;
                    foreach (var v in db.Audit_Log.Where(o => o.Page.Contains("Raw Item Master")))
                    {
                        if (lastUserAccess < v.Date_Time)
                        {
                            NotificationViewModel notif = new NotificationViewModel();
                            notif.ItemCode = v.ItemId;
                            notif.ItemName = v.Name;
                            notif.Action = v.Page_Action;
                            notif.Page = "Raw item";
                            list.Add(notif);
                        }
                    }
                }
                return list;
            }
        }

        public List<NotificationViewModel> GetVendorItemNotification(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<NotificationViewModel> list = new List<NotificationViewModel>();
                Account currentAccount = db.Accounts.Single(o => o.Username.Equals(username));
                if (currentAccount.TimeLastLogged != null)
                {
                    DateTime lastUserAccess = (DateTime)currentAccount.TimeLastLogged;
                    foreach (var v in db.Audit_Log.Where(o => o.Page.Contains("Vendor Master")))
                    {
                        if (lastUserAccess < v.Date_Time)
                        {
                            NotificationViewModel notif = new NotificationViewModel();
                            notif.ItemCode = v.ItemId;
                            notif.ItemName = v.Name;
                            notif.Action = v.Page_Action;
                            notif.Page = "Vendor";
                            list.Add(notif);
                        }
                    }
                }
                return list;
            }
        }
    }
}