using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ItemGroupManager
    {
        public static List<ItemGroupViewModel> GetGroup(ItemGroupViewModel IGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<ITMGRP> IGRows;
                List<ItemGroupViewModel> IGList = IGViewModel.GroupList;
                //IGRows = db.ITMGRPs.Where(o => o.Group_Type == IGViewModel.GroupType).OrderBy(o => o.Group_Id).ToList();
                IGRows = db.ITMGRPs.OrderBy(o => o.Group_Id).ToList();
                int CurrentGroup = 0;

                ItemGroupViewModel vm = new ItemGroupViewModel();

                for (int i = 0; i < IGRows.Count(); i++)
                {
                    if (CurrentGroup != IGRows[i].Group_Id)
                    {
                        vm = new ItemGroupViewModel();
                        vm.GroupId = IGRows[i].Group_Id;
                        vm.GroupType = IGRows[i].Group_Type;
                        vm.GroupName = IGRows[i].Group_Name;
                        vm.Id = IGRows[i].Id;
                    }
                    else
                    {
                        continue;
                    }

                    foreach (var v in IGRows.Where( o => o.Group_Id == IGRows[i].Group_Id))
                    {
                        Item item = new Item();
                        item.Name = v.Item_Name;
                        item.ItemId = v.Item_Code;
                        item.Id = v.Id;
                        item.ItemType = v.Item_Type;

                        // If item is store
                        if (v.Item_Type == 3)
                            vm.StoreList.Add(item);
                        else
                            vm.ItemList.Add(item);
                    }
                    IGList.Add(vm);
                    CurrentGroup = IGRows[i].Group_Id;
                }
                return IGList;
            }
        }

        public static bool UpdateGroup(ItemGroupViewModel IGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                // Update
                if (IGViewModel.GroupName == null || IGViewModel.GroupName.Equals("") || IGViewModel.GroupType == 0)
                    return false;

                var IGRowLookup = db.ITMGRPs.Where(o => o.Item_Code == IGViewModel.ItemCode);
                IGRowLookup = IGRowLookup.Where(o => o.Item_Type == IGViewModel.ItemType);
                if (IGRowLookup.Any())
                    return false;

                ITMGRP IGRow = new ITMGRP();
                IGRow.Id = new Random().Next(1, 999999999);
                // Get new Id if it exist
                for (int i = 0; (i < 10 && db.ITMGRPs.Where(o => o.Id == IGRow.Id).Any()); i++)
                    IGRow.Id = new Random().Next(1, 999999999);
                // If Group is already created
                if (db.ITMGRPs.Where(o => o.Group_Name.Equals(IGViewModel.GroupName)).Any())
                {
                    IGRow.Group_Id = IGViewModel.GroupId;
                    IGRow.Group_Name = IGViewModel.GroupName;
                    IGRow.Item_Code = IGViewModel.ItemCode;
                    IGRow.Item_Name = IGViewModel.ItemName;
                    IGRow.Item_Type = IGViewModel.ItemType;
                    IGRow.Group_Type = IGViewModel.GroupType;
                    // Add Group to Item table
                    if (IGRow.Item_Type == 1 && db.CSHMIMP0.Where(o => o.MIMMIC == IGViewModel.ItemCode).Any())
                    {
                        db.CSHMIMP0.Single(o => o.MIMMIC == IGViewModel.ItemCode).Group = IGViewModel.GroupId;
                    }
                    else if (IGRow.Item_Type == 2 && db.INVRIMP0.Where(o => o.RIMRIC == IGViewModel.ItemCode).Any())
                    {
                        db.INVRIMP0.Single(o => o.RIMRIC == IGViewModel.ItemCode).Group = IGViewModel.GroupId;
                    }
                    else if (IGRow.Item_Type == 3 && db.Store_Profile.Where(o => o.STORE_NO == IGViewModel.ItemCode).Any())
                    {
                        db.Store_Profile.Single(o => o.STORE_NO == IGViewModel.ItemCode).Group = IGViewModel.GroupId;
                    }
                    else if (IGRow.Item_Type == 4 && db.INVRIRP0.Where(o => o.RIRMIC == IGViewModel.ItemCode).Any())
                    {
                        foreach (var v in db.INVRIRP0.Where(o => o.RIRMIC == IGViewModel.ItemCode))
                            v.Group = IGViewModel.GroupId;
                    }
                }
                else
                {
                    IGRow.Group_Id = new Random().Next(1,999999);
                    // Get new Group_Id if it exist
                    for (int i = 0; (i < 10 && db.ITMGRPs.Where(o => o.Group_Id == IGRow.Group_Id).Any()); i++)
                        IGRow.Group_Id = new Random().Next(1, 999999);
                    IGRow.Group_Name = IGViewModel.GroupName;
                    IGRow.Item_Code = 0;
                    IGRow.Item_Name = "";
                    IGRow.Item_Type = IGViewModel.ItemType;
                    IGRow.Group_Type = IGViewModel.GroupType;
                }
                db.ITMGRPs.Add(IGRow);

                // Save DB
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

        public static bool DeleteItem(int Id)
        {
            using (CFMMCDEntities db =  new CFMMCDEntities())
            {
                if (db.ITMGRPs.Where(o => o.Id == Id).Any())
                {
                    ITMGRP IGRow = db.ITMGRPs.SingleOrDefault(o => o.Id == Id);
                    if (IGRow.Item_Type == 1)
                        db.CSHMIMP0.Single(o => o.MIMMIC == IGRow.Item_Code).Group = 0;
                    else if (IGRow.Item_Type == 2)
                        db.INVRIMP0.Single(o => o.RIMRIC == IGRow.Item_Code).Group = 0;
                    if (IGRow.Item_Type == 3)
                        db.Store_Profile.Single(o => o.STORE_NO == IGRow.Item_Code).Group = 0;
                    if (IGRow.Item_Type == 4)
                    {
                        foreach (var v in db.INVRIRP0.Where(o => o.RIRMIC == IGRow.Item_Code))
                            v.Group = 0;
                    }

                    db.ITMGRPs.Remove(IGRow);
                    // Save DB
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
                return false;
            }
        }

        public static bool DeleteGroup(int GroupId)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (db.ITMGRPs.Where(o => o.Group_Id == GroupId).Any())
                {
                    db.ITMGRPs.RemoveRange(db.ITMGRPs.Where(o => o.Group_Id == GroupId));
                    foreach (var v in db.CSHMIMP0.Where(o => o.Group == GroupId))
                    {
                        v.Group = 0;
                    }
                    foreach (var v in db.INVRIMP0.Where(o => o.Group == GroupId))
                    {
                        v.Group = 0;
                    }
                    foreach (var v in db.Store_Profile.Where(o => o.Group == GroupId))
                    {
                        v.Group = 0;
                    }
                    foreach (var v in db.INVRIRP0.Where(o => o.Group == GroupId))
                    {
                        v.Group = 0;
                    }
                    // Save DB
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
                else
                    return false;
            }
        }
        public static List<MenuItem> GetFilteredMenuItem()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MenuItem> MIList = MenuItemMasterManager.SearchMenuItems("ALL");
                List<ITMGRP> IGRows = db.ITMGRPs.Where( o => o.Item_Type == 1).ToList();
                foreach (var v in IGRows)
                {
                    MIList.RemoveAll(o => o.MIMMIC.Equals(v.Item_Code.ToString()));
                }
                return MIList;
            }
        }
        public static List<RawItem> GetFilteredRawItem()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItem> RIList = RawItemMasterManager.GetRawItems("ALL");
                List<ITMGRP> IGRows = db.ITMGRPs.ToList();
                foreach (var v in IGRows)
                {
                    RIList.RemoveAll(o => o.RIMRIC.Equals(v.Item_Code.ToString()));
                }
                return RIList;
            }
        }
        public static List<Store> GetFilteredStore()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<Store> STOList = new StoreProfileManager().SearchStores("ALL");
                List<ITMGRP> IGRows = db.ITMGRPs.ToList();
                foreach (var v in IGRows)
                {
                    STOList.RemoveAll(o => o.Store_No.Equals(v.Item_Code.ToString()));
                }
                return STOList;
            }
        }
        public static List<MenuItem> GetFilteredRecipe()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<MenuItem> RIRList = MenuRecipeManager.SearchMenuItem("ALL");
                List<ITMGRP> IGRows = db.ITMGRPs.Where(o => o.Item_Type == 4).ToList();
                foreach (var v in IGRows)
                    RIRList.RemoveAll(o => o.MIMMIC.Equals(v.Item_Code.ToString()));
                return RIRList;
            }
        }
        public static string GetGroupName(int id)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                if (db.ITMGRPs.Where(o => o.Group_Id == id).Any())
                    return db.ITMGRPs.Single(o => o.Group_Id == id).Group_Name;
                else
                    return null;
            }
        }
    }
}