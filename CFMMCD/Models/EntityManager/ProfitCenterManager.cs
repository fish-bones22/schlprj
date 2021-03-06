﻿using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ProfitCenterManager
    {
        public List<ProfitCenterViewModel> GetPRC()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ProfitCenterViewModel> PRCList = new List<ProfitCenterViewModel>();
                foreach (PROFIT_CEN prc in db.PROFIT_CEN)
                {
                    ProfitCenterViewModel PRCViewModel = new ProfitCenterViewModel();
                    PRCViewModel.Id = (prc.Id).ToString();
                    PRCViewModel.PRFCNT = prc.PRFCNT;
                    // Add to List
                    PRCList.Add(PRCViewModel);
                }
                return PRCList;
            }
        }

        public bool UpdateProfitCenter(ProfitCenterViewModel PRCViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                PROFIT_CEN prcRow = new PROFIT_CEN();

                prcRow.Id = int.Parse(PRCViewModel.Id);
                prcRow.PRFCNT = PRCViewModel.PRFCNT;
                try
                {
                    if (db.PROFIT_CEN.Where(o => o.Id.ToString().Equals(PRCViewModel.Id)).Any())
                    {
                        var rowToRemove = db.PROFIT_CEN.Single(o => o.Id.ToString().Equals(PRCViewModel.Id));
                        db.PROFIT_CEN.Remove(rowToRemove);
                        db.PROFIT_CEN.Add(prcRow);
                    }
                    else
                        db.PROFIT_CEN.Add(prcRow);
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

        public bool DeleteProfitCenter(ProfitCenterViewModel PRCViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                PROFIT_CEN prcRow;

                if (db.PROFIT_CEN.Where(o => o.Id.ToString().Equals(PRCViewModel.Id)).Any())
                    prcRow = db.PROFIT_CEN.Single(o => o.Id.ToString().Equals(PRCViewModel.Id));
                else
                    return false;
                try
                {
                    db.PROFIT_CEN.Remove(prcRow);
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