using CFMMCD.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class TextGeneratorViewModel
    {
        public TextGeneratorViewModel()
        {
            TableDropDown tdd = new TableDropDown();
            StoreList = tdd.SetStoreInformationList();
        }
        public string PromoTitle { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public bool IncludeMIM { get; set; }
        public bool IncludeREC { get; set; }
        public bool IncludeRIM { get; set; }
        public bool IncludeAll { get; set; }
        public bool IncludeAllStores { get; set; }
        public List<CheckBoxList> StoreList { get; set; }
    }
    public class StoreInformation
    {
        public bool StoreCb { get; set; }
        public string Store_Name { get; set; }
        public string Store_No { get; set; }
        public string Ownership { get; set; }
    }
}