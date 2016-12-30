using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class MenuRecipeViewModel
    {
        public MenuRecipeViewModel()
        {
            RIRRIC = new List<string>();
            RIMRID = new List<string>();
            RIMCPR = new List<string>();
            RIRSFQ = new List<string>();
            RIRCWC = new List<string>();
            RIRSTA = new List<string>();
            STOATT = new List<string>();
        }
        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }
        public string RIRMIC { get; set; }
        public string MIMLON { get; set; }
        public string MIMDSC { get; set; }
        public List<string> RIRRIC { get; set; }
        public List<string> RIMRID { get; set; }
        public List<string> RIMCPR { get; set; }
        public List<string> RIRSFQ { get; set; }
        public List<string> RIRCWC { get; set; }
        public List<string> RIRSTA { get; set; }
        public List<string> STOATT { get; set; }
    }
}