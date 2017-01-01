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
                foreach (OWNERSHIP osp in db.OWNERSHIP)
                {
                    OwnershipViewModel OSPViewModel = new OwnershipViewModel();

                    OSPViewModel.Id = (osp.Id).ToString();
                    OSPViewModel.OWNSHP = osp.OWNSHP;
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
                    if (db.OWNERSHIP.Where(o => o.Id.Equals(OSPViewModel.Id)).Any())
                    {
                        var rowToRemove = db.OWNERSHIP.Single(o => o.Id.Equals(OSPViewModel.Id));
                        db.OWNERSHIP.Remove(rowToRemove);
                        db.OWNERSHIP.Add(ospRow);
                    }
                    else
                        db.OWNERSHIP.Add(ospRow);
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

                if (db.OWNERSHIP.Where(o => o.Id.Equals(OSPViewModel.Id)).Any())
                    ospRow = db.OWNERSHIP.Single(o => o.Id.Equals(OSPViewModel.Id));
                else
                    return false;
                try
                {
                    db.OWNERSHIP.Remove(ospRow);
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