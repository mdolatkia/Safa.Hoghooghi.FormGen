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
    
    public partial class LetterSetting
    {
        public int ID { get; set; }
        public Nullable<int> LetterExternalInfoCodeID { get; set; }
        public Nullable<int> BeforeLetterLoadCodeID { get; set; }
        public Nullable<int> BeforeLetterSaveCodeID { get; set; }
        public Nullable<int> AfterLetterSaveCodeID { get; set; }
        public Nullable<int> LetterConvertToExternalCodeID { get; set; }
    
        public virtual CodeFunction CodeFunction { get; set; }
        public virtual CodeFunction CodeFunction1 { get; set; }
        public virtual CodeFunction CodeFunction2 { get; set; }
        public virtual CodeFunction CodeFunction3 { get; set; }
        public virtual CodeFunction CodeFunction4 { get; set; }
    }
}