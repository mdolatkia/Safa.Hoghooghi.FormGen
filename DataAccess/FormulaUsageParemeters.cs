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
    
    public partial class FormulaUsageParemeters
    {
        public int ID { get; set; }
        public int FormulaUsageID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string RelationshipKeyColumnTail { get; set; }
        public int EditDataItemColumnDetailsID { get; set; }
    
        public virtual EditDataItemColumnDetails EditDataItemColumnDetails { get; set; }
    }
}
