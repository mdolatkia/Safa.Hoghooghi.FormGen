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
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Controls.Diagrams.Primitives;
using Telerik.Windows.Diagrams.Core;

using MyProject_WPF.Diagram;
using System.Globalization;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows;
using ProxyLibrary.Workflow;
using MyModelManager;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmTransition.xaml
    /// </summary>
    public partial class frmTransitions : UserControl
    {
        BizTransition bizTransition = new BizTransition();
        BizProcess bizProcess = new BizProcess();
        BizState bizState = new BizState();
        int ProcessID { set; get; }
        public List<WFStateDTO> States { get; set; }
        public List<Tuple<TransitionDTO, IConnection>> TransitionConnections = new List<Tuple<TransitionDTO, IConnection>>();
        public frmTransitions(int processID)
        {
            InitializeComponent();
            SetRadButtons();
            ProcessID = processID;
            SerializationService.Default.ItemSerializing += Default_ItemSerializing;
            diagram.ShapeDeserialized += Diagram_ShapeDeserialized;
            diagram.ShapeSerialized += diagram_ShapeSerialized;
            diagram.ConnectionManipulationCompleted += Diagram_ConnectionManipulationCompleted;
            diagram.ItemsChanged += diagram_ItemsChanged;


            PrepareToolbox();
            LoadTransitions();

            //     EventManager.RegisterClassHandler(typeof(RadDiagramConnector), RadDiagramConnector.ActivationChangedEvent, new RadRoutedEventHandler(OnConnectorActivationChanged));
        }

        //private void Diagram_CommandExecuted(object sender, CommandRoutedEventArgs e)
        //{
        //    DiagramCommands.Delete
        //    if (e.Command.Name.ToLower().Contains("delete"))
        //    {
        //        if (MessageBox.Show("آیا از حذف مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //        {
        //            //var item = e.NewItems.FirstOrDefault();
        //            //if (item is StateShape)
        //            //{
        //            //    var stateShape = (item as StateShape);
        //            //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        //            //    {
        //            //        if (MessageBox.Show("آیا از حذف حالت مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //            //        {

        //            RemoveRedundantConnections();

        //            //        }
        //            //    }
        //            //}
        //        }
        //        else
        //            e.Handled = true;
        //    }
        //}

        private void SetRadButtons()
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

        private void LoadTransitions()
        {//?با جزئیات؟
            var transitions = bizTransition.GetTransitions(MyProjectManager.GetMyProjectManager.GetRequester(), ProcessID, true);
            string flowSTR = bizProcess.GetFlowSTR(ProcessID);
            diagram.Load(flowSTR);
            foreach (var transition in transitions)
            {
                var conenction = GetConnection(transition);
                if (conenction != null)
                {
                    conenction.Content = GetConnectionContent(transition);
                    TransitionConnections.Add(new Tuple<TransitionDTO, IConnection>(transition, conenction));
                }
            }

        }

        private IConnection GetConnection(TransitionDTO item)
        {
            //StateShape shape1 = GetStateShape(item.CurrentStateID);
            //StateShape shape2 = GetStateShape(item.CurrentStateID);
            foreach (var connection in diagram.Connections)
            {
                if (connection.Source is StateShape)
                    if (connection.Target is StateShape)
                    {
                        var source = (connection.Source as StateShape);
                        var target = (connection.Target as StateShape);
                        if (source.StateID == item.CurrentStateID && target.StateID == item.NextStateID)
                            return connection;
                    }
            }
            return null;
        }
        private UC_ConnectionContent GetConnectionContent(TransitionDTO transition)
        {
            var content = new UC_ConnectionContent(transition);

            //content.TransitionEditActions += content_TransitionEditActions;
            //content.TransitionEditActivities += Content_TransitionEditActivities;
            content.TransitionInfo += Content_TransitionInfo;
            //content.TransitionEditForms += Content_TransitionEditForms;
            return content;
        }

        private void Content_TransitionInfo(object sender, TransitionEditArg e)
        {
            frmTransitionInfo view = new frmTransitionInfo(e.Transition);
            view.InfoConfirmed += View_InfoConfirmed;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Transition", Enum_WindowSize.Big);
        }

        private void View_InfoConfirmed(object sender, TransitoinInfoArg e)
        {
            var transitionConnection = TransitionConnections.First(x => x.Item1 == e.Transition);
            var content = transitionConnection.Item2.Content as UC_ConnectionContent;
            content.SetInfo(transitionConnection.Item1);
        }



        //private StateShape GetStateShape(int stateID)
        //{
        //    return GetDiagramStateShapes().First(x => x.StateID == stateID);
        //}

        void diagram_ItemsChanged(object sender, DiagramItemsChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var item = e.NewItems.FirstOrDefault();
                if (item is StateShape)
                {
                    var stateShape = (item as StateShape);


                    RemoveFromGallery(stateShape);
                    if (stateShape.StateID > 0)
                        stateShape.StateShapeEdit += StateShape_StateShapeEdit;
                }

            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var fromtransitions = TransitionConnections.Where(x => x.Item2.Source == null);
                var totransitions = TransitionConnections.Where(x => x.Item2.Target == null);
                foreach (var item in fromtransitions.ToList())
                {
                    TransitionConnections.Remove(item);
                    diagram.RemoveConnection(item.Item2);
                }
                foreach (var item in totransitions.ToList())
                {
                    TransitionConnections.Remove(item);
                    diagram.RemoveConnection(item.Item2);
                }
                foreach (var item in e.OldItems)
                {
                    if (item is IConnection)
                    {
                        var removedTransitionConnection = TransitionConnections.FirstOrDefault(x => x.Item2 == (item as IConnection));
                        if (removedTransitionConnection != null)
                        {
                            TransitionConnections.Remove(removedTransitionConnection);
                            // diagram.RemoveConnection(item.Item2);
                        }
                    }
                }
            }
        }
        //private void Diagram_ItemsChanging(object sender, DiagramItemsChangingEventArgs e)
        //{

        //}
        private void RemoveRedundantConnections()
        {
            //diagram.Items.Remove(stateShape);

        }

        private void StateShape_StateShapeEdit(object sender, StateShapeEditArg e)
        {
            frmAddSelectState view = new frmAddSelectState(ProcessID, e.StateID);
            view.ItemSaved += (sender1, e1) => View_ItemSaved(sender1, e1, (sender as StateShape));
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }

        private void View_ItemSaved(object sender, SavedItemArg e, StateShape stateShape)
        {
            stateShape.SetStateInfo();
        }

        //List<WFActionDTO> lastSelectedActions = new List<WFActionDTO>();
        private void Diagram_ConnectionManipulationCompleted(object sender, Telerik.Windows.Controls.Diagrams.ManipulationRoutedEventArgs e)
        {
            //ساخت کانکشن
            if (e.ManipulationStatus == ManipulationStatus.Attaching)
            {
                if (e.Connection.Source == null || e.Connection.Source.GetType() != typeof(StateShape))
                {
                    e.Handled = true;
                    return;
                }
                if (e.Connector == null || e.Connector.Shape == null || e.Connector.Shape.GetType() != typeof(StateShape))
                {
                    e.Handled = true;
                    return;
                }
                if (e.Connection.Source == e.Connector.Shape)
                {
                    e.Handled = true;
                    return;
                }

                //کنترل تکراری بودن
                foreach (var item in diagram.Items)
                {
                    if (item is IConnection)
                    {
                        if ((item as IConnection).Source == e.Connection.Source
                            && (item as IConnection).Target == e.Connector.Shape)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }

                e.Handled = true;

                ConnectionManipulated(e.Connection, e.Connection.Source, e.Connector.Shape);
            }

        }



        private void ConnectionManipulated(IConnection tmpconnection, IShape shape1, IShape shape2)
        {
            var transitionConnection = TransitionConnections.FirstOrDefault(x => x.Item2 == tmpconnection);
            //var shape1 = tmpconnection.Source;
            //var shape2 = tmpconnection.Target;
            IConnection newConnection = null;
            TransitionDTO transition = null;
            if (transitionConnection != null)
            {
                transition = transitionConnection.Item1;
                diagram.RemoveConnection(tmpconnection);
                TransitionConnections.Remove(transitionConnection);

            }
            else
            {
                transition = new TransitionDTO();
                transition.ProcessID = ProcessID;
            }
            newConnection = CreateConnection(shape1, shape2);
            newConnection.Content = GetConnectionContent(transition);
            newConnection.TargetCapType = CapType.Arrow1Filled;
            transitionConnection = new Tuple<TransitionDTO, IConnection>(transition, newConnection);
            TransitionConnections.Add(transitionConnection);

            transitionConnection.Item1.CurrentStateID = (shape1 as StateShape).StateID;
            transitionConnection.Item1.CurrentState = States.First(x => x.ID == (shape1 as StateShape).StateID);
            transitionConnection.Item1.NextStateID = (shape2 as StateShape).StateID;
            transitionConnection.Item1.NextState = States.First(x => x.ID == (shape2 as StateShape).StateID);

            //////if (!transition.TransitionActions.Any())
            //////{
            //////    EditTransitionActions(transitionConnection);
            //////}

        }




        private IConnection CreateConnection(IShape shape1, IShape shape2)
        {
            //RadDiagramConnection connection = new RadDiagramConnection();
            //connection.Source = shape1;
            //connection.Target = shape2;
            var connection = diagram.AddConnection(shape1, shape2);



            if (diagram.Connections.Any(x => x.ConnectionType == ConnectionType.Polyline && x.Source == shape2 && x.Target == shape1))
            {
                var xDif = shape2.Position.X - shape1.Position.X;
                var yDif = shape2.Position.Y - shape1.Position.Y;
                connection.ConnectionType = ConnectionType.Spline;
                yDif += 90;
                //(connection as RadDiagramConnection).VerticalContentAlignment = VerticalAlignment.Bottom;
                connection.ConnectionPoints.Add(new Point(shape1.Position.X + (xDif / 2) + (shape1.Bounds.Width / 2), shape1.Position.Y + (yDif / 2) + (shape1.Bounds.Height / 2)));
                //(connection as RadDiagramConnection).mo
            }



            return connection;
        }



        private void Diagram_ShapeDeserialized(object sender, Telerik.Windows.Controls.Diagrams.ShapeSerializationRoutedEventArgs e)
        {
            if (e.Shape as StateShape != null)
            {
                (e.Shape as StateShape).StateID = Convert.ToInt32(e.SerializationInfo["StateID"]);
                (e.Shape as StateShape).Title = Convert.ToString(e.SerializationInfo["Title"]);
                (e.Shape as StateShape).IsInDiagram = true;
            }
        }

        //void Default_ItemDeserializing(object sender, SerializationEventArgs<IDiagramItem> e)
        //{
        //    if (e.Entity is StateShape)
        //    {
        //        e.SerializationInfo["StateID"] = (e.Entity as StateShape).StateID;
        //        e.SerializationInfo["Title"] = (e.Entity as StateShape).Title;
        //    }
        //}
        void diagram_ShapeSerialized(object sender, ShapeSerializationRoutedEventArgs e)
        {
            if (e.Shape is StateShape)
            {
                e.SerializationInfo["StateID"] = (e.Shape as StateShape).StateID;
                e.SerializationInfo["Title"] = (e.Shape as StateShape).Title;
            }
        }

        private void Default_ItemSerializing(object sender, SerializationEventArgs<IDiagramItem> e)
        {
            if (e.Entity is StateShape)
            {
                e.SerializationInfo["StateID"] = (e.Entity as StateShape).StateID;
                e.SerializationInfo["Title"] = (e.Entity as StateShape).Title;
            }
        }



        HierarchicalGalleryItemsCollection GalleryTree = new HierarchicalGalleryItemsCollection();

        private void PrepareToolbox()
        {
            toolBox.Header = GetToolBoxHeader();
            if (GalleryTree.Count == 4)
                GalleryTree.Clear();

            SetStatesGallery();
            this.toolBox.ItemsSource = GalleryTree;
        }

        private void SetStatesGallery()
        {
            States = bizState.GetStates(ProcessID, false);
            // AddStartAndEndStates();
            var stateGallery = GalleryTree.FirstOrDefault(x => x.Header == "حالات");
            if (stateGallery == null)
            {
                stateGallery = new Gallery() { Header = "حالات" };
                GalleryTree.Add(stateGallery);
            }
            stateGallery.Items.Clear();
            foreach (var state in States)
            {
                //برای زمان رفرش شدن
                if (!GetDiagramStateShapes().Any(x => x.StateID == state.ID))
                {
                    var galleryItem = new GalleryItem();

                    galleryItem.Header = state.Name;
                    galleryItem.ItemType = "State";
                    var shape = new StateShape();
                    // shape.Name = state.Name.Replace(" ", "") + state.ID.ToString();
                    if (state.StateType == StateType.Start || state.StateType == StateType.End)
                    {
                        shape.Background = new SolidColorBrush(Colors.Red);
                    }
                    shape.StateID = state.ID;

                    shape.Title = state.Name;

                    galleryItem.Shape = shape;
                    stateGallery.Items.Add(galleryItem);
                }
            }


        }

        //private void AddStartAndEndStates()
        //{
        //    States.Add(new StateDTO() { ID = -1, Name = "آغاز", Description = "وضعیت آغازین" });
        //    States.Add(new StateDTO() { ID = -99, Name = "پایان", Description = "وضعیت پایانی" });
        //}

        private void RemoveFromGallery(StateShape stateShape)
        {
            //برای زمان درگ شدن
            var stateGallery = GalleryTree.FirstOrDefault(x => x.Header == "حالات");
            if (stateGallery != null)
            {
                List<GalleryItem> removeList = new List<GalleryItem>();

                foreach (var galleryItem in stateGallery.Items)
                {
                    if ((galleryItem.Shape as StateShape).StateID == stateShape.StateID)
                    {
                        removeList.Add(galleryItem);
                    }
                }
                foreach (var galleryItem in removeList)
                    stateGallery.Items.Remove(galleryItem);
            }
        }


        private List<StateShape> GetDiagramStateShapes()
        {
            List<StateShape> result = new List<StateShape>();
            foreach (var item in diagram.Shapes)
            {
                if (item is StateShape)
                    result.Add(item as StateShape);
            }
            return result;
        }
        private object GetToolBoxHeader()
        {
            Grid grid = new Grid();

            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            System.Windows.Controls.TextBlock title = new System.Windows.Controls.TextBlock() { Text = "مجموعه ابزار" };
            grid.Children.Add(title);
            Grid.SetColumn(title, 1);

            StackPanel panel = new StackPanel();
            panel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            grid.Children.Add(panel);

            Button btnRefreshStates = new Button();
            btnRefreshStates.Click += btnRefreshStates_Click;
            Image img = new Image();
            Uri uriSource = new Uri("../Images/refresh.png", UriKind.Relative);
            img.Source = new BitmapImage(uriSource);
            btnRefreshStates.Content = img;
            btnRefreshStates.ToolTip = "بازآوری حالات";
            panel.Children.Add(btnRefreshStates);

            Button btnNewState = new Button();
            btnNewState.Click += btnNewState_Click;
            Image img1 = new Image();
            Uri uriSource1 = new Uri("../Images/addnew.png", UriKind.Relative);
            img1.Source = new BitmapImage(uriSource1);
            btnNewState.Content = img1;
            btnRefreshStates.ToolTip = "تعریف حالت جدید";
            panel.Children.Add(btnNewState);

            return grid;
        }

        void btnNewState_Click(object sender, RoutedEventArgs e)
        {
            frmAddSelectState view = new frmAddSelectState(ProcessID, 0);
            view.ItemSaved += view_ItemSaved;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }

        void view_ItemSaved(object sender, SavedItemArg e)
        {
            SetStatesGallery();
        }

        void btnRefreshStates_Click(object sender, RoutedEventArgs e)
        {
            SetStatesGallery();
        }

        private void OnToolChecked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadRibbonRadioButton;
            if (button != null && this.diagram != null)
            {
                if (button.Name == "ConnectionButton")
                {
                    this.diagram.ActiveTool = MouseTool.ConnectorTool;
                }
                else if (button.Name == "TextButton")
                {
                    this.diagram.ActiveTool = MouseTool.TextTool;
                }
                else if (button.Name == "PathButton")
                {
                    this.diagram.ActiveTool = MouseTool.PathTool;
                }
                else if (button.Name == "PencilButton")
                {
                    this.diagram.ActiveTool = MouseTool.PencilTool;
                }
                else
                {
                    this.diagram.ActiveTool = MouseTool.PointerTool;
                }
            }
        }
        private void CellWidthSpinner_OnValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (diagram != null && CellWidthSpinner != null && CellWidthSpinner.Value.HasValue)
                BackgroundGrid.SetCellSize(diagram, new Size(CellWidthSpinner.Value.Value, CellHeightSpinner.Value.Value));
        }

        private void CellHeightSpinner_OnValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (diagram != null && CellHeightSpinner != null && CellHeightSpinner.Value.HasValue)
                BackgroundGrid.SetCellSize(diagram, new Size(CellWidthSpinner.Value.Value, CellHeightSpinner.Value.Value));
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!TransitionConnections.Any(x => x.Item1.CurrentState.StateType == StateType.Start))
            {
                MessageBox.Show("وضعیت آغاز مشخص نشده است");
                return;
            }
            if (!TransitionConnections.Any(x => x.Item1.NextState.StateType == StateType.End))
            {
                MessageBox.Show("وضعیت پایان مشخص نشده است");
                return;
            }
            if (TransitionConnections.Any(x => string.IsNullOrEmpty(x.Item1.Name)))
            {
                MessageBox.Show("برای برخی از انتقالها نام مشخص نشده است");
                return;
            }
            var flowSTR = diagram.Save();
            //try
            //{
            var result = bizTransition.UpdateTransitions(ProcessID, TransitionConnections.Select(X => X.Item1).ToList(), flowSTR);
            if (result)
            {
                MessageBox.Show("با موفقیت ثبت شد");
            }
            else
            {
                MessageBox.Show("عملیات ثبت انجام نشد");
            }
            //}
            //catch (Exception ex)
            //{
            //احتمالن بعلت استفاده شدن از جریان کار و ریکوئست اکشن داشتن
            //  MessageBox.Show("خطا در ذخیره سازی جریان کار");
            //}
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }


    }


}
