using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class BusinessExtensionManager
    {
        public List<BusinessExtensionViewModel> GetBEX()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<BusinessExtensionViewModel> BEXList = new List<BusinessExtensionViewModel>();
                foreach (BUSINESS_EXT bex in db.BUSINESS_EXT)
                {
                    BusinessExtensionViewModel BEXViewModel = new BusinessExtensionViewModel();
                    BEXViewModel.ID = (bex.ID).ToString();
                    BEXViewModel.LONGNM = bex.LONGNM;
                    // Add to List
                    BEXList.Add(BEXViewModel);
                }
                return BEXList;
            }
        }

        public bool UpdateBusinessExtension(BusinessExtensionViewModel BEXViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                BUSINESS_EXT bexRow = new BUSINESS_EXT();
                bexRow.ID = int.Parse(BEXViewModel.ID);
                bexRow.LONGNM = BEXViewModel.LONGNM;
                try
                {
                    if (db.BUSINESS_EXT.Where(o => o.ID.ToString().Equals(BEXViewModel.ID)).Any())
                    {
                        var rowToRemove = db.BUSINESS_EXT.Single(o => o.ID.ToString().Equals(BEXViewModel.ID));
                        db.BUSINESS_EXT.Remove(rowToRemove);
                        db.BUSINESS_EXT.Add(bexRow);
                    }
                    else
                        db.BUSINESS_EXT.Add(bexRow);
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

        public bool DeleteBusinessExtension(BusinessExtensionViewModel BEXViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                BUSINESS_EXT bexRow;

                if (db.BUSINESS_EXT.Where(o => o.ID.ToString().Equals(BEXViewModel.ID)).Any())
                    bexRow = db.BUSINESS_EXT.Single(o => o.ID.ToString().Equals(BEXViewModel.ID));
                else
                    return false;

                // Get position in the table to be used in the deletion of entry in StoreProfile table
                int index = db.BUSINESS_EXT.ToList().IndexOf(bexRow);
                // Get Store_Profile rows that contain 'index' in `BET`
                List<Store_Profile> SPRows = db.Store_Profile.Where(o => o.BET.Contains(index.ToString())).ToList();
                for (int  i = 0; i < SPRows.Count(); i++)
                {
                    string BETString = SPRows[i].BET;
                    BETString = BETString.Replace(index.ToString(), "");  // Replace the index with empty
                    BETString = BETString.Replace(",,", ",");             // Removes instances of blank between delimiters
                    SPRows[i].BET = BETString;                        // Replace old entry
                }

                try
                {
                    db.BUSINESS_EXT.Remove(bexRow);
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