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
        public static List<MenuItem> SearchMenuItem(string SearchItem)
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
                    if (rim.MIMMIC != 0)
                        mi.MIMMIC = rim.MIMMIC.ToString();
                    if (rim.MIMDSC != null)
                        mi.MIMDSC = rim.MIMDSC.Trim();
                    if (rim.MIMLON != null)
                        mi.MIMLON = rim.MIMLON.Trim();
                    if (rim.MIMSTA != null)
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
                if (MRRowList.Count() > 0 && MRRowList.FirstOrDefault().Group != null)
                    MRViewModel.Group = (int)MRRowList.FirstOrDefault().Group;
                else MRViewModel.Group = 0;
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
                        db.SaveChanges();
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
                    if (v.RIRSFQ != null && !v.RIRSFQ.Equals(""))
                        MRRow.RIRSFQ = double.Parse(v.RIRSFQ);
                    MRRow.RIRCWC = v.RIRCWC;
                    MRRow.RIRSTA = "0";
                    MRRow.RIRVST = "";
                    MRRow.RIRUSR = user.Substring(0, 3).ToUpper();
                    MRRow.RIRDAT = DateTime.Now;
                    MRRow.RIRFLG = false;
                    // Group
                    var IGRowLookup = db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(MRViewModel.RIRMIC));
                    IGRowLookup = IGRowLookup.Where(o => o.Item_Type == 4);
                    if (IGRowLookup.Any())
                    {
                        if (MRViewModel.Group == 0)
                        {
                            int val = IGRowLookup.FirstOrDefault().Id;
                            ItemGroupManager.DeleteItem(val);
                        }
                        else
                        {
                            IGRowLookup.FirstOrDefault().Item_Code = int.Parse(MRViewModel.RIRMIC);
                            IGRowLookup.FirstOrDefault().Item_Name = MRViewModel.MIMDSC;
                            IGRowLookup.FirstOrDefault().Group_Id = MRViewModel.Group;
                            IGRowLookup.FirstOrDefault().Group_Name = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MRViewModel.Group).Group_Name;
                        }
                    }
                    else
                    {
                        if (MRViewModel.Group != 0)
                        {
                            ItemGroupViewModel IGRow = new ItemGroupViewModel();
                            IGRow.GroupName = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MRViewModel.Group).Group_Name;
                            IGRow.GroupId = MRViewModel.Group;
                            IGRow.ItemCode = int.Parse(MRViewModel.RIRMIC);
                            IGRow.ItemName = MRViewModel.MIMDSC;
                            IGRow.ItemType = 4;
                            IGRow.GroupType = 4;
                            ItemGroupManager.UpdateGroup(IGRow);
                        }
                    }
                    MRRow.Group = MRViewModel.Group;
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
                    if (MRViewModel.RIRSFQ[i] != null && !MRViewModel.RIRSFQ[i].Equals(""))
                        MRRow.RIRSFQ = double.Parse(MRViewModel.RIRSFQ[i]);
                    MRRow.RIRCWC = MRViewModel.RIRCWC[i];
                    MRRow.RIRSTA = "0";
                    MRRow.RIRVST = "";
                    MRRow.RIRUSR = user.Substring(0, 3).ToUpper();
                    MRRow.RIRDAT = DateTime.Now;
                    MRRow.RIRFLG = false;
                    // Group
                    var IGRowLookup = db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(MRViewModel.RIRMIC));
                    IGRowLookup = IGRowLookup.Where(o => o.Item_Type == 4);
                    if (IGRowLookup.Any())
                    {
                        if (MRViewModel.Group == 0)
                        {
                            int val = IGRowLookup.FirstOrDefault().Id;
                            ItemGroupManager.DeleteItem(val);
                        }
                        else
                        {
                            IGRowLookup.FirstOrDefault().Item_Code = int.Parse(MRViewModel.RIRMIC);
                            IGRowLookup.FirstOrDefault().Item_Name = MRViewModel.MIMDSC;
                            IGRowLookup.FirstOrDefault().Group_Id = MRViewModel.Group;
                            IGRowLookup.FirstOrDefault().Group_Name = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MRViewModel.Group).Group_Name;
                        }
                    }
                    else
                    {
                        if (MRViewModel.Group != 0)
                        {
                            ItemGroupViewModel IGRow = new ItemGroupViewModel();
                            IGRow.GroupName = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MRViewModel.Group).Group_Name;
                            IGRow.GroupId = MRViewModel.Group;
                            IGRow.ItemCode = int.Parse(MRViewModel.RIRMIC);
                            IGRow.ItemName = MRViewModel.MIMDSC;
                            IGRow.ItemType = 4;
                            IGRow.GroupType = 4;
                            ItemGroupManager.UpdateGroup(IGRow);
                        }
                    }
                    MRRow.Group = MRViewModel.Group;
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
                List<INVRIRP0> MRRowList = db.INVRIRP0.Where(o => o.RIRMIC.ToString().Equals(MRViewModel.RIRMIC)).ToList();
                if (MRRowList.Count() == 0)
                {
                    var RIRRow = db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(MRViewModel.RIRMIC));
                    RIRRow = RIRRow.Where(o => o.Item_Type == 4);
                    if (RIRRow.Any())
                    {
                        db.ITMGRPs.RemoveRange(RIRRow);
                        db.SaveChanges();
                    }
                }
                    return true;
            }
        }
    }
}