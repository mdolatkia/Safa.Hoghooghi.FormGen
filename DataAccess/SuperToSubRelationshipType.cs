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
    
    public partial class SuperToSubRelationshipType
    {
        public int ID { get; set; }
        public int ISARelationshipID { get; set; }
        public Nullable<bool> Dummy { get; set; }
    
        public virtual ISARelationship ISARelationship { get; set; }
        public virtual RelationshipType RelationshipType { get; set; }
    }
}
