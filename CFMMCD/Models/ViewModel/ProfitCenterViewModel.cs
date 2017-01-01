using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Models.ViewModel
{
    public class ProfitCenterViewModel
    {
        public string Id { get; set; }
        public string PRFCNT { get; set; }
        // Added 
        public List<ProfitCenterViewModel> PCList { get; set; }
    }
}