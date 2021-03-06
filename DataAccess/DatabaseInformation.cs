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
    
    public partial class DatabaseInformation
    {
        public DatabaseInformation()
        {
            this.DBSchema = new HashSet<DBSchema>();
        }
    
        public int ID { get; set; }
        public int DBServerID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public bool DBHasDate { get; set; }
        public string DBType { get; set; }
    
        public virtual DBServer DBServer { get; set; }
        public virtual SecurityObject SecurityObject { get; set; }
        public virtual DatabaseUISetting DatabaseUISetting { get; set; }
        public virtual ICollection<DBSchema> DBSchema { get; set; }
    }
}
