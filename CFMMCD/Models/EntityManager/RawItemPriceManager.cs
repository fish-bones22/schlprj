﻿using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class RawItemPriceManager
    {
        public bool UpdateRawItemPrice( RawItemPriceUpdateViewModel RIPViewModel )
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                RIM_VEM_Lookup RVLRow = new RIM_VEM_Lookup();
                RVLRow.RIM_VEM_ID = RIPViewModel.RIM_VEM_ID;
                RVLRow.RIMRIC = int.Parse(RIPViewModel.RIMRIC);
                RVLRow.VEMVEN = int.Parse(RIPViewModel.VEMVEN);
                RVLRow.VEMDS1 = RIPViewModel.VEMDS1.Trim();
                RVLRow.RIMCPR = double.Parse(RIPViewModel.RIMCPR);
                RVLRow.RIMRID = RIPViewModel.RIMRID;
                RVLRow.RIMPDT = DateTime.Parse(RIPViewModel.RIMPDT);
                RVLRow.RIMCPN = double.Parse(RIPViewModel.RIMCPN);

                try
                {
                    if ( db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID)).Any() )
                    {
                        var RowToDelete = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID));
                        RVLRow.PPERUN = RowToDelete.PPERUN;
                        RVLRow.SCMCOD = RowToDelete.SCMCOD;
                        db.RIM_VEM_Lookup.Remove(RowToDelete);
                        db.RIM_VEM_Lookup.Add(RVLRow);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch ( Exception e )
                {
                    return false;
                }
            }
        }
        public bool DeleteRawItemPrice( RawItemPriceUpdateViewModel RIPViewModel )
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                try
                {
                    if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID)).Any())
                    {
                        RIM_VEM_Lookup rowToRemove = db.RIM_VEM_Lookup.FirstOrDefault(o => o.RIM_VEM_ID.Equals(RIPViewModel.RIM_VEM_ID));
                        db.RIM_VEM_Lookup.Remove(rowToRemove);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public List<RawItemPriceUpdateViewModel> SearchRawItemPrice( RawItemPriceUpdateViewModel RIPViewModel )
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItemPriceUpdateViewModel> RIPList = new List<RawItemPriceUpdateViewModel>();
                List<RIM_VEM_Lookup> RIPRowList;
                if (RIPViewModel.SearchItem == null || RIPViewModel.SearchItem.Equals(""))
                    return null;
                if (db.RIM_VEM_Lookup.Where(o => o.RIMRID.Contains(RIPViewModel.SearchItem)).Any())
                    RIPRowList = db.RIM_VEM_Lookup.Where(o => o.RIMRID.Equals(RIPViewModel.SearchItem)).ToList();
                else if (db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(RIPViewModel.SearchItem)).Any())
                    RIPRowList = db.RIM_VEM_Lookup.Where(o => o.RIMRIC.ToString().Equals(RIPViewModel.SearchItem)).ToList();
                else
                    return null;
                foreach (RIM_VEM_Lookup rim in RIPRowList)
                {
                    RawItemPriceUpdateViewModel vm = new RawItemPriceUpdateViewModel();
                    vm.RIM_VEM_ID = rim.RIM_VEM_ID;
                    vm.RIMRIC = rim.RIMRIC.ToString();
                    vm.VEMVEN = rim.VEMVEN.ToString();
                    vm.VEMDS1 = rim.VEMDS1.Trim();
                    vm.RIMCPR = rim.RIMCPR.ToString();
                    vm.RIMRID = rim.RIMRID.ToString();
                    vm.RIMPDT = rim.RIMPDT.ToString();
                    vm.RIMCPN = rim.RIMCPN.ToString();
                    RIPList.Add(vm);
                }
                if (RIPList == null || RIPList.ElementAt(0) == null)
                    return null;
                return RIPList;
            }
        }
    }
}