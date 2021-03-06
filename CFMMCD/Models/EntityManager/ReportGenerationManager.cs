﻿using CFMMCD.Models.DB;
using CFMMCD.Models.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace CFMMCD.Models.EntityManager
{
    public class ReportGenerationManager
    {
        public bool ManageReport(ReportGenerationViewModel RGViewModel, string command)
        {
            using(CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CSHMIMP0> MIMRow = new List<CSHMIMP0>();
                List<INVRIRP0> RECRow = new List<INVRIRP0>();
                List<INVRIMP0> RIMRow = new List<INVRIMP0>();
                List<Store_Profile> SPRow = new List<Store_Profile>();

                if (RGViewModel.ItemToExport.Equals("Store"))
                {
                    SPRow = db.Store_Profile.Where(o => o.STORE_NO.ToString().Equals(RGViewModel.Store)).ToList();
                }
                else
                {
                    if (RGViewModel.MenuItem)
                    {
                        MIMRow = db.CSHMIMP0.ToList();
                        if (RGViewModel.DateFrom != null && !RGViewModel.DateFrom.Equals(""))
                            MIMRow = MIMRow.Where(o => o.MIMDAT >= DateTime.Parse(RGViewModel.DateFrom)).ToList();

                        if (RGViewModel.DateTo != null && !RGViewModel.DateTo.Equals(""))
                            MIMRow = MIMRow.Where(o => o.MIMDAT <= DateTime.Parse(RGViewModel.DateTo)).ToList();
                    }
                    if (RGViewModel.RawItem)
                    {
                        RIMRow = db.INVRIMP0.ToList();
                        if (!RGViewModel.FRESH_OR_FROZEN.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.FRESH_OR_FROZEN)).ToList();
                        else if (!RGViewModel.PAPER_OR_PLASTIC.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.PAPER_OR_PLASTIC)).ToList();
                        else if (!RGViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.SOFT_SERVE_OR_VANILLA_POWDER_MIX)).ToList();
                        else if (!RGViewModel.SIMPLOT_OR_MCCAIN.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.SIMPLOT_OR_MCCAIN)).ToList();
                        else if (!RGViewModel.MCCORMICK_OR_GSF.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.MCCORMICK_OR_GSF)).ToList();
                        else if (!RGViewModel.FRESHB_OR_FROZENB.Equals("0"))
                            RIMRow = RIMRow.Where(o => o.Store_Attrib.Equals(RGViewModel.FRESHB_OR_FROZENB)).ToList();

                        if (RGViewModel.DateFrom != null && !RGViewModel.DateFrom.Equals(""))
                            RIMRow = RIMRow.Where(o => o.RIMDAT >= DateTime.Parse(RGViewModel.DateFrom)).ToList();

                        if (RGViewModel.DateTo != null && !RGViewModel.DateTo.Equals(""))
                            RIMRow = RIMRow.Where(o => o.RIMDAT <= DateTime.Parse(RGViewModel.DateTo)).ToList();
                    }
                    if (RGViewModel.Recipe)
                    {
                        RECRow = db.INVRIRP0.ToList();
                        if (RGViewModel.DateFrom != null && !RGViewModel.DateFrom.Equals(""))
                            RECRow = RECRow.Where(o => o.RIRDAT >= DateTime.Parse(RGViewModel.DateFrom)).ToList();

                        if (RGViewModel.DateTo != null && !RGViewModel.DateTo.Equals(""))
                            RECRow = RECRow.Where(o => o.RIRDAT <= DateTime.Parse(RGViewModel.DateTo)).ToList();
                    }
                }
                if ((MIMRow == null || MIMRow.Count() <= 0) &&
                    (RECRow == null || RECRow.Count() <= 0) &&
                    (RIMRow == null || RIMRow.Count() <= 0) &&
                    (SPRow == null || SPRow.Count() <= 0))
                    return false;

                if (command.Equals("toPDF"))
                {
                    ExportToPDF(MIMRow, RECRow, RIMRow, SPRow);
                }
                else if (command.Equals("toExcel"))
                {
                    ExportToExcel(MIMRow, RECRow, RIMRow, SPRow);
                }
                return true;
            }
        }

        public DataSet PrepareDataTable (List<CSHMIMP0> MIMRow, List<INVRIRP0> RECRow, List<INVRIMP0> RIMRow, List<Store_Profile> SPRow)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                using (DataSet ds = new DataSet())
                {
                    if (MIMRow.Count() > 0)
                    {
                        DataTable dt1 = new DataTable("MIM");
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
                            dt1.Rows.Add(row);
                        }
                        ds.Tables.Add(dt1);
                    }

                    if (RIMRow.Count() > 0)
                    {
                        DataTable dt2 = new DataTable("RIM");
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
                            dt2.Rows.Add(row);
                        }
                        ds.Tables.Add(dt2);
                    }

                    if (SPRow.Count() > 0)
                    {
                        DataTable dt3 = new DataTable("STO");
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
                        dt3.Columns.Add("FRESHB_OR_FROZENB", typeof(string));

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
                            row["FRESHB_OR_FROZENB"] = vm.FRESHB_OR_FROZENB;
                            dt3.Rows.Add(row);
                        }
                        ds.Tables.Add(dt3);
                    }

                    if (RECRow.Count() > 0)
                    {
                        DataTable dt4 = new DataTable("REC");
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
                            dt4.Rows.Add(row);
                        }
                        ds.Tables.Add(dt4);
                    }
                   
                    return ds;
                }
            }

       }

        public bool ExportToExcel(List<CSHMIMP0> MIMRow, List<INVRIRP0> RECRow, List<INVRIMP0> RIMRow, List<Store_Profile> SPRow)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                DataSet ds = PrepareDataTable(MIMRow, RECRow, RIMRow, SPRow);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        wb.Worksheets.Add(dt);
                    }
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    string filename = "attachment;filename=Report" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", filename);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
            }
            return true;
        }
        public bool ExportToPDF(List<CSHMIMP0> MIMRow, List<INVRIRP0> RECRow, List<INVRIMP0> RIMRow, List<Store_Profile> SPRow)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                DataSet ds = PrepareDataTable(MIMRow, RECRow, RIMRow, SPRow);
                List<GridView> GVList = new List<GridView>();
                string filename = "attachment;filename=Report" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", filename);
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                foreach (var dt in ds.Tables)
                {
                    //Creates a dummy Gridview
                    GridView gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();
                    GVList.Add(gv);
                    gv.RenderControl(hw);
                }
                
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(iTextSharp.text.PageSize.B2.Rotate(), 10, 10, 10, 10);
#pragma warning disable CS0612 // Type or member is obsolete
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
#pragma warning restore CS0612 // Type or member is obsolete
                PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                HttpContext.Current.Response.Write(pdfDoc);
                HttpContext.Current.Response.End();
            }
            return true;

        }
}
    }