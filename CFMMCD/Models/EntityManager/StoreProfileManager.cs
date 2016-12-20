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
         * Creates a new `Store Profile` row and inserts it in the table
         */
        public void CreateStoreProfile(StoreProfileViewModel StoreProfile)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile spRow = new Store_Profile();

                spRow.STORE_NO = StoreProfile.STORE_NO;
                spRow.STORE_NAME = StoreProfile.STORE_NAME;
                spRow.OWNERSHIP = StoreProfile.OWNERSHIP;
                spRow.BREAKFAST_PRICE_TIER = StoreProfile.BREAKFAST_PRICE_TIER;
                spRow.REGULAR_PRICE_TIER = StoreProfile.REGULAR_PRICE_TIER;
                spRow.DC_PRICE_TIER = StoreProfile.DC_PRICE_TIER;
                spRow.MDS_PRICE_TIER = StoreProfile.MDS_PRICE_TIER;
                spRow.MCCAFE_LEVEL_2_PRICE_TIER = StoreProfile.MCCAFE_LEVEL_2_PRICE_TIER;
                spRow.MCCAFE_LEVEL_3_PRICE_TIER = StoreProfile.MCCAFE_LEVEL_3_PRICE_TIER;
                spRow.MCCAFE_BISTRO_PRICE_TIER = StoreProfile.MCCAFE_BISTRO_PRICE_TIER;
                spRow.PROJECT_GOLD_PRICE_TIER = StoreProfile.PROJECT_GOLD_PRICE_TIER;
                spRow.BET = BetSelected(StoreProfile);
                spRow.PROFIT_CENTER = StoreProfile.PROFIT_CENTER;
                spRow.REGION = StoreProfile.REGION;
                spRow.PROVINCE = StoreProfile.PROVINCE;
                spRow.LOCATION = StoreProfile.LOCATION;
                spRow.ADDRESS = StoreProfile.ADDRESS;
                spRow.CITY = StoreProfile.CITY;
                spRow.FRESH_OR_FROZEN = StoreProfile.FRESH_OR_FROZEN;
                spRow.PAPER_OR_PLASTIC = StoreProfile.PAPER_OR_PLASTIC;
                spRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = StoreProfile.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                spRow.SIMPLOT_OR_MCCAIN = StoreProfile.SIMPLOT_OR_MCCAIN;
                spRow.MCCORMICK_OR_GSF = StoreProfile.MCCORMICK_OR_GSF;

                db.Store_Profile.Add(spRow);
                db.SaveChanges();
            }
        }
        /*
         * Deletes a row in the `Store Profile` table 
         */
         public void DeleteStoreProfile(StoreProfileViewModel StoreProfile)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile StoreProf;
                if (db.Store_Profile.Where(o => o.STORE_NAME.Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NAME == StoreProfile.StoreNameNumber);
                }
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber));
                }
                else
                    return;

                db.Store_Profile.Remove(StoreProf);
                db.SaveChanges();
            }
        }
        /*
         * Updates a row in the `Store Profile` table
         */
         public void UpdateStoreProfile(StoreProfileViewModel StoreProfile)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile StoreProf;
                if (db.Store_Profile.Where(o => o.STORE_NAME.Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NAME == StoreProfile.StoreNameNumber);
                }
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber));
                }
                else
                    return;

                StoreProf.STORE_NO = StoreProfile.STORE_NO;
                StoreProf.STORE_NAME = StoreProfile.STORE_NAME;
                StoreProf.OWNERSHIP = StoreProfile.OWNERSHIP;
                StoreProf.BREAKFAST_PRICE_TIER = StoreProfile.BREAKFAST_PRICE_TIER;
                StoreProf.REGULAR_PRICE_TIER = StoreProfile.REGULAR_PRICE_TIER;
                StoreProf.DC_PRICE_TIER = StoreProfile.DC_PRICE_TIER;
                StoreProf.MDS_PRICE_TIER = StoreProfile.MDS_PRICE_TIER;
                StoreProf.MCCAFE_LEVEL_2_PRICE_TIER = StoreProfile.MCCAFE_LEVEL_2_PRICE_TIER;
                StoreProf.MCCAFE_LEVEL_3_PRICE_TIER = StoreProfile.MCCAFE_LEVEL_3_PRICE_TIER;
                StoreProf.MCCAFE_BISTRO_PRICE_TIER = StoreProfile.MCCAFE_BISTRO_PRICE_TIER;
                StoreProf.PROJECT_GOLD_PRICE_TIER = StoreProfile.PROJECT_GOLD_PRICE_TIER;
                StoreProf.BET = BetSelected(StoreProfile);
                StoreProf.PROFIT_CENTER = StoreProfile.PROFIT_CENTER;
                StoreProf.REGION = StoreProfile.REGION;
                StoreProf.PROVINCE = StoreProfile.PROVINCE;
                StoreProf.LOCATION = StoreProfile.LOCATION;
                StoreProf.ADDRESS = StoreProfile.ADDRESS;
                StoreProf.CITY = StoreProfile.CITY;
                StoreProf.FRESH_OR_FROZEN = StoreProfile.FRESH_OR_FROZEN;
                StoreProf.PAPER_OR_PLASTIC = StoreProfile.PAPER_OR_PLASTIC;
                StoreProf.SOFT_SERVE_OR_VANILLA_POWDER_MIX = StoreProfile.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                StoreProf.SIMPLOT_OR_MCCAIN = StoreProfile.SIMPLOT_OR_MCCAIN;
                StoreProf.MCCORMICK_OR_GSF = StoreProfile.MCCORMICK_OR_GSF;

                db.SaveChanges();
            }
        }
        /*
         * Searches for the StoreNumber/StoreName from the table
         */
         public StoreProfileViewModel SearchStoreProfile(StoreProfileViewModel StoreProfile)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile StoreProf;
                if (db.Store_Profile.Where(o => o.STORE_NAME.Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NAME == StoreProfile.StoreNameNumber);
                }
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber)).Any())
                {
                    StoreProf = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(StoreProfile.StoreNameNumber));
                }
                else
                    return StoreProfile;

                StoreProfile.STORE_NO = StoreProf.STORE_NO;
                StoreProfile.STORE_NAME = StoreProf.STORE_NAME;
                StoreProfile.OWNERSHIP = StoreProf.OWNERSHIP;
                StoreProfile.BREAKFAST_PRICE_TIER = StoreProf.BREAKFAST_PRICE_TIER;
                StoreProfile.REGULAR_PRICE_TIER = StoreProf.REGULAR_PRICE_TIER;
                StoreProfile.DC_PRICE_TIER = StoreProf.DC_PRICE_TIER;
                StoreProfile.MDS_PRICE_TIER = StoreProf.MDS_PRICE_TIER;
                StoreProfile.MCCAFE_LEVEL_2_PRICE_TIER = StoreProf.MCCAFE_LEVEL_2_PRICE_TIER;
                StoreProfile.MCCAFE_LEVEL_3_PRICE_TIER = StoreProf.MCCAFE_LEVEL_3_PRICE_TIER;
                StoreProfile.MCCAFE_BISTRO_PRICE_TIER = StoreProf.MCCAFE_BISTRO_PRICE_TIER;
                StoreProfile.PROJECT_GOLD_PRICE_TIER = StoreProf.PROJECT_GOLD_PRICE_TIER;
                StoreProfile.PROFIT_CENTER = StoreProf.PROFIT_CENTER;
                StoreProfile.REGION = StoreProf.REGION;
                StoreProfile.PROVINCE = StoreProf.PROVINCE;
                StoreProfile.LOCATION = StoreProf.LOCATION;
                StoreProfile.ADDRESS = StoreProf.ADDRESS;
                StoreProfile.CITY = StoreProf.CITY;
                StoreProfile.FRESH_OR_FROZEN = StoreProf.FRESH_OR_FROZEN;
                StoreProfile.PAPER_OR_PLASTIC = StoreProf.PAPER_OR_PLASTIC;
                StoreProfile.SOFT_SERVE_OR_VANILLA_POWDER_MIX = StoreProf.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                StoreProfile.SIMPLOT_OR_MCCAIN = StoreProf.SIMPLOT_OR_MCCAIN;
                StoreProfile.MCCORMICK_OR_GSF = StoreProf.MCCORMICK_OR_GSF;

                return StoreProfile;
            }
        }
        /*
         * Creates a string list of the selected Business Extension
         */
         private string BetSelected(StoreProfileViewModel StoreProfile)
        {
            return StoreProfile.Hrs24Input + "," +
                   StoreProfile.MallInput + "," +
                   StoreProfile.McVanInput + "," +
                   StoreProfile.McDeliveryInput + "," +
                   StoreProfile.DriveThruInput + "," +
                   StoreProfile.TakeOutCounterInput;
        }
        /*
         * 
         */
         public bool[] BetSelectedArray(string betselect)
        {
            //using (CFMMCDEntities db = new CFMMCDEntities())
            //{
                bool[] betsel = new bool[3];
            //    var bet = db.Store_Profile.Where(o => o.)
            //}

            return betsel ;
        }


    }
}