
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_SearchEntityArea : I_View_SearchEntityArea
    {


        //View_Container View_Container { set; get; }
        public UC_SearchEntityArea()
        {
            InitializeComponent();

            //View_Container = new View_Container(basicGridSetting);
            //if (AgentHelper.GetAppMode() != AppMode.Paper)
                FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //grdArea.Children.Add(View_Container.Grid);




        }

        public bool IsSimpleSearchActiveOrAdvancedSearch
        {
            get
            {
                return tabSimpleSearch.IsSelected;
            }

            set
            {
                if (value)
                    tabSimpleSearch.IsSelected = true;
                else
                    tabAdvancedSearch.IsSelected = true;
            }
        }

        public void ActivateAdvancedView()
        {
            tabAdvancedSearch.IsSelected = true;
        }

        public void ActivateSimpleView()
        {
            tabSimpleSearch.IsSelected = true;
        }

        public void AddAdvancedSearchView(object view)
        {
            tabAdvancedSearch.Content = view;
        }

        public void AddSimpleSearchView(object view)
        {
            tabSimpleSearch.Content = view;
        }

        public void DisableEnable(bool enable)
        {
            this.IsEnabled = enable;
        }
    }
}
