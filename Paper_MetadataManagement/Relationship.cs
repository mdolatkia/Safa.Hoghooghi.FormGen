//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Paper_MetadataManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class Relationship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Relationship()
        {
            this.Relationship1 = new HashSet<Relationship>();
            this.RelationshipColumns = new HashSet<RelationshipColumns>();
        }
    
        public Nullable<int> RelationshipID { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public Nullable<byte> TypeEnum { get; set; }
        public int TableDrivedEntityID1 { get; set; }
        public int TableDrivedEntityID2 { get; set; }
        public Nullable<bool> Created { get; set; }
        public int ID { get; set; }
        public int FirstSideTableID { get; set; }
        public int SecondSideTableID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public Nullable<bool> DataEntryEnabled { get; set; }
        public Nullable<bool> SearchEnabled { get; set; }
        public Nullable<bool> ViewEnabled { get; set; }
        public Nullable<bool> IsFirstSideMandatory { get; set; }
        public Nullable<bool> IsSecondSideMandatory { get; set; }
        public Nullable<bool> IsFirstSideTransferable { get; set; }
        public Nullable<bool> IsSecondSideTransferable { get; set; }
        public Nullable<bool> IsFirstSideCreatable { get; set; }
        public Nullable<bool> IsSecondSideCreatable { get; set; }
        public Nullable<bool> IsFirstSideDirectlyCreatable { get; set; }
        public Nullable<bool> IsSecondSideDirectlyCreatable { get; set; }
    
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Relationship> Relationship1 { get; set; }
        public virtual Relationship Relationship2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelationshipColumns> RelationshipColumns { get; set; }
        public virtual RelationshipType RelationshipType { get; set; }
    }
}
