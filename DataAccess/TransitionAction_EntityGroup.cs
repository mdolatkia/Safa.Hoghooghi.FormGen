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
    
    public partial class TransitionAction_EntityGroup
    {
        public Nullable<bool> Dummy { get; set; }
        public int TransitionActionID { get; set; }
        public int EntityGroupID { get; set; }
    
        public virtual EntityGroup EntityGroup { get; set; }
        public virtual TransitionAction TransitionAction { get; set; }
    }
}
