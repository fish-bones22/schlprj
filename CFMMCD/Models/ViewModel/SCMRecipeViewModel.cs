using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class SCMRecipeViewModel
    {
        public SCMRecipeViewModel()
        {
            RIMRIC = new List<string>();
            RIMRID = new List<string>();
            CSMSFQ = new List<string>();
            RIMCPR = new List<string>();
            CSMCWC = new List<string>();
            StoAtt = new List<string>();
            CSMID = new List<string>();
            RIMRIC.Add("");
            RIMRID.Add("");
            CSMSFQ.Add("");
            RIMCPR.Add("");
            CSMCWC.Add("");
            StoAtt.Add("");
            CSMID.Add("");
            RawItemList = new List<SCMRawItem>();
        }
        public string SearchItem { get; set; }
        public string CSMDES { get; set; }

        public List<string> CSMID { get; set; }
        public List<string> RIMRIC { get; set; }
        public List<string> RIMRID { get; set; }
        public List<string> CSMSFQ { get; set; }
        public List<string> RIMCPR { get; set; }
        public List<string> CSMCWC { get; set; }
        public List<string> StoAtt { get; set; }
        
        public List<SCMRawItem> RawItemList { get; set; }
        public List<SCMRecipeViewModel> SCMRecipeList { get; set; }
    }
    public class SCMRawItem
    {
        public string CSMID { get; set; }
        public string RIMRIC { get; set; }
        public string RIMRID { get; set; }
        public string CSMSFQ { get; set; }
        public string RIMCPR { get; set; }
        public string CSMCWC { get; set; }
        public string StoAtt { get; set; }
    }
}