using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ModelEntites;
using ProxyLibrary;

namespace MyUILibrary
{
    public class StateHelper
    {


        //internal void CalculateFormula(EditEntityArea editEntityArea, ColumnControl columnControl, DP_DataRepository data)
        //{

        //    var result = CalculateFormula(editEntityArea, columnControl.Column.CustomFormula, data);


        //    editEntityArea.ShowTypePropertyControlValue(data, columnControl, result.ToString());
        //}

        internal object CalculateState(I_EditEntityArea editEntityArea, EntityStateDTO state, DP_DataRepository data)
        {
            // List<DP_DataRepository> mainItems = new List<DP_DataRepository>();
            // mainItems.Add(data);
        //    List<DP_DataRepository> dataItems = new List<DP_DataRepository>();
           // dataItems.Add(data);
            //if (state.FormulaID != 0)
            //{
            //    GetDataItems(dataItems, editEntityArea, data, state.Formula.FormulaItems);
            //}
            return AgentUICoreMediator.GetAgentUICoreMediator.StateFunctionManager.CalculateFormula(state.ID, data, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
        }
      
        private void GetDataItems(List<DP_DataRepository> dataItems, I_EditEntityArea editEntityArea, DP_DataRepository data, List<FormulaItemDTO> formulaItems)
        {

            foreach (var item in formulaItems.Where(x => x.ItemType == FormuaItemType.Relationship))
            {
                //اینجا یه ایرادی هست.اگر رابطه خود باعث بوجود اومدن فرم شده بود چه اتفاقی می افتد
                //////var relatedEditEntityArea = editEntityArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == item.ItemID)?.EditNdTypeArea;
                //////if (relatedEditEntityArea != null)
                //////{
                //////    var relatedData = AgentHelper.ExtractAreaInitializerData(relatedEditEntityArea.AreaInitializer, data);
                //////    foreach (var cdata in relatedData)
                //////    {
                //////        dataItems.Add(cdata);
                //////        GetDataItems(dataItems, relatedEditEntityArea, cdata, item.ChildFormulaItems);
                //////    }
                //////}

            }
        }

        //private string CalculateFormula(DP_DataRepository dP_DataRepository, FormulaDTO formula)
        //{
        //    List<FormulaParameterDTO> formulaParameters = new List<FormulaParameterDTO>();
        //    foreach (var item in formula.Parameters)
        //    {
        //        item.Value = GetFormulaParameterValue(dP_DataRepository, item, item.FormulaParameterPath);
        //    }
        //    return AgentUICoreMediator.GetAgentUICoreMediator.CalculateFormula(formula.ID, formula.Parameters.ToList());
        //}
        //public string GetFormulaParameterValue(DP_DataRepository editEntityArea, Formula_FormulaParameterDTO item, string parameterPath)
        //{
        //    if (parameterPath.Contains("."))
        //    {
        //        var splt = parameterPath.Split('.');

        //        var relName = splt[0];
        //        var otherParts = "";
        //        for (var i = 1; i <= splt.Count() - 1; i++)
        //        {
        //            otherParts += (otherParts == "" ? "" : ".") + splt[i];
        //        }
        //        var relID = GetRelationshipIDfromPath(relName);

        //        //var relatedEditEntityArea = editEntityArea.ColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == relID)?.EditNdTypeArea;

        //        //if (relatedEditEntityArea != null)
        //        //{
        //        //    if (relatedEditEntityArea.AreaInitializer.DirectionMode == CommonDefinitions.UISettings.DirectionMode.Direct
        //        //        && relatedEditEntityArea.AreaInitializer.FormComposed == true)
        //        //    {
        //        //        ////var existingData = AgentHelper.ExtractAreaInitializerData(relatedEditEntityArea.AreaInitializer, data);
        //        //        return GetFormulaParameterValue(relatedEditEntityArea, item, otherParts, data);

        //        //    }
        //        //    else
        //        //    {

        //        //////var dataFromRelation = AgentUICoreMediator.GetAgentUICoreMediator.GetData(relID, editEntityArea);
        //        //////if (dataFromRelation != null
        //        //////    && dataFromRelation.Count > 0)
        //        //////{
        //        //////    return GetFormulaParameterValue(dataFromRelation.First(), item, otherParts);
        //        //////}

        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        return GetFormulaParameterValue(editEntityArea, item);
        //    }
        //    return "";
        //}
        //public string GetFormulaParameterValue(EditEntityArea editEntityArea, Formula_FormulaParameterDTO item, string parameterPath, DP_DataRepository data)
        //{
        //    if (parameterPath.Contains("."))
        //    {
        //        var splt = parameterPath.Split('.');

        //        var relName = splt[0];
        //        var otherParts = "";
        //        for (var i = 1; i <= splt.Count() - 1; i++)
        //        {
        //            otherParts += (otherParts == "" ? "" : ".") + splt[i];
        //        }
        //        var relID = GetRelationshipIDfromPath(relName);
        //        var relatedEditEntityArea = editEntityArea.ColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == relID)?.EditNdTypeArea;

        //        if (relatedEditEntityArea != null)
        //        {
        //            if (relatedEditEntityArea.AreaInitializer.DirectionMode == CommonDefinitions.UISettings.DirectionMode.Direct
        //                && relatedEditEntityArea.AreaInitializer.FormComposed == true)
        //            {
        //                ////var existingData = AgentHelper.ExtractAreaInitializerData(relatedEditEntityArea.AreaInitializer, data);
        //                return GetFormulaParameterValue(relatedEditEntityArea, item, otherParts, data);

        //            }
        //            else
        //            {
        //                var existingData = AgentHelper.ExtractAreaInitializerData(relatedEditEntityArea.AreaInitializer, data);
        //                if (existingData.Any() && AgentHelper.DataHasValue(existingData.First()))
        //                {
        //                    return GetFormulaParameterValue(existingData.First(), item, otherParts);
        //                }
        //                else
        //                {
        //                    var dataFromRelation = AgentHelper.SelectDataFromDB(relatedEditEntityArea.AreaInitializer.SourceRelation.Relationship, data);
        //                    if (dataFromRelation != null
        //                        && dataFromRelation.Count > 0)
        //                    {
        //                        return GetFormulaParameterValue(dataFromRelation.First(), item, otherParts);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return GetFormulaParameterValue(editEntityArea, item, data);
        //    }
        //    return "";
        //}

        //private string GetFormulaParameterValue(DP_DataRepository dP_DataRepository, Formula_FormulaParameterDTO item)
        //{
        //    if (item.ColumnID != 0)
        //    {
        //        var fcolumn = dP_DataRepository.Properties.FirstOrDefault(x => x.ColumnID == item.ColumnID);
        //        if (fcolumn != null)
        //        {
        //            return fcolumn.Value;
        //        }
        //    }
        //    else if (item.FormulaParameterID != 0)
        //    {

        //        var formulaOfParameter = AgentUICoreMediator.GetAgentUICoreMediator.GetFormula(item.FormulaIDOfFormulaParameter);
        //        if (formulaOfParameter != null)
        //        {
        //            return CalculateFormula(dP_DataRepository, formulaOfParameter);
        //        }
        //    }
        //    return "";
        //}

        //private string GetFormulaParameterValue(EditEntityArea editEntityArea, Formula_FormulaParameterDTO item, DP_DataRepository data)
        //{

        //    if (item.ColumnID != 0)
        //    {
        //        var fcolumnControl = editEntityArea.ColumnControls.FirstOrDefault(x => x.Column.ID == item.ColumnID);
        //        if (fcolumnControl != null)
        //        {
        //            return editEntityArea.FetchTypePropertyControlValue(data, fcolumnControl);
        //        }
        //    }
        //    else if (item.FormulaParameterID != 0)
        //    {

        //        var formulaOfParameter = AgentUICoreMediator.GetAgentUICoreMediator.GetFormula(item.FormulaIDOfFormulaParameter);
        //        if (formulaOfParameter != null)
        //        {
        //            return CalculateFormula(editEntityArea, formulaOfParameter, data);
        //        }
        //    }
        //    return "";
        //}

        //private int GetRelationshipIDfromPath(string relName)
        //{
        //    return Convert.ToInt32(relName.Replace("rel", ""));
        //}
    }
}
