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
    
    public partial class DBSchema
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DBSchema()
        {
            this.Table = new HashSet<Table>();
        }
    
        public int ID { get; set; }
        public int DatabaseInformationID { get; set; }
        public string Name { get; set; }
    
        public virtual DatabaseInformation DatabaseInformation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table> Table { get; set; }
    }
}
