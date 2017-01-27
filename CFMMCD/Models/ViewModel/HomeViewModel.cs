using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            MenuItemNotif = new List<NotificationViewModel>();
            RawItemNotif = new List<NotificationViewModel>();
            VendorNotif = new List<NotificationViewModel>();
        }
        public List<NotificationViewModel> MenuItemNotif { get; set; }
        public List<NotificationViewModel> RawItemNotif { get; set; }
        public List<NotificationViewModel> VendorNotif { get; set; }
    }

    public class NotificationViewModel
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Action { get; set; }
        public string Page { get; set; }
    }
}