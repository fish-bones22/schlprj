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
        public List<McCafeBistroPriceTierViewModel> GetMBT()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<McCafeBistroPriceTierViewModel> MBTList = new List<McCafeBistroPriceTierViewModel>();
                foreach (McCafe_Bistro_Price_Tier mbt in db.McCafe_Bistro_Price_Tier)
                {
                    McCafeBistroPriceTierViewModel MBTViewModel = new McCafeBistroPriceTierViewModel();
                    MBTViewModel.Id = (mbt.Id).ToString();
                    MBTViewModel.Price_Tier = mbt.Price_Tier;
                    // Add to List
                    MBTList.Add(MBTViewModel);
                }
                return MBTList;
            }
        }

        public bool UpdateMcCafeBistroPriceTier(McCafeBistroPriceTierViewModel MBTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Bistro_Price_Tier mbtRow = new McCafe_Bistro_Price_Tier();
                mbtRow.Id = int.Parse(MBTViewModel.Id);
                mbtRow.Price_Tier = MBTViewModel.Price_Tier;
                try
                {
                    if (db.McCafe_Bistro_Price_Tier.Where(o => o.Id.ToString().Equals(MBTViewModel.Id)).Any())
                    {
                        var rowToRemove = db.McCafe_Bistro_Price_Tier.Single(o => o.Id.ToString().Equals(MBTViewModel.Id));
                        db.McCafe_Bistro_Price_Tier.Remove(rowToRemove);
                        db.McCafe_Bistro_Price_Tier.Add(mbtRow);
                    }
                    else
                        db.McCafe_Bistro_Price_Tier.Add(mbtRow);
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

        public bool DeleteMcCafeBistroPriceTier(McCafeBistroPriceTierViewModel MBTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                McCafe_Bistro_Price_Tier mbtRow;

                if (db.McCafe_Bistro_Price_Tier.Where(o => o.Id.ToString().Equals(MBTViewModel.Id)).Any())
                    mbtRow = db.McCafe_Bistro_Price_Tier.Single(o => o.Id.ToString().Equals(MBTViewModel.Id));
                else
                    return false;
                try
                {
                    db.McCafe_Bistro_Price_Tier.Remove(mbtRow);
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