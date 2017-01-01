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
                    if (db.BUSINESS_EXT.Where(o => o.ID.Equals(BEXViewModel.ID)).Any())
                    {
                        var rowToRemove = db.BUSINESS_EXT.Single(o => o.ID.Equals(BEXViewModel.ID));
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
                    return false;
                }
            }
        }

        public bool DeleteBusinessExtension(BusinessExtensionViewModel BEXViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                BUSINESS_EXT bexRow;

                if (db.BUSINESS_EXT.Where(o => o.ID.Equals(BEXViewModel.ID)).Any())
                    bexRow = db.BUSINESS_EXT.Single(o => o.ID.Equals(BEXViewModel.ID));
                else
                    return false;
                try
                {
                    db.BUSINESS_EXT.Remove(bexRow);
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