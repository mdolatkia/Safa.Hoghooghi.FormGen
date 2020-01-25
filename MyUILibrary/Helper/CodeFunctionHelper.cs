using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ModelEntites;
using ProxyLibrary;
using MyInterfaces;

namespace MyUILibrary
{
    //public class CodeFunctionHelper
    //{


    //    //internal void CalculateFormula(EditEntityArea editEntityArea, CodeFunctionDTO codeFunctionDTO, DP_DataRepository data)
    //    //{

    //    //    var result = CalculateCodeFunction(editEntityArea, columnControl.Column.CustomFormula, data);


    //    //    editEntityArea.ShowTypePropertyControlValue(data, columnControl, result.ToString());
    //    //}

    //    //internal CodeFunctionResult CalculateCodeFunction(I_EditEntityArea editEntityArea, CodeFunctionDTO codeFunctionDTO, DP_DataRepository data)
    //    //{
    //    //    List<DP_DataRepository> dataItems = new List<DP_DataRepository>();
    //    //    dataItems.Add(data);
    //    //    GetDataItems(dataItems, editEntityArea, data);

    //    //    return AgentUICoreMediator.GetAgentUICoreMediator.CalculateCodeFunction(codeFunctionDTO.ID, dataItems);
    //    //}

    //    private void GetDataItems(List<DP_DataRepository> dataItems, I_EditEntityArea editEntityArea, DP_DataRepository data)
    //    {

    //        //////foreach (var controlColumn in editEntityArea.RelationshipColumnControls)
    //        //////{
    //        //////    var relatedEditEntityArea = controlColumn.EditNdTypeArea;
    //        //////    if (relatedEditEntityArea != null)
    //        //////    {
    //        //////        var relatedData = AgentHelper.ExtractAreaInitializerData(relatedEditEntityArea.AreaInitializer, data);
    //        //////        foreach (var cdata in relatedData)
    //        //////        {
    //        //////            dataItems.Add(cdata);
    //        //////            GetDataItems(dataItems, relatedEditEntityArea, cdata);
    //        //////        }
    //        //////    }

    //        //////}
    //    }


    //}
}
