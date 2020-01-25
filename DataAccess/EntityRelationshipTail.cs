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
    
    public partial class EntityRelationshipTail
    {
        public EntityRelationshipTail()
        {
            this.EntityArchiveRelationshipTails = new HashSet<EntityArchiveRelationshipTails>();
            this.EntityLetterRelationshipTails = new HashSet<EntityLetterRelationshipTails>();
            this.DataLinkDefinition_EntityRelationshipTail = new HashSet<DataLinkDefinition_EntityRelationshipTail>();
            this.DataMenuDataViewRelationship = new HashSet<DataMenuDataViewRelationship>();
            this.DataMenuGridViewRelationship = new HashSet<DataMenuGridViewRelationship>();
            this.DataMenuReportRelationship = new HashSet<DataMenuReportRelationship>();
            this.EntityListReportSubs = new HashSet<EntityListReportSubs>();
            this.EntityListViewColumns = new HashSet<EntityListViewColumns>();
            this.EntityGroup_Relationship = new HashSet<EntityGroup_Relationship>();
            this.LetterTemplateRelationshipField = new HashSet<LetterTemplateRelationshipField>();
            this.RelationshipSearchFilter = new HashSet<RelationshipSearchFilter>();
            this.EntitySearchColumns = new HashSet<EntitySearchColumns>();
            this.EntitySecurityInDirect = new HashSet<EntitySecurityInDirect>();
            this.EntitySecurityCondition = new HashSet<EntitySecurityCondition>();
            this.RelationshipSearchFilter1 = new HashSet<RelationshipSearchFilter>();
        }
    
        public int ID { get; set; }
        public int TableDrivedEntityID { get; set; }
        public int TargetEntityID { get; set; }
        public string RelationshipPath { get; set; }
    
        public virtual ICollection<EntityArchiveRelationshipTails> EntityArchiveRelationshipTails { get; set; }
        public virtual ICollection<EntityLetterRelationshipTails> EntityLetterRelationshipTails { get; set; }
        public virtual ICollection<DataLinkDefinition_EntityRelationshipTail> DataLinkDefinition_EntityRelationshipTail { get; set; }
        public virtual ICollection<DataMenuDataViewRelationship> DataMenuDataViewRelationship { get; set; }
        public virtual ICollection<DataMenuGridViewRelationship> DataMenuGridViewRelationship { get; set; }
        public virtual ICollection<DataMenuReportRelationship> DataMenuReportRelationship { get; set; }
        public virtual ICollection<EntityListReportSubs> EntityListReportSubs { get; set; }
        public virtual ICollection<EntityListViewColumns> EntityListViewColumns { get; set; }
        public virtual ICollection<EntityGroup_Relationship> EntityGroup_Relationship { get; set; }
        public virtual ICollection<LetterTemplateRelationshipField> LetterTemplateRelationshipField { get; set; }
        public virtual ICollection<RelationshipSearchFilter> RelationshipSearchFilter { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity { get; set; }
        public virtual TableDrivedEntity TableDrivedEntity1 { get; set; }
        public virtual ICollection<EntitySearchColumns> EntitySearchColumns { get; set; }
        public virtual ICollection<EntitySecurityInDirect> EntitySecurityInDirect { get; set; }
        public virtual ICollection<EntitySecurityCondition> EntitySecurityCondition { get; set; }
        public virtual ICollection<RelationshipSearchFilter> RelationshipSearchFilter1 { get; set; }
    }
}
