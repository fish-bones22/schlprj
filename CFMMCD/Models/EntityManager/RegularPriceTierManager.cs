using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class RegularPriceTierManager
    {
        public List<RegularPriceTierViewModel> GetRPT()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RegularPriceTierViewModel> RPTList = new List<RegularPriceTierViewModel>();
                foreach (Regular_Price_Tier rpt in db.Regular_Price_Tier)
                {
                    RegularPriceTierViewModel RPTViewModel = new RegularPriceTierViewModel();

                    RPTViewModel.Id = (rpt.Id).ToString();
                    RPTViewModel.Price_Tier = rpt.Price_Tier;
                }
                return RPTList;
            }
        }

        public bool UpdateRegularPriceTier(RegularPriceTierViewModel RPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Regular_Price_Tier rptRow = new Regular_Price_Tier();

                rptRow.Id = int.Parse(RPTViewModel.Id);
                rptRow.Price_Tier = RPTViewModel.Price_Tier;
                try
                {
                    if (db.Regular_Price_Tier.Where(o => o.Id.Equals(RPTViewModel.Id)).Any())
                    {
                        var rowToRemove = db.Regular_Price_Tier.Single(o => o.Id.Equals(RPTViewModel.Id));
                        db.Regular_Price_Tier.Remove(rowToRemove);
                        db.Regular_Price_Tier.Add(rptRow);
                    }
                    else
                        db.Regular_Price_Tier.Add(rptRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteRegularPriceTier(RegularPriceTierViewModel RPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Regular_Price_Tier rptRow;

                if (db.Regular_Price_Tier.Where(o => o.Id.Equals(RPTViewModel.Id)).Any())
                    rptRow = db.Regular_Price_Tier.Single(o => o.Id.Equals(RPTViewModel.Id));
                else
                    return false;
                try
                {
                    db.Regular_Price_Tier.Remove(rptRow);
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