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
    
    public partial class Formula
    {
        public Formula()
        {
            this.Column = new HashSet<Column>();
            this.EntityValidation = new HashSet<EntityValidation>();
            this.ConditionalPermission = new HashSet<ConditionalPermission>();
            this.FormulaItems = new HashSet<FormulaItems>();
            this.FormulaItems1 = new HashSet<FormulaItems>();
            this.LetterTemplatePlainField = new HashSet<LetterTemplatePlainField>();
            this.State_Formula = new HashSet<State_Formula>();
            this.TableDrivedEntityState = new HashSet<TableDrivedEntityState>();
            this.TransitionAction_Formula = new HashSet<TransitionAction_Formula>();
        }
    
        public int ID { get; set; }
        public int TableDrivedEntityID { get; set; }
        public string Name { get; set; }
        public string Formula1 { get; set; }
        public short Version { get; set; }
        public string ResultType { get; set; }
        public string Title { get; set; }
    
        public virtual ICollection<Column> Column { get; set; }
        public virtual ICollection<EntityValidation> EntityValidation { get; set; }
        public virtual ICollection<ConditionalPermission> ConditionalPermission { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual ICollection<FormulaItems> FormulaItems { get; set; }
        public virtual ICollection<FormulaItems> FormulaItems1 { get; set; }
        public virtual ICollection<LetterTemplatePlainField> LetterTemplatePlainField { get; set; }
        public virtual ICollection<State_Formula> State_Formula { get; set; }
        public virtual ICollection<TableDrivedEntityState> TableDrivedEntityState { get; set; }
        public virtual ICollection<TransitionAction_Formula> TransitionAction_Formula { get; set; }
    }
}