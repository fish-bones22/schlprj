using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class TierManager
    {
        /*
         * Returns a list of available Tiers for an MI depending on
         * the `Trading Area` 
         */
        public static List<Tier> SetTierList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Tier> TierList = new List<Tier>();
                List<int> TierIdList = new List<int>();
                List<string> TierNameList = new List<string>();
                List<string> TradingAreaList = new List<string>();

                foreach (var v in db.Breakfast_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("Breakfast ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "Breakfast ";
                    }
                }
                foreach (var v in db.Dessert_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("DessertCenter ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "DessertCenter ";
                    }
                }
                foreach (var v in db.McCafe_Bistro_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("McCafeBistro ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "McCafeBistro ";
                    }
                }
                foreach (var v in db.McCafe_Level_2_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("McCafeLevel2 ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "McCafeLevel2 ";
                    }
                }
                foreach (var v in db.McCafe_Level_3_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("McCafeLevel3 ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "McCafeLevel3 ";
                    }
                }
                foreach (var v in db.MDS_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("MDS ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "MDS ";
                    }
                }
                foreach (var v in db.Project_Gold_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("ProjectGOLD ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "ProjectGOLD ";
                    }
                }
                foreach (var v in db.Regular_Price_Tier)
                {
                    if (!TierNameList.Contains(v.Price_Tier.Trim()))
                    {
                        TierNameList.Add(v.Price_Tier);
                        TierIdList.Add(v.Id);
                        TradingAreaList.Add("Regular ");
                    }
                    else
                    {
                        TradingAreaList[TierNameList.IndexOf(v.Price_Tier.Trim())] += "Regular ";
                    }
                }

                for (int i = 0; i < TierNameList.Count(); i++)
                {
                    Tier t = new Tier();
                    t.TierId = TierIdList[i] ;
                    t.TierName = TierNameList[i];
                    t.TradingAreas = TradingAreaList[i];
                    // OLD
                    t.MIMPRI = ""; // Eat in
                    t.MIMPRO = ""; // Take out
                    t.MIMPRG = ""; // Other
                    t.MIMNPA = ""; // Non-product
                    // New
                    t.MIMNPI = ""; // Eat in new
                    t.MIMNPO = ""; // Take out new
                    t.MIMNPD = ""; // Other new
                    t.MIMNNP = ""; // Non-product new
                    // Effective date
                    t.MIMPND = "";
                    TierList.Add(t);
                }
                TierList.OrderBy(m => m.TierName);
                return TierList;
            }
        }
    }
}