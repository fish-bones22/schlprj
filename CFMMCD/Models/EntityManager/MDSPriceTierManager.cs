﻿using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class MDSPriceTierManager
    {
        public List<MDSPriceTierViewModel> GetMDS()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MDSPriceTierViewModel> MDSList = new List<MDSPriceTierViewModel>();
                foreach (MDS_Price_Tier mds in db.MDS_Price_Tier)
                {
                    MDSPriceTierViewModel MDSViewModel = new MDSPriceTierViewModel();

                    MDSViewModel.Id = (mds.Id).ToString();
                    MDSViewModel.Price_Tier = mds.Price_Tier;
                }
                return MDSList;
            }
        }

        public bool UpdateMDSPriceTier(MDSPriceTierViewModel MDSViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                MDS_Price_Tier mdsRow = new MDS_Price_Tier();

                mdsRow.Id = int.Parse(MDSViewModel.Id);
                mdsRow.Price_Tier = MDSViewModel.Price_Tier;
                try
                {
                    if (db.MDS_Price_Tier.Where(o => o.Id.Equals(MDSViewModel.Id)).Any())
                    {
                        var rowToRemove = db.MDS_Price_Tier.Single(o => o.Id.Equals(MDSViewModel.Id));
                        db.MDS_Price_Tier.Remove(rowToRemove);
                        db.MDS_Price_Tier.Add(mdsRow);
                    }
                    else
                        db.MDS_Price_Tier.Add(mdsRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteMDSPriceTier(MDSPriceTierViewModel MDSViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                MDS_Price_Tier mdsRow;

                if (db.MDS_Price_Tier.Where(o => o.Id.Equals(MDSViewModel.Id)).Any())
                    mdsRow = db.MDS_Price_Tier.Single(o => o.Id.Equals(MDSViewModel.Id));
                else
                    return false;
                try
                {
                    db.MDS_Price_Tier.Remove(mdsRow);
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