﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyIdeaDataDBEntities : DbContext
    {
        public MyIdeaDataDBEntities()
            : base("name=MyIdeaDataDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ArchiveItem> ArchiveItem { get; set; }
        public virtual DbSet<ArchiveItem_Tag> ArchiveItem_Tag { get; set; }
        public virtual DbSet<DataLog> DataLog { get; set; }
        public virtual DbSet<EditDataItemColumnDetails> EditDataItemColumnDetails { get; set; }
        public virtual DbSet<EditDataItemExceptionLog> EditDataItemExceptionLog { get; set; }
        public virtual DbSet<ExternalReportKeys> ExternalReportKeys { get; set; }
        public virtual DbSet<FileRepository> FileRepository { get; set; }
        public virtual DbSet<FormulaUsageParemeters> FormulaUsageParemeters { get; set; }
        public virtual DbSet<Letter> Letter { get; set; }
        public virtual DbSet<LetterItemLog> LetterItemLog { get; set; }
        public virtual DbSet<MyDataItem> MyDataItem { get; set; }
        public virtual DbSet<MyDataItemKeyColumns> MyDataItemKeyColumns { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestAction> RequestAction { get; set; }
        public virtual DbSet<RequestNote> RequestNote { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
