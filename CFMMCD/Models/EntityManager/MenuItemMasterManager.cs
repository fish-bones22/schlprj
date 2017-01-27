using CFMMCD.DropDown;
using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class MenuItemMasterManager
    {
        /*
         * Searches the table for any given SearchItem (Name or ID) in the ViewModel.
         * Calls SearchSingleMenuItem
         * Returns List<ViewModel> if true, otherwise returns null
         */
        public static List<MenuItem> SearchMenuItems(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHMIMP0> MIMRowList;
                List<MenuItem> MIMList = new List<MenuItem>();
                if (SearchItem.ToUpper().Equals("ALL"))
                {
                    MIMRowList = db.CSHMIMP0.ToList();
                }
                else if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).ToList();
                }
                else if (db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Contains(SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Contains(SearchItem)).ToList();
                }
                else
                {
                    return null;
                }

                foreach (var MIMRow in MIMRowList)
                {
                    MenuItem vm = new MenuItem();
                    vm.MIMMIC = MIMRow.MIMMIC.ToString();
                    vm.MIMDSC = MIMRow.MIMNAM;
                    vm.MIMSTA = MIMRow.MIMSTA;
                    MIMList.Add(vm);
                }
               if (MIMList == null || MIMList.Count() == 0 )
                    return null;
                return MIMList;
            }
        }
        public static MenuItemMasterViewModel SearchSingleMenuItem(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                string ItemCode = "";
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).Any())
                {
                    MIMRow = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(SearchItem));
                    ItemCode = SearchItem;
                }
                else
                {
                    return null;
                }
                MenuItemMasterViewModel vm = new MenuItemMasterViewModel();
                vm.MIMMIC = MIMRow.MIMMIC.ToString();
                vm.MIMSTA = MIMRow.MIMSTA.Trim();
                if (MIMRow.MIMFGC != null && !MIMRow.MIMFGC.Equals(""))
                    vm.MIMFGC = MIMRow.MIMFGC.Trim();
                else vm.MIMFGC = "08";
                vm.MIMNAM = MIMRow.MIMNAM.Trim();
                vm.MIMDSC = MIMRow.MIMDSC.Trim();
                if (MIMRow.MIMDPC != null && !MIMRow.MIMDPC.Equals(""))
                    vm.MIMDPC = MIMRow.MIMDPC.Trim();
                else vm.MIMDPC = "02";
                if (MIMRow.MIMTCI != null)
                    vm.MIMTCI = MIMRow.MIMTCI.Trim();
                if (MIMRow.MIMPRI != null)
                    vm.MIMPRI = MIMRow.MIMPRI.ToString();
                if (MIMRow.MIMTCA != null)
                    vm.MIMTCA = MIMRow.MIMTCA.Trim();
                if (MIMRow.MIMPRO != null)
                    vm.MIMPRO = MIMRow.MIMPRO.ToString();
                if (MIMRow.MIMTCG != null)
                    vm.MIMTCG = MIMRow.MIMTCG.Trim();
                if (MIMRow.MIMPRG != null)
                    vm.MIMPRG = MIMRow.MIMPRG.ToString();
                if (MIMRow.MIMPND != null)
                    vm.MIMPND = Convert.ToDateTime(MIMRow.MIMPND).ToString("yyyy-MM-dd");
                if (MIMRow.MIMWGR != null && !MIMRow.Equals(""))
                    vm.MIMWGR = MIMRow.MIMWGR.Trim();
                else vm.MIMWGR = "02";
                if (MIMRow.MIMHPT != null && !MIMRow.MIMHPT.Equals(""))
                    vm.MIMHPT = MIMRow.MIMHPT.Trim();
                else vm.MIMHPT = "08";
                if (MIMRow.MIMEDT != null)
                    vm.MIMEDT = Convert.ToDateTime(MIMRow.MIMEDT).ToString("yyyy-MM-dd");
                if (MIMRow.MIMNPI != null)
                    vm.MIMNPI = MIMRow.MIMNPI.ToString();
                if (MIMRow.MIMNPO != null)
                    vm.MIMNPO = MIMRow.MIMNPO.ToString();
                if (MIMRow.MIMNPD != null)
                    vm.MIMNPD = MIMRow.MIMNPD.ToString();
                if (MIMRow.MIMNPA != null)
                    vm.MIMNPA = MIMRow.MIMNPA.ToString();
                if (MIMRow.MIMNNP != null)
                    vm.MIMNNP = MIMRow.MIMNNP.ToString();
                if (MIMRow.MIMNPT != null)
                    vm.MIMNPT = MIMRow.MIMNPT.Trim();
                if (MIMRow.MIMLON != null)
                    vm.MIMLON = MIMRow.MIMLON.Trim();
                if (MIMRow.MIMUTC != null)
                    vm.MIMUTC = MIMRow.MIMUTC.ToString();
                if (MIMRow.Group != null)
                    vm.Group = (int)MIMRow.Group;
                else vm.Group = 0;
                if (MIMRow.Trading_Area != null)
                    vm.Trading_Area = MIMRow.Trading_Area.ToString();
                if (MIMRow.Category != null)
                    vm.Category = MIMRow.Category.ToString();

                // Store
                vm.Store = MIMRow.Store;
                if ((MIMRow.Store != null) && MIMRow.Store.Equals("ALL"))
                {
                    vm.SelectAllCb = true;
                    vm.Store = "";
                }
                if ((MIMRow.Except_Store != null) && !(MIMRow.Except_Store.Equals("")))
                {
                    vm.SelectExcept = true;
                    vm.SelectAllCb = false;
                }
                // Attributes
                if (MIMRow.Category != null)
                    vm.Category = MIMRow.Category.ToString();
                if (MIMRow.Trading_Area != null)
                    vm.Category = MIMRow.Trading_Area.ToString();
                // Location
                if (MIMRow.Location != null)
                    vm.Location = MIMRow.Location.ToString();
                if (MIMRow.Region != null)
                    vm.Region = MIMRow.Region;
                if (MIMRow.Province != null)
                    vm.Province = MIMRow.Province;
                if (MIMRow.City != null)
                    vm.City = MIMRow.City;


                if (MIMRow.MIMSTA.Trim().Equals("1"))
                    vm.InactiveItemsCb = true;
                else
                    vm.InactiveItemsCb = false;
                // NP6
                if (MIMRow.MIMMIC_NP6 != null || MIMRow.MIMMIC_NP6.HasValue)
                {
                    vm.MIMMIC_NP6 = MIMRow.CSHMIMP0_NP6.MIMMIC.ToString();
                    vm.MIMNAM_NP6 = MIMRow.CSHMIMP0_NP6.MIMNAM.Trim();
                    vm.MIMLON_NP6 = MIMRow.CSHMIMP0_NP6.MIMLON.Trim();
                }
                // Price tier
                vm.TierList = GetPriceTier(vm.MIMMIC);
                if (vm.TierList.Count() > 0)
                    vm.EffectiveDate = vm.TierList[0].MIMPND;
                // Recipe List
                List<INVRIRP0> RIRRowList;
                if (db.INVRIRP0.Where(o => o.RIRMIC == MIMRow.MIMMIC).Any())
                    RIRRowList = db.INVRIRP0.Where(o => o.RIRMIC == MIMRow.MIMMIC).ToList();
                else
                    RIRRowList = new List<INVRIRP0>();
                foreach(INVRIRP0 RIRRow in RIRRowList)
                {
                    MenuRecipe mr = new MenuRecipe();
                    mr.RIRRIC = RIRRow.RIRRIC.ToString();
                    mr.RIMRID = db.INVRIMP0.Single(o => o.RIMRIC == RIRRow.RIRRIC).RIMRID;
                    vm.MenuRecipeList.Add(mr);
                }

                if (vm != null && vm.MIMMIC != null && !vm.MIMMIC.Equals(""))
                    return vm;
                else return null;
            }
        }
        /*
         * Combined Create and Update Menu Item method.
         * Creates a CSHMIMP0 instance (which will be a new table row)
         * and instantiates each MIMViewModel property to the respective property of the former.
         * Also checks if the given Menu Item Code is already in the table,
         * if true, the method performs an update, otherwise, creation. 
         *
         * Returns true if the operation is successful.
         * */
        public static bool UpdateMenuItem(MenuItemMasterViewModel MIMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                // Columns with Input fields
                if (MIMViewModel.MIMMIC != null)
                    MIMRow.MIMMIC = int.Parse(MIMViewModel.MIMMIC);
                else return false;
                MIMRow.MIMSTA = MIMViewModel.MIMSTA.Trim();
                if (MIMViewModel.MIMFGC != null && !MIMViewModel.MIMFGC.Equals(""))
                    MIMRow.MIMFGC = MIMViewModel.MIMFGC.Trim();
                else MIMRow.MIMFGC = "08";
                if (MIMViewModel.MIMNAM != null)
                    MIMRow.MIMNAM = MIMViewModel.MIMNAM.Trim();
                else return false;
                if (MIMViewModel.MIMDSC != null)
                    MIMRow.MIMDSC = MIMViewModel.MIMDSC.Trim();
                if (MIMViewModel.MIMDPC != null && !MIMViewModel.MIMDPC.Equals(""))
                    MIMRow.MIMDPC = MIMViewModel.MIMDPC.Trim();
                else MIMRow.MIMDPC = "02";
                MIMRow.MIMTCI = MIMViewModel.MIMTCI.Trim();
                MIMRow.MIMPRI = 0; //
                MIMRow.MIMTCA = MIMViewModel.MIMTCA.Trim();
                MIMRow.MIMPRO = 0; //
                MIMRow.MIMTCG = MIMViewModel.MIMTCG.Trim();
                MIMRow.MIMPRG = 0; //
                MIMRow.MIMPND = DateTime.Now; // 
                MIMRow.MIMWGR = MIMViewModel.MIMWGR.Trim();
                if (MIMViewModel.MIMUTC != null)
                    MIMRow.MIMUTC = int.Parse(MIMViewModel.MIMUTC);
                MIMRow.MIMHPT = MIMViewModel.MIMHPT.Trim();
                if (MIMViewModel.MIMEDT != null)
                    MIMRow.MIMEDT = Convert.ToDateTime(MIMViewModel.MIMEDT);
                MIMRow.MIMNPI = 0;
                MIMRow.MIMNPO = 0;
                MIMRow.MIMNPD = 0;
                MIMRow.MIMNPA = 0;
                MIMRow.MIMNNP = 0;
                MIMRow.MIMNPT = MIMViewModel.MIMNPT.Trim();
                // Group
                var IGRowLookup = db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(MIMViewModel.MIMMIC));
                IGRowLookup = IGRowLookup.Where(o => o.Item_Type == 1);
                if (IGRowLookup.Any())
                {
                    if (MIMViewModel.Group == 0)
                    {
                        int val = IGRowLookup.FirstOrDefault().Id;
                        ItemGroupManager.DeleteItem(val);
                    }
                    else
                    {
                        IGRowLookup.FirstOrDefault().Item_Code = int.Parse(MIMViewModel.MIMMIC);
                        IGRowLookup.FirstOrDefault().Item_Name = MIMViewModel.MIMNAM;
                        IGRowLookup.FirstOrDefault().Group_Id = MIMViewModel.Group;
                        IGRowLookup.FirstOrDefault().Group_Name = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MIMViewModel.Group).Group_Name;
                    }
                }
                else
                {
                    if (MIMViewModel.Group != 0)
                    {
                        ItemGroupViewModel IGRow = new ItemGroupViewModel();
                        IGRow.GroupName = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == MIMViewModel.Group).Group_Name;
                        IGRow.GroupId = MIMViewModel.Group;
                        IGRow.ItemCode = int.Parse(MIMViewModel.MIMMIC);
                        IGRow.ItemName = MIMViewModel.MIMNAM;
                        IGRow.ItemType = 1;
                        IGRow.GroupType = 1;
                        ItemGroupManager.UpdateGroup(IGRow);
                    }
                }
                MIMRow.Group = MIMViewModel.Group;
                // Items not originally in the table but
                // have Input field
                if (MIMViewModel.MIMLON != null)
                    MIMRow.MIMLON = MIMViewModel.MIMLON;
                // Items that do not have Input fields
                // but included in the table
                MIMRow.MIMSSC = "08";
                MIMRow.MIMCIN = "";
                MIMRow.MIMDGC = 0;
                MIMRow.MIMASC = "00";
                MIMRow.MIMTXC = "";
                MIMRow.MIMDWE = "0";
                MIMRow.MIMKBP = 0;
                MIMRow.MIMKSC = "";
                MIMRow.MIMSKT = "00";
                MIMRow.MIMGRP = "00";
                MIMRow.MIMWLV = "0";
                MIMRow.MIMWSD = "0";
                MIMRow.MIMUSR = user.Substring(0, 3).ToUpper();
                MIMRow.MIMDAT = DateTime.Now;
                MIMRow.MIMFLG = false;
                MIMRow.MIMBIN = "";
                MIMRow.MIMBIT = 0;
                MIMRow.MIMBIR = "";
                MIMRow.MIMBGR = "";
                MIMRow.MIMBQU = 0;
                MIMRow.MIMGRA = "";
                MIMRow.MIMIST = "";
                MIMRow.MIMCLR = "";
                MIMRow.MIMSKI = 0;
                MIMRow.MIMBMI = 0;
                MIMRow.STATUS = "A";
                // Attributes
                MIMRow.Category = int.Parse(MIMViewModel.Category);
                MIMRow.Trading_Area = int.Parse(MIMViewModel.Trading_Area);
                // Location
                MIMRow.City = MIMViewModel.City;
                MIMRow.Province = MIMViewModel.Province;
                MIMRow.Region = MIMViewModel.Region;
                if (MIMViewModel.Location != null && !MIMViewModel.Location.Equals(""))
                    MIMRow.Location = int.Parse(MIMViewModel.Location);
                // Store
                MIMRow.Store = MIMViewModel.Store;
                if (MIMViewModel.SelectAllCb)
                {
                    MIMRow.Store = "ALL";
                }
                if (MIMViewModel.SelectExcept)
                {
                    MIMRow.Store = "ALL";
                    MIMRow.Except_Store = MIMViewModel.Store.Trim();
                }
                else
                {
                    MIMRow.Except_Store = null;
                }
                // If NP6 fields are filled up
                if (MIMViewModel.MIMMIC_NP6 != null)
                {
                    MIMRow.CSHMIMP0_NP6 = new CSHMIMP0_NP6();
                    MIMRow.CSHMIMP0_NP6.MIMMIC = int.Parse(MIMViewModel.MIMMIC_NP6);
                    MIMRow.CSHMIMP0_NP6.MIMNAM = MIMViewModel.MIMNAM_NP6;
                    MIMRow.CSHMIMP0_NP6.MIMLON = MIMViewModel.MIMLON_NP6;
                }
                // If MIMMIC is already existing, update it instead of inserting a new row
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                {
                    var rowToUpdate = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)); ;
                    MIMRow.STATUS = "E";
                    if (rowToUpdate.CSHMIMP0_NP6 != null)
                        db.CSHMIMP0_NP6.Remove(rowToUpdate.CSHMIMP0_NP6);   // Delete existing row before inserting 
                    db.CSHMIMP0.Remove(rowToUpdate);                        // updated replacement
                    db.CSHMIMP0.Add(MIMRow);
                }
                else
                {
                    db.CSHMIMP0.Add(MIMRow);
                }
                UpdatePriceTier(MIMViewModel.TierList);
                try
                {
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

        /*
         * Deletes the specified row given in the ViewModel properties.
         * 
         * Returns true if operation is successful.
         * */
        public static bool DeleteMenuItem(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                    MIMRow = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));
                else
                    return false;
                // Try...Catch to produce appropriate warnings if ever
                // insertion is unsuccessful
                try
                {
                    // Delete Recipe
                    List<INVRIRP0> MIRecipe = db.INVRIRP0.Where(o => o.RIRMIC == MIMRow.MIMMIC).ToList();
                    db.INVRIRP0.RemoveRange(MIRecipe);
                    // Delete MIM_Price
                    if (db.MIM_Price.Where(o => o.MIMMIC == MIMRow.MIMMIC).Any())
                    {
                        List<MIM_Price> MIMPrice = db.MIM_Price.Where(o => o.MIMMIC == MIMRow.MIMMIC).ToList();
                        db.MIM_Price.RemoveRange(MIMPrice);
                    }
                    // Delete Group
                    var IGRowLookup = db.ITMGRPs.Where(o => o.Item_Code == MIMRow.MIMMIC);
                    IGRowLookup = IGRowLookup.Where(o => o.Item_Type == 1);
                    if (IGRowLookup.Any())
                    {
                        db.ITMGRPs.RemoveRange(IGRowLookup);
                    }
                    db.CSHMIMP0.Remove(MIMRow);
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
        public static bool UpdatePriceTier(List<Tier> TierList)
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                for (int i = 0; i < TierList.Count(); i++)
                {
                    bool IsUpdate = true;
                    MIM_Price MIMPriceRow;
                    if (TierList[i].MIMMIC == null || TierList[i].MIMMIC.Equals(""))
                        continue;
                    string id = TierList[i].MIMMIC + TierList[i].TierName;
                    if (db.MIM_Price.Where(o => o.Id.Equals(id)).Any())
                    {
                        MIMPriceRow = db.MIM_Price.Single(o => o.Id.Equals(id));
                    }
                    else
                    {
                        IsUpdate = false;
                        MIMPriceRow = new MIM_Price();
                        MIMPriceRow.Id = TierList[i].MIMMIC + TierList[i].TierName;
                        MIMPriceRow.MIMMIC = int.Parse(TierList[i].MIMMIC);
                        MIMPriceRow.MIMNAM = TierList[i].MIMNAM;
                        MIMPriceRow.MITIER = TierList[i].TierName;
                    }
                    if (TierList[i].MIMPRI != null && !TierList[i].MIMPRI.Equals(""))
                        MIMPriceRow.MIMPRI = double.Parse(TierList[i].MIMPRI); // Eat in
                    if (TierList[i].MIMPRO != null && !TierList[i].MIMPRO.Equals(""))
                        MIMPriceRow.MIMPRO = double.Parse(TierList[i].MIMPRO); // Take out
                    if (TierList[i].MIMPRG != null && !TierList[i].MIMPRG.Equals(""))
                        MIMPriceRow.MIMPRG = double.Parse(TierList[i].MIMPRG); // Other
                    if (TierList[i].MIMNPA != null && !TierList[i].MIMNPA.Equals(""))
                        MIMPriceRow.MIMNPA = double.Parse(TierList[i].MIMNPA); // Non-product
                    // New
                    if (TierList[i].MIMNPI != null && !TierList[i].MIMNPI.Equals(""))
                        MIMPriceRow.MIMNPI = double.Parse(TierList[i].MIMNPI); // Eat in new
                    if (TierList[i].MIMNPO != null && !TierList[i].MIMNPO.Equals(""))
                        MIMPriceRow.MIMNPO = double.Parse(TierList[i].MIMNPO); // Take out new
                    if (TierList[i].MIMNPD != null && !TierList[i].MIMNPD.Equals(""))
                        MIMPriceRow.MIMNPD = double.Parse(TierList[i].MIMNPD); // Other new
                    if (TierList[i].MIMNNP != null && !TierList[i].MIMNNP.Equals(""))
                        MIMPriceRow.MIMNNP = double.Parse(TierList[i].MIMNNP); // Non-product new
                    // Effective date
                    if (TierList[i].MIMPND != null && !TierList[i].MIMPND.Equals(""))
                        MIMPriceRow.MIMPND = DateTime.Parse(TierList[i].MIMPND);

                    if (!IsUpdate)
                    {
                        db.MIM_Price.Add(MIMPriceRow);
                    }
                    try
                    {
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
                return true;

            }
        }

        public static bool UpdatePriceTier(MenuItemMasterViewModel MIMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                MenuItemMasterViewModel mi = SearchSingleMenuItem(MIMViewModel.MIMMIC);
                mi.TierList = MIMViewModel.TierList;
                if (MIMViewModel.EffectiveDate != null)
                {
                    foreach (var v in mi.TierList)
                    {
                        v.MIMPND = MIMViewModel.EffectiveDate;
                    }
                }
                return UpdateMenuItem(mi, user);
            }
        }

        public static List<Tier> GetPriceTier(string MIMMIC)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MIM_Price> MIMPriceList;
                List<Tier> TierList = TierManager.SetTierList();
                if (db.MIM_Price.Where(o => o.MIMMIC.ToString().Equals(MIMMIC)).Any())
                    MIMPriceList = db.MIM_Price.Where(o => o.MIMMIC.ToString().Equals(MIMMIC)).ToList();
                else return TierList;

                foreach (var v in TierList)
                {
                    if (MIMPriceList.Where(o => o.MITIER.Equals(v.TierName)).Any())
                    {
                        MIM_Price MIMPrice = MIMPriceList.Single(m => m.MITIER.Equals(v.TierName));
                        v.MIMMIC = MIMPrice.MIMMIC.ToString();
                        v.MIMNAM = MIMPrice.MIMNAM;
                        v.MIMPRI = MIMPrice.MIMPRI.ToString(); // Eat in
                        v.MIMPRO = MIMPrice.MIMPRO.ToString(); // Take out
                        v.MIMPRG = MIMPrice.MIMPRG.ToString(); // Other
                        v.MIMNPA = MIMPrice.MIMNPA.ToString(); // Non-product
                                                               // New
                        v.MIMNPI = MIMPrice.MIMNPI.ToString(); // Eat in new
                        v.MIMNPO = MIMPrice.MIMNPO.ToString(); // Take out new
                        v.MIMNPD = MIMPrice.MIMNPD.ToString(); // Other new
                        v.MIMNNP = MIMPrice.MIMNNP.ToString(); // Non-product new
                                                               // Effective date
                        if (MIMPrice.MIMPND != null)
                            v.MIMPND = ((DateTime)MIMPrice.MIMPND).ToString("yyyy-MM-dd");
                    }
                    
                }
                return TierList;

            }
        }

        public static TierUpdate SearchPriceTierUpdate (string MIMMIC)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MIM_Price> MIMPriceRowList;
                List<TierUpdate> TierList = new List<TierUpdate>();
                if (db.MIM_Price.Where(o => o.MIMMIC.ToString().Equals(MIMMIC)).Any())
                {
                    MIMPriceRowList = db.MIM_Price.Where(o => o.MIMMIC.ToString().Equals(MIMMIC)).ToList();
                    TierUpdate tu = new TierUpdate();
                    tu.MIMMIC = MIMMIC;
                    tu.MIMNAM = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMMIC)).MIMNAM;
                    tu.MIMSTA = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMMIC)).MIMSTA;
                    if (MIMPriceRowList.Where(o => o.MITIER.Equals("A")).Any())
                    {
                        var v = MIMPriceRowList.Single(o => o.MITIER.Equals("A"));

                        if (v.MIMNPI != null)
                            tu.TierANew = v.MIMNPI.ToString();
                        else tu.TierANew = "";

                        if (v.MIMPRI != null)
                            tu.TierAOld = v.MIMPRI.ToString();
                        else tu.TierAOld = "";

                        if (v.MIMPND != null)
                            tu.EffectiveDate = ((DateTime)v.MIMPND).ToString("yyyy-MM-dd");
                        else tu.EffectiveDate = "";

                    }
                    else
                    {
                        tu.TierANew = "";
                        tu.TierAOld = "";
                        tu.EffectiveDate = "";
                    }
                    if (MIMPriceRowList.Where(o => o.MITIER.Equals("B")).Any())
                    {
                        var v = MIMPriceRowList.Single(o => o.MITIER.Equals("B"));

                        if (v.MIMNPI != null)
                            tu.TierBNew = v.MIMNPI.ToString();
                        else tu.TierBNew = "";

                        if (v.MIMPRI != null)
                            tu.TierBOld = v.MIMPRI.ToString();
                        else tu.TierBOld = "";
                    }
                    else
                    {
                        tu.TierBNew = "";
                        tu.TierBOld = "";
                    }

                    if (MIMPriceRowList.Where(o => o.MITIER.Equals("C")).Any())
                    {
                        var v = MIMPriceRowList.Single(o => o.MITIER.Equals("C"));

                        if (v.MIMNPI != null)
                            tu.TierCNew = v.MIMNPI.ToString();
                        else tu.TierCNew = "";

                        if (v.MIMPRI != null)
                            tu.TierCOld = v.MIMPRI.ToString();
                        else tu.TierCOld = "";
                    }
                    else
                    {
                        tu.TierCNew = "";
                        tu.TierCOld = "";
                    }
                    return tu;
                }
                else return null;
            }
        }

        public static ReportViewModel ImportExcel(Stream file, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                ReportViewModel error = new ReportViewModel();

                XLWorkbook workBook;
                try
                {
                    workBook = new XLWorkbook(file);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    error.Result = false;
                    error.Message = "File format not supported";
                    error.ErrorLevel = 3;
                    return error;
                }

                IXLWorksheet workSheet = workBook.Worksheet(1);
                var MIMRowList = new List<CSHMIMP0>();
                bool IsFirstRow = true;
                int succesfulRows = 0;
                int blankCounter = 0;
                IXLRow FirstRow = workSheet.Rows().ElementAt(0);
                if (FirstRow == null || FirstRow.CellCount() <= 0)
                {
                    error.Result = false;
                    error.ErrorLevel = 3;
                    error.Message = "File has incorrect or unsupported format";
                    return error;
                }
                int index = 1;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (row == null)
                        break;

                    if (IsFirstRow)
                    {
                        FirstRow = row;
                        IsFirstRow = false;
                    }
                    else
                    {
                        if (row.Cells() == null || row.CellCount() <= 0)
                            break;

                        CSHMIMP0 MIMRow = new CSHMIMP0();
                        int errorLevel = 0;
                        List<string> tierName = new List<string>();
                        List<double> tier = new List<double>();
                        for (int i = 1; i < row.CellCount(); i++)
                        {
                            System.Diagnostics.Debug.WriteLine("Cell count: " + i);
                            System.Diagnostics.Debug.WriteLine("Cell header: " + FirstRow.Cell(i).Value.ToString().ToUpper());
                            System.Diagnostics.Debug.WriteLine("Cell data: " + row.Cell(i).Value.ToString());
                            System.Diagnostics.Debug.WriteLine("Row: " + index);

                            if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMMIC") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MENU ITEM CODE") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MENU ITEM #"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMMIC = int.Parse(row.Cell(i).Value.ToString());
                                else
                                {
                                    error.Message += "[MIMMIC]/Menu item code [" + row.Cell(i).Value.ToString() + "] at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                    break;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMSTA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STATUS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMSTA = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Result = false;
                                    error.Message += "[MIMSTA]/Status [" + row.Cell(i).Value.ToString() + "]  at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMFGC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FAMILY GROUP CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMFGC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNAM") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MENU ITEM NAME"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNAM = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Message += "[MIMNAM]/Menu item name  [" + row.Cell(i).Value.ToString() + "] at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMDSC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MENU ITEM DESCRIPTION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMDSC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMSSC") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SUGGESTIVE SALES CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMSSC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMDPC") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DAYPART CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMDPC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMCIN") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ITEM LIST NUMBER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMCIN = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMDGC") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DISPLAY GROUPING CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMDGC = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMASC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ADD-ON SALES CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMASC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMTXC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("EMPLOYEE MEAL"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMTXC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMUTC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("US TAX CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMUTC = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMDWE") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DISPLAY FOR WASTE ENTRY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMDWE = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMTCI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("EAT-IN TAX CHAIN NR."))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMTCI = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMPRI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE 1 (EAT-IN)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMPRI = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMTCA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("TAKE-OUT TAX CHAIN NR."))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMTCA = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMPRO") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE 2 (TAKE-OUT)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMPRO = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMTCG") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("OTHER TAX CHAIN"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMTCG = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMPRG") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE 3 (OTHER)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMPRG = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMPND") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("EFFECTIVE DATE FOR NEW PRICE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMPND = DateTime.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMKBP") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("KEYBOARD POSITION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMKBP = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMKSC") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("KEYBOARD SHIFT CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMKSC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMSKT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SIZE KEY TABLE#"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMSKT = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMGRP") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("WINDOW GROUP #"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMGRP = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMWLV") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("WINDOW LEVEL #"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMWLV = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMWSD") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("WINDOW STAY/DOWN"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMWSD = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMWGR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FAMILY MAIN GROUP CODE 1"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMWGR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMHPT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FAMILY MAIN GROUP CODE 2"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMHPT = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMUSR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMUSR = row.Cell(i).Value.ToString();
                                else
                                    MIMRow.MIMUSR = user.ToUpper().Substring(0, 3);
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMDAT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DATE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    DateTime dt;
                                    bool res = DateTime.TryParse(row.Cell(i).Value.ToString(), out dt);
                                    if (res) MIMRow.MIMDAT = dt;
                                    System.Diagnostics.Debug.WriteLine("MIMDAT: " + dt);
                                }
                                else
                                    MIMRow.MIMDAT = DateTime.Now;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMEDT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("EFFECTIVE DATE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    DateTime dt;
                                    bool res = DateTime.TryParse(row.Cell(i).Value.ToString(), out dt);
                                    if (res) MIMRow.MIMEDT = dt;
                                    System.Diagnostics.Debug.WriteLine("MIMEDT: " + dt);
                                }
                                else
                                    MIMRow.MIMEDT = DateTime.Now;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMFLG") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CRITICAL TABLE UPDATE FIELD"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMFLG = bool.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBIN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BIN LEVEL CONTROL"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBIN = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBIT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BIN LEVEL TIME"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBIT = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBIR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BIN LEVEL ROUNDING"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBIR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBGR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BIN LEVEL GROUP"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBGR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBQU") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BIN LEVEL QUANTITY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBQU = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMGRA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE 2"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMGRA = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMIST") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ITEM STATUS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMIST = row.Cell(i).Value.ToString(); 
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMCLR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("COLOR FOR TOUCHSCREEN IFSA"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMCLR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNPI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE EAT-IN (NEW)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNPI = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNPO") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE TAKE-OUT (NEW)"))
                            {
                                if (row.Cell(i).Value != null)
                                    MIMRow.MIMNPO = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNPD") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE OTHER (NEW)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNPD = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMSKI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SIZE KEY INDEX"))
                            {
                                if (row.Cell(i).Value != null)
                                    MIMRow.MIMSKI = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMBMI") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ASSOCIATED BASE MENU ITEM"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMBMI = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNPA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("NONPRODUCT AMOUNT OF VALUE MEAL"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNPA = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNNP") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("NEW NONPRODUCT AMOUNT"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNNP = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMNPT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("NONPRODUCT TAX"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMNPT = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMLON") ||
                                     FirstRow.Cell(i).Value.ToString().ToUpper().Contains("LONG NAME") ||
                                     FirstRow.Cell(i).Value.ToString().ToUpper().Contains("LONGNAME"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    MIMRow.MIMLON = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMMIC_NP6") ||
                                     FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MENU ITEM CODE(NP6)"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    MIMRow.CSHMIMP0_NP6 = new CSHMIMP0_NP6();
                                    MIMRow.CSHMIMP0_NP6.MIMMIC = int.Parse(row.Cell(i).Value.ToString());
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMSTA_NP6") ||
                                     FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STATUS NP6"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    if (MIMRow.CSHMIMP0_NP6 != null)
                                        MIMRow.CSHMIMP0_NP6.MIMSTA = row.Cell(i).Value.ToString();
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MIMLON_NP6") ||
                                     FirstRow.Cell(i).Value.ToString().ToUpper().Contains("LONG NAME NP6"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    if (MIMRow.CSHMIMP0_NP6 != null)
                                        MIMRow.CSHMIMP0_NP6.MIMLON = row.Cell(i).Value.ToString();
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CATEGORY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    string cat = row.Cell(i).Value.ToString().ToUpper();
                                    if (db.Categories.Where(o => o.Category1.ToUpper().Equals(cat)).Any())
                                        MIMRow.Category = db.Categories.FirstOrDefault(o => o.Category1.ToUpper().Equals(cat)).Id;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("TRADING AREA"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    string cat = row.Cell(i).Value.ToString();
                                    if (db.Trading_Area.Where(o => o.Trading_Areea.ToUpper().Equals(cat.ToUpper())).Any())
                                        MIMRow.Trading_Area = db.Trading_Area.FirstOrDefault(o => o.Trading_Areea.ToUpper().Equals(cat.ToUpper())).Id;
                                }
                            }
                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                {
                                    string st = FirstRow.Cell(i).Value.ToString().ToUpper().Replace("TIER", "").Trim();
                                    if (st.Length > 5)
                                        st = st.Trim().Substring(0, 5);
                                    tierName.Add(st);
                                    tier.Add(double.Parse(row.Cell(i).Value.ToString()));
                                }
                            }
                            else if (FirstRow.Cell(i).Value == null ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains(""))
                            {
                                blankCounter++;
                                if (blankCounter > 20) break;
                                else continue;
                            }

                        }

                        if (MIMRow.MIMMIC == 0 || MIMRow.MIMNAM == null)
                        {
                            error.Result = false;
                            error.Message += "{Row " + index + "} has incorrect format | ";
                            errorLevel = 2;
                            break;
                        }

                        if (db.CSHMIMP0.Where(o => o.MIMMIC == MIMRow.MIMMIC).Any())
                        {
                            error.Result = false;
                            error.Message += "Menu item [" + MIMRow.MIMMIC + "] at {Row " + index + "} is already defined | ";
                            errorLevel = 2;
                        }

                        if (MIMRow.MIMFGC != null && !MIMRow.MIMFGC.Equals("") && !db.CSHPMGP0.Where(o => (o.PMGGRP.Equals("PMGFGC") && o.PMGNUM.Equals(MIMRow.MIMFGC))).Any())
                        {
                            error.Result = false;
                            error.Message += "[MIMFGC]/Family group code [" + MIMRow.MIMFGC + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (MIMRow.MIMHPT != null && !MIMRow.MIMHPT.Equals("") && !db.CSHPMGP0.Where(o => (o.PMGGRP.Equals("PMGHPT") && o.PMGNUM.Equals(MIMRow.MIMHPT))).Any())
                        {
                            error.Result = false;
                            error.Message += "[MIMHPT]/Family main group code 1 [" + MIMRow.MIMHPT + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (MIMRow.MIMWGR != null && !MIMRow.MIMWGR.Equals("") && !db.CSHPMGP0.Where(o => (o.PMGGRP.Equals("PMGWGR") && o.PMGNUM.Equals(MIMRow.MIMWGR))).Any())
                        {
                            error.Result = false;
                            error.Message += "[MIMWGR]/Family main group code 2 [" + MIMRow.MIMWGR + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (errorLevel >= 2)
                            error.Message += "{Row " + index + "} not inserted | ";

                        if (errorLevel < 2)
                        {
                            if (tier.Count() > 0)
                            {
                                for (int j = 0; j < tier.Count(); j++)
                                {
                                    MIM_Price MIM_Price = new MIM_Price();
                                    MIM_Price.Id = MIMRow.MIMMIC + tierName[j];
                                    MIM_Price.MIMMIC = MIMRow.MIMMIC;
                                    MIM_Price.MIMNAM = MIMRow.MIMNAM;
                                    MIM_Price.MITIER = tierName[j];
                                    MIM_Price.MIMPRI = tier[j];
                                    MIM_Price.MIMPND = MIMRow.MIMPND;
                                    db.MIM_Price.Add(MIM_Price);
                                    try
                                    {
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
                                        error.Result = false;
                                        error.Message += "Tier " + tierName[j] + " at {Row " + index + "} failed to insert. | \n";
                                        errorLevel = 2;
                                    }
                                }
                            }

                            MIMRowList.Add(MIMRow);
                            db.CSHMIMP0.Add(MIMRow);
                            try
                            {
                                db.SaveChanges();
                                // Special case for logging import 
                                succesfulRows++;
                                new AuditLogManager().Audit(user, DateTime.Now, "Menu Item Master", "Import", MIMRow.MIMMIC.ToString(), MIMRow.MIMNAM);
                                System.Diagnostics.Debug.WriteLine(MIMRow.MIMMIC);
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
                                error.Result = false;
                                error.Message += "{Row " + index + "} failed to insert. | \n";
                                if (error.ErrorLevel != 3) errorLevel = 2;
                            }
                        }
                        error.ErrorLevel = errorLevel;
                        index++;
                    }

                }
                if (succesfulRows <= 0)
                {
                    error.Result = false;
                    error.Message += "No rows imported | ";
                    error.ErrorLevel = 3;
                }
                else if (succesfulRows >= index)
                {
                    error.ErrorLevel = 0;
                    error.Result = true;
                }
                error.Message += "Imported " + index + " rows. ";
                return error;
            }
        }
    }
}