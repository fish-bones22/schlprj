using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CFMMCD.Sessions;
using CFMMCD.DropDown;

namespace CFMMCD.Models.ViewModel
{
    public class StoreProfileViewModel
    {
        public string StoreNameNumber { get; set; }
        [Required(ErrorMessage ="This field is required")]
        public string STORE_NO { get; set; }
        [Required(ErrorMessage = "This field is required")]
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
        public string PROFIT_CENTER { get; set; }
        public string REGION { get; set; }
        public string PROVINCE { get; set; }
        public string LOCATION { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string FRESH_OR_FROZEN { get; set; }
        public string PAPER_OR_PLASTIC { get; set; }
        public string SOFT_SERVE_OR_VANILLA_POWDER_MIX { get; set; }
        public string SIMPLOT_OR_MCCAIN { get; set; }
        public string MCCORMICK_OR_GSF { get; set; }

        public bool[] BET { get; set; }

        public List<StoreProfileViewModel> StoreList { get; set; }
        public List<CheckBoxList> BusinessExtList { get; set; }
        public List<GenericDropDownList> OwnershipList { get; set; }
        public List<GenericDropDownList> LocationList { get; set; }
        public List<GenericDropDownList> ProfitCenter { get; set; }
        public List<GenericDropDownList> BreakfastTier { get; set; }
        public List<GenericDropDownList> RegularTier { get; set; }
        public List<GenericDropDownList> DCTier { get; set; }
        public List<GenericDropDownList> MDSTier { get; set; }
        public List<GenericDropDownList> McCafeLevel2Tier { get; set; }
        public List<GenericDropDownList> McCafeLevel3Tier { get; set; }
        public List<GenericDropDownList> McCafeBistroTier { get; set; }
        public List<GenericDropDownList> ProjectGoldTier { get; set; }


    }
    public class CheckBoxList
    {
        public bool Cb { get; set; }
        public string value { get; set; }
        public string text { get; set; }
    }
}