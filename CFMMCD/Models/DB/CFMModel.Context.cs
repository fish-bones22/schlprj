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
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CSHMIMP0> CSHMIMP0 { get; set; }
        public virtual DbSet<CSHMIMP0_NP6> CSHMIMP0_NP6 { get; set; }
        public virtual DbSet<Store_Profile> Store_Profile { get; set; }
        public virtual DbSet<INVRIMP0> INVRIMP0 { get; set; }
        public virtual DbSet<Audit_Log> Audit_Log { get; set; }
    }
}
