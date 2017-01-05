using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class RawItemPriceUpdateViewModel
    {
        public RawItemPriceUpdateViewModel()
        {
            RawItemPriceMasterList = new List<RawItemPriceUpdateViewModel>();
        }
        public string SearchItem { get; set; }
        public string RIM_VEM_ID { get; set; }
        public string RIMRIC { get; set; }
        public string RIMRID { get; set; }
        public string VEMVEN { get; set; }
        public string VEMDS1 { get; set; }
        public string RIMCPR { get; set; }
        public string RIMCPN { get; set; }
        public string RIMPDT { get; set; }
        public bool RIMSTA { get; set; }

        public List<RawItemPriceUpdateViewModel> RawItemPriceMasterList { get; set; }
    }
}