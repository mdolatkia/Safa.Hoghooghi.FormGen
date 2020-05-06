using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class EntityAreaLogManager : I_EditAreaLogManager
    {
        I_DataTree DataTree = null;
        public I_DataTree GetLogDataTree(List<DP_DataRepository> datas)
        {
            var logs = GetDataLogsFromDatas(datas);
            SetDataTree(logs);
            return DataTree;
        }

        private void SetDataTree(List<EditAreaDataActionLog> logs)
        {
            if (DataTree == null)
            {
                DataTree = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDataTreeForm();
                DataTree.CloseRequested += DataTree_CloseRequested;
            }
            DataTree.ClearTree();

            AddTreeNodes(logs);
        }

        private void AddTreeNodes(List<EditAreaDataActionLog> logs, object parentNode = null)
        {
            int i = 1;
            foreach (var item in logs)
            {
                var caption = i + ".";
                caption += (caption == "" ? "" : " ") + GetActionTitle(item.ActionType) + ":";
                ////caption += (caption == "" ? "" : " ") + item.EntityName;
                caption += (caption == "" ? "" : " ") + item.DataInfo;

                var node = DataTree.AddTreeNode(parentNode, caption, i == 1, item.InfoColor);
                if (item.ActionType != LogAction.RemoveRelationship && item.ActionType != LogAction.DeleteData)
                {
                    AddLogProperties(node, item);
                    AddRelationshipProperties(node, item);
                }
                i++;
            }
        }

        private void AddRelationshipProperties(object node, EditAreaDataActionLog item)
        {
            foreach (var relation in item.RelatedLog)
            {
                string caption = "رابطه با" + " " + relation.RelationshipInfo;
                var relNode = DataTree.AddTreeNode(node, caption, true);
                AddTreeNodes(relation.RelatedActions, relNode);
            }
        }

        private void AddLogProperties(object node, EditAreaDataActionLog item)
        {
            if (item.ActionType == LogAction.NewData || item.ActionType == LogAction.AddedToRelationshipNewData)
            {
                foreach (var property in item.LogProperties)
                {
                    string caption = property.ColumnaName;
                    caption += (string.IsNullOrEmpty(caption) ? "" : " : ") + property.NewValue;
                    DataTree.AddTreeNode(node, caption, false, property.InfoColor);
                }
            }
            else
            {
                foreach (var property in item.LogProperties)
                {
                    bool longValue = false;
                    if ((property.NewValue != null && property.NewValue.Length > 50)
                        || (property.OldValue != null && property.OldValue.Length > 50))
                        longValue = true;
                    if (longValue)
                    {
                        string caption = property.ColumnaName;
                        var properyNode = DataTree.AddTreeNode(node, caption, false, property.InfoColor);
                        string oldValue = "مقدار قبلی" + ":";
                        oldValue += (string.IsNullOrEmpty(oldValue) ? "" : " ") + property.OldValue;
                        string newValue = "مقدار جدید" + ":";
                        newValue += (string.IsNullOrEmpty(newValue) ? "" : " ") + property.NewValue;
                        DataTree.AddTreeNode(properyNode, oldValue, false, property.InfoColor);
                        DataTree.AddTreeNode(properyNode, newValue, false, property.InfoColor);
                    }
                    else
                    {
                        string caption = property.ColumnaName;
                        caption += "   " + "مقدار قبلی" + ":";
                        caption += " " + property.OldValue;
                        caption += "   " + "مقدار جدید" + ":";
                        caption += " " + property.NewValue;
                        DataTree.AddTreeNode(node, caption, false, property.InfoColor);
                    }
                }
            }

        }
        private string GetActionTitle(LogAction action)
        {
            if (action == LogAction.NewData)
                return "ثبت داده جدید";
            else if (action == LogAction.EditData)
                return "اصلاح داده";
            else if (action == LogAction.EditDataNotEdited)
                return "داده بدون تغییر";
            else if (action == LogAction.AddedToRelationshipNewData)
                return "اقزودن داده جدید به رابطه";
            else if (action == LogAction.AddedToRelationshipAndEdited)
                return "داده اصلاح شده در رابطه";
            else if (action == LogAction.AddedToRelationshipAndNotEdited)
                return "داده بدون تغییر در رابطه";
            else if (action == LogAction.DeleteData)
                return "حذف داده";
            else if (action == LogAction.RemoveRelationship)
                return "حذف داده از رابطه";
            else
                return "اقدام نامشخص";
        }

        private void DataTree_CloseRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }
        private List<EditAreaDataActionLog> GetDataLogsFromDatas(List<DP_DataRepository> datas)
        {
            List<EditAreaDataActionLog> result = new List<EditAreaDataActionLog>();
            foreach (var item in datas)
            {
                result.Add(ToDataLog(item));
            }
            return result;
        }

        private EditAreaDataActionLog ToDataLog(DP_DataRepository item)
        {
            EditAreaDataActionLog log = new EditAreaDataActionLog();
            if (item.ParantChildRelationshipInfo != null && item.ParantChildRelationshipInfo.DataItemIsAdded(item))
            {
                if (item.IsNewItem)
                    log.ActionType = LogAction.AddedToRelationshipNewData;
                else
                {
                    if (item.IsEdited)
                        log.ActionType = LogAction.AddedToRelationshipAndEdited;
                    else
                        log.ActionType = LogAction.AddedToRelationshipAndNotEdited;
                }
            }
            else
            {
                if (item.IsNewItem)
                    log.ActionType = LogAction.NewData;
                else
                {
                    if (item.IsEdited)
                        log.ActionType = LogAction.EditData;
                    else
                        log.ActionType = LogAction.EditDataNotEdited;
                }
            }
            log.EntityID = item.TargetEntityID;
            //log.EntityName = item.;
            log.DataInfo = item.ViewInfo;
            log.KeyProperties = item.KeyProperties;

            List<EntityInstanceProperty> changedPropeties = null;
            if (item.IsNewItem)
                changedPropeties = item.GetProperties();
            else
                changedPropeties = item.GetProperties().Where(x => x.ValueIsChanged).ToList();
            foreach (var property in changedPropeties)
            {
                var actionLogProperty = new ActionLogProperty();
                actionLogProperty.ColumnaName = property.Column.Alias;
                actionLogProperty.ColumnID = property.ColumnID;
                actionLogProperty.InfoColor = InfoColor.Black;
                var stringvalue = property.Value == null ? "<Null>" : property.Value;
                if (property.FormulaID != 0)
                {
                    if (string.IsNullOrEmpty(property.FormulaException))
                    {
                        actionLogProperty.NewValue = stringvalue + " " + "محاسبه شده توسط فرمول" + " " + property.FormulaID;
                    }
                    else
                    {
                        actionLogProperty.NewValue = stringvalue + " " + property.FormulaException;
                        actionLogProperty.InfoColor = InfoColor.Red;
                    }
                }
                else
                    actionLogProperty.NewValue = stringvalue.ToString();

                if (item.IsNewItem)
                {
                    if (property.Column.IsIdentity)
                        if (AgentHelper.ValueIsEmpty(property))
                            actionLogProperty.NewValue = "<identity>";
                }
                if (item.OriginalProperties.Any(x => x.ColumnID == property.ColumnID))
                {
                    var oldvalue = item.OriginalProperties.First(x => x.ColumnID == property.ColumnID).Value;
                    actionLogProperty.OldValue = oldvalue == null ? "<Null>" : oldvalue.ToString();
                }
                log.LogProperties.Add(actionLogProperty);
            }
            foreach (var child in item.ChildRelationshipInfos)
            {
                var relatedLog = new RelatedDataLog();
                relatedLog.RelationshipID = child.Relationship.ID;
                relatedLog.RelationshipInfo = child.Relationship.Alias;

                foreach (var childData in child.RelatedData)
                {
                    relatedLog.RelatedActions.Add(ToDataLog(childData));
                }
                foreach (var childData in child.RemovedDataForUpdate)
                {
                    relatedLog.RelatedActions.Add(ToRemovedDataLog(childData));
                }
                log.RelatedLog.Add(relatedLog);
            }
            return log;
        }

        private EditAreaDataActionLog ToRemovedDataLog(DP_DataRepository originalData)
        {
            EditAreaDataActionLog log = new EditAreaDataActionLog();
            log.ActionType = LogAction.RemoveRelationship;
            log.EntityID = originalData.TargetEntityID;
            //log.EntityName = item.;
            log.DataInfo = originalData.ViewInfo;
            log.KeyProperties = originalData.KeyProperties;
            return log;
        }
    }
}
