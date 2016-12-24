using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class VendorMasterViewModel
    {
        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }
        public string VEMVEN { get; set; }
        public string VEMWSI { get; set; }
        public string VEMDS1 { get; set; }
        public string VEMDS2 { get; set; }
        public string VEMCCD { get; set; }
        public string VEMZIP { get; set; }
        public string VEMCTY { get; set; }
        public string VEMSTR { get; set; }
        public string VEMTEL { get; set; }
        public string VEMSTN { get; set; }
        public string VEMLOC { get; set; }
        public string VEMDAY { get; set; }
        public string VEMTID { get; set; }
        public string VEMSTA { get; set; }
        public string VEMDAT { get; set; }
        public string VEMUSR { get; set; }
        public string VEMADE { get; set; }
        public string VEMDEL { get; set; }

        public List<VendorMasterViewModel> VendorMasterList { get; set; }
    }
}