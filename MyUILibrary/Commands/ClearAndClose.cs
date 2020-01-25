using CommonDefinitions.UISettings;
using MyUILibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class ClearAndClose : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public ClearAndClose(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("پاک کردن و بستن");
            CommandManager.ImagePath = "Images//clear.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //if (EditArea is I_EditEntityArea)
            //{

            if (EditArea is I_EditEntityAreaOneData)
            {
                (EditArea as I_EditEntityAreaOneData).ClearData(false);
                
            }
            else if (EditArea is I_EditEntityAreaMultipleData)
            {
                (EditArea as I_EditEntityAreaMultipleData).ClearData();
            }
            if (EditArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
                || EditArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                if (EditArea is I_EditEntityAreaOneData)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog((EditArea as I_EditEntityAreaOneData).DataView);
                else if (EditArea is I_EditEntityAreaMultipleData)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog((EditArea as EditEntityAreaMultipleData).DataView);
            }
            //if (EditArea.AreaInitializer.SourceRelation == null)
            //    EditArea.AreaInitializer.Datas.Clear();
            //else
            //{
            //    EditArea.ChildRelationshipInfo.RelatedData.Clear();
            //    //if (EditArea.ParentTemporaryView != null)
            //    //    EditArea.ParentTemporaryView.SetLinkText("");
            //}
            //(EditArea as I_EditEntityAreaOneData).CreateDefaultData();
            //}
            //else if (EditArea is I_EditEntityAreaMultipleData)
            //{
            //    if (EditArea.AreaInitializer.SourceRelation == null)
            //        EditArea.AreaInitializer.Datas.Clear();
            //    else
            //    {
            //        EditArea.AreaInitializer.SourceRelation.RelatedData.RemoveRelatedData(EditArea.AreaInitializer.SourceRelation.Relationship.ID);
            //        if (EditArea.ParentTemporaryView != null)
            //            EditArea.ParentTemporaryView.SetLinkText("");
            //    }
            //    (EditArea as I_EditEntityAreaMultipleData).ClearDataContainsers();
            //}
        }





    }
}
