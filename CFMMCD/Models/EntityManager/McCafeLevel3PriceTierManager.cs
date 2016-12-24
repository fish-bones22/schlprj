using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class McCafeLevel3PriceTierManager
    {
        public List<McCafeLevel3PriceTierViewModel> GetMCL3()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeLevel3PriceTierViewModel> MCL3List = new List<McCafeLevel3PriceTierViewModel>();
                foreach (McCafe_Level_3_Price_Tier mcl3 in db.McCafe_Level_3_Price_Tier)
                {
                    McCafeLevel3PriceTierViewModel MCL3ViewModel = new McCafeLevel3PriceTierViewModel();

                    MCL3ViewModel.Id = (mcl3.Id).ToString();
                    MCL3ViewModel.Price_Tier = mcl3.Price_Tier;
                }
                return MCL3List;
            }
        }

        public bool UpdateMcCafeLevel3PriceTier(McCafeLevel3PriceTierViewModel MCL3ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_3_Price_Tier mcl3Row = new McCafe_Level_3_Price_Tier();

                mcl3Row.Id = int.Parse(MCL3ViewModel.Id);
                mcl3Row.Price_Tier = MCL3ViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Level_3_Price_Tier.Where(o => o.Id.Equals(MCL3ViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Level_3_Price_Tier.Single(o => o.Id.Equals(MCL3ViewModel.Id));
                        db.McCafe_Level_3_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Level_3_Price_Tier.Add(mcl3Row);
                    }
                    else
                        db.McCafe_Level_3_Price_Tier.Add(mcl3Row);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteMcCafeLevel3PriceTier(McCafeLevel3PriceTierViewModel MCL3ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_3_Price_Tier mcl3Row;

                if (db.McCafe_Level_3_Price_Tier.Where(o => o.Id.Equals(MCL3ViewModel.Id)).Any())
                    mcl3Row = db.McCafe_Level_3_Price_Tier.Single(o => o.Id.Equals(MCL3ViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Level_3_Price_Tier.Remove(mcl3Row);
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