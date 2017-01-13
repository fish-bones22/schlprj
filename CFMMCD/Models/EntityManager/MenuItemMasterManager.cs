using CFMMCD.DropDown;
using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public List<MenuItem> SearchMenuItems(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHMIMP0> MIMRowList;
                List<MenuItem> MIMList = new List<MenuItem>();
                string ItemCode = "";
                if (SearchItem.ToUpper().Equals("ALL"))
                {
                    MIMRowList = db.CSHMIMP0.ToList();
                }
                else if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(SearchItem)).ToList();
                    ItemCode = SearchItem;
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
                    vm.RIRMIC = MIMRow.MIMMIC.ToString();
                    vm.MIMDSC = MIMRow.MIMNAM;
                    vm.MIMSTA = MIMRow.MIMSTA;
                    MIMList.Add(vm);
                }
               if (MIMList == null || MIMList.Count() == 0 )
                    return null;
                return MIMList;
            }
        }
        public MenuItemMasterViewModel SearchSingleMenuItem(string SearchItem)
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
                vm.MIMFGC = MIMRow.MIMFGC.Trim();
                vm.MIMNAM = MIMRow.MIMNAM.Trim();
                vm.MIMDSC = MIMRow.MIMDSC.Trim();
                vm.MIMDPC = MIMRow.MIMDPC.Trim();
                vm.MIMTCI = MIMRow.MIMTCI.Trim();
                vm.MIMPRI = MIMRow.MIMPRI.ToString();
                vm.MIMTCA = MIMRow.MIMTCA.Trim();
                vm.MIMPRO = MIMRow.MIMPRO.ToString();
                vm.MIMTCG = MIMRow.MIMTCG.Trim();
                vm.MIMPRG = MIMRow.MIMPRG.ToString();
                vm.MIMPND = Convert.ToDateTime(MIMRow.MIMPND).ToString("yyyy-MM-dd");
                vm.MIMWGR = MIMRow.MIMWGR.Trim();
                vm.MIMHPT = MIMRow.MIMHPT.Trim();
                vm.MIMEDT = Convert.ToDateTime(MIMRow.MIMEDT).ToString("yyyy-MM-dd");
                vm.MIMNPI = MIMRow.MIMNPI.ToString();
                vm.MIMNPO = MIMRow.MIMNPO.ToString();
                vm.MIMNPD = MIMRow.MIMNPD.ToString();
                vm.MIMNPA = MIMRow.MIMNPA.ToString();
                vm.MIMNNP = MIMRow.MIMNNP.ToString();
                vm.MIMNPT = MIMRow.MIMNPT.Trim();
                vm.MIMLON = MIMRow.MIMLON.Trim();
                vm.MIMUTC = MIMRow.MIMUTC.ToString();

                vm.Trading_Area = MIMRow.Trading_Area.ToString();
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
                // Initialize external table row values
                if (ItemCode.Equals(""))
                    ItemCode = MIMRow.MIMMIC.ToString();
                // Price tier
                vm.TierList = SearchPriceTier(vm.MIMMIC);
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
        public bool UpdateMenuItem(MenuItemMasterViewModel MIMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                // Columns with Input fields
                if (MIMViewModel.MIMMIC != null)
                    MIMRow.MIMMIC = int.Parse(MIMViewModel.MIMMIC);
                else return false;
                MIMRow.MIMSTA = MIMViewModel.MIMSTA.Trim();
                MIMRow.MIMFGC = MIMViewModel.MIMFGC.Trim();
                if (MIMViewModel.MIMNAM != null)
                    MIMRow.MIMNAM = MIMViewModel.MIMNAM.Trim();
                else return false;
                if (MIMViewModel.MIMDSC != null)
                    MIMRow.MIMDSC = MIMViewModel.MIMDSC.Trim();
                MIMRow.MIMDPC = MIMViewModel.MIMDPC.Trim();
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
                if (MIMViewModel.Location != null && MIMViewModel.Location.Equals(""))
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
                // Try...Catch to produce appropriate warnings if ever
                // insertion is unsuccessful
                try
                {
                    // If MIMMIC is already existing, update it instead of inserting a new row
                    if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                    {
                        var rowToUpdate = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));;
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
        public bool DeleteMenuItem(MenuItemMasterViewModel MIMViewModel)
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
                    foreach (INVRIRP0 ri in MIRecipe)
                    {
                        db.INVRIRP0.Remove(ri);
                    }
                    // Delete MIM_Price
                    if (db.MIM_Price.Where(o => o.MIMMIC == MIMRow.MIMMIC).Any())
                    {
                        List<MIM_Price> MIMPrice = db.MIM_Price.Where(o => o.MIMMIC == MIMRow.MIMMIC).ToList();
                        foreach (MIM_Price mp in MIMPrice)
                        {
                            db.MIM_Price.Remove(mp);
                        }
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
        public bool UpdatePriceTier(List<Tier> TierList)
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

                    try
                    {
                        if (!IsUpdate)
                        {
                            db.MIM_Price.Add(MIMPriceRow);
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
        
        public List<Tier> SearchPriceTier(string MIMMIC)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MIM_Price> MIMPriceList;
                List<Tier> TierList = new TierManager().SetTierList();
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

        public bool UpdatePriceTier(List<TierUpdate> TierList)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                for (int i = 0; i < TierList.Count(); i++)
                {
                    List<Tier> TierListToUpdate = SearchPriceTier(TierList[i].MIMMIC);
                    if (TierListToUpdate == null)
                        return false;
                    if (TierListToUpdate.Where(o => o.TierName.Equals("A")).Any())
                    {
                        TierListToUpdate.Single(o => o.TierName.Equals("A")).MIMNPI = TierList[i].TierANew;
                        TierListToUpdate.Single(o => o.TierName.Equals("A")).MIMPND = TierList[i].EffectiveDate;
                    }
                    if (TierListToUpdate.Where(o => o.TierName.Equals("B")).Any())
                    {
                        TierListToUpdate.Single(o => o.TierName.Equals("B")).MIMNPI = TierList[i].TierBNew;
                        TierListToUpdate.Single(o => o.TierName.Equals("B")).MIMPND = TierList[i].EffectiveDate;
                    }
                    if (TierListToUpdate.Where(o => o.TierName.Equals("C")).Any())
                    {
                        TierListToUpdate.Single(o => o.TierName.Equals("C")).MIMNPI = TierList[i].TierCNew;
                        TierListToUpdate.Single(o => o.TierName.Equals("C")).MIMPND = TierList[i].EffectiveDate;
                    }
                    if (!UpdatePriceTier(TierListToUpdate))
                        return false;
                }
                return true;
            }
        }

        public TierUpdate SearchPriceTierUpdate (string MIMMIC)
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
    }
}