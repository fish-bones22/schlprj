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
                RIMRow.RIMUSR = user.Substring(0, 3).ToUpper();
                RIMRow.RIMDAT = DateTime.Now;
                RIMRow.RIMFLG = false;
                RIMRow.RIMLIN = null; // Report line

                // Raw Item and Vendor
                int i = 0;
                foreach (var vendor in RIMViewModel.VendorList)
                {
                    if (RIMViewModel.VendorsSelectedList[i])
                    {
                        RIM_VEM_Lookup RVLRow = new RIM_VEM_Lookup();
                        RVLRow.RIM_VEM_ID = RIMViewModel.RIMRIC + vendor.value;
                        RVLRow.RIMRIC = int.Parse(RIMViewModel.RIMRIC);
                        RVLRow.VEMVEN = int.Parse(vendor.value);
                        RVLRow.VEMDS1 = vendor.text;
                        RVLRow.RIMCPR = double.Parse(RIMViewModel.VendorCPR[i]);
                        RVLRow.PPERUN = double.Parse(RIMViewModel.VendorPUN[i]);
                        RVLRow.SCMCOD = double.Parse(RIMViewModel.VendorSCM[i]);
                        // Perform update
                        if (db.RIM_VEM_Lookup.Where(o => o.RIM_VEM_ID.Trim().Equals(RVLRow.RIM_VEM_ID.Trim())).Any())
                        {
                            RIM_VEM_Lookup rowToDelete = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Trim().Equals(RVLRow.RIM_VEM_ID.Trim()));
                            RVLRow.RIMCPN = rowToDelete.RIMCPN;
                            RVLRow.RIMPDT = rowToDelete.RIMPDT;
                            db.RIM_VEM_Lookup.Remove(rowToDelete);
                            db.RIM_VEM_Lookup.Add(RVLRow);
                        }
                        else
                            db.RIM_VEM_Lookup.Add(RVLRow);
                    }
                    i++;
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
                        RIMList.Add(vm);
                    }
                }
                if (RIMList == null || RIMList.ElementAt(0) == null)
                    return null;
                return RIMList;
            }
        }
    }
}