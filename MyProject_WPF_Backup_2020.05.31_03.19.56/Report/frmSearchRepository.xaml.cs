using ModelEntites;
using MyCommonWPFControls;
using MyModelManager;
using MyUIGenerator;
using MyUILibrary.EntityArea;
using ProxyLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmSearchEntityArea.xaml
    /// </summary>
    /// 

    public partial class frmSearchRepository : UserControl
    {
        DP_SearchRepository Message { set; get; }
        SearchEntityArea SearchEntityArea;
        BizSearchRepository bizSearchRepository = new BizSearchRepository();
        public event EventHandler<EntityPreDefinedSearchUpdatedArg> EntityPreDefinedSearchUpdated;
        int EntityID { set; get; }
        //int SearchEntityID { set; get; }
        public frmSearchRepository(int entityID, int preDefinedSearchID)
        {
            InitializeComponent();
            EntityID = entityID;

            MyUILibrary.AgentUICoreMediator.GetAgentUICoreMediator.SetUIManager(new UIManager());
            var userInfo = new MyUILibrary.UserInfo();
            userInfo.AdminSecurityInfo = new MyUILibrary.AdminSecurityInfo() { IsActive = true, ByPassSecurity = true };
            MyUILibrary.AgentUICoreMediator.GetAgentUICoreMediator.UserInfo = userInfo;

            SetEntitySearchList();

            if (preDefinedSearchID != 0)
            {
                GetSearchRepositoryh(preDefinedSearchID);
                //   ShowMessage();
            }
            else
            {
                SetNewItem();
            }
            lokEntitySearch.SelectionChanged += LokEntitySearch_SelectionChanged;
        }

        private void LokEntitySearch_SelectionChanged(object sender, SelectionChangedArg e)
        {
            ShowSearchArea();
        }

        private void ShowSearchArea()
        {
            int entitySearchID = lokEntitySearch.SelectedItem == null ? 0 : (int)lokEntitySearch.SelectedValue;
            //if (SearchEntityArea == null || SearchEntityArea.SearchInitializer.SearchEntityID != entitySearchID)
            //{
                SearchEntityArea = new SearchEntityArea();
                var searchViewInitializer = new SearchEntityAreaInitializer();
                searchViewInitializer.EntityID = EntityID;
                searchViewInitializer.SearchEntityID = entitySearchID;
                SearchEntityArea.SetAreaInitializer(searchViewInitializer);
                grdSearch.Children.Clear();
                grdSearch.Children.Add(SearchEntityArea.SearchView as UIElement);
            //}

            SearchEntityArea.ShowSearchRepository(Message);
        }

        private void GetSearchRepositoryh(int id)
        {
            Message = bizSearchRepository.GetSearchRepository(id);
            ShowMessage();
        }

        private void SetEntitySearchList()
        {
            if (lokEntitySearch.ItemsSource == null)
            {
                lokEntitySearch.EditItemClicked += LokEntitySearch_EditItemClicked; ;
            }
            BizEntitySearch biz = new BizEntitySearch();
            lokEntitySearch.DisplayMember = "Title";
            lokEntitySearch.SelectedValueMember = "ID";
            lokEntitySearch.ItemsSource = biz.GetEntitySearchs(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
        }

        private void LokEntitySearch_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmEntitySearch view;
            if (lookup.SelectedItem == null)
            {
                view = new frmEntitySearch(EntityID, 0);
            }
            else
            {
                view = new frmEntitySearch(EntityID, (int)lookup.SelectedValue);
            }
            view.EntitySearchUpdated += (sender1, e1) => View_EntitySearchUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
        }
        private void View_EntitySearchUpdated(object sender, EntitySearchUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntitySearchList();
            lookup.SelectedValue = e.ID;
        }

        private void ShowMessage()
        {
            txtTitle.Text = Message.Title;
            lokEntitySearch.SelectedValue = Message.EntitySearchID;
            ShowSearchArea();


        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان جستجو الزامی می باشد");
                return;
            }

            //if (SearchEntityArea.IsSimpleSearchActiveOrAdvancedSearch)
            //{
            //  Message.IsSimpleSearch = true;
            //   Message.SimpleColumns = SearchEntityArea.SimpleSearchEntityArea.GetSearchColumns();
            //  Message.SearchRepository = null;
            Message = SearchEntityArea.GetSearchRepository();
            Message.Title = txtTitle.Text;
            // }
            // else
            // {
            ////     Message.SimpleColumns = new List<PreDefinedSearchColumns>();
            //   //  Message.IsSimpleSearch = false;
            //     Message.SearchRepository = SearchEntityArea.AdvancedSearchEntityAre.GetSearchRepository();
            // }
            //if (lokEntitySearch.SelectedItem != null)
            //    Message.EntitySearchID = (int)lokEntitySearch.SelectedValue;
            //else
            //    Message.EntitySearchID = 0;
            Message.ID = bizSearchRepository.Update(Message);
            MessageBox.Show("اطلاعات ثبت شد");

            if (EntityPreDefinedSearchUpdated != null)
                EntityPreDefinedSearchUpdated(this, new MyProject_WPF.EntityPreDefinedSearchUpdatedArg() { ID = Message.ID });
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewItem();
         
        }

        private void SetNewItem()
        {
            Message = new DP_SearchRepository(EntityID);
            Message.Title = "جستجوی جدید";
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmSearchRepositoryList view = new MyProject_WPF.frmSearchRepositoryList(EntityID);
            view.PreDefinedSearchSelected += View_PreDefinedSearchSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityDataViewReportSelect");
        }

        private void View_PreDefinedSearchSelected(object sender, SearchRepositorySelectedArg e)
        {
            if (e.SearchRepositoryID != 0)
            {
                GetSearchRepositoryh(e.SearchRepositoryID);
            }
        }
    }
    public class EntityPreDefinedSearchUpdatedArg : EventArgs
    {
        public int ID { set; get; }
    }
}
