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
    
    public partial class CodeFunction
    {
        public CodeFunction()
        {
            this.BackendActionActivity = new HashSet<BackendActionActivity>();
            this.LetterSetting = new HashSet<LetterSetting>();
            this.CodeFunctionParameter = new HashSet<CodeFunctionParameter>();
            this.CodeFunction_TableDrivedEntity = new HashSet<CodeFunction_TableDrivedEntity>();
            this.EntityCommand = new HashSet<EntityCommand>();
            this.FormulaItems = new HashSet<FormulaItems>();
            this.LetterSetting1 = new HashSet<LetterSetting>();
            this.LetterSetting2 = new HashSet<LetterSetting>();
            this.LetterSetting3 = new HashSet<LetterSetting>();
            this.LetterSetting4 = new HashSet<LetterSetting>();
        }
    
        public int ID { get; set; }
        public string Path { get; set; }
        public string ClassName { get; set; }
        public string FunctionName { get; set; }
        public short Type { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<BackendActionActivity> BackendActionActivity { get; set; }
        public virtual ICollection<LetterSetting> LetterSetting { get; set; }
        public virtual ICollection<CodeFunctionParameter> CodeFunctionParameter { get; set; }
        public virtual ICollection<CodeFunction_TableDrivedEntity> CodeFunction_TableDrivedEntity { get; set; }
        public virtual ICollection<EntityCommand> EntityCommand { get; set; }
        public virtual ICollection<FormulaItems> FormulaItems { get; set; }
        public virtual ICollection<LetterSetting> LetterSetting1 { get; set; }
        public virtual ICollection<LetterSetting> LetterSetting2 { get; set; }
        public virtual ICollection<LetterSetting> LetterSetting3 { get; set; }
        public virtual ICollection<LetterSetting> LetterSetting4 { get; set; }
    }
}
