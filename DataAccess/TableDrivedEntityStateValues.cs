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
    
    public partial class TableDrivedEntityStateValues
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public int TableDrivedEntityStateID { get; set; }
    
        public virtual TableDrivedEntityState TableDrivedEntityState { get; set; }
    }
}
