﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CFMMCDEntities : DbContext
    {
        public CFMMCDEntities()
            : base("name=CFMMCDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CSHMIMP0> CSHMIMP0 { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Audit_Log> Audit_Log { get; set; }
        public virtual DbSet<Breakfast_Price_Tier> Breakfast_Price_Tier { get; set; }
        public virtual DbSet<BUSINESS_EXT> BUSINESS_EXT { get; set; }
        public virtual DbSet<CSHMIMP0_NP6> CSHMIMP0_NP6 { get; set; }
        public virtual DbSet<Dessert_Price_Tier> Dessert_Price_Tier { get; set; }
        public virtual DbSet<INVRIMP0> INVRIMP0 { get; set; }
        public virtual DbSet<INVVEMP0> INVVEMP0 { get; set; }
        public virtual DbSet<LOCATION> LOCATIONs { get; set; }
        public virtual DbSet<McCafe_Bistro_Price_Tier> McCafe_Bistro_Price_Tier { get; set; }
        public virtual DbSet<McCafe_Level_2_Price_Tier> McCafe_Level_2_Price_Tier { get; set; }
        public virtual DbSet<McCafe_Level_3_Price_Tier> McCafe_Level_3_Price_Tier { get; set; }
        public virtual DbSet<MDS_Price_Tier> MDS_Price_Tier { get; set; }
        public virtual DbSet<OWNERSHIP> OWNERSHIPs { get; set; }
        public virtual DbSet<PROFIT_CEN> PROFIT_CEN { get; set; }
        public virtual DbSet<Project_Gold_Price_Tier> Project_Gold_Price_Tier { get; set; }
        public virtual DbSet<Regular_Price_Tier> Regular_Price_Tier { get; set; }
        public virtual DbSet<RIM_VEM_Lookup> RIM_VEM_Lookup { get; set; }
        public virtual DbSet<STORE_MIMMIC_Lookup> STORE_MIMMIC_Lookup { get; set; }
        public virtual DbSet<Store_Profile> Store_Profile { get; set; }
        public virtual DbSet<Tier_Lookup> Tier_Lookup { get; set; }
        public virtual DbSet<INVRIRP0> INVRIRP0 { get; set; }
    }
}
