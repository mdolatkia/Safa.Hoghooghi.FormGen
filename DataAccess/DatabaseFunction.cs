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
    
    public partial class DatabaseFunction
    {
        public DatabaseFunction()
        {
            this.BackendActionActivity = new HashSet<BackendActionActivity>();
            this.DatabaseFunctionParameter = new HashSet<DatabaseFunctionParameter>();
            this.DatabaseFunction_TableDrivedEntity = new HashSet<DatabaseFunction_TableDrivedEntity>();
            this.EntitySecurityCondition = new HashSet<EntitySecurityCondition>();
            this.FormulaItems = new HashSet<FormulaItems>();
        }
    
        public int ID { get; set; }
        public string FunctionName { get; set; }
        public string ReturnType { get; set; }
        public string Title { get; set; }
        public short Type { get; set; }
        public int DBSchemaID { get; set; }
        public bool Enable { get; set; }
    
        public virtual ICollection<BackendActionActivity> BackendActionActivity { get; set; }
        public virtual ICollection<DatabaseFunctionParameter> DatabaseFunctionParameter { get; set; }
        public virtual ICollection<DatabaseFunction_TableDrivedEntity> DatabaseFunction_TableDrivedEntity { get; set; }
        public virtual DBSchema DBSchema { get; set; }
        public virtual ICollection<EntitySecurityCondition> EntitySecurityCondition { get; set; }
        public virtual ICollection<FormulaItems> FormulaItems { get; set; }
    }
}
