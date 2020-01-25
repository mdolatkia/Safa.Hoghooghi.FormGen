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
    
    public partial class Column
    {
        public Column()
        {
            this.CodeFunction_TableDrivedEntity_Parameters = new HashSet<CodeFunction_TableDrivedEntity_Parameters>();
            this.RelationshipSearchFilter = new HashSet<RelationshipSearchFilter>();
            this.RelationshipSearchFilter1 = new HashSet<RelationshipSearchFilter>();
            this.TableDrivedEntity = new HashSet<TableDrivedEntity>();
            this.ColumnPhrase = new HashSet<ColumnPhrase>();
            this.UIColumnValue = new HashSet<UIColumnValue>();
            this.ConditionalPermission = new HashSet<ConditionalPermission>();
            this.DatabaseFunction_TableDrivedEntity_Columns = new HashSet<DatabaseFunction_TableDrivedEntity_Columns>();
            this.EntityListViewColumns = new HashSet<EntityListViewColumns>();
            this.EntitySearchColumns = new HashSet<EntitySearchColumns>();
            this.EntitySecurityCondition = new HashSet<EntitySecurityCondition>();
            this.FormulaItems = new HashSet<FormulaItems>();
            this.TableDrivedEntity_Columns = new HashSet<TableDrivedEntity_Columns>();
            this.TableDrivedEntityState = new HashSet<TableDrivedEntityState>();
            this.UIEnablityDetails = new HashSet<UIEnablityDetails>();
            this.RelationshipColumns = new HashSet<RelationshipColumns>();
            this.RelationshipColumns1 = new HashSet<RelationshipColumns>();
            this.UniqueConstraint = new HashSet<UniqueConstraint>();
        }
    
        public string Value { get; set; }
        public bool IsMandatory { get; set; }
        public Nullable<byte> TypeEnum { get; set; }
        public bool IsCustomColumn { get; set; }
        public int ID { get; set; }
        public int TableID { get; set; }
        public string Name { get; set; }
        public bool DataEntryEnabled { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsReadonly { get; set; }
        public string Alias { get; set; }
        public bool PrimaryKey { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNull { get; set; }
        public string DefaultValue { get; set; }
        public string DataType { get; set; }
        public Nullable<int> Position { get; set; }
        public string Description { get; set; }
        public string DBCalculateFormula { get; set; }
        public Nullable<int> CustomCalculateFormulaID { get; set; }
        public bool IsNotTransferable { get; set; }
    
        public virtual ICollection<CodeFunction_TableDrivedEntity_Parameters> CodeFunction_TableDrivedEntity_Parameters { get; set; }
        public virtual Formula Formula { get; set; }
        public virtual ColumnValueRange ColumnValueRange { get; set; }
        public virtual ICollection<RelationshipSearchFilter> RelationshipSearchFilter { get; set; }
        public virtual ICollection<RelationshipSearchFilter> RelationshipSearchFilter1 { get; set; }
        public virtual ICollection<TableDrivedEntity> TableDrivedEntity { get; set; }
        public virtual Table Table { get; set; }
        public virtual SecurityObject SecurityObject { get; set; }
        public virtual ICollection<ColumnPhrase> ColumnPhrase { get; set; }
        public virtual ICollection<UIColumnValue> UIColumnValue { get; set; }
        public virtual ICollection<ConditionalPermission> ConditionalPermission { get; set; }
        public virtual ICollection<DatabaseFunction_TableDrivedEntity_Columns> DatabaseFunction_TableDrivedEntity_Columns { get; set; }
        public virtual DateColumnType DateColumnType { get; set; }
        public virtual ICollection<EntityListViewColumns> EntityListViewColumns { get; set; }
        public virtual ICollection<EntitySearchColumns> EntitySearchColumns { get; set; }
        public virtual ICollection<EntitySecurityCondition> EntitySecurityCondition { get; set; }
        public virtual ICollection<FormulaItems> FormulaItems { get; set; }
        public virtual NumericColumnType NumericColumnType { get; set; }
        public virtual StringColumnType StringColumnType { get; set; }
        public virtual ICollection<TableDrivedEntity_Columns> TableDrivedEntity_Columns { get; set; }
        public virtual ICollection<TableDrivedEntityState> TableDrivedEntityState { get; set; }
        public virtual ICollection<UIEnablityDetails> UIEnablityDetails { get; set; }
        public virtual ICollection<RelationshipColumns> RelationshipColumns { get; set; }
        public virtual ICollection<RelationshipColumns> RelationshipColumns1 { get; set; }
        public virtual ICollection<UniqueConstraint> UniqueConstraint { get; set; }
    }
}