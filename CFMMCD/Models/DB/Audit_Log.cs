//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFMMCD.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Audit_Log
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Page { get; set; }
        public string Page_Action { get; set; }
        public string Name { get; set; }
    }
}
