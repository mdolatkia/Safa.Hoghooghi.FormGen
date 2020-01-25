using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyUILibrary.EntityArea.Commands;
using CommonDefinitions.UISettings;
using MyUILibraryInterfaces.FormulaCalculationArea;
using MyUILibrary.FormulaArea;

namespace MyUILibrary.EntityArea
{
    public class UIFomulaManager : I_UIFomulaManager
    {
        BaseEditEntityArea EditArea { set; get; }

        public UIFomulaManager(BaseEditEntityArea editArea)
        {
            EditArea = editArea;
            editArea.DataViewGenerated += EditArea_DataViewGenerated;
        }
        List<SimpleColumnControl> FormulaColumns = new List<SimpleColumnControl>();
        private void EditArea_DataViewGenerated(object sender, EventArgs e)
        {
            foreach (var columnControl in EditArea.SimpleColumnControls)
            {
                if (columnControl.Column.CustomFormula != null)
                {
                    FormulaColumns.Add(columnControl);
                }
            }
            if (FormulaColumns.Any())
            {
                AddMenu();
                EditArea.DataItemShown += EditArea_DataItemShown;
                EditArea.DataItemUnShown += EditArea_DataItemUnShown;
            }
        }

        private void EditArea_DataItemUnShown(object sender, EditAreaDataItemArg e)
        {
            foreach (var columnControl in FormulaColumns)
            {
                var fullFormula = AgentUICoreMediator.GetAgentUICoreMediator.formulaManager.GetFormula(columnControl.Column.CustomFormula.ID);
                if (fullFormula.FormulaItems.Any(x => x.ItemType == FormuaItemType.Column || !string.IsNullOrEmpty(x.RelationshipIDTail)))
                {
                    e.DataItem.RemoveChangeMonitorByGenaralKey("formulaColumn" + AgentHelper.GetUniqueDataPostfix(e.DataItem));
                }
            }
        }

        private void EditArea_DataItemShown(object sender, EditAreaDataItemArg e)
        {
            //برای ریمو شدن هم راهی در نظر گرفته شود که ایونتها درست شوند
            foreach (var columnControl in FormulaColumns)
            {

                var fullFormula = AgentUICoreMediator.GetAgentUICoreMediator.formulaManager.GetFormula(columnControl.Column.CustomFormula.ID);
                if (fullFormula.FormulaItems.Any(x => x.ItemType == FormuaItemType.Column || !string.IsNullOrEmpty(x.RelationshipIDTail)))
                {
                    e.DataItem.RelatedDataTailOrColumnChanged += (sender1, e1) => DataItem_RelatedDataTailOrColumnChanged(sender1, e1, columnControl);
                }
                var columnItems = fullFormula.FormulaItems.Where(x => x.ItemType == FormuaItemType.Column);
                if (columnItems.Any())
                {
                    foreach (var item in columnItems)
                    {
                        e.DataItem.AddChangeMonitor("formulaColumn" + AgentHelper.GetUniqueDataPostfix(e.DataItem), columnControl.Column.ID.ToString(), item.RelationshipIDTail, item.ItemID);
                    }
                }
                var relationshipItems = fullFormula.FormulaItems.Where(x => !string.IsNullOrEmpty(x.RelationshipIDTail)).GroupBy(x => x.RelationshipIDTail);
                if (relationshipItems.Any())
                {
                    foreach (var item in relationshipItems)
                    {
                        e.DataItem.AddChangeMonitor("formulaColumn" + AgentHelper.GetUniqueDataPostfix(e.DataItem), columnControl.Column.ID.ToString(), item.Key, 0);
                    }
                }
            }
        }

        private void DataItem_RelatedDataTailOrColumnChanged(object sender, ChangeMonitor e, SimpleColumnControl columnControl)
        {
            if (e.GeneralKey.StartsWith("formulaColumn"))
            {
                if (e.UsageKey == columnControl.Column.ID.ToString())
                {
                    var formulaColumn = FormulaColumns.First(x => x.Column.ID == columnControl.Column.ID).Column.CustomFormula;
                    var dataProperty = e.DataToCall.GetProperty(columnControl.Column.ID);
                    if (dataProperty != null)
                    {
                        CalculateProperty(dataProperty, formulaColumn, e.DataToCall);
                    }
                }
            }
        }




        //private void RelatedDataColumnValueChanged(object sender, RelatedDataColumnValueChangedArg e, FormulaDTO formula, DP_DataRepository dataItem, int columnID)
        //{

        //}


        public void CalculateProperty(EntityInstanceProperty dataProperty, FormulaDTO formula, DP_DataRepository dataItem)
        {
            var result = AgentUICoreMediator.GetAgentUICoreMediator.formulaManager.CalculateFormula(formula.ID, dataItem, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            dataProperty.FormulaID = formula.ID;
            dataProperty.FormulaException = null;
            dataProperty.FormulaUsageParemeters = result.FormulaUsageParemeters;
            EditArea.RemoveControlManagerMessage(dataItem, "formulaCalculated");
            DataMessageItem baseMessageItem = new DataMessageItem();
            baseMessageItem.CausingDataItem = dataItem;
            baseMessageItem.Key = "formulaCalculated";

            baseMessageItem.Priority = ControlItemPriority.Normal;

            if (string.IsNullOrEmpty(result.Exception))
            {
                dataProperty.Value = result.Result.ToString();
                baseMessageItem.Message = "محاسبه شده توسط فرمول" + " " + formula.Title;
            }
            else
            {
                dataProperty.FormulaException = result.Exception;
                dataProperty.Value = "";
                baseMessageItem.Message = "خطا در محاسبه فرمول" + " " + formula.Title + ":" + " " + dataProperty.FormulaException;
            }
            EditArea.AddControlManagerMessage(baseMessageItem);
        }

        //private List<FormulaItemDTO> GetFlatList(List<FormulaItemDTO> treeFormulaItems, List<FormulaItemDTO> result = null)
        //{
        //    if (result == null)
        //        result = new List<FormulaItemDTO>();
        //    foreach (var item in treeFormulaItems)
        //    {
        //        if (item.ItemType == FormuaItemType.Column)
        //            result.Add(item);
        //        GetFlatList(item.ChildFormulaItems, result);
        //    }
        //    return result;
        //}

        private void AddMenu()
        {
            foreach (var columnControl in FormulaColumns)
            {
                var cpMenuFormulaCalculation = new ConrolPackageMenu();
                cpMenuFormulaCalculation.Name = "mnuFormulaCalculation";
                cpMenuFormulaCalculation.Title = "محاسبه فرمول";
                cpMenuFormulaCalculation.Tooltip = columnControl.Column.CustomFormula.Formula;
                columnControl.SimpleControlManager.AddButtonMenu(cpMenuFormulaCalculation);
                cpMenuFormulaCalculation.MenuClicked += (sender1, e1) => CpMenuFormulaCalculation_MenuClicked(sender1, e1, columnControl as SimpleColumnControl);
            }
        }

        private void CpMenuFormulaCalculation_MenuClicked(object sender, ConrolPackageMenuArg e, SimpleColumnControl columnControl)
        {
            DP_DataRepository dataItem = null;
            if (EditArea is I_EditEntityAreaOneData)
                dataItem = EditArea.GetDataList().First();
            else
                dataItem = e.data as DP_DataRepository;
            FormulaCalculationAreaInitializer initializer = new FormulaCalculationAreaInitializer();
            initializer.DataItem = dataItem;
            initializer.FomulaManager = this;
            initializer.Formula = columnControl.Column.CustomFormula;
            initializer.ColumnControl = columnControl;
            var formulaCalculationArea = new FormulaCalculationArea(initializer);
            if (formulaCalculationArea.View != null)
            {
                var window = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow();
                window.ShowDialog(formulaCalculationArea.View, "محاسبه فرمول", Enum_WindowSize.Big);
            }
        }

        bool? decided = null;
        public void UpdateFromulas()
        {
            List<CalculatedPropertyTree> calculatedColumns = new List<EntityArea.CalculatedPropertyTree>();
            UpdateFromulas(calculatedColumns);
            //RemoveRedundantData(calculatedColumns);
            //if (calculatedColumns.Any())
            //{
            //    decided = null;
            //    I_FormulaDataTree formulaTree = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOdFormulaTree();
            //    formulaTree.YesClicked += FormulaTree_YesClicked;
            //    formulaTree.NoClicked += FormulaTree_NoClicked; ;
            //    formulaTree.AddTitle("برای داده های مورد تایید فرمولهای زیر محاسبه شده اند");
            //    AddFormulaNode(formulaTree, calculatedColumns, null);
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(formulaTree, "تایید", Enum_WindowSize.Big, true);
            //    return decided == true;
            //}
            //else
            //    return true;

        }

        //private void FormulaTree_NoClicked(object sender, EventArgs e)
        //{
        //    decided = false;
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        //}

        //private void FormulaTree_YesClicked(object sender, EventArgs e)
        //{
        //    decided = true;
        //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);

        //}

        //private void AddFormulaNode(I_FormulaDataTree formulaTree, List<CalculatedPropertyTree> calculatedColumns, object parentNode)
        //{
        //    foreach (var item in calculatedColumns)
        //    {

        //        var node = formulaTree.AddTreeNode(parentNode, item.DataItem.ViewInfo, item.RelationshipInfo, Temp.InfoColor.Default, true);
        //        foreach (var property in item.Properties)
        //        {
        //            if (!string.IsNullOrEmpty(property.FormulaException))
        //            {
        //                formulaTree.AddTreeNode(node, property.Column.Alias + ":" + " " + property.FormulaException, item.RelationshipInfo, Temp.InfoColor.Red, true);
        //            }
        //            else
        //            {
        //                formulaTree.AddTreeNode(node, property.Column.Alias + ":" +  property.Value, item.RelationshipInfo, Temp.InfoColor.Default, true);
        //            }
        //        }
        //        AddFormulaNode(formulaTree, item.ChildItems, node);
        //    }
        //}


        //private void RemoveRedundantData(List<CalculatedPropertyTree> calculatedColumns)
        //{
        //    foreach (var item in calculatedColumns.ToList())
        //    {
        //        if (!IsValid(item))
        //            calculatedColumns.Remove(item);
        //    }
        //    foreach (var item in calculatedColumns)
        //    {
        //        RemoveRedundantData(item.ChildItems);
        //    }
        //}
        //private bool IsValid(CalculatedPropertyTree item)
        //{
        //    if (item.Properties.Any())
        //        return true;
        //    bool childIsValid = false;
        //    foreach (var child in item.ChildItems)
        //    {
        //        if (IsValid(child))
        //            childIsValid = true;
        //    }
        //    return childIsValid;
        //}
        public void UpdateFromulas(List<CalculatedPropertyTree> result, RelationshipDTO relationship = null)
        {
            var datalist = EditArea.GetDataList().Where(x => x.ShouldBeSkipped == false).ToList();
            foreach (var data in datalist)
            {
                //اینم دیگه بیخوده چوم فقط محاسبه فرمول برای هر پراپرتی رو میخوایمcalculatedPropertyTree
                //بعدا حذف بشه
                CalculatedPropertyTree calculatedPropertyTree = new CalculatedPropertyTree();
                calculatedPropertyTree.DataItem = data;
                if (relationship != null)
                    calculatedPropertyTree.RelationshipInfo = relationship.Alias;
                result.Add(calculatedPropertyTree);
                foreach (var columnControl in EditArea.SimpleColumnControls.Where(x => x.Column.CustomFormula != null))
                {
                    var dataProperty = data.GetProperty(columnControl.Column.ID);
                    if (dataProperty != null)
                    {
                        calculatedPropertyTree.Properties.Add(dataProperty);
                        CalculateProperty(dataProperty, columnControl.Column.CustomFormula, data);
                    }
                    else
                    {
                        throw new Exception("asdasdF");
                    }
                }
                foreach (var relationshipControl in EditArea.RelationshipColumnControls)
                {
                    relationshipControl.EditNdTypeArea.SetChildRelationshipInfo(data.ChildRelationshipInfos.First(x => x.Relationship == relationshipControl.Relationship));
                    if (relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect
                       || relationshipControl.EditNdTypeArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                    {
                        relationshipControl.EditNdTypeArea.AreaInitializer.UIFomulaManager.UpdateFromulas(calculatedPropertyTree.ChildItems, relationshipControl.Relationship);
                    }
                }
            }
        }

    }

}
