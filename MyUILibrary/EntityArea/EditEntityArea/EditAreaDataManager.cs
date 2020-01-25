using CommonDefinitions.UISettings;
using ModelEntites;
using MyDataManagerService;
using MyRelationshipDataManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class EditAreaDataManager : I_EditAreaDataManager
    {
        public EditAreaDataManager()
        {
            EditEntityAreas = new List<I_EditEntityArea>();
            DataList = new ObservableCollection<DP_DataRepository>();
        }
        RelationshipDataManager relationshipManager = new RelationshipDataManager();
        List<I_EditEntityArea> EditEntityAreas { set; get; }
        ObservableCollection<DP_DataRepository> DataList { set; get; }

        /// <summary>
        /// /////////////////////////مشترک
        /// </summary>
        /// <param name="relationshipID"></param>
        /// <param name="editEntityArea"></param>
        /// <param name="parentRelationData"></param>
        /// <returns></returns>
        public ChildRelationshipInfo SerachDataFromParentRelationForChildDataView(RelationshipDTO relationship, I_EditEntityAreaOneData sourceEditEntityArea, I_EditEntityArea targetEditEntityArea, DP_DataRepository parentRelationData)
        {
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

            ChildRelationshipInfo childRelationshipInfo = null;
            childRelationshipInfo = parentRelationData.ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == relationship.ID);
            if (childRelationshipInfo == null)
            {
                childRelationshipInfo = parentRelationData.AddChildRelationshipInfo(relationship);
            }
            else
            {
                throw new Exception("Asd");
            }

            //سکوریتی داده اعمال میشود

            var searchDataItem = relationshipManager.GetSecondSideSearchDataItemByRelationship(parentRelationData, relationship.ID);
            if (searchDataItem != null)
            {
                // DR_SearchEditRequest request = new DR_SearchEditRequest(requester, searchDataItem, targetEditEntityArea.AreaInitializer.SecurityReadOnly, true);
                DR_SearchEditRequest request = new DR_SearchEditRequest(requester, searchDataItem, true, true);
                var childFullData = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchEditRequest(request).ResultDataItems;
                var countRequest = new DR_SearchCountRequest(requester);
                countRequest.SearchDataItems = searchDataItem;
                countRequest.Requester.SkipSecurity = true;
                var count = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchCountRequest(countRequest);
                bool secutrityImposed = false;
                if (count.ResultCount != childFullData.Count)
                    secutrityImposed = true;


                if (!secutrityImposed)
                {
                    foreach (var data in childFullData)
                    {
                        data.IsDBRelationship = true;
                        data.DataView = GetDataView(data);
                        childRelationshipInfo.AddDataToChildRelationshipInfo(data, true);
                    }

                }
                else
                    childRelationshipInfo.SecurityIssue = true;
            }
            return childRelationshipInfo;
            //foreach (var item in childFullData)
            //    searchedData.Add(new Tuple<DP_DataRepository, DP_DataView>(item, null));

            //return AddEditSearchData(searchedData, editEntityArea);
        }
        public DP_DataView GetDataView(DP_DataRepository data)
        {
            //بعدا بررسی شود.کلا روش خوبی نیست.بهتره تو همون سرچ ادیت یه پارامتری سرچ بشه و دیتاویو ساخته بشه
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

            DP_SearchRepository searchDataViewItem = new DP_SearchRepository(data.TargetEntityID);
            foreach (var col in data.KeyProperties)
            {
                searchDataViewItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            DR_SearchViewRequest requestDataView = new DR_SearchViewRequest(requester, searchDataViewItem);
            //requestDataView.EntityViewID = listViewID;
            var childViewData = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(requestDataView).ResultDataItems;
            if (childViewData.Any())
                return childViewData[0];
            else
                return null;
        }
        public ChildRelationshipInfo SerachDataFromParentRelationForChildTempView(RelationshipDTO relationship, I_EditEntityArea sourceEditEntityArea, I_EditEntityArea targetEditEntityArea, DP_DataRepository parentRelationData)
        {

            //List<DP_DataRepository> re = null;
            //if (parentRelationData.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipID))
            //{
            //    childViewData = new List<DP_DataView>();
            //    foreach (var child in parentRelationData.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipID).RelatedData)
            //        childViewData.Add(child.DataView);
            //}
            //else
            //{
            ChildRelationshipInfo childRelationshipInfo = null;
            childRelationshipInfo = parentRelationData.ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == relationship.ID);
            if (childRelationshipInfo == null)
            {
                childRelationshipInfo = parentRelationData.AddChildRelationshipInfo(relationship);
            }
            else
            {
                throw new Exception("Asd");
            }

            List<DP_DataRepository> result = new List<DP_DataRepository>();
            var searchDataItem = relationshipManager.GetSecondSideSearchDataItemByRelationship(parentRelationData, relationship.ID);


            if (searchDataItem != null)
            {
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchDataItem);
                if (targetEditEntityArea.DefaultEntityListViewDTO != null)
                    request.EntityViewID = targetEditEntityArea.DefaultEntityListViewDTO.ID;
                var childViewData = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(request).ResultDataItems;

                var countRequest = new DR_SearchCountRequest(requester);
                countRequest.SearchDataItems = searchDataItem;
                countRequest.Requester.SkipSecurity = true;
                var count = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchCountRequest(countRequest);
                bool secutrityImposed = false;
                if (count.ResultCount != childViewData.Count)
                    secutrityImposed = true;
                if (!secutrityImposed)
                {
                    foreach (var item in childViewData)
                    {
                        var dpItem = ConvertDP_DataViewToDP_DataRepository(item, targetEditEntityArea);
                        result.Add(dpItem);
                        dpItem.IsDBRelationship = true;
                        childRelationshipInfo.AddDataToChildRelationshipInfo(dpItem, true);
                    }
                }
                else
                    childRelationshipInfo.SecurityIssue = true;
            }
            //}
            return childRelationshipInfo;

        }
        public DP_DataRepository ConvertDP_DataViewToDP_DataRepository(DP_DataView item, I_EditEntityArea editEntityArea)
        {
            DP_DataRepository dataRepository = new DP_DataRepository(item.TargetEntityID, item.TargetEntityAlias);
            if (dataRepository.EntityListView == null)
                dataRepository.EntityListView = editEntityArea.DefaultEntityListViewDTO;
            dataRepository.DataView = item;
            foreach (var key in item.Properties.Where(x => x.IsKey))
            {
                dataRepository.AddProperty(editEntityArea.EntityWithSimpleColumns.Columns.First(x => x.ID == key.ColumnID), key.Value);
            }
            return dataRepository;
        }

        public bool ConvertDataViewToFullData(int entityID, DP_DataRepository dataITem, I_EditEntityArea editEntityArea)
        {
            //اوکی نشده
            DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
            foreach (var col in dataITem.KeyProperties)
            {
                SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

            // var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem, editEntityArea.AreaInitializer.SecurityReadOnly, false);
            var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem, true, false);
            var foundItem = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchEditRequest(requestSearchEdit).ResultDataItems;
            if (foundItem.Any())
            {
                dataITem.ClearProperties();
                dataITem.SetProperties(foundItem[0].GetProperties());
                dataITem.IsFullData = true;

                return true;
            }
            else
            {
                return false;
            }
        }













        public DP_DataRepository SearchDataForEditFromExternalSource(int entityID, DP_BaseData searchViewData, I_EditEntityArea editEntityArea)
        {
            DP_SearchRepository searchDataItem = new DP_SearchRepository(entityID);
            foreach (var col in searchViewData.KeyProperties)
            {
                searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

            //   var requestSearchEdit = new DR_SearchEditRequest(requester, searchDataItem, editEntityArea.AreaInitializer.SecurityReadOnly, true);
            var requestSearchEdit = new DR_SearchEditRequest(requester, searchDataItem, true, true);
            var foundItem = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchEditRequest(requestSearchEdit).ResultDataItems;
            if (foundItem.Any())
            {
                foundItem[0].DataView = GetDataView(foundItem[0]);
                return foundItem[0];
            }
            else
                return null;
        }
        //public List<DP_DataRepository> SearchDataForEditFromExternalSource(int entityID, List<DP_DataRepository> searchViewData, I_EditEntityArea editEntityArea)
        //{

        //    DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (var item in searchViewData)
        //    {
        //        foreach (var col in item.KeyProperties)
        //        {
        //            SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
        //        }

        //        var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //        var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem);
        //        var froundItem = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchEditRequest(requestSearchEdit).ResultDataItems;
        //        if (froundItem.Any())
        //            result.Add(froundItem[0]);
        //        else
        //        {
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", item.ProperyValues, Temp.InfoColor.Red);
        //        }
        //    }
        //    return result;
        //}
        public DP_DataView SearchDataForViewFromExternalSource(int entityID, DP_BaseData searchViewData, I_EditEntityArea editEntityArea)
        {
            DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
            foreach (var col in searchViewData.KeyProperties)
            {
                SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

            var requestSearchView = new DR_SearchViewRequest(requester, SearchDataItem);
            var foundItem = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(requestSearchView).ResultDataItems;
            if (foundItem.Any())
                return foundItem[0];
            else
                return null;
        }

        //public List<DP_DataView> SearchDataForViewFromExternalSource(int entityID, List<DP_DataRepository> searchViewData, I_EditEntityArea editEntityArea)
        //{
        //    //اوکی نشده
        //    DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
        //    List<DP_DataView> result = new List<DP_DataView>();
        //    foreach (var item in searchViewData)
        //    {
        //        foreach (var col in item.KeyProperties)
        //        {
        //            SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
        //        }

        //        var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //        var requestSearchView = new DR_SearchViewRequest(requester, SearchDataItem);
        //        var foundItem = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(requestSearchView).ResultDataItems;
        //        if (foundItem.Any())
        //            result.Add(foundItem[0]);
        //        else
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", item.ProperyValues, Temp.InfoColor.Red);

        //    }
        //    return result;
        //}
        public DP_DataRepository GetFullDataFromDataViewSearch(int entityID, DP_DataView searchViewData, I_EditEntityArea editEntityArea)
        {
            //سکوریتی داده اعمال میشود
            DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
            foreach (var col in searchViewData.Properties.Where(x => x.IsKey))
            {
                SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
            // var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem, editEntityArea.AreaInitializer.SecurityReadOnly, false);
            var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem, true, false);
            var res = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchEditRequest(requestSearchEdit).ResultDataItems;
            if (res.Any())
            {
                var froundItem = res[0];
                froundItem.DataView = searchViewData;
                return froundItem;
            }

            else
            {

                return null;
            }
        }

        //public List<DP_DataRepository> GetFullDataFromDataViewSearch(int entityID, List<DP_DataView> searchViewData, I_EditEntityArea editEntityArea)
        //{
        //    //سکوریتی داده اعمال میشود
        //    //حتما اینجا باید وقتی بیاد که فرم شروع کننه باشد
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (var item in searchViewData)
        //    {
        //        DP_SearchRepository SearchDataItem = new DP_SearchRepository(entityID);
        //        foreach (var col in item.Properties.Where(x => x.IsKey))
        //        {
        //            SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
        //        }
        //        var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //        var requestSearchEdit = new DR_SearchEditRequest(requester, SearchDataItem);
        //        var res = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchEditRequest(requestSearchEdit).ResultDataItems;

        //        if (res.Any())
        //        {
        //            var froundItem = res[0];
        //            froundItem.DataView = item;
        //            result.Add(froundItem);
        //        }
        //        else
        //        {
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به داده", item.ProperyValues, Temp.InfoColor.Red);
        //        }
        //    }
        //    return result;
        //}

        //public void ConvertDataViewToFullData(int entityID, List<DP_DataRepository> searchViewDatas, I_EditEntityArea editEntityArea)
        //{
        //    foreach (var searchViewData in searchViewData)
        //    {
        //        var requestSearchEdit = new DR_SearchEditRequest();
        //        requestSearchEdit.SearchDataItem.TargetEntityID = entityID;
        //        foreach (var col in searchViewData.KeyProperties)
        //        {
        //            requestSearchEdit.SearchDataItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
        //        }
        //        var froundItem = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchEditRequest(requestSearchEdit).ResultDataItems[0];
        //        searchViewData.ClearProperties();
        //        searchViewData.SetProperties(froundItem.GetProperties());
        //        searchViewData.IsFullData = true;
        //    }
        //}

        //public List<DP_DataRepository> ConvertDP_DataViewToDP_DataRepository(List<DP_DataView> searechedData, I_EditEntityArea editEntityArea)
        //{
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (var item in searechedData)
        //        result.Add(ConvertDataViewToDataRepository(item, editEntityArea));
        //    return result;
        //}

        //private List<DP_DataRepository> AddEditSearchData(List<Tuple<DP_DataRepository, DP_DataView>> searchedData, I_EditEntityArea editEntityArea)
        //{
        //    CheckEditArea(editEntityArea);
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    //bool newItemAdded = false;
        //    foreach (var titem in searchedData)
        //    {
        //        var item = titem.Item1;
        //        //foreach (var key in item.KeyProperties)
        //        //{
        //        //var existData = DataList.FirstOrDefault(x => x.IsNewItem == false && item.KeyProperties.All(z => x.KeyProperties.Any(h => h.ColumnID == z.ColumnID && z.Value == h.Value)));
        //        //if (existData == null)
        //        //{
        //        //item.RelatedDataChanged += Item_RelatedDataChanged;
        //        item.IsFullData = true;
        //        result.Add(item);
        //        //CheckItemInfo(item, titem.Item2, editEntityArea);
        //        //}
        //        //else
        //        //{
        //        //    if (existData.IsFullData)
        //        //    {
        //        //        result.Add(existData);
        //        //    }
        //        //    else
        //        //    {
        //        //        existData.ClearProperties();
        //        //        existData.RelatedDataChanged += Item_RelatedDataChanged;
        //        //        foreach (var property in item.GetProperties())
        //        //            existData.AddProperty(editEntityArea.EntityWithSimpleColumns.Columns.First(x => x.ID == property.ColumnID), property.Value);
        //        //        existData.IsFullData = true;
        //        //        result.Add(existData);
        //        //    }
        //        //}
        //        //}
        //    }

        //    return result;
        //}



        //private void CheckEditArea(I_EditEntityArea editEntityArea)
        //{
        //    if (!EditEntityAreas.Any(x => x == editEntityArea))
        //    {
        //        EditEntityAreas.Add(editEntityArea);
        //        if (editEntityArea.AreaInitializer.SourceRelation == null)
        //        {
        //            editEntityArea.AreaInitializer.Datas.CollectionChanged += Datas_CollectionChanged;
        //        }
        //    }
        //}

        private void Datas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
        //private void Item_RelatedDataChanged(object sender, RelatedDataChangedArg e)
        //{

        //}
        //private List<DP_DataRepository> AddViewSearchData(List<DP_DataView> searchedData, I_EditEntityArea editEntityArea)
        //{
        //    CheckEditArea(editEntityArea);
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (var item in searchedData)
        //    {
        //        var keys = item.Properties.Where(x => x.IsKey);

        //        var existData = DataList.FirstOrDefault(x => x.IsNewItem == false && keys.All(z => x.KeyProperties.Any(h => h.ColumnID == z.ColumnID && z.Value == h.Value)));
        //        if (existData == null)
        //        {
        //            result.Add(ConvertDataViewToDataRepository(item, editEntityArea));
        //            //CheckItemInfo(item, item, editEntityArea);
        //        }
        //        else
        //        {
        //            if (existData.IsFullData)
        //            {
        //                result.Add(existData);
        //            }
        //            else
        //            {
        //                result.Add(existData);
        //            }
        //        }

        //    }
        //    return result;
        //}

        //private DP_DataRepository ConvertDataViewToDataRepository(DP_DataView item, I_EditEntityArea editEntityArea)
        //{

        //}

        //private void CheckItemInfo(DP_DataRepository mainItem, I_EditEntityArea editEntityArea)
        //{
        //    if (string.IsNullOrEmpty(mainItem.ViewInfo))

        //    {

        //        List<int> columnIDs = new List<int>();
        //        if (editEntityArea.EntityListViewDTO.EntityListViewAllColumns.Any(x => x.RelationshipTailID == 0))
        //        {
        //            foreach (var prop in editEntityArea.EntityListViewDTO.EntityListViewAllColumns.Where(x => x.RelationshipTailID == 0))
        //            {
        //                columnIDs.Add(prop.ColumnID);
        //            }
        //        }
        //        else
        //        {
        //            var columnList = editEntityArea.FullEntity.Columns;
        //            int columnID = 0;
        //            if (columnList.Any(x => x.Name.ToLower() == "name"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "name").ID;
        //            }
        //            else if (columnList.Any(x => x.Name.ToLower() == "title"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "title").ID;
        //            }
        //            else if (columnList.Any(x => x.Name.ToLower() == "desc"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "desc").ID;
        //            }
        //            else if (columnList.Any(x => x.Name.ToLower() == "description"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "description").ID;
        //            }
        //            else if (columnList.Any(x => x.Name.ToLower() == "code"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "code").ID;
        //            }
        //            else if (columnList.Any(x => x.Name.ToLower() == "id"))
        //            {
        //                columnID = columnList.First(x => x.Name.ToLower() == "id").ID;
        //            }
        //            else
        //                columnID = mainItem.Properties.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value))?.ColumnID ?? 0;

        //            if (columnID != 0)
        //                columnIDs.Add(columnID);
        //        }
        //        List<EntityInstanceProperty> viewItemProperties = new List<EntityInstanceProperty>();
        //        foreach (var col in columnIDs)
        //        {
        //            var title = "";
        //            var property = mainItem.Properties.FirstOrDefault(x => x.ColumnID == col);
        //            if (property != null)
        //            {
        //                if (property.Value == "<Null>" || property.Value == "0" || property.Value == "")
        //                {
        //                    title += (title == "" ? "" : ",") + " Selected " + (string.IsNullOrEmpty(editEntityArea.SimpleEntity.Alias) ? editEntityArea.SimpleEntity.Name : editEntityArea.SimpleEntity.Alias);
        //                }
        //                else
        //                    title += (title == "" ? "" : ",") + property.Value;
        //            }
        //            viewItemProperties.Add(property);
        //        }
        //        mainItem.ViewEntityProperties.Add(new Tuple<int, List<EntityInstanceProperty>>(editEntityArea.AreaInitializer.EntityID, viewItemProperties));
        //    }
        //}


        //private void CheckItemInfo(DP_DataRepository mainItem, DP_DataRepository viewItem, I_EditEntityArea editEntityArea)
        //{
        //    if (string.IsNullOrEmpty(mainItem.ViewInfo))
        //    {
        //        List<EntityInstanceProperty> viewItemProperties = null;
        //        if (viewItem != null)
        //        {
        //            viewItemProperties = viewItem.Properties;

        //        }
        //        else
        //        {
        //            viewItemProperties = new List<EntityInstanceProperty>();
        //            foreach (var column in editEntityArea.EntityListViewDTO.EntityListViewAllColumns)
        //            {
        //                if (column.RelationshipTailID == 0)
        //                {
        //                    var value = editEntityArea.FetchTypePorpertyValue()
        //                }
        //                l
        //            }
        //            DR_SearchViewRequest request = new DR_SearchViewRequest();
        //            request.Requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //            foreach (var keyProperty in mainItem.Properties.Where(x => x.IsKey))
        //            {
        //                var searchProperty = new SearchProperty();
        //                searchProperty.ColumnID = keyProperty.ColumnID;
        //                searchProperty.Value = keyProperty.Value;
        //                request.SearchDataItems.Phrases.Add(searchProperty);
        //            }
        //            request.EntityID = request.SearchDataItems.TargetEntityID;
        //            request.Requester.SkipSecurity = true;
        //            request.EntityViewID = editEntityArea.EntityListViewDTO.ID;
        //            var foundView = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(request).ResultDataItems[0];
        //            viewItemProperties = foundView.Properties;

        //        }

        //        foreach (var property in viewItemProperties)
        //        {
        //            if (!string.IsNullOrEmpty(property.Value))
        //            {
        //                mainItem.ViewInfo += (string.IsNullOrEmpty(mainItem.ViewInfo) ? "" : ",") + property.Name + ":" + property.Value;
        //            }
        //        }
        //    }
        //}


        //public List<DP_DataRepository> SerachDataViewFromParentRelation(int relationshipID, DP_DataRepository parentRelationData)
        //{
        //    return null;
        //}



        //private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    //if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //    //{
        //    //    foreach (DP_DataRepository item in e.NewItems)
        //    //    {
        //    //        if (item.IsNewItem)
        //    //        {
        //    //            item.ViewInfo = GetDataItemInfo(editArea, data[0]);
        //    //        }
        //    //    }
        //    //}
        //    SetTempInfo(sender as I_EditEntityArea);
        //}



        //internal void SetTempInfo(I_EditEntityArea editArea)
        //{
        //    if (editArea.AreaInitializer.SourceRelation != null)
        //    {
        //        if (editArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect ||
        //          editArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect)
        //        {
        //            var data = editArea.AreaInitializer.SourceRelation.RelatedData.GetRelatedData(editArea.AreaInitializer.SourceRelation.Relationship.ID);
        //            if (data.Any())
        //            {
        //                var text = "";
        //                if (editArea is I_EditEntityAreaOneData)
        //                {
        //                    if (data.Count > 1)
        //                        throw new Exception("asdasd");
        //                    text = GetInfo(data[0], editArea);
        //                }
        //                else if (editArea is I_EditEntityAreaMultipleData)
        //                {
        //                    text = data.Count + " " + "داده وجود دارد";
        //                    foreach (var item in data)
        //                    {
        //                        text += Environment.NewLine;
        //                        text += GetInfo(item, editArea);
        //                    }
        //                }

        //                if (editArea.AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaOneData)
        //                    editArea.TemporaryDisplayView.SetLinkText(text);
        //                else if (editArea.AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaMultipleData)
        //                {
        //                    var parentMultipleEditArea = (editArea.AreaInitializer.SourceRelation.SourceEditArea as I_EditEntityAreaMultipleData);
        //                    var relationshipControl = parentMultipleEditArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == editArea.AreaInitializer.SourceRelation.Relationship.ID);
        //                    if (relationshipControl != null)
        //                        relationshipControl.ControlPackage.View.SetTemporaryViewText(editArea.AreaInitializer.SourceRelation.RelatedData, text);
        //                }
        //            }
        //            else
        //            {
        //                if (editArea.AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaOneData)
        //                    editArea.TemporaryDisplayView.SetLinkText("");
        //                else if (editArea.AreaInitializer.SourceRelation.SourceEditArea is I_EditEntityAreaMultipleData)
        //                {
        //                    var parentMultipleEditArea = (editArea.AreaInitializer.SourceRelation.SourceEditArea as I_EditEntityAreaMultipleData);
        //                    var relationshipControl = parentMultipleEditArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == editArea.AreaInitializer.SourceRelation.Relationship.ID);
        //                    if (relationshipControl != null)
        //                        relationshipControl.ControlPackage.View.SetTemporaryViewText(editArea.AreaInitializer.SourceRelation.RelatedData, "");
        //                }

        //            }

        //        }
        //    }

        //}

        private string GetInfo(DP_DataRepository dP_DataRepository, I_EditEntityArea editEntityArea)
        {
            string result = "";
            if (dP_DataRepository.DataView == null)
            {
                if (!dP_DataRepository.IsFullData)
                    throw new Exception("Asdadsf");
                var columns = editEntityArea.DefaultEntityListViewDTO.EntityListViewAllColumns.Where(x => x.RelationshipTailID == 0);
                if (columns.Any())
                {
                    foreach (var item in columns)
                    {
                        var property = dP_DataRepository.GetProperty(item.ColumnID);
                        result += (result == "" ? "" : ",") + property.Name + ":" + property.Value;
                    }
                }
                else
                {
                    foreach (var item in dP_DataRepository.GetProperties().Take(3))
                    {
                        var property = dP_DataRepository.GetProperty(item.ColumnID);
                        result += (result == "" ? "" : ",") + property.Name + ":" + property.Value;
                    }
                    result += " ...";
                }
            }
            else
            {
                foreach (var item in dP_DataRepository.DataView.Properties)
                {
                    //var column = dP_DataRepository.DataView.Properties;
                    result += (result == "" ? "" : ",") + item.Name + ":" + item.Value;
                }
            }
            return result;
        }



        //private   GetDataItemInfo(I_EditEntityArea editArea, DP_DataRepository specificDate)
        //{

        //    return title;

        //}
    }
}
