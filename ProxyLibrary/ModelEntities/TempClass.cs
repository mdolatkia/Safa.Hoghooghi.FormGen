
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEntites
{
    //public class ArcRelationshipGroupDTO
    //{
    //    public int ID { set; get; }
    //    public int TableDrivedEntityID { set; get; }
    //    public string GroupName { set; get; }
    //    public List<ArcRelationshipGroup_RelationshipDTO> Relationships { set; get; }
    //}

    //public class ArcRelationshipGroup_RelationshipDTO
    //{
    //    public int RelationshipID { set; get; }
    //    public int ArcRelationshipGroupID { set; get; }
    //}







    public class ColumnDTO
    {
        public ColumnDTO()
        {
            // NumericColumnType = new ModelEntites.NumericColumnTypeDTO();
            DatabaseDescriptions = new List<ColumnDescriptionDTO>();
        }
        public List<ColumnDescriptionDTO> DatabaseDescriptions { get; set; }
        //public ColumnDTO column { set; get; }
        public int ID { set; get; }
        public string Name { set; get; }
        public string DataType { set; get; }
        public string Alias { set; get; }
        public int TableID { set; get; }
        public string Description { set; get; }
        public bool IsNull { get; set; }

        public bool PrimaryKey { get; set; }
        public bool ForeignKey { get; set; }
        //public int SecurityObjectID { set; get; }
        public bool DataEntryEnabled { get; set; }
        public string DefaultValue { get; set; }
        public bool IsMandatory { get; set; }
        public int Position { get; set; }
        public bool IsDisabled { get; set; }
        //public bool ViewEnabled { get; set; }
        public bool IsReadonly { get; set; }
        public Enum_ColumnType ColumnType { set; get; }

        public Type DotNetType { set; get; }
        public bool IsNotTransferable { get; set; }

        public bool IsIdentity { get; set; }

        public bool IsBoolean { get; set; }
        public NumericColumnTypeDTO NumericColumnType { set; get; }
        public StringColumnTypeDTO StringColumnType { set; get; }
        public DateColumnTypeDTO DateColumnType { set; get; }
        public int ColumnValueRangeID { set; get; }
        public ColumnValueRangeDTO ColumnValueRange { set; get; }

        public string DBFormula { get; set; }
        public bool IsDBCalculatedColumn
        {
            get
            {
                return !string.IsNullOrEmpty(DBFormula);
            }
        }

        public int CustomFormulaID { set; get; }
        public FormulaDTO CustomFormula { set; get; }

        public bool ColumnsAdded { get; set; }
        public bool IsDescriptive { get; set; }
    }
    public class ColumnDescriptionDTO
    {
        public string Key { set; get; }
        public string Value { set; get; }
    }
    public class StringColumnTypeDTO
    {
        public int ColumnID { set; get; }
        public string Format { set; get; }
        public int MaxLength { set; get; }
    }

    public class NumericColumnTypeDTO
    {
        public NumericColumnTypeDTO()
        {
            TTT = new ModelEntites.DateColumnTypeDTO();
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

    public class ColumnValueRangeDTO
    {
        public ColumnValueRangeDTO()
        {
            Details = new List<ModelEntites.ColumnValueRangeDetailsDTO>();
        }
        public int ID { set; get; }
        //public int ColumnID { set; get; }
        //public int EntityID { set; get; }
        public bool ValueFromTitleOrValue { set; get; }

        public List<ColumnValueRangeDetailsDTO> Details { set; get; }
        //public string Name { get; set; }
    }
    public class ColumnValueRangeDetailsDTO
    {
        public int ID { set; get; }
        public int ColumnValueRangeID { set; get; }
        public string KeyTitle { set; get; }
        public string Value { set; get; }

        public string Tag1 { set; get; }
        public string Tag2 { set; get; }
    }
    public enum Enum_ColumnType
    {
        None,
        String,
        Numeric,
        Boolean,
        Date
    }











    public class UIColumnValueDTO
    {
        public UIColumnValueDTO()
        {
            //     ValidValues = new List<ModelEntites.ColumnValueValidValuesDTO>();
        }

        public int ID { set; get; }
        public string ExactValue { set; get; }
        public int ColumnID { set; get; }
        // public int EntityRelationshipTailID { set; get; }
        // public EntityRelationshipTailDTO EntityRelationshipTail { set; get; }

        //public List<ColumnValueValidValuesDTO> ValidValues { set; get; }

    }
    public class ColumnValueValidValuesDTO
    {
        public string Value { set; get; }
    }


    //public class RelationshipEnablityDTO
    //{


    //    public int ID { set; get; }
    //    public int EntityRelationshipTailID { set; get; }
    //    public EntityRelationshipTailDTO EntityRelationshipTail { set; get; }
    //    public bool ?Enable { set; get; }
    //    public bool ?Readonly { set; get; }

    //}
    //public class EntityRelationshipTailDTO
    //{
    //    public int ID { set; get; }
    //    public int TableDrivedEntityID { set; get; }

    //    public int RelationshipID { set; get; }

    //    public EntityRelationshipTailDTO ChildTail { set; get; }
    //}
    //public class UIEnablityDTO
    //{
    //    public int ID { set; get; }
    //    //public int EntityRelationshipTailID { set; get; }
    //    //public EntityRelationshipTailDTO EntityRelationshipTail { set; get; }

    //    public List<UIEnablityDetailsDTO> Details { set; get; }
    //}

    public class UIEnablityDetailsDTO
    {

        public int UIEnablityID { set; get; }
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public int RelationshipID { set; get; }
        public bool? Hidden { set; get; }
        public bool? Readonly { set; get; }
        public int UICompositionID { set; get; }


    }
    //public class ColumnValue_RelationshipDTO
    //{
    //    public int RelationshipID { set; get; }
    //    public int TableDrivedEntityStateID { set; get; }
    //    public bool Enabled { set; get; }

    //}
    //public class ColumnValue_ColumnDTO
    //{
    //    public int ColumnID { set; get; }
    //    public int TableDrivedEntityStateID { set; get; }
    //    public string ValidValue { set; get; }
    //}





    public class DbServerDTO
    {
        public DbServerDTO()
        {
            LinkedServers = new List<ModelEntites.LinkedServerDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public string IPAddress { set; get; }
        public enum_DBType ServerType { set; get; }
        public List<LinkedServerDTO> LinkedServers { set; get; }
    }

    public class LinkedServerDTO
    {
        public int ID { set; get; }
        public int SourceDBServerID { set; get; }
        public string SourceDBServerName { set; get; }
        public string SourceDBServerTitle { set; get; }
        public int TargetDBServerID { set; get; }
        public string TargetDBServerName { set; get; }
        public string TargetDBServerTitle { set; get; }

        public string Name { set; get; }

    }
    public class DatabaseDTO
    {
        public int ID { set; get; }

        public string Name { set; get; }
        public string Title { set; get; }
        public int DBServerID { set; get; }
        public string DBServerName { set; get; }
        public string ConnectionString { set; get; }
        public enum_DBType ServerType { set; get; }

        public bool DBHasData { get; set; }
    }
    public enum enum_DBType
    {
        SQLServer,
        Oracle
    }
    public class SchemaDTO
    {
        public int SecurityObjectID { set; get; }
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
        public bool InternalTable { get; set; }
        //public int InternalTableColumnID { get; set; }
    }



    public class SubSystemDTO
    {
        public int ID { set; get; }

        public string Name { set; get; }
        public string Description { set; get; }

    }








    public class RelationshipDTO
    {
        public RelationshipDTO()
        {
            RelationshipColumns = new List<ModelEntites.RelationshipColumnDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public string Info { set; get; }
        public Enum_RelationshipType TypeEnum { set; get; }
        public Enum_MasterRelationshipType MastertTypeEnum { set; get; }
        public Enum_OrginalRelationshipType OrginalTypeEnum { set; get; }
        public string OrginalRelationshipGroup { set; get; }
        public int EntityID1 { set; get; }
        public int EntityID2 { set; get; }
        public int TableID1 { set; get; }
        public int TableID2 { set; get; }

        public string Entity1 { set; get; }
        public string Entity2 { set; get; }

        //public int ISAUnionRelationshipID { set; get; }

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
        public bool Removed { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsNotTransferable { get; set; }
        public List<RelationshipColumnDTO> RelationshipColumns { set; get; }
        // public List<ColumnDTO> SecondSideColumns { set; get; }


        public string PairRelationship { set; get; }
        public int PairRelationshipID { set; get; }
        public string Alias { set; get; }

        //public string RelationshipColumns { set; get; }
        public string TypeStr { set; get; }

        public bool DataEntryEnabled { set; get; }
        //public bool IsReadonly { set; get; }
        //public bool? ViewEnabled { set; get; }

        public bool Select { set; get; }
        public bool Created { set; get; }
        public bool IsOtherSideTransferable { set; get; }
        public bool IsOtherSideMandatory { set; get; }
        public bool IsOtherSideCreatable { set; get; }
        public bool IsOtherSideDirectlyCreatable { set; get; }
        //public bool IsPrimarySideTransferable { set; get; }
        public bool IsPrimarySideMandatory { set; get; }
        //    public bool IsPrimarySideCreatable { set; get; }
        //    public bool IsPrimarySideDirectlyCreatable { set; get; }

        //public bool IsForeignSideTransferable { set; get; }
        //public bool IsForeignSideMandatory { set; get; }
        //public bool IsForeignSideCreatable { set; get; }
        //public bool IsForeignSideDirectlyCreatable { set; get; }

        public string LinkedServer { get; set; }
        //  public string LinkedServer2 { get; set; }

        public bool SearchInitially { set; get; }
        public RelationshipDeleteOption DeleteOption { get; set; }

        public RelationInfo RelationInfo { set; get; }
        public bool AllForeignKeysArePrimaryKey { get; set; }
        public bool AnyForeignKeyIsPrimaryKey { get; set; }
        public bool FKCoumnIsNullable { get; set; }

        public CreateRelationshipType CreateType { set; get; }
        public int ServerID2 { get; set; }
        public int ServerID1 { get; set; }
        public bool FromDifferentServer { get; set; }
        public string Entity1Alias { get; set; }
        public string Entity2Alias { get; set; }
        public bool OtherSideIsView { get; set; }
    }
    public enum CreateRelationshipType
    {
        OneToMany,
        OneToOne
    }
    public enum RelationshipDeleteOption
    {
        //None,
        SetNull,
        DeleteCascade
    }
    public class RelationshipColumnDTO
    {
        public int FirstSideColumnID { set; get; }
        public ColumnDTO FirstSideColumn { set; get; }
        public int SecondSideColumnID { set; get; }
        public ColumnDTO SecondSideColumn { set; get; }
        //public string PrimarySideFixedValue { set; get; }

        //public string FirstSideFixedValue { set; get; }
        //public string SecondSideFixedValue { set; get; }

    }

    public enum Enum_OrginalRelationshipType
    {
        None,
        OneToMany,
        OneToOne,
        SuperToSub,
        SuperUnionToSubUnion
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
        TableToView,
        ViewToTable
        //ViewToView
        //SubUnionToUnion_SubUnionHoldsKeys,
        //UnionToSubUnion_SubUnionHoldsKeys
    }
    public enum Enum_MasterRelationshipType
    {
        FromPrimartyToForeign,
        FromForeignToPrimary,
        NotImportant
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












    public class TableDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public string Alias { set; get; }

        public bool IsInheritanceImplementation { set; get; }
    }


    //public class PlainNavigationDTO
    //{

    //    public string ObjectIdentity { set; get; }
    //    public DatabaseObjectCategory ObjectCategory { set; get; }
    //    public int SecurityObjectID { set; get; }
    //    public bool Permitted { set; get; }
    //}

    public class NavigationTreeDTO
    {
        public NavigationTreeDTO()
        {
            TreeItems = new List<ModelEntites.NavigationItemDTO>();
            //PlainItems = new List<ModelEntites.PlainNavigationDTO>();
        }
        public List<NavigationItemDTO> TreeItems { set; get; }
        //public List<PlainNavigationDTO> PlainItems { set; get; }

    }
    public class NavigationItemDTO
    {
        public int ID { set; get; }

        public int? ParentID { set; get; }
        public NavigationItemDTO ParentItem { set; get; }
        public string Title { set; get; }
        public int ObjectIdentity { set; get; }
        public DatabaseObjectCategory ObjectCategory { set; get; }
        public int TableDrivedEntityID { set; get; }
        public bool? Permitted { set; get; }
        public string Tooltip { get; set; }
        public string Name { get; set; }
        //public List<string> AllowedActions { set; get; }
        //public List<NavigationItemDTO> ChildItems { set; get; }
        public I_NavigationMenu Menu { set; get; }
        //public int EntityReportID { get; set; }
    }
    public interface I_NavigationMenu
    {
        object Node { set; get; }
        event EventHandler Clicked;
    }
    public class EntityUICompositionCompositeDTO
    {
        public EntityUICompositionDTO RootItem { set; get; }
        public List<ColumnUISettingDTO> ColumnItems { set; get; }
        public List<RelationshipUISettingDTO> RelationshipItems { set; get; }
    }
    public class EntityUICompositionDTO
    {
        public List<ColumnUISettingDTO> RootColumnItems { set; get; }
        public List<RelationshipUISettingDTO> RootRelationshipItems { set; get; }

        public EntityUICompositionDTO()
        {
            ChildItems = new List<EntityUICompositionDTO>();
        }
        public int ID { set; get; }

        public int? ParentID { set; get; }
        public EntityUICompositionDTO ParentItem { set; get; }
        public string Title { set; get; }
        public string ObjectIdentity { set; get; }
        public DatabaseObjectCategory ObjectCategory { set; get; }
        public List<EntityUICompositionDTO> ChildItems { set; get; }
        public int Position { set; get; }

        public EntityUISettingDTO EntityUISetting { set; get; }
        public ColumnUISettingDTO ColumnUISetting { set; get; }
        public RelationshipUISettingDTO RelationshipUISetting { set; get; }

        public EmptySpaceUISettingDTO EmptySpaceUISetting { set; get; }
        public GroupUISettingDTO GroupUISetting { set; get; }
        public TabGroupUISettingDTO TabGroupUISetting { set; get; }
        public TabPageUISettingDTO TabPageUISetting { set; get; }

    }

    public class EditBaseEntityDTO
    {
        public EditBaseEntityDTO()
        {
            DrivedEntities = new List<TableDrivedEntityDTO>();
        }
        public TableDrivedEntityDTO BaseEntity { set; get; }
        public List<TableDrivedEntityDTO> DrivedEntities { set; get; }
        public ISARelationshipDTO ISARelationship { set; get; }
    }
    //public class EditDrivedEntityDTO
    //{
    //    public EditDrivedEntityDTO()
    //    {
    //    }
    //    public TableDrivedEntityDTO DrivedEntity { set; get; }

    // //   public string DeterminerValue { set; get; }
    //}
    //public class EditEntityDeterminerDTO
    //{
    //    public string Value { set; get; }
    //}
    public class RelationshipImportItem
    {
        public RelationshipImportItem(RelationshipDTO relationship, bool exception, string tooltip)
        {
            Relationship = relationship;
            Exception = exception;
            Tooltip = tooltip;
        }
        public RelationshipImportItem(RelationshipDTO relationship, string tooltip)
        {
            Relationship = relationship;
            // Desc = desc;
            Tooltip = tooltip;
        }

        public RelationshipDTO Relationship { set; get; }
        public bool Selected { set; get; }
        public bool Exception { set; get; }
        public string Tooltip { set; get; }

        public bool IsValid { get; set; }
        public string ValidationTooltip { get; set; }
    }

    public class EntityRelationInfo
    {
        public EntityRelationInfo()
        {
            RelationInfos = new List<RelationInfo>();
        }
        public TableDrivedEntityDTO TableDrivedEntity { set; get; }

        public List<RelationInfo> RelationInfos { set; get; }
    }

    public class RelationInfo
    {
        public RelationInfo()
        {
            //Relationship = relationship;
            //FKRelatesOnPrimaryKey = fKRelatesOnPrimaryKey;
            //FKHasData = fKHasData;
            //AllPrimarySideHasFkSideData = allPrimarySideHasFkSideData;
            //FKColumnIsMandatory = fKColumnIsMandatory;
            //RelationType = relationType;
            //FKRelatesOnPartOfPrimaryKey = fKRelatesOnPartOfPrimaryKey;

        }
        //public RelationshipDTO Relationship { set; get; }
        public bool? AllPrimarySideHasFkSideData { set; get; }
        public bool? AllFKSidesHavePKSide { set; get; }
        //public bool FKCoumnIsNullable { set; get; }
        //public bool FKRelatesOnPrimaryKey { set; get; }

        //public bool FKRelatesOnPartOfPrimaryKey { set; get; }

        public bool? FKHasData { set; get; }

        //public RelationType RelationType { set; get; }
        //public bool IsPrecise { get; set; }
        public bool? MoreThanOneFkForEachPK { get; set; }
    }
    public enum RelationType
    {
        Unknown,
        OnePKtoManyFK,
        OnePKtoOneFK
        //    ,
        //ManyFKtoOnePK,
        //OneFKtoOnePK
    }
    public class FunctionImportItem
    {
        public FunctionImportItem(DatabaseFunctionDTO function, bool exception, string tooltip)
        {
            Function = function;
            Exception = exception;
            Tooltip = tooltip;
        }
        public FunctionImportItem(DatabaseFunctionDTO function, string tooltip, bool selected)
        {
            Function = function;
            Tooltip = tooltip;
            Selected = selected;
        }
        public bool Exception { set; get; }
        public DatabaseFunctionDTO Function { set; get; }
        public bool Selected { set; get; }
        public string Tooltip { set; get; }
    }
    public class ItemImportingStartedArg : EventArgs
    {
        public string ItemName;
        public int TotalProgressCount;
        public int CurrentProgress;
    }
    public class TableImportItem : INotifyPropertyChanged
    {
        public TableImportItem(TableDrivedEntityDTO entity, bool exception, string tooltip)
        {
            Entity = entity;
            Exception = exception;
            Tooltip = tooltip;
            Relationships = new List<ModelEntites.RelationshipDTO>();


        }
        public TableImportItem(TableDrivedEntityDTO entity, string tooltip, bool selected)
        {
            Entity = entity;
            // Desc = desc;
            Tooltip = tooltip;
            Selected = selected;
            Relationships = new List<ModelEntites.RelationshipDTO>();

        }

        public TableDrivedEntityDTO Entity { set; get; }
        string _RelatedEntityNames;

        public event PropertyChangedEventHandler PropertyChanged;

        public string RelatedEntityNames
        {
            get
            {
                return _RelatedEntityNames;
            }
            set
            {
                _RelatedEntityNames = value;
                OnPropertyChanged("RelatedEntityNames");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public List<RelationshipDTO> Relationships { set; get; }

        //public bool Rel1 { set; get; }
        //public bool Rel2 { set; get; }
        //public bool Rel3 { set; get; }
        public bool Selected { set; get; }
        public bool Exception { set; get; }
        public string Tooltip { set; get; }
        public string ValidationTooltip { get; set; }
        public bool IsValid { get; set; }

        public List<EntityUICompositionDTO> UIComposition { get; set; }

        public EntityListViewDTO DefaultListView { get; set; }

    }
    public class TableDrivedEntityDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TableDrivedEntityDTO()
        {
            Relationships = new List<RelationshipDTO>();
            DatabaseDescriptions = new List<Tuple<string, string>>();
            EntityDeterminers = new List<ModelEntites.EntityDeterminerDTO>();
            //OneToManyRelationships = new List<OneToManyRelationshipDTO>();
            //ManyToOneRelationships = new List<ManyToOneRelationshipDTO>();
            //ImplicitOneToOneRelationships = new List<ImplicitOneToOneRelationshipDTO>();
            //ExplicitOneToOneRelationships = new List<ExplicitOneToOneRelationshipDTO>();
            //SuperToSubRelationships = new List<SuperToSubRelationshipDTO>();
            //SubToSuperRelationships = new List<SubToSuperRelationshipDTO>();
            //SuperUnionToSubUnionRelationships = new List<SuperUnionToSubUnionRelationshipDTO>();
            //SubUnionToSuperUnionRelationships = new List<SubUnionToSuperUnionRelationshipDTO>();
            Columns = new List<ModelEntites.ColumnDTO>();
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public List<Tuple<string, string>> DatabaseDescriptions { get; set; }
        public int ID { set; get; }
        public int TableID { set; get; }
        public string TableName { set; get; }
        public string Description { set; get; }
        public bool IsView { set; get; }
        // public bool SelectAsComboBox { set; get; }

        //public string Schema { set; get; }
        public int DatabaseID { set; get; }
        public int ServerID { set; get; }
        //public int SecurityObjectID { set; get; }
        public string DatabaseName { set; get; }
        public string Name { set; get; }
        public int RelatedSchemaID { set; get; }
        public string RelatedSchema { set; get; }
        public string Alias { set; get; }
        public bool? IndependentDataEntry { set; get; }
        //public string Criteria { get; set; }
        public List<ColumnDTO> Columns { set; get; }
        public string SuperTypeEntities { set; get; }
        public string SubTypeEntities { set; get; }
        //public List<int> SuperTypeEntityIDs { set; get; }
        public string UnionTypeEntities { set; get; }
        public string SubUnionTypeEntities { set; get; }
        //public List<int> UnionTypeEntityIDs { set; get; }
        public bool BatchDataEntry { set; get; }

        bool? _IsDataReference;
        public bool? IsDataReference
        {
            set
            {
                _IsDataReference = value;
                OnPropertyChanged("IsDataReference");
            }
            get
            {
                return _IsDataReference;
            }
        }
        public bool DataIsLimited { set; get; }
        public bool? DataCountIsFew { set; get; }

        //bool _DataIsLimited;
        //public bool DataIsLimited
        //{
        //    set
        //    {
        //        _DataIsLimited = value;
        //        OnPropertyChanged("DataIsLimited");
        //    }
        //    get
        //    {
        //        return _IsDataReference;
        //    }
        //}

        public string Color { set; get; }
        public bool IsDisabled { set; get; }
        public bool IsReadonly { set; get; }

        public bool IsStructurReferencee { set; get; }
        public bool IsAssociative { set; get; }
        public bool SearchInitially { set; get; }

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
        //public List<OneToManyRelationshipDTO> OneToManyRelationships { set; get; }
        //public List<ManyToOneRelationshipDTO> ManyToOneRelationships { set; get; }
        //public List<ImplicitOneToOneRelationshipDTO> ImplicitOneToOneRelationships { set; get; }
        //public List<ExplicitOneToOneRelationshipDTO> ExplicitOneToOneRelationships { set; get; }
        //public List<SuperToSubRelationshipDTO> SuperToSubRelationships { set; get; }
        //public List<SubToSuperRelationshipDTO> SubToSuperRelationships { set; get; }
        //public List<SuperUnionToSubUnionRelationshipDTO> SuperUnionToSubUnionRelationships { set; get; }
        //public List<SubUnionToSuperUnionRelationshipDTO> SubUnionToSuperUnionRelationships { set; get; }
        public int EntityListViewID { get; set; }
        public int EntitySearchID { get; set; }
        public bool LoadArchiveRelatedItems { get; set; }
        public bool LoadLetterRelatedItems { get; set; }

        public int DeterminerColumnID { get; set; }
        public ColumnDTO DeterminerColumn { get; set; }
        //public string DeterminerColumnValue { get; set; }
        public List<EntityDeterminerDTO> EntityDeterminers { set; get; }
        public bool Reviewed { get; set; }
        public bool ColumnsReviewed { get; set; }
        //public bool ColumnsAdded { get; set; }
    }
    public class EntityDeterminerDTO
    {
        public int ID { set; get; }
        //public int ColumnID { get; set; }
        public string Value { get; set; }
    }
    public class EntityUIPositionDTO
    {
        public int ID { set; get; }

        public int? ParentID { set; get; }
        public EntityUIPositionDTO ParentItem { set; get; }
        public int Position { set; get; }
        public string Title { set; get; }
        public string ObjectIdentity { set; get; }
        public string ObjectCategory { set; get; }
        public List<EntityUIPositionDTO> ChildItems { set; get; }

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
    }
    public class SubUnionToSuperUnionRelationshipDTO : RelationshipDTO
    {
        public UnionRelationshipDTO UnionRelationship { set; get; }
    }





    public class UniqueConstraintsDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int TableID { set; get; }
        //public string Alias { set; get; }


    }
    //public class ViewImportItem
    //{


    //    public ViewImportItem(ViewDTO view, bool exception, string tooltip)
    //    {
    //        View = view;
    //        Exception = exception;
    //        Tooltip = tooltip;
    //    }
    //    public ViewImportItem(ViewDTO view, string tooltip, bool selected)
    //    {
    //        View = view;
    //        Selected = selected;
    //        Tooltip = tooltip;
    //    }



    //    public ViewDTO View { set; get; }
    //    public bool Selected { set; get; }
    //    public bool Exception { set; get; }
    //    public string Tooltip { set; get; }
    //}
    //public class ViewDTO
    //{
    //    public ViewDTO()
    //    {
    //        Columns = new List<ModelEntites.ViewColumnsDTO>();
    //    }
    //    public int ID { set; get; }
    //    public string Name { set; get; }
    //    public bool Enable { set; get; }

    //    public int DatabaseID { set; get; }
    //    public string RelatedSchema { set; get; }

    //    public List<ViewColumnsDTO> Columns { set; get; }
    //}
    //public class ViewColumnsDTO
    //{
    //    public int ID { set; get; }
    //    public int ColumnID { set; get; }

    //    public string TableName { set; get; }
    //    public string Name { get; set; }
    //    //  public string ColumnName { set; get; }
    //    //     public string Name { set; get; }
    //}

    public class EntityValidationDTO
    {
        public EntityValidationDTO()
        {

        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public int FormulaID { set; get; }
        //public FormulaDTO Formula { set; get; }
        //public int CodeFunctionID { set; get; }
        //public CodeFunctionDTO CodeFunction { set; get; }
        //public string Value { set; get; }
        public string Message { set; get; }
        public string Title { set; get; }
        //   public bool ResultSensetive { set; get; }
    }

    public class EntityReportDTO
    {
        public int ID { set; get; }
        //public int EntitySearchID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public ReportType ReportType { set; get; }
        public SearchableReportType SearchableReportType { set; get; }
        //public EntityListReportDTO EntityListReport { set; get; }
        //public EntityListReportGroupedDTO EntityListReportGrouped { set; get; }
        //public EntityChartReportDTO EntityChartReport { set; get; }

        //public EntityExternalReportDTO EntityExternalReport { set; get; }
        //public EntityCrosstabReportDTO CrosstabReportDTO { set; get; }

        //public EntityPreDefinedSearchDTO EntityPreDefinedSearch { set; get; }
        public string ReportTitle { get; set; }
    }
    public class EntitySearchableReportDTO : EntityReportDTO
    {
        public DP_SearchRepository SearchRepository { set; get; }
        public int SearchRepositoryID { get; set; }
    }
    public enum SearchableReportType
    {
        None,
        ListReport,
        ChartReport,
        ExternalReport,
        CrosstabReport,
        DataView,
        GridView

    }
    public enum ReportType
    {
        SearchableReport,
        DirectReport
    }
    public class EntityListReportDTO : EntitySearchableReportDTO
    {
        public EntityListReportDTO()
        {
            EntityListReportSubs = new List<ModelEntites.EntityListReportSubsDTO>();
            ReportGroups = new List<ModelEntites.ReportGroupDTO>();
        }


        public int EntityListViewID { set; get; }
        public EntityListViewDTO EntityListView { set; get; }
        public List<EntityListReportSubsDTO> EntityListReportSubs { set; get; }
        public List<ReportGroupDTO> ReportGroups { set; get; }
    }
    public class EntityDataViewReportDTO : EntitySearchableReportDTO
    {
        public EntityDataViewReportDTO()
        {
        }

        public int DefaultEntityListViewID { set; get; }

    }
    public class EntityDataLinkReportDTO : EntityReportDTO
    {
        public EntityDataLinkReportDTO()
        {
        }

        public int DataLinkID { set; get; }

    }
    public class EntityGridViewReportDTO : EntitySearchableReportDTO
    {
        public EntityGridViewReportDTO()
        {
        }

        public int DefaultEntityListViewID { set; get; }

    }
    //public class EntityListReportGroupedDTO : EntityReportDTO
    //{
    //    public EntityListReportGroupedDTO()
    //    {
    //        ReportGroups = new List<ModelEntites.ReportGroupDTO>();
    //    }

    //    public int EntityListReportID { set; get; }
    //    public EntityListReportDTO EntityListReport { set; get; }
    //    //public List<EntityListReportSubsDTO> EntityListReportSubs { set; get; }

    //    public List<ReportGroupDTO> ReportGroups { set; get; }
    //}
    public class ReportGroupDTO
    {
        public int ID { set; get; }
        public int ListViewColumnID { set; get; }

        public string ColumnName { set; get; }
        public EntityListViewColumnsDTO EntityListViewColumn { set; get; }
    }
    public class EntityExternalReportDTO : EntitySearchableReportDTO
    {
        public EntityExternalReportDTO()
        {

        }

        public string URL { set; get; }
    }
    public class SecurityAdjustable
    {
        public bool SercurityImposed { set; get; }
    }
    public class EntityListViewDTO : SecurityAdjustable
    {
        public EntityListViewDTO()
        {
            //EntityListViewRelationshipTails = new List<ModelEntites.EntityListViewRelationshipTailDTO>();
            EntityListViewAllColumns = new List<EntityListViewColumnsDTO>();
        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public string Title { set; get; }
        //public List<EntityListViewRelationshipTailDTO> EntityListViewRelationshipTails { set; get; }
        public List<EntityListViewColumnsDTO> EntityListViewAllColumns { set; get; }

    }
    public class EntityListViewColumnsDTO
    {
        public EntityListViewColumnsDTO()
        {

        }
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public ColumnDTO Column { set; get; }
        public short OrderID { set; get; }
        public short WidthUnit { set; get; }
        public string Alias { set; get; }
        public List<ColumnDTO> vwValueColumns { set; get; }
        //  public string RelationshipPath { set; get; }
        public string EntityPath { set; get; }
        //public int EntityListViewRelationshipTailsID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }

        //string _RelativeColumnName;
        public string RelativeColumnName
        {
            get
            {
                return Column.Name + RelationshipTailID.ToString();
            }
        }

        public string CreateRelationshipTailPath { get; set; }
        public bool IsDescriptive { get; set; }
        public bool SubToSuperRelationship { get; set; }

        //public int CreateRelationshipID { get; set; }
        //public int CreateRelationshipTailTargetEntityID { get; set; }
    }

    public class EntitySearchDTO
    {
        public EntitySearchDTO()
        {
            //EntitySearchRelationshipTails = new List<ModelEntites.EntitySearchRelationshipTailDTO>();
            EntitySearchAllColumns = new List<EntitySearchColumnsDTO>();
        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public string Title { set; get; }
        //public List<EntitySearchRelationshipTailDTO> EntitySearchRelationshipTails { set; get; }
        public List<EntitySearchColumnsDTO> EntitySearchAllColumns { set; get; }
    }
    public class EntitySearchColumnsDTO
    {
        public EntitySearchColumnsDTO()
        {

        }
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public List<ColumnDTO> vwValueColumns { set; get; }
        public ColumnDTO Column { set; get; }
        public short OrderID { set; get; }
        public short WidthUnit { set; get; }
        public string Alias { set; get; }
        public string RelationshipPath { set; get; }
        //public int EntitySearchRelationshipTailsID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public int RelationshipTailID { set; get; }

        public bool RelationshipTailSelectable { set; get; }
        public string CreateRelationshipTailPath { get; set; }
        //public int CreateRelationshipTargetEntityID { get; set; }
    }
    //public class EntityListViewRelationshipTailDTO
    //{
    //    public EntityListViewRelationshipTailDTO()
    //    {
    //        EntityListViewColumns = new List<ModelEntites.EntityListViewColumnsDTO>();
    //    }
    //    public int ID { set; get; }
    //    public EntityRelationshipTailDTO EntityRelationshipTail { set; get; }
    //    public int EntityRelationshipTailID { set; get; }
    //    public List<EntityListViewColumnsDTO> EntityListViewColumns { set; get; }
    //}



    public class EntityChartReportDTO : EntitySearchableReportDTO
    {
        public EntityChartReportDTO()
        {
            EntityChartReportSeries = new List<ModelEntites.EntityChartReportSerieDTO>();
            EntityChartReportCategories = new List<ModelEntites.EntityChartReportCategoryDTO>();
            EntityChartReportValues = new List<ModelEntites.EntityChartReportValueDTO>();
        }

        public ChartType ChartType { set; get; }

        public List<EntityChartReportSerieDTO> EntityChartReportSeries { set; get; }
        public List<EntityChartReportCategoryDTO> EntityChartReportCategories { set; get; }
        public List<EntityChartReportValueDTO> EntityChartReportValues { set; get; }
        public int EntityListViewID { get; set; }
        public EntityListViewDTO EntityListView { get; set; }
    }



    public enum ChartType
    {
        Column,
        Pie,
        Line,
        Radar
    }
    public class EntityChartReportSerieDTO
    {
        public EntityChartReportSerieDTO()
        {

        }
        public int ID { set; get; }
        public int EntityListViewColumnID { set; get; }

        public EntityListViewColumnsDTO EntityListViewColumn { set; get; }
        //public int RelationshipTailID { set; get; }
        //public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //public string ColumnName { set; get; }
        //public int ColumnID { set; get; }
        //    public string RelationshipPath { get; set; }

        public ChartSerieArrangeType ArrangeType { set; get; }
        //public List<ColumnDTO> vwValueColumns { get; set; }
    }
    public enum ChartSerieArrangeType
    {
        Clustered,
        Overlapped,
        Stacked,
        Stacked100

    }
    public class EntityChartReportCategoryDTO
    {
        public EntityChartReportCategoryDTO()
        {

        }
        public int ID { set; get; }
        public int EntityListViewColumnID { set; get; }

        public EntityListViewColumnsDTO EntityListViewColumn { set; get; }
        //public int RelationshipTailID { set; get; }
        //public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //public string ColumnName { set; get; }
        // public string RelationshipPath { get; set; }
        //public int ColumnID { set; get; }
        //public List<ColumnDTO> vwValueColumns { get; set; }
    }
    public class EntityChartReportValueDTO
    {
        public EntityChartReportValueDTO()
        {

        }
        public int ID { set; get; }
        //public int RelationshipTailID { set; get; }
        //public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //  public string RelationshipPath { get; set; }
        //public int ColumnID { set; get; }
        //public string ColumnName { set; get; }
        public int EntityListViewColumnID { set; get; }
        public EntityListViewColumnsDTO EntityListViewColumn { set; get; }
        public ChartReportValueFunction FunctionType { set; get; }
        //public List<ColumnDTO> vwValueColumns { get; set; }
    }
    public enum ChartReportValueFunction
    {
        Count,
        Sum,
        Avg,
        Min,
        Max
    }

    public class DataMenuSettingDTO
    {
        public DataMenuSettingDTO()
        {
            ReportRelationships = new List<DataMenuReportRelationshipDTO>();
            DataViewRelationships = new List<DataMenuDataViewRelationshipDTO>();
            GridViewRelationships = new List<ModelEntites.DataMenuGridViewRelationshipDTO>();
        }
        public int ID { set; get; }
        public List<DataMenuDataViewRelationshipDTO> DataViewRelationships { set; get; }
        public List<DataMenuReportRelationshipDTO> ReportRelationships { set; get; }
        public List<DataMenuGridViewRelationshipDTO> GridViewRelationships { set; get; }
        public int EntityID { set; get; }

    }

    public class DataMenuDataViewRelationshipDTO
    {
        public DataMenuDataViewRelationshipDTO()
        {

        }
        public int ID { set; get; }
        // public int RelationshipID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public string Group1 { set; get; }
        public string Group2 { set; get; }
    }

    public class DataMenuReportRelationshipDTO
    {
        public List<EntitySearchableReportDTO> vwReports { set; get; }

        public DataMenuReportRelationshipDTO()
        {

        }
        public int EntitySearchableReportID { set; get; }
        public EntitySearchableReportDTO EntityReport { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public string Group1 { set; get; }
        public string Group2 { set; get; }
        public int ID { get; set; }
    }

    public class DataMenuGridViewRelationshipDTO
    {
        public DataMenuGridViewRelationshipDTO()
        {

        }
        public int ID { set; get; }
        // public int RelationshipID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public string Group1 { set; get; }
        public string Group2 { set; get; }
    }


    public class DataMenu
    {
        //public event EventHandler MenuClicked;
        public DataMenu()
        {
            SubMenus = new List<DataMenu>();
        }

        //public void OnMenuClicked()
        //{
        //    if (MenuClicked != null)
        //        MenuClicked(this, null);
        //}
        public string Tooltip { set; get; }
        public string Title { set; get; }
        public List<DataMenu> SubMenus { set; get; }
        public DataMenuType Type { set; get; }
        public DP_DataView ViewRelTargetDataItem { get; set; }
        public DataLinkDTO Datalink { get; set; }
        public DataMenuReportRelationshipDTO ReportRelationshipTail { get; set; }
        public EntityRelationshipTailDTO DataviewRelationshipTail { get; set; }
        public EntityRelationshipTailDTO GridviewRelationshipTail { get; set; }
        public EntityDirectReportDTO DirectReport { get; set; }
    }

    public enum DataMenuType
    {
        DataLink,
        RelationshipTailDataGrid,
        RelationshipTailDataView,
        RelationshipTailSearchableReport,
        Archive,
        Form,
        Folder,
        DirectReport,
        Letter,
        Workflow,
        ViewRel

    }

    public class EntityDirectReportDTO : EntityReportDTO
    {
        public EntityDirectReportDTO()
        {

        }

        public string URL { set; get; }
    }
    public class EntityCrosstabReportDTO : EntitySearchableReportDTO
    {
        public EntityCrosstabReportDTO()
        {
            Columns = new List<ModelEntites.CrosstabReportColumnDTO>();
            Rows = new List<ModelEntites.CrosstabReportRowDTO>();
            Values = new List<ModelEntites.CrosstabReportValueDTO>();
        }

        public ChartType ChartType { set; get; }

        public List<CrosstabReportColumnDTO> Columns { set; get; }
        public EntityListViewDTO EntityListView { get; set; }
        public int EntityListViewID { get; set; }
        public List<CrosstabReportRowDTO> Rows { set; get; }
        public List<CrosstabReportValueDTO> Values { set; get; }

    }

    public class CrosstabReportColumnDTO
    {
        public CrosstabReportColumnDTO()
        {

        }
        public int ID { set; get; }


        public int EntityListViewColumnID { get; set; }
        public EntityListViewColumnsDTO EntityListViewColumn { get; set; }
    }
    public class CrosstabReportRowDTO
    {
        public CrosstabReportRowDTO()
        {

        }

        public EntityListViewColumnsDTO EntityListViewColumn { get; set; }
        public int EntityListViewColumnID { get; set; }
        public int ID { set; get; }


    }

    public class CrosstabReportValueDTO
    {
        public CrosstabReportValueDTO()
        {

        }

        public EntityListViewColumnsDTO EntityListViewColumn { get; set; }
        public int EntityListViewColumnID { get; set; }
        public int ID { set; get; }

        public ChartReportValueFunction ValueFunction { set; get; }

    }
    public class EnumModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    //public class EntityListViewAllColumnsDTO
    //{
    //    public EntityListViewAllColumnsDTO()
    //    {

    //    }
    //    public int ColumnID { set; get; }
    //    public string ColumnName { set; get; }
    //    public short OrderID { set; get; }
    //    public string Alias { set; get; }
    //    public string RelationshipPath { set; get; }
    //    //public int EntityListViewRelationshipTailID { set; get; }
    //    //public  EntityRelationshipTailDTO EntityRelationshipTail { set; get; }
    //}
    public class EntityListReportSubsDTO
    {
        public List<EntityListReportDTO> vwListReports { set; get; }

        public EntityListReportSubsDTO()
        {
            SubsColumnsDTO = new List<ModelEntites.EntityListReportSubsColumnsDTO>();
        }
        public int ID { set; get; }
        public short OrderID { set; get; }
        public string Title { set; get; }
        public int EntityRelationshipTailID { set; get; }
        public int EntityListReportID { set; get; }
        public List<EntityListReportSubsColumnsDTO> SubsColumnsDTO { set; get; }
    }
    public class EntityListReportSubsColumnsDTO
    {
        public EntityListReportSubsColumnsDTO()
        {

        }
        public int ID { set; get; }

        public int ParentEntityListViewColumnsID { set; get; }
        public string ParentEntityListViewColumnAlias { set; get; }
        public string ParentEntityListViewColumnRelativeName { set; get; }
        public Enum_ColumnType ParentEntityListViewColumnType { set; get; }


        public int ChildEntityListViewColumnsID { set; get; }
        public string ChildEntityListViewColumnAlias { set; get; }
        public string ChildEntityListViewColumnRelativeName { set; get; }

        public Enum_ColumnType ChildEntityListViewColumnType { set; get; }
    }
    //public class DataItemDTO
    //{
    //    public int EntityID { set; get; }
    //    public int ID { set; get; }
    //}
    public class EntityCommandDTO
    {
        public EntityCommandDTO()
        {
            Entities = new List<EntityCommandEntityDTO>();
        }
        public int ID { set; get; }
        public List<EntityCommandEntityDTO> Entities { set; get; }
        //public int SecurityObjectID { set; get; }
        //public int FormulaID { set; get; }
        //public FormulaDTO Formula { set; get; }
        public int CodeFunctionID { set; get; }
        public CodeFunctionDTO CodeFunction { set; get; }
        //public string Value { set; get; }
        //public string Message { set; get; }
        public string Title { set; get; }
        public EntityCommandType Type { set; get; }
    }
    public class EntityCommandEntityDTO
    {
        public int EntityID { set; get; }
        public int ID { set; get; }

    }
    public enum EntityCommandType
    {
        None
    }

    public class EntityUISettingDTO
    {
        public EntityUISettingDTO()
        {
            //Columns = new List<ModelEntites.ColumnUISettingDTO>();
            //Relationships = new List<ModelEntites.RelationshipUISettingDTO>();
        }
        public int ID { set; get; }
        public short UIColumnsCount { set; get; }
        //public List<ColumnUISettingDTO> Columns { set; get; }
        //public List<RelationshipUISettingDTO> Relationships { set; get; }
    }
    public class ColumnUISettingDTO
    {
        public ColumnUISettingDTO()
        {

        }
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public Enum_UIColumnsType UIColumnsType { set; get; }
        public short UIRowsCount { set; get; }
    }

    public class RelationshipUISettingDTO
    {
        public RelationshipUISettingDTO()
        {

        }
        public int ID { set; get; }
        public int RelationshipID { set; get; }
        public bool Expander { set; get; }
        public Enum_UIColumnsType UIColumnsType { set; get; }
        public short UIRowsCount { get; set; }
        public bool IsExpanded { get; set; }
        //public Enum_UIRowsType UIRowsCount { set; get; }
    }
    public class EmptySpaceUISettingDTO
    {
        public EmptySpaceUISettingDTO()
        {

        }
        public int ID { set; get; }
        public bool ExpandToEnd { set; get; }
        public Enum_UIColumnsType UIColumnsType { set; get; }
        //public Enum_UIRowsType UIRowsCount { set; get; }
    }
    public class GroupUISettingDTO
    {
        public GroupUISettingDTO()
        {

        }
        public int ID { set; get; }
        //public int GroupID { set; get; }
        public bool Expander { set; get; }
        public short InternalColumnsCount { set; get; }
        public Enum_UIColumnsType UIColumnsType { set; get; }
        public short UIRowsCount { get; set; }
        public bool IsExpanded { get; set; }
        //public Enum_UIRowsType UIRowsCount { set; get; }
    }
    public class TabGroupUISettingDTO
    {
        public TabGroupUISettingDTO()
        {

        }
        public int ID { set; get; }
        public int TabGroupID { set; get; }
        public bool Expander { set; get; }
        public Enum_UIColumnsType UIColumnsType { set; get; }
        public short UIRowsCount { get; set; }
        public bool IsExpanded { get; set; }
        //public Enum_UIRowsType UIRowsCount { set; get; }
    }
    public class TabPageUISettingDTO
    {
        public TabPageUISettingDTO()
        {

        }
        public int ID { set; get; }
        public int TabPageID { set; get; }
        public short InternalColumnsCount { set; get; }
        //public Enum_UIColumnsType UIColumnsType { set; get; }
        //public Enum_UIRowsType UIRowsCount { set; get; }
    }
    public enum Enum_UIColumnsType
    {
        Normal,
        Half,
        Full
    }
    public enum Enum_UIRowsType
    {
        One,
        Two,
        Unlimited
    }
    public class DataViewSettingDTO
    {
        public DataViewSettingDTO()
        {
            //EntityDataViewRelationships = new List<ModelEntites.DataMenuDataViewRelationshipDTO>();
        }
        public int ID { set; get; }
        public byte[] IconContent { set; get; }
        public int EntityListViewID { set; get; }

    }



    public class GridViewSettingDTO
    {
        public GridViewSettingDTO()
        {
            //EntityGridViewRelationships = new List<ModelEntites.DataMenuGridViewRelationshipDTO>();
        }
        public int ID { set; get; }
        //public byte[] IconContent { set; get; }
        public int EntityListViewID { set; get; }

    }




    public class DataLinkDTO
    {
        public DataLinkDTO()
        {
            RelationshipsTails = new List<ModelEntites.DataLinkRelationshipTailDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }
        public int FirstSideEntityID { set; get; }
        public int SecondSideEntityID { set; get; }

        public List<DataLinkRelationshipTailDTO> RelationshipsTails { set; get; }
    }
    public class DataLinkRelationshipTailDTO
    {
        public int ID { set; get; }
        public int RelationshipTailID { set; get; }

        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //    public bool FromFirstSideToSecondSide { set; get; }
    }
    public class LetterTemplateDTO
    {
        public LetterTemplateDTO()
        {
            PlainFields = new List<ModelEntites.LetterTemplatePlainFieldDTO>();
            RelationshipFields = new List<ModelEntites.LetterTemplateRelationshipFieldDTO>();
        }
        public int TableDrivedEntityID { set; get; }
        public string Name { set; get; }
        public int ID { set; get; }
        public List<LetterTemplatePlainFieldDTO> PlainFields { set; get; }
        public List<LetterTemplateRelationshipFieldDTO> RelationshipFields { set; get; }
        public int EntityListViewID { set; get; }
        public EntityListViewDTO EntityListView { get; set; }
    }
    public class PartialLetterTemplateDTO : LetterTemplateDTO
    {
        public PartialLetterTemplateDTO()
        {
        }

    }


    public class MainLetterTemplateDTO : LetterTemplateDTO
    {
        public MainLetterTemplateDTO()
        {

        }

        public LetterTemplateType Type { set; get; }
        public string FileExtension { set; get; }
        public byte[] Content { set; get; }


    }


    public enum LetterTemplateType
    {
        None
    }
    public class LetterTemplatePlainFieldDTO
    {
        public LetterTemplatePlainFieldDTO()
        {

        }
        public string tmpParentTail { set; get; }
        public int ID { set; get; }
        public int EntityListViewColumnsID { set; get; }
        public EntityListViewColumnsDTO EntityListViewColumns { set; get; }
        public int FormulaID { set; get; }
        public string FieldName { set; get; }
        public object LetterField { set; get; }

    }

    public class LetterTemplateRelationshipFieldDTO
    {
        public LetterTemplateRelationshipFieldDTO()
        {
            tmpPartialLetterTemplates = new List<ModelEntites.PartialLetterTemplateDTO>();
            //tmpEntityPreDefinedSearches = new List<ModelEntites.EntityPreDefinedSearchDTO>();
            PartialLetterTemplate = new PartialLetterTemplateDTO();
        }
        public object ParagrafFormat { set; get; }
        public object StartLetterField { set; get; }
        public object EndLetterField { set; get; }
        public int ID { set; get; }
        //public bool NextLine { set; get; }
        public bool IsRow { set; get; }
        public string FieldName { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //public int InternalLetterTemplateID { set; get; }
        //public int EntityID { set; get; }
        //public LetterTemplateFieldOwnerDTO FieldOwnerDTO { set; get; }
        //public List<LetterRelationshipTemplateDTO> tmpRelationshipTemplates { set; get; }
        public Tuple<int, int> tmpConsumedRange { set; get; }
        //public int EntityPreDefinedSearchID { get; set; }
        //public List<EntityPreDefinedSearchDTO> tmpEntityPreDefinedSearches { set; get; }
        public List<PartialLetterTemplateDTO> tmpPartialLetterTemplates { set; get; }
        public PartialLetterTemplateDTO PartialLetterTemplate { set; get; }
        public int PartialLetterTemplateID { set; get; }

    }

    //public class LetterRelationshipTemplateDTO
    //{
    //    public LetterRelationshipTemplateDTO()
    //    {
    //        Fields = new List<ModelEntites.LetterTemplateFieldDTO>();
    //    }
    //    public int ID { set; get; }

    //    public int EntityID { set; get; }

    //    public string Name { set; get; }
    //    //public int RelatoinshipFilteredID { set; get; }
    //    public List<LetterTemplateFieldDTO> Fields { set; get; }

    //}

    public enum LetterTemplateFieldType
    {
        Column,
        Parameter,
        RangeRelationship
    }
    //public class RelatoinshipFilteredDTO
    //{
    //    public int ID { set; get; }
    //    public int RelationshipID { set; get; }
    //    public int EntityPreDefinedSearchID { set; get; }
    //    public EntityPreDefinedSearchDTO EntityPreDefinedSearch { set; get; }
    //}
    public class LetterSettingDTO
    {
        public LetterSettingDTO()
        {

        }

        public int LetterExternalInfoCodeID { set; get; }
        //    public CodeFunctionDTO LetterExternalInfoCode { set; get; }
        public int BeforeLetterLoadCodeID { set; get; }
        //  public CodeFunctionDTO BeforeLetterLoadCode { set; get; }
        public int BeforeLetterSaveCodeID { set; get; }
        //  public CodeFunctionDTO BeforeLetterSaveCode { set; get; }
        public int AfterLetterSaveCodeID { set; get; }
        public int LetterSendToExternalCodeID { get; set; }
        //  public CodeFunctionDTO AfterLetterSaveCode { set; get; }


    }

    //public class EntityPreDefinedSearchDTO
    //{
    //    public EntityPreDefinedSearchDTO()
    //    {
    //        //  SimpleColumns = new List<ModelEntites.PreDefinedSearchColumns>();
    //    }
    //    public int ID { set; get; }
    //    public int TabledDrivedEntityID { set; get; }
    //    public DP_SearchRepository SearchRepository { set; get; }
    //    public string Title { get; set; }
    //    public int EntitySearchID { get; set; }
    //    // public bool IsSimpleSearch { get; set; }

    //    //  public List<PreDefinedSearchColumns> SimpleColumns { set; get; }
    //}
    public class PreDefinedSearchColumns
    {
        public int ID { set; get; }
        public int ColumnID { set; get; }
        public int EntitySearchColumnsID { get; set; }
        public CommonOperator Operator { get; set; }
        public string Value { get; set; }
    }

    public class LetterDTO
    {
        public string vwLetterType;

        public LetterDTO()
        {
            RelatedDataItems = new List<ModelEntites.DataItemDTO>();
        }
        public int ID { set; get; }
        public List<DataItemDTO> RelatedDataItems { set; get; }
        public DP_DataView DataItem { set; get; }
        public int LetterTypeID { set; get; }
        public int RelatedLetterID { set; get; }
        public int LetterTemplateID { set; get; }

        public bool IsExternalOrInternal { set; get; }
        public bool? IsGeneratedOrSelected { set; get; }
        public string Title { set; get; }
        public string ExternalCode { set; get; }
        public string Desc { set; get; }
        public DateTime LetterDate { set; get; }
        public FileRepositoryDTO AttechedFile { set; get; }
        public int AttechedFileID { set; get; }
        public bool HasAttechedFile { set; get; }
        public DateTime CreationDate { set; get; }
        public int UserID { set; get; }
        public string vwUser { set; get; }
        public string LetterNumber { get; set; }
    }
    public class LetterTypeDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }

    public class FileRepositoryDTO
    {
        public FileRepositoryDTO()
        {
            tmpTagIDs = new List<int>();
        }
        public int ID { set; get; }
        public string FileName { set; get; }
        public string FileExtension { set; get; }
        public byte[] Content { set; get; }

        public string tmpPath { set; get; }
        public FileRepositoryState tmpState { set; get; }
        public string tmpUploadMessage { set; get; }

        public List<int> tmpTagIDs { set; get; }
        public string tmpTags { set; get; }
    }
    public enum FileRepositoryState
    {
        Normal,
        Succeed,
        Failed
    }
    public class DataItemDTO
    {
        public DataItemDTO()
        {

        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public List<EntityInstanceProperty> KeyProperties { set; get; }

    }

    //public class ArchiveItemDataItemDTO
    //{
    //    public ArchiveItemDataItemDTO()
    //    {
    //        TagIds = new List<int>();
    //    }
    //    public int ID { set; get; }
    //    public int DatItemID { set; get; }
    //    public DataItemDTO DatItem { set; get; }
    //    public int? FolderID { set; get; }

    //    public List<int> TagIds { set; get; }
    //    public ArchiveItemDTO ArchiveItem { set; get; }
    //    public int ArchiveItemID { set; get; }
    //}
    //public class EntityOrganizationSecurityDirectDTO
    //{
    //    public EntityOrganizationSecurityDirectDTO()
    //    {

    //    }
    //    public int ID { set; get; }
    //    public int TableDrivedEntityID { set; get; }
    //    public string EntityName { set; get; }
    //    public int ColumnID { set; get; }
    //    public int DBFunctionID { set; get; }
    //    public EntitySecurityOperator Operator { set; get; }
    //}
    //public class EntityOrganizationSecurityInDirectDTO
    //{
    //    public EntityOrganizationSecurityInDirectDTO()
    //    {

    //    }
    //    public int ID { set; get; }
    //    public int TableDrivedEntityID { set; get; }
    //    public int DirectOrganizationSecurityID { set; get; }
    //    public EntityOrganizationSecurityDirectDTO DirectOrganizationSecurity { set; get; }
    //    public int RelationshipTailID { set; get; }
    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }

    //}

    //public class RoleOrRoleGroupDTO
    //{
    //    public int ID { set; get; }
    //    public string Name { set; get; }
    //    public RoleOrRoleGroupType Type { set; get; }

    //}
    //public enum RoleOrRoleGroupType
    //{
    //    Role,
    //    RoleGroup

    //}
    //public class RoleGroupDTO
    //{
    //    public RoleGroupDTO()
    //    {
    //        Roles = new List<RoleDTO>();
    //    }
    //    public int ID { set; get; }
    //    public string Name { set; get; }
    //    public List<RoleDTO> Roles { set; get; }
    //}
    public class PermissionDTO
    {
        public PermissionDTO()
        {
            Actions = new List<SecurityAction>();
        }
        public int ID { set; get; }
        public List<SecurityAction> Actions { set; get; }
        public int SecurityObjectID { set; get; }
        public int SecuritySubjectID { set; get; }
        public DatabaseObjectCategory SecurityObjectCategory { set; get; }
        //public DatabaseObjectCategory ObjectCategory { set; get; }
        //public RoleOrRoleGroupDTO RoleOrRoleGroup { set; get; }


    }
    public class ConditionalPermissionDTO
    {
        public ConditionalPermissionDTO()
        {
            Actions = new List<SecurityAction>();
        }
        public string Title { set; get; }
        public int ID { set; get; }
        public List<SecurityAction> Actions { set; get; }
        public int EntityID { set; get; }
        public string EntityName { set; get; }
        //public int ColumnID { set; get; }
        //public int CommandID { set; get; }
        public string ColumnName { set; get; }

        public int FormulaID { set; get; }
        public FormulaDTO Formula { set; get; }
        public int SecurityObjectID { set; get; }
        public int SecuritySubjectID { set; get; }
        public SecuritySubjectDTO SecuritySubject { set; get; }
        public SecurityObjectDTO SecurityObject { set; get; }
        public int ConditinColumnID { set; get; }
        public string Value { set; get; }
        public bool HasNotRole { get; set; }
    }

    //public class EntityRoleSecurityDTO
    //{
    //    public int ID { set; get; }
    //    public int TableDrivedEntityID { set; get; }
    //    public int RoleID { set; get; }
    //}
    public class EntitySecurityDirectDTO
    {
        public EntitySecurityDirectDTO()
        {
            Conditions = new List<ModelEntites.EntitySecurityConditionDTO>();

        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public int SecuritySubjectID { set; get; }
        //   public AndORType ConditionAndORType { set; get; }
        public SecuritySubjectDTO SecuritySubject { set; get; }
        public List<EntitySecurityConditionDTO> Conditions { set; get; }
        public bool IgnoreSecurity { set; get; }
        public SecurityMode Mode { set; get; }

    }
    public enum SecurityMode
    {
        View,
        Edit
    }
    public class EntitySecurityConditionDTO
    {
        public EntitySecurityConditionDTO()
        {
            Columns = new List<ColumnDTO>();
        }
        public int ID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public int ColumnID { set; get; }
        public ColumnDTO Column { set; get; }
        public SecurityReservedValue ReservedValue { set; get; }
        public int DBFunctionID { set; get; }
        public string Value { set; get; }

        public EntitySecurityOperator Operator { set; get; }
        public List<ColumnDTO> Columns { set; get; }
    }
    public enum SecurityReservedValue
    {
        None,
        OrganizationID,
        OrganizationTypeID,
        RoleTypeID,
        OrganizationTypeRoleTypeID,
        OrganizationPostID
    }
    public class EntitySecurityInDirectDTO
    {
        public EntitySecurityInDirectDTO()
        {

        }
        public int ID { set; get; }
        public int TableDrivedEntityID { set; get; }
        public int DirectRoleSecurityID { set; get; }

        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }

    }

    public enum EntitySecurityOperator
    {
        Equals,
        In
    }


    //public class EntityActionActivityDTO
    //{
    //    public EntityActionActivityDTO()
    //    {

    //    }
    //    public int ID { set; get; }
    //    public int EntityID { set; get; }
    //    public int ActionActivityID { set; get; }
    //    public ActionActivityDTO ActionActivity { set; get; }
    //    public Enum_EntityActionActivityStep Step { set; get; }
    //    public bool ResultSensetive { set; get; }
    //}
    public enum Enum_EntityActionActivityStep
    {
        //AsRootForm اضافه شد به اسامی زیرا اگر انتیتی فرم شروع کننده باشه صدا زده میشوند و اگر از طریق فرم وابسته باشد صدا زده نمیشوند
        None,
        BeforeLoad,
        BeforeSave,
        AfterSave,
        BeforeDelete,
        AfterDelete
    }

    public class UIActionActivityDTO
    {
        public UIActionActivityDTO()
        {
            UIColumnValueRange = new List<ModelEntites.UIColumnValueRangeDTO>();
            UIColumnValue = new List<ModelEntites.UIColumnValueDTO>();
            UIEnablityDetails = new List<ModelEntites.UIEnablityDetailsDTO>();
            UIColumnValueRangeReset = new List<ModelEntites.UIColumnValueRangeResetDTO>();
        }
        public int ID { set; get; }
        public string RelatedStates { set; get; }
        //public int ColumnValueID { set; get; }
        public List<UIColumnValueDTO> UIColumnValue { set; get; }
        public int UIEnablityID { set; get; }
        public List<UIEnablityDetailsDTO> UIEnablityDetails { set; get; }
        public string Title { set; get; }
        public int EntityID { set; get; }
        public Enum_ActionActivityType Type { set; get; }
        public List<UIColumnValueRangeDTO> UIColumnValueRange { set; get; }
        public List<UIColumnValueRangeResetDTO> UIColumnValueRangeReset { set; get; }
    }
    public class UIColumnValueRangeDTO
    {
        public UIColumnValueRangeDTO()
        {
            vwCandidateValues = new List<string>();
        }
        public int ID { set; get; }
        public int ColumnValueRangeID { set; get; }
        public ColumnValueRangeDTO ColumnValueRange { set; get; }
        public EnumColumnValueRangeTag EnumTag { set; get; }

        public string Value { set; get; }

        public List<string> vwCandidateValues { set; get; }
    }
    public class UIColumnValueRangeResetDTO
    {
        public UIColumnValueRangeResetDTO()
        {

        }
        public int ID { set; get; }
        public int ColumnValueRangeID { set; get; }

    }

    public enum EnumColumnValueRangeTag
    {
        Value,
        Title,
        Tag1,
        Tag2
    }


    public class BackendActionActivityDTO
    {
        public BackendActionActivityDTO()
        {

        }
        public int ID { set; get; }

        //public string RelatedStates { set; get; }
        public int CodeFunctionID { set; get; }
        public CodeFunctionDTO CodeFunction { set; get; }
        public int DatabaseFunctionID { set; get; }
        public DatabaseFunctionDTO DatabaseFunction { set; get; }
        //public Enum_ActionActivityType ActionActivityType { set; get; }



        public string Title { set; get; }
        public int EntityID { set; get; }

        public Enum_EntityActionActivityStep Step { set; get; }
        public bool ResultSensetive { set; get; }

        public Enum_ActionActivityType Type { set; get; }
    }

    public class ArchiveTagDTO
    {
        public ArchiveTagDTO()
        {

        }

        public int? EntityID { get; set; }
        public int ID { set; get; }
        public bool tmpSelect { set; get; }
        public string Name { set; get; }
    }
    public class ArchiveFolderDTO
    {
        public ArchiveFolderDTO()
        {

        }
        public int? EntityID { get; set; }
        public int ID { set; get; }
        public string Name { set; get; }
        public int tmpCount { set; get; }
    }
    public class ArchiveFolderWithNullDTO
    {
        public ArchiveFolderWithNullDTO()
        {

        }
        public int? EntityID { get; set; }
        public int? ID { set; get; }
        public string Name { set; get; }
        public int tmpCount { set; get; }
    }

    public class ArchiveItemDTO
    {
        public ArchiveItemDTO()
        {
            TagIDs = new List<int>();
        }
        public FileRepositoryDTO ThumbnailFile { get; set; }
        public FileRepositoryDTO AttechedFile { get; set; }
        public object AttechedFileID { get; set; }
        public DateTime CreationDate { get; set; }
        //    public int vwDatItemID { set; get; }
        public DP_DataView DatItem { set; get; }
        public int? FolderID { set; get; }
        public int ID { set; get; }
        public string Name { set; get; }
        public List<int> TagIDs { set; get; }
        public Enum_ArchiveItemFileType FileType { set; get; }
        public Enum_ArchiveItemMainType MainType { set; get; }
        public int UserID { get; set; }
        public string Tooltip { get; set; }
        public string Color { get; set; }
    }
    public enum Enum_ArchiveItemFileType
    {
        UnKnown,
        JPEG,
        GIF,
        BMP,
        PNG,
        PDF,
        DOC,
        DOCX
    }
    public enum Enum_ArchiveItemMainType
    {
        UnKnown,
        Image,
        Pdf,
        MsWord,
    }
    public enum Enum_ActionActivityType
    {
        CodeFunction,
        DatabaseFunction,
        UIEnablity,
        ColumnValueRange,
       // ColumnValueRangeResetOld,
        ColumnValue
    }

    public class ArchiveRelationshipTailDTO
    {
        public ArchiveRelationshipTailDTO()
        {

        }
        public int EntityID { get; set; }
        public int ID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
    }
    public class LetterRelationshipTailDTO
    {
        public LetterRelationshipTailDTO()
        {

        }
        public int EntityID { get; set; }
        public int ID { set; get; }
        public int RelationshipTailID { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
    }
    public class RelationshipFilterDTO
    {
        public RelationshipFilterDTO()
        {
            //    RelationshipFilterColumns = new List<ModelEntites.RelationshipFilterColumnDTO>();
        }
        public int ID { set; get; }
        public int RelationshipID { set; get; }
        public int ValueRelationshipTailID { set; get; }
        public int SearchRelationshipTailID { set; get; }
        public EntityRelationshipTailDTO ValueRelationshipTail { set; get; }
        public EntityRelationshipTailDTO SearchRelationshipTail { set; get; }
        public int ValueColumnID { set; get; }
        public int SearchColumnID { set; get; }

        public List<ColumnDTO> vwValueColumns { set; get; }
        public List<ColumnDTO> vwSearchColumns { set; get; }
        //public int SearchRelationshipTailID { set; get; }
        //public EntityRelationshipTailDTO SearchRelationshipTail { set; get; }

        //public List<RelationshipFilterColumnDTO> RelationshipFilterColumns { set; get; }
    }
    //public class RelationshipFilterColumnDTO
    //{
    //    public int ValueColumnID { set; get; }
    //    public int SearchColumnID { set; get; }
    //}

    public class CodeFunctionDTO
    {
        public CodeFunctionDTO()
        {
            Parameters = new List<ModelEntites.CodeFunctionColumnDTO>();
        }
        public bool? ShowInFormula { set; get; }
        public int ID { set; get; }

        public int EntityID { set; get; }
        public string Path { set; get; }
        public string ClassName { set; get; }

        public string RetrunType { set; get; }
        public Type RetrunDotNetType { set; get; }
        public string FunctionName { set; get; }

        public Enum_CodeFunctionParamType ParamType { set; get; }
        public string Catalog { set; get; }

        public List<CodeFunctionColumnDTO> Parameters { set; get; }
        public string Name { get; set; }

        //public ValueCustomType ValueCustomType { set; get; }
    }
    public class CodeFunctionColumnDTO
    {
        public CodeFunctionColumnDTO()
        {

        }
        public int ID { set; get; }
        //public int ColumnID { set; get; }
        public string DataType { set; get; }
        public Type DotNetType { set; get; }
        public string ParameterName { set; get; }

        //public int ColumnID { set; get; }
    }

    public enum Enum_CodeFunctionParamType
    {
        ManyDataItems,
        OneDataItem,
        KeyColumns,
        CommandFunction,
        LetterFunction,
        LetterConvert
    }
    public class EntityRelationshipTailDTO
    {
        public EntityRelationshipTailDTO()
        {
            //RelationshipColumns = new List<ModelEntites.RelationshipColumnDTO>();

        }
        public int ID { set; get; }
        public int InitialEntityID { set; get; }
        public int TargetEntityID { get; set; }
        //public int ReverseRelationshipTailID { set; get; }
        //public int RelationshipSourceEntityID { set; get; }
        //public int RelationshipTargetEntityID { set; get; }
        //public int RelationshipID { set; get; }
        public RelationshipDTO Relationship { set; get; }
        //public List<RelationshipColumnDTO> RelationshipColumns { set; get; }
        //public List<int> RelationshipSecondSideColumnIds { set; get; }
        // public RelationshipDTO LastRelationship { set; get; }

        //public Enum_RelationshipType SourceToTargetRelationshipType;
        //public Enum_MasterRelationshipType SourceToTargetMasterRelationshipType;
        public string TargetEntityAlias { set; get; }
        public int TargetEntityPreDefinedSearchID { set; get; }
        //public EntityPreDefinedSearchDTO TargetEntityPreDefinedSearch { set; get; }
        public EntityRelationshipTailDTO ChildTail { set; get; }
        public string RelationshipIDPath { set; get; }
        public string EntityPath { set; get; }
        public bool IsOneToManyTail { set; get; }
        public EntityRelationshipTailDTO ReverseRelationshipTail { set; get; }

        //public string TargetEntityAlias { get; set; }
    }
    //public class SearchEntityRelationshipTailDTO
    //{
    //    public int EntityID { get; set; }
    //    public EntityPreDefinedSearchDTO EntityPreDefinedSearch { set; get; }

    //}


    public class EntityStateDTO
    {
        public EntityStateDTO()
        {
            ActionActivities = new ObservableCollection<ModelEntites.UIActionActivityDTO>();
            Values = new List<ModelEntites.EntityStateValueDTO>();
        }
        public int ID { set; get; }
        //public bool Preserve { set; get; }
        public int TableDrivedEntityID { set; get; }
        public int FormulaID { set; get; }
        public FormulaDTO Formula { set; get; }
        public int RelationshipIDTailID { set; get; }
        public string RelationshipIDTail { set; get; }
        public int ColumnID { set; get; }
        public Enum_EntityStateOperator EntityStateOperator { set; get; }
        public string Title { set; get; }
        public List<EntityStateValueDTO> Values { set; get; }
        //public int ActionActivityID { set; get; }
        public ObservableCollection<UIActionActivityDTO> ActionActivities { set; get; }
    }
    public class EntityStateValueDTO
    {
        public string Value { set; get; }
    }
    public enum Enum_EntityStateOperator
    {
        Equals,
        NotEquals
    }
    public class ObjectDTO
    {
        public ObjectDTO()
        {
            ChildObjects = new List<ObjectDTO>();
        }
        //public string ParentCategory { set; get; }
        //public string ParentItemIdentity { set; get; }
        //public int SecurityObjectID { set; get; }
        public int ObjectIdentity { set; get; }
        //   public object Object { set; get; }
        public string Title { set; get; }
        public DatabaseObjectCategory ObjectCategory { set; get; }
        public EntityObjectType EntityType { set; get; }
        public List<ObjectDTO> ChildObjects { set; get; }
        public bool NeedsExplicitPermission { set; get; }
        public string Name { get; set; }
        public int TableDrivedEntityID { get; set; }
    }
    public enum EntityObjectType
    {
        None,
        Entity,
        View
    }
    public enum DatabaseObjectCategory
    {
        Database,
        Schema,
        Entity,
        Column,
        Relationship,
        Group,
        TabControl,
        TabPage,
        Folder,
        CodeFunction,
        DatabaseFunction,
        Formula,
        Command,
        Report,
        DirectDataReport,
        RootMenu,
        Menu,
        Archive,
        Letter,
        EmptySpace,
        DataView,
        GridView
    }


    public class FormulaDTO
    {
        public FormulaDTO()
        {
            FormulaItems = new List<FormulaItemDTO>();
            //AllFormulaItems = new List<ModelEntites.FormulaItemDTO>();
        }
        public int EntityID { set; get; }
        public int ID { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public string Formula { set; get; }
        public int Version { set; get; }
        public string ResultType { set; get; }
        public bool FormulaUsed { set; get; }
        //public ValueCustomType ValueCustomType { set; get; }
        public Type ResultDotNetType { get; set; }

        public List<FormulaItemDTO> FormulaItems;
    }
    //public class FormulaUsageDTO
    //{
    //    public FormulaUsageDTO()
    //    {
    //        FormulaUsageParemeters = new List<FormulaUsageParemetersDTO>();
    //    }
    //    public List<FormulaUsageParemetersDTO> FormulaUsageParemeters { set; get; }
    //    public int FormulaID { set; get; }
    //    public int ID { set; get; }
    //    public int ColumnID { set; get; }
    //    public int DataItemID { set; get; }
    //}

    public class FormulaResult
    {
        public object Result { set; get; }
        public List<FormulaUsageParemetersDTO> FormulaUsageParemeters { set; get; }
        public string Exception { set; get; }
        //private StateFunctionResult GetStateFunctionResult(StateFunctionDTO StateFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(StateFunction.Path, StateFunction.ClassName, StateFunction.FunctionName, parameters.ToArray());
        //    return result as StateFunctionResult;
        //}




    }
    //public enum ValueCustomType
    //{
    //    None,
    //    IsPersianDate
    //}
    //public class Formula_FormulaParameterDTO
    //{

    //    public int ID { set; get; }

    //    public int ColumnID { set; get; }

    //    public int FormulaIDOfFormulaParameter { set; get; }
    //    public int FormulaParameterID { set; get; }
    //    public string FormulaParameterPath { set; get; }
    //    //public string Name { set; get; }
    //    public string Title { set; get; }

    //    public string Value { set; get; }

    //}
    public class FormulaItemDTO
    {
        public FormulaItemDTO()
        {
            //     ChildFormulaItems = new List<FormulaItemDTO>();
        }
        //public int ParentRelationshipID { set; get; }
        //public FormulaObject FomulaObject { set; get; }

        //public FormulaItem ParentFormulaItem { set; get; }
        //public List<FormulaObject> RelationshipFomulaObjects { set; get; }
        public FormuaItemType ItemType { set; get; }
        public int ItemID { set; get; }
        public string ItemTitle { set; get; }
        //public int FormulaIDOfFormulaParameter { set; get; }
        public int FormulaID { set; get; }
        public int ID { set; get; }
        public string RelationshipNameTail { get; set; }

        //public int FormulaParameterID { set; get; }
        //public int RelationshipID { set; get; }

        public string RelationshipIDTail { get; set; }

        //public List<FormulaItemDTO> ChildFormulaItems { set; get; }


    }
    public enum FormuaItemType
    {
        Column,
        FormulaParameter,
        Relationship,
        DatabaseFunction,
        Code,
        State,
        None
    }
    //public class FormulaParameterDTO
    //{
    //    public FormulaParameterDTO()
    //    {

    //    }
    //    public int ID { set; get; }
    //    public int EntityID { set; get; }
    //    public int FormulaID { set; get; }
    //    public string Name { set; get; }
    //    public string Title { set; get; }
    //    public Type ResultType { set; get; }

    //}

    public class DatabaseFunctionDTO
    {
        public DatabaseFunctionDTO()
        {
            DatabaseFunctionParameter = new List<ModelEntites.DatabaseFunctionColumnDTO>();
        }
        public int ID { set; get; }
        public string FunctionName { set; get; }
        public string Catalog { set; get; }
        public int SchemaID { set; get; }
        public int DatabaseID { set; get; }
        public string RelatedSchema { set; get; }
        public string ReturnType { set; get; }

        public bool Enable { set; get; }
        //public ValueCustomType ValueCustomType { set; get; }
        public Type DotNetType { set; get; }
        public Enum_DatabaseFunctionType Type { set; get; }
        public string Title { set; get; }

        public List<DatabaseFunctionColumnDTO> DatabaseFunctionParameter { set; get; }
    }

    public class DatabaseFunctionColumnDTO
    {
        public DatabaseFunctionColumnDTO()
        {

        }
        public bool Enable { set; get; }
        public int ID { set; get; }
        //public int ColumnID { set; get; }
        public string DataType { set; get; }
        public Type DotNetType { set; get; }
        public string ParameterName { set; get; }

    }
    public enum Enum_DatabaseFunctionType
    {
        None,
        Function,
        StoredProcedure
    }
    public class DatabaseFunction_EntityDTO
    {
        public DatabaseFunction_EntityDTO()
        {
            DatabaseFunctionEntityColumns = new List<ModelEntites.DatabaseFunction_Entity_ColumnDTO>();
        }
        public int ID { set; get; }
        public int EntityID { set; get; }
        public DatabaseFunctionDTO DatabaseFunction { set; get; }
        public List<DatabaseFunction_Entity_ColumnDTO> DatabaseFunctionEntityColumns { set; get; }
        public int DatabaseFunctionID { get; set; }
    }

    public class DatabaseFunction_Entity_ColumnDTO
    {

        public int DatabaseFunction_EntityID { set; get; }
        public int DatabaseFunctionParameterID { set; get; }
        public int ColumnID { set; get; }

        public string FunctionDataType { set; get; }
        public string FunctionColumnParamName { set; get; }
        public Type FunctionColumnDotNetType { set; get; }

        public Enum_FixedParam FixedParam { set; get; }
        public int ID { get; set; }
    }
    public enum Enum_FixedParam
    {
        None,
        RequesterIdentity
    }

    public class CodeFunction_EntityDTO
    {
        public CodeFunction_EntityDTO()
        {
            CodeFunctionEntityColumns = new List<ModelEntites.CodeFunction_Entity_ColumnDTO>();
        }
        public bool? ShowInFormula { set; get; }
        public int ID { set; get; }
        public int EntityID { set; get; }
        public CodeFunctionDTO CodeFunction { set; get; }
        public List<CodeFunction_Entity_ColumnDTO> CodeFunctionEntityColumns { set; get; }
        public int CodeFunctionID { get; set; }
    }

    public class CodeFunction_Entity_ColumnDTO
    {

        public int CodeFunction_EntityID { set; get; }
        public int CodeFunctionParameterID { set; get; }
        public int ColumnID { set; get; }

        public string FunctionDataType { set; get; }
        public string FunctionColumnParamName { set; get; }
        public Type FunctionColumnDotNetType { set; get; }
        public int ID { get; set; }
    }

    //public enum Enum_FormulaIntention
    //{
    //    FormulaForColumn,
    //    FormulaForParameter
    //}
    //public enum Enum_FormulaParameterIntention
    //{
    //    FormulaParameterForTable

    //}

    public enum Enum_FormulaDefinitionIntention
    {
        FormulaForFormula,
        FormulaForFormulaParameter

    }


    public enum EntityColumnInfoType
    {
        WithoutColumn,
        WithSimpleColumns,
        WithFullColumns,

    }
    public enum EntityRelationshipInfoType
    {

        WithRelationships,
        WithoutRelationships

    }





    public enum CommonOperator
    {
        Equals,
        Contains,
        StartsWith,
        EndsWith,
        BiggerThan,
        SmallerThan,
        InValues
    }


    public class SimpleSearchOperator
    {
        public CommonOperator Operator { set; get; }
        public string Title { set; get; }
    }
    public class SearchOperator
    {
        public string Name { set; get; }
        public string Title { set; get; }
    }
    public enum StringOperator
    {
        Equals,
        Contains,
        StartsWith,
        EndsWith,
        NotEquals,
        In,
        NullValue
    }

    public enum SearchEnumerableType
    {
        None,
        From,
        To,
        Null,
        HasRelation,
        HasNotRelation
    }












}
