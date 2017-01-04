using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class OwnershipManager
    {
        public List<OwnershipViewModel> GetOSP()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<OwnershipViewModel> OSPList = new List<OwnershipViewModel>();
                foreach (OWNERSHIP osp in db.OWNERSHIPs)
                {
                    OwnershipViewModel OSPViewModel = new OwnershipViewModel();
                    OSPViewModel.Id = (osp.Id).ToString();
                    OSPViewModel.OWNSHP = osp.OWNSHP;
                    // Add to List
                    OSPList.Add(OSPViewModel);
                }
                return OSPList;
            }
        }

        public bool UpdateOwnership(OwnershipViewModel OSPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                OWNERSHIP ospRow = new OWNERSHIP();

                ospRow.Id = int.Parse(OSPViewModel.Id);
                ospRow.OWNSHP = OSPViewModel.OWNSHP;
                try
                {
                    if (db.OWNERSHIPs.Where(o => o.Id.ToString().Equals(OSPViewModel.Id)).Any())
                    {
                        var rowToRemove = db.OWNERSHIPs.Single(o => o.Id.ToString().Equals(OSPViewModel.Id));
                        db.OWNERSHIPs.Remove(rowToRemove);
                        db.OWNERSHIPs.Add(ospRow);
                    }
                    else
                        db.OWNERSHIPs.Add(ospRow);
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

        public bool DeleteOwnership(OwnershipViewModel OSPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                OWNERSHIP ospRow;

                if (db.OWNERSHIPs.Where(o => o.Id.ToString().Equals(OSPViewModel.Id)).Any())
                    ospRow = db.OWNERSHIPs.Single(o => o.Id.ToString().Equals(OSPViewModel.Id));
                else
                    return false;
                try
                {
                    db.OWNERSHIPs.Remove(ospRow);
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