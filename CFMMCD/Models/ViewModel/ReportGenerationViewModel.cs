using CFMMCD.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class ReportGenerationViewModel
    {
        public string DUMMY { get; set; }
        public bool DUMMYb { get; set; }

        public ReportGenerationViewModel()
        {
            TableDropDown tdd = new TableDropDown();
            OwnershipList = tdd.SetOwnershipDropDown();
            LocationList = tdd.SetLocationDropDown();
            ProfitCenter = tdd.SetProfitCenterDropDown();
            BreakfastTier = tdd.SetBreakfastPriceTierDropDown();
            RegularTier = tdd.SetRegularPriceTierDropDown();
            DCTier = tdd.SetDessertPriceTierDropDown();
            MDSTier = tdd.SetMDSPriceTierDropDown();
            McCafeLevel2Tier = tdd.SetMcCafeLevel2PriceTierDropDown();
            McCafeLevel3Tier = tdd.SetMcCafeLevel3PriceTierDropDown();
            McCafeBistroTier = tdd.SetMcCafeBistroPriceTierDropDown();
            ProjectGoldTier = tdd.SetProjectGoldPriceTierDropDown();
            BusinessExtList = tdd.SetBusinessExtensionList();
            StoreList = tdd.SetStoreDropDown();
            BET = new List<bool>();
            foreach (CheckBoxList cbl in BusinessExtList)
                BET.Add(cbl.Cb);
        }
        public string ItemToExport { get; set; }
        public bool MenuItem { get; set; }
        public bool RawItem { get; set; }
        public bool Recipe { get; set; }

        public bool NP3 { get; set; }
        public bool NP6 { get; set; }

        public string DateTo { get; set; }
        public string DateFrom { get; set; }

        public List<GenericDropDownList> StoreList { get; set; }

        public List<bool> BET { get; set; }

        public List<CheckBoxList> BusinessExtList { get; set; }

        public List<GenericDropDownList> OwnershipList { get; set; }
        public List<GenericDropDownList> ProfitCenter { get; set; }

        public List<GenericDropDownList> LocationList { get; set; }

        public List<GenericDropDownList> BreakfastTier { get; set; }
        public List<GenericDropDownList> RegularTier { get; set; }
        public List<GenericDropDownList> DCTier { get; set; }
        public List<GenericDropDownList> MDSTier { get; set; }
        public List<GenericDropDownList> McCafeLevel2Tier { get; set; }
        public List<GenericDropDownList> McCafeLevel3Tier { get; set; }
        public List<GenericDropDownList> McCafeBistroTier { get; set; }
        public List<GenericDropDownList> ProjectGoldTier { get; set; }

        public string BREAKFAST_PRICE_TIER { get; set; }
        public string REGULAR_PRICE_TIER { get; set; }
        public string DC_PRICE_TIER { get; set; }
        public string MDS_PRICE_TIER { get; set; }
        public string MCCAFE_LEVEL_2_PRICE_TIER { get; set; }
        public string MCCAFE_LEVEL_3_PRICE_TIER { get; set; }
        public string MCCAFE_BISTRO_PRICE_TIER { get; set; }
        public string PROJECT_GOLD_PRICE_TIER { get; set; }

        public string PROFIT_CENTER { get; set; }
        public string OWNERSHIP { get; set; }

        public string LOCATION { get; set; }
        public string REGION { get; set; }
        public string PROVINCE { get; set; }
        public string CITY { get; set; }

        public string FRESH_OR_FROZEN { get; set; }
        public string PAPER_OR_PLASTIC { get; set; }
        public string SOFT_SERVE_OR_VANILLA_POWDER_MIX { get; set; }
        public string SIMPLOT_OR_MCCAIN { get; set; }
        public string MCCORMICK_OR_GSF { get; set; }
        public string FRESHB_OR_FROZENB { get; set; }

        public string Store { get; set; }
    }
}