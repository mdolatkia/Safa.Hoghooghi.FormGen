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
    
    public partial class RequestNote
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public string NoteTitle { get; set; }
        public string Note { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Request Request { get; set; }
    }
}
