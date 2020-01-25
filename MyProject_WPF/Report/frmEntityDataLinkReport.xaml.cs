using ModelEntites;
using MyCommonWPFControls;

using MyModelManager;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityDataLinkReport.xaml
    /// </summary>
    /// 

    public partial class frmEntityDataLinkReport : UserControl
    {
        EntityDataLinkReportDTO Message { set; get; }
        BizEntityDataLinkReport bizEntityDataLinkReport = new BizEntityDataLinkReport();
        int EntityID { set; get; }
        public frmEntityDataLinkReport(int entityID, int entityViewReportID)
        {
            InitializeComponent();
            EntityID = entityID;
            //  SetSubReports();
            //    SetSubReportRelationships();
            SetDataLinksList();
            if (entityViewReportID != 0)
            {
                GetEntityDataLinkReport(entityViewReportID);
            }
            else
            {
                Message = new EntityDataLinkReportDTO();
                ShowMessage();
            }

        }

        private void SetDataLinksList()
        {
            if (lokDataLink.ItemsSource == null)
            {
                lokDataLink.EditItemClicked += LokEntityPreDefined_EditItemClicked; ;
            }
            BizDataLink biz = new BizDataLink();
            lokDataLink.DisplayMember = "Name";
            lokDataLink.SelectedValueMember = "ID";
            lokDataLink.ItemsSource = biz.GetDataLinkByEntitiyID(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
        }
        private void LokEntityPreDefined_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmDataLink view;
          
            if (lookup.SelectedItem == null)
            {
                view = new frmDataLink(EntityID, 0);
            }
            else
            {
                view = new frmDataLink(EntityID, (int)lookup.SelectedValue);
            }
            view.Updated +=(sender1,e1)=> View_EntityPreDefinedSearchUpdated(sender1,e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه",Enum_WindowSize.Maximized);
        }

        private void View_EntityPreDefinedSearchUpdated(object sender1, UpdatedEventArg e1, MyStaticLookup lookup)
        {
            SetDataLinksList();
            lookup.SelectedValue = e1.ID;
        }

      
      
        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            lokDataLink.SelectedValue = Message.DataLinkID;

            //  dtgSubReports.ItemsSource = Message.EntityDataLinkReportSubs;
            //SetGridSearch();
        }

        //private void SetGridSearch()
        //{
        //    //grdSearch.Children.Clear();
        //    //grdSearch.Children.Add(new frmSearchEntityArea(EntityID, Message.EntitySearchID)
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokDataLink.SelectedItem == null)
            {
                MessageBox.Show("لطفا ابتدا لیست لینک را ثبت و انتخاب نمایید");
                return;
            }
            if (txtReportName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text;
            Message.DataLinkID = (int)lokDataLink.SelectedValue;
          
            bizEntityDataLinkReport.UpdateEntityDataLinkReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityDataLinkReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmEntityDataLinkReportSelect view = new MyProject_WPF.frmEntityDataLinkReportSelect(EntityID);
            view.EntityDataLinkReportSelected += View_EntityDataLinkReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityDataLinkReportSelect");
        }

        private void View_EntityDataLinkReportSelected1(object sender, EntityDataLinkReportSelectedArg e)
        {
            if (e.EntityDataLinkReportID != 0)
            {
                GetEntityDataLinkReport(e.EntityDataLinkReportID);
            }
        }

        private void GetEntityDataLinkReport(int entityDataLinkReportID)
        {
            Message = bizEntityDataLinkReport.GetEntityDataLinkReport(entityDataLinkReportID, true);
            ShowMessage();
        }
        //SearchEntityArea SearchEntityArea;
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SearchEntityArea == null)
        //    {
        //        SearchEntityArea = new SearchEntityArea();
        //        var searchViewInitializer = new SearchEntityAreaInitializer();
        //        searchViewInitializer.EntityID = EntityID;
        //        SearchEntityArea.SetAreaInitializer(searchViewInitializer);
        //        SearchEntityArea.SearchDataDefined += SearchEntityArea_SearchDataDefined;
        //    }
        //    MyProjectManager.GetMyProjectManager.ShowDialog(SearchEntityArea.SearchView, "جستجو");
        //}

       
    }
}
