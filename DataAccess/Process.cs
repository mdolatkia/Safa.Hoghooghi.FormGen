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
    
    public partial class Process
    {
        public Process()
        {
            this.Action = new HashSet<Action>();
            this.Activity = new HashSet<Activity>();
            this.EntityGroup = new HashSet<EntityGroup>();
            this.State = new HashSet<State>();
            this.Transition = new HashSet<Transition>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string TransitionFlowSTR { get; set; }
        public Nullable<int> TableDrivedEntityID { get; set; }
    
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual ICollection<Action> Action { get; set; }
        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<EntityGroup> EntityGroup { get; set; }
        public virtual ICollection<State> State { get; set; }
        public virtual ICollection<Transition> Transition { get; set; }
    }
}
