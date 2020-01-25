using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;
using ModelEntites;


namespace MyUILibraryInterfaces.EntityArea
{
    public interface I_EntitySelectArea
    {
        event EventHandler<EditAreaDataItemArg> DataItemSelected;

        event EventHandler<int?> EntitySelected;
        EntitySelectAreaInitializer EntitySelectAreaInitializer { set; get; }
        I_View_EntitySelectArea View { set; get; }
        //I_EditEntityAreaOneData SelectDataArea { set; get; }
        TableDrivedEntityDTO SelectedEntity { get; }
        void EnableDisableSelectArea(bool v);
        void SelectData(DP_DataView dataInstance);
    }

    public interface I_View_EntitySelectArea
    {
        void AddEntitySelector(object view, string title);
        void AddDataSelector(object view, string title);
        void AddExternalArea(object view);
        void RemoveDataSelector();
    }

    public class EntitySelectAreaInitializer
    {
        public EntitySelectAreaInitializer()
        {

        }
        public bool LockEntitySelector { set; get; }
        public int EntityID { set; get; }
        public object ExternalView { get; set; }

        public List<SecurityAction> SpecificActions { set; get; }
    }
}
