using CFMMCD.DropDown;
using CFMMCD.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class MenuRecipeViewModel
    {
        public MenuRecipeViewModel()
        {
            TableDropDown tdd = new TableDropDown();
            RIRRIC = new List<string>();
            RIMRID = new List<string>();
            RIMCPR = new List<string>();
            RIRSFQ = new List<string>();
            RIRCWC = new List<string>();
            RIRSTA = new List<string>();
            STOATT = new List<string>();
            GroupList = tdd.SetGroupList();
            RIRRIC.Add("");
            RIMRID.Add("");
            RIMCPR.Add("");
            RIRSFQ.Add("");
            RIRCWC.Add("");
            RIRSTA.Add("");
            STOATT.Add("");
            MenuRecipeList = new List<MenuRecipe>();
        }
        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }
        public string RIRMIC { get; set; }
        public string MIMLON { get; set; }
        public string MIMDSC { get; set; }
        public int Group { get; set; }
        public List<RecipeTextBox> RecipeList { get; set; }
        public List<string> RIRRIC { get; set; }
        public List<string> RIMRID { get; set; }
        public List<string> RIMCPR { get; set; }
        public List<string> RIRSFQ { get; set; }
        public List<string> RIRCWC { get; set; }
        public List<string> RIRSTA { get; set; }
        public List<string> STOATT { get; set; }
        public List<GenericDropDownList> GroupList { get; set; }
        public List<MenuItem> MenuItemList { get; set; }
        public List<MenuRecipe> MenuRecipeList { get; set; }
        public List<RawItem> RawItemList { get; set; }
    }

    public class MenuRecipe
    {
        public string RIRRID { get; set; }
        public string RIRRIC { get; set; }
        public string RIMRID { get; set; }
        public string RIMCPR { get; set; }
        public string RIRSFQ { get; set; }
        public string RIRCWC { get; set; }
        public string RIRSTA { get; set; }
        public string STOATT { get; set; }
        public string PreviousRIRRIC { get; set; }
    }
}