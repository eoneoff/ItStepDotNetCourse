﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organizer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class organizerEntities : DbContext
    {
        public organizerEntities()
            : base("name=organizerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<ExpenditureName> ExpenditureName { get; set; }
        public virtual DbSet<ExpenditureType> ExpenditureType { get; set; }
        public virtual DbSet<IncomeSource> IncomeSource { get; set; }
    }
}