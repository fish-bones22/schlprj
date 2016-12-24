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
        public List<ProjectGoldPriceTierViewModel> GetPG()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ProjectGoldPriceTierViewModel> PGList = new List<ProjectGoldPriceTierViewModel>();
                foreach (Project_Gold_Price_Tier pg in db.Project_Gold_Price_Tier)
                {
                    ProjectGoldPriceTierViewModel PGViewModel = new ProjectGoldPriceTierViewModel();

                    PGViewModel.Id = (pg.Id).ToString();
                    PGViewModel.Price_Tier = pg.Price_Tier;
                }
                return PGList;
            }
        }

        public bool UpdateProjectGoldPriceTier(ProjectGoldPriceTierViewModel PGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Project_Gold_Price_Tier pgRow = new Project_Gold_Price_Tier();

                pgRow.Id = int.Parse(PGViewModel.Id);
                pgRow.Price_Tier = PGViewModel.Price_Tier;
                try
                {
                    if (db.Project_Gold_Price_Tier.Where(o => o.Id.Equals(PGViewModel.Id)).Any())
                    {
                        var rowToRemove = db.Project_Gold_Price_Tier.Single(o => o.Id.Equals(PGViewModel.Id));
                        db.Project_Gold_Price_Tier.Remove(rowToRemove);
                        db.Project_Gold_Price_Tier.Add(pgRow);
                    }
                    else
                        db.Project_Gold_Price_Tier.Add(pgRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public bool DeleteProjectGoldPriceTier(ProjectGoldPriceTierViewModel PGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Project_Gold_Price_Tier pgRow;

                if (db.Project_Gold_Price_Tier.Where(o => o.Id.Equals(PGViewModel.Id)).Any())
                    pgRow = db.Project_Gold_Price_Tier.Single(o => o.Id.Equals(PGViewModel.Id));
                else
                    return false;
                try
                {
                    db.Project_Gold_Price_Tier.Remove(pgRow);
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