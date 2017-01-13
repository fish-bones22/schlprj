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
                    int tierIdToUse;
                    string tierToUse = "" ;
                    int tradingArea = (int) mi.Trading_Area;
                    if (tradingArea == 1)
                    {
                        tierIdToUse = (int) Store.BREAKFAST_PRICE_TIER;
                        tierToUse = db.Breakfast_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 2)
                    {
                        tierIdToUse = (int)Store.REGULAR_PRICE_TIER;
                        tierToUse = db.Regular_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 3)
                    {
                        tierIdToUse = (int)Store.MDS_PRICE_TIER;
                        tierToUse = db.MDS_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 4)
                    {
                        tierIdToUse = (int)Store.DC_PRICE_TIER;
                        tierToUse = db.Dessert_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 5)
                    {
                        tierIdToUse = (int)Store.PROJECT_GOLD_PRICE_TIER;
                        tierToUse = db.Project_Gold_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 6)
                    {
                        tierIdToUse = (int)Store.MCCAFE_LEVEL_2_PRICE_TIER;
                        tierToUse = db.McCafe_Level_2_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 7)
                    {
                        tierIdToUse = (int)Store.MCCAFE_LEVEL_3_PRICE_TIER;
                        tierToUse = db.McCafe_Level_3_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    else if (tradingArea == 8)
                    {
                        tierIdToUse = (int)Store.MCCAFE_BISTRO_PRICE_TIER;
                        tierToUse = db.McCafe_Bistro_Price_Tier.Single(o => o.Id == tierIdToUse).Price_Tier;
                    }
                    string id = mi.MIMMIC + tierToUse;
                    MIM_Price MIMPriceRow;
                    if (db.MIM_Price.Where(o => o.Id.Equals(id)).Any())
                    {
                        MIMPriceRow = db.MIM_Price.Single(o => o.Id.Equals(id)); if (MIMPriceRow.MIMPRI != null && !MIMPriceRow.MIMPRI.Equals(""))
                            mi.MIMPRI = MIMPriceRow.MIMPRI; // Eat in
                        if (MIMPriceRow.MIMPRO != null && !MIMPriceRow.MIMPRO.Equals(""))
                            mi.MIMPRO = MIMPriceRow.MIMPRO; // Take out
                        if (MIMPriceRow.MIMPRG != null && !MIMPriceRow.MIMPRG.Equals(""))
                            mi.MIMPRG = MIMPriceRow.MIMPRG; // Other
                        if (MIMPriceRow.MIMNPA != null && !MIMPriceRow.MIMNPA.Equals(""))
                            mi.MIMNPA = MIMPriceRow.MIMNPA; // Non-product
                                                                          // New
                        if (MIMPriceRow.MIMNPI != null && !MIMPriceRow.MIMNPI.Equals(""))
                            mi.MIMNPI = MIMPriceRow.MIMNPI; // Eat in new
                        if (MIMPriceRow.MIMNPO != null && !MIMPriceRow.MIMNPO.Equals(""))
                            mi.MIMNPO = MIMPriceRow.MIMNPO; // Take out new
                        if (MIMPriceRow.MIMNPD != null && !MIMPriceRow.MIMNPD.Equals(""))
                            mi.MIMNPD = MIMPriceRow.MIMNPD; // Other new
                        if (MIMPriceRow.MIMNNP != null && !MIMPriceRow.MIMNNP.Equals(""))
                            mi.MIMNNP = MIMPriceRow.MIMNNP; // Non-product new
                                                                          // Effective date
                        if (MIMPriceRow.MIMPND != null && !MIMPriceRow.MIMPND.Equals(""))
                            mi.MIMPND = MIMPriceRow.MIMPND;
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