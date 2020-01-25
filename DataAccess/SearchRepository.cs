//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class SearchRepository
    {
        public SearchRepository()
        {
            this.EntitySearchableReport = new HashSet<EntitySearchableReport>();
            this.Phrase = new HashSet<Phrase>();
        }
    
        public int ID { get; set; }
        public Nullable<int> SourceRelationID { get; set; }
        public Nullable<bool> HasNotRelationshipCheck { get; set; }
        public Nullable<int> RelationshipFromCount { get; set; }
        public Nullable<int> RelationshipToCount { get; set; }
        public Nullable<bool> IsSimpleSearch { get; set; }
        public Nullable<int> TableDrivedEntityID { get; set; }
        public Nullable<int> EntitySearchID { get; set; }
        public string Title { get; set; }
    
        public virtual EntitySearch EntitySearch { get; set; }
        public virtual ICollection<EntitySearchableReport> EntitySearchableReport { get; set; }
        public virtual LogicPhrase LogicPhrase { get; set; }
        public virtual ICollection<Phrase> Phrase { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
    }
}