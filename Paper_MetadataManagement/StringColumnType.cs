//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Paper_MetadataManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class StringColumnType
    {
        public int ColumnID { get; set; }
        public int MaxLength { get; set; }
        public string Format { get; set; }
    
        public virtual Column Column { get; set; }
    }
}
