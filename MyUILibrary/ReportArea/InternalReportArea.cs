using AutoMapper;
using CommonDefinitions.UISettings;
using ModelEntites;
using MyReportManager;

using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.EntitySearchArea;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Telerik.Reporting;

namespace MyUILibrary.EntityArea
{
    public class InternalReportArea : I_InternalReportArea
    {
        ReportResolver reportResolver = new ReportResolver();
        //  EntityReportDTO Report { set; get; }
        //    DP_SearchRepository InitialSearchRepository { set; get; }
        public bool SecurityNoAccess { set; get; }
        public bool SecurityReadonly { set; get; }
        public bool SecurityEdit { set; get; }
        public I_GeneralEntitySearchArea GeneralEntitySearchArea { set; get; }
        public object MainView { set; get; }
        public InternalReportArea(InternalReportAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            var entityReport = AgentUICoreMediator.GetAgentUICoreMediator.ReportManager.GetReport(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), initParam.ReportID);
            if (entityReport == null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("دسترسی به گزارش به شناسه" + " " + initParam.ReportID + " " + "امکانپذیر نمی باشد", "", Temp.InfoColor.Red);
                return;
            }

            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfInternalReportArea();
            View.Title = AreaInitializer.Title;
            View.OrderColumnsChanged += View_OrderColumnsChanged;
            View.ExceptionThrown += View_ExceptionThrown;

            GeneralEntitySearchAreaInitializer selectAreaInitializer = new GeneralEntitySearchAreaInitializer();
            selectAreaInitializer.ExternalView = View;
            selectAreaInitializer.EntityID = AreaInitializer.EntityID;
            if (AreaInitializer.EntityID != 0)
                selectAreaInitializer.LockEntitySelector = true;
            if (initParam.InitialSearchRepository != null && !initParam.ShowInitializeSearchRepository)
                selectAreaInitializer.PreDefinedSearch = AreaInitializer.InitialSearchRepository;
            GeneralEntitySearchArea = new GeneralEntitySearchArea();
            GeneralEntitySearchArea.SearchDataDefined += GeneralEntitySearchArea_SearchDataDefined;
            GeneralEntitySearchArea.SetInitializer(selectAreaInitializer);
            GeneralEntitySearchArea.EnableDisableSearchArea(AreaInitializer.UserCanChangeSearch);
            MainView = GeneralEntitySearchArea.View;
            //View.AddGenerealSearchAreaView(GeneralEntitySearchArea.View);

            if (initParam.ReportType == SearchableReportType.ListReport)
                SetEntityOrderColumns();
            else
                View.OrderColumnsVisibility = false;

            if (AreaInitializer.InitialSearchRepository != null && initParam.ShowInitializeSearchRepository)
                SetReport(AreaInitializer.InitialSearchRepository);
        }


        private void View_OrderColumnsChanged(object sender, EventArgs e)
        {
            if (SearchRepository != null)
                SetReport(SearchRepository);
        }

        private void SetEntityOrderColumns()
        {
            View.OrderColumnsVisibility = true;
            List<EntityListViewColumnsDTO> listColumns = null;
            if (AreaInitializer.ReportType == SearchableReportType.ListReport)
            {
                var report = AgentUICoreMediator.GetAgentUICoreMediator.ReportManager.GetListReport(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.ReportID);
                listColumns = report.EntityListView.EntityListViewAllColumns;
            }
            //else if (AreaInitializer.Report.Type == ReportType.ListReportGrouped)
            //{
            //    var report = AgentUICoreMediator.GetAgentUICoreMediator.ReportManager.GetListReportGrouped(AreaInitializer.Report.ID);
            //    listColumns = report.EntityListReport.EntityListView.EntityListViewAllColumns;
            //}
            List<Tuple<int, string>> columns = new List<Tuple<int, string>>();

            foreach (var col in listColumns)
            {
                columns.Add(new Tuple<int, string>(col.ColumnID, col.Column.Alias));
            }
            View.SetOrderColumns(columns);
            View.SetOrderSorts(new List<string>() { "Ascending", "Descending" });
        }
        private void GeneralEntitySearchArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            SetReport(e.SearchItems);
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
        //public bool InitialSearchShouldBeIncluded
        //{
        //    set; get;
        //}
        DP_SearchRepository SearchRepository { set; get; }
        private void SetReport(DP_SearchRepository searchRepository)
        {
            SearchRepository = searchRepository;

            //بالاخره درست شد. بعد از مدتها که اومدم سراغ گزارشات خطا میداد 
            // پروژه رست سرویس از اول ایجاد شد. از تمپلیت خود تلریک استفاده شد تو اد نیو پروژکت .فقط ریزولور رو اصلاح کردم و تو وب کانفیک کانکشن استرینگ رو ست کردم
            //برای ویوئر هم مجددا با رایت کلیک و اد کردن یک ریپورت ویوئر wpf 
            //خودش یسری dll اشافه میکنه و همچنین app.config رو دستکاری میکنه
            //یه مشکل با ورژن Newtonsoft.Json بود که با اصلاح خط زیر حل شد
            //        < dependentAssembly >
            //          < assemblyIdentity name = "Newtonsoft.Json" publicKeyToken = "30ad4fe6b2a6aeed" culture = "neutral" />
            //          < bindingRedirect oldVersion = "0.0.0.0-9.0.0.0" newVersion = "9.0.0.0" />
            //      </ dependentAssembly >

            //به نظرم ریپورت اریا اینترنال و اکسترنال رو جدا کنم
            if (searchRepository != null)
            {

                var request = new RR_ReportSourceRequest(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
                request.ReportID = AreaInitializer.ReportID;
                request.SearchDataItems = searchRepository;
                if (View.OrderColumnsVisibility)
                {
                    request.OrderByEntityViewColumnID = View.GetOrderColumnID;
                    if (View.GetSortText == "Ascending")
                        request.SortType = Enum_OrderBy.Ascending;
                    else if (View.GetSortText == "Descending")
                        request.SortType = Enum_OrderBy.Descending;
                }
                var reportEngineConnection = "engine=RestService;uri=http://localhost:12345/api/reports;useDefaultCredentials=True";
                List<Type> types = new List<Type>();
                types.Add(typeof(DP_SearchRepository));
                //types.Add(typeof(LogicPhrase));
                types.Add(typeof(SearchProperty));
                RemoveUnWantedTypes(request.SearchDataItems);
                var uri = SerializeObject<RR_ReportSourceRequest>(request, types);
                //   uri = "MyReportRestServices.Report1, MyReportRestServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                var rpSource = new UriReportSource() { Uri = uri };
                //rpSource.Parameters.Add(new Parameter() { Name = "bb", Value = request });
                View.SetReportSource(null, null);
                View.SetReportSource(reportEngineConnection, rpSource);

            }
            //var rpSource = reportResolver.GetReportSource(request);
            //View.SetReportSource(rpSource);
        }
        private void View_ExceptionThrown(object sender, Exception e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در نمایش گزارش", e.Message);
            View.SetReportSource(null, null);

        }

        private void RemoveUnWantedTypes(DP_SearchRepository searchRepository)
        {
            if (searchRepository.SourceRelationship != null)
            {
                foreach (var column in searchRepository.SourceRelationship.RelationshipColumns)
                {
                    if (column.FirstSideColumn != null)
                        column.FirstSideColumn.DotNetType = null;
                    if (column.SecondSideColumn != null)
                        column.SecondSideColumn.DotNetType = null;
                }
            }
            foreach (var item in searchRepository.Phrases)
            {
                if (item is DP_SearchRepository)
                    RemoveUnWantedTypes(item as DP_SearchRepository);
                else if (item is LogicPhrase)
                {
                    foreach (var log in (item as LogicPhrase).Phrases)
                    {
                        if (log is DP_SearchRepository)
                            RemoveUnWantedTypes(log as DP_SearchRepository);
                    }
                }
            }
        }

        public string SerializeObject<T>(object toSerialize, List<Type> types)
        {
            XmlSerializer xmlSerializer = null;
            if (types == null)
                xmlSerializer = new XmlSerializer(toSerialize.GetType());
            else
                xmlSerializer = new XmlSerializer(toSerialize.GetType(), types.ToArray());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        //private void View_SearchCommandRequested(object sender, EventArgs e)
        //{
        //    if (SearchEntityArea == null)
        //    {
        //        SearchEntityArea = new SearchEntityArea();
        //        var searchViewInitializer = new SearchEntityAreaInitializer();

        //        searchViewInitializer.EntityID = AreaInitializer.EntityID;
        //        SearchEntityArea.SetAreaInitializer(searchViewInitializer);
        //        //SearchEntityArea.GenerateSearchView();
        //        SearchEntityArea.SearchDataDefined += SearchEntityArea_SearchDataDefined;

        //    }
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(SearchEntityArea.SearchView, "جستجو");
        //}
        //private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        //{
        //    SetReport(e.SearchItems);

        //}
        //private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        //{
        //    //RR_ReportSourceRequest request = new RR_ReportSourceRequest();
        //    //request.ReportID = AreaInitializer.TemplateReport.ID;
        //    //request.SearchDataItems = e.SearchItems;
        //    //var resportSource=  AgentUICoreMediator.GetAgentUICoreMediator.SendReportSourceRequest(request);
        //    //string reportEngine = "engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False";
        //    //var rs = new UriReportSource() { Uri = "aa" };
        //    //AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowTestReport(reportEngine, rs);

        //    ////// DR_SearchViewRequest request = new DR_SearchViewRequest();
        //    ////// request.SearchDataItems = e.SearchItems;
        //    ////// request.EntityID = AreaInitializer.TemplateReport.TableDrivedEntityID;
        //    ////// //اینج کوئری
        //    ////// var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(request);

        //    //////ViewEntityArea.ShowReport(reuslt.ResultDataItems, true);
        //}
        //private DP_SearchRepository FindOrCreateSearchItem(List<DP_SearchRepository> dataList, DP_SearchRepository mainItem, EntityRelationshipTailDTO searchRelationshipTail)
        //{
        //    if (searchRelationshipTail == null)
        //        return mainItem;
        //    else
        //    {  //بازنویسی شود
        //       ////var foundItem = dataList.FirstOrDefault(x => x.SourceRelatedData == mainItem && x.SourceRelationID == searchRelationshipTail.RelationshipID);
        //       ////if (foundItem == null)
        //       ////{
        //       ////    //sourcecolumn , target کجا ست میشوند؟؟
        //       ////    //فهمیدم اول کار  CreateDefaultData(); برای هر فرم صدا زده میشود
        //       ////    //بهتر شود که دوباره کاری نشود
        //       ////    foundItem = new DP_SearchRepository();
        //       ////    foundItem.SourceRelatedData = mainItem;
        //       ////    foundItem.TargetEntityID = searchRelationshipTail.RelationshipTargetEntityID;
        //       ////    foundItem.SourceToTargetRelationshipType = searchRelationshipTail.SourceToTargetRelationshipType;
        //       ////    foundItem.SourceToTargetMasterRelationshipType = searchRelationshipTail.SourceToTargetMasterRelationshipType;
        //       ////    dataList.Add(foundItem);
        //       ////    return FindOrCreateSearchItem(dataList, foundItem, searchRelationshipTail.ChildTail);
        //       ////}
        //       ////else
        //       ////    return foundItem;
        //        return null;

        //    }
        //}

        //private DP_EntityResult GetFullEntity(int entityID2)
        //{
        //    var request = new DP_EntityRequest();
        //    request.EntityID = entityID2;
        //    return AgentUICoreMediator.GetAgentUICoreMediator.GetEntity(request, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, true, true, true, true, true, true);
        //}
        //private SearchEntityArea GenereateSearchArea()
        //{
        //    var searchEntityArea = new SearchEntityArea();
        //    var searchViewInitializer = new SearchEntityAreaInitializer();
        //    searchViewInitializer.SearchEntity = GetFullEntity(AreaInitializer.TemplateReport.TableDrivedEntityID).Entity;
        //    searchEntityArea.SetAreaInitializer(searchViewInitializer);
        //    searchEntityArea.GenerateSearchView();
        //    return searchEntityArea;
        //}




        public I_View_InternalReportArea View
        {
            set; get;
        }

        //public List<I_Command> SearchViewCommands
        //{
        //    set; get;
        //}

        //public I_SearchEntityArea SearchEntityArea
        //{
        //    set; get;
        //}



        public InternalReportAreaInitializer AreaInitializer
        {
            set; get;
        }


        //public void ShowTemporarySearchView()
        //{
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(View, AreaInitializer.TemplateReport.ReportTitle);
        //}


    }
}
