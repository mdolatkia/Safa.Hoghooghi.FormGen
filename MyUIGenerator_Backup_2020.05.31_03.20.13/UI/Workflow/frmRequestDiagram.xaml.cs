using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;

using ProxyLibrary.Workflow;
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
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Primitives;
using Telerik.Windows.Diagrams.Core;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmRequestAction.xaml
    /// </summary>
    public partial class frmRequestDiagram : UserControl, I_View_RequestDiagram
    {
        //public event EventHandler RequestActionUpdated;

        public frmRequestDiagram()
        {
            InitializeComponent();
            SerColorPickers();

        }

        private void SerColorPickers()
        {
            var colorSelector = new RadColorSelector();
            colorSelector.MainPaletteHeaderText = "Metro Palette";
            colorSelector.StandardPaletteVisibility = Visibility.Collapsed;
            colorSelector.NoColorVisibility = Visibility.Collapsed;
            colorSelector.SelectedColorChanged += RadColorSelector_SelectedColorChanged;
            BackgroundColorButton.DropDownContent = colorSelector;


            var colorSelector1 = new RadColorSelector();
            colorSelector1.MainPaletteHeaderText = "Metro Palette";
            colorSelector1.NoColorVisibility = Visibility.Collapsed;
            colorSelector1.StandardPaletteVisibility = Visibility.Collapsed;
            colorSelector1.SelectedColorChanged += GridColorSelectorOnSelectionChanged;
            GridColorButton.DropDownContent = colorSelector1;
        }

        public int RequestID
        {
            set; get;
        }

        public I_View_StateConnection AddConnection(I_View_StateShape shape, I_View_StateShape previousShape, int connectionCount, int index)
        {
            CustomConnector connector = new View.CustomConnector();
            var shape1 = (previousShape as UserControl).Parent as IShape;
            var shape2 = (shape as UserControl).Parent as IShape;
            var radConnector = diagram.AddConnection(shape1, shape2);
            if (connectionCount > 1)
            {
                var shapeWidth = (shape1.Content as UserControl).Width + 8;//خود شیپ یه بوردر اضافه دارد
                var shapeHeight = (shape1.Content as UserControl).Height + 8;//خود شیپ یه بوردر اضافه دارد
                var connectionAreaHeight = 130;
                var perConnectionHeight = connectionAreaHeight / connectionCount;
                //var xDif = shape2.Position.X - shape1.Position.X;
                //var yDif = shape2.Position.Y - shape1.Position.Y;
                var midYShapes = shape1.Position.Y + shapeHeight / 2;
                var connectionY = (midYShapes - connectionAreaHeight / 2) + (index * perConnectionHeight) + (perConnectionHeight / 2);
                radConnector.ConnectionPoints.Add(new Point(shape1.Position.X + shapeWidth + 25, midYShapes));
                radConnector.ConnectionPoints.Add(new Point(shape1.Position.X + shapeWidth + 50, connectionY));
                var connectionWidth = shape2.Position.X - (shape1.Position.X + shapeWidth);
                radConnector.ConnectionPoints.Add(new Point((shape1.Position.X + shapeWidth) + connectionWidth - 50, connectionY));
                radConnector.ConnectionPoints.Add(new Point((shape1.Position.X + shapeWidth) + connectionWidth - 25, midYShapes));

            }

               (radConnector as RadDiagramConnection).Stroke = new SolidColorBrush() { Color = Colors.SteelBlue };
            (radConnector as RadDiagramConnection).StrokeThickness = 3;
            (radConnector as RadDiagramConnection).IsConnectorsManipulationEnabled = false;
            (radConnector as RadDiagramConnection).IsDraggingEnabled = false;
            (radConnector as RadDiagramConnection).IsManipulationEnabled = false;
            radConnector.TargetCapSize = new Size(15, 15);
            radConnector.TargetCapType = CapType.Arrow1Filled;
            connector.Connector = radConnector;
            return connector;
        }



        //public void ShowDiagram(RequestDiagramDTO requestDiagram)
        //{
        //    int index = 0;
        //    if (requestDiagram.FirstDiagramState != null)
        //        AddStateShape(requestDiagram.FirstDiagramState, index);

        //    foreach (var item in requestDiagram.DiagramStates)
        //    {
        //        index++;
        //        AddStateShape(item, index);
        //    }
        //}

        public I_View_StateShape AddStateShape(int index)
        {
            //StateShape aa;

            StateShape1 shape = new StateShape1();
            int x = 100 + 500 * index;
            diagram.Items.Add(shape);
            var aaa = diagram.Items[diagram.Items.Count - 1];
            ((aaa as UserControl).Parent as RadDiagramShape).Position = new Point(x, 100);
            //shape.SetLocation(x, 100);
            ((aaa as UserControl).Parent as RadDiagramShape).IsDraggingEnabled = false;
            ((aaa as UserControl).Parent as RadDiagramShape).IsConnectorsManipulationEnabled = false;
            ((aaa as UserControl).Parent as RadDiagramShape).IsRotationEnabled = false;
            ((aaa as UserControl).Parent as RadDiagramShape).IsResizingEnabled = false;
            return shape;
        }



        private void ZoomSpinner_OnValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (diagram != null && ZoomSpinner != null && ZoomSpinner.Value.HasValue)
                diagram.Zoom = ZoomSpinner.Value.Value / 100d;
        }

        private void RadColorSelector_SelectedColorChanged(object sender, EventArgs e)
        {
            RadColorSelector selector = sender as RadColorSelector;
            if (selector.SelectedColor != null)
            {
                var color = selector.SelectedColor;
                diagram.Background = new SolidColorBrush(color);
            }
        }

        private void GridColorSelectorOnSelectionChanged(object sender, EventArgs e)
        {
            RadColorSelector selector = sender as RadColorSelector;
            if (selector.SelectedColor != null)
            {
                var color = selector.SelectedColor;
                BackgroundGrid.SetLineStroke(diagram, new SolidColorBrush(color));
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
    }


}
