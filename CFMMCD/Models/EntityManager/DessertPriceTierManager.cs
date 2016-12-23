using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class DessertPriceTierManager
    {
        public List<DessertPriceTierViewModel> GetDPT()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<DessertPriceTierViewModel> DPTList = new List<DessertPriceTierViewModel>();
                foreach (Dessert_Price_Tier dpt in db.Dessert_Price_Tier)
                {
                    DessertPriceTierViewModel DPTViewModel = new DessertPriceTierViewModel();

                    DPTViewModel.Id = (dpt.Id).ToString();
                    DPTViewModel.Price_Tier = dpt.Price_Tier;
                }
                return DPTList;
            }
        }

        public bool UpdateDessertPriceTier(DessertPriceTierViewModel DPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Dessert_Price_Tier dptRow = new Dessert_Price_Tier();

                dptRow.Id = int.Parse(DPTViewModel.Id);
                dptRow.Price_Tier = DPTViewModel.Price_Tier;
                try
                {
                    if (db.Dessert_Price_Tier.Where(o => o.Id.Equals(DPTViewModel.Id)).Any())
                    {
                        var rowToRemove = db.Dessert_Price_Tier.Single(o => o.Id.Equals(DPTViewModel.Id));
                        db.Dessert_Price_Tier.Remove(rowToRemove);
                        db.Dessert_Price_Tier.Add(dptRow);
                    }
                    else
                        db.Dessert_Price_Tier.Add(dptRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteDessertPriceTier(DessertPriceTierViewModel DPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Dessert_Price_Tier dptRow;

                if (db.Dessert_Price_Tier.Where(o => o.Id.Equals(DPTViewModel.Id)).Any())
                    dptRow = db.Dessert_Price_Tier.Single(o => o.Id.Equals(DPTViewModel.Id));
                else
                    return false;
                try
                {
                    db.Dessert_Price_Tier.Remove(dptRow);
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
