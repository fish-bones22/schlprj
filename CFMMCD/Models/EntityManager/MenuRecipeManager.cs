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
        public static List<MenuItem> SearchMenuItem( string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MenuItem> MIList = new List<MenuItem>();
                List<CSHMIMP0> MIMRowList;
                if (SearchItem == null || SearchItem.Equals(""))
                    return null;
                if (SearchItem.ToUpper().Equals("ALL"))
                {
                    MIMRowList = db.CSHMIMP0.ToList();
                }
                else if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).ToList();
                } 
                else if (db.CSHMIMP0.Where(o => o.MIMNAM.Trim().Equals(SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMNAM.Trim().Equals(SearchItem)).ToList();
                }
                else
                    return null;
                foreach (CSHMIMP0 rim in MIMRowList)
                {
                    MenuItem mi = new MenuItem();
                    mi.RIRMIC = rim.MIMMIC.ToString();
                    mi.MIMDSC = rim.MIMDSC.Trim();
                    mi.MIMLON = rim.MIMLON.Trim();
                    mi.MIMSTA = rim.MIMSTA;
                    MIList.Add(mi);
                }
                if (MIList == null || MIList.ElementAt(0) == null)
                    return null;
                return MIList;
            }
        }

        public static MenuRecipeViewModel SearchMenuRecipe(string MenuItemCode)
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
                    // Get price by getting primary vendor id first
                    int primVenId = (int)db.INVRIMP0.Single(o => o.RIMRIC == v.RIRRIC).RIMPVN;
                    // Look up Raw item and Vendor
                    if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(v.RIRRIC.ToString() + primVenId.ToString())).Any())
                        mr.RIMCPR = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(v.RIRRIC.ToString() + primVenId.ToString())).RIMCPR.ToString();
                    mr.RIRSFQ = v.RIRSFQ.ToString();
                    mr.RIRCWC = v.RIRCWC;
                    mr.RIRSTA = v.RIRSTA;
                    mr.STOATT = db.INVRIMP0.Single(o => o.RIMRIC == v.RIRRIC).Store_Attrib;
                    MRViewModel.MenuRecipeList.Add(mr);
                }
                return MRViewModel;
            }
        }

        public static bool UpdateMenuItem(MenuRecipeViewModel MRViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (MRViewModel.RIRMIC == null || MRViewModel.RIRMIC.Equals(""))
                    return false;
                // Existing row
                foreach (var v in MRViewModel.MenuRecipeList)
                {
                    if (!v.PreviousRIRRIC.Equals(v.RIRRIC))
                    {
                        INVRIRP0 MRRowToDelete = db.INVRIRP0.Single(o => o.RIRRID.Equals(v.RIRRID));
                        db.INVRIRP0.Remove(MRRowToDelete);
                    }
                    if (!db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(v.RIRRIC)).Any())
                        continue;
                    INVRIRP0 MRRow;
                    if (db.INVRIRP0.Where(o => o.RIRRID.Equals(MRViewModel.RIRMIC + v.RIRRIC)).Any())
                        MRRow = db.INVRIRP0.Single(o => o.RIRRID.Equals(MRViewModel.RIRMIC + v.RIRRIC));
                    else MRRow = new INVRIRP0();
                    
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
                            MRRow.STATUS = "E";
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
                // New created row
                for (int i = 0; i < MRViewModel.RIMRID.Count(); i++)
                {
                    if (MRViewModel.RIRRIC[i] == null || MRViewModel.RIRRIC[i].Equals(""))
                        continue;
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
                return true;
            }
        }
    }
}