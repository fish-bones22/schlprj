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
         * 
         * Returns List<ViewModel> if true, otherwise returns null
         */
        public List<MenuItemMasterViewModel> SearchMenuItem(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHMIMP0> MIMRowList;
                List<MenuItemMasterViewModel> MIMList = new List<MenuItemMasterViewModel>();
                string ItemCode = "";
                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem)).Any())
                {
                    MIMRowList = db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.SearchItem)).ToList();
                    ItemCode = MIMViewModel.SearchItem;
                }
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
                        vm.Location = "";
                        vm.Store = "";


                        if (MIMRow.MIMMIC_NP6 != null || MIMRow.MIMMIC_NP6.HasValue)
                        {
                            vm.MIMMIC_NP6 = MIMRow.CSHMIMP0_NP6.MIMMIC.ToString();
                            vm.MIMNAM_NP6 = MIMRow.CSHMIMP0_NP6.MIMNAM.Trim();
                            vm.MIMLON_NP6 = MIMRow.CSHMIMP0_NP6.MIMLON.Trim();
                        }
                        // Initialize external table row values
                        if (ItemCode.Equals(""))
                            ItemCode = MIMRow.MIMMIC.ToString();
                        vm = SearchPriceTier(vm);
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
            using (CFMMCDEntities db = new CFMMCDEntities())
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
                MIMRow.MIMPRI = 0; //
                MIMRow.MIMTCA = MIMViewModel.MIMTCA.Trim();
                MIMRow.MIMPRO = 0; //
                MIMRow.MIMTCG = MIMViewModel.MIMTCG.Trim();
                MIMRow.MIMPRG = 0; //
                MIMRow.MIMPND = DateTime.Now; // 
                MIMRow.MIMWGR = MIMViewModel.MIMWGR.Trim();
                MIMRow.MIMUTC = int.Parse(MIMViewModel.MIMUTC);
                MIMRow.MIMHPT = MIMViewModel.MIMHPT.Trim();
                MIMRow.MIMEDT = Convert.ToDateTime(MIMViewModel.MIMEDT);
                MIMRow.MIMNPI = 0;
                MIMRow.MIMNPO = 0;
                MIMRow.MIMNPD = 0;
                MIMRow.MIMNPA = 0;
                MIMRow.MIMNNP = 0;
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
                        System.Diagnostics.Debug.WriteLine("Passed: fetching existing table");
                        MIMRow.STATUS = "E";
                        System.Diagnostics.Debug.WriteLine("Passed: assignment of STATUS");
                        if (rowToUpdate.CSHMIMP0_NP6 != null)
                            db.CSHMIMP0_NP6.Remove(rowToUpdate.CSHMIMP0_NP6);   // Delete existing row before inserting 
                        System.Diagnostics.Debug.WriteLine("Passed: removal of np6");
                        db.CSHMIMP0.Remove(rowToUpdate);                        // updated replacement
                        System.Diagnostics.Debug.WriteLine("Passed: removal of row");
                        db.CSHMIMP0.Add(MIMRow);
                        System.Diagnostics.Debug.WriteLine("Passed: update of row");
                    }
                    else
                    {
                        db.CSHMIMP0.Add(MIMRow);
                        System.Diagnostics.Debug.WriteLine("Passed:saving of row");
                    }
                    UpdatePriceTier(MIMViewModel);
                    System.Diagnostics.Debug.WriteLine("Passed: Update of price tier");
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Passed:saving of DB");
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    foreach (var eve in ((DbEntityValidationException)e).EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
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
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    return false;
                }
            }
        }
        public bool UpdatePriceTier(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                Tier_Lookup tier = new Tier_Lookup();
                tier.MIMMIC = int.Parse(MIMViewModel.MIMMIC);
                tier.OLDPRA = MIMViewModel.OLDPRA;
                tier.NEWPRA = MIMViewModel.NEWPRA;
                tier.OLDPAO = MIMViewModel.OLDPAO;
                tier.NEWPAO = MIMViewModel.NEWPAO;
                tier.OLDAOT = MIMViewModel.OLDAOT;
                tier.NEWAOT = MIMViewModel.NEWAOT;
                tier.OLDNPA = MIMViewModel.OLDNPA;
                tier.NEWNPA = MIMViewModel.NEWNPA;
                tier.OLDPRB = MIMViewModel.OLDPRB;
                tier.NEWPRB = MIMViewModel.NEWPRB;
                tier.OLDPBO = MIMViewModel.OLDPBO;
                tier.NEWPBO = MIMViewModel.NEWPBO;
                tier.OLDBOT = MIMViewModel.OLDBOT;
                tier.NEWBOT = MIMViewModel.NEWBOT;
                tier.OLDNPB = MIMViewModel.OLDNPB;
                tier.NEWNPB = MIMViewModel.NEWNPB;
                tier.OLDPRC = MIMViewModel.OLDPRC;
                tier.NEWPRC = MIMViewModel.NEWPRC;
                tier.OLDPCO = MIMViewModel.OLDPCO;
                tier.NEWPCO = MIMViewModel.NEWPCO;
                tier.OLDCOT = MIMViewModel.OLDCOT;
                tier.NEWCOT = MIMViewModel.NEWCOT;
                tier.OLDNPC = MIMViewModel.OLDNPC;
                tier.NEWNPC = MIMViewModel.NEWNPC;
                tier.OLDPRD = MIMViewModel.OLDPRD;
                tier.NEWPRD = MIMViewModel.NEWPRD;
                tier.OLDPDO = MIMViewModel.OLDPDO;
                tier.NEWPDO = MIMViewModel.NEWPDO;
                tier.OLDDOT = MIMViewModel.OLDDOT;
                tier.NEWDOT = MIMViewModel.NEWDOT;
                tier.OLDNPD = MIMViewModel.OLDNPD;
                tier.NEWNPD = MIMViewModel.NEWNPD;
                tier.OLDPRE = MIMViewModel.OLDPRE;
                tier.NEWPRE = MIMViewModel.NEWPRE;
                tier.OLDPEO = MIMViewModel.OLDPEO;
                tier.NEWPEO = MIMViewModel.NEWPEO;
                tier.OLDEOT = MIMViewModel.OLDEOT;
                tier.NEWEOT = MIMViewModel.NEWEOT;
                tier.OLDNPE = MIMViewModel.OLDNPE;
                tier.NEWNPE = MIMViewModel.NEWNPE;
                tier.OLDPRF = MIMViewModel.OLDPRF;
                tier.NEWPRF = MIMViewModel.NEWPRF;
                tier.OLDPFO = MIMViewModel.OLDPFO;
                tier.NEWPFO = MIMViewModel.NEWPFO;
                tier.OLDFOT = MIMViewModel.OLDFOT;
                tier.NEWFOT = MIMViewModel.NEWFOT;
                tier.OLDNPF = MIMViewModel.OLDNPF;
                tier.NEWNPF = MIMViewModel.NEWNPF;
                tier.OLDMDS = MIMViewModel.OLDMDS;
                tier.NEWMDS = MIMViewModel.NEWMDS;
                tier.OLDMDO = MIMViewModel.OLDMDO;
                tier.NEWMDO = MIMViewModel.NEWMDO;
                tier.OLDMOT = MIMViewModel.OLDMOT;
                tier.NEWMOT = MIMViewModel.NEWMOT;
                tier.OLDMDN = MIMViewModel.OLDMDN;
                tier.NEWMDN = MIMViewModel.NEWMDN;
                tier.OLDPRS = MIMViewModel.OLDPRS;
                tier.NEWPRS = MIMViewModel.NEWPRS;
                tier.OLDPSO = MIMViewModel.OLDPSO;
                tier.NEWPSO = MIMViewModel.NEWPSO;
                tier.OLDSOT = MIMViewModel.OLDSOT;
                tier.NEWSOT = MIMViewModel.NEWSOT;
                tier.OLDNPS = MIMViewModel.OLDNPS;
                tier.NEWNPS = MIMViewModel.NEWNPS;

                tier.EDTA = DateTime.Parse(MIMViewModel.EDTA);
                tier.PNDA = DateTime.Parse(MIMViewModel.PNDA);
                tier.EDTB = DateTime.Parse(MIMViewModel.EDTB);
                tier.PNDB = DateTime.Parse(MIMViewModel.PNDB);
                tier.EDTC = DateTime.Parse(MIMViewModel.EDTC);
                tier.PNDC = DateTime.Parse(MIMViewModel.PNDC);
                tier.EDTD = DateTime.Parse(MIMViewModel.EDTD);
                tier.PNDD = DateTime.Parse(MIMViewModel.PNDD);
                tier.PNDE = DateTime.Parse(MIMViewModel.PNDE);
                tier.EDTE = DateTime.Parse(MIMViewModel.EDTE);
                tier.PNDF = DateTime.Parse(MIMViewModel.PNDF);
                tier.EDTF = DateTime.Parse(MIMViewModel.EDTF);
                tier.PNDM = DateTime.Parse(MIMViewModel.PNDM);
                tier.EDTM = DateTime.Parse(MIMViewModel.EDTM);
                tier.EDTS = DateTime.Parse(MIMViewModel.EDTS);
                tier.PNDS = DateTime.Parse(MIMViewModel.PNDS);

                try
                {
                    if(db.Tier_Lookup.Where( o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                    {
                        Tier_Lookup RowToDelete = db.Tier_Lookup.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));
                        System.Diagnostics.Debug.WriteLine("Passed: fetching existing row");
                        db.Tier_Lookup.Remove(RowToDelete);
                        System.Diagnostics.Debug.WriteLine("Passed: removal of existing row");
                        db.Tier_Lookup.Add(tier);
                        System.Diagnostics.Debug.WriteLine("Passed: update of existing row");
                    }
                    else
                    {
                        db.Tier_Lookup.Add(tier);
                        System.Diagnostics.Debug.WriteLine("Passed: addition of new row");
                    }
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Passed: DB saved changes");
                    return true;
                }
                catch ( Exception e )
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.InnerException);
                    System.Diagnostics.Debug.WriteLine(e.Data);
                    foreach (var eve in ((DbEntityValidationException) e).EntityValidationErrors )
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }
        /*
         * IMPORTANT: Method used by Search Menu item. Not designed to be used by Menu Item Price Tier, 
         * as it uses the same ViewModel of Menu Item Master.
         * */
        private MenuItemMasterViewModel SearchPriceTier(MenuItemMasterViewModel MIMViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Tier_Lookup MIMRow;

                if (db.CSHMIMP0.Where(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC)).Any())
                    MIMRow = db.Tier_Lookup.Single(o => o.MIMMIC.ToString().Equals(MIMViewModel.MIMMIC));
                else
                    return null;
                MenuItemMasterViewModel vm = new MenuItemMasterViewModel();
                MIMViewModel.MIMMIC = MIMRow.MIMMIC.ToString();
                MIMViewModel.OLDPRA = (double)MIMRow.OLDPRA;
                MIMViewModel.NEWPRA = (double)MIMRow.NEWPRA;
                MIMViewModel.OLDPAO = (double)MIMRow.OLDPAO;
                MIMViewModel.NEWPAO = (double)MIMRow.NEWPAO;
                MIMViewModel.OLDAOT = (double)MIMRow.OLDAOT;
                MIMViewModel.NEWAOT = (double)MIMRow.NEWAOT;
                MIMViewModel.OLDNPA = (double)MIMRow.OLDNPA;
                MIMViewModel.NEWNPA = (double)MIMRow.NEWNPA;
                MIMViewModel.OLDPRB = (double)MIMRow.OLDPRB;
                MIMViewModel.NEWPRB = (double)MIMRow.NEWPRB;
                MIMViewModel.OLDPBO = (double)MIMRow.OLDPBO;
                MIMViewModel.NEWPBO = (double)MIMRow.NEWPBO;
                MIMViewModel.OLDBOT = (double)MIMRow.OLDBOT;
                MIMViewModel.NEWBOT = (double)MIMRow.NEWBOT;
                MIMViewModel.OLDNPB = (double)MIMRow.OLDNPB;
                MIMViewModel.NEWNPB = (double)MIMRow.NEWNPB;
                MIMViewModel.OLDPRC = (double)MIMRow.OLDPRC;
                MIMViewModel.NEWPRC = (double)MIMRow.NEWPRC;
                MIMViewModel.OLDPCO = (double)MIMRow.OLDPCO;
                MIMViewModel.NEWPCO = (double)MIMRow.NEWPCO;
                MIMViewModel.OLDCOT = (double)MIMRow.OLDCOT;
                MIMViewModel.NEWCOT = (double)MIMRow.NEWCOT;
                MIMViewModel.OLDNPC = (double)MIMRow.OLDNPC;
                MIMViewModel.NEWNPC = (double)MIMRow.NEWNPC;
                MIMViewModel.OLDPRD = (double)MIMRow.OLDPRD;
                MIMViewModel.NEWPRD = (double)MIMRow.NEWPRD;
                MIMViewModel.OLDPDO = (double)MIMRow.OLDPDO;
                MIMViewModel.NEWPDO = (double)MIMRow.NEWPDO;
                MIMViewModel.OLDDOT = (double)MIMRow.OLDDOT;
                MIMViewModel.NEWDOT = (double)MIMRow.NEWDOT;
                MIMViewModel.OLDNPD = (double)MIMRow.OLDNPD;
                MIMViewModel.NEWNPD = (double)MIMRow.NEWNPD;
                MIMViewModel.OLDPRE = (double)MIMRow.OLDPRE;
                MIMViewModel.NEWPRE = (double)MIMRow.NEWPRE;
                MIMViewModel.OLDPEO = (double)MIMRow.OLDPEO;
                MIMViewModel.NEWPEO = (double)MIMRow.NEWPEO;
                MIMViewModel.OLDEOT = (double)MIMRow.OLDEOT;
                MIMViewModel.NEWEOT = (double)MIMRow.NEWEOT;
                MIMViewModel.OLDNPE = (double)MIMRow.OLDNPE;
                MIMViewModel.NEWNPE = (double)MIMRow.NEWNPE;
                MIMViewModel.OLDPRF = (double)MIMRow.OLDPRF;
                MIMViewModel.NEWPRF = (double)MIMRow.NEWPRF;
                MIMViewModel.OLDPFO = (double)MIMRow.OLDPFO;
                MIMViewModel.NEWPFO = (double)MIMRow.NEWPFO;
                MIMViewModel.OLDFOT = (double)MIMRow.OLDFOT;
                MIMViewModel.NEWFOT = (double)MIMRow.NEWFOT;
                MIMViewModel.OLDNPF = (double)MIMRow.OLDNPF;
                MIMViewModel.NEWNPF = (double)MIMRow.NEWNPF;
                MIMViewModel.OLDMDS = (double)MIMRow.OLDMDS;
                MIMViewModel.NEWMDS = (double)MIMRow.NEWMDS;
                MIMViewModel.OLDMDO = (double)MIMRow.OLDMDO;
                MIMViewModel.NEWMDO = (double)MIMRow.NEWMDO;
                MIMViewModel.OLDMOT = (double)MIMRow.OLDMOT;
                MIMViewModel.NEWMOT = (double)MIMRow.NEWMOT;
                MIMViewModel.OLDMDN = (double)MIMRow.OLDMDN;
                MIMViewModel.NEWMDN = (double)MIMRow.NEWMDN;
                MIMViewModel.OLDPRS = (double)MIMRow.OLDPRS;
                MIMViewModel.NEWPRS = (double)MIMRow.NEWPRS;
                MIMViewModel.OLDPSO = (double)MIMRow.OLDPSO;
                MIMViewModel.NEWPSO = (double)MIMRow.NEWPSO;
                MIMViewModel.OLDSOT = (double)MIMRow.OLDSOT;
                MIMViewModel.NEWSOT = (double)MIMRow.NEWSOT;
                MIMViewModel.OLDNPS = (double)MIMRow.OLDNPS;
                MIMViewModel.NEWNPS = (double)MIMRow.NEWNPS;

                MIMViewModel.EDTA = ((DateTime)MIMRow.EDTA).ToString("yyyy-MM-dd");
                MIMViewModel.PNDA = ((DateTime)MIMRow.PNDA).ToString("yyyy-MM-dd");
                MIMViewModel.EDTB = ((DateTime)MIMRow.EDTB).ToString("yyyy-MM-dd");
                MIMViewModel.PNDB = ((DateTime)MIMRow.PNDB).ToString("yyyy-MM-dd");
                MIMViewModel.EDTC = ((DateTime)MIMRow.EDTC).ToString("yyyy-MM-dd");
                MIMViewModel.PNDC = ((DateTime)MIMRow.PNDC).ToString("yyyy-MM-dd");
                MIMViewModel.EDTD = ((DateTime)MIMRow.EDTD).ToString("yyyy-MM-dd");
                MIMViewModel.PNDD = ((DateTime)MIMRow.PNDD).ToString("yyyy-MM-dd");
                MIMViewModel.PNDE = ((DateTime)MIMRow.PNDE).ToString("yyyy-MM-dd");
                MIMViewModel.EDTE = ((DateTime)MIMRow.EDTE).ToString("yyyy-MM-dd");
                MIMViewModel.PNDF = ((DateTime)MIMRow.PNDF).ToString("yyyy-MM-dd");
                MIMViewModel.EDTF = ((DateTime)MIMRow.EDTF).ToString("yyyy-MM-dd");
                MIMViewModel.PNDM = ((DateTime)MIMRow.PNDM).ToString("yyyy-MM-dd");
                MIMViewModel.EDTM = ((DateTime)MIMRow.EDTM).ToString("yyyy-MM-dd");
                MIMViewModel.EDTS = ((DateTime)MIMRow.EDTS).ToString("yyyy-MM-dd");
                MIMViewModel.PNDS = ((DateTime)MIMRow.PNDS).ToString("yyyy-MM-dd");
                if (MIMViewModel == null || MIMViewModel.MIMMIC == null)
                    return null;
                return MIMViewModel;

            }

        }
    }
}