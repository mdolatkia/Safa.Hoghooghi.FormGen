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
    
    public partial class EntityListReportSubs
    {
        public EntityListReportSubs()
        {
            this.EntityListReportSubsColumns = new HashSet<EntityListReportSubsColumns>();
        }
    
        public int ID { get; set; }
        public int ParentEntityListReportID { get; set; }
        public int ChildEntityListReportID { get; set; }
        public Nullable<short> OrderID { get; set; }
        public string Title { get; set; }
        public Nullable<int> EntityRelationshipTailID { get; set; }
    
        public virtual EntityListReport EntityListReport { get; set; }
        public virtual EntityListReport EntityListReport1 { get; set; }
        public virtual EntityRelationshipTail EntityRelationshipTail { get; set; }
        public virtual ICollection<EntityListReportSubsColumns> EntityListReportSubsColumns { get; set; }
    }
}
