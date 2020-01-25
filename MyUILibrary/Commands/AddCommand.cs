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
    public class AddCommand : BaseCommand
    {
        I_EditEntityAreaMultipleData EditArea { set; get; }
        public AddCommand(I_EditEntityAreaMultipleData editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Add");
            //else
            CommandManager.SetTitle("افزودن");
            CommandManager.ImagePath = "Images//add.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            int relationID = 0;
            DP_DataRepository RelationData = null;
            //if (EditArea.AreaInitializer.SourceRelation != null)
            //{
            //    relationID = EditArea.AreaInitializer.SourceRelation.Relationship.ID;
            //    RelationData = EditArea.AreaInitializer.SourceRelation.RelatedData;
            //    //if (RelationData != null)
            //    //    RelationData.ValueChanged = true;
            //}
           
            DP_DataRepository newData = AgentHelper.CreateAreaInitializerNewData(EditArea);
            //var list = AgentHelper.CreateListFromSingleObject<DP_DataRepository>(newData);
            //if (EditArea.AreaInitializer.SourceRelation == null)
            //{
            //    EditArea.AreaInitializer.Datas.Add(newData);
            //}
            //else
            //{
            //    EditArea.ChildRelationshipInfo.RelatedData.Add(newData);
            //}
            EditArea.AddData(newData, true);
            //EditArea.ShowDataInDataView(list, false);
        }

        public void Execute(I_EditEntityArea editArea)
        {


            //     editArea.ShowData(AgentHelper.CreateListFromSingleObject<DP_DataRepository>(newData));
        }
        //bool _Enabled;
        //public bool Enabled
        //{
        //    get
        //    {
        //        return _Enabled;
        //    }
        //    set
        //    {
        //        _Enabled = value;
        //        OnEnabledChanged();
        //    }
        //}

        //public event EventHandler EnabledChanged;


        //public void OnEnabledChanged()
        //{
        //    if (EnabledChanged != null)
        //        EnabledChanged(this, null);
        //}



    }
}
