using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyUILibrary.EntityArea.Commands;
using CommonDefinitions.UISettings;
using System.Collections.ObjectModel;

namespace MyUILibrary.EntityArea
{
    public class UIActionActivityManager : I_UIActionActivityManager
    {
        public List<DataAndStates> ListDataAndStates = new List<DataAndStates>();
        I_EditEntityArea EditArea { set; get; }
        public UIActionActivityManager(I_EditEntityArea editArea)
        {
            EditArea = editArea;
            //EditArea.DataItemLoaded += EditArea_DataItemLoaded;
            EditArea.DataItemShown += EditArea_DataItemShown;
            //     EditArea.DataItemUnShown += EditArea_DataItemUnShown;
        }

        private void EditArea_DataItemShown(object sender, EditAreaDataItemLoadedArg e)
        {
            if ((sender as I_EditEntityArea).EntityStates == null || (sender as I_EditEntityArea).EntityStates.Count == 0)
                return;
            DataAndStates item = null;
            if (ListDataAndStates.Any(x => x.DataItem == e.DataItem))
                item = ListDataAndStates.First(x => x.DataItem == e.DataItem);
            else
            {
                item = new EntityArea.DataAndStates(e.DataItem);
                //  item.EntityStates.CollectionChanged += (sender1, e1) => EntityStates_CollectionChanged(sender1, e1, item);
                ListDataAndStates.Add(item);
                foreach (var state in EditArea.EntityStates)
                {
                    CheckDataItemChangeMonitors(e.DataItem, state);
                }
            }

            ResetActionActivities(e.DataItem);
            item.EntityStates.Clear();
            //List<EntityStateDTO> trueStates = new List<EntityStateDTO>();
            foreach (var state in GetAppliableStates(e.DataItem))
            {
                if (CheckEntityState(e.DataItem, state))
                    item.EntityStates.Add(state);
            }
            if (item.EntityStates.Any())
                ImposetStates(item, false);

        }

        //private void EditArea_DataItemLoaded(object sender, EditAreaDataItemLoadedArg e)
        //{



        //    //   item.OnShow = true;
        //    //    if (item.EntityStates.Any())
        //    //  EntityStates_CollectionChanged(null, null, item);
        //    //     item.OnShow = false;
        //}

        private void CheckDataItemChangeMonitors(DP_DataRepository dataItem, EntityStateDTO entityState)
        {
            var generalKey = "stateWatch" + AgentHelper.GetUniqueDataPostfix(dataItem);
            var usageKey = entityState.ID.ToString();

            if (dataItem.ChangeMonitorExists(generalKey, usageKey))
                return;

            List<Tuple<string, int>> columns = new List<Tuple<string, int>>();
            List<Tuple<string, int>> rels = new List<Tuple<string, int>>();
            if (entityState.Formula != null)
            {
                foreach (var fItem in entityState.Formula.FormulaItems)
                {
                    if (fItem.ItemType == FormuaItemType.Column)
                        columns.Add(new Tuple<string, int>(fItem.RelationshipIDTail, fItem.ItemID));
                    else if (!string.IsNullOrEmpty(fItem.RelationshipIDTail))
                        rels.Add(new Tuple<string, int>(fItem.RelationshipIDTail, 0));
                }
            }
            else if (entityState.ColumnID != 0)
            {
                columns.Add(new Tuple<string, int>(entityState.RelationshipTail?.RelationshipIDPath, entityState.ColumnID));
            }
            if (columns.Any() || rels.Any())
            {
                dataItem.RelatedDataTailOrColumnChanged += DataItem_RelatedDataTailOrColumnChanged;
            }
            foreach (var item in columns)
                dataItem.AddChangeMonitor(generalKey, usageKey, item.Item1, item.Item2);
            foreach (var item in rels)
                dataItem.AddChangeMonitor(generalKey, usageKey, item.Item1, 0);

        }




        //private void EditArea_DataItemUnShown(object sender, EditAreaDataItemArg e)
        //{
        //    e.DataItem.RemoveChangeMonitorByGenaralKey("stateWatch" + AgentHelper.GetUniqueDataPostfix(e.DataItem));
        //    foreach (var item in ListDataAndStates.Where(x => x.DataItem == e.DataItem).ToList())
        //        ListDataAndStates.Remove(item);
        //}


        //private void SetDataChangeEventsForDataItem(DP_DataRepository dataItem)
        //{

        //}

        private void DataItem_RelatedDataTailOrColumnChanged(object sender, ChangeMonitor e)
        {
            if (e.GeneralKey.StartsWith("stateWatch"))
            {
                foreach (var entityState in GetAppliableStates(e.DataToCall).Where(x => x.ID.ToString() == e.UsageKey))
                {
                    if (e.UsageKey == entityState.ID.ToString())
                    {
                        bool changed = false;
                        var listItem = ListDataAndStates.FirstOrDefault(x => x.DataItem == e.DataToCall);
                        if (listItem != null)
                        {
                            bool entityStateValue = CheckEntityState(e.DataToCall, entityState);
                            if (entityStateValue)
                            {
                                if (!listItem.EntityStates.Any(x => x.ID == entityState.ID))
                                {
                                    listItem.EntityStates.Add(entityState);
                                    changed = true;
                                }
                            }
                            else
                            {
                                if (listItem.EntityStates.Any(x => x.ID == entityState.ID))
                                {
                                    listItem.EntityStates.Remove(listItem.EntityStates.First(x => x.ID == entityState.ID));
                                    changed = true;
                                }
                            }
                        }
                        if (changed)
                        {
                            ImposetStates(listItem, true);
                        }

                    }
                }
            }
        }
        private void ImposetStates(DataAndStates dataAndState, bool reset)
        {
            if (reset)
                ResetActionActivities(dataAndState.DataItem);
            DoStateActionActivity(dataAndState, dataAndState.EntityStates);
        }
        //private void EntityStates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, DataAndStates dataAndState)
        //{
        //    //var allActionActivities = dataAndState.EntityStates.SelectMany(x => x.ActionActivities);
        //    ResetActionActivities(dataAndState.DataItem);
        //}

        //private List<EntityStateDTO> GetEntityStates(DP_DataRepository dataItem)
        //{
        //    //  ResetActionActivities(dataItem);
        //    List<EntityStateDTO> result = new List<EntityStateDTO>();

        //    return result;
        //}

        private bool CheckEntityState(DP_DataRepository dataItem, EntityStateDTO state)
        {
            bool stateIsValid = false;
            var stateResult = AgentUICoreMediator.GetAgentUICoreMediator.StateManager.CalculateState(state.ID, dataItem, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            if (string.IsNullOrEmpty(stateResult.Message))
            {
                stateIsValid = stateResult.Result;
            }
            return stateIsValid;

        }

        private void ResetActionActivities(DP_DataRepository dataItem)
        {
            foreach (var state in GetAppliableStates(dataItem))
            {
                foreach (var actionActivity in state.ActionActivities)
                {
                    foreach (var detail in actionActivity.UIEnablityDetails)
                    {
                        if (detail.ColumnID != 0)
                        {
                            var simpleColumn = GetColumnControl(EditArea, detail.ColumnID);
                            if (simpleColumn != null)
                            {
                                if (detail.Hidden == true)
                                    EditArea.ChangeSimpleColumnVisiblityFromState(dataItem, simpleColumn, true, "غیر فعال سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, ShouldImposeInUI(state, detail));
                                else if (detail.Readonly == true)
                                    EditArea.ChangeSimpleColumnReadonlyFromState(dataItem, simpleColumn, false, "غیر فعال سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title);
                            }
                        }
                        else if (detail.RelationshipID != 0)
                        {
                            var relationshipControl = GetRelationshipControl(EditArea, detail.RelationshipID);
                            if (relationshipControl != null)
                            {
                                if (detail.Hidden == true)
                                    EditArea.ChangeRelatoinsipColumnVisiblityFromState(dataItem, relationshipControl, true, "فقط خواندنی سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, ShouldImposeInUI(state, detail));
                                else if (detail.Readonly == true)
                                    EditArea.ChangeRelatoinsipColumnReadonlyFromState(dataItem, relationshipControl, false, "فقط خواندنی سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title);
                            }
                        }
                        else if (detail.UICompositionID != 0)
                        {
                            if (detail.Hidden == true)
                            {
                                var uiComposition = GetUIComposition(EditArea, detail.UICompositionID);
                                if (uiComposition != null)
                                    uiComposition.Container.Visiblity(true);
                            }
                        }
                    }
                    if (actionActivity.UIColumnValueRange.Any())
                    {
                        foreach (var columnValueRange in actionActivity.UIColumnValueRange.GroupBy(x => x.ColumnValueRangeID))
                        {
                            if (EditArea.SimpleColumnControls.Any(x => x.Column.ID == columnValueRange.Key))
                            {
                                var simpleColumn = EditArea.SimpleColumnControls.First(x => x.Column.ID == columnValueRange.Key);
                                if (dataItem.ColumnKeyValueRanges.Any(x => x.Key == simpleColumn.Column.ID))
                                    dataItem.ColumnKeyValueRanges.Remove(simpleColumn.Column.ID);
                                EditArea.ResetColumnValueRangeFromState(simpleColumn, dataItem, state);
                            }
                        }
                    }
                }
            }
            //////item.EntityStates.Clear();
        }

        private List<EntityStateDTO> GetAppliableStates(DP_DataRepository dataItem)
        {
            List<EntityStateDTO> result = new List<EntityStateDTO>();
            foreach (var state in EditArea.EntityStates.Where(x => x.ActionActivities.Any()))
            {
                if (state.ActionActivities.Any(x => x.UIEnablityDetails.Any(y => EditArea.AreaInitializer.SourceRelation != null && y.RelationshipID == EditArea.AreaInitializer.SourceRelation.Relationship.PairRelationshipID)))
                {
                    //bool dataIsInValidMode = EditArea.DataItemIsInEditMode(dataItem) || (EditArea is I_EditEntityAreaOneData && EditArea.DataItemIsInTempViewMode(dataItem));
                    bool dataIsInValidMode = EditArea.DataItemIsInEditMode(dataItem) ||  EditArea.DataItemIsInTempViewMode(dataItem);
                    if (dataIsInValidMode)
                        result.Add(state);
                }
                else
                {
                    if (EditArea.DataItemIsInEditMode(dataItem))
                        result.Add(state);
                }
            }
            return result;
        }


        //private void ApplyState(I_EditEntityArea editArea, DP_DataRepository dataItem, EntityStateDTO state)
        //{

        //}





        public void DoStateActionActivity(DataAndStates dataAndState, ObservableCollection<EntityStateDTO> entityStates)
        {
            var dataItem = dataAndState.DataItem;
            foreach (var state in entityStates)
            {
                foreach (var item in state.ActionActivities)
                {
                    if (EditArea.RunningActionActivities.Any(x => x.ID == item.ID))
                        continue;
                    EditArea.RunningActionActivities.Add(item);
                    if (item.UIEnablityDetails.Any())
                    {
                        foreach (var detail in item.UIEnablityDetails)
                        {
                            if (detail.ColumnID != 0)
                            {
                                var simpleColumn = GetColumnControl(EditArea, detail.ColumnID);
                                if (simpleColumn != null)
                                {
                                    if (detail.Hidden == true)
                                        EditArea.ChangeSimpleColumnVisiblityFromState(dataItem, simpleColumn, false, "غیر فعال سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, ShouldImposeInUI(state, detail));
                                    else if (detail.Readonly == true)
                                        EditArea.ChangeSimpleColumnReadonlyFromState(dataItem, simpleColumn, true, "غیر فعال سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title);
                                }
                            }
                            else if (detail.RelationshipID != 0)
                            {
                                var relationshipControl = GetRelationshipControl(EditArea, detail.RelationshipID);
                                if (relationshipControl != null)
                                {
                                    if (detail.Hidden == true)
                                        EditArea.ChangeRelatoinsipColumnVisiblityFromState(dataItem, relationshipControl, false, "فقط خواندنی سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title , ShouldImposeInUI(state, detail));
                                    else if (detail.Readonly == true)
                                        EditArea.ChangeRelatoinsipColumnReadonlyFromState(dataItem, relationshipControl, true, "فقط خواندنی سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title);
                                }
                            }
                            else if (detail.UICompositionID != 0)
                            {
                                if (detail.Hidden == true)
                                {
                                    var uiComposition = GetUIComposition(EditArea, detail.UICompositionID);
                                    if (uiComposition != null)
                                        uiComposition.Container.Visiblity(false);
                                }
                            }
                        }

                    }
                    if (item.UIColumnValueRange.Any())
                    {
                        foreach (var columnValueRange in item.UIColumnValueRange.GroupBy(x => x.ColumnValueRangeID))
                        {
                            if (EditArea.SimpleColumnControls.Any(x => x.Column.ID == columnValueRange.Key))
                            {
                                var simpleColumn = EditArea.SimpleColumnControls.First(x => x.Column.ID == columnValueRange.Key);
                                List<ColumnValueRangeDetailsDTO> candidates = GetFilteredRange(simpleColumn, columnValueRange);
                                if (dataItem.ColumnKeyValueRanges.Any(x => x.Key == simpleColumn.Column.ID))
                                    dataItem.ColumnKeyValueRanges[simpleColumn.Column.ID] = candidates;
                                else
                                    dataItem.ColumnKeyValueRanges.Add(simpleColumn.Column.ID, candidates);
                                EditArea.SetColumnValueRangeFromState(simpleColumn, candidates, dataItem, state);
                            }
                        }
                    }
                    //if (item.UIColumnValueRangeReset.Any())
                    //{
                    //    foreach (var columnValueRange in item.UIColumnValueRangeReset.GroupBy(x => x.ColumnValueRangeID))
                    //    {
                    //        if (EditArea.SimpleColumnControls.Any(x => x.Column.ID == columnValueRange.Key))
                    //        {
                    //            var simpleColumn = EditArea.SimpleColumnControls.First(x => x.Column.ID == columnValueRange.Key);
                    //            if (dataItem.ColumnKeyValueRanges.Any(x => x.Key == simpleColumn.Column.ID))
                    //                dataItem.ColumnKeyValueRanges.Remove(simpleColumn.Column.ID);
                    //            EditArea.ResetColumnValueRange(simpleColumn, dataItem);
                    //        }
                    //    }
                    //}
                    if (item.UIColumnValue.Any())
                    {
                        //در واقع مقادیر پیش فرض را ست میکند
                        EditArea.SetColumnValueFromState(dataItem, item.UIColumnValue, state);
                    }
                    EditArea.RunningActionActivities.Remove(item);
                }
            }
        }

        private bool ShouldImposeInUI(EntityStateDTO state, UIEnablityDetailsDTO detail)
        {
            if (state.FormulaID != 0)
                return false;
            else if (state.RelationshipTailID != 0)
            {
                if (detail.ColumnID != 0)
                    return true;
                else if (detail.RelationshipID != 0)
                {
                    if (state.RelationshipTail.RelationshipIDPath == detail.RelationshipID.ToString() || state.RelationshipTail.RelationshipIDPath.StartsWith(detail.RelationshipID.ToString() + ","))
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                    //فعلا
                }
            }
            else
            {
                if (state.ColumnID == detail.ColumnID)
                    return false;
                else
                    return true;
            }
        }

        private List<ColumnValueRangeDetailsDTO> GetFilteredRange(SimpleColumnControl simpleColumn, IGrouping<int, UIColumnValueRangeDTO> columnValueRange)
        {
            var result = new List<ColumnValueRangeDetailsDTO>();
            foreach (var columnValueItem in columnValueRange)
            {
                foreach (var detail in simpleColumn.Column.ColumnValueRange.Details)
                {
                    var value = "";
                    if (columnValueItem.EnumTag == EnumColumnValueRangeTag.Value)
                        value = detail.Value;
                    else if (columnValueItem.EnumTag == EnumColumnValueRangeTag.Title)
                        value = detail.KeyTitle;
                    else if (columnValueItem.EnumTag == EnumColumnValueRangeTag.Tag1)
                        value = detail.Tag1;
                    else if (columnValueItem.EnumTag == EnumColumnValueRangeTag.Tag2)
                        value = detail.Tag2;
                    if (columnValueItem.Value == value)
                    {
                        result.Add(detail);
                    }
                }
            }
            return result;
        }

        private static EditEntityAreaByRelationshipTail GetEditEntityAreaByRelationshipTail(I_EditEntityArea editEntityArea, EntityRelationshipTailDTO entityRelationshipTail)
        {
            //اگر مالتی پل بود و ...
            RelationshipColumnControl relatedEntityArea = null;
            if (editEntityArea is I_EditEntityAreaOneData)
                relatedEntityArea = (editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == entityRelationshipTail.Relationship.ID);
            else if (editEntityArea is I_EditEntityAreaMultipleData)
                relatedEntityArea = (editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == entityRelationshipTail.Relationship.ID);

            if (relatedEntityArea != null)
            {
                if (entityRelationshipTail.ChildTail == null)
                {
                    EditEntityAreaByRelationshipTail result = new MyUILibrary.EditEntityAreaByRelationshipTail();
                    result.EditEntityAreaFound = true;
                    result.FoundEditEntityArea = relatedEntityArea.EditNdTypeArea;
                    return result;
                }
                else
                {
                    return GetEditEntityAreaByRelationshipTail(relatedEntityArea.EditNdTypeArea, entityRelationshipTail.ChildTail);
                }
            }
            else
            {
                EditEntityAreaByRelationshipTail result = new MyUILibrary.EditEntityAreaByRelationshipTail();
                result.EditEntityAreaFound = false;
                result.LastFoundEntityArea = new Tuple<I_EditEntityArea, EntityRelationshipTailDTO>(editEntityArea, entityRelationshipTail);
                return result;
            }

        }
        private UIControlPackageTree GetUIComposition(I_EditEntityArea editEntityArea, int uiCompositionID)
        {
            if (editEntityArea is I_EditEntityAreaOneData)
                return (editEntityArea as I_EditEntityAreaOneData).UICompositionContainers.FirstOrDefault(x => x.UIComposition.ID == uiCompositionID);
            return null;
        }
        private RelationshipColumnControl GetRelationshipControl(I_EditEntityArea editEntityArea, int relationshipID)
        {
            RelationshipColumnControl control = null;

            if (editEntityArea is I_EditEntityAreaOneData)
            {
                if ((editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.Any(x => x.Relationship.ID == relationshipID))
                {
                    control = (editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.First(x => x.Relationship.ID == relationshipID);

                }
                else if (editEntityArea.AreaInitializer.SourceRelation != null && editEntityArea.AreaInitializer.SourceRelation.Relationship.PairRelationshipID == relationshipID)
                {
                    control = editEntityArea.AreaInitializer.SourceRelation.SourceRelationshipColumnControl;
                }
            }
            else if (editEntityArea is I_EditEntityAreaMultipleData)
            {
                if ((editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.Any(x => x.Relationship.ID == relationshipID))
                {
                    control = (editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.First(x => x.Relationship.ID == relationshipID);
                }
                else if (editEntityArea.AreaInitializer.SourceRelation != null && editEntityArea.AreaInitializer.SourceRelation.Relationship.PairRelationshipID == relationshipID)
                {
                    control = editEntityArea.AreaInitializer.SourceRelation.SourceRelationshipColumnControl;
                }
            }
            return control;
        }



        private SimpleColumnControl GetColumnControl(I_EditEntityArea editEntityArea, int columnID)
        {
            if (editEntityArea is I_EditEntityAreaOneData)
                return (editEntityArea as I_EditEntityAreaOneData).SimpleColumnControls.FirstOrDefault(x => x.Column.ID == columnID);
            else if (editEntityArea is I_EditEntityAreaMultipleData)
                return (editEntityArea as I_EditEntityAreaMultipleData).SimpleColumnControls.FirstOrDefault(x => x.Column.ID == columnID);
            return null;
        }
    }

    public class DataAndStates
    {
        //////   public event EventHandler<ObserverDataChangedArg> RelatedDataChanged;

        public DataAndStates(DP_DataRepository dataItem)
        {
            DataItem = dataItem;
            EntityStates = new ObservableCollection<EntityStateDTO>();
        }


        //  public bool IsPresent { set; get; }
        //   public I_EditEntityArea SourceEditArea { set; get; }
        public DP_DataRepository DataItem { set; get; }
        // public UIActionActivityDTO ActionActivity { set; get; }
        public ObservableCollection<EntityStateDTO> EntityStates { set; get; }
        public bool OnShow { get; internal set; }
        //public bool InAction { set; get; }
        //public void RegisterEvent()
        //{
        //    ///////      DataItem.RelatedDataChanged += DataItem_RelatedDataChanged;
        //}
        //public void UnRegisterEvent()
        //{
        //    //////     DataItem.RelatedDataChanged -= DataItem_RelatedDataChanged;
        //}
        //////private void DataItem_RelatedDataChanged(object sender, ObserverDataChangedArg e)
        //////{
        //////    if (RelatedDataChanged != null)
        //////        RelatedDataChanged(this, e);
        //////}
    }
}
