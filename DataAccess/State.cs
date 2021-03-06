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
    
    public partial class State
    {
        public State()
        {
            this.State_Formula = new HashSet<State_Formula>();
            this.StateActivity = new HashSet<StateActivity>();
            this.Transition = new HashSet<Transition>();
            this.Transition1 = new HashSet<Transition>();
        }
    
        public int ID { get; set; }
        public int ProcessID { get; set; }
        public short StateTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual Process Process { get; set; }
        public virtual ICollection<State_Formula> State_Formula { get; set; }
        public virtual ICollection<StateActivity> StateActivity { get; set; }
        public virtual ICollection<Transition> Transition { get; set; }
        public virtual ICollection<Transition> Transition1 { get; set; }
    }
}
