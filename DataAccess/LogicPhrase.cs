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
    
    public partial class LogicPhrase
    {
        public LogicPhrase()
        {
            this.Phrase = new HashSet<Phrase>();
            this.Phrase1 = new HashSet<Phrase>();
        }
    
        public int ID { get; set; }
        public bool AndOrType { get; set; }
    
        public virtual SearchRepository SearchRepository { get; set; }
        public virtual ICollection<Phrase> Phrase { get; set; }
        public virtual ICollection<Phrase> Phrase1 { get; set; }
    }
}
