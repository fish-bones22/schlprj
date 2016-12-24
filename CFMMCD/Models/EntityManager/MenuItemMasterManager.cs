using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class MenuItemMasterManager
    {
        /*
         * Searches the table for any given SearchItem (Name or ID) in the ViewModel.
         * 
         * Returns List<ViewModel> if true, otherwise returns null
         */
        public List<MenuItemMasterViewModel> SearchMenuItem(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHMIMP0> MIMRowList;
                List<MenuItemMasterViewModel> MIMList = new List<MenuItemMasterViewModel>();

                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem)).Any())
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem)).ToList();
                else if (db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Contains(MIMViewModel.SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMNAM.ToString().Contains(MIMViewModel.SearchItem)).ToList();
                }
                else
                    return null;
                foreach (CSHMIMP0 MIMRow in MIMRowList)
                {
                    // Check if 'Include inactive items' is checked
                    if (MIMViewModel.InactiveItemsCb || MIMRow.MIMSTA.Equals("0"))
                    {
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

                        if (MIMRow.MIMMIC_NP6 != null || MIMRow.MIMMIC_NP6.HasValue)
                        {
                            vm.MIMMIC_NP6 = MIMRow.CSHMIMP0_NP6.MIMMIC.ToString();
                            vm.MIMNAM_NP6 = MIMRow.CSHMIMP0_NP6.MIMNAM.Trim();
                            vm.MIMLON_NP6 = MIMRow.CSHMIMP0_NP6.MIMLON.Trim();
                        }
                        MIMList.Add(vm);
                    }
                }
                if (MIMList == null || MIMList.ElementAt(0) == null)
                    return null;
                return MIMList;
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
            using ( CFMMCDEntities db = new CFMMCDEntities())
            {
                CSHMIMP0 MIMRow = new CSHMIMP0();
                // Columns with Input fields
                MIMRow.MIMMIC = int.Parse(MIMViewModel.MIMMIC);
                MIMRow.MIMSTA = MIMViewModel.MIMSTA.Trim();
                MIMRow.MIMFGC = MIMViewModel.MIMFGC.Trim();
                MIMRow.MIMNAM = MIMViewModel.MIMNAM.Trim();
                MIMRow.MIMDSC = MIMViewModel.MIMDSC.Trim();
                MIMRow.MIMDPC = MIMViewModel.MIMDPC.Trim();
                MIMRow.MIMTCI = MIMViewModel.MIMTCI.Trim();
                MIMRow.MIMPRI = double.Parse(MIMViewModel.MIMPRI);
                MIMRow.MIMTCA = MIMViewModel.MIMTCA.Trim();
                MIMRow.MIMPRO = double.Parse(MIMViewModel.MIMPRO);
                MIMRow.MIMTCG = MIMViewModel.MIMTCG.Trim();
                MIMRow.MIMPRG = double.Parse(MIMViewModel.MIMPRG);
                MIMRow.MIMPND = Convert.ToDateTime(MIMViewModel.MIMPND);
                MIMRow.MIMWGR = MIMViewModel.MIMWGR.Trim();
                MIMRow.MIMUTC = int.Parse(MIMViewModel.MIMUTC);
                MIMRow.MIMHPT = MIMViewModel.MIMHPT.Trim();
                MIMRow.MIMEDT = Convert.ToDateTime(MIMViewModel.MIMEDT);
                MIMRow.MIMNPI = double.Parse(MIMViewModel.MIMNPI);
                MIMRow.MIMNPO = double.Parse(MIMViewModel.MIMNPO);
                MIMRow.MIMNPD = double.Parse(MIMViewModel.MIMNPD);
                MIMRow.MIMNPA = double.Parse(MIMViewModel.MIMNPA);
                MIMRow.MIMNNP = double.Parse(MIMViewModel.MIMNNP);
                MIMRow.MIMNPT = MIMViewModel.MIMNPT.Trim();
                // Items not originally in the table but
                // have Input field
                // * Wala pong nakalista sa CSHMIMP0.dbf na Long Name
                //   pero merong input field para sa kanya sa pptx
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
                        var rowToUpdate = db.CSHMIMP0.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));
                        MIMRow.STATUS = "E";
                        db.CSHMIMP0_NP6.Remove(rowToUpdate.CSHMIMP0_NP6); // Delete existing row before inserting 
                        db.CSHMIMP0.Remove(rowToUpdate);                  // updated replacement
                        db.CSHMIMP0.Add(MIMRow);
                    }
                    else
                        db.CSHMIMP0.Add(MIMRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
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
                    db.CSHMIMP0.Remove(MIMRow);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}