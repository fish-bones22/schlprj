using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFMMCD.Sessions
{
    [Serializable]
    public class UserSession
    {
        public string Username { get; set; }
        public int UserID { get; set; }
    }

    [Serializable]
    public class UserAccessSession
    {
        public bool MIM { get; set; }
        public bool RIM { get; set; }
        public bool MER { get; set; }
        public bool STP { get; set; }
        public bool SCM { get; set; }
        public bool VEN { get; set; }
        public bool VAM { get; set; }
        public bool UAP { get; set; }
        public bool MIP { get; set; }
        public bool RIP { get; set; }
        public bool AUL { get; set; }
        public bool REG { get; set; }
        public bool TEG { get; set; }
        public bool TIP { get; set; }
        public bool BUE { get; set; }
        public bool OWN { get; set; }
        public bool PRC { get; set; }
        public bool LOC { get; set; }
    }
    [Serializable]
    public class CurrentPageSession
    {
        public LinkString Grandparent { get; set; }
        public LinkString Parent { get; set; }
        public LinkString Self { get; set; }
        public LinkString Child { get; set; }
        public LinkString Grandchild { get; set; }

        public class LinkString
        {
            public string Action { get; set; }
            public string Controller { get; set; }
        }
    }
}