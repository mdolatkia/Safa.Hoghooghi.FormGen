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
    public interface I_GeneralEntitySearchArea
    {
        event EventHandler<int?> EntitySelected;

        event EventHandler<SearchDataArg> SearchDataDefined;
        GeneralEntitySearchAreaInitializer AreaInitializer { set; get; }
        I_View_GeneralEntitySearchArea View { set; get; }
        I_SearchEntityArea SearchArea { set; get; }

    void    SetInitializer(GeneralEntitySearchAreaInitializer areaInitializer);
        void EnableDisableSearchArea(bool v);
        TableDrivedEntityDTO SelectedEntity { get; }
        //  void SelectData(DP_DataRepository dataInstance);
    }

    public interface I_View_GeneralEntitySearchArea
    {
        event EventHandler SearchLinkClicked;
        void AddExternalArea(object view);
        void AddEntitySelector(object view, string title);
        void EnableDisable(bool enable);
        void EnableDisableSearchLink(bool enable);
    }

    public class GeneralEntitySearchAreaInitializer
    {
        public int EntityID { set; get; }
        public bool LockEntitySelector { get; set; }

       public object ExternalView { get; set; }
        public DP_SearchRepository PreDefinedSearch { get; set; }
        public List<SecurityAction> SpecificActions { get; set; }
    }
}
