
//using MyUIGenerator.UIContainerHelper;
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProxyLibrary;
using MyUIGenerator.View;
using System.Windows.Data;

namespace MyUIGenerator.UIControlHelper
{
    public class View_GridContainer : View_Container, I_View_GridContainer
    {
        public int ColumnsCount
        {
            set; get;
        }

        public View_GridContainer(int columnsCount)
        {
            //columnsCount = 4;
            if (columnsCount == 0)
                columnsCount = 2;
            ColumnsCount = columnsCount;
            Grid = GenerateControl();

        }

        //public Grid ContentGrid
        //{
        //    get { return Grid; }
        //}
        ScrollViewer _ContentScrollViewer;
        public ScrollViewer ContentScrollViewer
        {
            get
            {
                if (_ContentScrollViewer == null)
                {
                    _ContentScrollViewer = new ScrollViewer();
                    _ContentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    _ContentScrollViewer.Content = Grid;
                }
                return _ContentScrollViewer;
            }
        }

        private Grid Grid
        {
            set;
            get;
        }
        //public override void AddUIControlPackage(UIControlPackageOneData controlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{

        //}
        ////public GridSetting GridSetting { set; get; }





        private Grid GenerateControl()
        {

            var grid = new Grid();

            int uiColumnsCount = ColumnsCount * 2;
            for (var i = 0; i < uiColumnsCount; i++)
            {
                var columnDefinition = new ColumnDefinition();
                if (i % 2 == 0)
                {
                    //columnDefinition.Width = GridLength.Auto;
                    columnDefinition.Width = new GridLength(100);
                }
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(30);
            grid.RowDefinitions.Add(rowDefinition);
            return grid;
        }

        private void MoveToNewRow()
        {
            if (Grid.RowDefinitions.Count - 1 <= CurrentRow)
                AddRow();

            CurrentColumn = 1;
            CurrentRow++;
        }

        private void AddRow()
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(30);
            Grid.RowDefinitions.Add(rowDefinition);
        }

        //  ControlHelper _ControlHelper;
        //public ControlHelper ControlHelper
        //{
        //    get
        //    {
        //        if (_ControlHelper == null)
        //            _ControlHelper = new ControlHelper();
        //        return _ControlHelper;
        //    }
        //}


        //public void RemoveUIControlPackage(I_SimpleControlManager control)
        //{
        //    var control1 = (control as LocalControlManager);
        //    Grid.Children.Remove(control1.MainControl);

        //    if (control1.RelatedControl != null)
        //        foreach (var relControl in control1.RelatedControl)
        //            Grid.Children.Remove(relControl);

        //}

        public void RemoveView(object view)
        {
            Grid.Children.Remove(view as UIElement);
        }
        public override void AddUIControlPackage(I_SimpleControlManager control, I_LabelControlManager labelControlManager)
        {

            var localControlManager = control as SimpleControlManagerForOneDataForm;
            var localLabelControlManager = labelControlManager as LabelControlManager;
            //FrameworkElement labelControl = null;
            //if (!localControlManager.RelatedControl.Any())
            //{



            //var labelControl = LabelHelper.GenerateLabelControl(title, (control as SimpleControlManagerForOneDataForm).ColumnUISettingDTO);
            //////localControlManager.RelatedControl.Add(labelControl);




            //}
            //else
            //var labelControl = localControlManager.RelatedControl.First();
            //if (controlPackage.UIControl.Control is I_View_Container)
            //    (controlPackage.UIControl.Control as I_View_Container).SetExpanderInfo(labelControl.Control);
            //else
            //AddControlToGrid(labelControl);
            //}
            //var uiControl = controlPackage.UIControl;
            AddControlToGrid(localControlManager.MyControlHelper.WholeControl, localControlManager.ColumnUISettingDTO.UIColumnsType, localControlManager.ColumnUISettingDTO.UIRowsCount, localLabelControlManager.WholeControl);
        }


        public override void AddView(object view, I_LabelControlManager labelControlManager)
        {
            //UISingleControl labelControl = new UISingleControl();
            ////if (!string.IsNullOrEmpty(title))
            ////{
            //labelControl.ColumnSetting = new ModelEntites.ColumnUISettingDTO();
            //labelControl.ColumnSetting.UIColumnsType = ModelEntites.Enum_UIColumnsType.Normal;
            //labelControl.ColumnSetting.UIRowsCount = 1;
            var localRelationshipControlManager = (view as RelationshipControlManagerForOneDataForm);
            var localLabelControlManager = labelControlManager as LabelControlManager;

            //FrameworkElement labelControl = null;
            ////برای پریویو که دوباره لیبل تولید نشود
            //if (!localRelationshipControlManager.RelatedControl.Any())
            //{
            //FrameworkElement labelControl = null;
            //if (localRelationshipControlManager.Expander == null)
            //{
            //    labelControl = LabelHelper.GenerateLabelControl(title, new ColumnUISettingDTO());
            //    //////localRelationshipControlManager.RelatedControl.Add(labelControl);
            //}
            //}
            //else
            //  var labelControl = localRelationshipControlManager.RelatedControl.First();
            //if (controlPackage.UIControl.Control is I_View_Container)
            //    (controlPackage.UIControl.Control as I_View_Container).SetExpanderInfo(labelControl.Control);
            //else
            //AddControlToGrid(labelControl);
            //}
            //var uiControl = controlPackage.UIControl;


            FrameworkElement labelElement = null;
            if (localRelationshipControlManager.Expander != null)
            {
                if (localRelationshipControlManager.View is UC_EditEntityArea)
                {
                    Grid grid = new Grid();
                    Binding bnd = new Binding("ActualWidth");
                    bnd.Mode = BindingMode.OneWay;
                    bnd.Converter = new WidthConverter();
                    bnd.Source = localRelationshipControlManager.Expander;
                    grid.SetBinding(Grid.WidthProperty, bnd);
                    //    grid.Background = new SolidColorBrush(Colors.Aqua);
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.Children.Add(localLabelControlManager.WholeControl);
                    var toolbar = (localRelationshipControlManager.View as UC_EditEntityArea).Toolbar;
                    (toolbar.Parent as Grid).Children.Remove(toolbar);
                    grid.Children.Add(toolbar);
                    Grid.SetColumn(toolbar, 1);
                    localRelationshipControlManager.Expander.Header = grid;
                }
                else
                    localRelationshipControlManager.Expander.Header = localLabelControlManager.WholeControl;
            }
            else
            {
                if (localRelationshipControlManager.TabPageContainer != null)
                    (localRelationshipControlManager.TabPageContainer as TabPageContainerManager).TabItem.Header = localLabelControlManager.WholeControl;
                else
                    labelElement = localLabelControlManager.WholeControl;
            }
            AddControlToGrid(localRelationshipControlManager.MainControl, localRelationshipControlManager.RelationshipUISettingDTO.UIColumnsType,
         localRelationshipControlManager.RelationshipUISettingDTO.UIRowsCount == 0 ? (short)-1 : localRelationshipControlManager.RelationshipUISettingDTO.UIRowsCount, labelElement);
        }

        //private short ConvertRowsTypeToRowsCount(Enum_UIRowsType uIRowsCount)
        //{
        //    if (uIRowsCount == Enum_UIRowsType.One)
        //        return 1;
        //    else if (uIRowsCount == Enum_UIRowsType.Two)
        //        return 2;
        //    else if (uIRowsCount == Enum_UIRowsType.Unlimited)
        //        return -1;
        //    return 1;
        //}

        List<Tuple<int, int>> ReservedCells = new List<Tuple<int, int>>();
        List<Tuple<int, int>> EmptyCells = new List<Tuple<int, int>>();
        int CurrentColumn = 1;
        int CurrentRow = 0;
        //     private int physicalColumnCount;

        public void AddEmptySpace(EmptySpaceUISettingDTO setting)
        {
            if (setting.ExpandToEnd)
            {
                MoveToNewRow();
            }
            else
            {
                if (setting.UIColumnsType != Enum_UIColumnsType.Normal)
                {

                    int physicalColumnCount = GetColumnsCount(setting.UIColumnsType);
                    for (int col = 1; col <= physicalColumnCount; col++)
                    {
                        AddEmptySpace(new EmptySpaceUISettingDTO() { UIColumnsType = Enum_UIColumnsType.Normal });
                    }
                }
                else
                {
                    int physicalColumnCount = GetColumnsCount(setting.UIColumnsType);
                    physicalColumnCount = Convert.ToInt16(physicalColumnCount * 2 - 1);
                    int rowsCount = 1;
                    Tuple<int, int> GetReadyCursor = GetReadeyCursor(physicalColumnCount, rowsCount);
                    CurrentColumn = GetReadyCursor.Item1;
                    CurrentRow = GetReadyCursor.Item2;
                    for (int col = CurrentColumn; col < CurrentColumn + physicalColumnCount; col++)
                        for (int row = CurrentRow; row < CurrentRow + rowsCount; row++)
                        {
                            ReservedCells.Add(new Tuple<int, int>(col, row));
                            ReservedCells.Add(new Tuple<int, int>(col, row));
                        }
                    ReservedCells.Add(new Tuple<int, int>(CurrentColumn - 1, CurrentRow));
                    CurrentColumn += physicalColumnCount + 1;
                }

            }





        }

        private void AddControlToGrid(FrameworkElement mainElement, Enum_UIColumnsType UIColumnsType, short UIRowsCount
            , FrameworkElement labelElement)
        {
            int logicalColumnCount = GetColumnsCount(UIColumnsType);
            int physicalColumnCount = Convert.ToInt16(logicalColumnCount * 2 - 1);
            int rowsCount = 0;
            bool makeRowHeightAuto = false;
            if (UIRowsCount == -1)
            {
                rowsCount = 1;
                makeRowHeightAuto = true;
            }
            else if (UIRowsCount == 0)
                rowsCount = 1;
            else
                rowsCount = UIRowsCount;

            Tuple<int, int> GetReadyCursor = GetReadeyCursor(physicalColumnCount, rowsCount);
            CurrentColumn = GetReadyCursor.Item1;
            CurrentRow = GetReadyCursor.Item2;

            Grid.SetColumn(mainElement, CurrentColumn - (labelElement != null ? 0 : 1));
            Grid.SetRow(mainElement, CurrentRow);
            if (makeRowHeightAuto)
                Grid.RowDefinitions[CurrentRow].Height = GridLength.Auto;
            if (physicalColumnCount > 1)
                Grid.SetColumnSpan(mainElement, physicalColumnCount + (labelElement != null ? 0 : 1));
            else if (labelElement == null)
                Grid.SetColumnSpan(mainElement, physicalColumnCount + 1);

            if (rowsCount > 1)
            {
                var rowcount = Grid.RowDefinitions.Count - 1;
                for (int i = CurrentRow; i <= CurrentRow + rowsCount - 1; i++)
                {
                    if (Grid.RowDefinitions.Count - 1 < i)
                        AddRow();
                }
                Grid.SetRowSpan(mainElement, rowsCount);
                mainElement.VerticalAlignment = VerticalAlignment.Stretch;
            }
            for (int col = CurrentColumn; col < CurrentColumn + physicalColumnCount; col++)
                for (int row = CurrentRow; row < CurrentRow + rowsCount; row++)
                {
                    ReservedCells.Add(new Tuple<int, int>(col, row));
                }
            if (labelElement != null)
            {
                Grid.SetColumn(labelElement, CurrentColumn - 1);
                Grid.SetRow(labelElement, CurrentRow);
                for (int row = CurrentRow; row < CurrentRow + rowsCount; row++)
                {
                    ReservedCells.Add(new Tuple<int, int>(CurrentColumn - 1, row));
                }

                Grid.Children.Add(labelElement);
            }
            CurrentColumn += physicalColumnCount + 1;
            //foreach (var item in Grid.Children)
            //{
            //    if (item == mainElement)
            //        return;
            //}
            Grid.Children.Add(mainElement);
        }
        private Tuple<int, int> GetReadeyCursor(int physicalColumnCount, int rowsCount)
        {
            while (GetReadeyCursor1(physicalColumnCount, rowsCount))
            {
                CurrentColumn += 2;
            }
            return new Tuple<int, int>(CurrentColumn, CurrentRow);
        }
        private bool GetReadeyCursor1(int physicalColumnCount, int rowsCount)
        {
            if (physicalColumnCount + CurrentColumn > Grid.ColumnDefinitions.Count)
            {
                MoveToNewRow();
            }
            bool reserved = false;
            for (int col = CurrentColumn; col < CurrentColumn + physicalColumnCount; col++)
            {
                if (reserved)
                    break;
                for (int row = CurrentRow; row < CurrentRow + rowsCount; row++)
                {
                    if (ReservedCells.Any(x => x.Item1 == col && x.Item2 == row))
                    {
                        reserved = true;
                        break;
                    }
                }
            }
            return reserved;
        }

        private int GetColumnsCount(Enum_UIColumnsType uIColumnsType)
        {
            if (ColumnsCount == 1)
            {
                if (uIColumnsType == Enum_UIColumnsType.Normal)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Half)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Full)
                    return 1;
            }
            else if (ColumnsCount == 2)
            {
                if (uIColumnsType == Enum_UIColumnsType.Normal)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Half)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Full)
                    return 2;
            }
            else if (ColumnsCount == 3)
            {
                if (uIColumnsType == Enum_UIColumnsType.Normal)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Half)
                    return 2;
                else if (uIColumnsType == Enum_UIColumnsType.Full)
                    return 3;
            }
            else if (ColumnsCount == 4)
            {
                if (uIColumnsType == Enum_UIColumnsType.Normal)
                    return 1;
                else if (uIColumnsType == Enum_UIColumnsType.Half)
                    return 2;
                else if (uIColumnsType == Enum_UIColumnsType.Full)
                    return 4;
            }
            return 1;

        }

        public void AddGroup(I_UICompositionContainer view, string title, GroupUISettingDTO groupUISettingDTO)
        {

            //UISingleControl labelControl = new UISingleControl();
            //labelControl.ColumnSetting = new ModelEntites.ColumnUISettingDTO();
            //labelControl.ColumnSetting.UIColumnsType = ModelEntites.Enum_UIColumnsType.Normal;
            //labelControl.ColumnSetting.UIRowsCount = 1;
            //labelControl.Control = LabelHelper.GenerateLabelControl(title, new ColumnUISettingDTO());

            var localContainerManager = (view as LocalContainerManager);

            var headerControl = new LabelControlManager(title, false);

            if ((view as LocalContainerManager).Expander == null)
            {
                (view as LocalContainerManager).GroupBox.Header = headerControl.WholeControl;
            }
            else
            {
                (view as LocalContainerManager).Expander.Header = headerControl.WholeControl;
            }
            AddControlToGrid(localContainerManager.MainControl, localContainerManager.GroupUISettingDTO.UIColumnsType,
         localContainerManager.GroupUISettingDTO.UIRowsCount == 0 ? (short)-1 : localContainerManager.GroupUISettingDTO.UIRowsCount, null);
        }





        public void AddTabGroup(I_TabGroupContainer view, string title, TabGroupUISettingDTO groupUISettingDTO)
        {
            var headerControl = new LabelControlManager(title, false);
            var tabGroupContainerManager = (view as TabGroupContainerManager);
            if (tabGroupContainerManager.Expander != null)
                tabGroupContainerManager.Expander.Header = headerControl.WholeControl;
            AddControlToGrid(tabGroupContainerManager.MainControl, tabGroupContainerManager.TabGroupSetting.UIColumnsType,
            tabGroupContainerManager.TabGroupSetting.UIRowsCount == 0 ? (short)-1 : tabGroupContainerManager.TabGroupSetting.UIRowsCount, null);
        }


        public override void ClearControls()
        {
            Grid.Children.Clear();
        }

        public int ControlsCount { get { return Grid.Children.Count; } }

    }



    public class WidthConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((double)value >= 25)
                return (double)value - 25;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }


}
