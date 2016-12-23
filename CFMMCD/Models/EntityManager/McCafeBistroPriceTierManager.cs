using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class McCafeBistroPriceTierManager
    {
        public List<McCafeBistroPriceTierViewModel> GetMCB()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeBistroPriceTierViewModel> MCBList = new List<McCafeBistroPriceTierViewModel>();
                foreach (McCafe_Bistro_Price_Tier mcb in db.McCafe_Bistro_Price_Tier)
                {
                    McCafeBistroPriceTierViewModel MCBViewModel = new McCafeBistroPriceTierViewModel();

                    MCBViewModel.Id = (mcb.Id).ToString();
                    MCBViewModel.Price_Tier = mcb.Price_Tier;
                }
                return MCBList;
            }
        }

        public bool UpdateMcCafeBistroPriceTier(McCafeBistroPriceTierViewModel MCBViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Bistro_Price_Tier mcbRow = new McCafe_Bistro_Price_Tier();

                mcbRow.Id = int.Parse(MCBViewModel.Id);
                mcbRow.Price_Tier = MCBViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Bistro_Price_Tier.Where(o => o.Id.Equals(MCBViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Bistro_Price_Tier.Single(o => o.Id.Equals(MCBViewModel.Id));
                        db.McCafe_Bistro_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Bistro_Price_Tier.Add(mcbRow);
                    }
                    else
                        db.McCafe_Bistro_Price_Tier.Add(mcbRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteMcCafeBistroPriceTier(McCafeBistroPriceTierViewModel MCBViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Bistro_Price_Tier mcbRow;

                if (db.McCafe_Bistro_Price_Tier.Where(o => o.Id.Equals(MCBViewModel.Id)).Any())
                    mcbRow = db.McCafe_Bistro_Price_Tier.Single(o => o.Id.Equals(MCBViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Bistro_Price_Tier.Remove(mcbRow);
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