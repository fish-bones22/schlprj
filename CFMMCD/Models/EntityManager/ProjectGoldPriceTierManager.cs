using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ProjectGoldPriceTierManager
    {
        public List<ProjectGoldPriceTierViewModel> GetPGT()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ProjectGoldPriceTierViewModel> PGTList = new List<ProjectGoldPriceTierViewModel>();
                foreach (Project_Gold_Price_Tier pgt in db.Project_Gold_Price_Tier)
                {
                    ProjectGoldPriceTierViewModel PGTViewModel = new ProjectGoldPriceTierViewModel();
                    PGTViewModel.Id = (pgt.Id).ToString();
                    PGTViewModel.Price_Tier = pgt.Price_Tier;
                    // Add to List
                    PGTList.Add(PGTViewModel);
                }
                return PGTList;
            }
        }

        public bool UpdateProjectGoldPriceTier(ProjectGoldPriceTierViewModel PGTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Project_Gold_Price_Tier pgtRow = new Project_Gold_Price_Tier();
                pgtRow.Id = int.Parse(PGTViewModel.Id);
                pgtRow.Price_Tier = PGTViewModel.Price_Tier;
                try
                {
                    if (db.Project_Gold_Price_Tier.Where(o => o.Id.ToString().Equals(PGTViewModel.Id)).Any())
                    {
                        var rowToRemove = db.Project_Gold_Price_Tier.Single(o => o.Id.ToString().Equals(PGTViewModel.Id));
                        db.Project_Gold_Price_Tier.Remove(rowToRemove);
                        db.Project_Gold_Price_Tier.Add(pgtRow);
                    }
                    else
                        db.Project_Gold_Price_Tier.Add(pgtRow);
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

        public bool DeleteProjectGoldPriceTier(ProjectGoldPriceTierViewModel PGTViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Project_Gold_Price_Tier pgtRow;

                if (db.Project_Gold_Price_Tier.Where(o => o.Id.ToString().Equals(PGTViewModel.Id)).Any())
                    pgtRow = db.Project_Gold_Price_Tier.Single(o => o.Id.ToString().Equals(PGTViewModel.Id));
                else
                    return false;
                try
                {
                    db.Project_Gold_Price_Tier.Remove(pgtRow);
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