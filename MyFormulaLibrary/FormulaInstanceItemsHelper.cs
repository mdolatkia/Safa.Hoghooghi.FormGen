using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;

namespace MyFormulaFunctionStateFunctionLibrary
{
    class FormulaInstanceItemsHelper
    {
        List<FormulaItemDTO> FormulaItems = new List<FormulaItemDTO>();
        //////internal List<FormulaItemDTO> GetFormulaItems(FormulaDefinitionInstance formulaInstance, string expressionStr)
        //////{
        //////    FormulaItems.Clear();
        //////    //if (MainFormulaObject is FormulaObject)
        //////    //{
        //////    //////formulaInstance.PropertyGetCalled += FormulaInstance_PropertyGetCalled;
        //////    //}
        //////    //else if (mainFormulaObjectOrList is IList)
        //////    //{
        //////    //    foreach (FormulaObject item in (mainFormulaObjectOrList as IList))
        //////    //    {
        //////    //        item.PropertyGetCalled += FormulaInstance_PropertyGetCalled;
        //////    //    }
        //////    //}
        //////    //     GetExpression(expressionStr);
        //////    //////formulaInstance.CalculateExpression(expressionStr);
        //////    //////formulaInstance.PropertyGetCalled -= FormulaInstance_PropertyGetCalled;
        //////    return FormulaItems;
        //////}

        private void FormulaInstance_PropertyGetCalled(object sender, PropertyGetArg e)
        {
            //var parentItem = GetParentFormulaItem(FormulaItems, e);

            if (FormulaItemExists(e.PropertyInfo, e.PropertyInfo.ID))
            {
                return;
            }
            FormulaItemDTO newItem = new FormulaItemDTO();
            newItem.ItemType = GetFormulaItemType(e.PropertyInfo.PropertyType);
            if (newItem.ItemType == FormuaItemType.None)
                return;
            newItem.ItemID = e.PropertyInfo.ID;
            newItem.ItemTitle = e.PropertyInfo.Name;
            newItem.RelationshipIDTail = e.PropertyInfo.RelationshipTail;
            newItem.RelationshipNameTail = e.PropertyInfo.RelationshipPropertyTail;

            //if (parentItem == null)
            FormulaItems.Add(newItem);
            //else
            //    parentItem.ChildFormulaItems.Add(newItem);
        }

        //private string GetItemTitle(MyPropertyInfo propertyInfo)
        //{
        //    propertyInfo.titl
        //}

        private FormuaItemType GetFormulaItemType(PropertyType propertyType)
        {
            if (propertyType == PropertyType.Column)
                return FormuaItemType.Column;
            else if (propertyType == PropertyType.Relationship)
                return FormuaItemType.Relationship;
            else if (propertyType == PropertyType.FormulaParameter)
                return FormuaItemType.FormulaParameter;
            //else if (propertyType == PropertyType.DBFormula)
            //    return FormuaItemType.DatabaseFunction;
            else if (propertyType == PropertyType.Code)
                return FormuaItemType.Code;
            else if (propertyType == PropertyType.State)
                return FormuaItemType.State;
            //else if (propertyType == PropertyType.Helper)
            //    return FormuaItemType.he;
            return FormuaItemType.None;
        }

        //private FormulaItemDTO GetParentFormulaItem(List<FormulaItemDTO> formulaItems, PropertyGetArg e)
        //{

        //    if (string.IsNullOrEmpty(e.PropertyInfo.RelationshipTail))
        //        return null;

        //    foreach (var item in formulaItems)
        //    {

        //        if (item.RelationshipTail == e.PropertyInfo.RelationshipTail)
        //            return item;

        //        var fItem = GetParentFormulaItem(item.ChildFormulaItems, e);
        //        if (fItem != null)
        //            return fItem;
        //    }
        //    return null;
        //}
        private bool FormulaItemExists(MyPropertyInfo propertyInfo, int formulaItemID)
        {
            foreach (var item in FormulaItems)
            {
                if (item.RelationshipIDTail == propertyInfo.RelationshipTail && item.ItemType == GetFormulaItemType(propertyInfo.PropertyType) && item.ItemID == propertyInfo.ID)
                    return true;
                //if (propertyInfo.PropertyType == PropertyType.Relationship)
                //{
                //    if (item.RelationshipTail == propertyInfo.RelationshipTail + (string.IsNullOrEmpty(propertyInfo.RelationshipTail) ? "" : ",") + propertyInfo.ID)
                //        return true;
                //}
                //else
                //{
                //if (item.ItemType == GetFormulaItemType(propertyInfo.PropertyType)
                //  && item.ItemID == formulaItemID)
                //{
                //    return true;
                //}
            }
            return false;
        }

    }
}
