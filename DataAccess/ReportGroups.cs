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
    
    public partial class ReportGroups
    {
        public int ID { get; set; }
        public int EntityListReportID { get; set; }
        public int EntityListViewColumnsID { get; set; }
    
        public virtual EntityListReport EntityListReport { get; set; }
        public virtual EntityListViewColumns EntityListViewColumns { get; set; }
    }
}
