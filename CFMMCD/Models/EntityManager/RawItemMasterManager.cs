using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class RawItemMasterManager
    {
        public bool UpdateRawItem(RawItemMasterViewModel RIMViewModel, string user)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                INVRIMP0 RIMRow = new INVRIMP0();
                RIM_VEM_Lookup RVLRow = new RIM_VEM_Lookup();
                RIMRow.RIMRIC = int.Parse(RIMViewModel.RIMRIC);
                RIMRow.RIMRID = RIMViewModel.RIMRID.Trim();
                RIMRow.RIMRIG = RIMViewModel.RIMRIG.Trim();
                RIMRow.RIMPIS = RIMViewModel.RIMPIS.Trim();
                RIMRow.RIMBVP = RIMViewModel.RIMBVP.Trim();
                RIMRow.RIMBZP = RIMViewModel.RIMBZP.Trim();
                RIMRow.RIMUMC = RIMViewModel.RIMUMC.Trim();
                RIMRow.RIMUPC = double.Parse(RIMViewModel.RIMUPC);
                RIMRow.RIMSUQ = double.Parse(RIMViewModel.RIMSUQ);
                RIMRow.RIMLAY = int.Parse(RIMViewModel.RIMLAY);
                RIMRow.RIMCPR = double.Parse(RIMViewModel.RIMCPR);
                RIMRow.RIMCPN = double.Parse(RIMViewModel.RIMCPN);
                RIMRow.RIMPDT = DateTime.Parse(RIMViewModel.RIMPDT);
                RIMRow.RIMPVN = int.Parse(RIMViewModel.RIMPVN);
                RIMRow.RIMCWC = RIMViewModel.RIMCWC.Trim();
                RIMRow.RIMPRO = RIMViewModel.RIMPRO.Trim();
                RIMRow.RIMSE4 = RIMViewModel.RIMSE4.Trim();
                RIMRow.RIMERT = RIMViewModel.RIMERT.Trim();
                RIMRow.RIMUSF = double.Parse(RIMViewModel.RIMUSF);
                RIMRow.RIMMSD = double.Parse(RIMViewModel.RIMMSD);
                RIMRow.RIMMSL = double.Parse(RIMViewModel.RIMMSL);
                RIMRow.RIMLA1 = RIMViewModel.RIMLA1.Trim();
                RIMRow.RIMLA2 = RIMViewModel.RIMLA2.Trim();
                RIMRow.RIMSTA = RIMViewModel.RIMSTA.Trim();
                RIMRow.RIMEDT = DateTime.Parse(RIMViewModel.RIMEDT);
                RIMRow.RIMORD = RIMViewModel.RIMORD.Trim();
                RIMRow.RIMADE = RIMViewModel.RIMADE.Trim();
                RIMRow.RIMBAR = RIMViewModel.RIMBAR.Trim();
                // Non-field Row with default values
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
                RIMRow.RIMUSR = user.Substring(0, 3).ToUpper();
                RIMRow.RIMDAT = DateTime.Now;
                RIMRow.RIMFLG = false;
                RIMRow.RIMLIN = null; // Report line
                
                // Raw Item and Vendor
                if (RIMViewModel.VendorList == null)
                    RIMViewModel.VendorList = new List<Vendor>();
                foreach (Vendor vendor in RIMViewModel.VendorList)
                {
                    if (vendor.VendorCheckBox)
                    {
                        RVLRow.RIM_VEM_ID = RIMViewModel.RIMRIC + vendor.vendorId;
                        RVLRow.RIMRIC = int.Parse(RIMViewModel.RIMRIC);
                        RVLRow.VEMVEN = int.Parse(vendor.vendorId);
                        RVLRow.RIMCPR = double.Parse(vendor.RIMCPR);
                        // Perform update
                        if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Equals(RVLRow.RIM_VEM_ID)).Any())
                        {
                            RIM_VEM_Lookup rowToDelete = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(RVLRow.RIM_VEM_ID));
                            db.RIM_VEM_Lookup.Remove(rowToDelete);
                            db.RIM_VEM_Lookup.Add(RVLRow);
                        }
                        else
                            db.RIM_VEM_Lookup.Add(RVLRow);
                        db.SaveChanges();
                    }
                }
                
                try
                {
                    // If RIMRIC exists in the Table, perform an update
                    if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC)).Any())
                    {
                        INVRIMP0 rowToRemove = db.INVRIMP0.SingleOrDefault(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC));
                        RIMRow.STATUS = "E";
                        db.INVRIMP0.Remove(rowToRemove);
                        db.INVRIMP0.Add(RIMRow);
                    }
                    else
                    {
                        RIMRow.STATUS = "A";
                        db.INVRIMP0.Add(RIMRow);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch ( Exception e )
                {
                    return false;
                }
               
            }
        }
        public bool DeleteRawItem(RawItemMasterViewModel RIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                try
                {
                    if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC)).Any())
                    {
                        INVRIMP0 rowToRemove = db.INVRIMP0.FirstOrDefault(o => o.RIMRIC.ToString().Equals(RIMViewModel.RIMRIC));
                        List<RIM_VEM_Lookup> listOfRowsToRemove = db.RIM_VEM_Lookup.Where(o => o.RIMRIC == rowToRemove.RIMRIC).ToList();
                        foreach ( RIM_VEM_Lookup RVLRow in listOfRowsToRemove )
                        {
                            db.RIM_VEM_Lookup.Remove(RVLRow);
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
                    return false;
                }
            }
        }
        /*
         * Searches the table for any given SearchItem (Name or ID) in the ViewModel.
         * 
         * Returns List<ViewModel> if true, otherwise returns null
         */
        public List<RawItemMasterViewModel> SearchRawItem(RawItemMasterViewModel RIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItemMasterViewModel> RIMList = new List<RawItemMasterViewModel>();
                List<INVRIMP0> RIMRowList;
                if (RIMViewModel.SearchItem == null || RIMViewModel.SearchItem.Equals(""))
                    return null;
                if (db.INVRIMP0.Where(o => o.RIMRID.Equals(RIMViewModel.SearchItem)).Any())
                    RIMRowList = db.INVRIMP0.Where(o => o.RIMRID.Equals(RIMViewModel.SearchItem)).ToList();
                else if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.SearchItem)).Any())
                    RIMRowList = db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(RIMViewModel.SearchItem)).ToList();
                else
                    return null;
                foreach ( INVRIMP0 rim in RIMRowList )
                {
                    // Check if 'Include inactive items' is checked
                    if (RIMViewModel.InactiveItemsCb || rim.RIMSTA.Equals("0") )
                    {
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
                        vm.RIMCPR = rim.RIMCPR.ToString();
                        vm.RIMCPN = rim.RIMCPN.ToString();
                        vm.RIMPDT = ((DateTime)rim.RIMPDT).ToString("yyyy-MM-dd");
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
                        vm.VendorList = GetVendorList();
                        List<RIM_VEM_Lookup> RVLRowList = db.RIM_VEM_Lookup.Where(o => o.RIMRIC == rim.RIMRIC).ToList();
                        foreach ( RIM_VEM_Lookup RVLRow in  RVLRowList )
                        {
                            foreach ( Vendor vendor in vm.VendorList )
                            {
                                if (vendor.vendorId.Equals(RVLRow.VEMVEN.ToString()))
                                {
                                    vendor.RIMCPR = RVLRow.RIMCPR.ToString();
                                    vendor.VendorCheckBox = true;
                                }
                            }
                        }
                        RIMList.Add(vm);
                    }
                }
                if (RIMList == null || RIMList.ElementAt(0) == null)
                    return null;
                return RIMList;
            }
        }
        //public bool UpdateRawItemPrice( RawItemMasterViewModel RIMList, string user )
        //{
        //    using ( CFMMCDEntities db = new CFMMCDEntities() )
        //    {
        //        try
        //        {
        //            foreach (RawItemMasterViewModel vm in RIMList)
        //            {
        //                if (db.INVRIMP0.Where(o => o.RIMRIC.ToString().Equals(vm.RIMRIC)).Any())
        //                {
        //                    INVRIMP0 RIMRow = db.INVRIMP0.Single(o => o.RIMRIC.ToString().Equals(vm.RIMRIC));
        //                    db.INVRIMP0.Remove(RIMRow);
        //                    RIMRow.RIMCPN = double.Parse(vm.RIMCPN);
        //                    RIMRow.RIMPDT = DateTime.Parse(vm.RIMPDT);
        //                    RIMRow.STATUS = "E";
        //                    RIMRow.RIMUSR = user.Substring(0, 3).ToUpper();
        //                    db.INVRIMP0.Add(RIMRow);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch ( Exception e )
        //        {
        //            return false;
        //        }
                
        //    }
        //}

        public List<Vendor> GetVendorList()
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                RawItemMasterViewModel RIMViewModel = new RawItemMasterViewModel();
                RIMViewModel.VendorList = new List<Vendor>();
                List<INVVEMP0> VEMRowList = db.INVVEMP0.ToList();
                if (VEMRowList == null)
                    return new List<Vendor>();
                foreach ( INVVEMP0 VEMRow in VEMRowList )
                {
                    Vendor vendor = new Vendor();
                    vendor.vendorId = VEMRow.VEMVEN.ToString();
                    vendor.VendorName = VEMRow.VEMDS1.Trim();
                    vendor.VendorCheckBox = false;
                    vendor.RIMCPR = "";
                    vendor.PPERUN = "";
                    vendor.SCMCOD = "";
                    RIMViewModel.VendorList.Add(vendor);
                }
                return RIMViewModel.VendorList;
            }
        }
    }
}