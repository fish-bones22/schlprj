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
        public List<McCafeLevel2PriceTierViewModel> GetML2()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeLevel2PriceTierViewModel> ML2List = new List<McCafeLevel2PriceTierViewModel>();
                foreach (McCafe_Level_2_Price_Tier ml2 in db.McCafe_Level_2_Price_Tier)
                {
                    McCafeLevel2PriceTierViewModel ML2ViewModel = new McCafeLevel2PriceTierViewModel();
                    ML2ViewModel.Id = (ml2.Id).ToString();
                    ML2ViewModel.Price_Tier = ml2.Price_Tier;
                    // Add to List
                    ML2List.Add(ML2ViewModel);
                }
                return ML2List;
            }
        }

        public bool UpdateMcCafeLevel2PriceTier(McCafeLevel2PriceTierViewModel ML2ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_2_Price_Tier ml2Row = new McCafe_Level_2_Price_Tier();
                ml2Row.Id = int.Parse(ML2ViewModel.Id);
                ml2Row.Price_Tier = ML2ViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Level_2_Price_Tier.Where(o => o.Id.ToString().Equals(ML2ViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Level_2_Price_Tier.Single(o => o.Id.ToString().Equals(ML2ViewModel.Id));
                        db.McCafe_Level_2_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Level_2_Price_Tier.Add(ml2Row);
                    }
                    else
                        db.McCafe_Level_2_Price_Tier.Add(ml2Row);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteMcCafeLevel2PriceTier(McCafeLevel2PriceTierViewModel ML2ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_2_Price_Tier ml2Row;

                if (db.McCafe_Level_2_Price_Tier.Where(o => o.Id.ToString().Equals(ML2ViewModel.Id)).Any())
                    ml2Row = db.McCafe_Level_2_Price_Tier.Single(o => o.Id.ToString().Equals(ML2ViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Level_2_Price_Tier.Remove(ml2Row);
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