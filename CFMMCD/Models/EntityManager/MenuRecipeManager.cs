using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class MenuRecipeManager
    {
        public List<MenuItem> SearchMenuItem(MenuRecipeViewModel MRViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MenuItem> MIList = new List<MenuItem>();
                List<CSHMIMP0> MIMRowList;
                if (MRViewModel.SearchItem == null || MRViewModel.SearchItem.Equals(""))
                    return null;
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MRViewModel.SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MRViewModel.SearchItem)).ToList();
                } 
                else if (db.CSHMIMP0.Where(o => o.MIMNAM.Trim().Equals(MRViewModel.SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMNAM.Trim().Equals(MRViewModel.SearchItem)).ToList();
                }
                else
                    return null;
                foreach (CSHMIMP0 rim in MIMRowList)
                {
                    // Check if 'Include inactive items' is checked
                    if (MRViewModel.InactiveItemsCb || rim.MIMSTA.Equals("0"))
                    {
                        MenuItem mi = new MenuItem();
                        mi.RIRMIC = rim.MIMMIC.ToString();
                        mi.MIMDSC = rim.MIMDSC.Trim();
                        mi.MIMLON = rim.MIMLON.Trim();
                        MIList.Add(mi);
                    }
                }
                if (MIList == null || MIList.ElementAt(0) == null)
                    return null;
                return MIList;
            }
        }

        public MenuRecipeViewModel SearchMenuRecipe(string MenuItemCode)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVRIRP0> MRRowList = new List<INVRIRP0>();
                MenuRecipeViewModel MRViewModel = new MenuRecipeViewModel();

                if (db.INVRIRP0.Where(o => o.RIRMIC.ToString().Equals(MenuItemCode)).Any())
                {
                    MRRowList = db.INVRIRP0.Where(o => o.RIRMIC.ToString().Equals(MenuItemCode)).ToList();
                }

                MRViewModel.RIRMIC = MenuItemCode;
                MRViewModel.MIMDSC = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MenuItemCode)).MIMDSC;
                MRViewModel.MIMLON = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MenuItemCode)).MIMLON;
                MRViewModel.MenuRecipeList = new List<MenuRecipe>();
                foreach (var v in MRRowList)
                {
                    MenuRecipe mr = new MenuRecipe();
                    mr.RIRRID = v.RIRRID;
                    mr.RIRRIC = v.RIRRIC.ToString();
                    mr.RIMRID = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(mr.RIRRIC)).RIMRID;
                    mr.RIMCPR = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(mr.RIRRIC)).RIMCPR.ToString();
                    mr.RIRSFQ = v.RIRSFQ.ToString();
                    mr.RIRCWC = v.RIRCWC;
                    mr.RIRSTA = v.RIRSTA;
                    mr.STOATT = "";
                    MRViewModel.MenuRecipeList.Add(mr);
                }
                return MRViewModel;
            }
        }

        public bool UpdateMenuItem(MenuRecipeViewModel MRViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (MRViewModel.RIRMIC == null || MRViewModel.RIRMIC.Equals(""))
                    return false;
                // Existing
                foreach (var v in MRViewModel.MenuRecipeList)
                {
                    INVRIRP0 MRRow = new INVRIRP0();
                    MRRow.RIRRID = MRViewModel.RIRMIC + v.RIRRIC;
                    MRRow.RIRMIC = int.Parse(MRViewModel.RIRMIC);
                    MRRow.RIRRIC = int.Parse(v.RIRRIC);
                    MRRow.RIRVPC = 0;
                    MRRow.RIRSFQ = double.Parse(v.RIRSFQ);
                    MRRow.RIRCWC = v.RIRCWC;
                    MRRow.RIRSTA = "0";
                    MRRow.RIRVST = "";
                    MRRow.RIRUSR = user.Substring(0, 3).ToUpper();
                    MRRow.RIRDAT = DateTime.Now;
                    MRRow.RIRFLG = false;
                    try
                    {
                        if (db.INVRIRP0.Where(o => o.RIRRID == MRRow.RIRRID).Any())
                        {
                            INVRIRP0 rowToDelete = db.INVRIRP0.Single(o => o.RIRRID == MRRow.RIRRID);
                            db.INVRIRP0.Remove(rowToDelete);
                            MRRow.STATUS = "E";
                            db.INVRIRP0.Add(MRRow);
                        }
                        else
                        {
                            MRRow.STATUS = "A";
                            db.INVRIRP0.Add(MRRow);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                // New
                for (int i = 0; i < MRViewModel.RIMRID.Count(); i++)
                {
                    INVRIRP0 MRRow = new INVRIRP0();
                    MRRow.RIRRID = MRViewModel.RIRMIC + MRViewModel.RIRRIC[i];
                    MRRow.RIRMIC = int.Parse(MRViewModel.RIRMIC);
                    MRRow.RIRRIC = int.Parse(MRViewModel.RIRRIC[i]);
                    MRRow.RIRVPC = 0;
                    MRRow.RIRSFQ = double.Parse(MRViewModel.RIRSFQ[i]);
                    MRRow.RIRCWC = MRViewModel.RIRCWC[i];
                    MRRow.RIRSTA = "0";
                    MRRow.RIRVST = "";
                    MRRow.RIRUSR = user.Substring(0, 3).ToUpper();
                    MRRow.RIRDAT = DateTime.Now;
                    MRRow.RIRFLG = false;
                    try
                    {
                        if (db.INVRIRP0.Where(o => o.RIRRID == MRRow.RIRRID).Any())
                        {
                            INVRIRP0 rowToDelete = db.INVRIRP0.Single(o => o.RIRRID == MRRow.RIRRID);
                            db.INVRIRP0.Remove(rowToDelete);
                            MRRow.STATUS = "E";
                            db.INVRIRP0.Add(MRRow);
                        }
                        else
                        {
                            MRRow.STATUS = "A";
                            db.INVRIRP0.Add(MRRow);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}