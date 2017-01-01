using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class RegularPriceTierViewModel
    {
        public string Id { get; set; }
        public string Price_Tier { get; set; }
        // Added 
        public List<RegularPriceTierViewModel> RegPTList { get; set; }
    }
}