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
    
    public partial class ArcRelationshipGroup_Relationship
    {
        public int RelationshipID { get; set; }
        public int ArcRelationshipGroupID { get; set; }
        public Nullable<bool> Dummy { get; set; }
    
        public virtual ArcRelationshipGroup ArcRelationshipGroup { get; set; }
        public virtual Relationship Relationship { get; set; }
    }
}