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
            EditArea.UIGenerated += EditArea_UIGenerated;
            //     EditArea.DataItemUnShown += EditArea_DataItemUnShown;

        }

        private void EditArea_UIGenerated(object sender, EventArgs e)
        {
            CheckISADeterminerStates();
            CheckUnionDeterminerStates();
        }

        private void CheckUnionDeterminerStates()
        {
            if (EditArea.RelationshipColumnControls.Any(x => x.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion))
            {
                foreach (var item in EditArea.RelationshipColumnControls.Where(x => x.Relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion))
                {
                    var superToSubRel = item.Relationship as UnionToSubUnionRelationshipDTO;
                    if (superToSubRel.DeterminerColumnID != 0 && !string.IsNullOrEmpty(superToSubRel.DeterminerColumnValue))
                    {
                        var state = new EntityStateDTO();
                        state.ID = -1 * superToSubRel.ID;
                        state.ColumnID = superToSubRel.DeterminerColumnID;
                        state.Values.Add(new EntityStateValueDTO() { Value = superToSubRel.DeterminerColumnValue });
                        state.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                        var actionActivity = new UIActionActivityDTO();
                        actionActivity.ID = -1 * superToSubRel.ID;
                        actionActivity.Type = Enum_ActionActivityType.UIEnablity;
                        actionActivity.UIEnablityDetails.Add(new UIEnablityDetailsDTO() { Hidden = true, RelationshipID = superToSubRel.ID });
                        state.ActionActivities.Add(actionActivity);
                        EditArea.EntityStates.Add(state);
                    }
                }
            }
            if (EditArea.AreaInitializer.SourceRelation != null && EditArea.AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion)
            {

                var subToSuperRel = EditArea.AreaInitializer.SourceRelation.Relationship as SubUnionToSuperUnionRelationshipDTO;
                if (subToSuperRel.DeterminerColumnID != 0 && !string.IsNullOrEmpty(subToSuperRel.DeterminerColumnValue))
                {
                    var state = new EntityStateDTO();
                    state.ID = -1 * subToSuperRel.ID;
                    state.ColumnID = subToSuperRel.DeterminerColumnID;
                    state.Values.Add(new EntityStateValueDTO() { Value = subToSuperRel.DeterminerColumnValue });
                    state.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                    var actionActivity = new UIActionActivityDTO();
                    actionActivity.ID = -1 * subToSuperRel.ID;
                    actionActivity.Type = Enum_ActionActivityType.UIEnablity;
                    actionActivity.UIEnablityDetails.Add(new UIEnablityDetailsDTO() { Hidden = true, RelationshipID = subToSuperRel.PairRelationshipID });
                    state.ActionActivities.Add(actionActivity);
                    EditArea.EntityStates.Add(state);


                    //شرط اگر داده جدید باشد
                    var setDeterminerState = new EntityStateDTO();
                    setDeterminerState.ID = -2 * subToSuperRel.ID;
                    setDeterminerState.ColumnID = subToSuperRel.DeterminerColumnID;
                    setDeterminerState.Values.Add(new EntityStateValueDTO() { Value = "!@#$#" });
                    setDeterminerState.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                    var setDeterminerActionActivity = new UIActionActivityDTO();
                    setDeterminerActionActivity.ID = -2 * subToSuperRel.ID;
                    setDeterminerActionActivity.Type = Enum_ActionActivityType.ColumnValue;
                    setDeterminerActionActivity.UIColumnValue.Add(new UIColumnValueDTO() { ColumnID = subToSuperRel.DeterminerColumnID, ExactValue = subToSuperRel.DeterminerColumnValue, EvenHasValue = true });
                    setDeterminerState.ActionActivities.Add(setDeterminerActionActivity);
                    EditArea.EntityStates.Add(setDeterminerState);

                }

            }
        }

        private void CheckISADeterminerStates()
        {
            if (EditArea.RelationshipColumnControls.Any(x => x.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub))
            {
                foreach (var item in EditArea.RelationshipColumnControls.Where(x => x.Relationship.TypeEnum == Enum_RelationshipType.SuperToSub))
                {
                    var superToSubRel = item.Relationship as SuperToSubRelationshipDTO;
                    if (superToSubRel.DeterminerColumnID != 0 && !string.IsNullOrEmpty(superToSubRel.DeterminerColumnValue))
                    {
                        var state = new EntityStateDTO();
                        state.ID = -1 * superToSubRel.ID;
                        state.ColumnID = superToSubRel.DeterminerColumnID;
                        state.Values.Add(new EntityStateValueDTO() { Value = superToSubRel.DeterminerColumnValue });
                        state.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                        var actionActivity = new UIActionActivityDTO();
                        actionActivity.ID = -1 * superToSubRel.ID;
                        actionActivity.Type = Enum_ActionActivityType.UIEnablity;
                        actionActivity.UIEnablityDetails.Add(new UIEnablityDetailsDTO() { Hidden = true, RelationshipID = superToSubRel.ID });
                        state.ActionActivities.Add(actionActivity);
                        EditArea.EntityStates.Add(state);
                    }
                }
            }
            if (EditArea.AreaInitializer.SourceRelation != null && EditArea.AreaInitializer.SourceRelation.Relationship.TypeEnum == Enum_RelationshipType.SubToSuper)
            {

                var subToSuperRel = EditArea.AreaInitializer.SourceRelation.Relationship as SubToSuperRelationshipDTO;
                if (subToSuperRel.DeterminerColumnID != 0 && !string.IsNullOrEmpty(subToSuperRel.DeterminerColumnValue))
                {
                    var state = new EntityStateDTO();
                    state.ID = -1 * subToSuperRel.ID;
                    state.ColumnID = subToSuperRel.DeterminerColumnID;
                    state.Values.Add(new EntityStateValueDTO() { Value = subToSuperRel.DeterminerColumnValue });
                    state.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                    var actionActivity = new UIActionActivityDTO();
                    actionActivity.ID = -1 * subToSuperRel.ID;
                    actionActivity.Type = Enum_ActionActivityType.UIEnablity;
                    actionActivity.UIEnablityDetails.Add(new UIEnablityDetailsDTO() { Hidden = true, RelationshipID = subToSuperRel.PairRelationshipID });
                    state.ActionActivities.Add(actionActivity);
                    EditArea.EntityStates.Add(state);


                    //شرط اگر داده جدید باشد
                    var setDeterminerState = new EntityStateDTO();
                    setDeterminerState.ID = -2 * subToSuperRel.ID;
                    setDeterminerState.ColumnID = subToSuperRel.DeterminerColumnID;
                    setDeterminerState.Values.Add(new EntityStateValueDTO() { Value = "!@#$#" });
                    setDeterminerState.EntityStateOperator = Enum_EntityStateOperator.NotEquals;
                    var setDeterminerActionActivity = new UIActionActivityDTO();
                    setDeterminerActionActivity.ID = -2 * subToSuperRel.ID;
                    setDeterminerActionActivity.Type = Enum_ActionActivityType.ColumnValue;
                    setDeterminerActionActivity.UIColumnValue.Add(new UIColumnValueDTO() { ColumnID = subToSuperRel.DeterminerColumnID, ExactValue = subToSuperRel.DeterminerColumnValue, EvenHasValue = true });
                    setDeterminerState.ActionActivities.Add(setDeterminerActionActivity);
                    EditArea.EntityStates.Add(setDeterminerState);

                }

            }

        }

        bool statesSet = false;
        private void SetEntityStates()
        {
            if (statesSet)
                return;
            statesSet = true;
            if (EditArea.EntityStates.Any(x => x.ActionActivities.Any(y => y.Type == Enum_ActionActivityType.EntityReadonly)))
            {
                foreach (var item in EditArea.EntityStates.Where(x => x.ActionActivities.Any(y => y.Type == Enum_ActionActivityType.EntityReadonly)))
                {
                    var newActionActivity = new UIActionActivityDTO();
                    newActionActivity.Type = Enum_ActionActivityType.UIEnablity;
                    item.ActionActivities.Add(newActionActivity);
                    foreach (var column in EditArea.DataEntryEntity.Columns)
                    {
                        if (!column.PrimaryKey)
                        {
                            if (!EditArea.DataEntryEntity.Relationships.Any(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.SecondSideColumnID == column.ID)))
                            {
                                if (item.RelationshipTailID == 0 && item.ColumnID != 0)
                                {
                                    if (item.ColumnID == column.ID)
                                        continue;
                                }
                                var detail = new UIEnablityDetailsDTO();
                                detail.ColumnID = column.ID;
                                detail.Readonly = true;
                                detail.ID = column.ID;
                                newActionActivity.UIEnablityDetails.Add(detail);
                            }
                        }
                    }
                    foreach (var relationship in EditArea.DataEntryEntity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary))
                    {
                        if (item.RelationshipTail != null)
                        {
                            //همون قضیه که خود رابطه ک=ه با وضعیت مشترکه نمیتونه اعمال بشه
                            //if (item.RelationshipTail.Relationship.ID == relationship.ID)
                            //    continue;
                        }
                        var detail = new UIEnablityDetailsDTO();
                        detail.RelationshipID = relationship.ID;
                        detail.Readonly = true;
                        detail.ID = relationship.ID;
                        newActionActivity.UIEnablityDetails.Add(detail);
                    }
                }
            }
        }

        private void EditArea_DataItemShown(object sender, EditAreaDataItemLoadedArg e)
        {
            //////if (EditArea.AreaInitializer.SourceRelation != null && e.InEditMode)
            //////    CheckParentReadonlyTail(e.DataItem);
            //حتمکا اینجا چک شود مخصوصا برای تمپ ویو ها که تک داده ای نیز باشند
            if (EditArea.EntityStates == null || EditArea.EntityStates.Count == 0)
                return;
            SetEntityStates();
            foreach (var state in EditArea.EntityStates)
            {
                CheckDataItemChangeMonitors(e.DataItem, state);
            }
            CheckAndImposeEntityStates(e.DataItem, false, ActionActivitySource.OnShowData);

        }

        //private void CheckParentReadonlyTail(DP_DataRepository data)
        //{
        //    var parentReadonlyTail = GetParentReadonlyTails(data.ParantChildRelationshipInfo);

        //    foreach (var item in EditArea.RelationshipColumnControls)
        //    {
        //        var childRel = data.ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == item.Relationship.ID);
        //        if (childRel != null)
        //        {
        //            if (parentReadonlyTail.Any(x => x.Item2.Contains(x.Item1 + "," + item.Relationship.ID)))
        //            {
        //                childRel.IsReadonlyBecauseOfParentTail = true;
        //                EditArea.MakeRelatoinsipColumnReadonlyFromState(childRel, data, item, "فقط خواندنی به واسطه رشته رابطه");

        //            }
        //            else
        //            {
        //                EditArea.MakeRelatoinsipColumnUnReadonlyFromState(childRel, data, item, "فقط خواندنی به واسطه رشته رابطه");

        //            }

        //        }
        //    }
        //}

        //private List<Tuple<string, string>> GetParentReadonlyTails(ChildRelationshipInfo parantChildRelationshipInfo, string tail = "", List<Tuple<string, string>> result = null)
        //{
        //    if (result == null)
        //        result = new List<Tuple<string, string>>();
        //    if (tail == "")
        //        tail = parantChildRelationshipInfo.Relationship.ID.ToString();
        //    else
        //        tail = parantChildRelationshipInfo.Relationship.ID + "," + tail;

        //    if (parantChildRelationshipInfo.ReadonlyStateFromTails.Any(x => x.Contains(tail)))
        //    {
        //        foreach (var item in parantChildRelationshipInfo.ReadonlyStateFromTails.Where(x => x.Contains(tail)))
        //        {
        //            result.Add(new Tuple<string, string>(tail, item));
        //        }
        //    }
        //    else
        //    {
        //        if (parantChildRelationshipInfo.SourceData.ParantChildRelationshipInfo == null)
        //            return result;
        //        else
        //            GetParentReadonlyTails(parantChildRelationshipInfo.SourceData.ParantChildRelationshipInfo, tail, result);
        //    }
        //    return result;
        //}

        public void CheckAndImposeEntityStates(DP_DataRepository dataItem, bool skipUICheck, ActionActivitySource actionActivitySource)
        {
            if (EditArea.EntityStates == null || EditArea.EntityStates.Count == 0)
                return;
            DataAndStates item = null;
            if (ListDataAndStates.Any(x => x.DataItem == dataItem))
                item = ListDataAndStates.First(x => x.DataItem == dataItem);
            else
            {
                item = new EntityArea.DataAndStates(dataItem);
                //  item.EntityStates.CollectionChanged += (sender1, e1) => EntityStates_CollectionChanged(sender1, e1, item);
                ListDataAndStates.Add(item);
            }

            ResetActionActivities(dataItem, skipUICheck);
            item.EntityStates.Clear();
            //List<EntityStateDTO> trueStates = new List<EntityStateDTO>();
            foreach (var state in GetAppliableStates(dataItem, skipUICheck))
            {
                if (CheckEntityState(dataItem, state))
                    item.EntityStates.Add(state);
            }
            if (item.EntityStates.Any())
                DoStateActionActivity(item, item.EntityStates, skipUICheck, actionActivitySource);
        }
        //private void EditArea_DataItemLoaded(object sender, EditAreaDataItemLoadedArg e)
        //{
        private List<EntityStateDTO> GetAppliableStates(DP_DataRepository dataItem, bool skipUICheck)
        {
            List<EntityStateDTO> result = new List<EntityStateDTO>();
            foreach (var state in EditArea.EntityStates.Where(x => x.ActionActivities.Any()))
            {
                if (skipUICheck)
                    result.Add(state);
                else
                {
                    if (state.ActionActivities.Any(x => x.UIEnablityDetails.Any(y => EditArea.AreaInitializer.SourceRelation != null && y.RelationshipID == EditArea.AreaInitializer.SourceRelation.Relationship.PairRelationshipID)))
                    {
                        //bool dataIsInValidMode = EditArea.DataItemIsInEditMode(dataItem) || (EditArea is I_EditEntityAreaOneData && EditArea.DataItemIsInTempViewMode(dataItem));
                        bool dataIsInValidMode = EditArea.DataItemIsInEditMode(dataItem) || EditArea.DataItemIsInTempViewMode(dataItem);
                        if (dataIsInValidMode)
                            result.Add(state);
                    }
                    else
                    {
                        if (EditArea.DataItemIsInEditMode(dataItem))
                            result.Add(state);
                    }
                }
            }
            return result;
        }


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
                foreach (var entityState in GetAppliableStates(e.DataToCall, false).Where(x => x.ID.ToString() == e.UsageKey))
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
                            ResetActionActivities(listItem.DataItem, false);
                            DoStateActionActivity(listItem, listItem.EntityStates, false, ActionActivitySource.TailOrPropertyChange);
                        }

                    }
                }
            }
        }
        //private void ImposetStates(DataAndStates dataAndState, bool reset)
        //{
        //    if (reset)
        //        ResetActionActivities(dataAndState.DataItem);
        //    DoStateActionActivity(dataAndState, dataAndState.EntityStates);
        //}
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
            var stateResult = AgentUICoreMediator.GetAgentUICoreMediator.StateManager.CalculateState(state, dataItem, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            if (string.IsNullOrEmpty(stateResult.Message))
            {
                stateIsValid = stateResult.Result;
            }
            return stateIsValid;

        }

        public void ResetActionActivities(DP_DataRepository dataItem, bool skipUICheck)
        {
            List<BaseColumnControl> hiddenControls = new List<BaseColumnControl>();

            foreach (var state in GetAppliableStates(dataItem, skipUICheck))
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
                                {
                                    EditArea.ChangeSimpleColumnVisiblityFromState(dataItem, simpleColumn, false, "غیر فعال سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString());//, ImposeControlState.Both);
                                    hiddenControls.Add(simpleColumn);
                                }
                                else if (detail.Readonly == true)
                                    EditArea.ChangeSimpleColumnReadonlyFromState(dataItem, simpleColumn, false, "فقط خواندنی سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString());//, ImposeControlState.Both);
                            }
                        }
                        else if (detail.RelationshipID != 0)
                        {
                            var relationshipControl = GetRelationshipControl(EditArea, detail.RelationshipID);
                            if (relationshipControl != null)
                            {
                                if (detail.Hidden == true)
                                {
                                    if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
                                    {
                                        var childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                                        EditArea.ChangeRelatoinsipColumnVisiblityFromState(childRelationshipInfo, dataItem, relationshipControl, false, "غیر فعال سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), ImposeControlState.Both);
                                        hiddenControls.Add(relationshipControl);

                                        //حتما داده باید در یو آی حاضر باشد
                                    }
                                    else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
                                    {
                                        var childRelationshipInfo = dataItem.ParantChildRelationshipInfo;
                                        EditArea.ChangeClearDataItemVisiblityFromState(dataItem, detail.ID.ToString(), skipUICheck);
                                    }

                                }
                                else if (detail.Readonly == true)
                                {
                                    if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
                                    {
                                        var childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                                        EditArea.ChangeRelatoinsipColumnReadonlyFromState(childRelationshipInfo, dataItem, relationshipControl, false, "فقط خواندنی سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), ImposeControlState.Both);
                                        //حتما داده باید در یو آی حاضر باشد
                                    }
                                    else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
                                    {
                                        //  var childRelationshipInfo = dataItem.ParantChildRelationshipInfo;
                                        EditArea.ChangeClearDataItemReadonlyFromState(dataItem, detail.ID.ToString(), skipUICheck);
                                    }
                                }
                            }
                        }
                        //else if (detail.UICompositionID != 0)
                        //{
                        //    if (detail.Hidden == true)
                        //    {
                        //        var uiComposition = GetUIComposition(EditArea, detail.UICompositionID);
                        //        if (uiComposition != null)
                        //            uiComposition.Container.Visiblity(true);
                        //    }
                        //}
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
            if (EditArea is I_EditEntityAreaOneData && hiddenControls.Any())
            {
                (EditArea as I_EditEntityAreaOneData).CheckContainersVisiblity(hiddenControls);
            }
            //////item.EntityStates.Clear();
        }




        //private void ApplyState(I_EditEntityArea editArea, DP_DataRepository dataItem, EntityStateDTO state)
        //{

        //}





        public void DoStateActionActivity(DataAndStates dataAndState, ObservableCollection<EntityStateDTO> entityStates, bool skipUICheck, ActionActivitySource actionActivitySource)
        {
            var dataItem = dataAndState.DataItem;
            List<BaseColumnControl> hiddenControls = new List<BaseColumnControl>();
            foreach (var state in entityStates.ToList())
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

                            if (detail.RelationshipID != 0)
                            {
                                var relationshipControl = GetRelationshipControl(EditArea, detail.RelationshipID);
                                if (relationshipControl != null)
                                {
                                    if (detail.Hidden == true)
                                    {
                                        if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
                                        {
                                            var childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                                            EditArea.ChangeRelatoinsipColumnVisiblityFromState(childRelationshipInfo, dataItem, relationshipControl, true, "غیر فعال سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), GetRelationshipHiddenControlState(childRelationshipInfo, state, detail.RelationshipID, actionActivitySource));
                                            hiddenControls.Add(relationshipControl);
                                            //حتما داده باید در یو آی حاضر باشد
                                        }
                                        else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
                                        {
                                            if (actionActivitySource == ActionActivitySource.OnShowData)
                                            {
                                                if (state.RelationshipTail != null &&
                                                    (state.RelationshipTail.RelationshipIDPath == dataItem.ParantChildRelationshipInfo.Relationship.PairRelationshipID.ToString() ||
                                                    state.RelationshipTail.RelationshipIDPath.StartsWith(dataItem.ParantChildRelationshipInfo.Relationship.PairRelationshipID.ToString() + ",")))
                                                {

                                                }
                                                else
                                                    dataItem.IsReadonlyBecauseOfCreatorRelationshipOnShow = true;
                                            }
                                            EditArea.ChangeDataItemVisiblityFromState(dataItem, "غیر فعال سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), skipUICheck);
                                        }
                                    }
                                    else if (detail.Readonly == true)
                                    {
                                        if (dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipControl.Relationship.ID))
                                        {
                                            var childRelationshipInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipControl.Relationship.ID);
                                            //چون در این حالت باید کل تیل ریدونلی شود. چون اگه تیل تغییر کنه داده هدف دیگر موجود نیست که ذخیره شود. بنابراین رابطه موجود در اکشن اکتیویتی میتواند تغییر کند در حالیکه استیت همون قبلی باشد
                                            //////if (actionActivitySource == ActionActivitySource.OnShowData)
                                            //////{
                                            //////    childRelationshipInfo.IsReadonlyBecauseOfParentTail = true;
                                            //////    if (state.RelationshipTailID != 0 && state.RelationshipTail.ChildTail != null)
                                            //////        childRelationshipInfo.ReadonlyStateFromTails.Add(state.RelationshipTail.RelationshipIDPath);
                                            //////}

                                            EditArea.ChangeRelatoinsipColumnReadonlyFromState(childRelationshipInfo, dataItem, relationshipControl, true, "فقط خواندنی سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), GetRelationshipReadonlyControlState(childRelationshipInfo, state, detail.RelationshipID, actionActivitySource));
                                            //حتما داده باید در یو آی حاضر باشد
                                        }
                                        else if (dataItem.ParantChildRelationshipInfo != null && dataItem.ParantChildRelationshipInfo.Relationship.ID == relationshipControl.Relationship.ID)
                                        {
                                            // var childRelationshipInfo = dataItem.ParantChildRelationshipInfo;
                                            if (actionActivitySource == ActionActivitySource.OnShowData)
                                            {
                                                if (state.RelationshipTail != null &&
                                                      (state.RelationshipTail.RelationshipIDPath == dataItem.ParantChildRelationshipInfo.Relationship.PairRelationshipID.ToString() ||
                                                      state.RelationshipTail.RelationshipIDPath.StartsWith(dataItem.ParantChildRelationshipInfo.Relationship.PairRelationshipID.ToString() + ",")))
                                                {

                                                }
                                                else
                                                    dataItem.IsReadonlyBecauseOfCreatorRelationshipOnShow = true;
                                            }
                                            EditArea.ChangeDataItemReadonlyFromState(dataItem, "فقط خواندنی سازی رابطه" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString(), skipUICheck);
                                        }
                                    }
                                }
                            }
                            else if (detail.ColumnID != 0)
                            {
                                var simpleColumn = GetColumnControl(EditArea, detail.ColumnID);
                                if (simpleColumn != null)
                                {
                                    if (detail.Hidden == true)
                                    {
                                        EditArea.ChangeSimpleColumnVisiblityFromState(dataItem, simpleColumn, true, "غیر فعال سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString());//, ImposeControlState.Impose);// GetColumnHiddenControlState(state, dataItem, detail.ColumnID, actionActivitySource));
                                        hiddenControls.Add(simpleColumn);
                                    }
                                    else if (detail.Readonly == true)
                                        EditArea.ChangeSimpleColumnReadonlyFromState(dataItem, simpleColumn, true, "فقط خواندنی سازی ستون" + " " + "بر اساس وضعیت" + " " + state.Title, detail.ID.ToString());//, ImposeControlState.Impose);// GetColumnReadonlyControlState(state, dataItem, detail.ColumnID, actionActivitySource));
                                }
                            }
                            //else if (detail.UICompositionID != 0)
                            //{
                            //    if (detail.Hidden == true)
                            //    {
                            //        var uiComposition = GetUIComposition(EditArea, detail.UICompositionID);
                            //        if (uiComposition != null)
                            //            uiComposition.Container.Visiblity(false);
                            //    }
                            //}
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
            if (EditArea is I_EditEntityAreaOneData && hiddenControls.Any())
            {
                (EditArea as I_EditEntityAreaOneData).CheckContainersVisiblity(hiddenControls);
            }
        }





        //private bool AllChildItemsAreDBRel(DP_DataRepository dataItem, EntityRelationshipTailDTO relationshipTail)
        //{
        //    if (relationshipTail == null || !dataItem.ChildRelationshipInfos.Any(x => x.Relationship.ID == relationshipTail.Relationship.ID))
        //        return true;
        //    else
        //    {
        //        var childRelInfo = dataItem.ChildRelationshipInfos.First(x => x.Relationship.ID == relationshipTail.Relationship.ID);
        //        var childData = childRelInfo.RelatedData.First();
        //        if (childData.IsDBRelationship)
        //            return AllChildItemsAreDBRel(childData, relationshipTail.ChildTail);
        //        else
        //            return false;
        //    }
        //}
        private ImposeControlState GetRelationshipReadonlyControlState(ChildRelationshipInfo childRelationshipInfo, EntityStateDTO state, int relationshipID, ActionActivitySource actionActivitySource)
        {
            if (state.RelationshipTailID != 0)
            {
                if (state.RelationshipTail.RelationshipIDPath == relationshipID.ToString() || state.RelationshipTail.RelationshipIDPath.StartsWith(relationshipID.ToString() + ","))
                {
                    if (actionActivitySource == ActionActivitySource.TailOrPropertyChange)
                        return ImposeControlState.AddMessageColor;
                    else if (actionActivitySource == ActionActivitySource.BeforeUpdate)
                    {
                        if (childRelationshipInfo.CheckRelationshipIsChanged())
                        {
                            return ImposeControlState.AddMessageColor;
                        }
                    }
                }
            }
            return ImposeControlState.Impose;
        }
        private ImposeControlState GetRelationshipHiddenControlState(ChildRelationshipInfo childRelationshipInfo, EntityStateDTO state, int relationshipID, ActionActivitySource actionActivitySource)
        {
            if (state.RelationshipTailID != 0)
            {
                if (state.RelationshipTail.RelationshipIDPath == relationshipID.ToString() || state.RelationshipTail.RelationshipIDPath.StartsWith(relationshipID.ToString() + ","))
                {
                    if (actionActivitySource == ActionActivitySource.TailOrPropertyChange)
                        return ImposeControlState.AddMessageColor;
                    else if (actionActivitySource == ActionActivitySource.BeforeUpdate)
                    {
                        if (childRelationshipInfo.CheckRelationshipIsChanged())
                        {
                            return ImposeControlState.AddMessageColor;
                        }
                    }
                }
            }
            return ImposeControlState.Impose;
        }
        private ImposeControlState GetColumnReadonlyControlState(EntityStateDTO state, DP_DataRepository dataItem, int columnID, ActionActivitySource actionActivitySource)
        {
            if (state.RelationshipTailID == 0 && state.ColumnID != 0)
            {
                if (state.ColumnID == columnID)
                {
                    if (actionActivitySource == ActionActivitySource.TailOrPropertyChange)
                    {
                        //چون همونیه که تغییر کرده
                        return ImposeControlState.AddMessageColor;
                    }
                    else if (actionActivitySource == ActionActivitySource.BeforeUpdate)
                    {
                        if (dataItem.PropertyValueIsChanged(dataItem.GetProperty(columnID)))
                            return ImposeControlState.AddMessageColor;
                    }
                }
            }

            return ImposeControlState.Impose;
        }

        private ImposeControlState GetColumnHiddenControlState(EntityStateDTO state, DP_DataRepository dataItem, int columnID, ActionActivitySource actionActivitySource)
        {
            if (state.RelationshipTailID == 0 && state.ColumnID != 0)
            {
                if (state.ColumnID == columnID)
                {
                    if (actionActivitySource == ActionActivitySource.TailOrPropertyChange)
                    {
                        //چون همونیه که تغییر کرده
                        return ImposeControlState.AddMessageColor;
                    }
                    else if (actionActivitySource == ActionActivitySource.BeforeUpdate)
                    {
                        if (dataItem.PropertyValueIsChanged(dataItem.GetProperty(columnID)))
                            return ImposeControlState.AddMessageColor;
                    }
                }
            }
            return ImposeControlState.Impose;
        }
        private bool ShouldImposeRelationshipControlInUI(EntityStateDTO state, int relationshipID)
        {
            if (state.FormulaID != 0)
                return false;
            else if (state.RelationshipTailID != 0)
            {
                if (state.RelationshipTail.RelationshipIDPath == relationshipID.ToString() || state.RelationshipTail.RelationshipIDPath.StartsWith(relationshipID.ToString() + ","))
                    return false;
                else
                    return true;
            }
            else
            {
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
        //private UIControlPackageTree GetUIComposition(I_EditEntityArea editEntityArea, int uiCompositionID)
        //{
        //    if (editEntityArea is I_EditEntityAreaOneData)
        //        return (editEntityArea as I_EditEntityAreaOneData).UICompositionContainers.FirstOrDefault(x => x.UIComposition.ID == uiCompositionID);
        //    return null;
        //}
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
