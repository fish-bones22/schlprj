using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class BreakfastPriceTierManager
    {
        public List<BreakfastPriceTierViewModel> GetBPT()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<BreakfastPriceTierViewModel> BPTList = new List<BreakfastPriceTierViewModel>();
                foreach (Breakfast_Price_Tier bpt in db.Breakfast_Price_Tier)
                {
                    BreakfastPriceTierViewModel BPTViewModel = new BreakfastPriceTierViewModel();
                    BPTViewModel.Id = (bpt.Id).ToString();
                    BPTViewModel.Price_Tier = bpt.Price_Tier;
                    // Add to List
                    BPTList.Add(BPTViewModel);
                }
                return BPTList;
            }
        }
        
        public bool UpdateBreakfastPriceTier(BreakfastPriceTierViewModel BPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Breakfast_Price_Tier bptRow = new Breakfast_Price_Tier();
                bptRow.Id = int.Parse(BPTViewModel.Id);
                bptRow.Price_Tier = BPTViewModel.Price_Tier;
                try
                {
                    if (db.Breakfast_Price_Tier.Where(o => o.Id.ToString().Equals(BPTViewModel.Id)).Any())
                    {
                        var rowToRemove = db.Breakfast_Price_Tier.Single(o => o.Id.ToString().Equals(BPTViewModel.Id));
                        db.Breakfast_Price_Tier.Remove(rowToRemove);
                        db.Breakfast_Price_Tier.Add(bptRow);
                    }
                    else
                        db.Breakfast_Price_Tier.Add(bptRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
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

        public bool DeleteBreakfastPriceTier(BreakfastPriceTierViewModel BPTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Breakfast_Price_Tier bptRow;

                if (db.Breakfast_Price_Tier.Where(o => o.Id.ToString().Equals(BPTViewModel.Id)).Any())
                    bptRow = db.Breakfast_Price_Tier.Single(o => o.Id.ToString().Equals(BPTViewModel.Id));
                else
                    return false;
                try
                {
                    db.Breakfast_Price_Tier.Remove(bptRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
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
    }
}