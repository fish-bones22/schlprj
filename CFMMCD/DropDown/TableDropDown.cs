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
    }
    public class GenericDropDownList
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}