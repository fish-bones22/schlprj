using CFMMCD.Generators;
using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class TextGeneratorManager
    {
        /*
         * Generates text from database
         */
        public bool GeneratePackets(TextGeneratorViewModel TGViewModel)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                for (int i = 0; i < TGViewModel.StoreList.Count(); i++)
                {
                    if (TGViewModel.StoreList[i].Cb || TGViewModel.IncludeAllStores)
                    {   
                        // Data creation
                        StringBuilder sb = new StringBuilder();
                        TextGenerator tg = new TextGenerator();
                        string Store_No = TGViewModel.StoreList[i].value;
                        string Store_Name = TGViewModel.StoreList[i].text;
                        string DateAndTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        DateTime DateFrom = DateTime.Parse(TGViewModel.DateFrom);
                        DateTime DateTo = DateTime.Parse(TGViewModel.DateTo);

                        // Append first line
                        sb.Append("01," + Store_No + "," + Store_Name + "," + DateAndTimeNow + "," + TGViewModel.PromoTitle + "\n");

                        if (TGViewModel.IncludeMIM || TGViewModel.IncludeAll)
                        {
                            List<CSHMIMP0> MIRows = GetMenutems(Store_No, DateFrom, DateTo);
                            foreach (CSHMIMP0 mi in MIRows)
                            {
                                sb.Append(tg.GenerateMenuItemMasterText(mi));
                            }
                        }
                        if (TGViewModel.IncludeRIM || TGViewModel.IncludeAll)
                        {
                            List<INVRIMP0> RIRows = GetRawItems(Store_No, DateFrom, DateTo);
                            foreach (INVRIMP0 ri in RIRows)
                            {
                                sb.Append(tg.GenerateRawItemMasterText(ri));
                            }
                        }
                        if (TGViewModel.IncludeREC || TGViewModel.IncludeAll)
                        {
                            List<INVRIRP0> RERows = GetRecipes(Store_No, DateFrom, DateTo);
                            foreach(INVRIRP0 re in RERows)
                            {
                                sb.Append(tg.GenerateRecipeText(re));
                            }
                        }

                        List<CSHVMLP0> VMRows = GetValueMeal(Store_No, DateFrom, DateTo);
                        foreach (CSHVMLP0 vm in VMRows) { 
                            sb.Append(tg.GenerateValueMealText(vm));
                        }

                        List<CSHPMGP0> PMGRows = GetProductMixGroup(Store_No, DateFrom, DateTo);
                        foreach (CSHPMGP0 pmg in PMGRows)
                        {
                            sb.Append(tg.GenerateProductMixText(pmg));
                        }

                        List<INVMGRP0> MGRows = GetMaterialGroup(Store_No, DateFrom, DateTo);
                        foreach (INVMGRP0 mg in MGRows)
                        {
                            sb.Append(tg.GenerateMaterialGroupText(mg));
                        }
                        List<INVUOMP0> UOMRows = GetUnitOfMeasure(Store_No, DateFrom, DateTo);
                        foreach (INVUOMP0 uom in UOMRows)
                        {
                            sb.Append(tg.GenerateInitOfMeasureText(uom));
                        }
                        List<INVVEMP0>  VERows = GetVendor(Store_No, DateFrom, DateTo);
                        foreach (INVVEMP0 ve in VERows)
                        {
                            sb.Append(tg.GenerateVendorText(ve));
                        }
                        // File creation
                        //  *Used for file name
                        string PaddedStore_No = TGViewModel.StoreList[i].value;
                        int len = PaddedStore_No.Length;
                        for (int j = 0; j < (5 - len); j++)
                            PaddedStore_No = "0" + PaddedStore_No;
                        string filename = "CFM" + PaddedStore_No + "_" + DateTime.Now.ToString("MMddyyyy_HHmm");
                        if (TGViewModel.IncludeAll)
                            filename += ".ALL";
                        else
                            filename += ".PRO";
                        string path = @"C:/SMS/SMSWEBCFM/";

                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        System.IO.File.WriteAllText(path+filename, sb.ToString());
                    }
                }
                return true;
            }
        }

        public List<CSHMIMP0> GetMenutems(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                Store_Profile Store = db.Store_Profile.Single(o => o.STORE_NO.ToString().Equals(Store_No));
                List<CSHMIMP0> sublist = db.CSHMIMP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<CSHMIMP0> list = new List<CSHMIMP0>();
                List<CSHMIMP0> masterlist = new List<CSHMIMP0>();
                list.AddRange(sublist);
                sublist = db.CSHMIMP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);

                sublist = list.Where(o => (o.Store != null && o.Store.Equals(Store_No))).ToList();
                masterlist.AddRange(sublist);
                
                sublist = list.Where(o => (o.Store != null && o.Store.Equals("ALL"))).ToList();
                masterlist.AddRange(sublist);

                masterlist.RemoveAll(o => (o.Except_Store != null && o.Except_Store.Equals(Store_No)));

                masterlist.RemoveAll(o => o.MIMDAT < DateFrom);
                masterlist.RemoveAll(o => o.MIMDAT > DateTo);

                foreach (CSHMIMP0 mi in masterlist)
                {
                    int tierToUse;
                    int tradingArea = (int) mi.Trading_Area;
                    if (tradingArea == 0)
                    {
                        // Price
                        tierToUse = (int) Store.BREAKFAST_PRICE_TIER;
                        Tier_Lookup tier = db.Tier_Lookup.Single(o => o.MIMMIC == mi.MIMMIC);
                        if (tierToUse == 1)
                        {
                            mi.MIMPND = tier.PNDA;
                            mi.MIMPRI = tier.OLDPRA;
                            mi.MIMPRO = tier.OLDPAO;
                            mi.MIMPRG = tier.OLDAOT;
                            mi.MIMNPI = tier.NEWPRA;
                            mi.MIMNPO = tier.NEWPAO;
                            mi.MIMNPD = tier.NEWAOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPA;
                                mi.MIMNNP = tier.NEWNPA;
                            }
                        }
                        else if (tierToUse == 2)
                        {
                            mi.MIMPND = tier.PNDB;
                            mi.MIMPRI = tier.OLDPRB;
                            mi.MIMPRO = tier.OLDPBO;
                            mi.MIMPRG = tier.OLDBOT;
                            mi.MIMNPI = tier.NEWPRB;
                            mi.MIMNPO = tier.NEWPBO;
                            mi.MIMNPD = tier.NEWBOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPB;
                                mi.MIMNNP = tier.NEWNPB;
                            }
                        }
                        else if (tierToUse == 3)
                        {
                            mi.MIMPND = tier.PNDC;
                            mi.MIMPRI = tier.OLDPRC;
                            mi.MIMPRO = tier.OLDPCO;
                            mi.MIMPRG = tier.OLDCOT;
                            mi.MIMNPI = tier.NEWPRC;
                            mi.MIMNPO = tier.NEWPCO;
                            mi.MIMNPD = tier.NEWCOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPC;
                                mi.MIMNNP = tier.NEWNPC;
                            }
                        }
                        else if (tierToUse == 4)
                        {
                            mi.MIMPND = tier.PNDD;
                            mi.MIMPRI = tier.OLDPRD;
                            mi.MIMPRO = tier.OLDPDO;
                            mi.MIMPRG = tier.OLDDOT;
                            mi.MIMNPI = tier.NEWPRD;
                            mi.MIMNPO = tier.NEWPDO;
                            mi.MIMNPD = tier.NEWDOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPD;
                                mi.MIMNNP = tier.NEWNPD;
                            }
                        }
                        else if (tierToUse == 5)
                        {
                            mi.MIMPND = tier.PNDE;
                            mi.MIMPRI = tier.OLDPRE;
                            mi.MIMPRO = tier.OLDPEO;
                            mi.MIMPRG = tier.OLDEOT;
                            mi.MIMNPI = tier.NEWPRE;
                            mi.MIMNPO = tier.NEWPEO;
                            mi.MIMNPD = tier.NEWEOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPE;
                                mi.MIMNNP = tier.NEWNPE;
                            }
                        }
                        else if (tierToUse == 6)
                        {
                            mi.MIMPND = tier.PNDF;
                            mi.MIMPRI = tier.OLDPRF;
                            mi.MIMPRO = tier.OLDPFO;
                            mi.MIMPRG = tier.OLDFOT;
                            mi.MIMNPI = tier.NEWPRF;
                            mi.MIMNPO = tier.NEWPFO;
                            mi.MIMNPD = tier.NEWFOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPF;
                                mi.MIMNNP = tier.NEWNPF;
                            }
                        }
                        else if (tierToUse == 7)
                        {
                            mi.MIMPND = tier.PNDM;
                            mi.MIMPRI = tier.OLDMDS;
                            mi.MIMPRO = tier.OLDMDO;
                            mi.MIMPRG = tier.OLDMOT;
                            mi.MIMNPI = tier.NEWMDS;
                            mi.MIMNPO = tier.NEWMDO;
                            mi.MIMNPD = tier.NEWMOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDMDN;
                                mi.MIMNNP = tier.NEWMDN;
                            }
                        }
                        else if (tierToUse == 8)
                        {
                            mi.MIMPND = tier.PNDS;
                            mi.MIMPRI = tier.OLDPRS;
                            mi.MIMPRO = tier.OLDPSO;
                            mi.MIMPRG = tier.OLDSOT;
                            mi.MIMNPI = tier.NEWPRS;
                            mi.MIMNPO = tier.NEWPSO;
                            mi.MIMNPD = tier.NEWSOT;
                            if (mi.MIMFGC.Trim().Equals("08"))
                            {
                                mi.MIMNPA = tier.OLDNPS;
                                mi.MIMNNP = tier.NEWNPS;
                            }
                        }
                    }
                }
                return masterlist;
            }
        }
        
        public List<INVRIMP0> GetRawItems(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVRIMP0> sublist = db.INVRIMP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<INVRIMP0> list = new List<INVRIMP0>();
                List<INVRIMP0> masterlist = new List<INVRIMP0>();
                list.AddRange(sublist);
                sublist = db.INVRIMP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);

                sublist = list.Where(o => (o.Store != null && o.Store.Equals(Store_No))).ToList();
                masterlist.AddRange(sublist);

                sublist = list.Where(o => (o.Store != null && o.Store.Equals("ALL"))).ToList();
                masterlist.AddRange(sublist);

                masterlist.RemoveAll(o => (o.Except_Store != null && o.Except_Store.Equals(Store_No)));
                masterlist.RemoveAll(o => o.RIMDAT < DateFrom);
                masterlist.RemoveAll(o => o.RIMDAT > DateTo);

                // Prices
                foreach (var v in masterlist)
                {
                    RIM_VEM_Lookup RVLRow = db.RIM_VEM_Lookup.Single(o => o.RIM_VEM_ID.Equals(v.RIMRIC + "" + v.RIMPVN));
                    v.RIMCPR = RVLRow.RIMCPR;
                    v.RIMCPN = RVLRow.RIMCPN;
                    v.RIMPDT = RVLRow.RIMPDT; 
                }

                return masterlist;
            }
        }

        public List<INVRIRP0> GetRecipes(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVRIRP0> sublist = db.INVRIRP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<INVRIRP0> list = new List<INVRIRP0>();
                list.AddRange(sublist);
                sublist = db.INVRIRP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);
                list.RemoveAll(o => o.RIRDAT < DateFrom);
                list.RemoveAll(o => o.RIRDAT > DateTo);
                return list;
            }
        }

        public List<CSHVMLP0> GetValueMeal(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new  CFMMCDEntities())
            {
                List<CSHVMLP0> sublist = db.CSHVMLP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<CSHVMLP0> list = new List<CSHVMLP0>();
                list.AddRange(sublist);
                sublist = db.CSHVMLP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);
                return list;
            }
        }

        public List<CSHPMGP0> GetProductMixGroup(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHPMGP0> sublist = db.CSHPMGP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<CSHPMGP0> list = new List<CSHPMGP0>();
                list.AddRange(sublist);
                sublist = db.CSHPMGP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);
                return list;
            }
        }

        public List<INVMGRP0> GetMaterialGroup(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVMGRP0> sublist = db.INVMGRP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<INVMGRP0> list = new List<INVMGRP0>();
                list.AddRange(sublist);
                sublist = db.INVMGRP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);
                return list;
            }
        }

        public List<INVUOMP0> GetUnitOfMeasure(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVUOMP0> sublist = db.INVUOMP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<INVUOMP0> list = new List<INVUOMP0>();
                List<INVUOMP0> masterlist = new List<INVUOMP0>();
                list.AddRange(sublist);
                sublist = db.INVUOMP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);
                list.RemoveAll(o => o.UOMDAT < DateFrom);
                list.RemoveAll(o => o.UOMDAT > DateTo);
                return list;
            }
        }
        public List<INVVEMP0> GetVendor(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVVEMP0> sublist = db.INVVEMP0.Where(o => o.STATUS.Equals("A")).ToList();
                List<INVVEMP0> list = new List<INVVEMP0>();
                List<INVVEMP0> masterlist = new List<INVVEMP0>();
                list.AddRange(sublist);
                sublist = db.INVVEMP0.Where(o => o.STATUS.Equals("E")).ToList();
                list.AddRange(sublist);

                sublist = list.Where(o => (o.Store != null && o.Store.Equals(Store_No))).ToList();
                masterlist.AddRange(sublist);

                sublist = list.Where(o => (o.Store != null && o.Store.Equals("ALL"))).ToList();
                masterlist.AddRange(sublist);

                masterlist.RemoveAll(o => (o.Except_Store != null && o.Except_Store.Equals(Store_No)));

                masterlist.RemoveAll(o => o.VEMDAT < DateFrom);
                masterlist.RemoveAll(o => o.VEMDAT > DateTo);
                return masterlist;
            }
        }

        public List<StoreProfileViewModel> GetStoreList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<StoreProfileViewModel> spList = new List<StoreProfileViewModel>();
                for (int i = 0; i < db.Store_Profile.Count(); i++)
                {
                    StoreProfileViewModel spViewModel = new StoreProfileViewModel();
                    spViewModel.STORE_NO = db.Store_Profile.ElementAt(i).STORE_NO.ToString();
                    spViewModel.STORE_NAME = db.Store_Profile.ElementAt(i).STORE_NAME;
                    spViewModel.LOCATION = db.Store_Profile.ElementAt(i).LOCATION.ToString();
                    spList.Add(spViewModel);
                }
                return spList;
            }
        }
    }
}