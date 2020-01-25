

using CommonDefinitions.UISettings;
using ModelEntites;

using MyUILibrary;
using MyUILibrary.EntityArea.Commands;
using MyUILibraryInterfaces.DataViewArea;
using MyUILibraryInterfaces.EditEntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibraryInterfaces.DataReportArea;
using System.Collections.Specialized;
using MyUILibrary.Temp;

namespace MyUILibrary.EntityArea
{
    public class EditEntityAreaOneData : BaseEditEntityArea, I_EditEntityAreaOneData
    {

        //StateHelper stateHelper = new StateHelper();
        public List<UIControlPackageTree> UICompositionContainers { set; get; }
        public event EventHandler<EditAreaDataItemArg> DataItemSelected;
        public EditEntityAreaOneData(TableDrivedEntityDTO simpleEntity) : base(simpleEntity)
        {
            UICompositionContainers = new List<UIControlPackageTree>();
            //SimpleColumnControls = new List<SimpleColumnControl>();
            //RelationshipColumnControls = new List<RelationshipColumnControl>();
        }


        public override bool AddData(DP_DataRepository data, bool showDataInDataView)
        {
            if (base.BaseAddData(data))
            {
                if (showDataInDataView)
                {
                    if (ShowDataInDataView(data))
                        return true;
                    else
                    {
                        //بعدا بررسی شود اینجا زیاد مفهوم نیست
                        bool createDefault = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                                AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
                        ClearData(createDefault);
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
            //ManageDataSecurity();
        }
        public void OnDataItemSelected(DP_DataRepository dP_DataRepository)
        {
            if (DataItemSelected != null)
                DataItemSelected(this, new EditAreaDataItemArg() { DataItem = dP_DataRepository });
        }
        public void ShowDataFromExternalSource(DP_DataView dataRepository = null)
        {
            bool dataView = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                 AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
            if (dataRepository != null)
            {
                if (!dataRepository.KeyProperties.Any())
                    throw new Exception("asdad");
                if (dataView)
                {
                    var result = AreaInitializer.EditAreaDataManager.SearchDataForEditFromExternalSource(AreaInitializer.EntityID, dataRepository, this);
                    if (result != null)
                    {
                        var addResult = AddData(result, true);
                        if (!addResult)
                            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده و یا داده های وابسته", dataRepository.ViewInfo, Temp.InfoColor.Red);
                    }
                    else
                    {
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", dataRepository.ViewInfo, Temp.InfoColor.Red);
                    }
                }
                else
                {
                    var viewData = AreaInitializer.EditAreaDataManager.SearchDataForViewFromExternalSource(AreaInitializer.EntityID, dataRepository, this);
                    if (viewData != null)
                    {
                        var result = AreaInitializer.EditAreaDataManager.ConvertDP_DataViewToDP_DataRepository(viewData, this);
                        var addResult = AddData(result, false);
                        if (!addResult)
                            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده و یا داده های وابسته", dataRepository.ViewInfo, Temp.InfoColor.Red);
                    }
                    else
                    {
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", dataRepository.ViewInfo, Temp.InfoColor.Red);
                    }
                }
            }
            else
            {
                if (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                       AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                    CreateDefaultData();
            }
        }

        public void GenerateUIComposition(UIControlPackageTree parentUIControlPackage, List<EntityUICompositionDTO> UICompositions)
        {
            List<UIControlPackageTree> parentList = null;
            I_View_GridContainer container;
            if (parentUIControlPackage == null)
            {
                container = SpecializedDataView;
                parentList = UICompositionContainers;
            }
            else
            {
                parentList = parentUIControlPackage.ChildItems;
                container = parentUIControlPackage.Item as I_View_GridContainer;
            }

            foreach (var uiCompositionItem in UICompositions.OrderBy(x => x.Position))
            {
                UIControlPackageTree item = new UIControlPackageTree();
                //if (parentUIControlPackage == null)
                //{
                //    item.UIComposition = uiCompositionItem;
                //    item.Container = DataView;

                //}+
                if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Entity)
                {
                    item.Item = DataView;
                    parentList.Add(item);
                    GenerateUIComposition(item, uiCompositionItem.ChildItems);

                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                {
                    var groupSetting = uiCompositionItem.GroupUISetting;
                    if (groupSetting == null)
                    {
                        groupSetting = new GroupUISettingDTO();
                        groupSetting.Expander = true;
                        groupSetting.InternalColumnsCount = GetEntityUISetting().UIColumnsCount;
                        groupSetting.UIColumnsType = Enum_UIColumnsType.Full;
                    }
                    var groupItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateGroup(groupSetting);
                    item.Item = groupItem;
                    GenerateUIComposition(item, uiCompositionItem.ChildItems);
                    if (item.ChildItems.Count != 0)
                    {
                        container.AddGroup(groupItem, uiCompositionItem.Title, groupSetting);
                        parentList.Add(item);
                    }

                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabControl)
                {
                    var tabgroupSetting = uiCompositionItem.TabGroupUISetting;
                    if (tabgroupSetting == null)
                    {
                        tabgroupSetting = new TabGroupUISettingDTO();
                        tabgroupSetting.Expander = true;
                        tabgroupSetting.UIColumnsType = Enum_UIColumnsType.Full;
                    }
                    var tabGroupContainer = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabGroup(tabgroupSetting);
                    item.Item = tabGroupContainer;
                    GenerateUIComposition(item, uiCompositionItem.ChildItems);
                    if (item.ChildItems.Count != 0)
                    {
                        container.AddTabGroup(tabGroupContainer, uiCompositionItem.Title, tabgroupSetting);
                        parentList.Add(item);
                    }

                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabPage)
                {
                    var tabpageSetting = uiCompositionItem.TabPageUISetting;
                    if (tabpageSetting == null)
                    {
                        tabpageSetting = new TabPageUISettingDTO();
                        tabpageSetting.InternalColumnsCount = GetEntityUISetting().UIColumnsCount;
                    }
                    var groupItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateTabPage(tabpageSetting);
                    item.Item = groupItem;
                    GenerateUIComposition(item, uiCompositionItem.ChildItems);
                    if (item.ChildItems.Count != 0)
                    {
                        var parentTabGroupContainer = parentUIControlPackage.Item as I_TabGroupContainer;
                        parentTabGroupContainer.AddTabPage(groupItem, uiCompositionItem.Title, tabpageSetting, groupItem.HasHeader);
                        parentList.Add(item);


                    }
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Column)
                {
                    var columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        item.Item = columnControl.ControlManager;
                        container.AddUIControlPackage(columnControl.SimpleControlManager, columnControl.ControlManager.LabelControlManager);
                        parentList.Add(item);
                        columnControl.Visited = true;
                    }
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Relationship)
                {
                    var columnControl = RelationshipColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        if (UICompositions.Count == 1 && parentUIControlPackage.Item is I_TabPageContainer)
                        {
                            (columnControl.ControlManager as I_RelationshipControlManager).TabPageContainer = parentUIControlPackage.Item as I_TabPageContainer;
                        }
                        item.Item = columnControl.ControlManager;
                        container.AddView(columnControl.ControlManager, columnControl.ControlManager.LabelControlManager);
                        parentList.Add(item);
                        columnControl.Visited = true;
                    }
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.EmptySpace)
                {
                    var setting = uiCompositionItem.EmptySpaceUISetting;
                    if (setting == null)
                    {
                        setting = new EmptySpaceUISettingDTO();
                        setting.ExpandToEnd = false;
                        setting.UIColumnsType = Enum_UIColumnsType.Normal;
                    }
                    //var emptyItem = SpecializedDataView.GenerateEmptySpace(setting);
                    //item.Item = emptyItem;
                    container.AddEmptySpace(setting);
                    parentList.Add(item);
                }
                //حالت تب اضافه شود
                //uiControlPackageList.Add(item);
                //if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                //{

                //}

            }




        }
        public I_View_EditEntityAreaDataView SpecializedDataView
        {
            get
            {
                return base.DataView as I_View_EditEntityAreaDataView;
            }
        }

        public void CreateDefaultData()
        {
            bool shouldCreatData = true;

            //if (DataEntryEntity.IsReadonly)
            //{
            //    shouldCreatData = false;
            //}
            //if (AreaInitializer.SourceRelation != null)
            //{
            //    if (AreaInitializer.SourceRelation.Relationship.IsReadonly)
            //        shouldCreatData = false;
            //}
            if (shouldCreatData)
                shouldCreatData = GetDataList().Count == 0;

            if (shouldCreatData)
            {
                DP_DataRepository newData = AgentHelper.CreateAreaInitializerNewData(this);
                var addResult = AddData(newData, true);
                if (!addResult)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده پیش فرض و یا داده های وابسته", newData.ViewInfo, Temp.InfoColor.Red);
                if (AreaInitializer.SourceRelation != null)
                {
                    if (DataView != null)
                    {
                        //////SpecializedDataView.DisableEnableDataSection(true);
                        //////if (AreaInitializer.BusinessReadOnlyByParent || AreaInitializer.ParentDataItemBusinessReadOnly.Any(x => x == ChildRelationshipInfo.SourceData) || AreaInitializer.SecurityReadOnlyByParent || AreaInitializer.SecurityReadOnly)
                        //////{
                        //////    SpecializedDataView.DisableEnableDataSection(false);
                        //////}
                    }
                }
            }
        }
        //صدا زده میشود RelationData تنها بوسیله ShowData برای نمایش داده های اضافه شده در آن صدا زده میشود.در مابقی موارد ShowData که AddData فقط یکجا مشخص میشود و آنهم در specificDate
        public override bool ShowDataInDataView(DP_DataRepository specificDate)
        {

            if (!specificDate.IsFullData)
                throw new Exception("asdasd");

            foreach (var propertyControl in SimpleColumnControls)
            {
                var property = specificDate.GetProperty(propertyControl.Column.ID);
                if (property != null)
                {

                    //////ShowTypePropertyControlValue(specificDate, propertyControl, property.Value);
                    //if (propertyControl.Column.ColumnValueRange != null && propertyControl.Column.ColumnValueRange.Details.Any())
                    //{
                    //    var columnKeyValue = propertyControl.Column.ColumnValueRange;
                    //    CheckItemsSourceAndPropertyValue(propertyControl, specificDate);
                    //}
                    SetBinding(specificDate, propertyControl, property);

                }
                else
                {
                    //????
                }
            }
            bool result = true;
            //جدید--دقت شود که اگر نمایش مستقیم نیست داخل فرم رابطه ای نباید همه کنترلها مقداردهی شوند
            foreach (var relationshipControl in RelationshipColumnControls)
            {
                bool relationshipFirstSideHasValue = relationshipControl.Relationship.RelationshipColumns.Any()
                    && relationshipControl.Relationship.RelationshipColumns.All(x => specificDate.GetProperties().Any(y => !AgentHelper.ValueIsEmpty(y) && y.ColumnID == x.FirstSideColumnID));

                //relationshipControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelatedData = specificDate;
                //اینجا یکارایی بشه دسترسی موقت

                bool childLoadedBefore = specificDate.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID);

                ChildRelationshipInfo childData = null;
                if (childLoadedBefore)
                    childData = specificDate.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                else
                {
                    if (!relationshipFirstSideHasValue)
                    {
                        childData = specificDate.AddChildRelationshipInfo(relationshipControl.Relationship);
                    }
                    else
                    {
                        bool childIsDataView = (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                                               relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
                        if (childIsDataView)
                            childData = AreaInitializer.EditAreaDataManager.SerachDataFromParentRelationForChildDataView(relationshipControl.Relationship, this, relationshipControl.EditNdTypeArea, specificDate);
                        else
                            childData = AreaInitializer.EditAreaDataManager.SerachDataFromParentRelationForChildTempView(relationshipControl.Relationship, this, relationshipControl.EditNdTypeArea, specificDate);
                    }
                }
                if (childData.SecurityIssue == false)
                {

                    //    if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
                    //relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
                    //    {
                    //        if (relationshipControl.EditNdTypeArea.TemporaryDisplayView != null)
                    //            relationshipControl.EditNdTypeArea.TemporaryDisplayView.DisableEnable(TemporaryLinkType.DataView, true);

                    //        if (relationshipControl.EditNdTypeArea.TemporaryDisplayView != null)
                    //        {
                    //            if (relationshipControl.EditNdTypeArea.SecurityReadOnlyByParent|| relationshipControl.EditNdTypeArea.AreaInitializer.SecurityReadOnly )
                    //            {
                    //                if (!childData.RelatedData.Any())
                    //                {

                    //                    //اگر مستقیم بود چی..اصن نباید دیتای دیفالت تولید بشه .درست شود
                    //                    relationshipControl.EditNdTypeArea.TemporaryDisplayView.DisableEnable(TemporaryLinkType.DataView, false);

                    //                }
                    //            }
                    //        }
                    //    }

                    var childResult = relationshipControl.EditNdTypeArea.SetChildRelationshipInfoAndShow(childData);
                    if (!childResult)
                        result = false;
                }
                else
                    result = false;
            }
            OnDataItemShown(new EditAreaDataItemArg() { DataItem = specificDate });
            CheckRelationshipReadonlyEnablity();
            return result;


            //else

        }
        private void CheckRelationshipReadonlyEnablity()
        {
            var data = GetDataList();
            bool enablity = true;
            if (data.Any() && DataIsNewInOneEditAreaAndReadonly(data.First()))
                enablity = false;
            DataView.DisableEnableDataSection(enablity);
        }
        //اونی که کلید نداره قتی اول میشه موقع آپدیت اونی که کلید داره مقدار موجود اولیو نمیگیره؟؟

        //تنها یکبار و برای نمایش مقادیر در کنترلهای ساده و غیر فرمی صدا زده میشود
        //public bool ShowTypePropertyControlValue(DP_DataRepository dataItem, SimpleColumnControl typePropertyControl, string value)
        //{
        //    return typePropertyControl.SetValue(value);

        //}
        public void SetBinding(DP_DataRepository dataItem, SimpleColumnControl typePropertyControl, EntityInstanceProperty property)
        {
            typePropertyControl.SimpleControlManager.SetBinding(dataItem, property);
        }

    }



}
