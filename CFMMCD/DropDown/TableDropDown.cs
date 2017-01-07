using CFMMCD.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.DropDown
{
    public class TableDropDown
    {
        public List<GenericDropDownList> SetOwnershipDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.OWNERSHIPs)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.OWNSHP;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetProfitCenterDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.PROFIT_CEN)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.PRFCNT;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetLocationDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.LOCATIONs)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.LOCATN;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetRegularPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Regular_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetProjectGoldPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Project_Gold_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetMDSPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.MDS_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetMcCafeLevel3PriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.McCafe_Level_3_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetMcCafeLevel2PriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.McCafe_Level_2_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetMcCafeBistroPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.McCafe_Bistro_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetDessertPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Dessert_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetBreakfastPriceTierDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Breakfast_Price_Tier)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Price_Tier;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetStoreDropDown()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Store_Profile)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.STORE_NAME;
                    option.value = i.STORE_NO.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<CheckBoxList> SetBusinessExtensionList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CheckBoxList> list = new List<CheckBoxList>();
                foreach (var i in db.BUSINESS_EXT)
                {
                    CheckBoxList cbList = new CheckBoxList();
                    cbList.Cb = false;
                    cbList.value = i.ID.ToString();
                    cbList.text = i.LONGNM.Trim();
                    list.Add(cbList);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetPrimaryVendorList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.INVVEMP0)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.VEMDS1;
                    option.value = i.VEMVEN.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetTradingAreaList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Trading_Area)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Trading_Areea;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetCategoryList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.Categories)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.Category1;
                    option.value = i.Id.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetPMGList(String group)
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.CSHPMGP0.Where(o => o.PMGGRP.Equals(group)))
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.PMGTXT;
                    option.value = i.PMGNUM;
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetMaterialsGroupList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.INVMGRP0)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.MGRTXT;
                    option.value = i.MGRGRP;
                    list.Add(option);
                }
                return list;
            }
        }
        public List<GenericDropDownList> SetUnitOfMeasureList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<GenericDropDownList> list = new List<GenericDropDownList>();
                foreach (var i in db.INVUOMP0)
                {
                    GenericDropDownList option = new GenericDropDownList();
                    option.text = i.UOMDEL;
                    option.value = i.UOMDES;
                    list.Add(option);
                }
                return list;
            }
        }
        public List<CheckBoxList> SetVendorList()
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<CheckBoxList> list = new List<CheckBoxList>();
                foreach ( var i in db.INVVEMP0 )
                {
                    CheckBoxList option = new CheckBoxList();
                    option.Cb = false;
                    option.value = i.VEMVEN.ToString();
                    option.text = i.VEMDS1.ToString();
                    list.Add(option);
                }
                return list;
            }
        }
        public List<RecipeTextBox> SetRecipeList()
        {
            using ( CFMMCDEntities db = new CFMMCDEntities() )
            {
                List<RecipeTextBox> list = new List<RecipeTextBox>();
                foreach (var i in db.INVRIRP0)
                {
                    RecipeTextBox option = new RecipeTextBox();
                    option.RIRRID = i.RIRRID.Trim();
                    option.RIRRIC = i.RIRRIC.ToString();
                    option.RIRMIC = i.RIRMIC.ToString();
                    option.RIMRID = db.INVRIMP0.Single(o => o.RIMRIC == i.RIRRIC).RIMRID;
                    option.RIRSFQ = i.RIRSFQ.ToString();
                    option.RIRCWC = i.RIRCWC;
                    option.RIRSTA = i.RIRSTA;
                    option.MIMLON = db.CSHMIMP0.Single(o => o.MIMMIC == i.RIRMIC).MIMLON;
                    option.STOATT = "";
                    list.Add(option);
                }
                return list;
            }
        }
        public List<CheckBoxList> SetStoreInformationList()
        {
            using (CFMMCDEntities db = new CFMMCDEntities())
            {
                List<CheckBoxList> list = new List<CheckBoxList>();
                foreach (var i in db.Store_Profile)
                {
                    CheckBoxList option = new CheckBoxList();
                    option.Cb = false;
                    option.value = i.STORE_NO.ToString();
                    option.text = i.STORE_NAME;
                    string st = i.OWNERSHIP.Trim();
                    option.text2 = db.OWNERSHIPs.Single(o => o.Id.ToString().Equals(st)).OWNSHP;
                    list.Add(option);
                }
                return list;
            }
        }
    }

    public class GenericDropDownList
    {
        public string value { get; set; }
        public string text { get; set; }
    }
    public class CheckBoxList
    {
        public bool Cb { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public string text2 { get; set; }
    }
    public class RecipeTextBox
    {
        public string RIRRID { get; set; }
        public string RIRRIC { get; set; }
        public string RIMRID { get; set; }
        public string RIRMIC { get; set; }
        public string RIRSFQ { get; set; }
        public string RIRCWC { get; set; }
        public string RIRSTA { get; set; }
        public string MIMLON { get; set; }
        public string STOATT { get; set; }
    }
}