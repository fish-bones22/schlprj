using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class ValueMealViewModel
    {
        public ValueMealViewModel()
        {
            MIMMIC = new List<string>();
            MIMNAM = new List<string>();
            MIMPRI = new List<string>();
            MIMPRO = new List<string>();
            VMLQUA = new List<string>();
            MIMMIC.Add("");
            MIMNAM.Add("");
            MIMPRI.Add("");
            MIMPRO.Add("");
            VMLQUA.Add("");
            MenuItemList = new List<VMMenuItem>();
        }
        public string SearchItem { get; set; }
        public string VMLNUM { get; set; }
        public string VMLNAM { get; set; }
        public string VMLMIC { get; set; }

        public List<string> MIMMIC { get; set; }
        public List<string> MIMNAM { get; set; }
        public List<string> MIMPRI { get; set; }
        public List<string> MIMPRO { get; set; }
        public List<string> VMLQUA { get; set; }

        public string VMLPRI { get; set; }
        public string VMLPRO { get; set; }
        public List<VMMenuItem> MenuItemList { get; set; }
        public List<ValueMealViewModel> ValueMealList { get; set; }
    }
    public class VMMenuItem
    {
        public string MIMMIC { get; set; }
        public string MIMNAM { get; set; }
        public string MIMPRI { get; set; }
        public string MIMPRO { get; set; }
        public string VMLQUA { get; set; }
    }
}