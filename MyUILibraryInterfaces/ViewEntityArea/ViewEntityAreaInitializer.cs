
using CommonDefinitions.UISettings;
using ModelEntites;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public class ViewEntityAreaInitializer
    {


        public ViewEntityAreaInitializer()
        {

            ViewData = new List<DP_DataView>();
            UISettings = new TemplateEntityUISettings();
            //UISettings.FlowDirection = FlowDirection.RightToLeft;
            UISettings.Language = "farsi";


        }
        //public MyUILibrary.EntityArea.I_EditEntityArea SourceEditArea;

        public TemplateEntityUISettings UISettings { set; get; }


        public bool AllowSelect;
        public bool MultipleSelection { set; get; }
        //public TableDrivedEntityDTO TempEntity { set; get; }
        public List<DP_DataView> ViewData { set; get; }
        public int EntityListViewID { set; get; }
        public List<EntityUICompositionDTO> UICompositions { get; set; }
        public AssignedPermissionDTO Permissoins { set; get; }
        public int EntityID { get; set; }
    }

}
