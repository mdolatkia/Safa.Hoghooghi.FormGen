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
    
    public partial class DataLinkDefinition
    {
        public DataLinkDefinition()
        {
            this.DataLinkDefinition_EntityRelationshipTail = new HashSet<DataLinkDefinition_EntityRelationshipTail>();
            this.EntityDataLinkReport = new HashSet<EntityDataLinkReport>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int FirstSideEntityID { get; set; }
        public int SecondSideEntityID { get; set; }
    
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity1 { get; set; }
        public virtual ICollection<DataLinkDefinition_EntityRelationshipTail> DataLinkDefinition_EntityRelationshipTail { get; set; }
        public virtual ICollection<EntityDataLinkReport> EntityDataLinkReport { get; set; }
    }
}
