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
    
    public partial class EntityUIComposition
    {
        public EntityUIComposition()
        {
            this.EntityUIComposition1 = new HashSet<EntityUIComposition>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int TableDrivedEntityID { get; set; }
        public string ItemTitle { get; set; }
        public string ItemIdentity { get; set; }
        public string Category { get; set; }
        public int Position { get; set; }
    
        public virtual ColumnUISetting ColumnUISetting { get; set; }
        public virtual EmptySpaceUISetting EmptySpaceUISetting { get; set; }
        public virtual ICollection<EntityUIComposition> EntityUIComposition1 { get; set; }
        public virtual EntityUIComposition EntityUIComposition2 { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual EntityUISetting EntityUISetting { get; set; }
        public virtual GroupUISetting GroupUISetting { get; set; }
        public virtual RelationshipUISetting RelationshipUISetting { get; set; }
        public virtual TabGroupUISetting TabGroupUISetting { get; set; }
        public virtual TabPageUISetting TabPageUISetting { get; set; }
    }
}
