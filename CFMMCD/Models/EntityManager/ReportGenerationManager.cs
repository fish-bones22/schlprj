using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.EntityManager
{
    public class ReportGenerationManager
    {
        public bool RGtoExcel(MenuItemMasterViewModel MIMVM, MenuRecipeViewModel MRVM, RawItemMasterViewModel RIMVM, StoreProfileViewModel SPVM)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                using (DataSet ds = new DataSet())
                {
                    DataTable dt1 = new DataTable("MIM");
                    DataTable dt2 = new DataTable("RIM");
                    DataTable dt3 = new DataTable("STO");
                    DataTable dt4 = new DataTable("REC");

                    dt1.Columns.Add("MIMMIC", typeof(string));
                    dt1.Columns.Add("MIMSTA", typeof(string));
                    dt1.Columns.Add("MIMFGC", typeof(string));
                    dt1.Columns.Add("MIMNAM", typeof(string));
                    dt1.Columns.Add("MIMDSC", typeof(string));
                    dt1.Columns.Add("MIMSSC", typeof(string));
                    dt1.Columns.Add("MIMDPC", typeof(string));
                    dt1.Columns.Add("MIMCIN", typeof(string));
                    dt1.Columns.Add("MIMDGC", typeof(string));
                    dt1.Columns.Add("MIMASC", typeof(string));
                    dt1.Columns.Add("MIMTXC", typeof(string));
                    dt1.Columns.Add("MIMUTC", typeof(string));
                    dt1.Columns.Add("MIMDWE", typeof(string));
                    dt1.Columns.Add("MIMTCI", typeof(string));
                    dt1.Columns.Add("MIMPRI", typeof(string));
                    dt1.Columns.Add("MIMTCA", typeof(string));
                    dt1.Columns.Add("MIMPRO", typeof(string));
                    dt1.Columns.Add("MIMTCG", typeof(string));
                    dt1.Columns.Add("MIMPRG", typeof(string));
                    dt1.Columns.Add("MIMPND", typeof(string));
                    dt1.Columns.Add("MIMKBP", typeof(string));
                    dt1.Columns.Add("MIMKSC", typeof(string));
                    dt1.Columns.Add("MIMSKT", typeof(string));
                    dt1.Columns.Add("MIMGRP", typeof(string));
                    dt1.Columns.Add("MIMWLV", typeof(string));
                    dt1.Columns.Add("MIMWSD", typeof(string));
                    dt1.Columns.Add("MIMWGR", typeof(string));
                    dt1.Columns.Add("MIMHPT", typeof(string));
                    dt1.Columns.Add("MIMUSR", typeof(string));
                    dt1.Columns.Add("MIMDAT", typeof(string));
                    dt1.Columns.Add("MIMEDT", typeof(string));
                    dt1.Columns.Add("MIMFLG", typeof(string));
                    dt1.Columns.Add("MIMBIN", typeof(string));
                    dt1.Columns.Add("MIMBIT", typeof(string));
                    dt1.Columns.Add("MIMBIR", typeof(string));
                    dt1.Columns.Add("MIMBGR", typeof(string));
                    dt1.Columns.Add("MIMBQU", typeof(string));
                    dt1.Columns.Add("MIMGRA", typeof(string));
                    dt1.Columns.Add("MIMIST", typeof(string));
                    dt1.Columns.Add("MIMCLR", typeof(string));
                    dt1.Columns.Add("MIMNPI", typeof(string));
                    dt1.Columns.Add("MIMNPO", typeof(string));
                    dt1.Columns.Add("MIMNPD", typeof(string));
                    dt1.Columns.Add("MIMSKI", typeof(string));
                    dt1.Columns.Add("MIMBMI", typeof(string));
                    dt1.Columns.Add("MIMNPA", typeof(string));
                    dt1.Columns.Add("MIMNNP", typeof(string));
                    dt1.Columns.Add("MIMNPT", typeof(string));
                    dt1.Columns.Add("MIMMIC_NP6", typeof(string));
                    dt1.Columns.Add("MIMLON", typeof(string));
                    dt1.Columns.Add("STATUS", typeof(string));
                    dt1.Columns.Add("Category", typeof(string));
                    dt1.Columns.Add("Trading_Area", typeof(string));
                    dt1.Columns.Add("Location", typeof(string));
                    dt1.Columns.Add("Region", typeof(string));
                    dt1.Columns.Add("Province", typeof(string));
                    dt1.Columns.Add("City", typeof(string));
                    dt1.Columns.Add("Store", typeof(string));
                    dt1.Columns.Add("Except_Store", typeof(string));

                    List<CSHMIMP0> MIMRow = db.CSHMIMP0.ToList();
                    foreach (var vm in MIMRow)
                    {
                        var row = dt1.NewRow();
                        row["MIMMIC"] = vm.MIMMIC;
                        row["MIMSTA"] = vm.MIMSTA;
                        row["MIMFGC"] = vm.MIMFGC;
                        row["MIMNAM"] = vm.MIMNAM;
                        row["MIMDSC"] = vm.MIMDSC;
                        row["MIMSSC"] = vm.MIMSSC;
                        row["MIMDPC"] = vm.MIMDPC;
                        row["MIMCIN"] = vm.MIMCIN;
                        row["MIMDGC"] = vm.MIMDGC;
                        row["MIMASC"] = vm.MIMASC;
                        row["MIMTXC"] = vm.MIMTXC;
                        row["MIMUTC"] = vm.MIMUTC;
                        row["MIMDWE"] = vm.MIMDWE;
                        row["MIMTCI"] = vm.MIMTCI;
                        row["MIMPRI"] = vm.MIMPRI;
                        row["MIMTCA"] = vm.MIMTCA;
                        row["MIMPRO"] = vm.MIMPRO;
                        row["MIMTCG"] = vm.MIMTCG;
                        row["MIMPRG"] = vm.MIMPRG;
                        row["MIMPND"] = vm.MIMPND;
                        row["MIMKBP"] = vm.MIMKBP;
                        row["MIMKSC"] = vm.MIMKSC;
                        row["MIMSKT"] = vm.MIMSKT;
                        row["MIMGRP"] = vm.MIMGRP;
                        row["MIMWLV"] = vm.MIMWLV;
                        row["MIMWSD"] = vm.MIMWSD;
                        row["MIMWGR"] = vm.MIMWGR;
                        row["MIMHPT"] = vm.MIMHPT;
                        row["MIMUSR"] = vm.MIMUSR;
                        row["MIMDAT"] = vm.MIMDAT;
                        row["MIMEDT"] = vm.MIMEDT;
                        row["MIMFLG"] = vm.MIMFLG;
                        row["MIMBIN"] = vm.MIMBIN;
                        row["MIMBIT"] = vm.MIMBIT;
                        row["MIMBIR"] = vm.MIMBIR;
                        row["MIMBGR"] = vm.MIMBGR;
                        row["MIMBQU"] = vm.MIMBQU;
                        row["MIMGRA"] = vm.MIMGRA;
                        row["MIMIST"] = vm.MIMIST;
                        row["MIMCLR"] = vm.MIMCLR;
                        row["MIMNPI"] = vm.MIMNPI;
                        row["MIMNPO"] = vm.MIMNPO;
                        row["MIMNPD"] = vm.MIMNPD;
                        row["MIMSKI"] = vm.MIMSKI;
                        row["MIMBMI"] = vm.MIMBMI;
                        row["MIMNPA"] = vm.MIMNPA;
                        row["MIMNNP"] = vm.MIMNNP;
                        row["MIMNPT"] = vm.MIMNPT;
                        row["MIMMIC_NP6"] = vm.MIMMIC_NP6;
                        row["MIMLON"] = vm.MIMLON;
                        row["STATUS"] = vm.STATUS;
                        row["Category"] = vm.Category;
                        row["Trading_Area"] = vm.Trading_Area;
                        row["Location"] = vm.Location;
                        row["Region"] = vm.Region;
                        row["Province"] = vm.Province;
                        row["City"] = vm.City;
                        row["Store"] = vm.Store;
                        row["Except_Store"] = vm.Except_Store;
                        dt1.Rows.Add(row);
                    }

                    dt2.Columns.Add("RIMRIC", typeof(string));
                    dt2.Columns.Add("RIMVPC", typeof(string));
                    dt2.Columns.Add("RIMRID", typeof(string));
                    dt2.Columns.Add("RIMRIG", typeof(string));
                    dt2.Columns.Add("RIMTEM", typeof(string));
                    dt2.Columns.Add("RIMPGR", typeof(string));
                    dt2.Columns.Add("RIMPIS", typeof(string));
                    dt2.Columns.Add("RIMBVP", typeof(string));
                    dt2.Columns.Add("RIMBZP", typeof(string));
                    dt2.Columns.Add("RIMUMC", typeof(string));
                    dt2.Columns.Add("RIMUPC", typeof(string));
                    dt2.Columns.Add("RIMSUQ", typeof(string));
                    dt2.Columns.Add("RIMLAY", typeof(string));
                    dt2.Columns.Add("RIMCPR", typeof(string));
                    dt2.Columns.Add("RIMCPN", typeof(string));
                    dt2.Columns.Add("RIMPDT", typeof(string));
                    dt2.Columns.Add("RIMPVN", typeof(string));
                    dt2.Columns.Add("RIMSVN", typeof(string));
                    dt2.Columns.Add("RIMCWC", typeof(string));
                    dt2.Columns.Add("RIMPRO", typeof(string));
                    dt2.Columns.Add("RIMSE4", typeof(string));
                    dt2.Columns.Add("RIMERT", typeof(string));
                    dt2.Columns.Add("RIMUSF", typeof(string));
                    dt2.Columns.Add("RIMSDP", typeof(string));
                    dt2.Columns.Add("RIMUS1", typeof(string));
                    dt2.Columns.Add("RIMUS2", typeof(string));
                    dt2.Columns.Add("RIMUS3", typeof(string));
                    dt2.Columns.Add("RIMUS4", typeof(string));
                    dt2.Columns.Add("RIMUS5", typeof(string));
                    dt2.Columns.Add("RIMUSX", typeof(string));
                    dt2.Columns.Add("RIMMSD", typeof(string));
                    dt2.Columns.Add("RIMMSL", typeof(string));
                    dt2.Columns.Add("RIMLA1", typeof(string));
                    dt2.Columns.Add("RIMLA2", typeof(string));
                    dt2.Columns.Add("RIMLP1", typeof(string));
                    dt2.Columns.Add("RIMLP2", typeof(string));
                    dt2.Columns.Add("RIMSTA", typeof(string));
                    dt2.Columns.Add("RIMUSR", typeof(string));
                    dt2.Columns.Add("RIMDAT", typeof(string));
                    dt2.Columns.Add("RIMEDT", typeof(string));
                    dt2.Columns.Add("RIMFLG", typeof(string));
                    dt2.Columns.Add("RIMORD", typeof(string));
                    dt2.Columns.Add("RIMLIN", typeof(string));
                    dt2.Columns.Add("RIMADE", typeof(string));
                    dt2.Columns.Add("RIMBAR", typeof(string));
                    dt2.Columns.Add("STATUS", typeof(string));
                    dt2.Columns.Add("Store", typeof(string));
                    dt2.Columns.Add("Except_Store", typeof(string));
                    dt2.Columns.Add("Location", typeof(string));
                    dt2.Columns.Add("Stre_Attrib", typeof(string));
                    dt2.Columns.Add("Region", typeof(string));
                    dt2.Columns.Add("Province", typeof(string));
                    dt2.Columns.Add("City", typeof(string));

                    List<INVRIMP0> RIMRow = db.INVRIMP0.ToList();
                    foreach (var vm in RIMRow)
                    {
                        var row = dt2.NewRow();

                        row["RIMRIC"] = vm.RIMRIC;
                        row["RIMVPC"] = vm.RIMVPC;
                        row["RIMRID"] = vm.RIMRID;
                        row["RIMRIG"] = vm.RIMRIG;
                        row["RIMTEM"] = vm.RIMTEM;
                        row["RIMPGR"] = vm.RIMPGR;
                        row["RIMPIS"] = vm.RIMPIS;
                        row["RIMBVP"] = vm.RIMBVP;
                        row["RIMBZP"] = vm.RIMBZP;
                        row["RIMUMC"] = vm.RIMUMC;
                        row["RIMUPC"] = vm.RIMUPC;
                        row["RIMSUQ"] = vm.RIMSUQ;
                        row["RIMLAY"] = vm.RIMLAY;
                        row["RIMCPR"] = vm.RIMCPR;
                        row["RIMCPN"] = vm.RIMCPN;
                        row["RIMPDT"] = vm.RIMPDT;
                        row["RIMPVN"] = vm.RIMPVN;
                        row["RIMSVN"] = vm.RIMSVN;
                        row["RIMCWC"] = vm.RIMCWC;
                        row["RIMPRO"] = vm.RIMPRO;
                        row["RIMSE4"] = vm.RIMSE4;
                        row["RIMERT"] = vm.RIMERT;
                        row["RIMUSF"] = vm.RIMUSF;
                        row["RIMSDP"] = vm.RIMSDP;
                        row["RIMUS1"] = vm.RIMUS1;
                        row["RIMUS2"] = vm.RIMUS2;
                        row["RIMUS3"] = vm.RIMUS3;
                        row["RIMUS4"] = vm.RIMUS4;
                        row["RIMUS5"] = vm.RIMUS5;
                        row["RIMUSX"] = vm.RIMUSX;
                        row["RIMMSD"] = vm.RIMMSD;
                        row["RIMMSL"] = vm.RIMMSL;
                        row["RIMLA1"] = vm.RIMLA1;
                        row["RIMLA2"] = vm.RIMLA2;
                        row["RIMLP1"] = vm.RIMLP1;
                        row["RIMLP2"] = vm.RIMLP2;
                        row["RIMSTA"] = vm.RIMSTA;
                        row["RIMUSR"] = vm.RIMUSR;
                        row["RIMDAT"] = vm.RIMDAT;
                        row["RIMEDT"] = vm.RIMEDT;
                        row["RIMFLG"] = vm.RIMFLG;
                        row["RIMORD"] = vm.RIMORD;
                        row["RIMLIN"] = vm.RIMLIN;
                        row["RIMADE"] = vm.RIMADE;
                        row["RIMBAR"] = vm.RIMBAR;
                        row["STATUS"] = vm.STATUS;
                        row["Store"] = vm.Store;
                        row["Except_Store"] = vm.Except_Store;
                        row["Location"] = vm.Location;
                        row["Stre_Attrib"] = vm.Store_Attrib;
                        row["Region"] = vm.Region;
                        row["Province"] = vm.Province;
                        row["City"] = vm.City;
                        dt2.Rows.Add(row);
                    }

                    dt3.Columns.Add("STORE_NO", typeof(string));
                    dt3.Columns.Add("STORE_NAME", typeof(string));
                    dt3.Columns.Add("OWNERSHIP", typeof(string));
                    dt3.Columns.Add("BREAKFAST_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("REGULAR_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("DC_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("MDS_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("MCCAFE_LEVEL2_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("MCCAFE_LEVEL3_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("MCCAFE_BISTRO_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("PROJECT_GOLD_PRICE_TIER", typeof(string));
                    dt3.Columns.Add("BET", typeof(string));
                    dt3.Columns.Add("PROFIT_CENTER", typeof(string));
                    dt3.Columns.Add("REGION", typeof(string));
                    dt3.Columns.Add("PROVINCE", typeof(string));
                    dt3.Columns.Add("LOCATION", typeof(string));
                    dt3.Columns.Add("ADDRESS", typeof(string));
                    dt3.Columns.Add("CITY", typeof(string));
                    dt3.Columns.Add("FRESH_OR_FROZEN", typeof(string));
                    dt3.Columns.Add("PAPER_OR_PLASTIC", typeof(string));
                    dt3.Columns.Add("SOFT_SERVE_OR_VANILLA_POWDER_MIX", typeof(string));
                    dt3.Columns.Add("SIMPLOT_OR_MCCAIN", typeof(string));
                    dt3.Columns.Add("MCCOMICK_OR_GSF", typeof(string));
                    dt3.Columns.Add("STATUS", typeof(string));

                    List<Store_Profile> SPRow = db.Store_Profile.ToList();
                    foreach (var vm in SPRow)
                    {
                        var row = dt3.NewRow();

                        row["STORE_NO"] = vm.STORE_NO;
                        row["STORE_NAME"] = vm.STORE_NAME;
                        row["OWNERSHIP"] = vm.OWNERSHIP;
                        row["BREAKFAST_PRICE_TIER"] = vm.BREAKFAST_PRICE_TIER;
                        row["REGULAR_PRICE_TIER"] = vm.REGULAR_PRICE_TIER;
                        row["DC_PRICE_TIER"] = vm.DC_PRICE_TIER;
                        row["MDS_PRICE_TIER"] = vm.MDS_PRICE_TIER;
                        row["MCCAFE_LEVEL2_PRICE_TIER"] = vm.MCCAFE_LEVEL_2_PRICE_TIER;
                        row["MCCAFE_LEVEL3_PRICE_TIER"] = vm.MCCAFE_LEVEL_3_PRICE_TIER;
                        row["MCCAFE_BISTRO_PRICE_TIER"] = vm.MCCAFE_BISTRO_PRICE_TIER;
                        row["PROJECT_GOLD_PRICE_TIER"] = vm.PROJECT_GOLD_PRICE_TIER;
                        row["BET"] = vm.BET;
                        row["PROFIT_CENTER"] = vm.PROFIT_CENTER;
                        row["REGION"] = vm.REGION;
                        row["PROVINCE"] = vm.PROVINCE;
                        row["LOCATION"] = vm.LOCATION;
                        row["ADDRESS"] = vm.ADDRESS;
                        row["CITY"] = vm.CITY;
                        row["FRESH_OR_FROZEN"] = vm.FRESH_OR_FROZEN;
                        row["PAPER_OR_PLASTIC"] = vm.PAPER_OR_PLASTIC;
                        row["SOFT_SERVE_OR_VANILLA_POWDER_MIX"] = vm.SOFT_SERVE_OR_VANILLA_POWDER_MIX;
                        row["SIMPLOT_OR_MCCAIN"] = vm.SIMPLOT_OR_MCCAIN;
                        row["MCCOMICK_OR_GSF"] = vm.MCCORMICK_OR_GSF;
                        row["STATUS"] = vm.STATUS;
                        dt3.Rows.Add(row);
                    }

                    dt4.Columns.Add("RIRRID", typeof(string));
                    dt4.Columns.Add("RIRMIC", typeof(string));
                    dt4.Columns.Add("RIRRIC", typeof(string));
                    dt4.Columns.Add("RIRVPC", typeof(string));
                    dt4.Columns.Add("RIRSFQ", typeof(string));
                    dt4.Columns.Add("RIRCWC", typeof(string));
                    dt4.Columns.Add("RIRSTA", typeof(string));
                    dt4.Columns.Add("RIRVST", typeof(string));
                    dt4.Columns.Add("RIRUSR", typeof(string));
                    dt4.Columns.Add("RIRDAT", typeof(string));
                    dt4.Columns.Add("RIRFLG", typeof(string));
                    dt4.Columns.Add("STATUS", typeof(string));
                    dt4.Columns.Add("RIMCPR", typeof(string));
                    dt4.Columns.Add("LongName", typeof(string));
                    dt4.Columns.Add("Description", typeof(string));

                    List<INVRIRP0> RECRow = db.INVRIRP0.ToList();
                    foreach (var vm in RECRow)
                    {
                        var row = dt4.NewRow();

                        row["RIRRID"] = vm.RIRRID;
                        row["RIRMIC"] = vm.RIRMIC;
                        row["RIRRIC"] = vm.RIRRIC;
                        row["RIRVPC"] = vm.RIRVPC;
                        row["RIRSFQ"] = vm.RIRSFQ;
                        row["RIRCWC"] = vm.RIRCWC;
                        row["RIRSTA"] = vm.RIRSTA;
                        row["RIRVST"] = vm.RIRVST;
                        row["RIRUSR"] = vm.RIRUSR;
                        row["RIRDAT"] = vm.RIRDAT;
                        row["RIRFLG"] = vm.RIRFLG;
                        row["STATUS"] = vm.STATUS;
                        row["RIMCPR"] = vm.RIMCPR;
                        row["LongName"] = vm.LongName;
                        row["Description"] = vm.Description;
                        dt4.Rows.Add(row);
                    }

                    ds.Tables.Add(dt1);
                    ds.Tables.Add(dt2);
                    ds.Tables.Add(dt3);
                    ds.Tables.Add(dt4);

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            wb.Worksheets.Add(dt);
                        }
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.Charset = "";
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Report.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.End();
                        }
                    }
                }
            }
            return true;
        }
    }
}