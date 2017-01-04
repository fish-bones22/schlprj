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
        public List<McCafeLevel3PriceTierViewModel> GetML3()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeLevel3PriceTierViewModel> ML3List = new List<McCafeLevel3PriceTierViewModel>();
                foreach (McCafe_Level_3_Price_Tier ml3 in db.McCafe_Level_3_Price_Tier)
                {
                    McCafeLevel3PriceTierViewModel ML3ViewModel = new McCafeLevel3PriceTierViewModel();
                    ML3ViewModel.Id = (ml3.Id).ToString();
                    ML3ViewModel.Price_Tier = ml3.Price_Tier;
                    // Add to List
                    ML3List.Add(ML3ViewModel);
                }
                return ML3List;
            }
        }

        public bool UpdateMcCafeLevel3PriceTier(McCafeLevel3PriceTierViewModel ML3ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_3_Price_Tier ml3Row = new McCafe_Level_3_Price_Tier();
                ml3Row.Id = int.Parse(ML3ViewModel.Id);
                ml3Row.Price_Tier = ML3ViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Level_3_Price_Tier.Where(o => o.Id.ToString().Equals(ML3ViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Level_3_Price_Tier.Single(o => o.Id.ToString().Equals(ML3ViewModel.Id));
                        db.McCafe_Level_3_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Level_3_Price_Tier.Add(ml3Row);
                    }
                    else
                        db.McCafe_Level_3_Price_Tier.Add(ml3Row);
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

        public bool DeleteMcCafeLevel3PriceTier(McCafeLevel3PriceTierViewModel ML3ViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Level_3_Price_Tier ml3Row;

                if (db.McCafe_Level_3_Price_Tier.Where(o => o.Id.ToString().Equals(ML3ViewModel.Id)).Any())
                    ml3Row = db.McCafe_Level_3_Price_Tier.Single(o => o.Id.ToString().Equals(ML3ViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Level_3_Price_Tier.Remove(ml3Row);
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