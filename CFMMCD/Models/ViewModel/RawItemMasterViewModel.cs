using CFMMCD.DropDown;
using CFMMCD.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class RawItemMasterViewModel
    {
        public RawItemMasterViewModel()
        {
            TableDropDown tdd = new TableDropDown();
            PrimaryVendorList = tdd.SetPrimaryVendorList();
            StoreList = tdd.SetStoreDropDown();
            LocationList = tdd.SetLocationDropDown();
            VendorList = tdd.SetVendorList();
            UnitOfMeasureList = tdd.SetUnitOfMeasureList();
            MaterialsGroupList = tdd.SetMaterialsGroupList();
            RawItemMasterList = new List<RawItem>();
            MenuItemList = new List<MenuItem>();
            VendorsSelectedList = new List<bool>(VendorList.Count());
            VendorCPR = new List<string>(VendorList.Count());
            VendorPUN = new List<string>(VendorList.Count());
            VendorSCM = new List<string>(VendorList.Count());
            int i = 0;
            foreach (var v in VendorList)
            {
                VendorsSelectedList.Add(v.Cb);
                VendorCPR.Add("");
                VendorPUN.Add("");
                VendorSCM.Add("");
                i++;
            }
        }
        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }
        public string RIMRIC { get; set; }
        public string RIMRID { get; set; }
        public string RIMRIG { get; set; }
        public string RIMPIS { get; set; }
        public string RIMBVP { get; set; }
        public string RIMBZP { get; set; }
        public string RIMUMC { get; set; }
        public string RIMUPC { get; set; }
        public string RIMSUQ { get; set; }
        public string RIMLAY { get; set; }
        public string RIMCPR { get; set; }
        public string RIMCPN { get; set; }
        public string RIMPDT { get; set; }
        public string RIMPVN { get; set; }
        public string RIMCWC { get; set; }
        public string RIMPRO { get; set; }
        public string RIMSE4 { get; set; }
        public string RIMERT { get; set; }
        public string RIMUSF { get; set; }
        public string RIMMSD { get; set; }
        public string RIMMSL { get; set; }
        public string RIMLA1 { get; set; }
        public string RIMLA2 { get; set; }
        public string RIMSTA { get; set; }
        public string RIMEDT { get; set; }
        public string RIMORD { get; set; }
        public string RIMADE { get; set; }
        public string RIMBAR { get; set; }
        public string STATUS { get; set; }

        public string DUMMY { get; set; }
        public string StoreSelected { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public bool SelectAllCb { get; set; }
        public bool SelectExceptCb { get; set; }


        public string FRESH_OR_FROZEN { get; set; }
        public string PAPER_OR_PLASTIC { get; set; }
        public string SOFT_SERVE_OR_VANILLA_POWDER_MIX { get; set; }
        public string SIMPLOT_OR_MCCAIN { get; set; }
        public string MCCORMICK_OR_GSF { get; set; }
        public List<GenericDropDownList> UnitOfMeasureList { get; set; }
        public List<GenericDropDownList> MaterialsGroupList { get; set; }
        public List<GenericDropDownList> PrimaryVendorList { get; set; }
        public List<GenericDropDownList> StoreList { get; set; }
        public List<GenericDropDownList> LocationList { get; set; }
        public string SearchVendor { get; set; }
        public List<RawItem> RawItemMasterList { get; set; }
        public List<bool> VendorsSelectedList { get; set; }
        public List<string> VendorCPR { get; set; }
        public List<string> VendorPUN { get; set; }
        public List<string> VendorSCM { get; set; }
        public List<CheckBoxList> VendorList { get; set; }
        public List<MenuItem> MenuItemList { get; set; }
    }
    public class RawItem
    {
        public string RIMRIC { get; set; }
        public string RIMRID { get; set; }
        public string RIMSTA { get; set; }
    }
}