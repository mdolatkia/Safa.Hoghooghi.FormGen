using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyUIGenerator.UIControlHelper
{

    public class LocalContainerManager : View_GridContainer, I_UICompositionContainer
    {
        public Expander Expander { set; get; }
        public GroupBox GroupBox { set; get; }
        public FrameworkElement MainControl { set; get; }
        public GroupUISettingDTO GroupUISettingDTO { set; get; }

        public object UIMainControl
        {
            get
            {
                return MainControl;
            }
        }

        public LocalContainerManager(GroupUISettingDTO groupUISettingDTO, int columnsCount) : base(columnsCount)
        {

            GroupUISettingDTO = groupUISettingDTO;
            GroupBox = new GroupBox();
            GroupBox.Margin = new Thickness(0, 1, 0, 0);
            GroupBox.BorderThickness = new Thickness(1);
            GroupBox.BorderBrush = new SolidColorBrush(Colors.LightGray);
            GroupBox.Content = ContentScrollViewer;

            if (groupUISettingDTO.Expander == false)
            {
                MainControl = GroupBox;
            }
            else
            {
                Expander = new Expander();
                Expander.Margin = new Thickness(0, 1, 0, 0);
                Expander.BorderThickness = new Thickness(1);
                Expander.BorderBrush = new SolidColorBrush(Colors.LightGray);
                Expander.Content = GroupBox;
                Expander.IsExpanded = groupUISettingDTO.IsExpanded;
                MainControl = Expander;
            }

        }

        public void SetVisibility(bool visible)
        {
            MainControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    public class TabGroupContainerManager : I_TabGroupContainer
    {
        public UIControlPackageTree UIControlPackageTreeItem { get; set; }

        public TabGroupUISettingDTO TabGroupSetting { get; internal set; }
        public TabControl TabControl;
        public FrameworkElement MainControl { get; }

        public object UIMainControl
        {
            get
            {
                return MainControl;
            }
        }

        public Expander Expander;
        public TabGroupContainerManager(TabGroupUISettingDTO groupUISettingDTO)
        {

            TabGroupSetting = groupUISettingDTO;
            TabControl = new TabControl();
            TabControl.Margin = new Thickness(5);
            TabControl.BorderThickness = new Thickness(1);
            TabControl.BorderBrush = new SolidColorBrush(Colors.LightGray);
            if (groupUISettingDTO.Expander == false)
            {
                MainControl = TabControl;
            }
            else
            {
                Expander = new Expander();
                Expander.Margin = new Thickness(0, 1, 0, 0);
                Expander.BorderThickness = new Thickness(1);
                Expander.BorderBrush = new SolidColorBrush(Colors.LightGray);
                Expander.Content = TabControl;
                Expander.IsExpanded = groupUISettingDTO.IsExpanded;
                MainControl = Expander;
            }

        }
        public void AddTabPage(I_TabPageContainer view, string title, TabPageUISettingDTO groupUISettingDTO, bool skipTitle)
        {

            //LabelHelper.GenerateLabelControl(title, new ColumnUISettingDTO());
            var tabItem = (view as TabPageContainerManager).TabItem;
            if (!skipTitle)
            {
                var headerControl = new LabelControlManager(title, false);
                tabItem.Header = headerControl.WholeControl;
            }
            TabControl.Items.Add(tabItem);
        }
        public void SetVisibility(bool visible)
        {
            MainControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }



    public class TabPageContainerManager : View_GridContainer, I_TabPageContainer
    {

        public TabItem TabItem { set; get; }
        public TabPageUISettingDTO TabPageUISettingDTO { set; get; }

        public bool HasHeader
        {
            get
            {
                return TabItem.Header != null;
            }

        }

        public object UIMainControl
        {
            get
            {
                return TabItem;
            }
        }

        public TabPageContainerManager(TabPageUISettingDTO tabpageUISettingDTO, int columnsCount) : base(columnsCount)
        {
            TabItem = new TabItem();
            TabItem.Content = ContentScrollViewer;
            TabPageUISettingDTO = tabpageUISettingDTO;
        }
        public void SetVisibility(bool visible)
        {
            TabItem.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
