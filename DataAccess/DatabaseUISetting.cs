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
    
    public partial class DatabaseUISetting
    {
        public int DatabaseInformationID { get; set; }
        public int FlowDirectionLTR { get; set; }
    
        public virtual DatabaseInformation DatabaseInformation { get; set; }
    }
}