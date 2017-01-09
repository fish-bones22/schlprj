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
                            List<INVRIMP0> MIRows = GetRawItems(Store_No, DateFrom, DateTo);
                            foreach (INVRIMP0 mi in MIRows)
                            {
                                sb.Append(tg.GenerateRawItemMasterText(mi));
                            }
                        }
                        // File creation
                        //  *Used for file name
                        string PaddedStore_No = TGViewModel.StoreList[i].value;
                        for (int j = 0; j < 5 - PaddedStore_No.Length + 2; j++)
                            PaddedStore_No = "0" + PaddedStore_No;
                        string filename = "CFM" + PaddedStore_No + "_" + DateTime.Now.ToString("MMddyyyy_HHmm");
                        if (TGViewModel.IncludeAll)
                            filename += ".ALL";
                        else
                            filename += ".PRO";
                        System.IO.File.WriteAllText(@"C:/SMS/SMSWEBCFM/" + filename, sb.ToString());
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
                List<CSHMIMP0> list = db.CSHMIMP0.Where(o => o.Store.Equals(Store_No)).ToList();
                list.AddRange(db.CSHMIMP0.Where(o => o.Store.Equals("ALL")).ToList());
                list.RemoveAll(o => (o.Except_Store != null && o.Except_Store.Equals(Store_No)));
                list.Where(o => o.STATUS.Equals("A"));
                list.Where(o => o.STATUS.Equals("E"));
                list.Where(o => o.MIMEDT >= DateFrom);
                list.Where(o => o.MIMEDT <= DateTo);

                foreach (CSHMIMP0 mi in list)
                {
                    string tierToUse;
                    int tradingArea = (int) mi.Trading_Area;
                    if (tradingArea == 0)
                    {
                        // Price
                        tierToUse = Store.BREAKFAST_PRICE_TIER;
                        Tier_Lookup tier = db.Tier_Lookup.Single(o => o.MIMMIC == mi.MIMMIC);
                        if (tierToUse.Equals("1"))
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
                        else if (tierToUse.Equals("2"))
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
                        else if (tierToUse.Equals("3"))
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
                        else if (tierToUse.Equals("4"))
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
                        else if (tierToUse.Equals("5"))
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
                        else if (tierToUse.Equals("6"))
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
                        else if (tierToUse.Equals("7"))
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
                        else if (tierToUse.Equals("8"))
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
                return list;
            }
        }
        
        public List<INVRIMP0> GetRawItems(string Store_No, DateTime DateFrom, DateTime DateTo)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<INVRIMP0> list = db.INVRIMP0.Where(o => o.Store.Equals(Store_No)).ToList();
                list.AddRange(db.INVRIMP0.Where(o => o.Store.Equals("ALL")).ToList());
                list.RemoveAll(o => (o.Except_Store != null && o.Except_Store.Equals(Store_No)));
                list.Where(o => o.STATUS.Equals("A"));
                list.Where(o => o.STATUS.Equals("E"));
                list.Where(o => o.RIMEDT >= DateFrom);
                list.Where(o => o.RIMEDT <= DateTo);
                return list;
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
                    spViewModel.LOCATION = db.Store_Profile.ElementAt(i).LOCATION;
                    spList.Add(spViewModel);
                }
                return spList;
            }
        }
    }
}