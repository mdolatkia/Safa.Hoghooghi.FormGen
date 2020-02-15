using CommonDefinitions.UISettings;
using ModelEntites;

using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;
using MyUILibraryInterfaces.DataReportArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public abstract class BaseEditEntityArea : I_EditEntityArea
    {
        public event EventHandler<EditAreaGeneratedArg> RelationshipAreaGenerated;
        //public event EventHandler<EditAreaDataItemLoadedArg> DataItemLoaded;
        public event EventHandler<EditAreaDataItemLoadedArg> DataItemShown;

        public event EventHandler<DisableEnableChangedArg> DisableEnableChanged;
        public event EventHandler DataViewGenerated;
        public abstract void DataItemVisiblity(object data, bool visiblity);
        public abstract void DataItemEnablity(object data, bool visiblity);
        public abstract bool AddData(DP_DataRepository data, bool showDataInDataView);
        public abstract bool ShowDataInDataView(DP_DataRepository dataItem);
        public BaseEditEntityArea(TableDrivedEntityDTO simpleEntity)
        {
            RunningActionActivities = new List<UIActionActivityDTO>();
            SimpleEntity = simpleEntity;

            Commands = new List<I_Command>();
            //MessageItems = new ObservableCollection<BaseMessageItem>();
            //  ValidationItems.CollectionChanged += ValidationItems_CollectionChanged;
            //  BusinesMessageItems = new ObservableCollection<BaseMessageItem>();
            //   BusinesMessageItems.CollectionChanged += ControlMessageItems_CollectionChanged;
            FormulaExceptionReulsts = new ObservableCollection<FormulaResult>();
            FormulaExceptionReulsts.CollectionChanged += FormulaExceptionReulsts_CollectionChanged;
        }



        private void FormulaExceptionReulsts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ////if (e.Action == NotifyCollectionChangedAction.Reset)
            ////{
            ////    foreach (var columnControl in SimpleColumnControls)
            ////    {
            ////        columnControl.ControlManager.ClearFormulaMessage,...();
            ////    }
            ////    foreach (var columnControl in RelationshipColumnControls)
            ////    {
            ////        columnControl.ControlManager.ClearValidation();
            ////    }
            ////}
            ////else if (e.Action == NotifyCollectionChangedAction.Add)
            ////{
            ////    foreach (BaseValidationItem item in e.NewItems)
            ////    {
            ////        if (item is SimpleColumnValidationItem)
            ////        {
            ////            var simpleColumncontrol = (item as SimpleColumnValidationItem).ColumnControl;
            ////            (simpleColumncontrol as SimpleColumnControl).ControlManager.SetValidation(item.Message, item.Color);
            ////        }
            ////        else if (item is RelationshipColumnValidationItem)
            ////        {
            ////            var relationshipColumncontrol = (item as RelationshipColumnValidationItem).RelationshipControl;
            ////            (relationshipColumncontrol as RelationshipColumnControl).ControlManager.SetValidation(item.Message, item.Color);
            ////        }
            ////    }
            ////}
        }
        public EditEntityAreaInitializer AreaInitializer
        {
            set;
            get;
        }
        List<EntityValidationDTO> _EntityValidations;
        public List<EntityValidationDTO> EntityValidations
        {
            get
            {
                if (_EntityValidations == null)
                    _EntityValidations = AgentUICoreMediator.GetAgentUICoreMediator.entityValidationManagerService.GetEntityValidations(AreaInitializer.EntityID);
                return _EntityValidations;
            }
        }

        public List<I_Command> Commands { set; get; }
        List<EntityCommandDTO> _EntityCommands;
        public List<EntityCommandDTO> EntityCommands
        {
            get
            {
                if (_EntityCommands == null)
                    _EntityCommands = AgentUICoreMediator.GetAgentUICoreMediator.entityCommandManagerService.GetEntityCommands(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                return _EntityCommands;
            }
        }
        //public ObservableCollection<BaseMessageItem> MessageItems
        //{
        //    set; get;
        //}
        //public ObservableCollection<BaseMessageItem> BusinesMessageItems
        //{
        //    set; get;
        //}
        ChildRelationshipInfo _ChildRelationshipInfo;
        public ChildRelationshipInfo ChildRelationshipInfo
        {
            get
            {
                return _ChildRelationshipInfo;
            }
            set
            {
                _ChildRelationshipInfo = value;
            }
        }
        public I_EditEntityLetterArea EditLetterArea
        {
            set; get;
        }
        public I_DataListReportAreaContainer DataListReportAreaContainer
        {
            set; get;
        }
        public I_EntityLettersArea EntityLettersArea
        {
            set; get;
        }
        EntityListViewDTO _DefaultEntityListViewDTO;
        public EntityListViewDTO DefaultEntityListViewDTO
        {
            get
            {
                if (_DefaultEntityListViewDTO == null)
                    _DefaultEntityListViewDTO = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetDefaultEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                return _DefaultEntityListViewDTO;
            }
        }




        TableDrivedEntityDTO _EntityWithSimpleColumns;
        public TableDrivedEntityDTO EntityWithSimpleColumns
        {
            get
            {
                if (_EntityWithSimpleColumns == null)
                {
                    if (FullEntity != null)
                        return FullEntity;
                    else
                        _EntityWithSimpleColumns = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityWithSimpleColumns(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                }
                return _EntityWithSimpleColumns;
            }
        }

        List<EntityStateDTO> _EntityStates;
        public List<EntityStateDTO> EntityStates
        {
            get
            {
                if (_EntityStates == null)
                {
                    _EntityStates = AgentUICoreMediator.GetAgentUICoreMediator.entityStateManagerService.GetEntityStates(AreaInitializer.EntityID);

                }
                return _EntityStates;
            }
        }
        I_SearchViewEntityArea _SearchViewEntityArea;
        public I_SearchViewEntityArea SearchViewEntityArea
        {

            get
            {
                if (_SearchViewEntityArea == null)
                    _SearchViewEntityArea = GenerateSearchViewArea();
                return _SearchViewEntityArea;
            }
        }
        List<RelationshipColumnControl> _RelationshipColumnControls;
        public List<RelationshipColumnControl> RelationshipColumnControls
        {
            get
            {
                if (_RelationshipColumnControls == null)
                    _RelationshipColumnControls = new List<RelationshipColumnControl>();
                return _RelationshipColumnControls;
            }
        }

        List<SimpleColumnControl> _SimpleColumnControls;
        public List<SimpleColumnControl> SimpleColumnControls
        {
            get
            {
                if (_SimpleColumnControls == null)
                    _SimpleColumnControls = new List<SimpleColumnControl>();
                return _SimpleColumnControls;
            }
        }
        TableDrivedEntityDTO _FullEntity;
        //public List<RelationshipDTO> ValidRelationships = new List<RelationshipDTO>();
        //public List<ColumnDTO> ValidColumns = new List<ColumnDTO>();
        public TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                {
                    _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetFullEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                    //var item = AgentHelper.GetValidColumnsAndRelationships(_FullEntity, Permission);
                }
                //foreach (var rel in _FullEntity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.SubToSuper && (x as SubToSuperRelationshipDTO).ISARelationship.InternalTable).ToList())
                //{
                //    var pItem
                //}
                //روابطی که فعال نباشند اصلا در لیست روابط موجودیت نمی آیند

                //_FullEntity.Relationships.Clear();
                // _FullEntity.Relationships.RemoveAll(x => x.ID != 23 && x.ID != 25 && x.ID != 24 && x.ID != 26);
                //_FullEntity.Relationships.RemoveAll(x => x.ID != 15 && x.ID != 16);
                return _FullEntity;
            }
        }

        TableDrivedEntityDTO _DataEntryEntity;
        public TableDrivedEntityDTO DataEntryEntity
        {
            get
            {
                if (_DataEntryEntity == null)
                {
                    _DataEntryEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetDataEntryEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                }
                return _DataEntryEntity;
            }
        }

        TableDrivedEntityDTO _SimpleEntity;
        public TableDrivedEntityDTO SimpleEntity
        {
            get
            {
                return _SimpleEntity;
            }
            set
            {
                _SimpleEntity = value;
            }
        }

        bool UICompositionsCalled;
        EntityUICompositionCompositeDTO _UICompositions;
        public EntityUICompositionCompositeDTO UICompositions
        {
            get
            {
                if (_UICompositions == null && !UICompositionsCalled)
                {
                    UICompositionsCalled = true;
                    //if (AreaInitializer.Preview)
                    //    _UICompositions = AreaInitializer.PreviewUICompositionItems;
                    //else
                    _UICompositions = AgentUICoreMediator.GetAgentUICoreMediator.entityUICompositionService.GetEntityUICompositionTree(AreaInitializer.EntityID);
                }
                return _UICompositions;
            }
            set
            {
                _UICompositions = value;
            }
        }
        public RelationshipUISettingDTO GetRelationshipUISetting(RelationshipDTO relationship, bool isTemporaryView)
        {
            RelationshipUISettingDTO setting;
            if (UICompositions != null && UICompositions.RelationshipItems != null
            && UICompositions.RelationshipItems.Any(x => x.RelationshipID == relationship.ID))
            {
                setting = UICompositions.RelationshipItems.First(x => x.RelationshipID == relationship.ID);
            }
            else
            {
                setting = new RelationshipUISettingDTO();
                if (isTemporaryView)
                {
                    setting.UIColumnsType = Enum_UIColumnsType.Normal;
                }
                else
                {
                    setting.Expander = true;
                    setting.UIColumnsType = Enum_UIColumnsType.Full;
                }
                UICompositions.RelationshipItems.Add(setting);
            }
            return setting;
        }

        private ColumnUISettingDTO GetColumnUISetting(ColumnDTO column)
        {
            ColumnUISettingDTO setting;
            if (UICompositions != null && UICompositions.ColumnItems != null
                && UICompositions.ColumnItems.Any(x => x.ColumnID == column.ID))
            {
                setting = UICompositions.ColumnItems.First(x => x.ColumnID == column.ID);
            }
            else
            {
                setting = new ColumnUISettingDTO();
                setting.UIColumnsType = Enum_UIColumnsType.Normal;
                setting.UIRowsCount = 1;
                setting.ColumnID = column.ID;
                UICompositions.ColumnItems.Add(setting);
            }
            return setting;
        }
        AssignedPermissionDTO _Permission;
        public AssignedPermissionDTO Permission
        {
            get
            {
                if (_Permission == null)
                    _Permission = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID, true);
                return _Permission;
            }
        }
        public ObservableCollection<DP_DataRepository> GetDataList()
        {
            if (AreaInitializer.SourceRelation == null)
            {
                return AreaInitializer.Datas;
            }
            else
            {
                if (ChildRelationshipInfo != null)
                    return ChildRelationshipInfo.RelatedData;
                else return new ObservableCollection<DP_DataRepository>();
            }
        }
        TemporaryLinkState _TemporaryLinkState;
        public TemporaryLinkState TemporaryLinkState
        {
            get
            {
                if (_TemporaryLinkState == null)
                    _TemporaryLinkState = new TemporaryLinkState();
                return _TemporaryLinkState;
            }
        }
        public EntityUISettingDTO GetEntityUISetting()
        {
            if (UICompositions != null && UICompositions.RootItem != null && UICompositions.RootItem.EntityUISetting != null)
            {
                var entityUISetting = UICompositions.RootItem.EntityUISetting;
                return entityUISetting;
            }
            else
            {
                var setting = new EntityUISettingDTO();
                setting.UIColumnsCount = 4;
                return setting;
            }
        }



        public void SetAreaInitializer(EditEntityAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            DetermineInteractionMode();
            // ManageSecurity();
            GenerateUIifNeeded();
            if (AreaInitializer.SourceRelation == null)
            {
                AreaInitializer.Datas.CollectionChanged += Datas_CollectionChanged;
            }
        }
        private void DetermineInteractionMode()
        {
            if (AreaInitializer.IntracionMode == IntracionMode.None)
            {
                AreaInitializer.IntracionMode = CommonDefinitions.UISettings.IntracionMode.CreateSelectDirect;
            }
        }
        //public void ManageSecurity()
        //{
        //    if (SimpleEntity.IsReadonly)
        //        AreaInitializer.SecurityReadOnly = true;
        //    //اگر فرم حالت ادیت داشت چک شود
        //    if (Permission.GrantedActions.Any(x => x == SecurityAction.NoAccess ||
        //       x == SecurityAction.ReadOnly || x == SecurityAction.EditAndDelete ||
        //       x == SecurityAction.Edit))
        //    {
        //        if (Permission.GrantedActions.Any(x => x == SecurityAction.NoAccess))
        //            AreaInitializer.SecurityNoAccess = true;
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.ReadOnly))
        //        {
        //            AreaInitializer.SecurityReadOnly = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.Edit))
        //        {
        //            AreaInitializer.SecurityEditOnly = true;
        //        }
        //        else
        //        {
        //            AreaInitializer.SecurityEditAndDelete = true;
        //        }
        //    }
        //    else
        //        AreaInitializer.SecurityNoAccess = true;

        //    if (AreaInitializer.SourceRelation != null)
        //    {
        //        if (AreaInitializer.SourceRelation.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
        //        {
        //            if (AreaInitializer.SecurityReadOnly)
        //            {
        //                //اینجا بیخوده چون ریدونلی بالاتر از ریدونلی بای پرنته
        //                AreaInitializer.SecurityReadOnlyByParent = true;
        //            }
        //            else
        //            {
        //                var rel = AreaInitializer.SourceRelation.Relationship;
        //                foreach (var relCol in rel.RelationshipColumns)
        //                {
        //                    if (relCol.SecondSideColumn.IsReadonly)
        //                        AreaInitializer.SecurityReadOnlyByParent = true;
        //                    if (Permission.ChildsPermissions.Any(x => x.SecurityObjectID == relCol.SecondSideColumnID && x.GrantedActions.Any(y => y == SecurityAction.ReadOnly)))
        //                        AreaInitializer.SecurityReadOnlyByParent = true;
        //                }
        //            }
        //        }
        //    }

        //    //ImposeCommandSecurity();
        //}

        public void GenerateUIifNeeded()
        {

            if (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                 AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
            {
                GenerateDataView();
            }
            if (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
           AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect ||
            AreaInitializer.IntracionMode == IntracionMode.Select)
            {
                GenerateTempView();
            }
        }

        private void GenerateTempView()
        {
            TemporaryLinkState.info = true;
            TemporaryLinkState.clear = true;
            if (AreaInitializer.IntracionMode == IntracionMode.Select || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                TemporaryLinkState.searchView = true;
                TemporaryLinkState.popup = true;
                if (!SimpleEntity.SearchInitially && !(AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.SearchInitially))
                    TemporaryLinkState.quickSearch = true;
            }
            if (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                TemporaryLinkState.edit = true;
            }
            if (AreaInitializer.SourceRelation == null || !(AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaMultipleData))
            {
                TemporaryDisplayView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(TemporaryLinkState);
                if (TemporaryLinkState.quickSearch)
                {
                    TemporaryDisplayView.SearchTextChanged += TemporaryDisplayView_TextChanged;
                }
                if (TemporaryLinkState.quickSearch)
                    TemporaryDisplayView.FocusLost += TemporaryDisplayView_FocusLost;
                TemporaryDisplayView.TemporaryDisplayViewRequested += TemporaryDisplayView_TemporaryDisplayViewRequested;


            }

            DecideTempViewStaticButtons();
        }


        void TemporaryDisplayView_TemporaryDisplayViewRequested(object sender, Arg_TemporaryDisplayViewRequested e)
        {
            TemporaryViewActionRequestedInternal(sender as IAG_View_TemporaryView, e.LinkType);
        }
        private void TemporaryDisplayView_FocusLost(object sender, EventArgs e)
        {
            (sender as IAG_View_TemporaryView).PopupVisibility = false;
        }
        private void TemporaryDisplayView_TextChanged(object sender, Arg_TemporaryDisplaySerachText e)
        {
            TemporaryViewSearchTextChanged(sender as IAG_View_TemporaryView, e);
        }


        public void GenerateDataView()
        {
            //بعدا بررسی شود احتمالا بیخود
            //if (DataView != null)
            //    ClearDataViewControls();

            if (this is I_EditEntityAreaOneData)
                DataView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateEditEntityAreaOneDataView(GetEntityUISetting());
            else if (this is I_EditEntityAreaMultipleData)
                DataView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateEditEntityAreaMultipleDataView();

            ManageDataView();

            if (AreaInitializer.Preview)
                return;
            GenerateCommands();
            //////ImposeDataViewSecurity();
        }
        public I_View_DataContainer DataView
        {
            get; set;
        }


        private void ClearDataViewControls()
        {
            //ظاهرا بیخوده چون با صدا زدن جنریت دیتاویو همه چی از ازول ساخته میشه؟؟؟
            //برای فرم چینش داخل فرم که هر بار فرم ساخته نمیشود و فقط کنترلها تولید میشوند
            DataView.ClearControls();

            foreach (var item in SimpleColumnControls)
            {
                item.Visited = false;
                //     DataView.RemoveUIControlPackage(item.ControlManager);
            }
            foreach (var item in RelationshipColumnControls)
            {
                item.Visited = false;
                //   DataView.RemoveView(item.ControlManager);
            }
        }
        public void ManageDataView()
        {
            if (FullEntity != null)
            {
                //یبار بگیره که ولید کنترلها ساخته بشن
            }
            SimpleColumnControls.Clear();
            RelationshipColumnControls.Clear();
            List<BaseColumnControl> sortedListOfColumnControls = new List<EntityArea.BaseColumnControl>();
            foreach (var column in DataEntryEntity.Columns.OrderBy(x => x.Position))
            {

                if (DataEntryEntity.Relationships.Any(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
                {
                    var relationship = DataEntryEntity.Relationships.First(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
                    AddRelationshipControl(relationship, sortedListOfColumnControls);
                }
                else
                {
                    var propertyControl = new SimpleColumnControl() { Column = column };

                    //propertyControl.ControlPackage = new UIControlPackageForSimpleColumn();
                    bool hasRangeOfValues = column.ColumnValueRange != null && column.ColumnValueRange.Details.Any();
                    bool valueIsTitleOrValue = false;
                    if (hasRangeOfValues)
                        valueIsTitleOrValue = column.ColumnValueRange.ValueFromTitleOrValue;

                    AgentHelper.SetPropertyTitle(propertyControl);

                    if (this is I_EditEntityAreaOneData)
                        propertyControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSimpleControlManagerForOneDataForm(column, GetColumnUISetting(column), hasRangeOfValues, valueIsTitleOrValue, null, true, propertyControl.Alias);
                    else if (this is I_EditEntityAreaMultipleData)
                        propertyControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSimpleControlManagerForMultipleDataForm(column, GetColumnUISetting(column), hasRangeOfValues, valueIsTitleOrValue, true, propertyControl.Alias);

                    var info = column.ID + "," + column.Name;
                    propertyControl.AddLabelControlManagerMessage(new BaseMessageItem() { Key = "columninfo", Message = info });


                    DecideSimpleColumnReadony(propertyControl, true);



                    SimpleColumnControls.Add(propertyControl);
                    sortedListOfColumnControls.Add(propertyControl);
                    if (hasRangeOfValues)
                    {
                        SetColumnValueRange(propertyControl, propertyControl.Column.ColumnValueRange.Details);
                    }

                }
            }
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
            //        || AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //    {
            //        //////if (!AreaInitializer.SourceRelation.TargetRelationColumns.Select(x => x.ID).Contains(column.ID))
            //        //////    if (AreaInitializer.SourceRelation.SourceEditArea.FullEntity.Columns.Any(x => x.ID == column.ID))
            //        //////        continue;
            //    }
            //}

            foreach (var relationship in DataEntryEntity.Relationships
                            .Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
            {
                AddRelationshipControl(relationship, sortedListOfColumnControls);
            }

            if (UICompositions != null && UICompositions.RootItem != null)
            {
                if (this is I_EditEntityAreaOneData)
                    (this as I_EditEntityAreaOneData).GenerateUIComposition(null, UICompositions.RootItem.ChildItems);
                else if (this is I_EditEntityAreaMultipleData)
                    (this as I_EditEntityAreaMultipleData).GenerateUIComposition(UICompositions.RootItem.ChildItems);
            }

            foreach (var columnControl in sortedListOfColumnControls.Where(x => x.Visited == false))
            {
                //برای تغییر کرده ها
                if (columnControl.Visited == true)
                    continue;
                if (columnControl is SimpleColumnControl)
                {
                    var simpleColumn = (columnControl as SimpleColumnControl);
                    columnControl.Visited = true;
                    DataView.AddUIControlPackage(simpleColumn.SimpleControlManager, simpleColumn.ControlManager.LabelControlManager);
                }
                else if (columnControl is RelationshipColumnControl)
                {

                    var relationshipControl = (columnControl as RelationshipColumnControl);

                    //if (relationshipControl.EditNdTypeArea.TemporaryDisplayView != null)
                    //{
                    ////بعدا بررسی شود
                    //if (relationshipControl.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
                    //    ManageSuperToSubControl(sortedListOfColumnControls, relationshipControl);

                    //if (relationshipControl.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
                    //    ManageUnionToSubUnionControl(sortedListOfColumnControls, relationshipControl);

                    //if (!relationshipControl.Visited)
                    //{
                    columnControl.Visited = true;
                    DataView.AddView(relationshipControl.ControlManager, relationshipControl.ControlManager.LabelControlManager);
                    //}

                    //}
                }
            }

            ////بعدا بررسی شود
            //foreach (var relationshipControl in RelationshipColumnControls.Where(x => x.Visited == false))
            //{
            //    //اون بالایی مخصوص اوناییه که ویو موقت دارند
            //    //if (relationshipControl.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
            //    //    ManageSuperToSubControl(sortedListOfColumnControls, relationshipControl);

            //    //if (relationshipControl.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
            //    //    ManageUnionToSubUnionControl(sortedListOfColumnControls, relationshipControl);

            //    if (!relationshipControl.Visited)
            //    {
            //        relationshipControl.Visited = true;
            //        (this as I_EditEntityAreaOneData).DataView.AddView(relationshipControl.ControlManager, relationshipControl.ControlManager.LabelControlManager);
            //    }
            //}



            foreach (var columnControl in SimpleColumnControls)
            {
                if (columnControl.Column.IsMandatory)
                {
                    AddMandatoryMessage(columnControl);
                }
            }
            foreach (var columnControl in RelationshipColumnControls)
            {
                if (columnControl.Relationship.IsOtherSideMandatory)
                    AddMandatoryMessage(columnControl);
            }
            if ((AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship is SubToSuperRelationshipDTO && (AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO).ISARelationship.InternalTable)
            || RelationshipColumnControls.Any(x => x.Relationship is SuperToSubRelationshipDTO && (x.Relationship as SuperToSubRelationshipDTO).ISARelationship.InternalTable))
            {
                AddDeterminerStates();
            }
            //برای برعکسش فکر بشه//یعنی اگر زیر مجموعه وضعیتش تغییر کنه و رابطه پدر مخفی شود مثلا

            //foreach (var columnControl in SimpleColumnControls)
            //{
            //    //فقط ستونهای ساده اضافه میشوند
            //    ColumnAttributes columnAtrributes = new ColumnAttributes();
            //    columnAtrributes.ColumnID = columnControl.Column.ID;
            //    columnAtrributes.ColumnControl = columnControl;
            //    AreaInitializer.FormAttributes.ColumnAttributes.Add(columnAtrributes);
            //}
            if (DataViewGenerated != null)
                DataViewGenerated(this, null);
            AreaInitializer.FormComposed = true;

        }

        private bool DecideSimpleColumnReadony(SimpleColumnControl propertyControl, bool impose)
        {
            bool isReadonly = false;
            if (propertyControl.Column.IsIdentity == true)
                isReadonly = true;
            if (propertyControl.Column.DataEntryEnabled == false)
                isReadonly = true;
            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.SourceRelation.RelationshipColumns.Any(x => x.SecondSideColumnID == propertyControl.Column.ID))
                {
                    if (AreaInitializer.SourceRelation.MasterRelationshipType == Enum_MasterRelationshipType.FromPrimartyToForeign)
                        isReadonly = true;
                }
            }
            if (propertyControl.Column.IsReadonly)
            {
                isReadonly = true;
            }
            propertyControl.IsReadonly = isReadonly;
            if (isReadonly && impose)
            {
                if (DataView != null)
                {
                    propertyControl.SimpleControlManager.SetReadonly(isReadonly);
                }
            }
            return isReadonly;
        }

        private void AddRelationshipControl(RelationshipDTO relationship, List<BaseColumnControl> sortedListOfColumnControls)
        {
            RelationshipColumnControl propertyControl = new RelationshipColumnControl();
            propertyControl.Relationship = relationship;
            AgentHelper.SetPropertyTitle(propertyControl);

            foreach (var relcolumn in relationship.RelationshipColumns)
            {
                propertyControl.Columns.Add(relcolumn.FirstSideColumn);
            }
            var relatedTuple = GenerateEditEntityAreaWithUIControlPackage(propertyControl);
            if (relatedTuple != null)
            {


                // relatedTuple.Item2.AddMessage(new BaseMessageItem() { IsPermanentMessage = true, Key = "relationshipInfo", Message = relatedTuple.Item1.AreaInitializer.SourceRelation.Relationship.Info });
                //  propertyControl = new RelationshipColumnControl();
                propertyControl.EditNdTypeArea = relatedTuple.Item1;
                propertyControl.ControlManager = relatedTuple.Item2;
                RelationshipColumnControls.Add(propertyControl);
                sortedListOfColumnControls.Add(propertyControl);

                //برای ادمین فعال شود بد نیست
                var info = relationship.ID + "," + relationship.Name + Environment.NewLine + relationship.TypeStr + Environment.NewLine + relationship.Info;
                propertyControl.AddLabelControlManagerMessage(new BaseMessageItem() { Key = "relationshipinfo", Message = info });
                //    AddRelationshipColumnMessageItem(propertyControl, info, InfoColor.Black, "permanentCaption", null, true);

            }
        }
        public List<RelationshipColumnControl> SkippedRelationshipColumnControl { set; get; }
        private Tuple<I_EditEntityArea, I_RelationshipControlManager> GenerateEditEntityAreaWithUIControlPackage(RelationshipColumnControl relationshipColumnControl)
        {
            if (AreaInitializer.SourceRelation != null)
                if (IsReverseRelation(AreaInitializer.SourceRelation.Relationship, relationshipColumnControl.Relationship))
                    return null;
            if (SkipableRelationship(relationshipColumnControl))
            {
                if (SkippedRelationshipColumnControl == null)
                    SkippedRelationshipColumnControl = new List<RelationshipColumnControl>();
                SkippedRelationshipColumnControl.Add(relationshipColumnControl);
                return null;
            }

            var newAreaInitializer = GenereateAreaInitializer(relationshipColumnControl);
            if (newAreaInitializer != null)
            {
                var editAreaResult = EditEntityAreaConstructor.GetEditEntityArea(newAreaInitializer);
                if (editAreaResult.Item1 != null)
                {
                    var editArea = editAreaResult.Item1;
                    editArea.SetAreaInitializer(newAreaInitializer);
                    I_RelationshipControlManager controlManager = null;//= uiPackage = new UIControlPackageForRelationshipColumn();
                    if (this is I_EditEntityAreaOneData)
                    {
                        if (editArea.TemporaryDisplayView != null)
                        {

                            controlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateRelationshipControlManagerForOneDataForm(editArea.TemporaryDisplayView, GetRelationshipUISetting(relationshipColumnControl.Relationship, true), true, relationshipColumnControl.Alias);
                        }
                        else if (editArea.DataView != null)
                        {
                            //  uiPackage = new UIControlPackageForRelationshipColumn();
                            controlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateRelationshipControlManagerForOneDataForm(editArea.DataView, GetRelationshipUISetting(relationshipColumnControl.Relationship, false), true, relationshipColumnControl.Alias);
                        }
                    }
                    else if (this is I_EditEntityAreaMultipleData)
                    {
                        controlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateRelationshipControlManagerForMultipleDataForm(editArea.TemporaryLinkState, GetRelationshipUISetting(relationshipColumnControl.Relationship, true), true, relationshipColumnControl.Alias);
                        controlManager.TemporaryViewRequested += (sender, e) => View_TemporaryViewRequested(sender, e, editArea);
                        controlManager.TemporaryViewLoaded += (sender, e) => View_TemporaryViewLoaded(sender, e, relationshipColumnControl as RelationshipColumnControl);
                        controlManager.TemporaryViewSerchTextChanged += (sender, e) => View_TemporaryViewSearchText(sender, e, editArea);
                        controlManager.FocusLost += ControlManager_FocusLost;

                    }
                    if (controlManager != null)
                    {
                        if (RelationshipAreaGenerated != null)
                            RelationshipAreaGenerated(this, new EditAreaGeneratedArg() { GeneratedEditArea = editArea });
                        return new Tuple<I_EditEntityArea, I_RelationshipControlManager>(editArea, controlManager);
                    }
                }
            }
            return null;
        }

        private bool SkipableRelationship(RelationshipColumnControl relationshipColumnControl)
        {
            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.SourceRelation.Relationship.IsOtherSideDirectlyCreatable)
                {
                    if (SimpleEntity.IndependentDataEntry == true)
                    {
                        if (relationshipColumnControl.Relationship.IsOtherSideDirectlyCreatable)
                        {
                            if (AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub
                                || AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne
                                || AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
                                if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                                {
                                    if (!relationshipColumnControl.Relationship.IsOtherSideMandatory)
                                        return true;
                                }
                            //روابط اسکیپ شده لیست شوند
                            //    بعدش کاستومر با پرسون
                        }
                    }
                }
                if (AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.SubToSuper)
                {
                    var parentIsaRelationship = (AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO).ISARelationship;
                    if (parentIsaRelationship.IsDisjoint)
                    {
                        if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
                        {
                            var isaRelationship = (relationshipColumnControl.Relationship as SuperToSubRelationshipDTO).ISARelationship;
                            if (isaRelationship.ID == parentIsaRelationship.ID)
                                return true;
                        }
                    }
                }
                if (AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
                {
                    var parentUnionRelationship = (AreaInitializer.SourceRelation.Relationship as SubUnionToSuperUnionRelationshipDTO).UnionRelationship;
                    if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
                    {
                        var unionRelationship = (relationshipColumnControl.Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship;
                        if (unionRelationship.ID == parentUnionRelationship.ID)
                            return true;
                    }

                }
            }
            return false;
        }

        private void ControlManager_FocusLost(object sender, EventArgs e)
        {
            (sender as IAG_View_TemporaryView).PopupVisibility = false;
        }
        private void View_TemporaryViewSearchText(object sender, Arg_TemporaryDisplaySerachText e, I_EditEntityArea editEntityArea)
        {
            if (editEntityArea is I_EditEntityAreaOneData)
                (editEntityArea as I_EditEntityAreaOneData).TemporaryViewSearchTextChanged(sender as IAG_View_TemporaryView, e);
        }
        private void View_TemporaryViewRequested(object sender, Arg_MultipleTemporaryDisplayViewRequested e, I_EditEntityArea editEntityArea)
        {
            editEntityArea.TemporaryViewActionRequestedFromMultipleEditor(sender as IAG_View_TemporaryView, e.LinkType, editEntityArea.AreaInitializer.SourceRelation.Relationship, e.DataItem as DP_DataRepository);
        }
        private void View_TemporaryViewLoaded(object sender, Arg_MultipleTemporaryDisplayLoaded e, RelationshipColumnControl relationshipColumnControl)
        {
            //////if (relationshipColumnControl.EditNdTypeArea.AreaInitializer.SecurityReadOnlyByParent)
            //////{
            //////    relationshipColumnControl.RelationshipControlManager.EnableDisable(e.DataItem, TemporaryLinkType.SerachView, false);
            //////    relationshipColumnControl.RelationshipControlManager.EnableDisable(e.DataItem, TemporaryLinkType.Clear, false);
            //////}
        }
        private static bool IsReverseRelation(RelationshipDTO relationship1, RelationshipDTO relationship2)
        {
            if ((relationship1.PairRelationshipID == relationship2.ID) || (relationship2.PairRelationshipID == relationship1.ID))
                return true;
            return false;
        }
        private EditEntityAreaInitializer GenereateAreaInitializer(RelationshipColumnControl relationshipColumnControl)
        {
            EditEntityAreaInitializer newAreaInitializer = new EditEntityAreaInitializer();
            newAreaInitializer.EditAreaDataManager = AreaInitializer.EditAreaDataManager;
            newAreaInitializer.ActionActivityManager = AreaInitializer.ActionActivityManager;
            newAreaInitializer.Preview = AreaInitializer.Preview;
            newAreaInitializer.EntityID = relationshipColumnControl.Relationship.EntityID2;

            newAreaInitializer.SourceRelation = new EditAreaRelationSource();
            newAreaInitializer.SourceRelation.SourceEditArea = this;
            newAreaInitializer.SourceRelation.SourceRelationshipColumnControl = relationshipColumnControl;
            newAreaInitializer.SourceRelation.SourceEntityID = relationshipColumnControl.Relationship.EntityID1;
            newAreaInitializer.SourceRelation.SourceTableID = relationshipColumnControl.Relationship.TableID1;
            newAreaInitializer.SourceRelation.RelationshipColumns = relationshipColumnControl.Relationship.RelationshipColumns;
            newAreaInitializer.SourceRelation.Relationship = relationshipColumnControl.Relationship;
            newAreaInitializer.SourceRelation.TargetEntityID = relationshipColumnControl.Relationship.EntityID2;
            newAreaInitializer.SourceRelation.TargetTableID = relationshipColumnControl.Relationship.TableID2;
            newAreaInitializer.SourceRelation.TargetSideIsMandatory = relationshipColumnControl.Relationship.IsOtherSideMandatory;


            if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.OneToMany)
            {
                newAreaInitializer.DataMode = DataMode.Multiple;
            }
            else
                newAreaInitializer.DataMode = DataMode.One;

            if (this is I_EditEntityAreaMultipleData)
                relationshipColumnControl.Relationship.IsOtherSideDirectlyCreatable = false;

            if (relationshipColumnControl.Relationship.IsOtherSideCreatable == true)
            {
                if (relationshipColumnControl.Relationship.IsOtherSideDirectlyCreatable == true)
                {
                    if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                        newAreaInitializer.IntracionMode = IntracionMode.CreateDirect;
                    else
                        newAreaInitializer.IntracionMode = IntracionMode.CreateSelectDirect;
                }
                else
                {
                    if (relationshipColumnControl.Relationship.TypeEnum != Enum_RelationshipType.OneToMany)
                        newAreaInitializer.IntracionMode = IntracionMode.CreateSelectInDirect;
                    else
                        newAreaInitializer.IntracionMode = IntracionMode.CreateInDirect;
                }
            }
            else if (relationshipColumnControl.Relationship.IsOtherSideCreatable == false)
            {
                if (relationshipColumnControl.Relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    return null;
                else
                    newAreaInitializer.IntracionMode = IntracionMode.Select;
            }

            if (newAreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
              newAreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                CheckDirectPossiblity(newAreaInitializer);


            //////var relationshipPermission = Permission.ChildsPermissions.FirstOrDefault(x => x.SecurityObjectID == relationshipColumnControl.Relationship.ID);
            //////if (relationshipPermission != null && relationshipPermission.GrantedActions.Any(x => x == SecurityAction.ReadOnly))
            //////{
            //////    newAreaInitializer.SecurityReadOnlyByParent = true;
            //////}
            //////if (relationshipColumnControl.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
            //////{
            //////    if (AreaInitializer.SecurityReadOnly)
            //////    {
            //////        newAreaInitializer.SecurityReadOnlyByParent = true;
            //////    }
            //////    else
            //////    {
            //////        var rel = relationshipColumnControl.Relationship;
            //////        foreach (var relCol in rel.RelationshipColumns)
            //////        {
            //////            if (relCol.FirstSideColumn.IsReadonly)
            //////                newAreaInitializer.SecurityReadOnlyByParent = true;
            //////            if (Permission.ChildsPermissions.Any(x => x.SecurityObjectID == relCol.FirstSideColumnID && x.GrantedActions.Any(y => y == SecurityAction.ReadOnly)))
            //////                newAreaInitializer.SecurityReadOnlyByParent = true;
            //////        }
            //////    }
            //////}
            return newAreaInitializer;

        }
        private void CheckDirectPossiblity(EditEntityAreaInitializer newAreaInitializer)
        {
            if (RelationHistoryContainsEntityID(newAreaInitializer, newAreaInitializer.EntityID))
            {
                if (newAreaInitializer.IntracionMode == IntracionMode.CreateDirect)
                    newAreaInitializer.IntracionMode = IntracionMode.CreateInDirect;
                else if (newAreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                    newAreaInitializer.IntracionMode = IntracionMode.CreateSelectInDirect;

            }
        }
        private static bool RelationHistoryContainsEntityID(EditEntityAreaInitializer areaInitializer, int entityID)
        {
            if (areaInitializer.SourceRelation != null)
            {
                if (areaInitializer.SourceRelation.SourceEditArea.AreaInitializer.IntracionMode != IntracionMode.CreateDirect &&
                    areaInitializer.SourceRelation.SourceEditArea.AreaInitializer.IntracionMode != IntracionMode.CreateSelectDirect)
                    return false;
                else if (areaInitializer.SourceRelation.SourceEditArea.AreaInitializer.EntityID == entityID)
                {
                    return true;
                }
                else
                {
                    return RelationHistoryContainsEntityID(areaInitializer.SourceRelation.SourceEditArea.AreaInitializer, entityID);

                }

            }
            else
                return false;
        }

        //private void ManageSuperToSubControl(List<BaseColumnControl> sortedListOfColumnControls, RelationshipColumnControl relationshipControl)
        //{
        //    if (relationshipControl.Relationship is SuperToSubRelationshipDTO)
        //    {
        //        var mainSuperToSub = (relationshipControl.Relationship as SuperToSubRelationshipDTO);
        //        var listSuperToSubsRelationshipControlList = new List<RelationshipColumnControl>();
        //        foreach (var subRelationship in sortedListOfColumnControls.Where(x => x.Visited == false
        //         && (x is RelationshipColumnControl) && ((x as RelationshipColumnControl).Relationship is SuperToSubRelationshipDTO) &&
        //         ((x as RelationshipColumnControl).Relationship as SuperToSubRelationshipDTO).ISARelationship.ID == mainSuperToSub.ISARelationship.ID))
        //        {
        //            listSuperToSubsRelationshipControlList.Add((subRelationship as RelationshipColumnControl));
        //        }
        //        if (listSuperToSubsRelationshipControlList.Count > 1)
        //        {
        //            var tabgroupSetting = new TabGroupUISettingDTO();
        //            tabgroupSetting.Expander = true;
        //            tabgroupSetting.UIColumnsType = Enum_UIColumnsType.Full;

        //            var tabGroupContainer = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabGroup(tabgroupSetting);
        //            (this as I_EditEntityAreaOneData).SpecializedDataView.AddTabGroup(tabGroupContainer, (listSuperToSubsRelationshipControlList.First().Relationship as SuperToSubRelationshipDTO).ISARelationship.Name, tabgroupSetting);
        //            foreach (var suptoSubRelationshipControl in listSuperToSubsRelationshipControlList)
        //            {
        //                var tabpageSetting = new TabPageUISettingDTO();
        //                tabpageSetting.InternalColumnsCount = GetEntityUISetting().UIColumnsCount;
        //                var groupItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabPage(tabpageSetting);
        //                //هدر چک شود 
        //                tabGroupContainer.AddTabPage(groupItem, suptoSubRelationshipControl.Relationship.Alias, tabpageSetting, false);
        //                groupItem.AddView(suptoSubRelationshipControl.ControlManager, suptoSubRelationshipControl.ControlManager.LabelControlManager);
        //                suptoSubRelationshipControl.Visited = true;
        //            }
        //        }
        //    }
        //}
        //private void ManageUnionToSubUnionControl(List<BaseColumnControl> sortedListOfColumnControls, RelationshipColumnControl relationshipControl)
        //{
        //    if (relationshipControl.Relationship is SuperUnionToSubUnionRelationshipDTO)
        //    {
        //        var mainSuperToSub = (relationshipControl.Relationship as SuperUnionToSubUnionRelationshipDTO);
        //        var listSuperToSubsRelationshipControlList = new List<RelationshipColumnControl>();
        //        foreach (var subRelationship in sortedListOfColumnControls.Where(x => x.Visited == false
        //         && (x is RelationshipColumnControl) && ((x as RelationshipColumnControl).Relationship is SuperUnionToSubUnionRelationshipDTO) &&
        //         ((x as RelationshipColumnControl).Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship.ID == mainSuperToSub.UnionRelationship.ID))
        //        {
        //            listSuperToSubsRelationshipControlList.Add((subRelationship as RelationshipColumnControl));
        //        }
        //        if (listSuperToSubsRelationshipControlList.Count > 1)
        //        {
        //            var tabgroupSetting = new TabGroupUISettingDTO();
        //            tabgroupSetting.Expander = true;
        //            tabgroupSetting.UIColumnsType = Enum_UIColumnsType.Full;

        //            var tabGroupContainer = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabGroup(tabgroupSetting);
        //            (this as I_EditEntityAreaOneData).SpecializedDataView.AddTabGroup(tabGroupContainer, (listSuperToSubsRelationshipControlList.First().Relationship as SuperUnionToSubUnionRelationshipDTO).UnionRelationship.Name, tabgroupSetting);
        //            foreach (var suptoSubRelationshipControl in listSuperToSubsRelationshipControlList)
        //            {
        //                var tabpageSetting = new TabPageUISettingDTO();
        //                tabpageSetting.InternalColumnsCount = GetEntityUISetting().UIColumnsCount;
        //                var groupItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabPage(tabpageSetting);
        //                tabGroupContainer.AddTabPage(groupItem, suptoSubRelationshipControl.Relationship.Alias, tabpageSetting, false);
        //                groupItem.AddView(suptoSubRelationshipControl.ControlManager, suptoSubRelationshipControl.ControlManager.LabelControlManager);
        //                suptoSubRelationshipControl.Visited = true;
        //            }
        //        }
        //    }
        //}
        private void AddDeterminerStates()
        {
            //بهتره اضافه بشه به یو ای اکتیویتی منیجر و نسبت به دیتا حساس باشه
            List<Tuple<RelationshipDTO, I_EditEntityArea>> items = new List<Tuple<RelationshipDTO, I_EditEntityArea>>();
            if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship is SubToSuperRelationshipDTO && (AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO).ISARelationship.InternalTable)
                items.Add(new Tuple<RelationshipDTO, I_EditEntityArea>(AreaInitializer.SourceRelation.Relationship, AreaInitializer.SourceRelation.SourceEditArea));
            foreach (var item in RelationshipColumnControls.Where(x => x.Relationship is SuperToSubRelationshipDTO && (x.Relationship as SuperToSubRelationshipDTO).ISARelationship.InternalTable))
            {
                items.Add(new Tuple<RelationshipDTO, I_EditEntityArea>(item.Relationship, item.EditNdTypeArea));

            }
            foreach (var item in items)
            {
                //    var relationship = item.Item1 as SuperToSubRelationshipDTO;

                var entityState = new EntityStateDTO();
                //////entityState.ColumnID = item.Item2.SimpleEntity.DeterminerColumnID;
                //////entityState.Value = item.Item2.SimpleEntity.DeterminerColumnValue;
                entityState.Title = entityState.ColumnID + ">";// + entityState.Value;
                entityState.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                var stateAction = new UIActionActivityDTO();
                stateAction.Title = item.Item1.Name + ">Hidden";
                stateAction.UIEnablityDetails.Add(new UIEnablityDetailsDTO() { RelationshipID = item.Item1.ID, Hidden = true });
                entityState.ActionActivities.Add(stateAction);
                EntityStates.Add(entityState);
            }
            if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship is SubToSuperRelationshipDTO && (AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO).ISARelationship.InternalTable)
            {
                //////if (FullEntity.Columns.Any(x => x.ID == AreaInitializer.SourceRelation.SourceEditArea.FullEntity.DeterminerColumnID))
                //////{
                //////    var determinerColumn = FullEntity.Columns.First(x => x.ID == AreaInitializer.SourceRelation.SourceEditArea.FullEntity.DeterminerColumnID);
                //////    determinerColumn.DefaultValue = AreaInitializer.SourceRelation.SourceEditArea.FullEntity.DeterminerColumnValue;
                //////}
            }
            if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship is SubToSuperRelationshipDTO && (AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO).ISARelationship.InternalTable)
            {
                if (AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.SourceRelation.MasterRelationshipType == Enum_MasterRelationshipType.FromPrimartyToForeign)
                {
                    foreach (var item in SimpleColumnControls.Where(x => AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.SourceRelation.RelationshipColumns.Any(y => y.SecondSideColumnID == x.Column.ID)))
                    {
                        //(item as SimpleColumnControl).BusinessReadOnly[] = true;
                        //(item as SimpleColumnControl).BusinessHidden = true;
                        //(item as SimpleColumnControl).ControlManager.EnableDisable(false);
                    }
                }
            }
        }
        private void GenerateCommands()
        {
            if (Commands == null)
                Commands = new List<I_Command>();

            var clearCommand = new ClearCommand(this);
            Commands.Add(clearCommand);
            DataView.AddCommand(clearCommand.CommandManager, (!(this is I_EditEntityAreaMultipleData) && AreaInitializer.SourceRelation != null));
            //if (this.AreaInitializer.SourceRelation == null && DataEntryEntity.IsReadonly)
            //    clearCommand.CommandManager.SetEnabled(false);
            //if (this.AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly)
            //    clearCommand.CommandManager.SetEnabled(false);

            var infoCommand = new InfoCommand(this);
            Commands.Add(infoCommand);
            DataView.AddCommand(infoCommand.CommandManager, true);

            var logReportCommand = new LogReportCommand(this);
            Commands.Add(logReportCommand);
            DataView.AddCommand(logReportCommand.CommandManager, true);

            if (Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView || x == SecurityAction.ArchiveEdit))
            {
                var archiveCommand = new ArchiveCommand(this);
                Commands.Add(archiveCommand);
                DataView.AddCommand(archiveCommand.CommandManager, true);
            }

            if (Permission.GrantedActions.Any(x => x == SecurityAction.LetterView || x == SecurityAction.LetterEdit))
            {
                var letterCommand = new LetterCommand(this);
                Commands.Add(letterCommand);
                DataView.AddCommand(letterCommand.CommandManager, true);
            }
            if (AreaInitializer.SourceRelation == null)
            {
                var saveCommand = new SaveCommand(this);
                Commands.Add(saveCommand);
                DataView.AddCommand(saveCommand.CommandManager);
                //if (DataEntryEntity.IsReadonly)
                //saveCommand.CommandManager.SetEnabled(false);

                var deleteCommand = new DeleteCommand(this);
                Commands.Add(deleteCommand);
                DataView.AddCommand(deleteCommand.CommandManager);


                //var archivePermission = AgentUICoreMediator.GetSubSystemAssignedPermissions("Archive");
                //if (!archivePermission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
                //{


                //}

                var dataViewCommand = new DataViewCommand(this);
                Commands.Add(dataViewCommand);
                DataView.AddCommand(dataViewCommand.CommandManager, true);

                var gridViewCommand = new GridViewCommand(this);
                Commands.Add(gridViewCommand);
                DataView.AddCommand(gridViewCommand.CommandManager, true);

                var dataLinkCommand = new DataLinkCommand(this);
                Commands.Add(dataLinkCommand);
                DataView.AddCommand(dataLinkCommand.CommandManager, true);

                //var dataListReportCommand = new DataListReportCommand(this);
                //Commands.Add(dataListReportCommand);
                //DataView.AddCommand(dataListReportCommand.CommandManager, true);

            }
            if (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
         || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                var saveCloseCommand = new SaveAndCloseDialogCommand(this);
                Commands.Add(saveCloseCommand);
                DataView.AddCommand(saveCloseCommand.CommandManager);
                //این بعدا چک شود که ضروری است؟


                //var clearAndCloseCommand = new ClearAndClose(this);
                //Commands.Add(clearAndCloseCommand);
                //DataView.AddCommand(clearAndCloseCommand.CommandManager);
            }

            if (AreaInitializer.IntracionMode == IntracionMode.Select
               || AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect
                 || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                var searchCommand = new SearchCommand(this);
                Commands.Add(searchCommand);
                DataView.AddCommand(searchCommand.CommandManager, AreaInitializer.SourceRelation != null);
                //if (this.AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly)
                //    searchCommand.CommandManager.SetEnabled(false);
            }

            if (this is I_EditEntityAreaMultipleData)
            {

                var addCommand = new AddCommand((this as I_EditEntityAreaMultipleData));
                Commands.Add(addCommand);
                DataView.AddCommand(addCommand.CommandManager);

                var removeCommand = new RemoveCommand((this as I_EditEntityAreaMultipleData));
                Commands.Add(removeCommand);
                DataView.AddCommand(removeCommand.CommandManager);

            }


            foreach (var command in EntityCommands)
            {
                var commandNew = new EntityCommand(command, this);// CommandManager.GenerateCommand(commandAttribute); ;
                Commands.Add(commandNew);
                DataView.AddCommand(commandNew.CommandManager, true);
            }

            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect
                  || AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
                {
                    DataView.DeHighlightCommands();
                }
            }

            DecideDataViewStaticButtons();
        }
        //////public void ImposeDataViewSecurity()
        //////{
        //////    if (AreaInitializer.SecurityNoAccess == true)
        //////    {
        //////        if (DataView != null)
        //////            DataView.EnableDisable(false);
        //////        ////اینجا نباید بیاد قاعدتاً
        //////    }
        //////    else
        //////    {
        //////        if (AreaInitializer.SecurityReadOnly == true)
        //////        {
        //////            MakeFormReadOnly();
        //////        }
        //////        else if (AreaInitializer.SecurityEditOnly == true)
        //////        {
        //////            var deleteCommand = Commands.FirstOrDefault(x => x is DeleteCommand);
        //////            if (deleteCommand != null)
        //////                DisableEnableCommand(deleteCommand, false);
        //////        }

        //////        if (AreaInitializer.BusinessReadOnlyByParent || AreaInitializer.SecurityReadOnlyByParent)
        //////        {
        //////            var relChangeCommands = Commands.Where(x => x is ClearCommand || x is AddCommand || x is SearchCommand || x is RemoveCommand).ToList();
        //////            foreach (var command in relChangeCommands)
        //////            {
        //////                DisableEnableCommand(command, false);
        //////            }
        //////        }
        //////    }
        //////    foreach (var relationshipControl in RelationshipColumnControls)
        //////    {
        //////        if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
        //////           || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
        //////        {
        //////            relationshipControl.EditNdTypeArea.ImposeDataViewSecurity();
        //////        }
        //////        else if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
        //////          || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
        //////        {
        //////            relationshipControl.EditNdTypeArea.ImposeTemporaryViewSecurity();
        //////        }

        //////    }
        //////}
        //////public void ImposeTemporaryViewSecurity()
        //////{
        //////    if (AreaInitializer.SecurityNoAccess == true)
        //////    {
        //////        TemporaryDisplayView.DisableEnable(false);
        //////    }
        //////    else
        //////    {
        //////        if (AreaInitializer.BusinessReadOnlyByParent || AreaInitializer.SecurityReadOnlyByParent)
        //////        {
        //////            TemporaryDisplayView.DisableEnable(TemporaryLinkType.SerachView, false);
        //////            TemporaryDisplayView.DisableEnable(TemporaryLinkType.Clear, false);
        //////        }
        //////    }
        //////}
        public UpdateResult UpdateData()
        {
            //SetDataShouldBeCounted();
            UpdateResult result = new UpdateResult();
            var dataList = GetDataList().ToList();

            //if (editEntityArea == this)
            //{
            CheckRedundantData(this);
            AreaInitializer.UIFomulaManager.UpdateFromulas();
            //if (!formulaResult)
            //{
            //    result.IsValid = false;
            //    result.Message = "نتیجه فرمولهای محاسبه شده مورد تایید نمی باشد";
            //}
            //else
            //{
            bool validationResult = AreaInitializer.UIValidationManager.ValidateData();
            if (!validationResult)
            {
                result.IsValid = false;
                result.Message = "اعتبارسنجی: مقادیر وارد شده در برخی از فرمها معتبر نمی باشند";
            }
            else
                result.IsValid = true;
            //}
            //}
            //باید اینجا شوال پرسده شود که اصلا کاربر میخواد با فرمولهای خطادار ادامه دهد



            return result;
        }

        public void CheckRedundantData(I_EditEntityArea editEntityArea)
        {
            var dataList = GetDataList();
            //if (dataList == null)
            //    return
            foreach (var item in dataList)
                item.ShouldBeSkipped = DataShouldBeSkipped(item);
            //if (AreaShouldBeReviewed())
            //{
            //foreach (var item in dataList)
            //    item.ShouldBeSkipped = false;
            foreach (var relationshipControl in RelationshipColumnControls)
            {
                if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
                   || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                {
                    relationshipControl.EditNdTypeArea.CheckRedundantData(editEntityArea);
                }

            }
            //}
            //else
            //{
            //    foreach (var item in dataList)
            //        item.ShouldBeSkipped = true;

            //    //به نظر میاد اینجا بیخوده
            //    //foreach (var relationshipControl in RelationshipColumnControls)
            //    //{
            //    //    if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
            //    //       || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
            //    //    {
            //    //        relationshipControl.EditNdTypeArea.CheckRedundantData(editEntityArea);
            //    //    }

            //    //}
            //}
        }

        private bool DataShouldBeSkipped(DP_DataRepository dataItem)
        {
            if (this is I_EditEntityAreaMultipleData)
                return false;
            else
            {
                if (DataIsNewInOneEditAreaAndReadonly(dataItem))
                    return true;
                else
                    return !AgentHelper.DataOrRelatedChildDataHasValue(dataItem, null);
            }
        }

        public bool DataIsNewInOneEditAreaAndReadonly(DP_DataRepository dataItem)
        {
            if (this is I_EditEntityAreaOneData && dataItem.IsNewItem &&
                (DataEntryEntity.IsReadonly
                || (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly))
                )
                return true;
            else
                return false;

        }

        private bool AreaShouldBeReviewed()
        {
            //if (this == editEntityArea)
            //    return true;
            var data = GetDataList();
            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                {
                    if (data.Any())
                        return true;

                }
                else
                {

                    if (AreaInitializer.SourceRelation.Relationship.IsReadonly && data.Any(x => x.IsNewItem))
                        return false;
                    else
                        return data.Any(x => AgentHelper.DataOrRelatedChildDataHasValue(x, AreaInitializer.SourceRelation.Relationship));
                }
            }
            else
            {
                if (DataEntryEntity.IsReadonly && data.Any(x => x.IsNewItem))
                    return false;

            }

            return false;

        }




        //public void SetDataShouldBeCounted()
        //{
        //    List<DP_DataRepository> datalist = null;
        //    if (AreaInitializer.SourceRelation == null)
        //    {
        //        datalist = AreaInitializer.Datas.ToList();
        //    }
        //    else
        //    {
        //        datalist = ChildRelationshipInfo.GetRelatedData(AreaInitializer.SourceRelation.Relationship.ID);
        //    }
        //    foreach (var data in datalist)
        //    {
        //        SetDataShouldBeCounted(data);
        //    }
        //}
        //private void SetDataShouldBeCounted(DP_DataRepository data)
        //{
        //    data.HasDirectData = true;
        //    foreach (var relationshipControl in RelationshipColumnControls)
        //    {
        //        relationshipControl.EditNdTypeArea.SetChildRelationshipInfo(data.ChildRelationshipInfos.First(x => x.Relationship == relationshipControl.Relationship));
        //        if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
        //           || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
        //        {
        //            relationshipControl.EditNdTypeArea.SetDataShouldBeCounted();
        //        }
        //    }
        //}



        private void AddMandatoryMessage(BaseColumnControl baseColumnControl)
        {
            var text = baseColumnControl.ControlManager.LabelControlManager.Text;
            if (!text.StartsWith("*"))
            {
                text = "* " + text;
                baseColumnControl.ControlManager.LabelControlManager.Text = text;
            }
            baseColumnControl.AddLabelControlManagerColor(new BaseColorItem() { Key = "mandatory", Color = InfoColor.DarkRed, ColorTarget = ControlColorTarget.Foreground });
        }

        public void AddDataBusinessMessage(string message, InfoColor infoColor, string key, DP_DataRepository causingData, ControlItemPriority priority)
        {
            DataMessageItem baseMessageItem = new DataMessageItem();
            baseMessageItem.CausingDataItem = causingData;
            baseMessageItem.Key = key;
            baseMessageItem.Message = message;
            baseMessageItem.Priority = priority;
            AddControlManagerMessage(baseMessageItem);

            DataColorItem baseColorItem = new DataColorItem();
            baseColorItem.Key = key;
            baseColorItem.Color = infoColor;
            baseColorItem.ColorTarget = ControlColorTarget.Border;
            baseColorItem.Priority = priority;
            baseColorItem.CausingDataItem = causingData;
            AddControlManagerColor(baseColorItem); ;

            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("اعتبارسنجی", message, infoColor);

        }
        public void RemoveDataBusinessMessage(DP_DataRepository dataItem, string key)
        {
            RemoveControlManagerMessage(dataItem, key);
            RemoveControlManagerColor(dataItem, key);
        }

        //public void RemoveMessageItem(BaseMessageItem baseMessageItem)
        //{
        //    MessageItems.Remove(baseMessageItem);
        //    if (baseMessageItem.ColumnControl != null)
        //    {
        //        if (baseMessageItem.ColumnControl is SimpleColumnControl)
        //        {
        //            (baseMessageItem.ColumnControl as SimpleColumnControl).RemoveMessage(baseMessageItem);
        //        }
        //        else if (baseMessageItem.ColumnControl is RelationshipColumnControl)
        //        {
        //            (baseMessageItem.ColumnControl as RelationshipColumnControl).RemoveMessage(baseMessageItem);
        //        }
        //    }
        //}
        //public void RemoveMessagesItems(string key)
        //{
        //    foreach (var baseMessageItem in MessageItems.Where(x => x.Key == key).ToList())
        //    {
        //        RemoveMessageItem(baseMessageItem);
        //    }
        //}
        //public void RemoveMessagesItems(DP_DataRepository dataItem, string key)
        //{
        //    foreach (var baseMessageItem in MessageItems.Where(x => x.CausingDataItem == dataItem && x.Key == key).ToList())
        //    {
        //        RemoveMessageItem(baseMessageItem);
        //    }
        //}

        //SimpleColumnControl LastFormulaColumnControl1 { set; get; }


        public void TemporaryViewActionRequestedFromMultipleEditor(IAG_View_TemporaryView TemporaryView, TemporaryLinkType linkType, RelationshipDTO relationship, DP_DataRepository parentData)
        {
            SetChildRelationshipInfo(parentData.ChildRelationshipInfos.First(x => x.Relationship == relationship));
            TemporaryViewActionRequestedInternal(TemporaryView, linkType);
        }
        //public void ReadonlySimpleColumnControl(SimpleColumnControl column, bool readonlity)
        //{

        //}
        public void DisableEnableCommand(I_Command command, bool enabled)
        {
            command.CommandManager.SetEnabled(enabled);
        }
        //public void MakeFormReadOnly()
        //{
        //    List<I_Command> editCommands = Commands.Where(x => x is SaveCommand || x is SaveAndCloseDialogCommand || x is AddCommand).ToList();
        //    if (AreaInitializer.SourceRelation == null)
        //    {

        //    }
        //    else
        //    {
        //        if (AreaInitializer.SourceRelation.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
        //        {
        //            var otherCommands = Commands.Where(x => x is ClearCommand || x is SearchCommand);
        //            editCommands.AddRange(otherCommands);
        //        }
        //        else
        //        {

        //        }
        //    }

        //    if (AreaInitializer.SourceRelation != null)
        //    {
        //        var otherCommands = Commands.Where(x => x is ClearCommand || x is RemoveCommand);
        //        editCommands.AddRange(otherCommands);
        //    }

        //    foreach (var command in editCommands)
        //    {
        //        DisableEnableCommand(command, false);
        //    }

        //    foreach (var col in SimpleColumnControls)
        //    {
        //        ReadonlyDataItemColumn(col, true);
        //    }
        //    //foreach (var rel in RelationshipColumnControls.Where(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary))
        //    //{
        //    //    rel.EditNdTypeArea.AreaInitializer.SecurityReadOnlyByParent = true;
        //    //}
        //}
        public void TemporaryViewSearchTextChanged(IAG_View_TemporaryView view, Arg_TemporaryDisplaySerachText searchArg)
        {
            if (!string.IsNullOrEmpty(searchArg.Text))
            {
                SearchViewEntityArea.SearchTextBox(searchArg.Text);
                SearchViewEntityArea.RemoveViewEntityAreaView();
                if (!view.HasPopupView)
                    view.AddPopupView(SearchViewEntityArea.ViewEntityArea.ViewView);

                view.PopupVisibility = true;
            }
        }
        public void TemporaryViewActionRequestedInternal(IAG_View_TemporaryView TemporaryView, TemporaryLinkType linkType)
        {
            if (LastTemporaryView != null)
            {
                if (LastTemporaryView.HasPopupView)
                    LastTemporaryView.RemovePopupView(SearchViewEntityArea.ViewEntityArea.ViewView);
            }
            LastTemporaryView = TemporaryView;
            if (linkType == TemporaryLinkType.DataView)
            {
                ShowTemporaryDataView();
            }
            else if (linkType == TemporaryLinkType.SerachView)
            {
                //if (AreaInitializer.SourceRelation != null)
                //    AreaInitializer.SourceRelation.RelatedData = parentData;
                ShowTemporarySearchView(false);
            }
            else if (linkType == TemporaryLinkType.QuickSearch)
            {
                //if (AreaInitializer.SourceRelation != null)
                //    AreaInitializer.SourceRelation.RelatedData = parentData;
                TemporaryView.QuickSearchVisibility = !TemporaryView.QuickSearchVisibility;
                if (TemporaryView.QuickSearchVisibility)
                    TemporaryView.QuickSearchSelectAll();
            }
            else if (linkType == TemporaryLinkType.Popup)
            {
                //if (AreaInitializer.SourceRelation != null)
                //    AreaInitializer.SourceRelation.RelatedData = parentData;
                if (!TemporaryView.PopupVisibility)
                {
                    SearchViewEntityArea.RemoveViewEntityAreaView();
                    if (!TemporaryView.HasPopupView)
                        TemporaryView.AddPopupView(SearchViewEntityArea.ViewEntityArea.ViewView);
                }
                TemporaryView.PopupVisibility = !TemporaryView.PopupVisibility;
            }
            else if (linkType == TemporaryLinkType.Clear)
            {
                ClearData(false);
                //TemporaryView.SetLinkText("");
            }
            else if (linkType == TemporaryLinkType.Info)
            {

                AgentHelper.ShowEditEntityAreaInfo(this);
            }
        }
        public void ShowTemporarySearchView(bool fromDataView)
        {
            //if (AreaInitializer.SourceRelation != null && ChildRelationshipInfo == null)
            //    return;
            if (LastTemporaryView != null)
                LastTemporaryView.RemovePopupView(SearchViewEntityArea.ViewEntityArea.ViewView);
            SearchViewEntityArea.IsCalledFromDataView = fromDataView;
            SearchViewEntityArea.ShowTemporarySearchView();
        }
        public void ShowTemporaryDataView()
        {
            ObservableCollection<DP_DataRepository> existingData = GetDataList();
            //if (existingData == null)
            //    return;
            //////if (AreaInitializer.BusinessReadOnlyByParent || (ChildRelationshipInfo != null && AreaInitializer.ParentDataItemBusinessReadOnly.Any(x => x == ChildRelationshipInfo.SourceData)) || AreaInitializer.SecurityReadOnlyByParent || AreaInitializer.SecurityReadOnly)
            //////{
            //////    if (!existingData.Any())
            //////    {
            //////        return;
            //////    }
            //////}


            if (AreaInitializer.FormComposed == false)
            {
                GenerateDataView();
            }
            DataView.IsOpenedTemporary = true;

            if (this is I_EditEntityAreaOneData && existingData.Count == 0)
            {
                (this as I_EditEntityAreaOneData).CreateDefaultData();
            }
            else
            {
                if (this is I_EditEntityAreaMultipleData)
                    (this as I_EditEntityAreaMultipleData).RemoveDataContainers();

                foreach (var data in existingData)
                {
                    if (!data.IsFullData)
                    {
                        //if (!data.KeyProperties.Any())
                        //    throw new Exception("asdad");
                        var resConvert = AreaInitializer.EditAreaDataManager.ConvertDataViewToFullData(AreaInitializer.EntityID, data, this);
                        if (!resConvert)
                        {
                            //ممکن است اینجا داده وابسته فول شود اما در نمایش فرم بعلت عدم دسترسی به داده  وابسته برای داده وابسته جاری فرم نمایش داده نشود
                            //بنابراین هر فولی به معنی اصلاح شدن داده نیست و باید خصوصیت دیگری در نظر گرفت
                            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", data.ViewInfo, Temp.InfoColor.Red);
                            return;
                        }
                    }
                    if (!this.ShowDataInDataView(data))
                    {
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده و یا داده های وابسته", data.ViewInfo, Temp.InfoColor.Red);
                        return;
                    }
                }
            }
            var dialogManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow();
            dialogManager.WindowClosed += DialogManager_WindowClosed;
            dialogManager.ShowDialog(DataView, SimpleEntity.Alias, Enum_WindowSize.Big);
        }

        private void DialogManager_WindowClosed(object sender, EventArgs e)
        {
            DataView.IsOpenedTemporary = false;
            SetTempText(GetDataList());
            CheckRelationshipLabel();
            //foreach (var item in GetDataList())
            //{
            //    OnDataItemUnShown(new EditAreaDataItemArg() { DataItem = item });
            //}
        }

        private I_SearchViewEntityArea GenerateSearchViewArea()
        {
            var searchViewEntityArea = new SearchViewEntityArea();
            var searchViewInit = new SearchViewAreaInitializer();
            searchViewInit.SourceEditArea = this as I_EditEntityArea;
            //  searchViewInit.TempEntity = FullEntity;
            searchViewInit.EntityID = AreaInitializer.EntityID;
            searchViewEntityArea.SetAreaInitializer(searchViewInit);
            searchViewEntityArea.DataSelected += SearchViewEntityArea_DataSelected;
            return searchViewEntityArea;
        }
        private void SearchViewEntityArea_DataSelected(object sender, DataSelectedEventArg e)
        {
            if (e.DataItem.Count > 1)
            {
                throw new Exception("asdasd");
            }
            DP_DataRepository result = null;
            if (SearchViewEntityArea.IsCalledFromDataView)
            {
                result = AreaInitializer.EditAreaDataManager.GetFullDataFromDataViewSearch(AreaInitializer.EntityID, e.DataItem.First(), this);

            }
            else
            {
                result = AreaInitializer.EditAreaDataManager.ConvertDP_DataViewToDP_DataRepository(e.DataItem.First(), this);
            }
            //بهتره چک شه اگر دادهی ای موجود بود بعد کلیر بشه
            //بعدا بررسی شود که آیا لازم است ابتدا کلیر شود
            ClearData(false);

            if (result != null)
            {
                //اینجا تست شود
                bool dataCleared = true;
                if (AreaInitializer.SourceRelation != null)
                    if (GetDataList().Any())
                    {
                        //وقتی داده هنوز موجود باشد یعنی کلیر انجام نشده است
                        //چون ممکنه داده باید دیلیت بشه و کاربر موافقت نکنه
                        dataCleared = false;
                    }
                if (dataCleared)
                {
                    bool addResult = false;
                    addResult = AddData(result, SearchViewEntityArea.IsCalledFromDataView);
                    if (!addResult)
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده و یا داده های وابسته", result.ViewInfo, Temp.InfoColor.Red);
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", e.DataItem.First().ViewInfo, Temp.InfoColor.Red);
            }
            //}
        }
        private void Datas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataCollectionChanged(AreaInitializer.Datas, e);
            DecideDataRelatedButtons();
        }
        public bool CheckRemoveData(List<DP_DataRepository> datas)
        {
            bool clearIsOk = true;

            bool shouldDeleteFromDB = false;
            var existingdatas = datas.Where(x => x.IsDBRelationship);
            if (existingdatas.Count() != 0)
            {
                if (AreaInitializer.SourceRelation != null)
                {

                    if (AreaInitializer.SourceRelation.MasterRelationshipType == Enum_MasterRelationshipType.FromPrimartyToForeign)
                    {
                        if (AreaInitializer.SourceRelation.Relationship.DeleteOption == RelationshipDeleteOption.DeleteCascade || AreaInitializer.SourceRelation.Relationship.RelationshipColumns.Any(x => !x.SecondSideColumn.IsNull))
                        {
                            shouldDeleteFromDB = true;
                        }
                    }
                    //بعدا به این فکر شود
                    //var relationship = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetRelationship(AreaInitializer.SourceRelation.Relationship.PairRelationshipID);
                    //if (relationship.IsOtherSideMandatory)
                    //    shouldDeleteFromDB = true;
                }
            }

            if (shouldDeleteFromDB)
            {
                //////if (AreaInitializer.SecurityEditAndDelete == false)
                //////{
                //////    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("پیام", "به علت رابطه اجباری داده نیاز به حذف شدن دارد اما دسترسی حذف وجود ندارد");
                //////    clearIsOk = false;
                //////}
                var deleteList = existingdatas.ToList();
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                DR_DeleteInquiryRequest request = new DR_DeleteInquiryRequest(requester);
                request.DataItems = deleteList.Cast<DP_BaseData>().ToList();
                var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendDeleteInquiryRequest(request);
                I_ViewDeleteInquiry view = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDeleteInquiryView();
                view.SetTreeItems(reuslt.DataTreeItems);
                if (reuslt.Loop == true)
                {
                    clearIsOk = false;
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("بعلت وجود حلقه وابستگی بین داده ها امکان حذف داده (داده های) انتخاب شده وجود ندارد");
                }
                else
                {
                    view.SetUserConfirmMode(UserDialogMode.YesNo);
                    if (reuslt.DataTreeItems.Any(x => x.ChildRelationshipInfos.Any(y => y.RelationshipDeleteOption == ModelEntites.RelationshipDeleteOption.DeleteCascade && y.RelatedData.Any())))
                        view.SetMessage("داده های وابسته نمایش داده شده نیز حذف خواهند شد. آیا مطمئن هستید؟");
                    else
                        view.SetMessage("داده نمایش داده شده حذف خواهد شد. آیا مطمئن هستید؟");
                    var result = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowPromptDialog(view, "");
                    if (result == UserDialogResult.Ok || result == UserDialogResult.No)
                    {
                        clearIsOk = false;
                    }
                    else if (result == UserDialogResult.Yes)
                    {
                        clearIsOk = true;
                    }
                }
            }

            return clearIsOk;
        }
        public bool ClearData(bool createDefaultData)
        {
            var datas = GetDataList();
            //if (datas == null)
            //    return false;
            //if (this.AreaInitializer.SourceRelation == null && DataEntryEntity.IsReadonly)
            //    return false;
            //if (this.AreaInitializer.SourceRelation != null && this.AreaInitializer.SourceRelation.Relationship.IsReadonly)
            //    return false;


            foreach (var item in datas)
            {
                if (this.AreaInitializer.SourceRelation != null && this.AreaInitializer.SourceRelation.Relationship.IsReadonly && item.IsDBRelationship)
                    throw new Exception("Relationship is readonly!");
            }

            var clearIsOk = CheckRemoveData(datas.ToList());
            if (clearIsOk)
            {

                GetDataList().Clear();
                if (this is I_EditEntityAreaOneData)
                {
                    if (createDefaultData)
                        (this as I_EditEntityAreaOneData).CreateDefaultData();

                    return true;
                }
                else if (this is I_EditEntityAreaMultipleData)
                {
                    (this as I_EditEntityAreaMultipleData).RemoveDataContainers();
                    return true;
                }
                return false;
            }
            else
                return false;
        }
        //public void SetRemovedItem(ChildRelationshipInfo childRelationshipInfo, DP_DataRepository item, bool delete)
        //{
        //    //if (childRelationshipInfo.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
        //    //{
        //    if (!childRelationshipInfo.ParentData.IsNewItem)
        //    {
        //        //RemoveRelationshipInfo removeChildRelationshipInfo;
        //        //if (!childRelationshipInfo.RemovedItems.Any(x => x.Relationship.ID == childRelationshipInfo.Relationship.ID))
        //        //{
        //        //removeChildRelationshipInfo = new RemoveRelationshipInfo();
        //        //if (delete)
        //        //    removeChildRelationshipInfo.RelationshipDeleteOption = RelationshipDeleteOption.DeleteCascade;
        //        //else
        //        //    removeChildRelationshipInfo.RelationshipDeleteOption = RelationshipDeleteOption.SetNull;
        //        if (delete && childRelationshipInfo.RelationshipDeleteOption != RelationshipDeleteOption.DeleteCascade)
        //            throw new Exception("cbcbcxvbcvb");
        //        childRelationshipInfo.RemovedItems.Add(item);
        //        //}
        //        //else
        //        //    removeChildRelationshipInfo = childRelationshipInfo.RemovedItems.First(x => x.Relationship.ID == childRelationshipInfo.Relationship.ID);
        //        // removeChildRelationshipInfo.RelatedData.Add(item);
        //    }
        //    else
        //        throw new Exception("xvxcvb");
        //    //}
        //}

        public bool BaseAddData(DP_DataRepository data)
        {
            if (data == null)
                throw new Exception("sdf");
            if (data.EntityListView == null)
                data.EntityListView = DefaultEntityListViewDTO;
            if (AreaInitializer.SourceRelation == null)
            {
                AreaInitializer.Datas.Add(data);
            }
            else
            {
                if (ChildRelationshipInfo != null)
                    ChildRelationshipInfo.AddDataToChildRelationshipInfo(data, false);
                else
                    return false;
            }
            //OnDataItemLoaded(new EditAreaDataItemLoadedArg() { DataItem = data, InEditMode = true });
            return true;
        }

        List<ChildRelationshipInfo> RegisteredParentDataItems = new List<ChildRelationshipInfo>();
        public void SetChildRelationshipInfo(ChildRelationshipInfo value)
        {

            //اگر قبلا لود شده بود دیگر لود نمیشود. به احتمال زیاد اگر پرنت هم فرم یک داده ای باشد قبلا به هنگام نمایش داده در پرنت این هم یکبار صدا زده شده و داده لود شده است
            if (value == null)
                throw new Exception("dxvxcv");
            ChildRelationshipInfo = value;
            if (!RegisteredParentDataItems.Contains(ChildRelationshipInfo))
            {
                //برای این است که کالکشن چنج تنها یکبار برای هر مجموعه داده صدا زده شود
                ChildRelationshipInfo.CollectionChanged += (sender1, e1) => NewValue_RelatedDataChanged(sender1, e1, ChildRelationshipInfo);
                RegisteredParentDataItems.Add(ChildRelationshipInfo);
            }

            //تنها در صورت نمایش پرنت این یکبار صدا زده میشود و در حالات دیگر به مانند صدا زدن به هنگام آپدیت یا ولیدشن کاربردی ندارد


        }

        public bool SetChildRelationshipInfoAndShow(ChildRelationshipInfo value)
        {
            bool result = true;
            SetChildRelationshipInfo(value);

            bool isDataView = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
             AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
            if (isDataView)
            {
                if (this is I_EditEntityAreaOneData && !ChildRelationshipInfo.RelatedData.Any())
                    (this as I_EditEntityAreaOneData).CreateDefaultData();
                else
                {
                    if (this is I_EditEntityAreaMultipleData)
                        (this as I_EditEntityAreaMultipleData).RemoveDataContainers();
                    foreach (var data in ChildRelationshipInfo.RelatedData)
                    {
                        if (data.EntityListView == null)
                            data.EntityListView = DefaultEntityListViewDTO;
                        var dataViewResult = this.ShowDataInDataView(data);
                        if (!dataViewResult)
                            result = false;
                    }
                }

            }
            else
            {
                SetTempText(ChildRelationshipInfo.RelatedData);
            }

            CheckRelationshipLabel();
            DecideDataRelatedButtons();

            return result;
        }

        //میتونه یه فنکشن کلی برای برسسی فعال بودن فرم بشه. مثلا وقتی وضعیتها هم بخوان دستکاری کنن فعال بودن فرم رو


        public IAG_View_TemporaryView TemporaryDisplayView { set; get; }
        //public void OnDataItemLoaded(EditAreaDataItemLoadedArg arg)
        //{
        //    if (DataItemLoaded != null)
        //        DataItemLoaded(this, arg);
        //}


            //اصلاح و بهتر شود برای تمپ ویوها
        public void OnDataItemShown(EditAreaDataItemLoadedArg arg)
        {
            if (DataItemShown != null)
                DataItemShown(this, arg);
        }
        //public void OnDataItemUnShown(EditAreaDataItemArg arg)
        //{
        //    if (DataItemUnShown != null)
        //        DataItemUnShown(this, arg);
        //}
        public void SetTempText(ObservableCollection<DP_DataRepository> relatedData)
        {
            string text = "";
            //اینجا باید داده هایی که اسکیپ میشن رو در نظر نگیره
            if (relatedData.Count > 1)
                text = "تعداد" + " " + relatedData.Count + " " + "مورد";
            foreach (var item in relatedData)
                text += (text == "" ? "" : Environment.NewLine) + item.ViewInfo;

            if (AreaInitializer.SourceRelation == null || AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaOneData)
            {
                TemporaryDisplayView.SetLinkText(text);
                //if (string.IsNullOrEmpty(text))
                //{
                //    TemporaryDisplayView.ClearSearchText();
                //}
                if (TemporaryDisplayView.QuickSearchVisibility)
                    TemporaryDisplayView.QuickSearchVisibility = false;
            }
            else
            {
                var relationshipControl = (AreaInitializer.SourceRelation.SourceEditArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.First(x => x.Relationship.ID == ChildRelationshipInfo.Relationship.ID);
                relationshipControl.RelationshipControlManager.SetTemporaryViewText(ChildRelationshipInfo.SourceData, text);
                //if (string.IsNullOrEmpty(text) && relationshipControl.EditNdTypeArea.TemporaryLinkState.searchView)
                //    relationshipControl.RelationshipControlManager.SetQuickSearchVisibility(ChildRelationshipInfo.SourceData, true);
                //else
                //    relationshipControl.RelationshipControlManager.SetQuickSearchVisibility(ChildRelationshipInfo.SourceData, false);
                relationshipControl.RelationshipControlManager.SetQuickSearchVisibility(ChildRelationshipInfo.SourceData, false);

            }

            //خیلی ایده آل نیست
            //برای اینه که وضعیتها اونم فقط توحالتی که رابطه فارن گه پرنت باشه چک بشه که اگه لازم بود دور ویو تمپ قرمز بشه 
            foreach (var dataItem in relatedData)
                OnDataItemShown(new EditAreaDataItemLoadedArg() { DataItem = dataItem, InEditMode = true });

        }

        private void NewValue_RelatedDataChanged(object sender, NotifyCollectionChangedEventArgs e, ChildRelationshipInfo childRelationshipInfo)
        {
            DataCollectionChanged(ChildRelationshipInfo.RelatedData, e);
            if (AreaInitializer.SourceRelation != null)
            {
                if (AreaInitializer.SourceRelation.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                {
                    foreach (var col in AreaInitializer.SourceRelation.Relationship.RelationshipColumns)
                    {
                        var fkProp = ChildRelationshipInfo.SourceData.GetProperty(col.FirstSideColumnID);
                        if (fkProp != null)
                        {
                            if (ChildRelationshipInfo.RelatedData.Any())
                            {
                                var pkProp = ChildRelationshipInfo.RelatedData.First().GetProperty(col.SecondSideColumnID);
                                if (pkProp != null)
                                    fkProp.Value = pkProp.Value;
                            }
                            else
                                fkProp.Value = "<Null>";
                        }
                    }
                }
                else if (AreaInitializer.SourceRelation.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                {
                    foreach (var fkDataItem in ChildRelationshipInfo.RelatedData)
                    {
                        foreach (var col in AreaInitializer.SourceRelation.Relationship.RelationshipColumns)
                        {
                            var pkProp = ChildRelationshipInfo.SourceData.GetProperty(col.FirstSideColumnID);
                            if (pkProp != null)
                            {
                                var fkProp = fkDataItem.GetProperty(col.SecondSideColumnID);
                                if (fkProp != null)
                                    fkProp.Value = pkProp.Value;
                            }
                        }
                    }

                }
            }
        }
        public IAG_View_TemporaryView LastTemporaryView { set; get; }

        public void DataCollectionChanged(ObservableCollection<DP_DataRepository> dataList, NotifyCollectionChangedEventArgs e)
        {
            //bool result = true;
            bool childIsDataView = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
             AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);

            if (this is I_EditEntityAreaOneData)
            {

                if (LastTemporaryView != null || TemporaryDisplayView != null)
                {
                    if (LastTemporaryView != null)
                    {
                        LastTemporaryView.PopupVisibility = false;
                        //   LastTemporaryView.QuickSearchVisibility = false;
                    }
                    else
                    {
                        TemporaryDisplayView.PopupVisibility = false;
                        //    TemporaryDisplayView.QuickSearchVisibility = false;
                    }
                }

                if (!childIsDataView)
                {
                    SetTempText(dataList);

                    bool eventOk = false;
                    if (GetDataList().Count == 1 && !GetDataList().First().IsNewItem)
                    {
                        eventOk = true;
                    }
                    else if (GetDataList().Count == 0)
                        eventOk = true;
                    if (eventOk)
                    {
                        (this as I_EditEntityAreaOneData).OnDataItemSelected(GetDataList().FirstOrDefault());

                    }
                }
            }
            else if (this is I_EditEntityAreaMultipleData)
            {
                if (!childIsDataView)
                    SetTempText(dataList);
            }
            CheckRelationshipLabel();
        }

        private void CheckRelationshipLabel()
        {
            //////if (AreaInitializer.SourceRelation != null)
            //////{

            //////    if (AreaInitializer.SourceRelation.SourceRelationshipColumnControl.RelationshipControlManager.TabPageContainer != null ||
            //////                AreaInitializer.SourceRelation.SourceRelationshipColumnControl.RelationshipControlManager.HasExpander)
            //////    {
            //////        AreaInitializer.SourceRelation.SourceRelationshipColumnControl.RemoveLabelControlManagerColorByKey("hasdata");
            //////        bool color = false;
            //////        if (this is I_EditEntityAreaMultipleData)
            //////        {
            //////            if (GetDataList().Count() != 0)
            //////                color = true;
            //////        }
            //////        else if (this is I_EditEntityAreaOneData)
            //////        {
            //////            if (AreaInitializer.SourceRelation.Relationship.IsOtherSideDirectlyCreatable)
            //////            {
            //////                if (GetDataList().Count(x => !x.IsNewItem) != 0)
            //////                    color = true;
            //////            }
            //////            else
            //////            {
            //////                if (GetDataList().Count() != 0)
            //////                    color = true;
            //////            }
            //////        }
            //////        if (color)
            //////            AreaInitializer.SourceRelation.SourceRelationshipColumnControl.AddLabelControlManagerColor(new BaseColorItem() { Key = "hasdata", Color = InfoColor.LightGray, ColorTarget = ControlColorTarget.Background });
            //////    }
            //////}
        }

        public ObservableCollection<FormulaResult> FormulaExceptionReulsts
        {
            set; get;
        }

        public List<UIActionActivityDTO> RunningActionActivities
        {
            set; get;
        }

        public void SelectFromParent(DP_DataRepository parentDataItem, Dictionary<int, string> colAndValues)
        {
            bool fromDataview = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                            AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
            SearchViewEntityArea.SelectFromParent(fromDataview, AreaInitializer.SourceRelation.Relationship, parentDataItem, colAndValues);
        }

















        private List<DataMessageItem> ControlManagerMessageItems = new List<DataMessageItem>();
        private string GetTooltip(List<BaseMessageItem> MessageItems)
        {
            var tooltip = "";
            foreach (var item in MessageItems.OrderBy(x => x.Priority))
                tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;
            return tooltip;
        }
        public void AddControlManagerMessage(DataMessageItem baseMessageItem)
        {
            ControlManagerMessageItems.Add(baseMessageItem);
            var tooltip = GetTooltip(ControlManagerMessageItems.Where(x => x.CausingDataItem == baseMessageItem.CausingDataItem).ToList<BaseMessageItem>());
            DataView.SetTooltip(baseMessageItem.CausingDataItem, tooltip);
        }
        public void RemoveControlManagerMessage(DataMessageItem baseMessageItem)
        {
            ControlManagerMessageItems.Remove(baseMessageItem);
            var tooltip = GetTooltip(ControlManagerMessageItems.Where(x => x.CausingDataItem == baseMessageItem.CausingDataItem).ToList<BaseMessageItem>());
            DataView.SetTooltip(baseMessageItem.CausingDataItem, tooltip);
        }
        public void RemoveControlManagerMessage(DP_DataRepository dataItem, string key)
        {
            foreach (var item in ControlManagerMessageItems.Where(x => x.CausingDataItem == dataItem && x.Key == key).ToList())
                RemoveControlManagerMessage(item);
        }
        public void RemoveControlManagerMessageByKey(string key)
        {
            foreach (var item in ControlManagerMessageItems.Where(x => x.Key == key).ToList())
                RemoveControlManagerMessage(item);
        }


        private List<DataColorItem> ControlManagerColorItems = new List<DataColorItem>();
        public void AddControlManagerColor(DataColorItem baseColorItem)
        {
            ControlManagerColorItems.Add(baseColorItem);
            SetControlManagerColor(ControlManagerColorItems.Where(x => x.CausingDataItem == baseColorItem.CausingDataItem && x.ColorTarget == baseColorItem.ColorTarget).ToList<BaseColorItem>(), baseColorItem.ColorTarget, baseColorItem.CausingDataItem);
        }
        public void RemoveControlManagerColor(DataColorItem baseColorItem)
        {
            ControlManagerColorItems.Remove(baseColorItem);
            SetControlManagerColor(ControlManagerColorItems.Where(x => x.CausingDataItem == baseColorItem.CausingDataItem && x.ColorTarget == baseColorItem.ColorTarget).ToList<BaseColorItem>(), baseColorItem.ColorTarget, baseColorItem.CausingDataItem);
        }
        public void RemoveControlManagerColor(DP_DataRepository dataItem, string key)
        {
            foreach (var item in ControlManagerColorItems.Where(x => x.CausingDataItem == dataItem && x.Key == key).ToList())
                RemoveControlManagerColor(item);
        }
        public void RemoveControlManagerColorByKey(string key)
        {
            foreach (var item in ControlManagerColorItems.Where(x => x.Key == key).ToList())
                RemoveControlManagerColor(item);
        }
        private void SetControlManagerColor(List<BaseColorItem> list, ControlColorTarget colorTarget, object dataItem)
        {
            var color = GetColor(list);
           if(DataItemIsInEditMode(dataItem as DP_DataRepository))
                DataView

                TemporaryDisplayView گرفته شود
            if (colorTarget == ControlColorTarget.Background)
            {
                DataView.SetBackgroundColor(dataItem, color);
            }
            else if (colorTarget == ControlColorTarget.Foreground)
            {
                DataView.SetForegroundColor(dataItem, color);
            }
            if (colorTarget == ControlColorTarget.Border)
            {
                DataView.SetBorderColor(dataItem, color);
            }
        }
        private InfoColor GetColor(List<BaseColorItem> list)
        {
            var color = InfoColor.Null;
            foreach (var item in list.Where(x => x.Color != InfoColor.Null).OrderByDescending(x => x.Priority))
                color = item.Color;
            return color;
        }

        public void DecideDataViewStaticButtons()
        {
            if (DataEntryEntity.IsReadonly || (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly))
            {
                var addCommand = GetCommand(typeof(AddCommand));
                if (addCommand != null)
                    addCommand.CommandManager.SetEnabled(false);
            }

            if (AreaInitializer.SourceRelation == null && DataEntryEntity.IsReadonly)
            {
                var deleteCommand = GetCommand(typeof(DeleteCommand));
                deleteCommand.CommandManager.SetEnabled(false);

                //var saveCommand = GetCommand(typeof(SaveCommand));
                //saveCommand.CommandManager.SetEnabled(false);

                //var saveCloseCommand = GetCommand(typeof(SaveAndCloseDialogCommand));
                //if (saveCloseCommand != null)
                //    saveCloseCommand.CommandManager.SetEnabled(saveEnable);
            }
            else if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly)
            {
                var removeCommand = GetCommand(typeof(RemoveCommand));
                if (removeCommand != null)
                    removeCommand.CommandManager.SetEnabled(false);

                var searchCommand = GetCommand(typeof(SearchCommand));
                if (searchCommand != null)
                    searchCommand.CommandManager.SetEnabled(false);

                var clearCommand = GetCommand(typeof(ClearCommand));
                if (clearCommand != null)
                    clearCommand.CommandManager.SetEnabled(false);
            }
        }
        public void DecideTempViewStaticButtons()
        {
            if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly)
            {
                if (TemporaryDisplayView != null)
                {
                    TemporaryDisplayView.DisableEnable(TemporaryLinkType.Clear, false);
                    TemporaryDisplayView.DisableEnable(TemporaryLinkType.SerachView, false);
                    TemporaryDisplayView.DisableEnable(TemporaryLinkType.Popup, false);
                    TemporaryDisplayView.DisableEnable(TemporaryLinkType.QuickSearch, false);
                    TemporaryDisplayView.QuickSearchVisibility = false;
                }
                TemporaryLinkState.clear = false;
                TemporaryLinkState.searchView = false;
                TemporaryLinkState.popup = false;
                TemporaryLinkState.quickSearch = false;
            }
        }
        public void DecideDataRelatedButtons()
        {
            bool isDataView = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
             AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
            var dataList = GetDataList();
            if (AreaInitializer.SourceRelation == null && DataEntryEntity.IsReadonly)
            {
                var saveCommand = GetCommand(typeof(SaveCommand));
                bool saveEnable = true;
                if (!dataList.Any())
                    saveEnable = false;
                else if (dataList.Any(x => x.IsNewItem))
                    saveEnable = false;
                saveCommand.CommandManager.SetEnabled(saveEnable);
            }
            if (AreaInitializer.SourceRelation != null && AreaInitializer.SourceRelation.Relationship.IsReadonly)
            {
                if (!isDataView)
                {
                    bool dataview = true;
                    if (dataList.Count == 0)
                    {
                        dataview = false;
                    }

                    if (AreaInitializer.SourceRelation == null || AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaOneData)
                    {
                        TemporaryDisplayView.DisableEnable(TemporaryLinkType.DataView, dataview);
                        TemporaryLinkState.edit = dataview;
                    }
                    else
                    {
                        var relationshipControl = (AreaInitializer.SourceRelation.SourceEditArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.First(x => x.Relationship.ID == ChildRelationshipInfo.Relationship.ID);
                        relationshipControl.RelationshipControlManager.EnableDisable(ChildRelationshipInfo.SourceData, TemporaryLinkType.DataView, dataview);
                    }
                }


            }
        }

        private I_Command GetCommand(Type type)
        {
            return Commands.FirstOrDefault(x => x.GetType() == type);
        }
        public bool DataItemIsInEditMode(DP_DataRepository dataItem)
        {
            if (GetDataList().Any(x => x == dataItem))
            {
                bool hasTempView = (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
          AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect ||
           AreaInitializer.IntracionMode == IntracionMode.Select);
                if (hasTempView)
                {
                    if (DataView != null && DataView.IsOpenedTemporary)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (AreaInitializer.SourceRelation == null)
                    {
                        if (AreaInitializer.IntracionMode == IntracionMode.CreateDirect || AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return AreaInitializer.SourceRelation.SourceEditArea.DataItemIsInEditMode(dataItem.ParantChildRelationshipInfo.SourceData);
                    }
                }

            }
            return false;
        }
        public bool DataItemIsInViewMode(DP_DataRepository dataItem)
        {
            return DataItemIsInEditMode(dataItem) || DataItemIsInTempViewMode(dataItem);
        }
        public bool DataItemIsInTempViewMode(DP_DataRepository dataItem)
        {
            if (GetDataList().Any(x => x == dataItem))
            {
                bool hasTempView = (AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
           AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect ||
            AreaInitializer.IntracionMode == IntracionMode.Select);
                //if ((DataView == null || DataView.IsOpenedTemporary == false) && (AreaInitializer.SourceRelation == null || AreaInitializer.SourceRelation.SourceEditArea.DataItemIsInEditMode(dataItem.ParantChildRelationshipInfo.SourceData)))
                if (hasTempView && (AreaInitializer.SourceRelation == null || AreaInitializer.SourceRelation.SourceEditArea.DataItemIsInEditMode(dataItem.ParantChildRelationshipInfo.SourceData)))
                    return true;
            }
            return false;
        }
        public void SetColumnValueFromState(DP_DataRepository dataItem, List<UIColumnValueDTO> uIColumnValue, EntityStateDTO state)
        {
            if (DataItemIsInEditMode(dataItem))
            {
                List<Tuple<DP_DataRepository, SimpleColumnControl, string>> simpleColumnValues = new List<Tuple<DP_DataRepository, SimpleColumnControl, string>>();
                List<Tuple<DP_DataRepository, RelationshipColumnControl, Dictionary<int, string>>> relationshipColumnValues = new List<Tuple<DP_DataRepository, RelationshipColumnControl, Dictionary<int, string>>>();

                foreach (var columnValue in uIColumnValue)
                {
                    var column = dataItem.GetProperty(columnValue.ColumnID);
                    if (column != null)
                    {
                        //اینجا باید بیزینسی ریدونلی شدن داده هم تست شود
                        if (RelationshipColumnControls.Any(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && !x.Relationship.IsReadonly && x.Relationship.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ColumnID)))
                        {
                            var relationshipColumn = RelationshipColumnControls.First(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.Relationship.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ColumnID));
                            if (relationshipColumn.Relationship.RelationshipColumns.All(x => uIColumnValue.Any(z => z.ColumnID == x.FirstSideColumnID)))
                            {
                                Dictionary<int, string> listColumns = new Dictionary<int, string>();
                                foreach (var relCol in relationshipColumn.Relationship.RelationshipColumns)
                                {
                                    listColumns.Add(relCol.FirstSideColumnID, uIColumnValue.First(x => x.ColumnID == relCol.FirstSideColumnID).ExactValue);
                                }
                                relationshipColumnValues.Add(new Tuple<DP_DataRepository, RelationshipColumnControl, Dictionary<int, string>>(dataItem, relationshipColumn, listColumns));
                            }
                        }
                        else if (SimpleColumnControls.Any(x => x.Column.ID == column.ColumnID))
                        {
                            //اینجا باید بیزینسی ریدونلی شدن داده هم تست شود
                            var simpleColumn = SimpleColumnControls.First(x => x.Column.ID == column.ColumnID);
                            if (!simpleColumn.Column.IsReadonly)
                            {
                                simpleColumnValues.Add(new Tuple<DP_DataRepository, SimpleColumnControl, string>(dataItem, simpleColumn, columnValue.ExactValue));
                            }
                        }
                    }
                }


                foreach (var item in simpleColumnValues)
                {
                    if (dataItem.IsNewItem && AgentHelper.ValueIsEmptyOrDefaultValue(item.Item1.GetProperty(item.Item2.Column.ID)))
                        SetDataItemSimplePropertyValue(item, "بر اساس وضعیت" + " " + state.Title);
                }
                foreach (var item in relationshipColumnValues)
                {
                    if (dataItem.IsNewItem && item.Item3.All(x => AgentHelper.ValueIsEmptyOrDefaultValue(item.Item1.GetProperty(x.Key))))
                        SetDataItemRelationshipColumnValue(item, "بر اساس وضعیت" + " " + state.Title);
                }
            }
        }


        public void SetColumnValueRange(SimpleColumnControl propertyControl, List<ColumnValueRangeDetailsDTO> details)
        {
            propertyControl.SimpleControlManager.SetColumnValueRange(details);
        }
        public void SetColumnValueRangeFromState(SimpleColumnControl simpleColumn, List<ColumnValueRangeDetailsDTO> details, DP_DataRepository dataItem, EntityStateDTO state)
        {
            if (DataItemIsInEditMode(dataItem))
            {
                var property = dataItem.GetProperty(simpleColumn.Column.ID);
                if (!simpleColumn.Column.IsReadonly && !property.IsHidden && !property.IsReadonly)
                    simpleColumn.SimpleControlManager.SetColumnValueRange(details, dataItem);
            }
        }

        public void ResetColumnValueRangeFromState(SimpleColumnControl simpleColumn, DP_DataRepository dataItem, EntityStateDTO state)
        {
            if (DataItemIsInEditMode(dataItem))
            {
                var property = dataItem.GetProperty(simpleColumn.Column.ID);
                if (!simpleColumn.Column.IsReadonly && !property.IsHidden && !property.IsReadonly)
                    simpleColumn.SimpleControlManager.SetColumnValueRange(simpleColumn.Column.ColumnValueRange.Details, dataItem);
            }
        }
        public void ChangeSimpleColumnVisiblityFromState(DP_DataRepository dataItem, SimpleColumnControl simpleColumn, bool visible, string title, bool imposeInUI)
        {
            if (DataItemIsInEditMode(dataItem))
            {
                dataItem.GetProperty(simpleColumn.Column.ID).IsHidden = !visible;
                if (imposeInUI)
                {
                    (simpleColumn as SimpleColumnControl).ControlManager.Visiblity(dataItem, visible);
                }
                else
                {
                    if (visible)
                    {
                        simpleColumn.RemoveLabelControlManagerColorByKey(title);
                        simpleColumn.RemoveLabelControlManagerMessageByKey(title);
                        simpleColumn.RemoveControlManagerColorByKey(title);
                        simpleColumn.RemoveControlManagerMessageByKey(title);
                    }
                    else
                    {
                        simpleColumn.AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Background, Key = title, Priority = ControlItemPriority.High });
                        simpleColumn.AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Border, Key = title, Priority = ControlItemPriority.High });
                        simpleColumn.AddControlManagerMessage(new DataMessageItem() { CausingDataItem = dataItem, Message = title + Environment.NewLine + "ترتیب اثری به داده نخواهد شد", Key = title, Priority = ControlItemPriority.High });
                        simpleColumn.AddLabelControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Background, Key = title, Priority = ControlItemPriority.High });
                        simpleColumn.AddLabelControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Border, Key = title, Priority = ControlItemPriority.High });
                        simpleColumn.AddLabelControlManagerMessage(new DataMessageItem() { CausingDataItem = dataItem, Message = title + Environment.NewLine + "ترتیب اثری به داده نخواهد شد", Key = title, Priority = ControlItemPriority.High });
                    }
                }
            }
        }

        public void ChangeSimpleColumnReadonlyFromState(DP_DataRepository dataItem, SimpleColumnControl simpleColumn, bool isReadonly, string state)
        {
            if (DataItemIsInEditMode(dataItem))
            {
                dataItem.GetProperty(simpleColumn.Column.ID).IsReadonly = isReadonly;
                if (!DecideSimpleColumnReadony(simpleColumn, false))
                    (simpleColumn as SimpleColumnControl).SimpleControlManager.SetReadonly(dataItem, isReadonly);
            }
        }

        public void ChangeRelatoinsipColumnVisiblityFromState(DP_DataRepository dataItem, RelationshipColumnControl relationshipControl, bool visible, string title, bool imposeInUI)
        {
            ChildRelationshipInfo childRelationshipInfo = null;
            if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
            {
                if (DataItemIsInEditMode(dataItem))
                {
                    childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                    childRelationshipInfo.IsHidden = !visible;
                    //if (!visible && childRelationshipInfo.RelatedData.Any(x => x.IsDBRelationship))
                    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("رابطه" + " " + relationshipControl.Relationship.Alias + " " + "با وجود داده مخفی شده است");
                    if (imposeInUI)
                    {
                        relationshipControl.ControlManager.Visiblity(dataItem, visible);
                    }
                    else
                    {
                        if (visible)
                        {
                            relationshipControl.RemoveLabelControlManagerColorByKey(title);
                            relationshipControl.RemoveLabelControlManagerMessageByKey(title);
                            relationshipControl.RemoveControlManagerColorByKey(title);
                            relationshipControl.RemoveControlManagerMessageByKey(title);
                        }
                        else
                        {
                            relationshipControl.AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Background, Key = title, Priority = ControlItemPriority.High });
                            relationshipControl.AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Border, Key = title, Priority = ControlItemPriority.High });
                            relationshipControl.AddControlManagerMessage(new DataMessageItem() { CausingDataItem = dataItem, Message = title + Environment.NewLine + "ترتیب اثری به داده نخواهد شد", Key = title, Priority = ControlItemPriority.High });
                            relationshipControl.AddLabelControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Background, Key = title, Priority = ControlItemPriority.High });
                            relationshipControl.AddLabelControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Border, Key = title, Priority = ControlItemPriority.High });
                            relationshipControl.AddLabelControlManagerMessage(new DataMessageItem() { CausingDataItem = dataItem, Message = title + Environment.NewLine + "ترتیب اثری به داده نخواهد شد", Key = title, Priority = ControlItemPriority.High });
                        }
                    }
                }
            }
            else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
            {
                bool dataIsInValidMode = DataItemIsInEditMode(dataItem) || (this is I_EditEntityAreaOneData && DataItemIsInTempViewMode(dataItem));

                if (dataIsInValidMode)
                {
                    childRelationshipInfo = dataItem.ParantChildRelationshipInfo;
                    dataItem.IsHidden = !visible;
                    if (visible)
                    {
                        RemoveControlManagerColor(dataItem, title);
                        RemoveControlManagerMessage(dataItem, title);
                    }
                    else
                    {
                        AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Background, Key = title, Priority = ControlItemPriority.High });
                        AddControlManagerColor(new DataColorItem() { CausingDataItem = dataItem, Color = InfoColor.Red, ColorTarget = ControlColorTarget.Border, Key = title, Priority = ControlItemPriority.High });
                        AddControlManagerMessage(new DataMessageItem() { CausingDataItem = dataItem, Message = title + Environment.NewLine + "ترتیب اثری به داده نخواهد شد", Key = title, Priority = ControlItemPriority.High });
                    }
                }
            }


            if (childRelationshipInfo != null)
                foreach (var relCol in childRelationshipInfo.Relationship.RelationshipColumns)
                {
                    var fkProp = dataItem.GetProperty(childRelationshipInfo.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary ? relCol.FirstSideColumnID : relCol.SecondSideColumnID);
                    fkProp.IsHidden = !visible;
                }

        }



        public void ChangeRelatoinsipColumnReadonlyFromState(DP_DataRepository dataItem, RelationshipColumnControl relationshipControl, bool isReadonly, string title)
        {
            if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
            {
                var childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                childRelationshipInfo.IsReadonly = isReadonly;
                if (!childRelationshipInfo.Relationship.IsReadonly)
                {
                    relationshipControl.EditNdTypeArea.SetChildRelationshipInfo(childRelationshipInfo);
                    relationshipControl.EditNdTypeArea.DecideDataViewStaticButtons();
                    relationshipControl.EditNdTypeArea.DecideTempViewStaticButtons();
                    relationshipControl.EditNdTypeArea.DecideDataRelatedButtons();

                }
            }
            //else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
            //{
            //    childRelationshipInfo = dataItem.ParantChildRelationshipInfo;
            //}

        }

        private void SetDataItemRelationshipColumnValue(Tuple<DP_DataRepository, RelationshipColumnControl, Dictionary<int, string>> item, string titlev)
        {
            var childInfo = item.Item1.ChildRelationshipInfos.First(x => x.Relationship.ID == item.Item2.Relationship.ID);
            if (!childInfo.Relationship.IsReadonly && !childInfo.IsReadonly && !childInfo.IsHidden)
            {
                item.Item2.EditNdTypeArea.SetChildRelationshipInfo(childInfo);
                item.Item2.EditNdTypeArea.SelectFromParent(item.Item1, item.Item3);
            }
        }

        private void SetDataItemSimplePropertyValue(Tuple<DP_DataRepository, SimpleColumnControl, string> item, string title)
        {
            var property = item.Item1.GetProperty(item.Item2.Column.ID);
            if (!property.IsReadonly && !property.IsReadonly && !property.IsHidden)
                //اینجا باید بررسی بشه که نوع مقدار و پراپرتی مناسب هستند
                property.Value = item.Item3;
        }


    }
}
