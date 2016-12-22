using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public bool UpdateStoreProfile(StoreProfileViewModel SPViewModel)
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
                spRow.BET = BETSelectedToString(SPViewModel);
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
         public bool DeleteStoreProfile(StoreProfileViewModel SPViewModel)
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
         public StoreProfileViewModel SearchStoreProfile(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                if (db.Store_Profile.Where(o => o.STORE_NAME.Equals(SPViewModel.StoreNameNumber)).Any())
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NAME.Equals(SPViewModel.StoreNameNumber));
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber)).Any())
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SPViewModel.StoreNameNumber));
                else
                    return null; // Empty

                SPViewModel.STORE_NO = SPRow.STORE_NO.ToString();
                SPViewModel.STORE_NAME = SPRow.STORE_NAME;
                SPViewModel.OWNERSHIP = SPRow.OWNERSHIP;
                SPViewModel.BREAKFAST_PRICE_TIER = SPRow.BREAKFAST_PRICE_TIER;
                SPViewModel.REGULAR_PRICE_TIER = SPRow.REGULAR_PRICE_TIER;
                SPViewModel.DC_PRICE_TIER = SPRow.DC_PRICE_TIER;
                SPViewModel.MDS_PRICE_TIER = SPRow.MDS_PRICE_TIER;
                SPViewModel.MCCAFE_LEVEL_2_PRICE_TIER = SPRow.MCCAFE_LEVEL_2_PRICE_TIER;
                SPViewModel.MCCAFE_LEVEL_3_PRICE_TIER = SPRow.MCCAFE_LEVEL_3_PRICE_TIER;
                SPViewModel.MCCAFE_BISTRO_PRICE_TIER = SPRow.MCCAFE_BISTRO_PRICE_TIER;
                SPViewModel.PROJECT_GOLD_PRICE_TIER = SPRow.PROJECT_GOLD_PRICE_TIER;
                SPViewModel.PROFIT_CENTER = SPRow.PROFIT_CENTER.ToString();
                SPViewModel.REGION = SPRow.REGION;
                SPViewModel.PROVINCE = SPRow.PROVINCE;
                SPViewModel.LOCATION = SPRow.LOCATION;
                SPViewModel.ADDRESS = SPRow.ADDRESS;
                SPViewModel.CITY = SPRow.CITY;
                SPViewModel.FRESH_OR_FROZEN = SPRow.FRESH_OR_FROZEN;
                SPViewModel.PAPER_OR_PLASTIC = SPRow.PAPER_OR_PLASTIC;
                SPViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                SPViewModel.SIMPLOT_OR_MCCAIN = SPRow.SIMPLOT_OR_MCCAIN;
                SPViewModel.MCCORMICK_OR_GSF = SPRow.MCCORMICK_OR_GSF;
                // Initialize Business Extension Checkboxes
                bool[] bet = BETSelectedToArray(SPRow.BET);
                SPViewModel.Hrs24Input = bet[0];
                SPViewModel.MallInput = bet[1];
                SPViewModel.McVanInput = bet[2];
                SPViewModel.McDeliveryInput = bet[3];
                SPViewModel.DriveThruInput = bet[4];
                SPViewModel.TakeOutCounterInput = bet[5];

                return SPViewModel;
            }
        }
        /*
         * Creates a string list of the selected Business Extension
         */
        private string BETSelectedToString(StoreProfileViewModel SPViewModel)
        {
            return SPViewModel.Hrs24Input + "," +
                   SPViewModel.MallInput + "," +
                   SPViewModel.McVanInput + "," +
                   SPViewModel.McDeliveryInput + "," +
                   SPViewModel.DriveThruInput + "," +
                   SPViewModel.TakeOutCounterInput;
        }
         public bool[] BETSelectedToArray(string st)
        {
            string[] betSt = st.Split(',');
            bool[] bet = new bool[betSt.Length];
            for (int i = 0; i < betSt.Length; i++)
                bet[i] = bool.Parse(betSt[i]);
            return bet;
        }


    }
}