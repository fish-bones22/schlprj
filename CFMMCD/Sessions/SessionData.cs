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
        public bool VEM { get; set; }
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
        public CurrentPageSession() { }
        public CurrentPageSession(string self)
        {
            Self = GetLinkString(self);
        }
        public CurrentPageSession(string self, string parent)
        {
            Self = GetLinkString(self);
            Parent = GetLinkString(parent);
        }
        public CurrentPageSession( string self, string parent, string grandparent )
        {
            Self = GetLinkString(self);
            Parent = GetLinkString(parent);
            Grandparent = GetLinkString(grandparent);
        }

        public LinkString Grandparent { get; set; }
        public LinkString Parent { get; set; }
        public LinkString Self { get; set; }
        public LinkString Child { get; set; }
        public LinkString Grandchild { get; set; }

        public class LinkString
        {
            public string LinkName { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
        }

        public LinkString GetLinkString(string key)
        {
            if (key.Equals("HOME"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Home",
                    Action = "Index",
                    Controller = "Home"
                };
            else if (key.Equals("MIM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Menu Item Master",
                    Action = "Index",
                    Controller = "MenuItemMaster"
                };
            else if (key.Equals("RIM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Raw Item Master",
                    Action = "Index",
                    Controller = "RawItemMaster"
                };
            else if (key.Equals("UAP_CREATE"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Create Account",
                    Action = "CreateAccount",
                    Controller = "Account"
                };
            else if (key.Equals("UAP_EDIT"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Edit Account",
                    Action = "CreateAccount",
                    Controller = "Account"
                };
            else if (key.Equals("STP"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Store Profile",
                    Action = "Index",
                    Controller = "Store Profile"
                };
            else if (key.Equals("AUL"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Audit Log",
                    Action = "Index",
                    Controller = "AuditLog"
                };
            else if (key.Equals("TEG"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Text generation",
                    Action = "Index",
                    Controller = "TextGenerator"
                };
            else if (key.Equals("VEM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Vendor Master",
                    Action = "Index",
                    Controller = "VendorMaster"
                };
            else if(key.Equals("LOG"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Log out",
                    Action = "Login",
                    Controller = "Account"
                };
            else
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Login",
                    Action = "Login",
                    Controller = "Account"
                };
        }
    }
}