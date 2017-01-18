using CFMMCD.DropDown;
using CFMMCD.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class MenuItemMasterViewModel
    {
        public MenuItemMasterViewModel() {
            TableDropDown tdd = new TableDropDown();
            LocationList = tdd.SetLocationDropDown();
            StoreList = tdd.SetStoreDropDown();
            MenuItemMasterList = new List<MenuItem>();
            MenuRecipeList = new List<MenuRecipe>();
            TierList = new TierManager().SetTierList();
            TradingAreaList = tdd.SetTradingAreaList();
            CategoryList = tdd.SetCategoryList();
            MIMFGCList = tdd.SetPMGList("PMGFGC");
            MIMHPTList = tdd.SetPMGList("PMGHPT");
            MIMWGRList = tdd.SetPMGList("PMGWGR");
        }

        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }

        // CHMMIMP0 Table Items
        public string MIMMIC { get; set; }
        public string MIMSTA { get; set; }
        public string MIMFGC { get; set; }
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

        // Misc
        public string Trading_Area { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Store { get; set; }
        public bool SelectAllCb { get; set; }
        public bool SelectExcept { get; set; }

        public List<MenuItem> MenuItemMasterList { get; set; }

        public List<GenericDropDownList> TradingAreaList { get; set; }
        public List<GenericDropDownList> CategoryList { get; set; }
        public List<GenericDropDownList> MIMFGCList { get; set; }
        public List<GenericDropDownList> MIMHPTList { get; set; }
        public List<GenericDropDownList> MIMWGRList { get; set; }
        public List<GenericDropDownList> StoreList { get; set; }
        public List<GenericDropDownList> LocationList { get; set; }
        public List<MenuRecipe> MenuRecipeList { get; set; }
        public List<Tier> TierList { get; set; }
    }

    public class Tier
    {
        public string MIMMIC { get; set; }
        public string MIMNAM { get; set; }

        public int TierId { get; set; }
        public string TierName { get; set; }
        public string TradingAreas { get; set; }
        // OLD
        public string MIMPRI { get; set; } // Eat in
        public string MIMPRO { get; set; } // Take out
        public string MIMPRG { get; set; } // Other
        public string MIMNPA { get; set; } // Non-product
        // New
        public string MIMNPI { get; set; } // Eat in new
        public string MIMNPO { get; set; } // Take out new
        public string MIMNPD { get; set; } // Other new
        public string MIMNNP { get; set; } // Non-product new
        // Effective date
        public string MIMPND { get; set; }
    }

    public class MenuItem
    {
        public string MIMMIC { get; set; }
        public string MIMLON { get; set; }
        public string MIMDSC { get; set; }
        public string MIMSTA { get; set; }
    }

    public class MenuItemPriceUpdateViewModel
    {
        public MenuItemPriceUpdateViewModel()
        {
            TierUpdateList = new List<TierUpdate>();
        }
        public List<TierUpdate> TierUpdateList { get; set; }
    }

    public class TierUpdate
    {
        public string MIMMIC { get; set; }
        public string MIMNAM { get; set; }
        public string MIMSTA { get; set; }
        public string TierAOld { get; set; }
        public string TierANew { get; set; }
        public string TierBOld { get; set; }
        public string TierBNew { get; set; }
        public string TierCOld { get; set; }
        public string TierCNew { get; set; }
        public string EffectiveDate { get; set; }
    }
}