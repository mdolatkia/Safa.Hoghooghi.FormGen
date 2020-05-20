using AutoMapper;
using CommonDefinitions.UISettings;
using ModelEntites;

using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class SimpleSearchEntityArea : I_SimpleSearchEntityArea
    {
        public SimpleSearchEntityArea()
        {
            SearchCommands = new List<I_Command>();
            SimpleColumnControls = new List<SimpleSearchColumnControl>();
            RelationshipColumnControls = new List<RelationshipSearchColumnControl>();
        }
        DP_SearchRepository firstRepository { set; get; }
        public I_View_SimpleSearchEntityArea SimpleSearchView { set; get; }

        public List<I_Command> SearchCommands
        {
            set;
            get;
        }

        public SearchEntityAreaInitializer SearchInitializer { set; get; }

        //public event EventHandler<Arg_PackageSelected> DataPackageSelected;
        public event EventHandler<SearchDataArg> SearchDataDefined;
        public List<SimpleSearchColumnControl> SimpleColumnControls
        {
            set;
            get;
        }
        public List<RelationshipSearchColumnControl> RelationshipColumnControls
        {
            set;
            get;
        }

        //TableDrivedEntityDTO _SimpleEntity;
        //public TableDrivedEntityDTO SimpleEntity
        //{
        //    get
        //    {
        //        if (_SimpleEntity == null)
        //        {
        //            //if (FullEntity != null)
        //            //    return FullEntity;
        //            //else
        //                _SimpleEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
        //        }
        //        return _SimpleEntity;
        //    }
        //}

        //TableDrivedEntityDTO _FullEntity;
        //public TableDrivedEntityDTO FullEntity
        //{
        //    get
        //    {
        //        if (_FullEntity == null)
        //            _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetFullEntity(SearchInitializer.EntityID);
        //        List<ColumnDTO> removeList = new List<ColumnDTO>();
        //        foreach (var column in _FullEntity.Columns)
        //        {
        //            if (!column.PrimaryKey)
        //            {
        //                bool colAccess = true;

        //                if (Permission.ChildsPermissions.Any(y => y.SecurityObjectID == column.ID && y.GrantedActions.Any(x => x == SecurityAction.NoAccess)))
        //                    colAccess = false;

        //                if (!colAccess)
        //                    removeList.Add(column);
        //            }
        //        }
        //        foreach (var remove in removeList)
        //        {
        //            _FullEntity.Columns.Remove(remove);
        //        }
        //        //if (AreaInitializer.SourceRelation != null)
        //        //    _FullEntity.Relationships.Clear();
        //        return _FullEntity;
        //    }
        //}
        EntitySearchDTO _EntitySearch;
        public EntitySearchDTO EntitySearch
        {
            get
            {
                if (_EntitySearch == null)
                {
                    if (SearchInitializer.SearchEntityID != 0)
                        _EntitySearch = AgentUICoreMediator.GetAgentUICoreMediator.DataSearchManager.GetEntitySearch(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.SearchEntityID);
                    else
                        _EntitySearch = AgentUICoreMediator.GetAgentUICoreMediator.DataSearchManager.GetDefaultEntitySearch(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
                }
                return _EntitySearch;
            }
        }

        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.
        //        return _Permission;
        //    }
        //}
        public void SetAreaInitializer(SearchEntityAreaInitializer newAreaInitializer)
        {
            SearchInitializer = newAreaInitializer;
            //if (SearchInitializer.TempEntity != null)
            //    _SimpleEntity = SearchInitializer.TempEntity;
            GenerateSearchView();
        }

        private void GenerateSearchView()
        {
            SimpleSearchView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfSearchEntityArea(GetEntityUISetting());
            ManageSimpleSearchView();

            var searchClearCommand = new SearchClearCommand(this);
            SimpleSearchView.AddCommand(searchClearCommand.CommandManager);
            var simpleSearchconfirmcommand = new SimpleSearchConfirmCommand(this);
            SimpleSearchView.AddCommand(simpleSearchconfirmcommand.CommandManager);

        }
        //public bool ApplyPreDefinedSearch(EntityPreDefinedSearchDTO message)
        //{
        //    if (message.IsSimpleSearch)
        //    {
        //        if (message.EntitySearchID == SearchInitializer.SearchEntityID)
        //        {
        //            foreach (var item in message.SimpleColumns)
        //            {
        //                SimpleSearchColumnControl columnControl = null;
        //                if (item.EntitySearchColumnsID != 0)
        //                {
        //                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.EntitySearchColumn != null && x.EntitySearchColumn.ID == item.EntitySearchColumnsID);
        //                }
        //                else if (item.ColumnID != 0)
        //                {
        //                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == item.ColumnID);
        //                }
        //                if (columnControl != null)
        //                {
        //                    columnControl.ControlPackage.SetOperator(item.Operator);
        //                    columnControl.ControlPackage.SetValue(item.Value);
        //                }
        //                else
        //                    return false;
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public bool ShowSearchRepository(DP_SearchRepository item)
        {
            firstRepository = item;
            List<SearchProperty> listProperties = GetSearchProperties(item.Phrases);
            foreach (SearchProperty prop in listProperties)
            {
                SimpleSearchColumnControl columnControl = null;
                if (prop.SearchColumnID != 0)
                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == prop.ColumnID && x.EntitySearchColumn != null && x.EntitySearchColumn.ID == prop.SearchColumnID);
                else
                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == prop.ColumnID);
                if (columnControl == null)
                {
                    return false;
                }
            }
            foreach (SearchProperty prop in listProperties)
            {
                SimpleSearchColumnControl columnControl = null;
                if (prop.SearchColumnID != 0)
                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == prop.ColumnID && x.EntitySearchColumn != null && x.EntitySearchColumn.ID == prop.SearchColumnID);
                else
                    columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == prop.ColumnID);

                //برای عادی ها جواب میده اگر خودش کمنترل بود چی
                columnControl.ControlManager.SetValue(null, prop.Value);
                if (columnControl.ControlManager.HasOperator())
                    columnControl.ControlManager.SetOperator(prop.Operator);

            }
            return true;
        }

        private List<SearchProperty> GetSearchProperties(List<Phrase> phrase, List<SearchProperty> result = null)
        {
            if (result == null)
                result = new List<SearchProperty>();
            foreach (SearchProperty prop in phrase.Where(x => x is SearchProperty))
            {
                result.Add(prop);
            }
            foreach (LogicPhrase logic in phrase.Where(x => x is LogicPhrase))
            {
                return GetSearchProperties(logic.Phrases, result);
            }
            return result;
        }

        private void ManageSimpleSearchView()
        {
            foreach (var searchcolumn in EntitySearch.EntitySearchAllColumns.OrderBy(x => x.OrderID))
            {
                if (searchcolumn.Column == null)
                {
                    if (searchcolumn.RelationshipTail != null)
                    {
                        var propertyControl = new RelationshipSearchColumnControl();
                        propertyControl.RelationshipTail = searchcolumn.RelationshipTail;
                        propertyControl.EntitySearchColumn = searchcolumn;

                        EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
                        editEntityAreaInitializer1.EntityID = searchcolumn.RelationshipTail.TargetEntityID;
                        editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;

                        editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
                        var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
                        if (FirstSideEditEntityAreaResult.Item1 != null)
                        {
                            propertyControl.EditNdTypeArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                            propertyControl.EditNdTypeArea.SetAreaInitializer(editEntityAreaInitializer1);
                            //         propertyControl.ControlPackage = new UIControlPackageForRelationshipColumn();
                            propertyControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateRelationshipControlManagerForOneDataForm(propertyControl.EditNdTypeArea.TemporaryDisplayView, GetRelationshipUISetting(), true, propertyControl.EntitySearchColumn.Alias);
                            if (!string.IsNullOrEmpty(propertyControl.EntitySearchColumn.Tooltip))
                                propertyControl.ControlManager.LabelControlManager.SetTooltip(null, propertyControl.EntitySearchColumn.Tooltip);
                        }
                        RelationshipColumnControls.Add(propertyControl);

                        ////اینجا چیه بررسی شود چرا اینجا؟
                        //if (propertyControl.EditNdTypeArea.SimpleEntity.SearchInitially == true)
                        //    propertyControl.EditNdTypeArea.SearchViewEntityArea.SearchInitialy();

                    }

                }
                else
                {
                    var propertyControl = new SimpleSearchColumnControl();
                    propertyControl.Column = searchcolumn.Column;
                    propertyControl.EntitySearchColumn = searchcolumn;
                    propertyControl.Operators = GetSimpleColumnOperators(propertyControl.Column);
                    //        propertyControl.ControlPackage = new UIControlPackageForSimpleColumn();
                    propertyControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSimpleControlManagerForOneDataForm(propertyControl.Column, GetColumnUISetting(propertyControl.Column), false, propertyControl.Operators, true, propertyControl.EntitySearchColumn.Alias);
                    if (!string.IsNullOrEmpty(propertyControl.EntitySearchColumn.Tooltip))
                        propertyControl.ControlManager.LabelControlManager.SetTooltip(null, propertyControl.EntitySearchColumn.Tooltip);

                    if (propertyControl.Operators.Any())
                        propertyControl.ControlManager.SetOperator(propertyControl.Operators.First(x => x.Operator == GetDefaultOperator(propertyControl.Column)).Operator);
                    SimpleColumnControls.Add(propertyControl);
                }
            }

            foreach (var searchcolumn in EntitySearch.EntitySearchAllColumns.OrderBy(x => x.OrderID))
            {
                if (searchcolumn.Column == null)
                {
                    if (searchcolumn.RelationshipTail != null)
                    {
                        var relControl = RelationshipColumnControls.First(x => x.EntitySearchColumn == searchcolumn);
                        SimpleSearchView.AddView(relControl.ControlManager, relControl.ControlManager.LabelControlManager);
                    }
                }
                else
                {
                    var simpleControl = SimpleColumnControls.First(x => x.EntitySearchColumn == searchcolumn);
                    SimpleSearchView.AddUIControlPackage(simpleControl.ControlManager, simpleControl.ControlManager.LabelControlManager);
                }
            }
        }
        private RelationshipUISettingDTO GetRelationshipUISetting()
        {
            RelationshipUISettingDTO setting = new RelationshipUISettingDTO();
            setting.UIColumnsType = Enum_UIColumnsType.Half;
            return setting;
        }
        private CommonOperator GetDefaultOperator(ColumnDTO column)
        {

            if (column.ColumnType == Enum_ColumnType.String)
            {
                return CommonOperator.Contains;
            }
            else if (column.ColumnType == Enum_ColumnType.Numeric)
            {
                return CommonOperator.Equals;
            }
            return CommonOperator.Equals;
        }

        private List<SimpleSearchOperator> GetSimpleColumnOperators(ColumnDTO column)
        {
            List<SimpleSearchOperator> result = new List<SimpleSearchOperator>();
            if (column.ID != 0)
            {
                if (column.ColumnType == Enum_ColumnType.String || column.ColumnType == Enum_ColumnType.String)
                {
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.Equals, Title = "برابر" });
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.Contains, Title = "شامل" });
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.StartsWith, Title = "شروع شود با" });
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.EndsWith, Title = "تمام شود با" });
                }
                else if (column.ColumnType == Enum_ColumnType.Numeric)
                {
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.Equals, Title = "برابر" });
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.SmallerThan, Title = "کوچکتر از" });
                    result.Add(new SimpleSearchOperator() { Operator = CommonOperator.BiggerThan, Title = "بزرگتر از" });
                }
            }
            return result;
        }



        EntityUISettingDTO _EntityUISetting;
        private EntityUISettingDTO GetEntityUISetting()
        {
            if (_EntityUISetting == null)
            {
                _EntityUISetting = new EntityUISettingDTO();
                _EntityUISetting.UIColumnsCount = 4;
            }
            return _EntityUISetting;
        }

        ColumnUISettingDTO _ColumnUISetting;
        private ColumnUISettingDTO GetColumnUISetting(ColumnDTO column)
        {
            if (_ColumnUISetting == null)
            {
                _ColumnUISetting = new ColumnUISettingDTO();
                _ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Normal;
                _ColumnUISetting.UIRowsCount = 1;
            }
            return _ColumnUISetting;
        }





        public void ClearSearchData()
        {



        }




        //public List<PreDefinedSearchColumns> GetSearchColumns()
        //{
        //    List<PreDefinedSearchColumns> result = new List<PreDefinedSearchColumns>();
        //    foreach (var property in SimpleColumnControls)
        //    {
        //        var value = property.ControlManager.GetValue();
        //        if (PropertyHasValue(property, value))
        //        {
        //            PreDefinedSearchColumns item = new PreDefinedSearchColumns();
        //            if (property.EntitySearchColumn != null)
        //                item.EntitySearchColumnsID = property.EntitySearchColumn.ID;
        //            else
        //                item.ColumnID = property.Column.ID;
        //            item.Value = value;
        //            item.Operator = property.ControlManager.GetOperator();
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}
        public DP_SearchRepository GetSearchRepository()
        {
            if (firstRepository == null)
                firstRepository = new DP_SearchRepository(SearchInitializer.EntityID);
            else
                firstRepository.Phrases.Clear();
            firstRepository.Title = SearchInitializer.Title;
            firstRepository.IsSimpleSearch = true;
            firstRepository.EntitySearchID = EntitySearch == null ? 0 : EntitySearch.ID;
            foreach (var property in SimpleColumnControls)
            {
                var value = property.ControlManager.GetValue(null);
                if (PropertyHasValue(property, value))
                {
                    LogicPhrase logic = null;
                    if (property.EntitySearchColumn.RelationshipTail == null)
                        logic = firstRepository;
                    else
                        logic = AgentHelper.GetOrCreateSearchRepositoryFromRelationshipTail(firstRepository, property.EntitySearchColumn.RelationshipTail, null);

                    SearchProperty searchProperty = new SearchProperty();
                    searchProperty.SearchColumnID = property.EntitySearchColumn != null ? property.EntitySearchColumn.ID : 0;
                    searchProperty.ColumnID = property.Column.ID;
                    searchProperty.IsKey = property.Column.PrimaryKey;
                    searchProperty.Value = value;
                    searchProperty.Operator = property.ControlManager.GetOperator();
                    logic.Phrases.Add(searchProperty);
                }
            }

            foreach (var relControl in RelationshipColumnControls)
            {

                if (relControl.EditNdTypeArea.GetDataList().Any())
                {
                    var data = relControl.EditNdTypeArea.GetDataList().First();
                    LogicPhrase logic = AgentHelper.GetOrCreateSearchRepositoryFromRelationshipTail(firstRepository, relControl.EntitySearchColumn.RelationshipTail, data);

                    //foreach (var property in data.KeyProperties)
                    //{
                    //    SearchProperty searchProperty = new SearchProperty();
                    //    searchProperty.SearchColumnID = relControl.EntitySearchColumn != null ? relControl.EntitySearchColumn.ID : 0;
                    //    searchProperty.ColumnID = property.Column.ID;
                    //    searchProperty.IsKey = property.Column.PrimaryKey;
                    //    searchProperty.Value = property.Value;
                    //    searchProperty.Operator = CommonOperator.Equals;
                    //    logic.Phrases.Add(searchProperty);
                    //}
                }
                else
                {
                    var text = relControl.EditNdTypeArea.TemporaryDisplayView.GetSearchText();
                    if (!string.IsNullOrEmpty(text))
                    {
                        var entitySearch = relControl.EditNdTypeArea.SearchViewEntityArea.SearchEntityArea.SimpleSearchEntityArea.EntitySearch;
                        var quickSearchLogic = GetQuickSearchLogicPhrase(text, entitySearch);

                        firstRepository.Phrases.Add(quickSearchLogic);
                    }
                }
            }


            //foreach (var relControl in RelationshipColumnControls)
            //{

            //    else
            //    {
            //        var value = relControl.EditNdTypeArea.TemporaryDisplayView.GetSearchText();
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            SearchProperty searchProperty = new SearchProperty();
            //            searchProperty.SearchColumnID = relControl.EntitySearchColumn != null ? relControl.EntitySearchColumn.ID : 0;
            //            searchProperty.Value = value;
            //            searchProperty.Operator = CommonOperator.Contains;
            //            logic.Phrases.Add(searchProperty);
            //        }
            //    }

            //}
            return firstRepository;
        }

        public LogicPhrase GetQuickSearchLogicPhrase(string text, EntitySearchDTO entitySearch)
        {
            LogicPhrase quickSearchLogic = new LogicPhrase();
            quickSearchLogic.AndOrType = AndORType.Or;
            foreach (var item in entitySearch.EntitySearchAllColumns)
            {
                if (item.ColumnID != 0)
                {
                    SearchProperty searchProperty = new SearchProperty();
                    searchProperty.SearchColumnID = item.ID;
                    searchProperty.ColumnID = item.ColumnID;
                    searchProperty.IsKey = item.Column.PrimaryKey;
                    searchProperty.Value = text;
                    LogicPhrase logic = null;
                    if (item.RelationshipTail == null)
                        logic = quickSearchLogic;
                    else
                    {
                        logic = AgentHelper.GetOrCreateSearchRepositoryFromRelationshipTail(quickSearchLogic, item.RelationshipTail, null);
                        logic.AndOrType = AndORType.Or;
                    }

                    int n;
                    var isNumeric = int.TryParse(text, out n);
                    DateTime a;
                    var isDateTime = DateTime.TryParse(text, out a);

                    if (item.Column.ColumnType == Enum_ColumnType.Numeric)
                    {
                        if (!isNumeric)
                            continue;
                        searchProperty.Operator = CommonOperator.Equals;
                    }
                    else if (item.Column.ColumnType == Enum_ColumnType.Date)
                        continue;
                    else if (item.Column.ColumnType == Enum_ColumnType.Boolean)
                        continue;
                    else if (item.Column.ColumnType == Enum_ColumnType.String)
                        searchProperty.Operator = CommonOperator.Contains;
                    else
                        continue;
                    logic.Phrases.Add(searchProperty);
                }
            }
            return quickSearchLogic;
        }

        private bool PropertyHasValue(SimpleSearchColumnControl property, object value)
        {
            return value != null && !string.IsNullOrEmpty(value.ToString()) && value.ToString().ToLower() != "0";
        }

        //private bool PropertyHasValue(SearchProperty item)
        //{
        //    return !string.IsNullOrEmpty(item.Value)  && item.Value.ToLower() != "0";
        //}

        private bool SearchValueIsEmpty(SimpleSearchColumnControl typePropertyControl, string value)
        {
            if (typePropertyControl is NullColumnControl)
                return string.IsNullOrEmpty(value) || value == "false" || value == "0";
            else if (typePropertyControl is RelationCheckColumnControl || typePropertyControl is RelationCountCheckColumnControl)
                return string.IsNullOrEmpty(value) || value == "false" || value == "0";
            else
                return string.IsNullOrEmpty(value) || value == "0";
        }



        public void OnSearchDataDefined(DP_SearchRepository searchData)
        {
            if (SearchDataDefined != null)
            {
                SearchDataDefined(this, new SearchDataArg() { SearchItems = searchData });
            }
        }


    }
}
