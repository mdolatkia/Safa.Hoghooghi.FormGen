
using MyModelManager;

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;

namespace MyProject_WPF.Diagram
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyProject_WPF.Diagram"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyProject_WPF.Diagram;assembly=MyProject_WPF.Diagram"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:StateShape/>
    ///
    /// </summary>
    public class StateShape : RadDiagramShapeBase
    {
        public event EventHandler<StateShapeEditArg> StateShapeEdit;
        public bool IsInDiagram = false;
        BizState bizState = new BizState();
        WFStateDTO State { set; get; }
        public StateShape()
        {

            //this.Content=new TextBlock(){Text=}
            this.Loaded += StateShape_Loaded;

        }

        public void SetStateInfo()
        {
            if (IsInDiagram)
            {

                var tooltip = "";
                if (StateID > 0)
                    State = bizState.GetState(StateID, true);
                if (State != null)
                {
                    Title = State.Name;
                    var activityCount = 0;
                    string activities = "";
                    foreach (var activity in State.Activities)
                    {
                        activities += (activities == "" ? "" : Environment.NewLine) + activity.Name;
                        activityCount++;
                    }
                    tooltip = (State.Description == null ? "" : State.Description);
                    if (activityCount != 0)
                        tooltip += Environment.NewLine + "شامل " + activityCount + " فعالیت به عناوین" + activities;
                    if (tooltip != "")
                        this.ToolTip = tooltip;
                }

            }

        }

        void StateShape_Loaded(object sender, RoutedEventArgs e)
        {
            var buttonList = this.ChildrenOfType<Button>();
            foreach (var button in buttonList)
            {
                if (!IsInDiagram)
                    button.Visibility = System.Windows.Visibility.Collapsed;
                if (StateID <= 0)
                    button.Visibility = System.Windows.Visibility.Collapsed;
                button.Click += button_Click;
            }
            if (IsInDiagram)
                SetStateInfo();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name.ToLower().Contains("state"))
            {
                if (StateShapeEdit != null)
                {
                    StateShapeEdit(this, new StateShapeEditArg() { StateID = StateID });
                }

            }
        }








        public int StateID { set; get; }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty
    = DependencyProperty.Register(
          "Title",
          typeof(string),
          typeof(StateShape),
          null
      );

        static StateShape()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StateShape), new FrameworkPropertyMetadata(typeof(StateShape)));
        }
    }
    public class StateShapeEditArg : EventArgs
    {
        public int StateID { set; get; }
    }
}
