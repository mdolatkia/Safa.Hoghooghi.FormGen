

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
    public class EditEntityAreaMultipleData : BaseEditEntityArea, I_EditEntityAreaMultipleData
    {



        public event EventHandler<DataUpdatedArg> Updated;
        public event EventHandler<EditAreaDataItemArg> DataItemRemoved;
        public EditEntityAreaMultipleData(TableDrivedEntityDTO simpleEntity) : base(simpleEntity)
        {
            //SimpleColumnControls = new List<SimpleColumnControl>();
            //RelationshipColumnControls = new List<RelationshipColumnControl>();
        }
        public void RemoveData(List<DP_DataRepository> datas, bool fromDataView)
        {
            //DataChangedFromDataView = fromDataView;
            if (AreaInitializer.SourceRelation == null)
            {
                foreach (var item in datas)
                    RemoveData(item, true);
            }
            else
            {
                var removeList = datas.Where(x => x.IsNewItem);
                foreach (var item in removeList)
                    RemoveData(item, true);

                var clearIsOk = CheckRemoveData(datas);

                if (clearIsOk)
                {
                    var existingdatas = datas.Where(x => x.IsDBRelationship);
                    foreach (var item in existingdatas)
                    {
                        if (ChildRelationshipInfo != null)
                        {
                            //SetRemovedItem(ChildRelationshipInfo, item, shouldDeleteFromDB);
                        }
                        RemoveData(item, true);
                    }
                }

            }

        }
        //private void View_ButtonClicked(object sender, ConfirmModeClickedArg e, List<DP_DataRepository> deleteDataList)
        //{
        //    I_ViewDeleteInquiry view = sender as I_ViewDeleteInquiry;
        //    if (view != null)
        //    {
        //        if (e.Result == UserDialogResult.Ok || e.Result == UserDialogResult.No)
        //        {

        //        }
        //        else if (e.Result == UserDialogResult.Yes)
        //        {
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(view);
        //            foreach (var item in deleteDataList)
        //            {
        //                if (ChildRelationshipInfo != null)
        //                {
        //                    //SetRemovedItem(ChildRelationshipInfo, item, true);
        //                }
        //                RemoveData(item, true);
        //            }
        //        }
        //    }
        //}
        public void RemoveData(DP_DataRepository data, bool fromDataView)
        {
            //DataChangedFromDataView = fromDataView;
            GetDataList().Remove(data);
            RemoveDataContainer(data);

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
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
            //ManageDataSecurity();
        }
        public void RemoveDataContainers()
        {
            if (DataView != null)
            {
                var list = (DataView as I_View_EditEntityAreaMultiple).RemoveDataContainers();
                if (DataItemRemoved != null)
                {
                    foreach (var item in list)
                        DataItemRemoved(this, new EditAreaDataItemArg() { DataItem = item as DP_DataRepository });
                }
            }
        }
        private void RemoveDataContainer(DP_DataRepository dataItem)
        {
            (DataView as I_View_EditEntityAreaMultiple).RemoveDataContainer(dataItem);
            if (DataItemRemoved != null)
                DataItemRemoved(this, new EditAreaDataItemArg() { DataItem = dataItem });
        }
        public bool ShowDatasInDataView(List<DP_DataRepository> dataItems)
        {
            bool result = true;
            foreach (var item in dataItems)
            {
                var itemResult = ShowDataInDataView(item);
                if (!itemResult)
                    result = false;
            }
            return result;
        }
        public override bool ShowDataInDataView(DP_DataRepository dataItem)
        {
            (DataView as I_View_EditEntityAreaMultiple).AddDataContainer(dataItem);
            return InternalShowDataInDataView(dataItem);
        }
        //بعدا بررسی شود
        private bool InternalShowDataInDataView(DP_DataRepository specificDate)
        {
            if (!specificDate.IsFullData)
                throw new Exception("asdasd");
            foreach (var propertyControl in SimpleColumnControls)
            {
                var property = specificDate.GetProperty(propertyControl.Column.ID);
                if (property != null)
                {
                    //if (AreaInitializer.IntracionMode == IntracionMode.Create
                    //                           || AreaInitializer.IntracionMode == IntracionMode.CreateSelect)
                    //{

                    //if (propertyControl.Column.ColumnValueRange != null && propertyControl.Column.ColumnValueRange.Details.Any())
                    //{
                    //    var columnKeyValue = propertyControl.Column.ColumnValueRange;
                    //    if (!string.IsNullOrEmpty(property.Value))
                    //    {
                    //        if (columnKeyValue.ValueFromTitleOrValue)
                    //        {
                    //            if (!columnKeyValue.Details.Any(x => x.KeyTitle == property.Value))
                    //                property.Value = "";
                    //        }
                    //        else
                    //        {
                    //            if (!columnKeyValue.Details.Any(x => x.Value == property.Value))
                    //                property.Value = "";
                    //        }
                    //    }
                    //}

                    SetBinding(specificDate, propertyControl, property);
                    //ShowTypePropertyControlValue(specificDate, propertyControl, property.Value);
                    //}
                }
                else
                {
                    //????
                }
            }
            bool result = true;
            foreach (var relationshipControl in RelationshipColumnControls)
            {
                bool relationshipFirstSideHasValue = relationshipControl.Relationship.RelationshipColumns.Any()
               && relationshipControl.Relationship.RelationshipColumns.All(x => specificDate.GetProperties().Any(y => y.Value!=null && !string.IsNullOrEmpty(y.Value.ToString()) && y.ColumnID == x.FirstSideColumnID));

                bool childLoadedBefore = specificDate.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                ChildRelationshipInfo childData = null;
                if (childLoadedBefore)
                    childData = specificDate.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                else
                {
                    if (!relationshipFirstSideHasValue)
                        childData = specificDate.AddChildRelationshipInfo(relationshipControl.Relationship);
                    else
                        childData = AreaInitializer.EditAreaDataManager.SerachDataFromParentRelationForChildTempView(relationshipControl.Relationship, this, relationshipControl.EditNdTypeArea, specificDate);

                }
                if (childData.SecurityIssue == false)
                {
                    relationshipControl.EditNdTypeArea.SetChildRelationshipInfoAndShow(childData);

                    //if (relationshipControl.EditNdTypeArea.SecurityReadOnlyByParent || relationshipControl.EditNdTypeArea.AreaInitializer.SecurityReadOnly)
                    //    if (!childData.RelatedData.Any())
                    //    {
                    //        relationshipControl.View.DisableEnable(specificDate, TemporaryLinkType.DataView, false);
                    //    }
                }
                else
                {
                    result = false;
                }
            }
            if (result)
                OnDataItemShown(new EditAreaDataItemLoadedArg() { DataItem = specificDate, InEditMode = true });
            //    OnDataItemLoaded(new EditAreaDataItemLoadedArg() { DataItem = specificDate, InEditMode = true });
            return result;
        }
        //باشد Direct صدا زده میشود ، اگر LoadTemplate یکبار در
        //.باشد AreaInitializer.FormComposed == false کلیک میشود و اگر Data زمانی که لینک ShowTemporaryDataView یکبار هم در
        public void ShowDataFromExternalSource(List<DP_DataView> dataRepositories)
        {

            bool dataView = (AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                  AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect);
            if (dataRepositories != null && dataRepositories.Count > 0)
            {
                foreach (var dataRepository in dataRepositories)
                {
                    if (!dataRepository.KeyProperties.Any())
                        throw new Exception("asdad");
                }
                if (dataView)
                {
                    foreach (var dataRepository in dataRepositories)
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
                }
                else
                {
                    foreach (var dataRepository in dataRepositories)
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
                //ShowDataInDataView(result, true);
            }
            //if (dialog)
            //{
            //    if (dataView)
            //        AgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(DataView, SimpleEntity.Alias, Enum_WindowSize.Big);
            //    else
            //        AgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(TemporaryDisplayView, SimpleEntity.Alias, Enum_WindowSize.Big);
            //}
            //else
            //{
            //    if (dataView)
            //        AgentUICoreMediator.UIManager.ShowPane(DataView, SimpleEntity.Alias);
            //    else
            //        AgentUICoreMediator.UIManager.ShowPane(TemporaryDisplayView, SimpleEntity.Alias);
            //}
        }

        public override void GenerateUIComposition(List<EntityUICompositionDTO> UICompositions)
        {

            foreach (var uiCompositionItem in UICompositions.OrderBy(x => x.Position))
            {
                if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Entity)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabControl)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.TabPage)
                {
                    GenerateUIComposition(uiCompositionItem.ChildItems);
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Column)
                {
                    var columnControl = SimpleColumnControls.FirstOrDefault(x => x.Column.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        SpecializedDataView.AddUIControlPackage(columnControl.SimpleControlManager, columnControl.ControlManager.LabelControlManager);
                        columnControl.Visited = true;
                    }
                }
                else if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Relationship)
                {
                    var columnControl = RelationshipColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == Convert.ToInt32(uiCompositionItem.ObjectIdentity));
                    if (columnControl != null)
                    {
                        SpecializedDataView.AddView(columnControl.ControlManager, columnControl.ControlManager.LabelControlManager);
                        columnControl.Visited = true;
                    }
                }
                //حالت تب اضافه شود
                //uiControlPackageList.Add(item);
                //if (uiCompositionItem.ObjectCategory == DatabaseObjectCategory.Group)
                //{

                //}

            }
        }

        private void SearchViewEntityArea_DataSelected(object sender, DataSelectedEventArg e)
        {
            foreach (var data in e.DataItem)
            {
                DP_DataRepository result = null;
                if (SearchViewEntityArea.IsCalledFromDataView)
                {
                    result = AreaInitializer.EditAreaDataManager.GetFullDataFromDataViewSearch(AreaInitializer.EntityID, data, this);

                }
                else
                {
                    result = AreaInitializer.EditAreaDataManager.ConvertDP_DataViewToDP_DataRepository(data, this);
                }

                if (result != null)
                {
                    bool addResult = AddData(result, SearchViewEntityArea.IsCalledFromDataView);
                    if (!addResult)
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده و یا داده های وابسته", data.ViewInfo, Temp.InfoColor.Red);

                }
                else
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", e.DataItem.First().ViewInfo, Temp.InfoColor.Red);
                }
            }
        }

        public I_View_EditEntityAreaMultiple SpecializedDataView
        {
            get
            {
                return base.DataView as I_View_EditEntityAreaMultiple;
            }
        }
        public override void DataItemVisiblity(object dataItem, bool visible)
        {
            (DataView as I_View_EditEntityAreaMultiple).Visiblity(dataItem,visible);
        }
        public override void DataItemEnablity(object dataItem, bool visible)
        {
            (DataView as I_View_EditEntityAreaMultiple).EnableDisable(dataItem, visible);
        }
        //private I_SearchViewEntityArea GenerateSearchViewArea()
        //{
        //    if (AreaInitializer.SourceRelation != null)
        //        throw new Exception("asdasd");
        //    var searchViewEntityArea = new SearchViewEntityArea();
        //    var searchViewInit = new SearchViewAreaInitializer();
        //    searchViewInit.SourceEditArea = this;
        //    // searchViewInit.TempEntity = FullEntity;
        //    searchViewInit.EntityID = AreaInitializer.EntityID;
        //    searchViewInit.MultipleSelection = true;
        //    searchViewEntityArea.SetAreaInitializer(searchViewInit);
        //    searchViewEntityArea.DataSelected += SearchViewEntityArea_DataSelected;
        //    return searchViewEntityArea;
        //}

        public List<DP_DataRepository> GetSelectedData()
        {
            List<DP_DataRepository> selectedData = (DataView as I_View_EditEntityAreaMultiple).GetSelectedData().Cast<DP_DataRepository>().ToList();
            return selectedData;
        }


        public void SetBinding(DP_DataRepository dataItem, SimpleColumnControl typePropertyControl, EntityInstanceProperty property)
        {
            typePropertyControl.SimpleControlManager.SetBinding(dataItem, property);
        }

        public bool ShowTypePropertyControlValue(DP_DataRepository dataItem, SimpleColumnControl typePropertyControl, string value)
        {

            //ColumnSetting columnSetting = new ColumnSetting();

            //if (typePropertyControl.Column.PrimaryKey == true && (dataItem != null && !dataItem.IsNewItem))
            //{
            //    columnSetting.IsReadOnly = true;
            //}
            //else
            //    columnSetting.IsReadOnly = typePropertyControl.ColumnSetting.IsReadOnly;
            //بهتره جور دیگه نوشته بشه
            //if (typePropertyControl.ControlPackage != null)
            return typePropertyControl.SimpleControlManager.SetValue(dataItem, value);
            //else
            //    return typePropertyControl.SetValue(value);
            //AgentUICoreMediator.UIManager.ShowControlValue(typePropertyControl.ControlPackage, typePropertyControl.Column, value, columnSetting);

        }



        public object FetchTypePropertyControlValue(DP_DataRepository dataRepository, SimpleColumnControl SimpleColumnControlMultipleData)
        {
            //if (AreaInitializer.DataMode == DataMode.Multiple)
            return SimpleColumnControlMultipleData.SimpleControlManager.GetValue(dataRepository);
            //else
            //    return SimpleColumnControlMultipleData.GetValue();


            //////if (typePropertyControl.EditNdTypeArea != null)
            //////{
            //////    //var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //////    if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation != null)
            //////    {
            //////        if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ManyToOne
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ExplicitOneToOne
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubToSuper
            //////            || (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
            //////            || (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys))
            //////        {
            //////            var data = typePropertyControl.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault(x => x.SourceRelatedData == dataRepository);
            //////            //AreaInitializer.DataMode == DataMode.Multiple
            //////            //&& 
            //////            if (data == null)
            //////                return "";
            //////            else
            //////                return typePropertyControl.EditNdTypeArea.FetchTypePorpertyValue(data, typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipColumns.First().SecondSideColumn1);
            //////        }
            //////        else if (typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.OneToMany
            //////            || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.ImplicitOneToOne
            //////              || typePropertyControl.EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType == Enum_RelationshipType.SuperToSub)
            //////        {
            //////            //return FetchTypePorpertyValue(dataRepository, typePropertyControl.Column);
            //////            throw (new Exception("asfsdf"));
            //////        }
            //////    }
            //////    return "";


            //////}
            //////else
            //////{

            //////}


            //if (typePropertyControl is RelationSourceControl)
            //{
            //    var relationSourceControl = (typePropertyControl as RelationSourceControl);
            //    //////return relationSourceControl.EditNdTypeArea.FetchTypePorpertyValue(dataRepository, AgentHelper.GetRelationOperand(relationSourceControl.Relation, relationSourceControl.RelationSide == Enum_DP_RelationSide.FirstSide ? Enum_DP_RelationSide.SecondSide : Enum_DP_RelationSide.FirstSide));
            //    return "";
            //    // return FetchTypePropertyRelationSourceControl(typePropertyControl as RelationSourceControl);
            //}
            //else
            //    return SpecializedDataView.FetchTypePropertyControlValue(dataRepository, typePropertyControl);
        }





    }



}
