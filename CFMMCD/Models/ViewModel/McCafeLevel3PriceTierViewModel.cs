﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class McCafeLevel3PriceTierViewModel
    {
        public string Id { get; set; }
        public string Price_Tier { get; set; }
        // Added 
        public List<McCafeLevel3PriceTierViewModel> M3PTList { get; set; }
    }
}