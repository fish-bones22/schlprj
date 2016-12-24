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
        public List<RawItemMasterViewModel> SearchRawItem(RawItemMasterViewModel RIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<RawItemMasterViewModel> RIMList = new List<RawItemMasterViewModel>();
                List<INVRIMP0> RIMRowList = new List<INVRIMP0>();
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
                    if (RIMViewModel.InactiveItemsCb || rim.RIMSTA.Equals("0") )
                    {
                        RawItemMasterViewModel vm = new RawItemMasterViewModel();
                        vm.RIMRIC = rim.RIMRIC.ToString();
                        vm.RIMRID = rim.RIMRID;
                        vm.RIMRIG = rim.RIMRIG;
                        vm.RIMPIS = rim.RIMPIS;
                        vm.RIMBVP = rim.RIMBVP;
                        vm.RIMBZP = rim.RIMBZP;
                        vm.RIMUMC = rim.RIMUMC;
                        vm.RIMUPC = rim.RIMUPC.ToString();
                        vm.RIMSUQ = rim.RIMSUQ.ToString();
                        vm.RIMLAY = rim.RIMLAY.ToString();
                        vm.RIMCPR = rim.RIMCPR.ToString();
                        vm.RIMCPN = rim.RIMCPN.ToString();
                        vm.RIMPDT = ((DateTime )rim.RIMPDT).ToString("yyyy-MM-dd");
                        vm.RIMPVN = rim.RIMPVN.ToString();
                        vm.RIMCWC = rim.RIMCWC.ToString();
                        vm.RIMPRO = rim.RIMPRO;
                        vm.RIMSE4 = rim.RIMSE4;
                        vm.RIMERT = rim.RIMERT;
                        vm.RIMUSF = rim.RIMUSF.ToString();
                        vm.RIMMSD = rim.RIMMSD.ToString();
                        vm.RIMMSL = rim.RIMMSL.ToString();
                        vm.RIMLA1 = rim.RIMLA1;
                        vm.RIMLA2 = rim.RIMLA2;
                        vm.RIMSTA = rim.RIMSTA;
                        vm.RIMEDT = ((DateTime) rim.RIMEDT).ToString("yyyy-MM-dd");
                        vm.RIMORD = rim.RIMORD;
                        vm.RIMADE = rim.RIMADE;
                        vm.RIMBAR = rim.RIMBAR;
                        RIMList.Add(vm);
                    }
                }
                if (RIMList.ElementAt(0) == null)
                    return null;
                return RIMList;
            }
        }
    }
}