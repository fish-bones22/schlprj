using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class LocationManager
    {
        public List<LocationViewModel> GetLCN()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<LocationViewModel> LCNList = new List<LocationViewModel>();
                foreach (LOCATION lcn in db.LOCATIONs)
                {
                    LocationViewModel LCNViewModel = new LocationViewModel();
                    LCNViewModel.Id = (lcn.Id).ToString();
                    LCNViewModel.LOCATN = lcn.LOCATN;
                    // Add to List
                    LCNList.Add(LCNViewModel);
                }
                return LCNList;
            }
        }

        public bool UpdateLocation(LocationViewModel LCNViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                LOCATION lcnRow = new LOCATION();

                lcnRow.Id = int.Parse(LCNViewModel.Id);
                lcnRow.LOCATN = LCNViewModel.LOCATN;
                try
                {
                    if (db.LOCATIONs.Where(o => o.Id.ToString().Equals(LCNViewModel.Id)).Any())
                    {
                        var rowToRemove = db.LOCATIONs.Single(o => o.Id.ToString().Equals(LCNViewModel.Id));
                        db.LOCATIONs.Remove(rowToRemove);
                        db.LOCATIONs.Add(lcnRow);
                    }
                    else
                        db.LOCATIONs.Add(lcnRow);
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

        public bool DeleteLocation(LocationViewModel LCNViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                LOCATION lcnRow;

                if (db.LOCATIONs.Where(o => o.Id.ToString().Equals(LCNViewModel.Id)).Any())
                    lcnRow = db.LOCATIONs.Single(o => o.Id.ToString().Equals(LCNViewModel.Id));
                else
                    return false;
                try
                {
                    db.LOCATIONs.Remove(lcnRow);
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