using CFMMCD.DropDown;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class MenuItemMasterViewModel
    {
        public MenuItemMasterViewModel() {
            TableDropDown tdd = new TableDropDown();
            LocationList = tdd.SetLocationDropDown();
            StoreList = tdd.SetStoreDropDown();
            MenuItemMasterList = new List<MenuItem>();
            MenuRecipeList = new List<MenuRecipe>();
            TradingAreaList = tdd.SetTradingAreaList();
            CategoryList = tdd.SetCategoryList();
            MIMFGCList = tdd.SetPMGList("PMGFGC");
            MIMHPTList = tdd.SetPMGList("PMGHPT");
            MIMWGRList = tdd.SetPMGList("PMGWGR");
        }

        public string SearchItem { get; set; }
        public bool InactiveItemsCb { get; set; }

        // CHMMIMP0 Table Items
        [Required(ErrorMessage = "This field is required")]
        public string MIMMIC { get; set; }
        public string MIMSTA { get; set; }
        public string MIMFGC { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string MIMNAM { get; set; }
        public string MIMLON { get; set; }
        public string MIMDSC { get; set; }
        public string MIMDPC { get; set; }
        public string MIMTCI { get; set; }
        public string MIMPRI { get; set; }
        public string MIMTCA { get; set; }
        public string MIMPRO { get; set; }
        public string MIMTCG { get; set; }
        public string MIMPRG { get; set; }
        public string MIMPND { get; set; }
        public string MIMWGR { get; set; }
        public string MIMHPT { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string MIMUTC { get; set; }
        public string MIMEDT { get; set; }
        public string MIMNPI { get; set; }
        public string MIMNPO { get; set; }
        public string MIMNPD { get; set; }
        public string MIMNPA { get; set; }
        public string MIMNNP { get; set; }
        public string MIMNPT { get; set; }

        // NP6 Items
        public string MIMMIC_NP6 { get; set; }
        public string MIMNAM_NP6 { get; set; }
        public string MIMLON_NP6 { get; set; }

        // Misc
        public string Trading_Area { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Store { get; set; }
        public bool SelectAllCb { get; set; }
        public bool SelectExcept { get; set; }

        public List<MenuItem> MenuItemMasterList { get; set; }

        // Tier items
        public double OLDPRA { get; set; }
        public double NEWPRA { get; set; }
        public double OLDPAO { get; set; }
        public double NEWPAO { get; set; }
        public double OLDAOT { get; set; }
        public double NEWAOT { get; set; }
        public double OLDNPA { get; set; }
        public double NEWNPA { get; set; }
        public double OLDPRB { get; set; }
        public double NEWPRB { get; set; }
        public double OLDPBO { get; set; }
        public double NEWPBO { get; set; }
        public double OLDBOT { get; set; }
        public double NEWBOT { get; set; }
        public double OLDNPB { get; set; }
        public double NEWNPB { get; set; }
        public double OLDPRC { get; set; }
        public double NEWPRC { get; set; }
        public double OLDPCO { get; set; }
        public double NEWPCO { get; set; }
        public double OLDCOT { get; set; }
        public double NEWCOT { get; set; }
        public double OLDNPC { get; set; }
        public double NEWNPC { get; set; }
        public double OLDPRD { get; set; }
        public double NEWPRD { get; set; }
        public double OLDPDO { get; set; }
        public double NEWPDO { get; set; }
        public double OLDDOT { get; set; }
        public double NEWDOT { get; set; }
        public double OLDNPD { get; set; }
        public double NEWNPD { get; set; }
        public double OLDPRE { get; set; }
        public double NEWPRE { get; set; }
        public double OLDPEO { get; set; }
        public double NEWPEO { get; set; }
        public double OLDEOT { get; set; }
        public double NEWEOT { get; set; }
        public double OLDNPE { get; set; }
        public double NEWNPE { get; set; }
        public double OLDPRF { get; set; }
        public double NEWPRF { get; set; }
        public double OLDPFO { get; set; }
        public double NEWPFO { get; set; }
        public double OLDFOT { get; set; }
        public double NEWFOT { get; set; }
        public double OLDNPF { get; set; }
        public double NEWNPF { get; set; }
        public double OLDMDS { get; set; }
        public double NEWMDS { get; set; }
        public double OLDMDO { get; set; }
        public double NEWMDO { get; set; }
        public double OLDMOT { get; set; }
        public double NEWMOT { get; set; }
        public double OLDMDN { get; set; }
        public double NEWMDN { get; set; }
        public double OLDPRS { get; set; }
        public double NEWPRS { get; set; }
        public double OLDPSO { get; set; }
        public double NEWPSO { get; set; }
        public double OLDSOT { get; set; }
        public double NEWSOT { get; set; }
        public double OLDNPS { get; set; }
        public double NEWNPS { get; set; }

        public string EDTA { get; set; }
        public string PNDA { get; set; }
        public string EDTB { get; set; }
        public string PNDB { get; set; }
        public string EDTC { get; set; }
        public string PNDC { get; set; }
        public string EDTD { get; set; }
        public string PNDD { get; set; }
        public string PNDE { get; set; }
        public string EDTE { get; set; }
        public string PNDF { get; set; }
        public string EDTF { get; set; }
        public string PNDM { get; set; }
        public string EDTM { get; set; }
        public string EDTS { get; set; }
        public string PNDS { get; set; }

        public string EffectiveDate { get; set; }

        public List<GenericDropDownList> TradingAreaList { get; set; }
        public List<GenericDropDownList> CategoryList { get; set; }
        public List<GenericDropDownList> MIMFGCList { get; set; }
        public List<GenericDropDownList> MIMHPTList { get; set; }
        public List<GenericDropDownList> MIMWGRList { get; set; }
        public List<GenericDropDownList> StoreList { get; set; }
        public List<GenericDropDownList> LocationList { get; set; }
        public List<MenuRecipe> MenuRecipeList { get; set; }
    }
}