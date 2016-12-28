﻿using CFMMCD.DropDown;
using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CFMMCD.Models.ViewModel.StoreProfileViewModel;

namespace CFMMCD.Models.EntityManager
{
    public class StoreProfileManager
    {
        /*
         * Combined Create and Update Store profile method.
         * Creates a Store_Profile instance (which will be a new table row)
         * and instantiates each SPViewModel property to the respective property of the former.
         * Also checks if the given Store profile number is already in the table,
         * if true, the method performs an update, otherwise, creation. 
         *
         * Returns true if the operation is successful.
         * */
        public bool UpdateStore(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile spRow = new Store_Profile();
                spRow.STORE_NO = int.Parse(SPViewModel.STORE_NO);
                spRow.STORE_NAME = SPViewModel.STORE_NAME;
                spRow.OWNERSHIP = SPViewModel.OWNERSHIP;
                spRow.BREAKFAST_PRICE_TIER = SPViewModel.BREAKFAST_PRICE_TIER;
                spRow.REGULAR_PRICE_TIER = SPViewModel.REGULAR_PRICE_TIER;
                spRow.DC_PRICE_TIER = SPViewModel.DC_PRICE_TIER;
                spRow.MDS_PRICE_TIER = SPViewModel.MDS_PRICE_TIER;
                spRow.MCCAFE_LEVEL_2_PRICE_TIER = SPViewModel.MCCAFE_LEVEL_2_PRICE_TIER;
                spRow.MCCAFE_LEVEL_3_PRICE_TIER = SPViewModel.MCCAFE_LEVEL_3_PRICE_TIER;
                spRow.MCCAFE_BISTRO_PRICE_TIER = SPViewModel.MCCAFE_BISTRO_PRICE_TIER;
                spRow.PROJECT_GOLD_PRICE_TIER = SPViewModel.PROJECT_GOLD_PRICE_TIER;
                spRow.BET = SetBusinessExtention(SPViewModel.BusinessExtList);
                spRow.PROFIT_CENTER = int.Parse(SPViewModel.PROFIT_CENTER);
                spRow.REGION = SPViewModel.REGION;
                spRow.PROVINCE = SPViewModel.PROVINCE;
                spRow.LOCATION = SPViewModel.LOCATION;
                spRow.ADDRESS = SPViewModel.ADDRESS;
                spRow.CITY = SPViewModel.CITY;
                spRow.FRESH_OR_FROZEN = SPViewModel.FRESH_OR_FROZEN;
                spRow.PAPER_OR_PLASTIC = SPViewModel.PAPER_OR_PLASTIC;
                spRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                spRow.SIMPLOT_OR_MCCAIN = SPViewModel.SIMPLOT_OR_MCCAIN;
                spRow.MCCORMICK_OR_GSF = SPViewModel.MCCORMICK_OR_GSF;
                spRow.STATUS = "A";
                try
                {
                    // Check if STORE_NO already exists in the database, perform an update if true
                    if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO)).Any())
                    {
                        var rowToRemove = db.Store_Profile.Single(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));
                        spRow.STATUS = "E";
                        db.Store_Profile.Remove(rowToRemove); // Remove old row      
                        db.Store_Profile.Add(spRow);          // Insert replacement
                    }
                    else
                        db.Store_Profile.Add(spRow);
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
                {
                    return false;
                }
                
            }
        }
        /*
         * Deletes a row in the `Store Profile` table 
         * 
         * Returns true if operation is successful.
         */
         public bool DeleteStore(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO)).Any())
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));
                else
                    return false;
                try
                {
                    db.Store_Profile.Remove(SPRow);
                    db.SaveChanges();
                    return true;
                } catch ( Exception e )
                {
                    return false;
                }
            }
        }
        /*
         * Searches for the StoreNumber/StoreName from the table
         */
         public List<StoreProfileViewModel> SearchStore(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Store_Profile> SPRowList;
                List<StoreProfileViewModel> SPViewModelList = new List<StoreProfileViewModel>();
                if (db.Store_Profile.Where(o => o.STORE_NAME.Equals(SPViewModel.StoreNameNumber)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NAME.Contains(SPViewModel.StoreNameNumber)).ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber)).ToList();
                else
                    return null; // Empty
                foreach ( Store_Profile sp in SPRowList )
                {
                    StoreProfileViewModel vm = new StoreProfileViewModel();
                    vm.STORE_NO = sp.STORE_NO.ToString();
                    vm.STORE_NAME = sp.STORE_NAME;
                    vm.OWNERSHIP = sp.OWNERSHIP;
                    vm.BREAKFAST_PRICE_TIER = sp.BREAKFAST_PRICE_TIER;
                    vm.REGULAR_PRICE_TIER = sp.REGULAR_PRICE_TIER;
                    vm.DC_PRICE_TIER = sp.DC_PRICE_TIER;
                    vm.MDS_PRICE_TIER = sp.MDS_PRICE_TIER;
                    vm.MCCAFE_LEVEL_2_PRICE_TIER = sp.MCCAFE_LEVEL_2_PRICE_TIER;
                    vm.MCCAFE_LEVEL_3_PRICE_TIER = sp.MCCAFE_LEVEL_3_PRICE_TIER;
                    vm.MCCAFE_BISTRO_PRICE_TIER = sp.MCCAFE_BISTRO_PRICE_TIER;
                    vm.PROJECT_GOLD_PRICE_TIER = sp.PROJECT_GOLD_PRICE_TIER;
                    vm.PROFIT_CENTER = sp.PROFIT_CENTER.ToString();
                    vm.REGION = sp.REGION;
                    vm.PROVINCE = sp.PROVINCE;
                    vm.LOCATION = sp.LOCATION;
                    vm.ADDRESS = sp.ADDRESS;
                    vm.CITY = sp.CITY;
                    vm.FRESH_OR_FROZEN = sp.FRESH_OR_FROZEN;
                    vm.PAPER_OR_PLASTIC = sp.PAPER_OR_PLASTIC;
                    vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX = sp.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                    vm.SIMPLOT_OR_MCCAIN = sp.SIMPLOT_OR_MCCAIN;
                    vm.MCCORMICK_OR_GSF = sp.MCCORMICK_OR_GSF;
                    vm.BusinessExtList = GetBusinessExtension(sp.BET);
                    SPViewModelList.Add(vm);
                }
                if (SPViewModelList == null || SPViewModelList.ElementAt(0) == null)
                    return null;
                return SPViewModelList;
            }
        }
        public StoreProfileViewModel InitializeDropDowns(StoreProfileViewModel SPViewModel)
        {
            TableDropDown tdd = new TableDropDown();
            SPViewModel.RegularTier = tdd.SetRegularPriceTierDropDown();
            SPViewModel.BreakfastTier = tdd.SetBreakfastPriceTierDropDown();
            SPViewModel.DCTier = tdd.SetDessertPriceTierDropDown();
            SPViewModel.McCafeBistroTier = tdd.SetMcCafeBistroPriceTierDropDown();
            SPViewModel.McCafeLevel2Tier = tdd.SetMcCafeLevel2PriceTierDropDown();
            SPViewModel.McCafeLevel3Tier = tdd.SetMcCafeLevel3PriceTierDropDown();
            SPViewModel.ProjectGoldTier = tdd.SetProjectGoldPriceTierDropDown();
            SPViewModel.MDSTier = tdd.SetMDSPriceTierDropDown();

            SPViewModel.OwnershipList = tdd.SetOwnershipDropDown();
            SPViewModel.ProfitCenter = tdd.SetProfitCenterDropDown();
            SPViewModel.BusinessExtList = SetBusinessExtension();
            SPViewModel.LocationList = tdd.SetLocationDropDown();
            return SPViewModel;
        }
        /*
         * Creates a string list of the selected Business Extension
         */
        private List<CheckBoxList> GetBusinessExtension( string stArr )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                int capacity = db.BUSINESS_EXT.ToArray().Length;
                List<BUSINESS_EXT> BERowList = db.BUSINESS_EXT.ToList();
                List<CheckBoxList> BEList = new List<CheckBoxList>();
                string[] initArr = stArr.Split(',');
                for (int i = 0; i < capacity; i++)
                {
                    CheckBoxList BE = new CheckBoxList();
                    BE.value = BERowList[i].ID.ToString();
                    BE.text = BERowList[i].LONGNM.Trim();
                    if (initArr.Contains(i.ToString()))
                        BE.Cb = true;
                    else
                        BE.Cb = true;
                    BEList.Add(BE);
                }
                return BEList;
            }
        }
        private string SetBusinessExtention ( List<CheckBoxList> list )
        {
            string st = "";
            int i = 0;
            foreach ( CheckBoxList be in list )
            {
                if (be.Cb)
                    st += i + ",";
                i++;
            }
            if (st.Length <= 0)
                return st;
            return st.Substring(0, st.Length - 1);
        }
        public List<CheckBoxList> SetBusinessExtension()
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<CheckBoxList> list = new List<CheckBoxList>();
                foreach ( var i in db.BUSINESS_EXT )
                {
                    CheckBoxList cbList = new CheckBoxList();
                    cbList.Cb = false;
                    cbList.value = i.ID.ToString();
                    cbList.text = i.LONGNM.Trim();
                    list.Add(cbList);
                }
                return list;
            }
        }

    }
}