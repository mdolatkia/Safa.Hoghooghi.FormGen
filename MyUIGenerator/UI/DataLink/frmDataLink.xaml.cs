using MyUILibraryInterfaces.DataViewArea;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Primitives;
using ModelEntites;
using System.Windows.Threading;
using MyUILibraryInterfaces.DataLinkArea;
using MyUILibrary.EntityArea;
using Telerik.Windows.Diagrams.Core;
using MyCommonWPFControls;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmDataView.xaml
    /// </summary>
    public partial class frmDataLink : UserControl, I_View_DataLinkArea
    {
        public event EventHandler DataLinkConfirmed;
        public event EventHandler DataLinkChanged;

        DispatcherTimer timer = new DispatcherTimer();
        public frmDataLink()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
          //  cmbDataLinks.SelectionChanged += CmbDataLinks_SelectionChanged;
        }

        //private void CmbDataLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DataLinkChanged != null)
        //        DataLinkChanged(this, null);
        //}


        //public object SelectedDataLink
        //{
        //    get
        //    {
        //        return cmbDataLinks.SelectedItem;
        //    }

        //    set
        //    {
        //        cmbDataLinks.SelectedItem=value;

        //    }
        //}



        //private void CmbDataLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbDataLinks.SelectedItem != null)
        //    {
        //        DataLinkChanged(this, new DataLinkChangedArg() { ID = (int)cmbDataLinks.SelectedValue });
        //    }
        //}

        //private void Diagram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems != null)
        //        if (e.AddedItems.Count == 1)
        //        {
        //            var item = e.AddedItems[0] as I_DataViewItem;
        //            if (item != null)
        //            {
        //                item.OnSelected();
        //            }

        //        }
        //}

        //bool _loaded = false;



        //private void FrmDataView_Loaded(object sender, RoutedEventArgs e)
        //{

        //    if (!_loaded)
        //    {
        //        SetItemsPositions();

        //    }
        //    _loaded = true;
        //}

        //public void SetDataLink(string text)
        //{
        //    //cmbDataLinks.SelectedValuePath = "ID";
        //    //cmbDataLinks.DisplayMemberPath = "Name";
        //    //cmbDataLinks.ItemsSource = dataLinks;
        //    //if (defaultID != 0)
        //    //    cmbDataLinks.SelectedValue = defaultID;
        // //   txtDataLink.Text = text;
        //}
        public void ClearEntityViews()
        {
            grdSetting.Children.Remove(FirstEntity);
            grdSetting.Children.Remove(SecondEntity);
        }
        UIElement FirstEntity;
        UIElement SecondEntity;
        public void SetFirstSideEntityView(I_View_TemporaryView view,string title)
        {
            FirstEntity = view as UIElement;
            lblFirstSide.Text = title;
            Grid.SetColumn(FirstEntity, 1);
            grdSetting.Children.Add(FirstEntity);
        }

        public void SetSecondSideEntityView(I_View_TemporaryView view, string title)
        {
            SecondEntity = view as UIElement;
            lblSecondSide.Text = title;
            Grid.SetColumn(SecondEntity, 3);
            grdSetting.Children.Add(SecondEntity);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (DataLinkConfirmed != null)
                DataLinkConfirmed(this, null);
        }

        //public object GenerateTailPanel()
        //{
        //    //RadDiagramContainerShape containerShape = new RadDiagramContainerShape();
        //    //diagram.Items.Add(containerShape);
        //    //return containerShape;
        //    return null;
        //}

        //public void AddDataLinkItems(List<I_DataViewItem> views)
        //{
        //    if (diagram != null)
        //        grdDiagram.Children.Remove(diagram);
        //    diagram = new RadDiagram();
        //    diagram.Loaded += Diagram_Loaded1;
        //    grdDiagram.Children.Add(diagram);
        //    Grid.SetRow(diagram, 1);
        //    foreach (var view in views)
        //    {
        //        diagram.Items.Add(view);
        //        var shape = (diagram.Items[diagram.Items.Count - 1] as UserControl).Parent as RadDiagramShape;
        //        shape.IsConnectorsManipulationEnabled = false;
        //        shape.IsRotationEnabled = false;
        //        shape.IsResizingEnabled = false;
        //    }
        //}

        //private void Diagram_Loaded1(object sender, RoutedEventArgs e)
        //{
        //    if (DiagramLoaded != null)
        //        DiagramLoaded(this, null);
        //}

        //public void AddLink(I_DataViewItem view1, I_DataViewItem view2)
        //{
        //    var shape2 = (view2 as UserControl).Parent as RadDiagramShape;
        //    var shape1 = (view1 as UserControl).Parent as RadDiagramShape;
        //    diagram.AddConnection(shape1, shape2);
        //}

        //public void SetItemsPositions()
        //{
        //    TreeLayoutSettings settings = new TreeLayoutSettings()
        //    {
        //        TreeLayoutType = TreeLayoutType.TreeRight,
        //        HorizontalSeparation = 20,
        //    };
        //    settings.Roots.Add(diagram.Shapes.First());
        //    diagram.Layout(LayoutType.Tree, settings);
        //    diagram.AutoFit();
        //    //SugiyamaSettings settings = new SugiyamaSettings()
        //    //{
        //    //    //  = TreeLayoutType.TreeRight,
        //    //    //HorizontalSeparation = 20,
        //    //};
        //    ////settings.Roots.Add(diagram.Shapes.First());
        //    //diagram.Layout(LayoutType.Sugiyama, settings);
        //}

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //SetItemsPositions();
        }

        public void ClearItems()
        {
            //    diagram.Items.Clear();
        }
        RadDiagramShape shapeF;
        RadDiagramShape shapeS;
        List<DataLinkItemViewGroups> ViewGroups;

    

        public void ShowDiagram(List<DataLinkItemViewGroups> viewGroups, I_DataViewItem firstSideView, I_DataViewItem secondSideView)
        {
            diagram.Items.Clear();
            ViewGroups = viewGroups;
            foreach (var viewGroup in viewGroups)
            {
                foreach (var view in viewGroup.Views)
                {
                    diagram.Items.Add(view);
                    var shape = (diagram.Items[diagram.Items.Count - 1] as UserControl).Parent as RadDiagramShape;
                    shape.IsConnectorsManipulationEnabled = false;
                    shape.IsRotationEnabled = false;
                    shape.IsResizingEnabled = false;
                }
                foreach (var relation in viewGroup.ViewRelations)
                {
                    var shape2 = (relation.Item1 as UserControl).Parent as RadDiagramShape;
                    var shape1 = (relation.Item2 as UserControl).Parent as RadDiagramShape;
                    diagram.AddConnection(shape1, shape2);
                }

            }

            diagram.Items.Add(firstSideView);
            shapeF = (diagram.Items[diagram.Items.Count - 1] as UserControl).Parent as RadDiagramShape;
            shapeF.IsConnectorsManipulationEnabled = false;
            shapeF.IsRotationEnabled = false;
            shapeF.IsResizingEnabled = false;

            diagram.Items.Add(secondSideView);
            shapeS = (diagram.Items[diagram.Items.Count - 1] as UserControl).Parent as RadDiagramShape;
            shapeS.IsConnectorsManipulationEnabled = false;
            shapeS.IsRotationEnabled = false;
            shapeS.IsResizingEnabled = false;


            foreach (var viewGroup in viewGroups)
            {
                foreach (var view in viewGroup.Views.Where(x => !viewGroup.ViewRelations.Any(y => y.Item2 == x)))
                {
                    var shape = (view as UserControl).Parent as RadDiagramShape;
                    diagram.AddConnection(shapeF, shape);
                }
            }

            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();


            TreeLayoutSettings settings = new TreeLayoutSettings()
            {
                TreeLayoutType = TreeLayoutType.TreeRight,
                HorizontalSeparation = 20,
            };
            settings.Roots.Add(shapeF);
            diagram.Layout(LayoutType.Tree, settings);
            diagram.AutoFit();
            shapeS.Position = new Point(shapeS.Position.X, shapeF.Position.Y);

            foreach (var viewGroup in ViewGroups)
            {
                foreach (var view in viewGroup.Views.Where(x => !viewGroup.ViewRelations.Any(y => y.Item1 == x)))
                {
                    var shape = (view as UserControl).Parent as RadDiagramShape;
                    diagram.AddConnection(shape, shapeS);
                }
            }

        }

        //public void SetDataLinks(List<DataLinkDTO> dataLinks, int dataLinkID)
        //{
        //    cmbDataLinks.SelectedValuePath = "ID";
        //    cmbDataLinks.DisplayMemberPath = "Name";
        //    cmbDataLinks.ItemsSource = dataLinks;
        //    cmbDataLinks.SelectedValue = dataLinkID;
        //}

      

        public void AddDataLinkSelector(MySearchLookup dataLinkSearchLookup)
        {
            grdDataLink.Children.Add(dataLinkSearchLookup as UIElement);
        }

        public void EnabaleDisabeViewSection(bool enable)
        {
            grdDiagram.IsEnabled = enable;
            grdSetting.IsEnabled = enable;
        }
    }
}
