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
                    if (db.OWNERSHIPs.Where(o => o.Id.Equals(OSPViewModel.Id)).Any())
                    {
                        var rowToRemove = db.OWNERSHIPs.Single(o => o.Id.Equals(OSPViewModel.Id));
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
                    return false;
                }
            }
        }

        public bool DeleteOwnership(OwnershipViewModel OSPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                OWNERSHIP ospRow;

                if (db.OWNERSHIPs.Where(o => o.Id.Equals(OSPViewModel.Id)).Any())
                    ospRow = db.OWNERSHIPs.Single(o => o.Id.Equals(OSPViewModel.Id));
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
                    return false;
                }
            }
        }
    }
}