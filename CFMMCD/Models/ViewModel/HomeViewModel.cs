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
            MenuItemNotif = new List<MenuItem>();
            RawItemNotif = new List<RawItem>();
            VendorNotif = new List<Vendor>();
        }
        public List<MenuItem> MenuItemNotif { get; set; }
        public List<RawItem> RawItemNotif { get; set; }
        public List<Vendor> VendorNotif { get; set; }
    }
}