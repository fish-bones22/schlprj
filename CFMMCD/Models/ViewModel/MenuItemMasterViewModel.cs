using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class MenuItemMasterViewModel
    {
        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }

        // CHMMIMP0 Table Items
        [Required(ErrorMessage = "This field is required")]
        public string MIMMIC { get; set; }
        public string MIMSTA { get; set; }
        public string MIMFGC { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string MIMNAM { get; set; }
        public string MIMLON { get; set; }
        public string MIMDSC { get; set; }
        public string MIMDPC { get; set; }
        public string MIMTCI { get; set; }
        public string MIMPRI { get; set; }
        public string MIMTCA { get; set; }
        public string MIMPRO { get; set; }
        public string MIMTCG { get; set; }
        public string MIMPRG { get; set; }
        public string MIMPND { get; set; }
        public string MIMWGR { get; set; }
        public string MIMHPT { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string MIMUTC { get; set; }
        public string MIMEDT { get; set; }
        public string MIMNPI { get; set; }
        public string MIMNPO { get; set; }
        public string MIMNPD { get; set; }
        public string MIMNPA { get; set; }
        public string MIMNNP { get; set; }
        public string MIMNPT { get; set; }

        // NP6 Items
        public string MIMMIC_NP6 { get; set; }
        public string MIMNAM_NP6 { get; set; }
        public string MIMLON_NP6 { get; set; }

        public string Location { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string StoreList { get; set; }
        public bool SelectAllCb { get; set; }
        public bool SelectExcept { get; set; }

        public List<MenuItemMasterViewModel> MenuItemMasterList { get; set; }
    }
}