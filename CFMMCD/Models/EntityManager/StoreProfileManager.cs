using CFMMCD.DropDown;
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
                Store_Profile SPRow;
                bool isUpdating = false;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO)).Any())
                {
                    SPRow = db.Store_Profile.Single(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));
                    isUpdating = true;
                }
                else
                    SPRow = new Store_Profile();

                if (SPViewModel.STORE_NO != null && !SPViewModel.STORE_NO.Equals(""))
                    SPRow.STORE_NO = int.Parse(SPViewModel.STORE_NO);
                else return false;

                if (SPViewModel.STORE_NAME != null && !SPViewModel.STORE_NAME.Equals(""))
                    SPRow.STORE_NAME = SPViewModel.STORE_NAME;
                else return false;

                SPRow.OWNERSHIP = int.Parse(SPViewModel.OWNERSHIP);
                SPRow.BREAKFAST_PRICE_TIER = int.Parse(SPViewModel.BREAKFAST_PRICE_TIER);
                SPRow.REGULAR_PRICE_TIER = int.Parse(SPViewModel.REGULAR_PRICE_TIER);
                SPRow.DC_PRICE_TIER = int.Parse(SPViewModel.DC_PRICE_TIER);
                SPRow.MDS_PRICE_TIER = int.Parse(SPViewModel.MDS_PRICE_TIER);
                SPRow.MCCAFE_LEVEL_2_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_LEVEL_2_PRICE_TIER);
                SPRow.MCCAFE_LEVEL_3_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_LEVEL_3_PRICE_TIER);
                SPRow.MCCAFE_BISTRO_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_BISTRO_PRICE_TIER);
                SPRow.PROJECT_GOLD_PRICE_TIER = int.Parse(SPViewModel.PROJECT_GOLD_PRICE_TIER);
                SPRow.BET = SetBusinessExtention(SPViewModel.BET, SPViewModel.BusinessExtList);
                SPRow.PROFIT_CENTER = int.Parse(SPViewModel.PROFIT_CENTER);
                SPRow.REGION = SPViewModel.REGION;
                SPRow.PROVINCE = SPViewModel.PROVINCE;
                SPRow.LOCATION = int.Parse(SPViewModel.LOCATION);
                SPRow.ADDRESS = SPViewModel.ADDRESS;
                SPRow.CITY = SPViewModel.CITY;
                SPRow.FRESH_OR_FROZEN = SPViewModel.FRESH_OR_FROZEN;
                SPRow.PAPER_OR_PLASTIC = SPViewModel.PAPER_OR_PLASTIC;
                SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                SPRow.SIMPLOT_OR_MCCAIN = SPViewModel.SIMPLOT_OR_MCCAIN;
                SPRow.MCCORMICK_OR_GSF = SPViewModel.MCCORMICK_OR_GSF;
                SPRow.FRESHB_OR_FROZENB = SPViewModel.FRESHB_OR_FROZENB;
                SPRow.STATUS = "A";
                try
                {
                    // Check if STORE_NO already exists in the database, perform an update if true
                    if (isUpdating)
                    {
                        SPRow.STATUS = "E";
                    }
                    else
                        db.Store_Profile.Add(SPRow);
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
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
                {
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));

                    // Remove from Menu Items
                    //      Remove from `Store`
                    if (db.CSHMIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<CSHMIMP0> MIRow = db.CSHMIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.CSHMIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<CSHMIMP0> MIRow = db.CSHMIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    // Remove from Raw Items
                    //      Remove from `Store`
                    if (db.INVRIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVRIMP0> MIRow = db.INVRIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.INVRIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVRIMP0> MIRow = db.INVRIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    // Remove from Vendors
                    //      Remove from `Store`
                    if (db.INVVEMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVVEMP0> MIRow = db.INVVEMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.INVVEMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVVEMP0> MIRow = db.INVVEMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                }
                else
                    return false;

                try
                {
                    db.Store_Profile.Remove(SPRow);
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    Exception f = e.InnerException;
                    while (f != null)
                    {
                        System.Diagnostics.Debug.WriteLine("INNER:");
                        System.Diagnostics.Debug.WriteLine(f.Message);
                        System.Diagnostics.Debug.WriteLine(f.Source);
                        f = f.InnerException;
                    }
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        }
        /*
         * Searches for the StoreNumber/StoreName from the table
         */
         public List<Store> SearchStores(string SearchItem)
         {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Store_Profile> SPRowList;
                List<Store> SPList = new List<Store>();
                if (SearchItem.ToUpper().Equals("ALL"))
                    SPRowList = db.Store_Profile.ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NAME.Contains(SearchItem)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NAME.Contains(SearchItem)).ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SearchItem)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NO.ToString().Equals(SearchItem)).ToList();
                else
                    return null; // Empty
                foreach ( Store_Profile sp in SPRowList )
                {
                    Store st = new Store();
                    st.Store_No = sp.STORE_NO.ToString();
                    st.Store_Name = sp.STORE_NAME;
                    SPList.Add(st);
                }
                if (SPList == null || SPList.ElementAt(0) == null)
                    return null;
                return SPList;
            }
        }

        public StoreProfileViewModel SearchSingleStore (string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SearchItem)).Any())
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SearchItem));
                else return null;

                StoreProfileViewModel vm = new StoreProfileViewModel();
                vm.STORE_NO = SPRow.STORE_NO.ToString().Trim();
                vm.STORE_NAME = SPRow.STORE_NAME.Trim();
                vm.OWNERSHIP = SPRow.OWNERSHIP.ToString();
                vm.BREAKFAST_PRICE_TIER = SPRow.BREAKFAST_PRICE_TIER.ToString();
                vm.REGULAR_PRICE_TIER = SPRow.REGULAR_PRICE_TIER.ToString();
                vm.DC_PRICE_TIER = SPRow.DC_PRICE_TIER.ToString();
                vm.MDS_PRICE_TIER = SPRow.MDS_PRICE_TIER.ToString();
                vm.MCCAFE_LEVEL_2_PRICE_TIER = SPRow.MCCAFE_LEVEL_2_PRICE_TIER.ToString();
                vm.MCCAFE_LEVEL_3_PRICE_TIER = SPRow.MCCAFE_LEVEL_3_PRICE_TIER.ToString();
                vm.MCCAFE_BISTRO_PRICE_TIER = SPRow.MCCAFE_BISTRO_PRICE_TIER.ToString();
                vm.PROJECT_GOLD_PRICE_TIER = SPRow.PROJECT_GOLD_PRICE_TIER.ToString();
                vm.PROFIT_CENTER = SPRow.PROFIT_CENTER.ToString();
                vm.REGION = SPRow.REGION.Trim();
                vm.PROVINCE = SPRow.PROVINCE.Trim();
                vm.LOCATION = SPRow.LOCATION.ToString();
                vm.ADDRESS = SPRow.ADDRESS.Trim();
                vm.CITY = SPRow.CITY.Trim();
                vm.FRESH_OR_FROZEN = SPRow.FRESH_OR_FROZEN.Trim();
                vm.PAPER_OR_PLASTIC = SPRow.PAPER_OR_PLASTIC.Trim();
                vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX.Trim();
                vm.SIMPLOT_OR_MCCAIN = SPRow.SIMPLOT_OR_MCCAIN.Trim();
                vm.MCCORMICK_OR_GSF = SPRow.MCCORMICK_OR_GSF.Trim();
                vm.FRESHB_OR_FROZENB = SPRow.FRESHB_OR_FROZENB;
                vm.BET = GetBusinessExtension(SPRow.BET, vm.BusinessExtList);
                return vm;
            }
        }
        /*
         * Creates a List of bool from the string of 
         * selected Business Extensions
         * to be used in the View.
         */
        private List<bool> GetBusinessExtension ( string stArr, List<CheckBoxList> lookUpList )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                int capacity = lookUpList.Count();
                bool[] BEArr = new bool[capacity];
                string[] initArr = stArr.Split(',');
                for (int i = 0; i < capacity; i++)
                {
                    int index = int.Parse(lookUpList[i].value);
                    if (initArr.Contains(lookUpList[i].value))
                    {
                        BEArr[i] = true;
                    } else BEArr[i] = false;
                }
                return BEArr.ToList();
            }
        }
        /*
         * Creates a string of bool delimited by ',' 
         * that contains the selected Business Extensions
         * to be inserted in DB
         */
        private string SetBusinessExtention ( List<bool> boolArr, List<CheckBoxList> lookUpList )
        {
            string st = "";
            for (int i = 0; i < boolArr.Count(); i++ )
                if (boolArr[i])
                    st += lookUpList[i].value + ",";
            System.Diagnostics.Debug.WriteLine("BET (set):" + st);
            if (st.Length <= 0)
                return st;
            return st.Substring(0, st.Length - 1);
        }
    }
}