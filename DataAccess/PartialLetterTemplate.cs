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
    
    public partial class PartialLetterTemplate
    {
        public PartialLetterTemplate()
        {
            this.LetterTemplateRelationshipField = new HashSet<LetterTemplateRelationshipField>();
        }
    
        public int ID { get; set; }
    
        public virtual LetterTemplate LetterTemplate { get; set; }
        public virtual ICollection<LetterTemplateRelationshipField> LetterTemplateRelationshipField { get; set; }
    }
}
