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
            else if (key.Equals("MER"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Menu Recipe Master",
                    Action = "Index",
                    Controller = "MenuRecipe"
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
                    Action = "EditAccount",
                    Controller = "Account"
                };
            else if (key.Equals("UAP_ACCESS_EDIT"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Edit Account Access",
                    Action = "EditAccess",
                    Controller = "Account"
                };
            else if (key.Equals("STP"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Store Profile",
                    Action = "Index",
                    Controller = "StoreProfile"
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
            else if (key.Equals("REG"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Report generation",
                    Action = "Index",
                    Controller = "ReportGeneration"
                };
            else if (key.Equals("VEM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Vendor Master",
                    Action = "Index",
                    Controller = "VendorMaster"
                };
            else if (key.Equals("LOG"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Log out",
                    Action = "Login",
                    Controller = "Account"
                };
            else if (key.Equals("RIP"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Raw Item Price Update",
                    Action = "Index",
                    Controller = "RawItemPriceUpdate"
                };
            else if (key.Equals("MIP"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Menu Item Price Update",
                    Action = "Index",
                    Controller = "MenuItemPriceUpdate"
                };
            else if (key.Equals("BUE"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Business Extensions",
                    Action = "Index",
                    Controller = "BusinessExtension"
                };
            else if (key.Equals("LOC"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Location",
                    Action = "Index",
                    Controller = "Location"
                };
            else if (key.Equals("PRC"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Profit Center",
                    Action = "Index",
                    Controller = "ProfitCenter"
                };
            else if (key.Equals("OWN"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Ownership",
                    Action = "Index",
                    Controller = "Ownership"
                };
            else if (key.Equals("VAM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Value Meal",
                    Action = "Index",
                    Controller = "ValueMeal"
                };
            else if (key.Equals("SCM"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "SCM Recipe",
                    Action = "Index",
                    Controller = "SCMRecipe"
                };
            else if (key.Equals("TIP_REG"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Regular Price Tier",
                    Action = "Index",
                    Controller = "RegularPriceTier"
                };
            else if (key.Equals("TIP_BRE"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Breakfast Price Tier",
                    Action = "Index",
                    Controller = "BreakfastPriceTier"
                };
            else if (key.Equals("TIP_DES"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Dessert Price Tier",
                    Action = "Index",
                    Controller = "DessertPriceTier"
                };
            else if (key.Equals("TIP_BIS"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "McCafe Bistro Price Tier",
                    Action = "Index",
                    Controller = "McCafeBistroPriceTier"
                };
            else if (key.Equals("TIP_LE2"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "McCafe Level 2 Price Tier",
                    Action = "Index",
                    Controller = "McCafeLevel2PriceTier"
                };
            else if (key.Equals("TIP_LE3"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "McCafe Level 3 Price Tier",
                    Action = "Index",
                    Controller = "McCafeLevel3PriceTier"
                };
            else if (key.Equals("TIP_GOL"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "Project GOLD Price Tier",
                    Action = "Index",
                    Controller = "ProjectGoldPriceTier"
                };
            else if (key.Equals("TIP_MDS"))
                return new CurrentPageSession.LinkString
                {
                    LinkName = "MDS Price Tier",
                    Action = "Index",
                    Controller = "MDSPriceTier"
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