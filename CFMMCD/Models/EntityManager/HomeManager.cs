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
        public List<MenuItem> GetMenuItemNotification(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MenuItem> list = new List<MenuItem>();
                DateTime lastUserAccess = (DateTime) db.Accounts.Single(o => o.Username.Equals(username)).TimeLastLogged;
                foreach (var v in db.CSHMIMP0)
                {
                    if (lastUserAccess.CompareTo(v.MIMDAT) < 0)
                    {
                        MenuItem mi = new MenuItem();
                        mi.RIRMIC = v.MIMMIC.ToString();
                        mi.MIMDSC = v.MIMNAM.Trim();
                        list.Add(mi);
                    }
                }
                return list;
            }
        }

        public List<RawItem> GetRawItemNotification(string username)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItem> list = new List<RawItem>();
                DateTime lastUserAccess = (DateTime)db.Accounts.Single(o => o.Username.Equals(username)).TimeLastLogged;
                foreach (var v in db.INVRIMP0)
                {
                    if (lastUserAccess.CompareTo(v.RIMDAT) < 0)
                    {
                        RawItem mi = new RawItem();
                        mi.RIMRIC = v.RIMRIC.ToString();
                        mi.RIMRID = v.RIMRID.Trim();
                        list.Add(mi);
                    }
                }
                return list;
            }
        }
    }
}