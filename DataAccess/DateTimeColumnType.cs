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
    
    public partial class DateTimeColumnType
    {
        public int ColumnID { get; set; }
        public bool ShowMiladiDateInUI { get; set; }
        public bool HideTimePicker { get; set; }
        public bool ShowAMPMFormat { get; set; }
        public Nullable<bool> StringDateIsMiladi { get; set; }
        public Nullable<bool> StringTimeIsMiladi { get; set; }
        public Nullable<bool> StringTimeISAMPMFormat { get; set; }
    
        public virtual Column Column { get; set; }
    }
}
