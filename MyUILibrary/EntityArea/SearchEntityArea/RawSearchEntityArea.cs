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
    public class RawSearchEntityArea : I_RawSearchEntityArea
    {
        public RawSearchEntityArea()
        {
            SearchCommands = new List<I_Command>();
            SimpleColumnControls = new List<SimpleSearchColumnControl>();
        }

        public I_View_SimpleSearchEntityArea RawSearchView { set; get; }

        public List<I_Command> SearchCommands
        {
            set;
            get;
        }

        public SearchEntityAreaInitializer SearchInitializer { set; get; }

        //public event EventHandler<Arg_PackageSelected> DataPackageSelected;
        public event EventHandler<SearchPropertyArg> SearchDataDefined;






        public List<SimpleSearchColumnControl> SimpleColumnControls
        {
            set;
            get;
        }


        TableDrivedEntityDTO _SimpleEntity;
        public TableDrivedEntityDTO SimpleEntity
        {
            get
            {
                if (_SimpleEntity == null)
                {
                    if (FullEntity != null)
                        return FullEntity;
                    else
                        _SimpleEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
                }
                return _SimpleEntity;
            }
        }

        TableDrivedEntityDTO _FullEntity;
        public TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                    _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetPermissionedEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID);
             
                //if (AreaInitializer.SourceRelation != null)
                //    _FullEntity.Relationships.Clear();
                return _FullEntity;
            }
        }

        //List<UIControlPackageTree> uiControlPackageTree { set; get; }


        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), SearchInitializer.EntityID, true);
        //        return _Permission;
        //    }
        //}
        public void SetAreaInitializer(SearchEntityAreaInitializer newAreaInitializer)
        {
            SearchInitializer = newAreaInitializer;
            //if (SearchInitializer.TempEntity != null)
            //    _FullEntity = SearchInitializer.TempEntity;
            GenerateSearchView();
        }

        private void GenerateSearchView()
        {
            RawSearchView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfSearchEntityArea(GetEntityUISetting());
            ManageSimpleSearchView();

            var searchClearCommand = new SearchClearCommand(this);
            RawSearchView.AddCommand(searchClearCommand.CommandManager);
            var simpleSearchconfirmcommand = new RawSearchConfirmCommand(this);
            RawSearchView.AddCommand(simpleSearchconfirmcommand.CommandManager);
        }


        private void ManageSimpleSearchView()
        {

            foreach (var column in FullEntity.Columns.OrderBy(x => x.Position))
            {
                var propertyControl = new SimpleSearchColumnControl();
                propertyControl.Column = column;
                SimpleColumnControls.Add(propertyControl);
            }

            foreach (var columnControl in SimpleColumnControls)
            {
                columnControl.Operators = GetSimpleColumnOperators(columnControl.Column);
              //  columnControl.ControlPackage = new UIControlPackageForSimpleColumn();
                columnControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSimpleControlManagerForOneDataForm(columnControl.Column, GetColumnUISetting(columnControl.Column), false, columnControl.Operators, true, columnControl.Column.Alias);
                var operator1 = columnControl.Operators.FirstOrDefault(x => x.Operator == GetDefaultOperator(columnControl.Column));
                if (operator1 != null)
                    columnControl.ControlManager.SetOperator(operator1.Operator);
                RawSearchView.AddUIControlPackage(columnControl.ControlManager, columnControl.ControlManager.LabelControlManager);
            }
           

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
            if (column.ColumnType == Enum_ColumnType.String)
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


        public List<SearchProperty> GetSearchRepository()
        {
            List<SearchProperty> result = new List<SearchProperty>();
            foreach (var property in SimpleColumnControls)
            {
                var value = property.ControlManager.GetValue(null);
                if (PropertyHasValue(property, value))
                {

                    SearchProperty searchProperty = new SearchProperty();
                    searchProperty.ColumnID = property.Column.ID;
                    searchProperty.IsKey = property.Column.PrimaryKey;
                    searchProperty.Value = value;
                    searchProperty.Operator = property.ControlManager.GetOperator();
                    result.Add(searchProperty);
                }
            }
            return result;
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
                return string.IsNullOrEmpty(value)  || value == "false" || value == "0";
            else if (typePropertyControl is RelationCheckColumnControl || typePropertyControl is RelationCountCheckColumnControl)
                return string.IsNullOrEmpty(value) || value == "false" || value == "0";
            else
                return string.IsNullOrEmpty(value)  || value == "0";
        }



        public void OnSearchDataDefined(List<SearchProperty> searchData)
        {
            if (SearchDataDefined != null)
            {
                SearchDataDefined(this, new SearchPropertyArg() { SearchItems = searchData });
            }
        }
    }
}
