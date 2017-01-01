using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class McCafeLevel2PriceTierViewModel
    {
        public string Id { get; set; }
        public string Price_Tier { get; set; }
        // Added 
        public List<McCafeLevel2PriceTierViewModel> M2PTList { get; set; }
    }
}
}