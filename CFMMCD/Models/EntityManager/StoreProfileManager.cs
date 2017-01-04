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
                spRow.BET = SetBusinessExtention(SPViewModel.BET);
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
         public List<StoreProfileViewModel> SearchStore(StoreProfileViewModel SPViewModel)
         {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Store_Profile> SPRowList;
                List<StoreProfileViewModel> SPViewModelList = new List<StoreProfileViewModel>();
                if (db.Store_Profile.Where(o => o.STORE_NAME.Contains(SPViewModel.StoreNameNumber)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NAME.Contains(SPViewModel.StoreNameNumber)).ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber)).ToList();
                else
                    return null; // Empty
                foreach ( Store_Profile sp in SPRowList )
                {
                    StoreProfileViewModel vm = new StoreProfileViewModel();
                    vm.STORE_NO = sp.STORE_NO.ToString().Trim();
                    vm.STORE_NAME = sp.STORE_NAME.Trim();
                    vm.OWNERSHIP = sp.OWNERSHIP.Trim();
                    vm.BREAKFAST_PRICE_TIER = sp.BREAKFAST_PRICE_TIER.Trim();
                    vm.REGULAR_PRICE_TIER = sp.REGULAR_PRICE_TIER.Trim();
                    vm.DC_PRICE_TIER = sp.DC_PRICE_TIER.Trim();
                    vm.MDS_PRICE_TIER = sp.MDS_PRICE_TIER.Trim();
                    vm.MCCAFE_LEVEL_2_PRICE_TIER = sp.MCCAFE_LEVEL_2_PRICE_TIER.Trim();
                    vm.MCCAFE_LEVEL_3_PRICE_TIER = sp.MCCAFE_LEVEL_3_PRICE_TIER.Trim();
                    vm.MCCAFE_BISTRO_PRICE_TIER = sp.MCCAFE_BISTRO_PRICE_TIER.Trim();
                    vm.PROJECT_GOLD_PRICE_TIER = sp.PROJECT_GOLD_PRICE_TIER.Trim();
                    vm.PROFIT_CENTER = sp.PROFIT_CENTER.ToString();
                    vm.REGION = sp.REGION.Trim();
                    vm.PROVINCE = sp.PROVINCE.Trim();
                    vm.LOCATION = sp.LOCATION.Trim();
                    vm.ADDRESS = sp.ADDRESS.Trim();
                    vm.CITY = sp.CITY.Trim();
                    vm.FRESH_OR_FROZEN = sp.FRESH_OR_FROZEN.Trim();
                    vm.PAPER_OR_PLASTIC = sp.PAPER_OR_PLASTIC.Trim();
                    vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX = sp.SOFT_SERVE_OR_VANILLA_POWDER_MIX.Trim();
                    vm.SIMPLOT_OR_MCCAIN = sp.SIMPLOT_OR_MCCAIN.Trim();
                    vm.MCCORMICK_OR_GSF = sp.MCCORMICK_OR_GSF.Trim();
                    vm.BET = GetBusinessExtension(sp.BET);
                    SPViewModelList.Add(vm);
                }
                if (SPViewModelList == null || SPViewModelList.ElementAt(0) == null)
                    return null;
                return SPViewModelList;
            }
        }
        /*
         * Creates a string list of the selected Business Extension
         */
        private List<bool> GetBusinessExtension( string stArr )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                System.Diagnostics.Debug.WriteLine("BET (get):" + stArr);
                int capacity = db.BUSINESS_EXT.Count();
                bool[] BEArr = new bool[capacity];
                string[] initArr = stArr.Split(',');
                for (int i = 0; i < capacity; i++)
                {
                    if (initArr.Contains(i.ToString()))
                        BEArr[i] = true;
                    else
                        BEArr[i] = false;
                }
                return BEArr.ToList();
            }
        }
        private string SetBusinessExtention ( List<bool> boolArr )
        {
            string st = "";
            for (int i = 0; i < boolArr.Count(); i++ )
                if (boolArr[i])
                    st += i + ",";
            System.Diagnostics.Debug.WriteLine("BET (set):" + st);
            if (st.Length <= 0)
                return st;
            return st.Substring(0, st.Length - 1);
        }
    }
}