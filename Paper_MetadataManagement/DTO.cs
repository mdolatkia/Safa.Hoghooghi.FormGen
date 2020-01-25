using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public enum EntityColumnInfoType
    {
        WithoutColumn,
        WithSimpleColumns,
        WithFullColumns
    }
    public enum EntityRelationshipInfoType
    {

        WithRelationships,
        WithoutRelationships


    }
    public class DatabaseDTO
    {
        public int ID { set; get; }

        public string Name { set; get; }
        public string Title { set; get; }
        public int DBServerID { set; get; }
        public string DBServerName { set; get; }
        public string ConnectionString { set; get; }
    }
    public class TableDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public string Alias { set; get; }

        public bool IsInheritanceImplementation { set; get; }
    }
    public class TableDrivedEntityDTO
    {

        public TableDrivedEntityDTO()
        {
            Relationships = new List<RelationshipDTO>();
            OneToManyRelationships = new List<OneToManyRelationshipDTO>();
            ManyToOneRelationships = new List<ManyToOneRelationshipDTO>();
            ImplicitOneToOneRelationships = new List<ImplicitOneToOneRelationshipDTO>();
            ExplicitOneToOneRelationships = new List<ExplicitOneToOneRelationshipDTO>();
            SuperToSubRelationships = new List<SuperToSubRelationshipDTO>();
            SubToSuperRelationships = new List<SubToSuperRelationshipDTO>();
            SuperUnionToSubUnionRelationships = new List<SuperUnionToSubUnionRelationshipDTO>();
            SubUnionToSuperUnionRelationships = new List<SubUnionToSuperUnionRelationshipDTO>();
            Columns = new List<ColumnDTO>();
        }

        public int ID { set; get; }
        public int TableID { set; get; }

        public string TableName { set; get; }

        //public string Schema { set; get; }
        public int DatabaseID { set; get; }
        public int ServerID { set; get; }
        //public int SecurityObjectID { set; get; }
        public string DatabaseName { set; get; }
        public string Name { set; get; }
        public int RelatedSchemaID { set; get; }
        public string RelatedSchema { set; get; }
        public string Alias { set; get; }
        public bool IndependentDataEntry { set; get; }
        public string Criteria { get; set; }
        public List<ColumnDTO> Columns { set; get; }
        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
        //public List<int> SuperTypeEntityIDs { set; get; }
        public string UnionTypeEntities { set; get; }
        public string SubUnionTypeEntities { set; get; }
        //public List<int> UnionTypeEntityIDs { set; get; }
        public bool? BatchDataEntry { set; get; }
        public bool? IsDataReference { set; get; }
        public bool? IsStructurReferencee { set; get; }
        public bool? IsAssociative { set; get; }

        public List<RelationshipDTO> Relationships
        {
            //get
            //{
            //    List<RelationshipDTO> relationships = new List<RelationshipDTO>();
            //    OneToManyRelationships.ForEach(x => relationships.Add(x));
            //    ManyToOneRelationships.ForEach(x => relationships.Add(x));
            //    ImplicitOneToOneRelationships.ForEach(x => relationships.Add(x));
            //    ExplicitOneToOneRelationships.ForEach(x => relationships.Add(x));
            //    SuperToSubRelationships.ForEach(x => relationships.Add(x));
            //    SubToSuperRelationships.ForEach(x => relationships.Add(x));
            //    SuperUnionToSubUnionRelationships.ForEach(x => relationships.Add(x));
            //    SubUnionToSuperUnionRelationships.ForEach(x => relationships.Add(x));
            //    return relationships;
            //}
            set; get;
        }
        public List<OneToManyRelationshipDTO> OneToManyRelationships { set; get; }
        public List<ManyToOneRelationshipDTO> ManyToOneRelationships { set; get; }
        public List<ImplicitOneToOneRelationshipDTO> ImplicitOneToOneRelationships { set; get; }
        public List<ExplicitOneToOneRelationshipDTO> ExplicitOneToOneRelationships { set; get; }
        public List<SuperToSubRelationshipDTO> SuperToSubRelationships { set; get; }
        public List<SubToSuperRelationshipDTO> SubToSuperRelationships { set; get; }
        public List<SuperUnionToSubUnionRelationshipDTO> SuperUnionToSubUnionRelationships { set; get; }
        public List<SubUnionToSuperUnionRelationshipDTO> SubUnionToSuperUnionRelationships { set; get; }
        public int EntityListViewID { get; set; }
    }

    public class RelationshipDTO
    {
        public RelationshipDTO()
        {
            RelationshipColumns = new List<RelationshipColumnDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public Enum_RelationshipType TypeEnum { set; get; }
        public Enum_MasterRelationshipType MastertTypeEnum { set; get; }

        public int EntityID1 { set; get; }
        public int EntityID2 { set; get; }
        public int TableID1 { set; get; }
        public int TableID2 { set; get; }

        public string Entity1 { set; get; }
        public string Entity2 { set; get; }

        public string TableName1 { set; get; }
        public string TableName2 { set; get; }

        public string TableSchema1 { set; get; }
        public string TableSchema2 { set; get; }
        public int DatabaseID1 { set; get; }
        public int DatabaseID2 { set; get; }
        public string DatabaseName1 { set; get; }
        public string DatabaseName2 { set; get; }
        public string ServerName1 { set; get; }
        public string ServerName2 { set; get; }
        public bool Enabled { get; set; }

        public List<RelationshipColumnDTO> RelationshipColumns { set; get; }
        // public List<ColumnDTO> SecondSideColumns { set; get; }


        public string PairRelationship { set; get; }
        public int PairRelationshipID { set; get; }
        public string Alias { set; get; }

        //public string RelationshipColumns { set; get; }
        public string TypeStr { set; get; }

        public bool? DataEntryEnabled { set; get; }
        public bool? SearchEnabled { set; get; }
        //public bool? ViewEnabled { set; get; }

        public bool Select { set; get; }
        public bool Created { set; get; }
        public bool? IsOtherSideTransferable { set; get; }
        public bool IsOtherSideMandatory { set; get; }
        public bool? IsOtherSideCreatable { set; get; }
        public bool? IsOtherSideDirectlyCreatable { set; get; }
        public string LinkedServer1 { get; set; }
        public string LinkedServer2 { get; set; }
    }

    public class RelationshipColumnDTO
    {
        public int? FirstSideColumnID { set; get; }
        public ColumnDTO FirstSideColumn { set; get; }
        public int? SecondSideColumnID { set; get; }
        public ColumnDTO SecondSideColumn { set; get; }
        public string PrimarySideFixedValue { set; get; }

        //public string FirstSideFixedValue { set; get; }
        //public string SecondSideFixedValue { set; get; }

    }


    public enum Enum_RelationshipType
    {
        None,
        ManyToOne,
        OneToMany,
        ExplicitOneToOne,
        ImplicitOneToOne,
        SubToSuper,
        SuperToSub,
        SubUnionToUnion_UnionHoldsKeys,
        UnionToSubUnion_UnionHoldsKeys,
        SubUnionToUnion_SubUnionHoldsKeys,
        UnionToSubUnion_SubUnionHoldsKeys
    }
    public enum Enum_MasterRelationshipType
    {
        FromPrimartyToForeign,
        FromForeignToPrimary
    }
    class RuleImposedInfoArg : EventArgs
    {
        public int TotalProgressCount;
        public int CurrentProgress;
        public string Title;
    }



    public class OneToManyRelationshipDTO : RelationshipDTO
    {
        public short? MasterDetailState { set; get; }
        public int? DetailsCount { set; get; }


    }

    public class ManyToOneRelationshipDTO : RelationshipDTO
    {

    }

    public class ExplicitOneToOneRelationshipDTO : RelationshipDTO
    {

    }
    public class ImplicitOneToOneRelationshipDTO : RelationshipDTO
    {

    }
    public class SuperToSubRelationshipDTO : RelationshipDTO
    {
        public ISARelationshipDTO ISARelationship { set; get; }
    }
    public class SubToSuperRelationshipDTO : RelationshipDTO
    {
        public ISARelationshipDTO ISARelationship { set; get; }
    }

    public class ManyToManyRelationshipDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }
    public class ISARelationshipDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public bool IsSpecialization { set; get; }
        public bool IsGeneralization { set; get; }
        public bool IsTolatParticipation { set; get; }
        public bool IsDisjoint { set; get; }

        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
    }


    public class UnionRelationshipDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
        public bool IsTolatParticipation { set; get; }
        public bool UnionHoldsKeys { set; get; }

    }
    public class SuperUnionToSubUnionRelationshipDTO : RelationshipDTO
    {
        public UnionRelationshipDTO UnionRelationship { set; get; }
        public bool UnionHoldsKeys { set; get; }
    }
    public class SubUnionToSuperUnionRelationshipDTO : RelationshipDTO
    {
        public UnionRelationshipDTO UnionRelationship { set; get; }
        public bool UnionHoldsKeys { set; get; }
    }


    public class ColumnDTO
    {
        public ColumnDTO()
        {
            NumericColumnType = new NumericColumnTypeDTO();
        }

        //public ColumnDTO column { set; get; }
        public int ID { set; get; }
        public string Name { set; get; }
        public string DataType { set; get; }
        public string Alias { set; get; }
        public int TableID { set; get; }

        public bool IsNull { get; set; }

        public bool PrimaryKey { get; set; }
        //public int SecurityObjectID { set; get; }
        public bool DataEntryEnabled { get; set; }
        public string DefaultValue { get; set; }
        public bool IsMandatory { get; set; }
        public int Position { get; set; }
        public bool SearchEnabled { get; set; }
        //public bool ViewEnabled { get; set; }
        public bool DataEntryView { get; set; }
        public Enum_ColumnType ColumnType { set; get; }

        public Type DotNetType { set; get; }


        public bool IsIdentity { get; set; }

        public NumericColumnTypeDTO NumericColumnType { set; get; }
        public StringColumnTypeDTO StringColumnType { set; get; }
        public DateColumnTypeDTO DateColumnType { set; get; }

        public ColumnKeyValueDTO ColumnKeyValue { set; get; }

        public string DBFormula { get; set; }
        public bool IsDBCalculatedColumn { get; set; }

        //public FormulaDTO CustomFormula { set; get; }
    }
    public class StringColumnTypeDTO
    {
        public int ColumnID { set; get; }
        public string ColumnName { set; get; }
        public string Format { set; get; }
        public int MaxLength { set; get; }
    }

    public class NumericColumnTypeDTO
    {
        public NumericColumnTypeDTO()
        {
            TTT = new DateColumnTypeDTO();
        }

        public int ColumnID { set; get; }
        public int Precision { set; get; }
        public int Scale { set; get; }
        public int MinValue { set; get; }
        public int MaxValue { set; get; }

        public DateColumnTypeDTO TTT { set; get; }
    }

    public class DateColumnTypeDTO
    {
        public int ColumnID { set; get; }
        public bool IsPersianDate { set; get; }

    }

    public class ColumnKeyValueDTO
    {
        public int ColumnID { set; get; }
        public bool ValueFromKeyOrValue { set; get; }

        public List<ColumnKeyValueRangeDTO> ColumnKeyValueRange { set; get; }

    }
    public class ColumnKeyValueRangeDTO
    {
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public string ColumnName { set; get; }
        public string KeyTitle { set; get; }
        public int? Value { set; get; }

    }
    public enum Enum_ColumnType
    {
        None,
        String,
        Numeric,
        Boolean,
        Date
    }
}
