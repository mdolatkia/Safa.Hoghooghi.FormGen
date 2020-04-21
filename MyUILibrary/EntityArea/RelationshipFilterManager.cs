using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyUILibrary.EntityArea.Commands;
using CommonDefinitions.UISettings;

namespace MyUILibrary.EntityArea
{
    public class RelationshipFilterManager : I_RelationshipFilterManager
    {
        //public List<RelationshipFilterSource> ListRelationshipsFilters = new List<RelationshipFilterSource>();
        I_EditEntityArea EditArea { set; get; }
        public RelationshipFilterManager(I_EditEntityArea editArea)
        {
            EditArea = editArea;
            //editArea.DataItemLoaded += EditArea_DataItemLoaded;

            //shown استفاده شود
            if (editArea is I_EditEntityAreaMultipleData)
            {
                (editArea as I_EditEntityAreaMultipleData).DataItemRemoved += UIActionActivityManager_DataItemRemoved;
            }
        }

        private void EditArea_DataItemLoaded(object sender, EditAreaDataItemLoadedArg e)
        {
            //////if (SearchInitialyAreas == null)
            //////    GetInitialySearchEditAreas();

            //////if (SearchInitialyAreas == null || SearchInitialyAreas.Count == 0)
            //////{
            //////    EditArea.DataItemShown -= EditArea_DataItemShown;
            //////    return;
            //////}

            //////foreach (var item in SearchInitialyAreas.Where(x => !x.Item2.Any()))
            //////{
            //////    if (!item.Item1.SearchViewEntityArea.SearchInitialyDone)
            //////        item.Item1.SearchViewEntityArea.SearchInitialy();
            //////}

            //////if (!SearchInitialyAreas.Any(x => x.Item2.Any()))
            //////{
            //////    EditArea.DataItemShown -= EditArea_DataItemShown;
            //////    return;
            //////}

            //////if (sender is I_EditEntityAreaOneData)
            //////{
            //////    foreach (var item in ListRelationshipsFilters.ToList())
            //////    {
            //////        if (item.SourceEditArea == sender && e.DataItem != item.DataItem)
            //////            RemoveRelationshipsFilters(item);
            //////    }
            //////}

            //////var relFilter = new RelationshipFilterSource()
            //////{
            //////    SourceEditArea = (sender as I_EditEntityArea),
            //////    DataItem = e.DataItem
            //////};


            //////if (ListRelationshipsFilters.Any(x => x.DataItem == e.DataItem))
            //////{
            //////    //              throw new Exception("Asdasd");

            //////}
            //////ListRelationshipsFilters.Add(relFilter);

            //////CheckRelationships(relFilter);
            ////////if (testedDataItems.Any(x => x == e.DataItem))
            ////////    return;
            ////////testedDataItems.Add(e.DataItem);
            //////var dataItem = e.DataItem;
            //////List<Tuple<string, int>> relColumns = GetDependentColumns();
            //////if (relColumns.Any())
            //////{
            //////    relFilter.RegisterEvent();
            //////    relFilter.RelatedDataChanged += DataItem_RelatedDataChanged;
            //////    foreach (var column in relColumns)
            //////    {
            //////        dataItem.AddDataObserverForColumn("relfilter", column.Item1, column.Item2);

            //////    }
            //////}
        }

        private void UIActionActivityManager_DataItemRemoved(object sender, EditAreaDataItemArg e)
        {
            //foreach (var item in ListRelationshipsFilters.ToList())
            //{
            //    if (item.SourceEditArea == sender && e.DataItem == item.DataItem)
            //        RemoveRelationshipsFilters(item);
            //}

        }


        private void EditArea_DataItemShown(object sender, EditAreaDataItemArg e)
        {

        }
        List<Tuple<I_EditEntityArea, List<RelationshipFilterDTO>>> SearchInitialyAreas;
        private List<Tuple<I_EditEntityArea, List<RelationshipFilterDTO>>> GetInitialySearchEditAreas()
        {
            if (SearchInitialyAreas == null)
            {
                SearchInitialyAreas = new List<Tuple<I_EditEntityArea, List<RelationshipFilterDTO>>>();
                List<RelationshipColumnControl> relationships = new List<RelationshipColumnControl>();
                if (EditArea is I_EditEntityAreaOneData)
                {
                    foreach (var item in (EditArea as I_EditEntityAreaOneData).RelationshipColumnControls)
                        relationships.Add(item);
                }
                else if (EditArea is I_EditEntityAreaMultipleData)
                {
                    foreach (var item in (EditArea as I_EditEntityAreaMultipleData).RelationshipColumnControls)
                        relationships.Add(item);
                }
                foreach (var item in relationships.ToList())
                {
                    if (item.EditNdTypeArea.SimpleEntity.SearchInitially == true || (item.EditNdTypeArea.AreaInitializer.SourceRelation != null && item.EditNdTypeArea.AreaInitializer.SourceRelation.Relationship.SearchInitially))
                    {
                        var relFilters = AgentUICoreMediator.GetAgentUICoreMediator.relationshipFilterManagerService.GetRelationshipFilters(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), item.Relationship.ID);
                        //if (relFilters.Any())
                        //{
                        item.EditNdTypeArea.SearchViewEntityArea.RelationshipFilters = relFilters;
                        SearchInitialyAreas.Add(new Tuple<I_EditEntityArea, List<RelationshipFilterDTO>>(item.EditNdTypeArea, relFilters));
                        //}
                    }
                }
            }
            return SearchInitialyAreas;
        }

        private List<Tuple<string, int>> GetDependentColumns()
        {
            List<Tuple<string, int>> relColumns = new List<Tuple<string, int>>();

            foreach (var editArea in SearchInitialyAreas)
            {
                if (editArea.Item1.SearchViewEntityArea.RelationshipFilters != null)
                {
                    //List<int> columnsIds = new List<int>();

                    foreach (var relTailID in editArea.Item1.SearchViewEntityArea.RelationshipFilters.GroupBy(x => x.ValueRelationshipTailID))
                    {
                        string tail = "";
                        if (relTailID.Key != 0)
                        {
                            tail = editArea.Item1.SearchViewEntityArea.RelationshipFilters.First(x => x.ValueRelationshipTailID == relTailID.Key).ValueRelationshipTail.RelationshipIDPath;
                        }
                        foreach (var col in relTailID)
                        {
                            if (!relColumns.Any(x => x.Item1 == tail && x.Item2 == col.ValueColumnID))
                            {
                                relColumns.Add(new Tuple<string, int>(tail, col.ValueColumnID));
                            }
                        }

                    }
                }
            }
            return relColumns;
        }

        //////private void DataItem_RelatedDataChanged(object sender, RelatedDataColumnValueChangedArg e)
        //////{
        //////    var item = (sender as RelationshipFilterSource);
        //////    if (e.Key == "relfilter")
        //////    {
        //////        CheckRelationships(item);
        //////    }
        //////}

        //////private void RemoveRelationshipsFilters(RelationshipFilterSource item)
        //////{
        //تنها آیتمهایی که مطمئن هستیم دیتا شو نمیشوند
        //////ListRelationshipsFilters.Remove(item);
        //////List<Tuple<string, int>> relColumns = GetDependentColumns();
        //////foreach (var column in relColumns)
        //////{
        //////    item.DataItem.RemoveDataObserverForColumn("relfilter", column.Item1, item.DataItem);
        //////    item.UnRegisterEvent();
        //////    item.RelatedDataChanged -= DataItem_RelatedDataChanged;


        //////}
        //item.DataItem.OnObserverColumnChanged(new ObserverData(), null);
        //////}

        //private List<FormulaItemDTO> GetAllFormulaItems(List<FormulaItemDTO> treeFormulaItems, List<FormulaItemDTO> allFormulaItems = null)
        //{
        //    if (allFormulaItems == null)
        //        allFormulaItems = new List<FormulaItemDTO>();
        //    foreach (var item in treeFormulaItems)
        //    {
        //        allFormulaItems.Add(item);
        //        GetAllFormulaItems(item.ChildFormulaItems, allFormulaItems);
        //    }
        //    return allFormulaItems;
        //}
        //////private void CheckRelationships(RelationshipFilterSource relFilter)
        //////{
        //////    foreach (var item in SearchInitialyAreas)
        //////    {
        //////        if (item.Item2.Any())
        //////        {
        //////            item.Item1.SearchViewEntityArea.SearchInitialy();
        //////        }
        //////    }
        //////}




        //private void ApplyState(I_EditEntityArea editArea, DP_DataRepository dataItem, EntityStateDTO state)
        //{

        //}




    }
    //public class RelationshipFilterSource
    //{
    //    public event EventHandler<ObserverDataChangedArg> RelatedDataChanged;
    //    public RelationshipFilterSource()
    //    {
    //        //  EntityStates = new List<EntityStateDTO>();

    //    }

    //    //  public bool IsPresent { set; get; }
    //    public I_EditEntityArea SourceEditArea { set; get; }
    //    public DP_DataRepository DataItem { set; get; }
    //    // public UIActionActivityDTO ActionActivity { set; get; }

    //    public bool InAction { set; get; }
    //    public void RegisterEvent()
    //    {
    //        DataItem.RelatedDataChanged += DataItem_RelatedDataChanged;
    //    }
    //    public void UnRegisterEvent()
    //    {
    //        DataItem.RelatedDataChanged -= DataItem_RelatedDataChanged;
    //    }
    //    private void DataItem_RelatedDataChanged(object sender, ObserverDataChangedArg e)
    //    {
    //        if (RelatedDataChanged != null)
    //            RelatedDataChanged(this, e);
    //    }
    //}
}
