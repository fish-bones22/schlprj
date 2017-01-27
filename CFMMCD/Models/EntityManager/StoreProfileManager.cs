using CFMMCD.DropDown;
using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static CFMMCD.Models.ViewModel.StoreProfileViewModel;

namespace CFMMCD.Models.EntityManager
{
    public class StoreProfileManager
    {
        /*
         * Combined Create and Update Store profile method.
         * Creates a Store_Profile instance (which will be a new table row)
         * and instantiates each SPViewModel property to the respective property of the former.
         * Also checks if the given Store profile number is already in the table,
         * if true, the method performs an update, otherwise, creation. 
         *
         * Returns true if the operation is successful.
         * */
        public bool UpdateStore(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                bool isUpdating = false;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO)).Any())
                {
                    SPRow = db.Store_Profile.Single(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));
                    isUpdating = true;
                }
                else
                    SPRow = new Store_Profile();

                if (SPViewModel.STORE_NO != null && !SPViewModel.STORE_NO.Equals(""))
                    SPRow.STORE_NO = int.Parse(SPViewModel.STORE_NO);
                else return false;

                if (SPViewModel.STORE_NAME != null && !SPViewModel.STORE_NAME.Equals(""))
                    SPRow.STORE_NAME = SPViewModel.STORE_NAME;
                else return false;

                if (SPViewModel.OWNERSHIP != null)
                    SPRow.OWNERSHIP = int.Parse(SPViewModel.OWNERSHIP);
                if (SPViewModel.BREAKFAST_PRICE_TIER != null)
                    SPRow.BREAKFAST_PRICE_TIER = int.Parse(SPViewModel.BREAKFAST_PRICE_TIER);
                if (SPViewModel.REGULAR_PRICE_TIER != null)
                    SPRow.REGULAR_PRICE_TIER = int.Parse(SPViewModel.REGULAR_PRICE_TIER);
                if (SPViewModel.DC_PRICE_TIER != null)
                    SPRow.DC_PRICE_TIER = int.Parse(SPViewModel.DC_PRICE_TIER);
                if (SPViewModel.MDS_PRICE_TIER != null)
                    SPRow.MDS_PRICE_TIER = int.Parse(SPViewModel.MDS_PRICE_TIER);
                if (SPViewModel.MCCAFE_LEVEL_2_PRICE_TIER!= null)
                    SPRow.MCCAFE_LEVEL_2_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_LEVEL_2_PRICE_TIER);
                if (SPViewModel.MCCAFE_LEVEL_3_PRICE_TIER != null)
                    SPRow.MCCAFE_LEVEL_3_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_LEVEL_3_PRICE_TIER);
                if (SPViewModel.MCCAFE_BISTRO_PRICE_TIER != null)
                    SPRow.MCCAFE_BISTRO_PRICE_TIER = int.Parse(SPViewModel.MCCAFE_BISTRO_PRICE_TIER);
                if (SPViewModel.PROJECT_GOLD_PRICE_TIER != null)
                    SPRow.PROJECT_GOLD_PRICE_TIER = int.Parse(SPViewModel.PROJECT_GOLD_PRICE_TIER);
                if (SPViewModel.BusinessExtList != null)
                    SPRow.BET = SetBusinessExtention(SPViewModel.BET, SPViewModel.BusinessExtList);
                if (SPViewModel.PROFIT_CENTER != null)
                    SPRow.PROFIT_CENTER = int.Parse(SPViewModel.PROFIT_CENTER);
                if (SPViewModel.REGION != null)
                    SPRow.REGION = SPViewModel.REGION;
                if (SPViewModel.PROVINCE != null)
                    SPRow.PROVINCE = SPViewModel.PROVINCE;
                if (SPViewModel.LOCATION != null && !SPViewModel.LOCATION.Equals(""))
                    SPRow.LOCATION = int.Parse(SPViewModel.LOCATION);
                if (SPViewModel.ADDRESS != null)
                    SPRow.ADDRESS = SPViewModel.ADDRESS;
                if (SPViewModel.CITY != null)
                    SPRow.CITY = SPViewModel.CITY;
                if (SPViewModel.FRESH_OR_FROZEN != null)
                    SPRow.FRESH_OR_FROZEN = SPViewModel.FRESH_OR_FROZEN;
                if (SPViewModel.PAPER_OR_PLASTIC != null)
                    SPRow.PAPER_OR_PLASTIC = SPViewModel.PAPER_OR_PLASTIC;
                if (SPViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX != null)
                    SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                if (SPViewModel.SIMPLOT_OR_MCCAIN != null)
                    SPRow.SIMPLOT_OR_MCCAIN = SPViewModel.SIMPLOT_OR_MCCAIN;
                if (SPViewModel.MCCORMICK_OR_GSF != null)
                    SPRow.MCCORMICK_OR_GSF = SPViewModel.MCCORMICK_OR_GSF;
                if (SPViewModel.FRESHB_OR_FROZENB != null)
                    SPRow.FRESHB_OR_FROZENB = SPViewModel.FRESHB_OR_FROZENB;
                SPRow.STATUS = "A";
                // Group
                if (db.ITMGRPs.Where(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Any())
                {
                    if (SPViewModel.Group == 0)
                    {
                        int val = db.ITMGRPs.FirstOrDefault(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Id;
                        ItemGroupManager.DeleteItem(val);
                    }
                    else
                    {
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Item_Code = int.Parse(SPViewModel.STORE_NO);
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Item_Name = SPViewModel.STORE_NAME;
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Group_Id = SPViewModel.Group;
                        db.ITMGRPs.Single(o => o.Item_Code.ToString().Equals(SPViewModel.STORE_NO)).Group_Name = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == SPViewModel.Group).Group_Name;
                    }
                }
                else
                {
                    if (SPViewModel.Group != 0)
                    {
                        ItemGroupViewModel IGRow = new ItemGroupViewModel();
                        IGRow.GroupName = db.ITMGRPs.FirstOrDefault(o => o.Group_Id == SPViewModel.Group).Group_Name;
                        IGRow.GroupId = SPViewModel.Group;
                        IGRow.ItemCode = int.Parse(SPViewModel.STORE_NO);
                        IGRow.ItemName = SPViewModel.STORE_NAME;
                        IGRow.ItemType = 3;
                        IGRow.GroupType = 3;
                        ItemGroupManager.UpdateGroup(IGRow);
                    }
                }
                SPRow.Group = SPViewModel.Group;
                try
                {
                    // Check if STORE_NO already exists in the database, perform an update if true
                    if (isUpdating)
                    {
                        SPRow.STATUS = "E";
                    }
                    else
                        db.Store_Profile.Add(SPRow);
                    db.SaveChanges();
                    return true;
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
         * Deletes a row in the `Store Profile` table 
         * 
         * Returns true if operation is successful.
         */
         public bool DeleteStore(StoreProfileViewModel SPViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SPViewModel.STORE_NO)).Any())
                {
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SPViewModel.STORE_NO));

                    // Remove from Menu Items
                    //      Remove from `Store`
                    if (db.CSHMIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<CSHMIMP0> MIRow = db.CSHMIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.CSHMIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<CSHMIMP0> MIRow = db.CSHMIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    // Remove from Raw Items
                    //      Remove from `Store`
                    if (db.INVRIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVRIMP0> MIRow = db.INVRIMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.INVRIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVRIMP0> MIRow = db.INVRIMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    // Remove from Vendors
                    //      Remove from `Store`
                    if (db.INVVEMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVVEMP0> MIRow = db.INVVEMP0.Where(o => o.Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    //      Remove from `Except_Store`
                    if (db.INVVEMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).Any())
                    {
                        List<INVVEMP0> MIRow = db.INVVEMP0.Where(o => o.Except_Store.Equals(SPViewModel.STORE_NO)).ToList();
                        foreach (var v in MIRow)
                        {
                            v.Store = null;
                        }
                    }
                    // Delete Group
                    if (db.ITMGRPs.Where(o => o.Item_Code == SPRow.STORE_NO).Any())
                    {
                        db.ITMGRPs.RemoveRange(db.ITMGRPs.Where(o => o.Item_Code == SPRow.STORE_NO));
                    }
                }
                else
                    return false;

                try
                {
                    db.Store_Profile.Remove(SPRow);
                    db.SaveChanges();
                    return true;
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
         * Searches for the StoreNumber/StoreName from the table
         */
         public List<Store> SearchStores(string SearchItem)
         {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Store_Profile> SPRowList;
                List<Store> SPList = new List<Store>();
                if (SearchItem.ToUpper().Equals("ALL"))
                    SPRowList = db.Store_Profile.ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NAME.Contains(SearchItem)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NAME.Contains(SearchItem)).ToList();
                else if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SearchItem)).Any())
                    SPRowList = db.Store_Profile.Where(sp => sp.STORE_NO.ToString().Equals(SearchItem)).ToList();
                else
                    return null; // Empty
                foreach ( Store_Profile sp in SPRowList )
                {
                    Store st = new Store();
                    st.Store_No = sp.STORE_NO.ToString();
                    st.Store_Name = sp.STORE_NAME;
                    SPList.Add(st);
                }
                if (SPList == null || SPList.ElementAt(0) == null)
                    return null;
                return SPList;
            }
        }

        public StoreProfileViewModel SearchSingleStore (string SearchItem)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile SPRow;
                if (db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(SearchItem)).Any())
                    SPRow = db.Store_Profile.Single(sp => sp.STORE_NO.ToString().Equals(SearchItem));
                else return null;

                StoreProfileViewModel vm = new StoreProfileViewModel();
                vm.STORE_NO = SPRow.STORE_NO.ToString().Trim();

                if (SPRow.STORE_NAME != null)
                    vm.STORE_NAME = SPRow.STORE_NAME.Trim();

                if (SPRow.OWNERSHIP != null)
                    vm.OWNERSHIP = SPRow.OWNERSHIP.ToString();

                if (SPRow.BREAKFAST_PRICE_TIER != null)
                    vm.BREAKFAST_PRICE_TIER = SPRow.BREAKFAST_PRICE_TIER.ToString();

                if (SPRow.REGULAR_PRICE_TIER != null)
                    vm.REGULAR_PRICE_TIER = SPRow.REGULAR_PRICE_TIER.ToString();

                if (SPRow.DC_PRICE_TIER != null)
                    vm.DC_PRICE_TIER = SPRow.DC_PRICE_TIER.ToString();

                if (SPRow.MDS_PRICE_TIER != null)
                    vm.MDS_PRICE_TIER = SPRow.MDS_PRICE_TIER.ToString();

                if (SPRow.MCCAFE_LEVEL_2_PRICE_TIER != null)
                    vm.MCCAFE_LEVEL_2_PRICE_TIER = SPRow.MCCAFE_LEVEL_2_PRICE_TIER.ToString();

                if (SPRow.MCCAFE_LEVEL_3_PRICE_TIER != null)
                    vm.MCCAFE_LEVEL_3_PRICE_TIER = SPRow.MCCAFE_LEVEL_3_PRICE_TIER.ToString();

                if (SPRow.MCCAFE_BISTRO_PRICE_TIER != null)
                    vm.MCCAFE_BISTRO_PRICE_TIER = SPRow.MCCAFE_BISTRO_PRICE_TIER.ToString();

                if (SPRow.PROJECT_GOLD_PRICE_TIER != null)
                    vm.PROJECT_GOLD_PRICE_TIER = SPRow.PROJECT_GOLD_PRICE_TIER.ToString();

                if (SPRow.PROFIT_CENTER != null)
                    vm.PROFIT_CENTER = SPRow.PROFIT_CENTER.ToString();

                if (SPRow.REGION != null)
                    vm.REGION = SPRow.REGION.Trim();

                if (SPRow.PROVINCE != null)
                    vm.PROVINCE = SPRow.PROVINCE.Trim();

                if (SPRow.LOCATION != null)
                    vm.LOCATION = SPRow.LOCATION.ToString();

                if (SPRow.ADDRESS != null)
                    vm.ADDRESS = SPRow.ADDRESS.Trim();

                if (SPRow.CITY != null)
                    vm.CITY = SPRow.CITY.Trim();

                if (SPRow.FRESH_OR_FROZEN != null)
                    vm.FRESH_OR_FROZEN = SPRow.FRESH_OR_FROZEN.Trim();

                if (SPRow.PAPER_OR_PLASTIC != null)
                    vm.PAPER_OR_PLASTIC = SPRow.PAPER_OR_PLASTIC.Trim();

                if (SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX != null)
                    vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX = SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX.Trim();

                if (SPRow.SIMPLOT_OR_MCCAIN != null)
                    vm.SIMPLOT_OR_MCCAIN = SPRow.SIMPLOT_OR_MCCAIN.Trim();

                if (SPRow.MCCORMICK_OR_GSF != null)
                    vm.MCCORMICK_OR_GSF = SPRow.MCCORMICK_OR_GSF.Trim();

                if (SPRow.FRESHB_OR_FROZENB != null)
                    vm.FRESHB_OR_FROZENB = SPRow.FRESHB_OR_FROZENB;

                if (SPRow.BET != null)
                    vm.BET = GetBusinessExtension(SPRow.BET, vm.BusinessExtList);

                if (SPRow.Group != null)
                    vm.Group = (int)SPRow.Group;

                else vm.Group = 0;
                return vm;
            }
        }
        /*
         * Creates a List of bool from the string of 
         * selected Business Extensions
         * to be used in the View.
         */
        private List<bool> GetBusinessExtension ( string stArr, List<CheckBoxList> lookUpList )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                int capacity = lookUpList.Count();
                bool[] BEArr = new bool[capacity];
                string[] initArr = stArr.Split(',');
                for (int i = 0; i < capacity; i++)
                {
                    int index = int.Parse(lookUpList[i].value);
                    if (initArr.Contains(lookUpList[i].value))
                    {
                        BEArr[i] = true;
                    } else BEArr[i] = false;
                }
                return BEArr.ToList();
            }
        }
        /*
         * Creates a string of bool delimited by ',' 
         * that contains the selected Business Extensions
         * to be inserted in DB
         */
        private string SetBusinessExtention ( List<bool> boolArr, List<CheckBoxList> lookUpList )
        {
            string st = "";
            for (int i = 0; i < boolArr.Count(); i++ )
                if (boolArr[i])
                    st += lookUpList[i].value + ",";
            System.Diagnostics.Debug.WriteLine("BET (set):" + st);
            if (st.Length <= 0)
                return st;
            return st.Substring(0, st.Length - 1);
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

                        Store_Profile SPRow = new Store_Profile();
                        int errorLevel = 0;
                        for (int i = 1; i < row.CellCount(); i++)
                        {
                            System.Diagnostics.Debug.WriteLine("Cell count: " + i);
                            System.Diagnostics.Debug.WriteLine("Cell header: " + FirstRow.Cell(i).Value.ToString().ToUpper());
                            System.Diagnostics.Debug.WriteLine("Cell data: " + row.Cell(i).Value.ToString());
                            System.Diagnostics.Debug.WriteLine("Row: " + index);

                            if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("STORE_NO") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE NUMBER") ||
                                FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE #"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.STORE_NO = int.Parse(row.Cell(i).Value.ToString());
                                else
                                {
                                    error.Message += "Store number [" + row.Cell(i).Value.ToString() + "] at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                    error.Result = false;
                                    break;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("STORE_NAME") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("STORE NAME"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.STORE_NAME = row.Cell(i).Value.ToString();
                                else
                                {
                                    error.Result = false;
                                    error.Message += "Store name [" + row.Cell(i).Value.ToString() + "]  at {Row " + index + "} not in the correct format. | ";
                                    if (error.ErrorLevel != 3) errorLevel = 2;
                                }
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("OWNERSHIP") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("OWNERSHIP"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.OWNERSHIP = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("PROFIT_CENTER") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PROFIT CENTER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.PROFIT_CENTER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("BREAKFAST_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BREAKFAST PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.BREAKFAST_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("REGULAR_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("REGULAR PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.REGULAR_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("DC_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("DC PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.DC_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MDS_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MDS PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.MDS_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MCCAFE_LEVEL2_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MCCAFE LEVEL 2 PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.MCCAFE_LEVEL_2_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MCCAFE_LEVEL3_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MCCAFE LEVEL 3 PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.MCCAFE_LEVEL_3_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MCCAFE_BISTRO_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MCCAFE BISTRO PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.MCCAFE_BISTRO_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("PROJECT_GOLD_PRICE_TIER") ||
                                    FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PROJECT GOLD PRICE TIER"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.PROJECT_GOLD_PRICE_TIER = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("BET") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("BUSINESS EXTENSION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.BET = row.Cell(i).Value.ToString();
                            }


                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("REGION") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("REGION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.REGION = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("PROVINCE") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PROVINCE"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.PROVINCE = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("LOCATION") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("LOCATION"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.LOCATION = int.Parse(row.Cell(i).Value.ToString());
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("ADDRESS") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("ADDRESS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.ADDRESS = row.Cell(i).Value.ToString();;
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("CITY") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("CITY"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.CITY = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("FRESH_OR_FROZEN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FRESH OR FROZEN"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.FRESH_OR_FROZEN = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("PAPER_OR_PLASTIC") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("PAPER OR PLASTIC"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.PAPER_OR_PLASTIC = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("SOFT_SERVE_OR_VANILLA_POWDER_MIX") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SOFT SERVE OR VANILLA POWDER MIX"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("SIMPLOT_OR_MCCAIN") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("SIMPLOT OR MCCAIN"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.SIMPLOT_OR_MCCAIN = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("MCCORMICK_OR_GSF") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("MCCORMICK OR GSF"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.MCCORMICK_OR_GSF = row.Cell(i).Value.ToString();
                            }

                            else if (FirstRow.Cell(i).Value.ToString().ToUpper().Equals("FRESHB_OR_FROZENB") ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains("FRESH BUNS OR FROZEN BUNS"))
                            {
                                if (row.Cell(i).Value != null && !row.Cell(i).Value.ToString().Equals(""))
                                    SPRow.SOFT_SERVE_OR_VANILLA_POWDER_MIX = row.Cell(i).Value.ToString();
                            }
                            else if (FirstRow.Cell(i).Value == null ||
                                   FirstRow.Cell(i).Value.ToString().ToUpper().Contains(""))
                            {
                                blankCounter++;
                                if (blankCounter > 20) break;
                                else continue;
                            }

                        }

                        if (SPRow.STORE_NO == 0 || SPRow.STORE_NAME == null)
                        {
                            error.Result = false;
                            error.Message += "{Row " + index + "} has incorrect format | ";
                            errorLevel = 2;
                            break;
                        }

                        if (db.Store_Profile.Where(o => o.STORE_NO == SPRow.STORE_NO).Any())
                        {
                            error.Result = false;
                            error.Message += "Store profile [" + SPRow.STORE_NO + "] at {Row " + index + "} is already defined | ";
                            errorLevel = 2;
                        }

                        if (SPRow.OWNERSHIP != null && !SPRow.OWNERSHIP.Equals("") && !db.OWNERSHIPs.Where(o => o.Id == SPRow.OWNERSHIP).Any())
                        {
                            error.Result = false;
                            error.Message += "Ownership with Id [" + SPRow.OWNERSHIP+ "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }
                        
                        if (SPRow.PROFIT_CENTER != null && !SPRow.PROFIT_CENTER.Equals("") && !db.PROFIT_CEN.Where(o => o.Id == SPRow.PROFIT_CENTER).Any())
                        {
                            error.Result = false;
                            error.Message += "Ownership with Id [" + SPRow.PROFIT_CENTER + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (SPRow.LOCATION != null && !SPRow.PROFIT_CENTER.Equals("") && !db.LOCATIONs.Where(o => o.Id == SPRow.LOCATION).Any())
                        {
                            error.Result = false;
                            error.Message += "Location with Id [" + SPRow.LOCATION + "] at {Row " + index + "} not available | ";
                            errorLevel = 2;
                        }

                        if (errorLevel >= 2)
                            error.Message += "{Row " + index + "} not inserted | ";

                        if (errorLevel < 2)
                        {
                            db.Store_Profile.Add(SPRow);
                            try
                            {
                                db.SaveChanges();
                                // Special case for logging import 
                                succesfulRows++;
                                new AuditLogManager().Audit(user, DateTime.Now, "Store profile", "Import", SPRow.STORE_NO.ToString(), SPRow.STORE_NAME);
                                System.Diagnostics.Debug.WriteLine(SPRow.STORE_NO);
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
                error.Message += "Imported " + index + " rows. ";
                return error;
            }
        }
    }
}