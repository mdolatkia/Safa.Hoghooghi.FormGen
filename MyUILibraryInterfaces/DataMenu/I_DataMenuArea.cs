using ModelEntites;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.DataViewArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.DataMenuArea
{
    public class DataMenuAreaInitializer
    {
        public object SourceView { set; get; }
        public DP_DataView DataItem { set; get; }
        //public List<EntityInstanceProperty> KeyProperties { set; get; }
        public I_DataArea HostDataViewArea { set; get; }
        public I_DataViewItem HostDataViewItem { set; get; }
        //public Dictionary<string, EntityRelationshipTailDTO> DataViewRelationshipTails { set; get; }
        //public Dictionary<string, EntityRelationshipTailDTO> GridViewRelationshipTails { get; set; }
    }
    public interface I_DataMenuArea
    {
        //    void SetInitializer(DataMenuAreaInitializer initializer);
    }


    public class DataMenuUI
    {
        public event EventHandler MenuClicked;
        public DataMenuUI()
        {
            SubMenus = new List<DataMenuUI>();
        }

        public void OnMenuClicked()
        {
            if (MenuClicked != null)
                MenuClicked(this, null);
        }
        public string Tooltip { set; get; }
        public string Title { set; get; }
        public List<DataMenuUI> SubMenus { set; get; }
        public DataMenu DataMenu { set; get; }
        //public DP_DataView IsViewTargetDataItem { get; set; }
    }

}
