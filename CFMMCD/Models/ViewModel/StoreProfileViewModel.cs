using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class StoreProfileViewModel
    {
        public string StoreNameNumber { get; set; }

        public int STORE_NO { get; set; }
        public string STORE_NAME { get; set; }
        public string OWNERSHIP { get; set; }
        public string BREAKFAST_PRICE_TIER { get; set; }
        public string REGULAR_PRICE_TIER { get; set; }
        public string DC_PRICE_TIER { get; set; }
        public string MDS_PRICE_TIER { get; set; }
        public string MCCAFE_LEVEL_2_PRICE_TIER { get; set; }
        public string MCCAFE_LEVEL_3_PRICE_TIER { get; set; }
        public string MCCAFE_BISTRO_PRICE_TIER { get; set; }
        public string PROJECT_GOLD_PRICE_TIER { get; set; }
        public int PROFIT_CENTER { get; set; }
        public string REGION { get; set; }
        public string PROVINCE { get; set; }
        public string LOCATION { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string FRESH_OR_FROZEN { get; set; }
        public string PAPER_OR_PLASTIC { get; set; }
        public string SOFT_SERVE_OR_VANILLA_POWDER_MIX { get; set; }
        public string SIMPLOT_OR_MCCAIN { get; set; }
        public string MCCORMICK_OR_GSF { get; set; }

        public bool Hrs24Input { get; set; }
        public bool MallInput { get; set; }
        public bool McVanInput { get; set; }
        public bool McDeliveryInput { get; set; }
        public bool DriveThruInput { get; set; }
        public bool TakeOutCounterInput { get; set; }
    }
}