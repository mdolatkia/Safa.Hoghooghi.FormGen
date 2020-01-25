using MyUILibrary.DataViewArea;
using MyUILibraryInterfaces.DataViewArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyUILibraryInterfaces.DataLinkArea;
using MyRelationshipDataManager;
using MyUILibraryInterfaces.DataMenuArea;

using MyCommonWPFControls;

namespace MyUILibrary.DataLinkArea
{
    public class DataLinkArea : I_DataLinkArea
    {
        public DataLinkAreaInitializer AreaInitializer
        {
            set; get;
        }

        //public DataLinkDTO DataLink
        //{
        //    set; get;
        //}

        public I_View_DataLinkArea View
        {
            set; get;
        }
        public bool SecurityNoAccess { set; get; }
        public bool SecurityReadonly { set; get; }
        public bool SecurityEdit { set; get; }
        I_EditEntityAreaOneData FirstSideEditEntityArea { set; get; }
        I_EditEntityAreaOneData SecondSideEditEntityArea { set; get; }
        public List<DataLinkDTO> DataLinks { get; set; }
        public DataLinkDTO SelectedDataLink { get; set; }
        DP_DataView FirstData;
        // چون جایی نداریم فعلا که دوتا داده انتخاب کنیم و ارتباطشون رو بخوایم, DP_DataView otherData)
        DP_DataView OtherData;
        MySearchLookup dataLinkSearchLookup;
        public DataLinkArea(DataLinkAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfDataLinkArea();
            View.DataLinkConfirmed += View_DataLinkConfirmed;
            //     View.DataLinkChanged += View_DataLinkChanged;

            dataLinkSearchLookup = new MySearchLookup();
            dataLinkSearchLookup.DisplayMember = "Name";
            dataLinkSearchLookup.SelectedValueMember = "ID";
            dataLinkSearchLookup.SearchFilterChanged += dataLinkSearchLookup_SearchFilterChanged;
            dataLinkSearchLookup.SelectionChanged += dataLinkSearchLookup_SelectionChanged;
            View.AddDataLinkSelector(dataLinkSearchLookup);
            FirstData = AreaInitializer.FirstDataItem;
            //OtherData = AreaInitializer.OtherDataItem;
            if (AreaInitializer.DataLinkID != 0)
            {
                dataLinkSearchLookup.SelectedValue = AreaInitializer.DataLinkID;
                dataLinkSearchLookup.IsEnabledLookup = false;
            }
            else if (AreaInitializer.EntityID != 0)
            {
                DataLinks = AgentUICoreMediator.GetAgentUICoreMediator.DataLinkManager.GetDataLinks(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(),AreaInitializer.EntityID);
                dataLinkSearchLookup.ItemsSource = DataLinks;
                dataLinkSearchLookup.SearchIsEnabled = false;
                if (DataLinks.Count == 1)
                    dataLinkSearchLookup.SelectedItem = DataLinks[0];
            }


            //ManageSecurity();
        }

        private void dataLinkSearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var dataLink = AgentUICoreMediator.GetAgentUICoreMediator.DataLinkManager.GetDataLink(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue));
                    e.ResultItemsSource = new List<DataLinkDTO> { dataLink };
                }
                else
                {
                    var dataLinks = AgentUICoreMediator.GetAgentUICoreMediator.DataLinkManager.SearchDatalinks(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue);
                    e.ResultItemsSource = dataLinks;
                }
            }
        }
        private void dataLinkSearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {
                var dataLink = e.SelectedItem as DataLinkDTO;
                View.EnabaleDisabeViewSection(true);
                SetDataLink(dataLink.ID);
            }
            else
            {
                View.EnabaleDisabeViewSection(false);
            }
        }

        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID, false);
        //        return _Permission;
        //    }
        //}
        //private void ManageSecurity()
        //{
        //    if (Permission.GrantedActions.Any(x => x == SecurityAction.NoAccess))
        //    {
        //        SecurityNoAccess = true;
        //    }
        //    else
        //    {
        //        if (Permission.GrantedActions.Any(x => x == SecurityAction.EditAndDelete || x == SecurityAction.Edit))
        //        {
        //            SecurityEdit = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.ReadOnly))
        //        {
        //            SecurityReadonly = true;
        //        }
        //        else
        //            SecurityNoAccess = true;
        //    }
        //    ImposeSecurity();
        //}

        //private void ImposeSecurity()
        //{
        //    if (SecurityNoAccess)
        //    {
        //        View = null;
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //    }
        //    else
        //    {
        //        if (!SecurityReadonly && !SecurityEdit)
        //        {
        //            View = null;
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");

        //        }
        //    }
        //}
        //private void View_DataLinkChanged(object sender, EventArgs e)
        //{

        //}

        private void SetDataLink(int dataLinkID)
        {
            SelectedDataLink = AgentUICoreMediator.GetAgentUICoreMediator.DataLinkManager.GetDataLink(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), dataLinkID);
            View.ClearEntityViews();
            if (SelectedDataLink != null)
            {
                EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
                editEntityAreaInitializer1.EntityID = SelectedDataLink.FirstSideEntityID;
                editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
                editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
                var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
                if (FirstSideEditEntityAreaResult.Item1 != null)
                {
                    FirstSideEditEntityArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                    FirstSideEditEntityArea.SetAreaInitializer(editEntityAreaInitializer1);
                    View.SetFirstSideEntityView(FirstSideEditEntityArea.TemporaryDisplayView, FirstSideEditEntityArea.SimpleEntity.Alias);
                }
                else
                {
                    if (!string.IsNullOrEmpty(FirstSideEditEntityAreaResult.Item2))
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage(FirstSideEditEntityAreaResult.Item2);
                    return;

                }
                EditEntityAreaInitializer editEntityAreaInitializer2 = new EditEntityAreaInitializer();
                editEntityAreaInitializer2.EntityID = SelectedDataLink.SecondSideEntityID;
                editEntityAreaInitializer2.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
                editEntityAreaInitializer2.DataMode = CommonDefinitions.UISettings.DataMode.One;
                var SecondSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer2);
                if (SecondSideEditEntityAreaResult.Item1 != null)
                {
                    SecondSideEditEntityArea = SecondSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                    SecondSideEditEntityArea.SetAreaInitializer(editEntityAreaInitializer2);
                    View.SetSecondSideEntityView(SecondSideEditEntityArea.TemporaryDisplayView, SecondSideEditEntityArea.SimpleEntity.Alias);
                }
                else
                    return;
                bool firstDataSetToFirst = false;
                bool firstDataSetToSecond = false;
                if (FirstData != null)
                {
                    if (SelectedDataLink.FirstSideEntityID == FirstData.TargetEntityID)
                    {
                        FirstSideEditEntityArea.ClearData(false);
                        FirstSideEditEntityArea.ShowDataFromExternalSource(FirstData);
                        firstDataSetToFirst = true;
                    }
                    else if (SelectedDataLink.SecondSideEntityID == FirstData.TargetEntityID)
                    {
                        SecondSideEditEntityArea.ClearData(false);
                        SecondSideEditEntityArea.ShowDataFromExternalSource(FirstData);
                        firstDataSetToSecond = true;
                    }
                }
                if (OtherData != null)
                {
                    if (!firstDataSetToFirst && SelectedDataLink.FirstSideEntityID == OtherData.TargetEntityID)
                    {
                        FirstSideEditEntityArea.ClearData(false);
                        FirstSideEditEntityArea.ShowDataFromExternalSource(OtherData);
                    }
                    else if (!firstDataSetToSecond && SelectedDataLink.SecondSideEntityID == OtherData.TargetEntityID)
                    {
                        SecondSideEditEntityArea.ClearData(false);
                        SecondSideEditEntityArea.ShowDataFromExternalSource(OtherData);
                    }
                }
            }
        }

        private void View_DataLinkConfirmed(object sender, EventArgs e)
        {
            if (SelectedDataLink == null ||
                FirstSideEditEntityArea.AreaInitializer.Datas.Count == 0
                || SecondSideEditEntityArea.AreaInitializer.Datas.Count == 0)
                return;
            FirstData = FirstSideEditEntityArea.AreaInitializer.Datas[0].DataView;
            OtherData = SecondSideEditEntityArea.AreaInitializer.Datas[0].DataView;
            List<DataLinkItem> dataLinkItems = new List<DataLinkItem>();

            var fItems = new List<DataLinkItemGroups>();
            //البته از هر دو طرف میشه به طرف دیگر رسید
            //بهتره همین طور باشه چون برای لینک سرورها کافیه سرورهای طرف اول به طرف دوم لینک داشته باشد
            //اینطوری مفهوم تر است
            foreach (var tail in SelectedDataLink.RelationshipsTails)
            {
                var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(OtherData, tail.RelationshipTail.ReverseRelationshipTail);
                foreach (var item in FirstData.KeyProperties)
                {
                    searchDataTuple.Phrases.Add(new SearchProperty() { ColumnID = item.ColumnID, Value = item.Value });
                }
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                //سکوریتی داده اعمال میشود
                var existsRequest = new DR_SearchExistsRequest(requester);
                existsRequest.EntityID = SecondSideEditEntityArea.AreaInitializer.EntityID;
                existsRequest.SearchDataItems = searchDataTuple;
                var exists = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchExistsRequest(existsRequest);
                if (exists.Result == Enum_DR_ResultType.SeccessfullyDone)
                {
                    if (exists.ExistsResult)
                    {
                        //البته از هر دو طرف میشه به طرف دیگر رسید
                        //بهتره همین طور باشه چون برای لینک سرورها کافیه  در تیل سرورهای طرف اول هر رابطه به طرف دوم رابطه لینک داشته باشد
                        //اینطوری مفهوم تر است
                        fItems.Add(GetIncludedDataLinkItems(tail.RelationshipTail, FirstData, OtherData, 0));
                    }
                }
                else
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(exists.Message, exists.Details, MyUILibrary.Temp.InfoColor.Red);
                }
            }

            var firstSidelinkItem = new MyUILibrary.DataLinkArea.DataLinkItem();
            firstSidelinkItem.DataItem = FirstData;
            firstSidelinkItem.View = GetDataViewItem(firstSidelinkItem.DataItem);

            var secondSidelinkItem = new MyUILibrary.DataLinkArea.DataLinkItem();
            secondSidelinkItem.DataItem = OtherData;
            secondSidelinkItem.View = GetDataViewItem(secondSidelinkItem.DataItem);

            foreach (var pitem in fItems)
                foreach (var item in pitem.Items)
                    item.View = GetDataViewItem(item.DataItem);

            //بعدا تست شود ظاهر فرم








            //foreach (var item in fItems.Where(x => x != secondSidelinkItem && !fItems.Any(y => y.ParentDataLinkItem == x)))
            //{
            //    //   View.AddLink(item.View, secondSidelinkItem.View);
            //}

            //var groupedList = fItems.GroupBy(x => x.RelationshipTail);
            //foreach (var tail in groupedList)
            //{
            //    //object panel = View.GenerateTailPanel();
            //    foreach (var item in tail.Where(x => x.Level == 0))
            //    {
            //        View.AddFirstLevelDataLinkItem(item.View);
            //        CheckDependentItems(tail, item);
            //    }
            //}
            List<DataLinkItemViewGroups> viewGroups = new List<DataLinkItemViewGroups>();
            //var listViewGroups = fItems.Select(x => x.Items.Select(y => y.View).ToList()).ToList();
            //var listViewRelations = fItems.Select(x => x.Relations.Select(y => new Tuple<I_DataViewItem, I_DataViewItem>(y.Item1.View, y.Item2.View)).ToList()).ToList();
            foreach (var pitem in fItems)
            {
                DataLinkItemViewGroups viewGroup = new DataLinkItemViewGroups();
                viewGroup.Views.AddRange(pitem.Items.Select(x => x.View).ToList());
                viewGroup.ViewRelations.AddRange(pitem.Relations.Select(x => new Tuple<I_DataViewItem, I_DataViewItem>(x.Item1.View, x.Item2.View)).ToList());
                viewGroups.Add(viewGroup);
            }

            View.ShowDiagram(viewGroups, firstSidelinkItem.View, secondSidelinkItem.View);
        }

        //private void CheckDependentItems(List<DataLinkItem> tail, DataLinkItem parentItem)
        //{
        //    //   return;
        //    foreach (var item in tail.Where(x => x.ParentDataLinkItem == parentItem))
        //    {
        //        View.AddLink(parentItem.View, item.View);
        //        CheckDependentItems(tail, item);
        //    }
        //}

        private I_DataViewItem GetDataViewItem(DP_DataView item)
        {
            I_DataViewItem dataViewItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDataViewItem();
            //     درست شود
            dataViewItem.DataView = item;
            dataViewItem.InfoClicked += DataViewItem_InfoClicked;
            //dataViewItem.Selected += DataViewItem_Selected;
            //foreach (var column in item.Properties.Take(3))
            //{
            //    dataViewItem.AddTitleRow(column.Name, column.Value);
            //}
            if (!string.IsNullOrEmpty(item.TargetEntityAlias))
                dataViewItem.Title = item.TargetEntityAlias;
            dataViewItem.Body = item.ViewInfo;
            return dataViewItem;
        }

        //private void DataViewItem_Selected(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void DataViewItem_InfoClicked(object sender, EventArgs e)
        {
            //فانکشن شود در دیتا ویو هم هست
            var dataViewItem = (sender as I_DataViewItem);
            if (dataViewItem != null)
            {
                //var menus = GetDataViewItemMenus(dataViewItem);
                //dataViewItem.ShowDataViewItemMenus(menus);
                var menuInitializer = new DataMenuAreaInitializer();
                //menuInitializer.HostDataViewArea = this;
                //menuInitializer.HostDataViewItem = dataViewItem;
                menuInitializer.SourceView = sender;

                //DP_DataView dataRepository = new DP_DataView();
                //dataRepository.TargetEntityID = dataViewItem.DataView.TargetEntityID;
                var dataView = dataViewItem.DataView;
                //foreach (var key in dataViewItem.DataView.Properties.Where(x => x.IsKey))
                //{
                //    dataRepository.AddProperty(new ColumnDTO() { ID = key.ColumnID }, key.Value);
                //}
                menuInitializer.DataItem = dataView;


                AgentUICoreMediator.GetAgentUICoreMediator.ShowMenuArea(menuInitializer);

            }
        }

        private DataLinkItemGroups GetIncludedDataLinkItems(EntityRelationshipTailDTO relationshipTail, DP_DataView relationshipFirstData, DP_DataView targetData, int level, DataLinkItem parentDataLinkItem = null, DataLinkItemGroups result = null)
        {
            if (result == null)
                result = new DataLinkItemGroups();
            if (relationshipTail != null && relationshipTail.ChildTail != null)
            {
                RelationshipTailDataManager relationshipTailDataManager = new RelationshipTailDataManager();

                //var firstData = new DP_DataRepository();
                //firstData.TargetEntityID = FirstData.TargetEntityID;
                //firstData.DataView = FirstData;
                //foreach (var key in FirstData.KeyProperties)
                //{
                //    firstData.AddProperty(new ColumnDTO() { ID = key.ColumnID }, key.Value);
                //}
                var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipDataManager.GetSecondSideSearchDataItemByRelationship(relationshipFirstData, relationshipTail.Relationship.ID);
                var searchPhraseToOtherData = relationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(targetData, relationshipTail.ChildTail.ReverseRelationshipTail);
                searchDataTuple.Phrases.AddRange(searchPhraseToOtherData.Phrases);
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                //سکوریتی داده اعمال میشود
                var searchRequest = new DR_SearchViewRequest(requester, searchDataTuple);
                //searchRequest.EntityID = parentTail.RelationshipTargetEntityID;
                var searchResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(searchRequest);

                if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                {
                    foreach (var item in searchResult.ResultDataItems)
                    {
                        //درست شود
                        DataLinkItem linkItem = new MyUILibrary.DataLinkArea.DataLinkItem();
                        linkItem.DataItem = item;
                        // linkItem.Level = level;
                        //linkItem.ParentDataLinkItem = parentDataLinkItem;
                        //  linkItem.RelationshipTail = mainTail;
                        result.Items.Add(linkItem);
                        if (parentDataLinkItem != null)
                            result.Relations.Add(new Tuple<DataLinkItem, DataLinkItem>(parentDataLinkItem, linkItem));
                        GetIncludedDataLinkItems(relationshipTail.ChildTail, item, targetData, level + 1, linkItem, result);
                    }
                }
                else
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(searchResult.Message, searchResult.Details, MyUILibrary.Temp.InfoColor.Red);
                }

            }
            return result;
        }
    }
    public class DataLinkItemGroups
    {
        public DataLinkItemGroups()
        {
            Items = new List<MyUILibrary.DataLinkArea.DataLinkItem>();
            Relations = new List<Tuple<DataLinkItem, DataLinkItem>>();
        }
        public List<DataLinkItem> Items { set; get; }
        public List<Tuple<DataLinkItem, DataLinkItem>> Relations { set; get; }

    }

    public class DataLinkItem
    {
        //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
        //     public int Level { set; get; }
        //public DataLinkItem ParentDataLinkItem { set; get; }
        public DP_DataView DataItem { set; get; }
        //      public bool IsLastLevel { set; get; }
        public I_DataViewItem View { set; get; }
    }
}
