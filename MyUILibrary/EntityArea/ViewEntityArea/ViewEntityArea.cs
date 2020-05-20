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
    public class ViewEntityArea : I_ViewEntityArea
    {
        public ViewEntityArea()
        {
            ViewCommands = new List<I_ViewAreaCommand>();
            ViewColumnControls = new List<SimpleViewColumnControl>();
        }


        //public I_EditEntityArea SourceEditEntityArea
        //{
        //    set;
        //    get;
        //}


        public List<I_ViewAreaCommand> ViewCommands
        {
            set;
            get;
        }
        public AgentUICoreMediator AgentUICoreMediator
        {
            get
            {
                return AgentUICoreMediator.GetAgentUICoreMediator;
            }
        }
        public ViewEntityAreaInitializer ViewInitializer { set; get; }

        //internal static void ShowReport(List<DP_DataRepository> resultDataItems, bool v)
        //{

        //}

        //public void LoadTemplate(ViewEntityAreaInitializer initParam)
        //{
        //    ViewInitializer = initParam;

        //    //SearchView = AgentUICoreMediator.UIManager.GenerateViewOfSearchEntityArea(UISettingHelper.DefaultPackageAreaSetting);




        //    //SearchViewArea.AddSearchView(SearchView);
        //    //SearchViewArea.AddViewView(ViewView);


        //    //هنوز مشخص نیست که چه موقع استفاده میشود TemporarySearchView.که یک ویو ساده از نمایش تلفیق دو ویو اول است SearchViewArea و ViewView و SearchView.داریم View چهار تا


        //    //////TemporarySearchView = AgentUICoreMediator.UIManager.GenerateTemporaryLinkUI(TemporaryLinkType.SerachView);
        //    //////TemporarySearchView.TemporaryDisplayViewRequested += TemporarySearchView_TemporaryDisplayViewRequested;


        //    //CreateDefaultData();

        //}

        //void ViewView_CommandExecuted(object sender, Arg_CommandExecuted e)
        //{
        //    (e.Command as I_ViewAreaCommand).Execute(this);
        //}

        //void SearchView_CommandExecuted(object sender, Arg_CommandExecuted e)
        //{
        //    (e.Command as I_SearchAreaCommand).Execute(this);
        //}

        //void ViewView_DataSelected(object sender, DataSelectedEventArg e)
        //{
        //    ViewInitializer.SourceEditArea.DataSelected(e.DataItem, this);
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(ViewView);
        //}
        //public void CreateDefaultData()
        //{

        //    //bool creatDefault = false;
        //    //if (AreaInitializer.SourceRelation == null && AgentHelper.GetDataEntryMode(AreaInitializer. == DataMode.One)
        //    //    creatDefault = true;
        //    //if (AreaInitializer.SourceRelation != null && AgentHelper.GetDataEntryMode(AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer. != DataMode.Multiple)
        //    //    creatDefault = true;
        //    //if (force || creatDefault)
        //    //{


        //    DP_DataRepository newData = AgentHelper.CreateAreaInitializerNewData(ViewInitializer, true);
        //    if (newData != null)
        //    {
        //        //ViewInitializer.SearchData = newData;
        //    }


        //    //}

        //}

        //private void ManageTemporaryView()
        //{

        //}

        //////public void TemporarySearchView_TemporaryDisplayViewRequested(object sender, Arg_TemporaryDisplayViewRequested e)
        //////{
        //////    ShowTemporarySearchView(null, null);
        //////}
        //public void ShowTemporarySearchView()
        //{
        //    AgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(SearchViewArea, ViewInitializer.SearchEntity.Alias);
        //}

        public void SetAreaInitializer(ViewEntityAreaInitializer initParam)
        {
            ViewInitializer = initParam;
            //if (initParam.TempEntity != null)
            //    _FullEntity = initParam.TempEntity;

            GenerateView();
        }

        private void GenerateView()
        {
            ViewView = AgentUICoreMediator.UIManager.GenerateViewOfViewEntityArea();
            ViewView.MultipleSelection = ViewInitializer.MultipleSelection;
            var selectcommand = new SelectCommand(this);
            ViewView.AddCommand(selectcommand.CommandManager);
            ViewView.DataContainerLoaded += ViewView_DataContainerLoaded;
            ViewView.ItemDoubleClicked += ViewView_ItemDoubleClicked;

            //  ViewView.CommandExecuted += ViewView_CommandExecuted;
            //ViewView.DataSelected += ViewView_DataSelected;

            //if (ViewInitializer.ViewEntity.Columns.Count == 0)
            //{

            //    var result = GetFullEntity(ViewInitializer.ViewEntity.ID);
            //    ViewInitializer.ViewEntity = result.Entity;
            //    ViewInitializer.Permissoins = result.Permissoins;
            //    ViewInitializer.UICompositions = result.UICompositions;

            //}

            ManageViewView();
        }

        private void ViewView_ItemDoubleClicked(object sender, DataContainerLoadedArg e)
        {
            if (e.DataItem != null && e.DataItem is DP_DataView)
            {
                OnDataSelected(new List<DP_DataView>() { e.DataItem as DP_DataView });
            }
        }

        private void ViewView_DataContainerLoaded(object sender, DataContainerLoadedArg e)
        {
            //ShowTypePropertyData(e.DataItem as DP_DataView);
        }

        private void ManageViewView()
        {
            foreach (var column in EntityListView.EntityListViewAllColumns.OrderBy(x => x.OrderID))
            {
                var propertyControl = new SimpleViewColumnControl() { ListViewColumn = column };
                propertyControl.RelativeColumnName = column.RelativeColumnName;

                if (string.IsNullOrEmpty(column.Alias))
                    propertyControl.Alias = column.Column.Alias;
                else
                    propertyControl.Alias = column.Alias;
             //   propertyControl.ControlPackage = new UIControlPackageForSimpleColumn();
           //     propertyControl.IsPermanentReadOnly = true;
                propertyControl.ControlManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateSimpleControlManagerForMultipleDataForm(column.Column, GetColumnUISetting(column.Column), false,true, propertyControl.Alias);
                //      if (propertyControl.IsPermanentReadOnly)
                if (!string.IsNullOrEmpty(column.Tooltip))
                {
                    propertyControl.ControlManager.LabelControlManager.SetTooltip(null, column.Tooltip);
                }
                propertyControl.ControlManager.SetReadonly(true);

                ViewColumnControls.Add(propertyControl);


            }
            foreach (var columnControl in ViewColumnControls)
            {

                columnControl.Visited = true;
                var simpleColumn = (columnControl as SimpleViewColumnControl);
                ViewView.AddUIControlPackage(simpleColumn.ControlManager, simpleColumn.ControlManager.LabelControlManager);
            }
        }

        private void GenerateViewRelationshipColumns(RelationshipDTO parentRelationship, string parentRelatoinshipIds, TableDrivedEntityDTO entity, AssignedPermissionDTO permissoins, List<EntityUICompositionDTO> uICompositions)
        {

            //foreach (var column in entity.Columns.OrderBy(x => x.Position))
            //{

            //    //////var propertyControl = new SimpleViewColumnControl() { Column = column };
            //    //////if (parentRelationship != null)
            //    //////{
            //    //////    propertyControl.RelationshipColumnName = parentRelatoinshipIds + "#" + entity.ID + "_" + column.ID;
            //    //////    propertyControl.Alias = entity.Alias + "-" + propertyControl.Column.Alias;
            //    //////}
            //    ////////AgentHelper.SetPropertyTitle(propertyControl);
            //    ////////propertyControl.ColumnSetting = new ColumnSetting();
            //    //////propertyControl.IsPermanentReadOnly = true;
            //    //////propertyControl.MultipleDataControlPackage = new DataDependentControlPackage();
            //    ////////////propertyControl.MultipleDataControlPackage.ControlManager= AgentUICoreMediator.UIManager.GenerateMultipleDataDependentControl(column, propertyControl.ColumnSetting);


            //    //////ViewColumnControls.Add(propertyControl);


            //    //////ViewView.AddUIControlPackage(propertyControl.MultipleDataControlPackage , propertyControl.Alias, Temp.InfoColor.Black, "");

            //}

            //////foreach (var relationship in entity.Relationships
            //////    .Where(x => x.ViewEnabled == true))
            //////{

            //////    //////var propertyControl = new ViewColumnControl();
            //////    //////var column = ViewInitializer.ViewEntity.Columns.First(x => x.ID == relationship.FirstSideColumns.First().ID);
            //////    //////propertyControl.Column = column;
            //////    //////propertyControl.Relationship = relationship;
            //////    //////AgentHelper.SetPropertyTitle(propertyControl);

            //////    //////propertyControl.ColumnSetting = new ColumnSetting();
            //////    //////propertyControl.ColumnSetting.IsReadOnly = true;
            //////    //////propertyControl.ControlPackage = AgentUICoreMediator.UIManager.GenerateMultipleDataDependentControl(propertyControl.Column, propertyControl.ColumnSetting);


            //////    //////ViewColumnControls.Add(propertyControl);


            //////    //////ViewView.AddUIControlPackage(propertyControl.ControlPackage as DataDependentControlPackage, propertyControl.Alias, Temp.InfoColor.Black, "");
            //////    if (relationship.TypeEnum != Enum_RelationshipType.OneToMany)
            //////    {
            //////        var relationentity = GetFullEntity(relationship.EntityID2);
            //////        GenerateViewRelationshipColumns(relationship, parentRelatoinshipIds + "_" + relationship.ID, relationentity.Entity, relationentity.Permissoins, relationentity.UICompositions);
            //////    }
            //////    else
            //////    {
            //////        //ستون الکی ایجاد شده
            //////        var column = new ColumnDTO();
            //////        var propertyControl = new SimpleViewColumnControl() { Column = column };

            //////        propertyControl.RelationshipColumnName = parentRelatoinshipIds + "_" + relationship.ID + "#" + relationship.EntityID2;
            //////        propertyControl.Alias = relationship.Entity2;

            //////        //AgentHelper.SetPropertyTitle(propertyControl);
            //////        propertyControl.ColumnSetting = new ColumnSetting();
            //////        propertyControl.IsPermanentReadOnly = true;
            //////        propertyControl.ControlPackage = AgentUICoreMediator.UIManager.GenerateMultipleDataDependentControl(column, propertyControl.ColumnSetting);


            //////        ViewColumnControls.Add(propertyControl);


            //////        ViewView.AddUIControlPackage(propertyControl.ControlPackage as DataDependentControlPackage, propertyControl.Alias, Temp.InfoColor.Black, "");
            //////    }
            //////}
        }


        //دوتا فانکشن یکی بشن
        //private void GenerateViewRelationshipColumns(RelationshipDTO parentRelationship, string parentRelatoinshipIds)
        //{
        //    var entity = GetEntity(parentRelationship.EntityID2).Entity;
        //    foreach (var column in entity.Columns.OrderBy(x => x.Position).Where(x => x.ViewEnabled == true))
        //    {

        //        var propertyControl = new ViewColumnControl() { Column = column };
        //        propertyControl.RelationshipColumnName = parentRelatoinshipIds + "#" + entity.ID + "_" + column.ID;
        //        AgentHelper.SetPropertyTitle(propertyControl);
        //        propertyControl.ColumnSetting = new ColumnSetting();
        //        propertyControl.ColumnSetting.IsReadOnly = true;
        //        propertyControl.ControlPackage = AgentUICoreMediator.UIManager.GenerateMultipleDataDependentControl(column, propertyControl.ColumnSetting);


        //        ViewColumnControls.Add(propertyControl);


        //        ViewView.AddUIControlPackage(propertyControl.ControlPackage as DataDependentControlPackage, propertyControl.Alias, Temp.InfoColor.Black, "");

        //    }

        //    foreach (var relationship in entity.Relationships
        //        .Where(x => x.ViewEnabled == true && x.TypeEnum != Enum_RelationshipType.OneToMany))
        //    {

        //        var propertyControl = new ViewColumnControl();
        //        var column = entity.Columns.First(x => x.ID == relationship.FirstSideColumns.First().ID);
        //        propertyControl.Column = column;
        //        propertyControl.Relationship = relationship;
        //        AgentHelper.SetPropertyTitle(propertyControl);

        //        propertyControl.ColumnSetting = new ColumnSetting();
        //        propertyControl.ColumnSetting.IsReadOnly = true;
        //        propertyControl.ControlPackage = AgentUICoreMediator.UIManager.GenerateMultipleDataDependentControl(propertyControl.Column, propertyControl.ColumnSetting);


        //        ViewColumnControls.Add(propertyControl);


        //        ViewView.AddUIControlPackage(propertyControl.ControlPackage as DataDependentControlPackage, propertyControl.Alias, Temp.InfoColor.Black, "");

        //        GenerateViewRelationshipColumns(relationship, parentRelatoinshipIds + (parentRelatoinshipIds == "" ? "" : "_") + relationship.ID);
        //    }
        //}
        //private DP_EntityResult GetEntity(int entityID2)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    //if (directionMode == DirectionMode.Direct)
        //    //{
        //    //    //if (intracionMode == IntracionMode.Create || intracionMode == IntracionMode.CreateSelect)
        //    return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, true, true,false);
        //    //    //else
        //    //    //    return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, false, false);
        //    //}
        //    //else
        //    //{
        //    //return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, false, false);
        //    //}
        //}
        //List<SearchColumnControl> SearchColumnControls
        //{
        //    set;
        //    get;
        //}
        public List<SimpleViewColumnControl> ViewColumnControls { set; get; }
        //public List<RelationshipViewColumnControl> RelationshipViewColumnControls { set; get; }






        //////public DataManager.DataPackage.DP_Package SearchDataPackage
        //////{
        //////    set;
        //////    get;
        //////}

        //////public List<DataManager.DataPackage.DP_Package> ViewDataPackages
        //////{
        //////    set;
        //////    get;
        //////}




        //public void SearchCommandExecuted(I_SearchAreaCommand command)
        //{
        //    command.Execute(this);
        //}

        //public void ViewCommandExecuted(I_ViewAreaCommand command)
        //{
        //    command.Execute(this);
        //}


        //public void OnDataPackagesSelected(List<TableDrivedEntityDTO> packages)
        //{
        //    if (DataPackageSelected != null)
        //        DataPackageSelected(this, new Arg_PackageSelected() { Packages = packages, SourceEditEntityArea = ViewInitializer.SourceEditArea });
        //}

        public event EventHandler<Arg_PackageSelected> DataPackageSelected;
        public event EventHandler<DataSelectedEventArg> DataSelected;





        //public I_View_SearchEntityArea SearchView { set; get; }

        public I_View_ViewEntityArea ViewView { set; get; }

        //////public IAG_View_TemporaryView TemporarySearchView { set; get; }

        //public I_View_SearchViewArea SearchViewArea { set; get; }




        //public AG_View_SearchViewEntityArea View
        //{
        //    set;
        //    get;
        //}





        //public void ClearData()
        //{

        //    //if ( ViewInitializer.SearchData.Count > 0)
        //    //    ViewInitializer.SearchData.Clear();;

        //    // editArea.ClearUIData();


        //    //var newData = CreateDefaultData();

        //    foreach (var property in ViewInitializer.SearchEntity.Columns)
        //    {
        //        var typePropertyControls = SearchColumnControls.Where(x => x.Column.ID == property.ID);
        //        foreach (var typePropertyControl in typePropertyControls)
        //            if (typePropertyControl != null)
        //            {
        //                ShowTypePropertyControlValue(null, typePropertyControl, "");
        //            }
        //    }

        //}
        private EntityUISettingDTO GetEntityUISetting()
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
        private ColumnUISettingDTO GetColumnUISetting(ColumnDTO column)
        {
            if (UICompositions != null && UICompositions.ColumnItems != null
                && UICompositions.ColumnItems.Any(x => x.ColumnID == column.ID))
            {
                var setting = UICompositions.ColumnItems.First(x => x.ColumnID == column.ID);
                if (setting == null)
                {
                    setting = new ColumnUISettingDTO();
                    setting.UIColumnsType = Enum_UIColumnsType.Normal;
                    setting.UIRowsCount = 1;
                    setting.ColumnID = column.ID;
                    UICompositions.ColumnItems.Add(setting);
                }
                return setting;
            }
            return null;
        }
        public List<DP_DataView> GetSelectedData()
        {
            // ShowTypePropertyData(ViewInitializer.ViewData);

            List<DP_DataView> selectedData = new List<DP_DataView>();
            var viewSelectedData = ViewView.GetSelectedData();


            if (ViewInitializer.MultipleSelection == false)
            {
                if (viewSelectedData.Count > 0)
                {
                    selectedData.Add(viewSelectedData.First() as DP_DataView);
                }
            }
            else
            //if (packageArea.ViewTemplate.SelectCount > 1)
            {
                selectedData = viewSelectedData.Cast<DP_DataView>().ToList();
            }
            return selectedData;
        }
        public void ClearSelectedData()
        {
            ViewView.SetSelectedData(null);
        }
        public void AddData(List<DP_DataView> data, bool show)
        {
            if (data == null || data.Count == 0)
            {
                //اینجا باید اگر کلید پرشده بود دیتا را بگیرد
                //throw new Exception("sdf");

            }
            ViewInitializer.ViewData.Clear();
            ViewView.RemoveDataContainers();
            ViewInitializer.ViewData.AddRange(data);

            if (show)
                ShowData1(data);
        }

        public void ShowData1(List<DP_DataView> specificDatas)
        {
            if (specificDatas == null)
                throw new Exception("sdf");
            foreach (var item in specificDatas)
            {
                ViewView.AddDataContainer(item);
                ShowTypePropertyData(item);
            }

        }
        private void ShowTypePropertyData(DP_DataView dataRepository)
        {

            //foreach (var viewItem in dataRepository.DataViewItems)
            //{
            foreach (var property in dataRepository.Properties)
            {
                var columnControl = ViewColumnControls.FirstOrDefault(x => x.RelativeColumnName == property.RelativeName);
                if (columnControl != null)
                {
                    columnControl.ControlManager.SetValue(dataRepository, property.Value);
                }
            }
            //}
            //else if (property.Name.Contains("#"))
            //{
            //    foreach (var typePropertyControl in ViewColumnControls.Where(x => x.RelationshipColumnName == property.Name))
            //        if (typePropertyControl != null)
            //        {
            //            //if (!typePropertyControl.ControlPackage.IsDataDependentControl)

            //            ShowTypePropertyControlValue(dataRepository, typePropertyControl, property.Value);

            //        }
            //}

        }
        private void ShowTypePropertyData(List<DP_DataView> specificDate)
        {
            foreach (var dataRepository in specificDate)
            {
                //foreach (var viewItem in dataRepository.DataViewItems)
                //{
                foreach (var property in dataRepository.Properties)
                {
                    var columnControl = ViewColumnControls.FirstOrDefault(x => x.RelativeColumnName == property.RelativeName);
                    if (columnControl != null)
                    {
                        columnControl.ControlManager.SetValue(dataRepository, property.Value);
                    }
                }
                //}
                //else if (property.Name.Contains("#"))
                //{
                //    foreach (var typePropertyControl in ViewColumnControls.Where(x => x.RelationshipColumnName == property.Name))
                //        if (typePropertyControl != null)
                //        {
                //            //if (!typePropertyControl.ControlPackage.IsDataDependentControl)

                //            ShowTypePropertyControlValue(dataRepository, typePropertyControl, property.Value);

                //        }
                //}
            }
        }


        //private bool ShowTypePropertyControlValue(DP_DataRepository dataItem, SimpleViewColumnControl typePropertyControl, string value)
        //{
        //    //if (typePropertyControl is RelationSourceControl)
        //    //{
        //    //    return true;
        //    //    //////var relationSourceControl = (typePropertyControl as RelationSourceControl);
        //    //    //////return relationSourceControl.EditNdTypeArea.ShowTypePorpertyValue(dataRepository, AgentHelper.GetRelationOperand(relationSourceControl.Relation, relationSourceControl.RelationSide == Enum_DP_RelationSide.FirstSide ? Enum_DP_RelationSide.SecondSide : Enum_DP_RelationSide.FirstSide), value);
        //    //    // return FetchTypePropertyRelationSourceControl(typePropertyControl as RelationSourceControl);
        //    //}
        //    //else
        //    //    return DataView.ShowTypePropertyValue(dataRepository, typePropertyControl, value);
        //    //ColumnSetting columnSetting = new ColumnSetting();


        //    //////if (typePropertyControl.ControlPackage is DataDependentControlPackage)
        //    //////    return ViewView.ShowMultipleDateItemControlValue(dataItem, typePropertyControl.ControlPackage as DataDependentControlPackage, value);
        //    //////else
        //    //////    return typePropertyControl.ControlPackage.BaseControlHelper.SetValue(typePropertyControl.ControlPackage, value, columnSetting);
        //    return false;
        //}


        //public void UpdateSearchData()
        //{

        //    foreach (var typeProperty in ViewInitializer.SearchData.Properties)
        //    {
        //        //if (DataRepository.DataTypes.Contains(item))
        //        //{

        //        foreach (var typePropertyControl in SearchColumnControls.Where(x => x.Column.ID == typeProperty.ColumnID))
        //        {
        //            if (typePropertyControl != null)
        //            {
        //                typeProperty.Value = FetchTypePropertyControlValue(ViewInitializer.SearchData, typePropertyControl);
        //            }
        //        }
        //    }
        //    //}

        //}
        TableDrivedEntityDTO _FullEntity;
        private TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                    _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetFullEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), ViewInitializer.EntityID);
                return _FullEntity;
            }
        }


        EntityListViewDTO _EntityListView;
        public EntityListViewDTO EntityListView
        {
            get
            {
                if (_EntityListView == null)
                {
                    if (ViewInitializer.EntityListViewID != 0)
                        _EntityListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), ViewInitializer.EntityListViewID);
                    else
                        _EntityListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetDefaultEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), ViewInitializer.EntityID);
                }


                return _EntityListView;
            }
        }
        //private bool CheckRelationshipTailPermission(EntityRelationshipTailDTO relationshipTail, bool first = true)
        //{
        //    if (first)
        //    {
        //        var entityPermission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), relationshipTail.Relationship.EntityID2, false);

        //        if (entityPermission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
        //            return false;
        //    }

        //    var relationshipPermission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), relationshipTail.Relationship.ID, false);

        //    if (relationshipPermission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
        //        return false;



        //    if (relationshipTail.ChildTail != null)
        //        return CheckRelationshipTailPermission(relationshipTail.ChildTail, false);
        //    else
        //        return true;
        //}
        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), ViewInitializer.EntityID, true);
        //        return _Permission;
        //    }
        //}

        bool UICompositionsCalled;
        EntityUICompositionCompositeDTO _UICompositions;
        public EntityUICompositionCompositeDTO UICompositions
        {
            get
            {
                if (_UICompositions == null && !UICompositionsCalled)
                {
                    UICompositionsCalled = true;
                    _UICompositions = AgentUICoreMediator.GetAgentUICoreMediator.entityUICompositionService.GetEntityUICompositionTree(ViewInitializer.EntityID);
                }
                return _UICompositions;
            }
        }

        private string FetchTypePropertyControlValue(DP_DataRepository dataRepository, SimpleViewColumnControl typePropertyControl)
        {
            return null;
            //////return typePropertyControl.ControlPackage.BaseControlHelper.GetValue(typePropertyControl.ControlPackage);
        }



        //private DP_EntityResult GetFullEntity(int entityID2)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    return AgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, true, true, false, false, false, false);
        //}

        public void OnDataSelected(List<DP_DataView> dataItems)
        {

            if (DataSelected != null)
                DataSelected(this, new EntityArea.DataSelectedEventArg() { DataItem = dataItems });

        }
    }
}
