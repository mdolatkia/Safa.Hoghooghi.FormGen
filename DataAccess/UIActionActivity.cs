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
    
    public partial class UIActionActivity
    {
        public UIActionActivity()
        {
            this.EntityState_UIActionActivity = new HashSet<EntityState_UIActionActivity>();
            this.UIEnablityDetails = new HashSet<UIEnablityDetails>();
            this.UIColumnValue = new HashSet<UIColumnValue>();
            this.UIColumnValueRangeReset = new HashSet<UIColumnValueRangeReset>();
            this.UIColumnValueRange = new HashSet<UIColumnValueRange>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public short Type { get; set; }
        public int TableDrivedEntityID { get; set; }
    
        public virtual ICollection<EntityState_UIActionActivity> EntityState_UIActionActivity { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual ICollection<UIEnablityDetails> UIEnablityDetails { get; set; }
        public virtual ICollection<UIColumnValue> UIColumnValue { get; set; }
        public virtual ICollection<UIColumnValueRangeReset> UIColumnValueRangeReset { get; set; }
        public virtual ICollection<UIColumnValueRange> UIColumnValueRange { get; set; }
    }
}
