using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class RawItemMasterManager
    {
        public static bool UpdateRawItem(RawItemMasterViewModel RIMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                INVRIMP0 RIMRow;
                bool isUpdating;
                if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC)).Any())
                {
                    RIMRow = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC));
                    isUpdating = true;
                }
                else
                {
                    RIMRow = new INVRIMP0();
                    isUpdating = false;
                }

                if (RIMViewModel.RIMRIC != null)
                    RIMRow.RIMRIC = int.Parse(RIMViewModel.RIMRIC);
                else return false;

                if (RIMViewModel.RIMRID != null)
                    RIMRow.RIMRID = RIMViewModel.RIMRID.Trim();
                else return false;

                if (db.INVRIMP0.Where(o => (!o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC) && o.RIMRID.Equals(RIMViewModel.RIMRID))).Any())
                    return false;

                if (RIMViewModel.RIMRIG != null)
                    RIMRow.RIMRIG = RIMViewModel.RIMRIG.Trim();
                RIMRow.RIMPIS = RIMViewModel.RIMPIS.Trim();
                RIMRow.RIMBVP = RIMViewModel.RIMBVP.Trim();
                RIMRow.RIMBZP = RIMViewModel.RIMBZP.Trim();
                RIMRow.RIMUMC = RIMViewModel.RIMUMC.Trim();
                RIMRow.RIMUPC = double.Parse(RIMViewModel.RIMUPC);
                RIMRow.RIMSUQ = double.Parse(RIMViewModel.RIMSUQ);
                RIMRow.RIMLAY = int.Parse(RIMViewModel.RIMLAY);
                if (!RIMViewModel.RIMPVN.Equals("0"))
                    RIMRow.RIMPVN = int.Parse(RIMViewModel.RIMPVN);
                else
                    RIMRow.RIMPVN = 0;
                RIMRow.RIMCWC = RIMViewModel.RIMCWC.Trim();
                RIMRow.RIMPRO = RIMViewModel.RIMPRO.Trim();
                RIMRow.RIMSE4 = RIMViewModel.RIMSE4.Trim();
                RIMRow.RIMERT = RIMViewModel.RIMERT.Trim();
                if (RIMViewModel.RIMUSF != null)
                    RIMRow.RIMUSF = double.Parse(RIMViewModel.RIMUSF);
                if (RIMViewModel.RIMMSD != null)
                    RIMRow.RIMMSD = double.Parse(RIMViewModel.RIMMSD);
                if (RIMViewModel.RIMMSL != null)
                    RIMRow.RIMMSL = double.Parse(RIMViewModel.RIMMSL);
                RIMRow.RIMLA1 = RIMViewModel.RIMLA1.Trim();
                RIMRow.RIMLA2 = RIMViewModel.RIMLA2.Trim();
                RIMRow.RIMSTA = RIMViewModel.RIMSTA.Trim();
                if (RIMViewModel.RIMEDT != null)
                    RIMRow.RIMEDT = DateTime.Parse(RIMViewModel.RIMEDT);
                RIMRow.RIMORD = RIMViewModel.RIMORD.Trim();
                RIMRow.RIMADE = RIMViewModel.RIMADE.Trim();
                RIMRow.RIMBAR = RIMViewModel.RIMBAR.Trim();
                // Non-field Row with default values
                RIMRow.RIMUSR = user.Substring(0, 3).ToUpper();
                RIMRow.RIMDAT = DateTime.Now;
                // Group
                if (db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Any())
                {
                    if (RIMViewModel.Group == 0)
                    {
                        int val = db.ITMGRPs.FirstOrDefault(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Id;
                        ItemGroupManager.DeleteItem(val);
                    }
                    else
                    {
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Item_Code = int.Parse(RIMViewModel.RIMRIC);
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Item_Name = RIMViewModel.RIMRID;
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Group_Id = RIMViewModel.Group;
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(RIMViewModel.RIMRIC)).Group_Name = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == RIMViewModel.Group).Group_Name;
                    }
                }
                else
                {
                    if (RIMViewModel.Group != 0)
                    {
                        ItemGroupViewModel IGRow = new ItemGroupViewModel();
                        IGRow.GroupName = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == RIMViewModel.Group).Group_Name;
                        IGRow.GroupId = RIMViewModel.Group;
                        IGRow.ItemCode = int.Parse(RIMViewModel.RIMRIC);
                        IGRow.ItemName = RIMViewModel.RIMRID;
                        IGRow.ItemType = 2;
                        IGRow.GroupType = 2;
                        ItemGroupManager.UpdateGroup(IGRow);
                    }
                }
                RIMRow.Group = RIMViewModel.Group;
                // Location
                if (RIMViewModel.Location != null && !RIMViewModel.Equals(""))
                    RIMRow.Location = int.Parse(RIMViewModel.Location);
                RIMRow.Region = RIMViewModel.Region;
                RIMRow.Province = RIMViewModel.Province;
                RIMRow.City = RIMViewModel.City;
                // Store
                RIMRow.Store = RIMViewModel.StoreSelected;
                if (RIMViewModel.SelectAllCb)
                {
                    RIMRow.Store = "ALL";
                }
                if (RIMViewModel.SelectExceptCb)
                {
                    RIMRow.Store = "ALL";
                    RIMRow.Except_Store = RIMViewModel.StoreSelected.Trim();
                }
                else
                {
                    RIMRow.Except_Store = null;
                }
                // Store attributes
                if (RIMViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX != null && !RIMViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX.Equals("0"))
                    RIMRow.Store_Attrib = RIMViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX;

                if (RIMViewModel.FRESH_OR_FROZEN != null && !RIMViewModel.FRESH_OR_FROZEN.Equals("0"))
                    RIMRow.Store_Attrib = RIMViewModel.FRESH_OR_FROZEN;

                if (RIMViewModel.MCCORMICK_OR_GSF != null && !RIMViewModel.MCCORMICK_OR_GSF.Equals("0"))
                    RIMRow.Store_Attrib = RIMViewModel.MCCORMICK_OR_GSF;

                if (RIMViewModel.PAPER_OR_PLASTIC != null && !RIMViewModel.PAPER_OR_PLASTIC.Equals("0"))
                    RIMRow.Store_Attrib = RIMViewModel.PAPER_OR_PLASTIC;

                if (RIMViewModel.SIMPLOT_OR_MCCAIN != null && !RIMViewModel.SIMPLOT_OR_MCCAIN.Equals("0"))
                    RIMRow.Store_Attrib = RIMViewModel.SIMPLOT_OR_MCCAIN;
                
                // Default values assignment
                if (!isUpdating)
                {
                    RIMRow.RIMCPR = 0;
                    RIMRow.RIMCPN = 0;
                    RIMRow.RIMPDT = null;
                    RIMRow.RIMVPC = 0;
                    RIMRow.RIMTEM = "0";
                    RIMRow.RIMPGR = "";
                    RIMRow.RIMSVN = 0;
                    RIMRow.RIMSDP = null;
                    RIMRow.RIMUS1 = null; // System generated
                    RIMRow.RIMUS2 = null; // System generated
                    RIMRow.RIMUS3 = null; // System generated
                    RIMRow.RIMUS4 = null; // System generated
                    RIMRow.RIMUS5 = null; // System generated
                    RIMRow.RIMUSX = 0.0000;
                    RIMRow.RIMLP1 = null;
                    RIMRow.RIMLP2 = null;
                    RIMRow.RIMFLG = false;
                    RIMRow.RIMLIN = null; // Report line
                }

                // Vendor
                int i = 0;
                foreach (var vendor in RIMViewModel.VendorList)
                {
                    // If previous state is true and current state is false, perform deletion of vendor entry
                    if (RIMViewModel.PreviousVendorsSelectedList[i] && !RIMViewModel.VendorsSelectedList[i])
                    {
                        if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIMViewModel.RIMRIC + vendor.value)).Any())
                        {
                            RIM_VEM_Lookup RVLRow = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(RIMViewModel.RIMRIC + vendor.value));
                            db.RIM_VEM_Lookup.Remove(RVLRow);
                        }
                    } // if current state is false
                    else if (RIMViewModel.VendorsSelectedList[i])
                    {
                        RIM_VEM_Lookup RVLRow;
                        if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIMViewModel.RIMRIC + vendor.value)).Any())
                            RVLRow = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(RIMViewModel.RIMRIC + vendor.value));
                        else
                            RVLRow = new RIM_VEM_Lookup();
                        RVLRow.RIM_VEM_ID = RIMViewModel.RIMRIC + vendor.value;
                        RVLRow.RIMRID = RIMViewModel.RIMRID;
                        RVLRow.RIMRIC = int.Parse(RIMViewModel.RIMRIC);
                        RVLRow.VEMVEN = int.Parse(vendor.value);
                        RVLRow.VEMDS1 = vendor.text;
                        if (RIMViewModel.VendorCPR[i] != null && !RIMViewModel.VendorCPR[i].Equals(""))
                            RVLRow.RIMCPR = double.Parse(RIMViewModel.VendorCPR[i]);
                        if (RIMViewModel.VendorPUN[i] != null && !RIMViewModel.VendorPUN[i].Equals(""))
                            RVLRow.PPERUN = double.Parse(RIMViewModel.VendorPUN[i]);
                        if (RIMViewModel.VendorSCM[i] != null && !RIMViewModel.VendorSCM[i].Equals(""))
                            RVLRow.SCMCOD = double.Parse(RIMViewModel.VendorSCM[i]);
                        // Perform update
                        if (!db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Trim().Equals(RVLRow.RIM_VEM_ID.Trim())).Any())
                            db.RIM_VEM_Lookup.Add(RVLRow);
                    }
                    i++;
                }
                // If RIMRIC exists in the Table, perform an update
                if (isUpdating)
                {
                    RIMRow.STATUS = "E";
                }
                else
                {
                    RIMRow.STATUS = "A";
                    db.INVRIMP0.Add(RIMRow);
                }
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
                    return false;
                }
                return true;
            }
        }
        public static bool DeleteRawItem(RawItemMasterViewModel RIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                try
                {
                    if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC)).Any())
                    {
                        INVRIMP0 rowToRemove = db.INVRIMP0.FirstOrDefault(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC));
                        // Remove vendor
                        List<RIM_VEM_Lookup> RIVendor = db.RIM_VEM_Lookup.Where(o => o.RIMRIC == rowToRemove.RIMRIC).ToList();
                        db.RIM_VEM_Lookup.RemoveRange(RIVendor);
                        // Remove recipe
                        List<INVRIRP0> RIRecipe = db.INVRIRP0.Where(o => o.RIRRIC == rowToRemove.RIMRIC).ToList();
                        db.INVRIRP0.RemoveRange(RIRecipe);
                        // Remove Group
                        if (db.ITMGRPs.Where(o => o.Item_Code == rowToRemove.RIMRIC).Any())
                        {
                            db.ITMGRPs.RemoveRange(db.ITMGRPs.Where(o => o.Item_Code == rowToRemove.RIMRIC));
                        }

                        db.INVRIMP0.Remove(rowToRemove);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch ( Exception e )
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
         * Searches the table for any given SearchItem (Name or ID) in the ViewModel.
         * 
         * Returns List<ViewModel> if true, otherwise returns null
         */
        public static List<RawItem> GetRawItems(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItem> RIMList = new List<RawItem>();
                List<INVRIMP0> RIMRowList;
                if (SearchItem.ToUpper().Equals("ALL"))
                    RIMRowList = db.INVRIMP0.ToList();
                else if (db.INVRIMP0.Where(o => o.RIMRID.Contains(SearchItem)).Any())
                    RIMRowList = db.INVRIMP0.Where(o => o.RIMRID.Contains(SearchItem)).ToList();
                else if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(SearchItem)).Any())
                    RIMRowList = db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(SearchItem)).ToList();
                else
                    return RIMList;
                foreach (INVRIMP0 rim in RIMRowList)
                {
                    RawItem vm = new RawItem();
                    vm.RIMRIC = rim.RIMRIC.ToString();
                    vm.RIMRID = rim.RIMRID;
                    vm.RIMSTA = rim.RIMSTA;
                    RIMList.Add(vm);
                }
                if (RIMList == null || RIMList.ElementAt(0) == null)
                    return null;
                return RIMList;
            }
        }

        public static RawItemMasterViewModel SearchSingleRawItem(string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                INVRIMP0 rim;
                if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(SearchItem)).Any())
                    rim = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(SearchItem));
                else
                    return null;
                RawItemMasterViewModel vm = new RawItemMasterViewModel();
                vm.RIMRIC = rim.RIMRIC.ToString();
                vm.RIMRID = rim.RIMRID.Trim();
                vm.RIMRIG = rim.RIMRIG.Trim();
                vm.RIMPIS = rim.RIMPIS.Trim();
                vm.RIMBVP = rim.RIMBVP.Trim();
                vm.RIMBZP = rim.RIMBZP.Trim();
                vm.RIMUMC = rim.RIMUMC.Trim();
                vm.RIMUPC = rim.RIMUPC.ToString();
                vm.RIMSUQ = rim.RIMSUQ.ToString();
                vm.RIMLAY = rim.RIMLAY.ToString();
                vm.RIMPVN = rim.RIMPVN.ToString();
                vm.RIMCWC = rim.RIMCWC.ToString();
                vm.RIMPRO = rim.RIMPRO.Trim();
                vm.RIMSE4 = rim.RIMSE4.Trim();
                vm.RIMERT = rim.RIMERT.Trim();
                vm.RIMUSF = rim.RIMUSF.ToString();
                vm.RIMMSD = rim.RIMMSD.ToString();
                vm.RIMMSL = rim.RIMMSL.ToString();
                vm.RIMLA1 = rim.RIMLA1.Trim();
                vm.RIMLA2 = rim.RIMLA2.Trim();
                vm.RIMSTA = rim.RIMSTA.Trim();
                vm.RIMEDT = ((DateTime)rim.RIMEDT).ToString("yyyy-MM-dd");
                vm.RIMORD = rim.RIMORD.Trim();
                vm.RIMADE = rim.RIMADE.Trim();
                vm.RIMBAR = rim.RIMBAR.Trim();
                if (rim.Group != null)
                    vm.Group = (int)rim.Group;
                else vm.Group = 0;
                // Location
                vm.Location = rim.Location.ToString();
                vm.Region = rim.Region;
                vm.Province = rim.Province;
                vm.City = rim.City;
                // Store
                vm.StoreSelected = rim.Store;
                if ((rim.Store != null) && rim.Store.Equals("ALL"))
                {
                    vm.SelectAllCb = true;
                }
                if ((rim.Except_Store != null) && !(rim.Except_Store.Equals("")))
                {
                    vm.SelectExceptCb = true;
                    vm.SelectAllCb = false;
                    vm.StoreSelected = rim.Except_Store;
                }
                // Store attributes
                if (rim.Store_Attrib != null)
                {
                    if (rim.Store_Attrib.Equals("SOFTSERVE") || rim.Store_Attrib.Equals("VANILLA"))
                        vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX = rim.Store_Attrib;
                    else if (rim.Store_Attrib.Equals("FRESH") || rim.Store_Attrib.Equals("FROZEN"))
                        vm.FRESH_OR_FROZEN = rim.Store_Attrib;
                    else if (rim.Store_Attrib.Equals("SIMPLOT") || rim.Store_Attrib.Equals("MCCAIN"))
                        vm.SIMPLOT_OR_MCCAIN = rim.Store_Attrib;
                    else if (rim.Store_Attrib.Equals("PAPER") || rim.Store_Attrib.Equals("PLASTIC"))
                        vm.PAPER_OR_PLASTIC = rim.Store_Attrib;
                    else if (rim.Store_Attrib.Equals("MCORMICK") || rim.Store_Attrib.Equals("GSF"))
                        vm.MCCORMICK_OR_GSF = rim.Store_Attrib;
                    else if (rim.Store_Attrib.Equals("FRESHB") || rim.Store_Attrib.Equals("FROZENB"))
                        vm.FRESHB_OR_FROZENB = rim.Store_Attrib;
                }
                // Menu item list
                var MIList = db.INVRIRP0.Where(o => o.RIRRIC == rim.RIMRIC).ToList();
                foreach (var v in MIList)
                {
                    if (!db.CSHMIMP0.Where(o => o.MIMMIC == v.RIRMIC).Any())
                        continue;
                    MenuItem mi = new MenuItem();
                    mi.MIMMIC = v.RIRMIC.ToString();
                    mi.MIMDSC = db.CSHMIMP0.SingleOrDefault(o => o.MIMMIC == v.RIRMIC).MIMNAM;
                    vm.MenuItemList.Add(mi);
                }
                // Vendor List
                var rowList = db.RIM_VEM_Lookup.Where(o => o.RIMRIC == rim.RIMRIC);
                int i = 0;
                foreach (var v in vm.VendorList)
                {
                    if (rowList.Where(o => o.VEMVEN.ToString().Equals(v.value)).Any())
                    {
                        var row = rowList.Single(o => o.VEMVEN.ToString().Equals(v.value));
                        vm.VendorsSelectedList[i] = true;
                        vm.VendorCPR[i] = row.RIMCPR.ToString();
                        vm.VendorPUN[i] = row.PPERUN.ToString();
                        vm.VendorSCM[i] = row.SCMCOD.ToString();
                    }
                    else
                    {
                        vm.VendorsSelectedList[i] = false;
                        vm.VendorCPR[i] = string.Empty;
                        vm.VendorPUN[i] = string.Empty;
                        vm.VendorSCM[i] = string.Empty;
                    }
                    i++;
                }
                return vm;
            }
           
        }

        public static bool SwitchRawItem (string rawItem, string switchItem, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                RawItemMasterViewModel RIMViewModel;
                RawItemMasterViewModel SwRIMViewModel;
                List<MenuRecipeViewModel> RIRViewModel = new List<MenuRecipeViewModel>();
                List<MenuRecipeViewModel> SwRIRViewModel = new List<MenuRecipeViewModel>();
                string RIMRIC = "";
                string RIMRID = "";
                bool result = false;

                // Switch Raw item
                if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(rawItem)).Any())
                {
                    RIMViewModel = SearchSingleRawItem(rawItem);
                    RIMRIC = rawItem;
                    db.INVRIMP0.RemoveRange(db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(rawItem)));
                }
                else return false;
                if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(switchItem)).Any())
                {
                    SwRIMViewModel = SearchSingleRawItem(switchItem);
                    RIMRID = db.INVRIMP0.SingleOrDefault(o => o.RIMRIC.ToString().Equals(switchItem)).RIMRID;
                    db.INVRIMP0.RemoveRange(db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(switchItem)));
                }
                else return false;

                // Switch Vendor
                if (db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(rawItem)).Any())
                {
                    List<RIM_VEM_Lookup> RVLRow = db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(rawItem)).ToList();
                    db.RIM_VEM_Lookup.RemoveRange(RVLRow);
                }

                if (db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(switchItem)).Any())
                {
                    List<RIM_VEM_Lookup> RVLRow = db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(switchItem)).ToList();
                    db.RIM_VEM_Lookup.RemoveRange(RVLRow);
                }

                // Switch Recipe
                if (db.INVRIRP0.Where(o => o.RIRRIC.ToString().Equals(rawItem)).Any())
                {
                    List<INVRIRP0> RIRRow = db.INVRIRP0.Where(o => o.RIRRIC.ToString().Equals(rawItem)).ToList();
                    for (int i = 0; i < RIRRow.Count(); i++)
                    {
                        RIRViewModel.Add(MenuRecipeManager.SearchMenuRecipe(RIRRow[i].RIRMIC.ToString()));
                    }
                    db.INVRIRP0.RemoveRange(RIRRow);
                }
                if (db.INVRIRP0.Where(o => o.RIRRIC.ToString().Equals(switchItem)).Any())
                {
                    List<INVRIRP0> RIRRow = db.INVRIRP0.Where(o => o.RIRRIC.ToString().Equals(switchItem)).ToList();
                    for (int i = 0; i < RIRRow.Count(); i++)
                    {
                        SwRIRViewModel.Add(MenuRecipeManager.SearchMenuRecipe(RIRRow[i].RIRMIC.ToString()));
                        foreach (var v in SwRIRViewModel[i].MenuRecipeList)
                        {
                            v.RIRRIC = RIMRIC;
                            v.RIMRID = SwRIRViewModel[i].RIRMIC + v.RIRRIC;
                            v.PreviousRIRRIC = RIMRIC;
                        }
                        db.INVRIRP0.Remove(RIRRow[i]);
                    }
                }
                // Switch Group
                if (db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(rawItem)).Any())
                {
                    db.ITMGRPs.RemoveRange(db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(rawItem)));
                }
                if (db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(switchItem)).Any())
                {
                    List<ITMGRP> IGRows = db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(switchItem)).ToList();
                    foreach(var v in IGRows)
                    {
                        v.Item_Code = int.Parse(RIMRIC);
                        v.Item_Name = RIMRID;
                    }
                }

                try
                {
                    db.SaveChanges();
                    SwRIMViewModel.RIMRIC = RIMViewModel.RIMRIC;
                    SwRIMViewModel.RIMRID = RIMViewModel.RIMRID;
                    result = UpdateRawItem(SwRIMViewModel, user);
                    if (!result) return result;

                    if (SwRIRViewModel.Count() > 0)
                    {
                        foreach (var v in SwRIRViewModel)
                        {
                            result = MenuRecipeManager.UpdateMenuItem(v, user);
                            if (!result) return result;
                        }
                    }
                }
                catch(Exception e)
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
                var RIMRowList = new List<INVRIMP0>();
                bool IsFirstRow = true;
                IXLRow FirstRow = workSheet.Rows().ElementAt(0);
                if (FirstRow == null || FirstRow.CellCount() <= 0)
                {
                    error.Result = false;
                    error.ErrorLevel = 3;
                    error.Message = "File has incorrect or unsupported format";
                    return error;
                }
                int index = 1;
                int succesfulRows = 0;
                int blankCounter = 0;
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

                        INVRIMP0 vm = new INVRIMP0();
                        int errorLevel = 0;
                        for (int i = 1; i < row.CellCount(); i++)
                        {

                            if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMRIC") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMRIC = int.Parse(row.Cell(i).Value.ToString());
                                else
                                {
                                    error.Message += "[RIMRIC]/Raw item code at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMVPC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM PACKAGE NUMBER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMVPC = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMRID") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM DESCRIPTION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMRID = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Message += "[RIMRID]/Raw item description at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMRIG") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM GROUP CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMRIG = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMTEM") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM TEMPERATURE CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMTEM = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMPGR") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW ITEM WRIN PRODUCT CODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMPGR = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMPIS") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("INVENTORY PERIOD"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMPIS = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMBVP") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PACKAGE UNIT"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMBVP = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMBZP") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SLAVE UNIT"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMBZP = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUMC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USAGE UNIT"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUMC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUPC") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USAGE UNITS PER PACKAGE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUPC = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMSUQ") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SLAVE UNITS PER PACKAGE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMSUQ = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLAY") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SHIPPING UNITS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLAY = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMCPR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE PER PACKAGE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMCPR = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMCPN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRICE PER PACKAGE NEW"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMCPN = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMPDT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DATE FOR PRICE CHANGE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMPDT =  DateTime.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMPVN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRIMARY VENDOR"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMPVN = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMSVN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SECONDARY VENDOR"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMSVN = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMCWC") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW WASTE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMCWC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMPRO") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RAW PROMO"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMPRO = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMSE4") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RECIPE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMSE4 = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMERT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("YIELD CALCULATION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMERT = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUSF") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("YIELD"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUSF = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMSDP") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DEV. YIELD"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMSDP = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUS1") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USAGE PER TAUSEND / MONTH"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUS1 = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUS2") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USAGE PER TAUSEND / WEEK"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUS2 = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUS3") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USAGE PER TAUSEND / DAY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUS3 = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUS4") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("INVENTORY USAGE LAST MONTH"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUS4 = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUS5") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("INVENTORY USAGE CURRENT MONTH"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUS5 = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUSX") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FIX USAGE FACTOR"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUSX = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMMSD") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MINIMUM DAYS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMMSD = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMMSL") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MINIMUM STOCK LEVEL"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMMSL = double.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLA1") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("WAREHOUSE 1"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLA1 = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLA2") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("WAREHOUSE 2"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLA2 = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLP1") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE 1"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLP1 = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLP2") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE 2"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLP2 = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMSTA") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STATUS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMSTA = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Result = false;
                                    error.Message += "[RIMSTA]/Status at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMUSR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("USER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMUSR = row.Cell(i).Value.ToString();
                                else
                                    vm.RIMUSR = user.ToUpper().Substring(0, 3);
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMDAT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DATE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMDAT = DateTime.Parse(row.Cell(i).Value.ToString());
                                else
                                    vm.RIMDAT = DateTime.Now;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMEDT") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("EFFECTIVE DATE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMEDT = DateTime.Parse(row.Cell(i).Value.ToString());
                                else
                                {
                                    error.Result = false;
                                    error.Message += "[RIMEDT]/Effective date at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMFLG") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CRITICAL TABLE UPDATE FIELD"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMFLG = bool.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMORD") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ORDERING MATERIAL"))
                            {
                                if (row.Cell(i).Value != null )
                                    vm.RIMORD = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMLIN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PRINTOUT LINE OF TAX REPORT"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMLIN = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMADE") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("AUTO DELIVERY ITEM"))
                            {
                                if (row.Cell(i).Value != null)
                                    vm.RIMADE = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Contains("RIMBAR") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BARCODE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    vm.RIMBAR = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Result = false;
                                    error.Message += "[RIMBAR]/Barcode at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
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
                        if (vm.RIMRIC == 0 || vm.RIMRID == null)
                        {
                            error.Result = false;
                            error.Message += "{Row " + index + "} has incorrect format | ";
                            errorLevel = 2;
                            break;
                        }

                        if (db.INVRIMP0.Where(o => o.RIMRIC == vm.RIMRIC).Any())
                        {
                            error.Result = false;
                            error.Message += "Raw item " + vm.RIMRIC + " at {Row " + index + "} is already defined | ";
                            errorLevel = 2;
                        }
                        if (!db.INVVEMP0.Where(o => o.VEMVEN == vm.RIMPVN).Any())
                        {
                            error.Result = false;
                            error.Message += "[RIMPVN]/Primary vendor " + vm.RIMPVN +" at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (errorLevel == 2)
                            error.Message += "{Row " + index + "} not inserted | ";

                        if (errorLevel < 2)
                        {
                            RIMRowList.Add(vm);
                            db.INVRIMP0.Add(vm);

                            try
                            {
                                db.SaveChanges();// Special case for logging import 
                                new AuditLogManager().Audit(user, DateTime.Now, "Raw Item Master", "Import", vm.RIMRIC.ToString(), vm.RIMRID);
                                succesfulRows++;
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
                                error.Message += "{Row " + index + "} failed to insert." + Environment.NewLine;
                                errorLevel = 2;
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
                return error;
            }
        }
    }
}