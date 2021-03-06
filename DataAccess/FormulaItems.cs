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
    
    public partial class FormulaItems
    {
        public int ID { get; set; }
        public Nullable<int> FormulaID { get; set; }
        public Nullable<int> ColumnID { get; set; }
        public Nullable<int> FormulaParameterID { get; set; }
        public Nullable<int> RelationshipID { get; set; }
        public Nullable<int> DatabaseFunctionID { get; set; }
        public Nullable<int> CodeFunctionID { get; set; }
        public Nullable<int> TableDrivedEntityStateID { get; set; }
        public string ItemTitle { get; set; }
        public string RelationshipIDTail { get; set; }
        public string RelationshipNameTail { get; set; }
    
        public virtual CodeFunction CodeFunction { get; set; }
        public virtual Column Column { get; set; }
        public virtual DatabaseFunction DatabaseFunction { get; set; }
        public virtual Formula Formula { get; set; }
        public virtual Formula Formula1 { get; set; }
        public virtual TableDrivedEntityState TableDrivedEntityState { get; set; }
        public virtual Relationship Relationship { get; set; }
    }
}
