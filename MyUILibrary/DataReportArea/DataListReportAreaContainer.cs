using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyUILibraryInterfaces.DataReportArea;

namespace MyUILibrary.DataReportArea
{

    اینجا
    public class DataListReportAreaContainer : I_DataListReportAreaContainer
    {
        public DataListReportAreaContainer()
        {
            DataListReportAreas = new List<I_DataListReportArea>();
        }
        public DataListReportAreaContainerInitializer AreaInitializer
        {
            set; get;
        }


        public I_SearchEntityArea SearchEntityArea
        {
            set; get;
        }

        public DP_SearchRepository SearchRepository
        {
            set; get;
        }

        public List<I_DataListReportArea> DataListReportAreas
        {
            set; get;
        }
        public I_DataListReportArea CurrentDataListReportArea { set; get; }
        public I_View_DataListReportAreaContainer View
        {
            set; get;
        }

        public void SetAreaInitializer(DataListReportAreaContainerInitializer initParam)
        {
            AreaInitializer = initParam;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDataListReportAreaContainer();
            AddDataListReportArea(initParam.EntitiyID, initParam.Title, initParam.SearchRepository, false);

        }

        private void AddDataListReportArea(int entityID, string title, DP_SearchRepository searchRepository, bool initialSearchShouldBeIncluded, RelationshipDTO causingRelationship = null, EntityRelationshipTailDTO causingRelationshipTail = null)
        {
            var dataListReportArea = new DataListReportArea();
            dataListReportArea.InitialSearchShouldBeIncluded = initialSearchShouldBeIncluded;
            dataListReportArea.RelatedDataReportArearequested += FirstDataListReportArea_RelatedDataListReportArearequested;
            dataListReportArea.DataItemsSearchedByUser += DataListReportArea_DataItemsSearchedByUser;
            var firstInit = new DataListReportAreaInitializer();
            firstInit.SearchRepository = searchRepository;
            firstInit.EntitiyID = entityID;
            firstInit.Title = title;
            firstInit.CausingRelationship = causingRelationship;
            firstInit.CausingRelationshipTail = causingRelationshipTail;
            dataListReportArea.SetAreaInitializer(firstInit);
            View.AddDataListReportArea(dataListReportArea.View);

            if (CurrentDataListReportArea == null)
            {
                CurrentDataListReportArea = dataListReportArea;
                DataListReportAreas.Add(dataListReportArea);
            }
            else
            {
                var currentIndex = DataListReportAreas.IndexOf(CurrentDataListReportArea);
                DataListReportAreas.Insert(currentIndex + 1, dataListReportArea);
                CurrentDataListReportArea = dataListReportArea;
            }
            SetLinks();
        }

        private void DataListReportArea_DataItemsSearchedByUser(object sender, EventArgs e)
        {
            var hostDataListReportArea = sender as DataListReportArea;
            hostDataListReportArea.DefaultDataReportItem = null;
            CurrentDataListReportArea = hostDataListReportArea;
            SetLinks();
        }

        private void FirstDataListReportArea_RelatedDataListReportArearequested(object sender, DataReportAreaRequestedArg e)
        {
            var hostDataListReportArea = sender as DataListReportArea;
            hostDataListReportArea.DefaultDataReportItem = e.SourceDataReportItem;
            AddDataListReportArea(e.EntitiyID, e.Title, e.SearchRepository, true, e.Relationship, e.RelationshipTail);

        }

        private void SetLinks()
        {
            var lastindex = DataListReportAreas.IndexOf(CurrentDataListReportArea);
            if (lastindex != -1)
            {
                List<I_DataListReportArea> listRemove = new List<I_DataListReportArea>();
                var index = 0;
                foreach (var item in DataListReportAreas)
                {
                    if (index > lastindex)
                        listRemove.Add(item);
                    index++;
                }
                foreach (var item in listRemove)
                {
                    DataListReportAreas.Remove(item);
                }
            }
            List<DataReportLink> links = new List<DataReportLink>();
            foreach (var item in DataListReportAreas)
            {
                DataReportLink link = new DataReportLink();
                link.Title = item.AreaInitializer.Title.ToString();
                //link.Tooltip item.AreaInitializer
                link.DataReportLinkClicked += (sender, e) => Link_DataListReportLinkClicked(sender, e, item);
                links.Add(link);
            }
            View.ShowLinks(links);
        }

        private void Link_DataListReportLinkClicked(object sender, EventArgs e, I_DataListReportArea dataListReportArea)
        {
            CurrentDataListReportArea = dataListReportArea;
            //LastDataListReportArea = dataListReportArea;
            View.AddDataListReportArea(dataListReportArea.View);
            if (dataListReportArea.DefaultDataReportItem != null)
                dataListReportArea.View.BringIntoView(dataListReportArea.DefaultDataReportItem);
            //SetLinks();
        }
    }

}
