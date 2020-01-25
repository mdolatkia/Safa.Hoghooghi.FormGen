
using CommonDefinitions.UISettings;
using ModelEntites;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public class SearchViewAreaInitializer
    {


        public SearchViewAreaInitializer()
        {


            UISettings = new TemplateEntityUISettings();
            //UISettings.FlowDirection = FlowDirection.RightToLeft;
            UISettings.Language = "farsi";


        }
        public bool MultipleSelection { set; get; }
        //public int MainEntitySourceRelationshipID;
        public MyUILibrary.EntityArea.I_EditEntityArea SourceEditArea;
        public TemplateEntityUISettings UISettings { set; get; }


        //public RelationshipFilterInfo RelationshipFilter { set; get; }
        //public string Title
        //{
        //    get
        //    {
        //        return SearchEntity.Alias;
        //    }
        //}
        //public TableDrivedEntityDTO TempEntity { set; get; }
        public int EntityID { get; set; }
    }

}
