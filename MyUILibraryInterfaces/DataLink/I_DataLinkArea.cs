using ModelEntites;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibraryInterfaces.DataViewArea;
using MyCommonWPFControls;

namespace MyUILibraryInterfaces.DataLinkArea
{

    public interface I_DataLinkArea
    {

        DataLinkDTO SelectedDataLink { set; get; }
        DataLinkAreaInitializer AreaInitializer { set; get; }
        I_View_DataLinkArea View { set; get; }
    }

    public interface I_View_DataLinkArea
    {
        //event EventHandler<DataLinkChangedArg> DataLinkChanged;
        //void SetDataLink(string text);
        //event EventHandler DiagramLoaded;
        event EventHandler DataLinkConfirmed;
   //     event EventHandler DataLinkChanged;
     //   object SelectedDataLink { get; set; }
        void ClearEntityViews();
        void SetFirstSideEntityView(I_View_TemporaryView view,string title);
        void SetSecondSideEntityView(I_View_TemporaryView view,string title);
        //object GenerateTailPanel();
        //void AddDataLinkItems(List<I_DataViewItem> views);
        //void AddLink(I_DataViewItem view1, I_DataViewItem view2);
        //void SetItemsPositions();
        void ClearItems();
        void ShowDiagram(List<DataLinkItemViewGroups> viewGroups, I_DataViewItem view1, I_DataViewItem view2);

    //    void SetDataLinks(List<DataLinkDTO> dataLinks, int dataLinkID);
    //    void EnableDisableDataLinks(bool userCanChange);
        void AddDataLinkSelector(MySearchLookup dataLinkSearchLookup);
        void EnabaleDisabeViewSection(bool enable);
    }
    public class DataLinkItemViewGroups
    {
        public DataLinkItemViewGroups()
        {
            Views = new List<I_DataViewItem>();
            ViewRelations = new List<Tuple<I_DataViewItem, I_DataViewItem>>();
        }
        public List<I_DataViewItem> Views { set; get; }
        public List<Tuple<I_DataViewItem, I_DataViewItem>> ViewRelations { set; get; }

    }
    //public interface I_DataLinkItem
    //{
    //    event EventHandler Selected;
    //    event EventHandler InfoClicked;
    //    DP_DataRepository DataItem { set; get; }
    //    void AddTitleRow(string title, string value);

    //    void ShowDataLinkItemMenus(List<DataLinkMenu> menus);
    //    void OnSelected();
    //}

    public class DataLinkAreaInitializer
    {
        public DataLinkAreaInitializer()
        {

        }
     //   public bool UserCanChange { set; get; }
        public int DataLinkID { set; get; }
        public int EntityID { set; get; }
        public DP_DataView FirstDataItem { set; get; }
        // چون جایی نداریم فعلا که دوتا داده انتخاب کنیم و ارتباطشون رو بخوایم, DP_DataView otherData)
        // public DP_DataView OtherDataItem { set; get; }
    }

    public class DataLinkChangedArg : EventArgs
    {
        public int ID { set; get; }
    }

}
