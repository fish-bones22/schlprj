using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class McCafeLevel2PriceTierManager
    {
        public List<McCafeLevel2PriceTierViewModel> GetMCL2()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeLevel2PriceTierViewModel> MCL2List = new List<McCafeLevel2PriceTierViewModel>();
                foreach (McCafe_Level_2_Price_Tier mcl2 in db.McCafe_Level_2_Price_Tier)
                {
                    McCafeLevel2PriceTierViewModel MCL2ViewModel = new McCafeLevel2PriceTierViewModel();

                    MCL2ViewModel.Id = (mcl2.Id).ToString();
                    MCL2ViewModel.Price_Tier = mcl2.Price_Tier;
                }
                return MCL2List;
            }
        }

        public bool UpdateMcCafeLevel2PriceTier(McCafeLevel2PriceTierViewModel MCL2ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_2_Price_Tier mcl2Row = new McCafe_Level_2_Price_Tier();

                mcl2Row.Id = int.Parse(MCL2ViewModel.Id);
                mcl2Row.Price_Tier = MCL2ViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Level_2_Price_Tier.Where(o => o.Id.Equals(MCL2ViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Level_2_Price_Tier.Single(o => o.Id.Equals(MCL2ViewModel.Id));
                        db.McCafe_Level_2_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Level_2_Price_Tier.Add(mcl2Row);
                    }
                    else
                        db.McCafe_Level_2_Price_Tier.Add(mcl2Row);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteMcCafeLevel2PriceTier(McCafeLevel2PriceTierViewModel MCL2ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_2_Price_Tier mcl2Row;

                if (db.McCafe_Level_2_Price_Tier.Where(o => o.Id.Equals(MCL2ViewModel.Id)).Any())
                    mcl2Row = db.McCafe_Level_2_Price_Tier.Single(o => o.Id.Equals(MCL2ViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Level_2_Price_Tier.Remove(mcl2Row);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}