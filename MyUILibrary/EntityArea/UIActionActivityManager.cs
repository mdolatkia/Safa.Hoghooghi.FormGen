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
    public class UIActionActivityManager : I_UIActionActivityManager
    {
        //public List<UIActionActivitySource> ListUIActionActivities = new List<UIActionActivitySource>();
        I_EditEntityArea EditArea { set; get; }
        public UIActionActivityManager(I_EditEntityArea editArea)
        {
            EditArea = editArea;
            EditArea.DataItemShown += EditArea_DataItemShown;
            EditArea.DataItemUnShown += EditArea_DataItemUnShown;
        }
        private void EditArea_DataItemUnShown(object sender, EditAreaDataItemArg e)
        {
            e.DataItem.RemoveChangeMonitorByGenaralKey("stateWatch" + AgentHelper.GetUniqueDataPostfix(e.DataItem));
        }
        private void EditArea_DataItemShown(object sender, EditAreaDataItemArg e)
        {
            if ((sender as I_EditEntityArea).EntityStates == null || (sender as I_EditEntityArea).EntityStates.Count == 0)
                return;
            SetDataChangeEventsForDataItem(e.DataItem);
            CheckEntityStates(e.DataItem);
        }

        private void SetDataChangeEventsForDataItem(DP_DataRepository dataItem)
        {
            foreach (var entityState in EditArea.EntityStates)
            {
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
                    columns.Add(new Tuple<string, int>(entityState.RelationshipIDTail, entityState.ColumnID));
                }
                if (columns.Any() || rels.Any())
                {
                    dataItem.RelatedDataTailOrColumnChanged += (sender1, e1) => DataItem_RelatedDataTailOrColumnChanged(sender1, e1, entityState);
                }
                foreach (var item in columns)
                    dataItem.AddChangeMonitor("stateWatch" + AgentHelper.GetUniqueDataPostfix(dataItem), entityState.ID.ToString(), item.Item1, item.Item2);
                foreach (var item in rels)
                    dataItem.AddChangeMonitor("stateWatch" + AgentHelper.GetUniqueDataPostfix(dataItem), entityState.ID.ToString(), item.Item1, 0);
            }
        }

        private void DataItem_RelatedDataTailOrColumnChanged(object sender, ChangeMonitor e, EntityStateDTO entityState)
        {
            if (e.GeneralKey.StartsWith("stateWatch"))
            {
                if (e.UsageKey == entityState.ID.ToString())
                {
                    CheckEntityState(e.DataToCall, entityState, true);
                }
            }
        }

        //private List<Tuple<string, int>> GetDependentColumns(I_EditEntityArea editArea)
        //{
        //    List<Tuple<string, int>> relColumns = new List<Tuple<string, int>>();
        //    if (editArea.EntityStates != null)
        //    {
        //        //List<int> columnsIds = new List<int>();

        //        foreach (var state in editArea.EntityStates.Where(x => x.ActionActivities.Any()))
        //        {
        //            if (state.ColumnID != 0)
        //            {
        //                if (!relColumns.Any(x => x.Item1 == "" && x.Item2 == state.ColumnID))
        //                {
        //                    relColumns.Add(new Tuple<string, int>("", state.ColumnID));
        //                }
        //            }
        //            else if (state.FormulaID != 0)
        //            {
        //                //var columns = AgentHelper.GetFormulaColumnsList(this, state.Formula.FormulaItems);

        //                //foreach (var item in state.Formula.TreeFormulaItems.Where(x => x.ItemType == FormuaItemType.Column))
        //                //{
        //                //    if (!columnsIds.Contains(state.ColumnID))
        //                //    {
        //                //        columnsIds.Add(state.ColumnID);
        //                //    }
        //                //}
        //                List<FormulaItemDTO> allFormulaItems = GetAllFormulaItems(state.Formula.FormulaItems);
        //                var listColumnsOnly = allFormulaItems.Where(x => x.ItemType == FormuaItemType.Column).GroupBy(x => x.RelationshipIDTail);
        //                foreach (var item in listColumnsOnly)
        //                {
        //                    foreach (var col in item)
        //                    {
        //                        if (!relColumns.Any(x => x.Item1 == item.Key && x.Item2 == col.ItemID))
        //                            relColumns.Add(new Tuple<string, int>(item.Key, col.ItemID));
        //                    }
        //                }
        //                var listRelatoinshipOnly = allFormulaItems.Where(x => x.ItemType == FormuaItemType.Relationship).GroupBy(x => x.RelationshipIDTail);
        //                foreach (var item in listRelatoinshipOnly)
        //                {
        //                    foreach (var rel in item)
        //                    {
        //                        if (!relColumns.Any(x => x.Item1 == item.Key))
        //                            if (!relColumns.Any(x => x.Item1 == item.Key && x.Item2 == rel.ItemID))
        //                            {
        //                                var tail = string.IsNullOrEmpty(item.Key) ? rel.ItemID.ToString() : item.Key + "," + rel.ItemID;
        //                                relColumns.Add(new Tuple<string, int>(tail, 0));

        //                            }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    return relColumns;
        //}

        //////private void DataItem_RelatedDataChanged(object sender, ObserverDataChangedArg e)
        //////{
        //////    var item = (sender as UIActionActivitySource);
        //////    if (e.Key == "state")
        //////    {
        //////        CheckEntityStates(item);
        //////    }
        //////}

        //private void RemoveDataItemStates(UIActionActivitySource item)
        //{
        //    //تنها آیتمهایی که مطمئن هستیم دیتا شو نمیشوند
        //    ResetActionActivities(item);
        //    ListUIActionActivities.Remove(item);
        //    List<Tuple<string, int>> relColumns = GetDependentColumns(item.SourceEditArea);
        //    foreach (var column in relColumns)
        //    {
        //        //////item.DataItem.RemoveDataObserverForColumn("state", column.Item1, item.DataItem);
        //        item.UnRegisterEvent();
        //        //////item.RelatedDataChanged -= DataItem_RelatedDataChanged;


        //    }
        //    //item.DataItem.OnObserverColumnChanged(new ObserverData(), null);
        //}

        //private List<FormulaItemDTO> GetAllFormulaItems(List<FormulaItemDTO> treeFormulaItems, List<FormulaItemDTO> allFormulaItems = null)
        //{
        //    if (allFormulaItems == null)
        //        allFormulaItems = new List<FormulaItemDTO>();
        //    foreach (var item in treeFormulaItems)
        //    {
        //        allFormulaItems.Add(item);
        //        //GetAllFormulaItems(item.ChildFormulaItems, allFormulaItems);
        //    }
        //    return allFormulaItems;
        //}

        //private void WatchDataItemRelationship(DP_DataRepository dataItem, string key, I_EditEntityArea editArea)
        //{
        //    dataItem.RelatedDataChanged += (sender, e) => DataItem_RelatedDataChanged1(sender, e, dataItem, key, editArea);
        //}

        //private void DataItem_RelatedDataChanged1(object sender, RelatedDataChangedArg e, DP_DataRepository dataItem, string tailKey, I_EditEntityArea editArea)
        //{
        //    CheckEntityStates(dataItem, editArea);
        //}



        //private void Column_PropertyValueChanged(object sender, PropertyValueChangedArg e, DP_DataRepository dataItem, I_EditEntityArea editArea)
        //{
        //    CheckEntityStates(dataItem, editArea);
        //    //else
        //    //    dataItem.StateIds.Remove(state.ID);
        //}

        private void CheckEntityStates(DP_DataRepository dataItem)
        {
            ResetActionActivities(dataItem);

            foreach (var state in EditArea.EntityStates)
            {
                CheckEntityState(dataItem, state, false);
            }
        }

        private void CheckEntityState(DP_DataRepository dataItem, EntityStateDTO state, bool onChange)
        {
            bool stateIsValid = false;
            var stateResult = AgentUICoreMediator.GetAgentUICoreMediator.StateManager.CalculateState(state.ID, dataItem, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            if (string.IsNullOrEmpty(stateResult.Message))
            {
                stateIsValid = stateResult.Result;
            }
            if (stateIsValid)
            {
                if (state.ActionActivities != null && state.ActionActivities.Count != 0)
                {
                    //ممکن است به اراو بخورد زیرا مثلا اگر داده وابسته ای تغییر کند و در این اقدام دوباره همهما وابسته مقداردهی شود پیغام کالکشن چنج میدهد
                    //بهتر است در یک ترد دیگر اجرا شود
                    //item.InAction = true;
                    DoStateActionActivity(EditArea, dataItem, state, onChange);
                    //item.InAction = false;
                }

            }
        }

        private void ResetActionActivities(DP_DataRepository dataItem)
        {
            //////foreach (var state in EditArea.EntityStates)
            //////{
            //////    foreach (var actionActivity in state.ActionActivities)
            //////    {
            //////        foreach (var detail in actionActivity.UIEnablityDetails)
            //////        {
            //////            if (detail.ColumnID != 0)
            //////            {
            //////                var column = GetColumnControl(item.SourceEditArea, detail.ColumnID);
            //////                if (detail.Hidden == true)
            //////                {
            //////                    //if (column is SimpleColumnControl)
            //////                    //{
            //////                    //    (column as SimpleColumnControl).BusinessHidden = false;
            //////                    //    (column as SimpleColumnControl).ControlManager.Visiblity(true);
            //////                    //}
            //////                    if (column is SimpleColumnControl)
            //////                    {
            //////                        (column as SimpleColumnControl).BusinessHidden[item.DataItem] = false;
            //////                        (column as SimpleColumnControl).ControlManager.Visiblity(item.DataItem, true);

            //////                    }
            //////                }

            //////                else if (detail.Readonly == true)
            //////                {
            //////                    //if (column is SimpleColumnControl)
            //////                    //{
            //////                    //    if (!(column as SimpleColumnControl).SecurityReadOnly)
            //////                    //    {
            //////                    //        (column as SimpleColumnControl).BusinessReadonly = false;
            //////                    //        (column as SimpleColumnControl).ControlManager.SetReadonly(false);

            //////                    //    }

            //////                    //}
            //////                    if (column is SimpleColumnControl)
            //////                    {
            //////                        if (!(column as SimpleColumnControl).SecurityReadOnly)
            //////                        {
            //////                            (column as SimpleColumnControl).BusinessReadOnly[item.DataItem] = false;
            //////                            (column as SimpleColumnControl).SimpleControlManager.SetReadonly(item.DataItem, false);
            //////                        }
            //////                    }
            //////                }

            //////            }
            //////            else if (detail.RelationshipID != 0)
            //////            {
            //////                var relationshipControl = GetRelationshipControl(item.SourceEditArea, detail.RelationshipID);
            //////                if (detail.Hidden == true)
            //////                {
            //////                    if (relationshipControl.Item2 == false)
            //////                    {
            //////                        //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
            //////                        //{
            //////                        //    (relationshipControl.Item1 as RelationshipColumnControlOneData).BusinessHidden = false;
            //////                        //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.Visiblity(true);
            //////                        //}
            //////                        if (relationshipControl.Item1 is RelationshipColumnControl)
            //////                        {
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).BusinessHidden[item.DataItem] = false;
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).ControlManager.Visiblity(item.DataItem, true);
            //////                        }
            //////                    }
            //////                    else
            //////                    {
            //////                        //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
            //////                        //{
            //////                        //    item.DataItem.BusinessHidden = false;
            //////                        //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.ClearBusinessMessage();
            //////                        //}
            //////                        if (relationshipControl.Item1 is RelationshipColumnControl)
            //////                        {
            //////                            item.DataItem.BusinessHidden = false;
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).EditNdTypeArea.RemoveDataBusinessMessage(item.DataItem, "stateActionActivity");
            //////                        }
            //////                    }
            //////                }
            //////                else if (detail.Readonly == true)
            //////                {
            //////                    if (relationshipControl.Item2 == false)
            //////                    {
            //////                        //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
            //////                        //{
            //////                        //    relationshipControl.Item1.EditNdTypeArea.AreaInitializer.BusinessReadOnlyByParent = false;
            //////                        //    if (relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
            //////                        //         || relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
            //////                        //    {
            //////                        //        relationshipControl.Item1.EditNdTypeArea.ImposeDataViewSecurity();
            //////                        //    }
            //////                        //    else if (relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
            //////                        //      || relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            //////                        //    {
            //////                        //        relationshipControl.Item1.EditNdTypeArea.ImposeTemporaryViewSecurity();
            //////                        //    }
            //////                        //}

            //////                        if (relationshipControl.Item1 is RelationshipColumnControl)
            //////                        {
            //////                            relationshipControl.Item1.EditNdTypeArea.AreaInitializer.RemoveBusinessReadOnlyByParent(item.DataItem);
            //////                            /////اینجا درست شه به ریدئنلی بودن خود فرم هم دقت شود
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).RelationshipControlManager.EnableDisable(item.DataItem, TemporaryLinkType.SerachView, true);
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).RelationshipControlManager.EnableDisable(item.DataItem, TemporaryLinkType.Clear, true);
            //////                        }
            //////                    }
            //////                    else
            //////                    {
            //////                        //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
            //////                        //{
            //////                        //    item.DataItem.BusinessReadonly = true;
            //////                        //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.ClearBusinessMessage();
            //////                        //}
            //////                        if (relationshipControl.Item1 is RelationshipColumnControl)
            //////                        {
            //////                            item.DataItem.BusinessHidden = true;
            //////                            (relationshipControl.Item1 as RelationshipColumnControl).EditNdTypeArea.RemoveDataBusinessMessage(item.DataItem, "stateActionActivity");
            //////                        }
            //////                    }
            //////                }
            //////            }
            //////            else if (detail.UICompositionID != 0)
            //////            {
            //////                if (detail.Hidden == true)
            //////                {
            //////                    var uiComposition = GetUIComposition(item.SourceEditArea, detail.UICompositionID);
            //////                    uiComposition.Container.Visiblity(true);
            //////                }
            //////            }
            //////        }

            //////    }
            //////}
            //////item.EntityStates.Clear();
        }


        //private void ApplyState(I_EditEntityArea editArea, DP_DataRepository dataItem, EntityStateDTO state)
        //{

        //}


        public void DoStateActionActivity(I_EditEntityArea editEntityArea, DP_DataRepository dataItem, EntityStateDTO state, bool onChange)
        {
            foreach (var item in state.ActionActivities)
            {
                if (editEntityArea.RunningActionActivities.Any(x => x.ID == item.ID))
                    continue;
                editEntityArea.RunningActionActivities.Add(item);
                if (item.UIEnablityDetails.Any())
                {
                    foreach (var detail in item.UIEnablityDetails)
                    {
                        if (detail.ColumnID != 0)
                        {
                            var simpleColumn = GetColumnControl(editEntityArea, detail.ColumnID);
                            if (simpleColumn != null)
                            {
                                if (detail.Hidden == true)
                                    editEntityArea.SetSimpleColumnHidden(dataItem, simpleColumn,state);
                                else if (detail.Readonly == true)
                                    editEntityArea.SetSimpleColumnReadonly(dataItem, simpleColumn, state);
                            }
                        }
                        else if (detail.RelationshipID != 0)
                        {
                            var relationshipControl = GetRelationshipControl(editEntityArea, detail.RelationshipID);


                            if (detail.Hidden == true)
                            {
                                if (relationshipControl.Item2 == false)
                                {
                                    //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
                                    //{
                                    //    (relationshipControl.Item1 as RelationshipColumnControlOneData).BusinessHidden = true;
                                    //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.Visiblity(false);
                                    //}
                                    if (relationshipControl.Item1 is RelationshipColumnControl)
                                    {
                                        (relationshipControl.Item1 as RelationshipColumnControl).BusinessHidden[dataItem] = true;
                                        (relationshipControl.Item1 as RelationshipColumnControl).ControlManager.Visiblity(dataItem, false);
                                    }
                                }
                                else
                                {
                                    //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
                                    //{
                                    //    dataItem.BusinessHidden = true;
                                    //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.AddBusinessMessage(new BaseValidationItem() { CausingDataItem = dataItem, Color = Temp.InfoColor.Red, Message = "داده اثری نخواهد داشت" });
                                    //}

                                    //بعدا بررسی شود
                                    if (relationshipControl.Item1 is RelationshipColumnControl)
                                    {
                                        dataItem.BusinessHidden = true;
                                        (relationshipControl.Item1 as RelationshipColumnControl).EditNdTypeArea.AddDataBusinessMessage("داده اثری نخواهد داشت", Temp.InfoColor.Red, "stateActionActivity", dataItem, ControlItemPriority.High);
                                    }
                                }
                            }
                            else if (detail.Readonly == true)
                            {
                                if (relationshipControl.Item2 == false)
                                {
                                    //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
                                    //{
                                    //    relationshipControl.Item1.EditNdTypeArea.AreaInitializer.BusinessReadOnlyByParent = true;
                                    //    if (relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
                                    //         || relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                                    //    {
                                    //        relationshipControl.Item1.EditNdTypeArea.ImposeDataViewSecurity();
                                    //    }
                                    //    else if (relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
                                    //      || relationshipControl.Item1.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
                                    //    {
                                    //        relationshipControl.Item1.EditNdTypeArea.ImposeTemporaryViewSecurity();
                                    //    }
                                    //}

                                    if (relationshipControl.Item1 is RelationshipColumnControl)
                                    {
                                        relationshipControl.Item1.EditNdTypeArea.AreaInitializer.AddBusinessReadOnlyByParent(dataItem);
                                        /////اینجا درست شه به ریدئنلی بودن خود فرم هم دقت شود
                                        (relationshipControl.Item1 as RelationshipColumnControl).RelationshipControlManager.EnableDisable(dataItem, TemporaryLinkType.SerachView, false);
                                        (relationshipControl.Item1 as RelationshipColumnControl).RelationshipControlManager.EnableDisable(dataItem, TemporaryLinkType.Clear, false);
                                    }
                                }
                                else
                                {
                                    //if (relationshipControl.Item1 is RelationshipColumnControlOneData)
                                    //{
                                    //    dataItem.BusinessReadonly = true;
                                    //    (relationshipControl.Item1 as RelationshipColumnControlOneData).ControlManager.AddBusinessMessage(new BaseValidationItem() { CausingDataItem = dataItem, Color = Temp.InfoColor.Red, Message = "داده فقط خوادنی می باشد" });
                                    //}
                                    if (relationshipControl.Item1 is RelationshipColumnControl)
                                    {
                                        dataItem.BusinessReadonly = true;
                                        (relationshipControl.Item1 as RelationshipColumnControl).EditNdTypeArea.AddDataBusinessMessage("داده فقط خوادنی می باشد", Temp.InfoColor.Red, "stateActionActivity", dataItem, ControlItemPriority.High);
                                    }
                                }
                            }
                        }
                        else if (detail.UICompositionID != 0)
                        {
                            if (detail.Hidden == true)
                            {
                                var uiComposition = GetUIComposition(editEntityArea, detail.UICompositionID);

                                uiComposition.Container.Visiblity(false);
                            }
                        }
                    }

                }
                if (item.UIColumnValueRange.Any())
                {
                    foreach (var columnValueRange in item.UIColumnValueRange.GroupBy(x => x.ColumnValueRangeID))
                    {
                        if (editEntityArea.SimpleColumnControls.Any(x => x.Column.ID == columnValueRange.Key))
                        {
                            var simpleColumn = editEntityArea.SimpleColumnControls.First(x => x.Column.ID == columnValueRange.Key);
                            List<ColumnValueRangeDetailsDTO> candidates = GetFilteredRange(simpleColumn, columnValueRange);
                            if (dataItem.ColumnKeyValueRanges.Any(x => x.Key == simpleColumn.Column.ID))
                                dataItem.ColumnKeyValueRanges[simpleColumn.Column.ID] = candidates;
                            else
                                dataItem.ColumnKeyValueRanges.Add(simpleColumn.Column.ID, candidates);
                            editEntityArea.SetColumnValueRange(simpleColumn, candidates, dataItem);
                        }
                    }
                }
                if (item.UIColumnValueRangeReset.Any())
                {
                    foreach (var columnValueRange in item.UIColumnValueRangeReset.GroupBy(x => x.ColumnValueRangeID))
                    {
                        if (editEntityArea.SimpleColumnControls.Any(x => x.Column.ID == columnValueRange.Key))
                        {
                            var simpleColumn = editEntityArea.SimpleColumnControls.First(x => x.Column.ID == columnValueRange.Key);
                            if (dataItem.ColumnKeyValueRanges.Any(x => x.Key == simpleColumn.Column.ID))
                                dataItem.ColumnKeyValueRanges.Remove(simpleColumn.Column.ID);
                            editEntityArea.ResetColumnValueRange(simpleColumn, dataItem);
                        }
                    }
                }
                if (onChange && item.UIColumnValue.Any())
                {
                    //در واقع مقادیر پیش فرض را ست میکند
                    editEntityArea.SetColumnValueFromState(dataItem, item.UIColumnValue, state);
                }
                editEntityArea.RunningActionActivities.Remove(item);
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
        private Tuple<RelationshipColumnControl, bool?> GetRelationshipControl(I_EditEntityArea editEntityArea, int relationshipID)
        {
            RelationshipColumnControl control = null;
            bool? isSourceRelation = null;
            if (editEntityArea is I_EditEntityAreaOneData)
            {
                if ((editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.Any(x => x.Relationship.ID == relationshipID))
                {
                    control = (editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.First(x => x.Relationship.ID == relationshipID);
                    isSourceRelation = false;
                }
                else if (editEntityArea.AreaInitializer.SourceRelation != null && editEntityArea.AreaInitializer.SourceRelation.Relationship.ID == relationshipID)
                {
                    control = editEntityArea.AreaInitializer.SourceRelation.SourceRelationshipColumnControl;
                    isSourceRelation = true;
                }
            }
            else if (editEntityArea is I_EditEntityAreaMultipleData)
            {
                if ((editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.Any(x => x.Relationship.ID == relationshipID))
                {
                    control = (editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.First(x => x.Relationship.ID == relationshipID);
                    isSourceRelation = false;
                }
                else if (editEntityArea.AreaInitializer.SourceRelation != null && editEntityArea.AreaInitializer.SourceRelation.Relationship.ID == relationshipID)
                {
                    control = editEntityArea.AreaInitializer.SourceRelation.SourceRelationshipColumnControl;
                    isSourceRelation = true;
                }
            }
            return new Tuple<RelationshipColumnControl, bool?>(control, isSourceRelation);
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
    //public class UIActionActivitySource
    //{
    //    //////   public event EventHandler<ObserverDataChangedArg> RelatedDataChanged;
    //    public UIActionActivitySource()
    //    {
    //        EntityStates = new List<EntityStateDTO>();

    //    }

    //    //  public bool IsPresent { set; get; }
    // //   public I_EditEntityArea SourceEditArea { set; get; }
    //    public DP_DataRepository DataItem { set; get; }
    //    // public UIActionActivityDTO ActionActivity { set; get; }
    //    public List<EntityStateDTO> EntityStates { set; get; }
    //    public bool InAction { set; get; }
    //    public void RegisterEvent()
    //    {
    //        ///////      DataItem.RelatedDataChanged += DataItem_RelatedDataChanged;
    //    }
    //    public void UnRegisterEvent()
    //    {
    //        //////     DataItem.RelatedDataChanged -= DataItem_RelatedDataChanged;
    //    }
    //    //////private void DataItem_RelatedDataChanged(object sender, ObserverDataChangedArg e)
    //    //////{
    //    //////    if (RelatedDataChanged != null)
    //    //////        RelatedDataChanged(this, e);
    //    //////}
    //}
}
